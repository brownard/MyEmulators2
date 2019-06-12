extern alias nsoft;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using System.Threading;
using nsoft::Newtonsoft.Json;
using nsoft::Newtonsoft.Json.Linq;
using System.Linq;

namespace Cornerstone.ScraperEngine.Nodes {
    [ScraperNode("parse")]
    public class ParseNode : ScraperNode {
        #region Properties

        public string Input {
            get { return input; }
        } protected String input;

        public string Pattern {
            get { return pattern; }
        } protected String pattern;

        public string Json {
            get { return json; }
        }
        protected String json;

        public string Xpath {
            get { return xpath; }
        } protected String xpath;

        #endregion

        #region Methods

        public ParseNode(XmlNode xmlNode, InternalScriptSettings settings)
            : base(xmlNode, settings) {

            // Load attributes
            foreach (XmlAttribute attr in xmlNode.Attributes) {
                switch (attr.Name) {
                    case "input":
                        input = attr.Value;
                        break;
                    case "regex":
                        pattern = attr.Value;
                        break;
                    case "json":
                        json = attr.Value;
                        break;
                    case "xpath":
                        xpath = attr.Value;
                        break;
                }
            }

            // Validate INPUT attribute
            if (input == null) {
                logger.Error("Missing INPUT attribute on: " + xmlNode.OuterXml);
                loadSuccess = false;
                return;
            }

            // Validate REGEX/XPATH attribute
            if (pattern == null && json == null && xpath == null) {
                logger.Error("Missing REGEX or XPATH attribute on: " + xmlNode.OuterXml);
                loadSuccess = false;
                return;
            }

        }

        public override void Execute(Dictionary<string, string> variables) {
            if (ScriptSettings.DebugMode) logger.Debug("executing parse: " + xmlNode.OuterXml);
            // parse variables from the input string
            string parsedInput = parseString(variables, input);
            string parsedName = parseString(variables, Name);

            // do requested parsing
            if (pattern != null)
                processPattern(variables, parsedInput, parsedName);
            else if (json != null)
                processJson(variables, parsedInput, parsedName);
            else
                processXpath(variables, parsedInput, parsedName);
        }

        // Parse input using a regular expression
        private void processPattern(Dictionary<string, string> variables, string parsedInput, string parsedName) {
            string parsedPattern = parseString(variables, pattern);

            if (ScriptSettings.DebugMode) logger.Debug("name: " + parsedName + " ||| pattern: " + parsedPattern + " ||| input: " + parsedInput);

            // try to find matches via regex pattern
            MatchCollection matches;
            try {
                Regex regEx = new Regex(parsedPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                matches = regEx.Matches(parsedInput);
            }
            catch (Exception e) {
                if (e.GetType() == typeof(ThreadAbortException))
                    throw e;

                logger.Error("Regex expression failed!", e);
                return;
            }

            setVariable(variables, parsedName + ".count", matches.Count.ToString());

            if (matches.Count == 0) {
                if (ScriptSettings.DebugMode) logger.Debug("Parse node returned no results... " + xmlNode.OuterXml);
                return;
            }

            setVariable(variables, parsedName, matches[0].Value);

            // write matches and groups to variables
            int matchNum = 0;
            foreach (Match currMatch in matches) {
                // store the match itself
                string matchName = parsedName + "[" + matchNum + "]";
                setVariable(variables, matchName, currMatch.Value);

                // store the groups in the match
                for (int i = 1; i < currMatch.Groups.Count; i++)
                    setVariable(variables, matchName + "[" + (i - 1) + "]", currMatch.Groups[i].Value);

                matchNum++;
            }
        }

        // Parse input using an json token
        private void processJson(Dictionary<string, string> variables, string parsedInput, string parsedName)
        {
            string query = parseString(variables, json);

            try
            {
                JObject json = JObject.Parse(parsedInput);
                List<JToken> tokens = json.SelectTokens(query, false).ToList();

                setVariable(variables, parsedName + ".count", tokens.Count.ToString());

                int current = 0;
                foreach (JToken token in tokens)
                {
                    string varName = parsedName + "[" + current.ToString() + "]";
                    parseToken(variables, varName, token, true);
                    current++;
                }
            }
            catch (Exception e)
            {
                if (e is ThreadAbortException)
                    throw e;

                logger.Error("Scraper Script JSON parsing failed: {0}", e.Message);
            }
        }

        private void parseToken(Dictionary<string, string> variables, string name, JToken token, bool recursive)
        {
            JObject jObject = token as JObject;
            if (jObject != null)
            {
                if (jObject.Count > 0)
                {
                    setVariable(variables, name, jObject.ToString());
                    if (recursive)
                    {
                        foreach (var child in jObject)
                        {
                            string varName = name + "." + child.Key;
                            parseToken(variables, varName, child.Value, false);
                        }
                    }
                }
            }
            else
            {
                setVariable(variables, name, token.ToString());
            }
        }

        // Parse input using an xpath query
        private void processXpath(Dictionary<string, string> variables, string parsedInput, string parsedName) {
            string query = parseString(variables, xpath);

            try {
                XPathDocument xml = new XPathDocument(new StringReader(parsedInput));
                XPathNavigator navigator = xml.CreateNavigator();
                XPathNodeIterator nodes = navigator.Select(query);

                setVariable(variables, parsedName + ".count", nodes.Count.ToString());

                while (nodes.MoveNext()) {
                    XPathNavigator node = nodes.Current;
                    string varName = parsedName + "[" + (nodes.CurrentPosition - 1).ToString() + "]";
                    parseNode(variables, varName, node, true);
                }
            }
            catch (Exception e) {
                if (e.GetType() == typeof(ThreadAbortException))
                    throw e;

                logger.Error("Scraper Script XPATH parsing failed: {0}", e.Message);
            }
        }

        private void parseNode(Dictionary<string, string> variables, string name, XPathNavigator node, bool recursive) {
            XPathNodeIterator childNodes = node.SelectChildren(XPathNodeType.Element);
            if (childNodes.Count > 0) {
                // Create nodeset variable
                setVariable(variables, name, node.OuterXml);

                // Parse Children If Required
                // todo: if multiple children with the same name exist only the last
                // one will have a variable. It should be clear that in this case
                // the scripter should parse the OuterXml value 
                if (recursive) {
                    while (childNodes.MoveNext()) {
                        XPathNavigator child = childNodes.Current;
                        string varName = name + "." + child.Name;
                        parseNode(variables, varName, child, false);
                    }
                }
            }
            else {
                // Create node variable
                setVariable(variables, name, node.Value);
            }

            // create attribute variables
            if (node.HasAttributes && recursive) {
                XPathNavigator attrib = node.Clone();
                attrib.MoveToFirstAttribute();
                setVariable(variables, name + ".@" + attrib.Name, attrib.Value);
                while (attrib.MoveToNextAttribute()) {
                    setVariable(variables, name + ".@" + attrib.Name, attrib.Value);
                }
            }
        }

        #endregion
    }
}
