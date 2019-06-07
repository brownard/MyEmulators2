using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace myEmulators
{
    class DBSync
    {
        //Handles importing and exporting of game meta data

        public static bool import(Game[] items, String inputPath, bool overwrite)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(inputPath);
                XmlNodeList importedNodes = doc.GetElementsByTagName("game");
                foreach (Game item in items)
                {
                    XmlNode result = null;
                    try
                    {
                        result = doc.SelectSingleNode("//title[text()='" + item.Title + "']").ParentNode;
                    }
                    catch (Exception)
                    {
                        //Did not find a matching node
                    }
                    if (result != null)
                    {
                        bool updatedData = false;
                        foreach (XmlNode data in result.ChildNodes)
                        {
                            if (data.LocalName.Equals("grade"))
                            {
                                if (overwrite || item.Grade == 0)
                                {
                                    try
                                    {
                                        item.Grade = Int32.Parse(data.InnerText);
                                        updatedData = true;
                                    }
                                    catch (Exception)
                                    {
                                        //The xml file has been tampered with!
                                    }
                                }
                            }
                            else if (data.LocalName.Equals("yearmade"))
                            {
                                if (overwrite || item.Yearmade == 0)
                                {
                                    try
                                    {
                                        item.Yearmade = Int32.Parse(data.InnerText);
                                        updatedData = true;
                                    }
                                    catch (Exception) { }
                                }
                            }
                            else if (data.LocalName.Equals("description"))
                            {
                                if (overwrite || item.Description.Length == 0)
                                {
                                    item.Description = data.InnerText;
                                    updatedData = true;
                                }
                            }
                            else if (data.LocalName.Equals("genre"))
                            {
                                if (overwrite || item.Genre.Length == 0)
                                {
                                    item.Genre = data.InnerText;
                                    updatedData = true;
                                }
                            }
                            else if (data.LocalName.Equals("company"))
                            {
                                if (overwrite || item.Company.Length == 0)
                                {
                                    item.Company = data.InnerText;
                                    updatedData = true;
                                }
                            }
                        }
                        if (updatedData)
                        {
                            item.Save();
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool export(Game[] items, String outputPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode headNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(headNode);

                XmlNode topnode = doc.CreateElement("games");
                doc.AppendChild(topnode);

                foreach (Game item in items)
                {
                    bool hasData = false;
                    XmlNode node = doc.CreateElement("game");

                    XmlNode title = doc.CreateElement("title");
                    title.InnerText = item.Title;
                    node.AppendChild(title);

                    if (item.Grade > 0)
                    {
                        XmlNode data = doc.CreateElement("grade");
                        data.InnerText = "" + item.Grade;
                        node.AppendChild(data);
                        hasData = true;
                    }
                    if (item.Yearmade > 0)
                    {
                        XmlNode data = doc.CreateElement("yearmade");
                        data.InnerText = "" + item.Yearmade;
                        node.AppendChild(data);
                        hasData = true;
                    }
                    if (item.Description.Length > 0)
                    {
                        XmlNode data = doc.CreateElement("description");
                        data.InnerText = item.Description;
                        node.AppendChild(data);
                        hasData = true;
                    }
                    if (item.Genre.Length > 0)
                    {
                        XmlNode data = doc.CreateElement("genre");
                        data.InnerText = item.Genre;
                        node.AppendChild(data);
                        hasData = true;
                    }
                    if (item.Company.Length > 0)
                    {
                        XmlNode data = doc.CreateElement("company");
                        data.InnerText = item.Company;
                        node.AppendChild(data);
                        hasData = true;
                    }

                    //Only add to document if some data exists
                    if (hasData)
                    {
                        topnode.AppendChild(node);
                    }
                }

                doc.Save(outputPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool restore(String inputPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(inputPath);

                //Emulators
                DB.Instance.Execute("DELETE FROM {0}", Emulator.TABLE_NAME);

                XmlNodeList exportedEmulatorNodes = doc.SelectNodes("backup/emulators/emulator");

                foreach (XmlNode emulatorNode in exportedEmulatorNodes)
                {
                    Emulator restoreEmulator = new Emulator();

                    //Emulator path
                    restoreEmulator.PathToEmulator = emulatorNode.SelectSingleNode("emulator_path").InnerText;
                
                    //Position
                    restoreEmulator.Position = int.Parse(emulatorNode.SelectSingleNode("position").InnerText);
                    
                    //ROM path
                    restoreEmulator.PathToRoms = emulatorNode.SelectSingleNode("rom_path").InnerText;

                    //Title
                    restoreEmulator.Title = emulatorNode.SelectSingleNode("title").InnerText;

                    //Filter
                    restoreEmulator.Filter = emulatorNode.SelectSingleNode("filter").InnerText;
                    
                    //Working path
                    restoreEmulator.WorkingFolder = emulatorNode.SelectSingleNode("working_path").InnerText;

                    //Use quotes
                    restoreEmulator.UseQuotes = Boolean.Parse(emulatorNode.SelectSingleNode("use_quotes").InnerText);

                    //View
                    restoreEmulator.View = int.Parse(emulatorNode.SelectSingleNode("view").InnerText);

                    //Arguments
                    restoreEmulator.Arguments = emulatorNode.SelectSingleNode("args").InnerText;

                    //Suspend MP
                    restoreEmulator.SuspendRendering = Boolean.Parse(emulatorNode.SelectSingleNode("suspend_mp").InnerText);

                    restoreEmulator.Save();
                }

                //Games
                DB.Instance.Execute("DELETE FROM {0}", Game.TABLE_NAME);

                XmlNodeList exportedGameNodes = doc.SelectNodes("backup/games/game");

                foreach (XmlNode gameNode in exportedGameNodes)
                {
                    //Parent Emulator
                    Emulator parentEmu = DB.Instance.GetEmulator(int.Parse(gameNode.SelectSingleNode("parent_emulator").InnerText));

                    Game restoreGame = new Game(gameNode.SelectSingleNode("path").InnerText, parentEmu);

                    //Title
                    restoreGame.Title = gameNode.SelectSingleNode("title").InnerText;

                    //Grade
                    restoreGame.Grade = int.Parse(gameNode.SelectSingleNode("grade").InnerText);

                    //Play count
                    restoreGame.Playcount = int.Parse(gameNode.SelectSingleNode("play_count").InnerText);

                    //Year made
                    restoreGame.Yearmade = int.Parse(gameNode.SelectSingleNode("year_made").InnerText);

                    //Latest play
                    restoreGame.Latestplay = DateTime.Parse(gameNode.SelectSingleNode("latest_play").InnerText);

                    //Description
                    restoreGame.Description = gameNode.SelectSingleNode("description").InnerText;

                    //Genre
                    restoreGame.Genre = gameNode.SelectSingleNode("genre").InnerText;

                    //Company
                    restoreGame.Company = gameNode.SelectSingleNode("company").InnerText;

                    //Visible
                    restoreGame.Visible = bool.Parse(gameNode.SelectSingleNode("visible").InnerText);

                    //Favourite
                    restoreGame.Favourite = bool.Parse(gameNode.SelectSingleNode("favourite").InnerText);

                    //Save the game to the database
                    DB.Instance.AddGame(gameNode.SelectSingleNode("path").InnerText, parentEmu);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool backup(String outputPath)
        {
            try
            {
                List<Game> games = new List<Game>();

                //Create and begin the XML document
                XmlDocument doc = new XmlDocument();
                XmlNode headNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(headNode);

                XmlNode backupRootNode = doc.CreateElement("backup");
                doc.AppendChild(backupRootNode);

                //Add emulators
                XmlNode topEmulatorsNode = doc.CreateElement("emulators");
                backupRootNode.AppendChild(topEmulatorsNode);

                foreach (Emulator emu in DB.Instance.GetEmulators())
                {
                    XmlNode emulatorNode = doc.CreateElement("emulator");

                    //Emulator path
                    XmlNode emulatorPathNode = doc.CreateElement("emulator_path");
                    emulatorPathNode.InnerText = emu.PathToEmulator;
                    emulatorNode.AppendChild(emulatorPathNode);

                    //Position
                    XmlNode emulatorPositionNode = doc.CreateElement("position");
                    emulatorPositionNode.InnerText = emu.Position.ToString();
                    emulatorNode.AppendChild(emulatorPositionNode);

                    //ROM path
                    XmlNode emulatorROMPathNode = doc.CreateElement("rom_path");
                    emulatorROMPathNode.InnerText = emu.PathToRoms;
                    emulatorNode.AppendChild(emulatorROMPathNode);

                    //Title
                    XmlNode emulatorTitleNode = doc.CreateElement("title");
                    emulatorTitleNode.InnerText = emu.Title;
                    emulatorNode.AppendChild(emulatorTitleNode);

                    //Filter
                    XmlNode emulatorFilterNode = doc.CreateElement("filter");
                    emulatorFilterNode.InnerText = emu.Filter;
                    emulatorNode.AppendChild(emulatorFilterNode);

                    //Working path
                    XmlNode emulatorWorkingPathNode = doc.CreateElement("working_path");
                    emulatorWorkingPathNode.InnerText = emu.WorkingFolder;
                    emulatorNode.AppendChild(emulatorWorkingPathNode);

                    //Use quotes
                    XmlNode emulatorUseQuotesNode = doc.CreateElement("use_quotes");
                    emulatorUseQuotesNode.InnerText = emu.UseQuotes.ToString();
                    emulatorNode.AppendChild(emulatorUseQuotesNode);

                    //View
                    XmlNode emulatorViewNode = doc.CreateElement("view");
                    emulatorViewNode.InnerText = emu.View.ToString();
                    emulatorNode.AppendChild(emulatorViewNode);

                    //Arguments
                    XmlNode emulatorArgsNode = doc.CreateElement("args");
                    emulatorArgsNode.InnerText = emu.Arguments;
                    emulatorNode.AppendChild(emulatorArgsNode);

                    //Suspend MP
                    XmlNode emulatorSuspendMPNode = doc.CreateElement("suspend_mp");
                    emulatorSuspendMPNode.InnerText = emu.SuspendRendering.ToString();
                    emulatorNode.AppendChild(emulatorSuspendMPNode);

                    //Save the current emulator data
                    topEmulatorsNode.AppendChild(emulatorNode);

                    //Get the emulator games to be backed up
                    games.AddRange(DB.Instance.GetGames(emu));
                }

                //Get the PC games to be backed up.
                games.AddRange(DB.Instance.GetGames(Emulator.GetPC()));

                //Add games
                XmlNode topGameNode = doc.CreateElement("games");
                backupRootNode.AppendChild(topGameNode);

                foreach (Game game in games)
                {
                    XmlNode gameNode = doc.CreateElement("game");

                    //Path
                    XmlNode gamePathNode = doc.CreateElement("path");
                    gamePathNode.InnerText = game.Path;
                    gameNode.AppendChild(gamePathNode);

                    //Parent Emulator
                    XmlNode gameParentEmuNode = doc.CreateElement("parent_emulator");
                    gameParentEmuNode.InnerText = game.ParentEmulator.UID.ToString();
                    gameNode.AppendChild(gameParentEmuNode);

                    //Title
                    XmlNode gameTitleNode = doc.CreateElement("title");
                    gameTitleNode.InnerText = game.Title;
                    gameNode.AppendChild(gameTitleNode);

                    //Grade
                    XmlNode gameGradeNode = doc.CreateElement("grade");
                    gameGradeNode.InnerText = game.Grade.ToString();
                    gameNode.AppendChild(gameGradeNode);

                    //Play count
                    XmlNode gamePlayCountNode = doc.CreateElement("play_count");
                    gamePlayCountNode.InnerText = game.Playcount.ToString();
                    gameNode.AppendChild(gamePlayCountNode);

                    //Year made
                    XmlNode gameYearMadeNode = doc.CreateElement("year_made");
                    gameYearMadeNode.InnerText = game.Yearmade.ToString();
                    gameNode.AppendChild(gameYearMadeNode);

                    //Latest play
                    XmlNode gameLatestPlayNode = doc.CreateElement("latest_play");
                    gameLatestPlayNode.InnerText = game.Latestplay.ToString();
                    gameNode.AppendChild(gameLatestPlayNode);

                    //Description
                    XmlNode gameDescriptionNode = doc.CreateElement("description");
                    gameDescriptionNode.InnerText = game.Description;
                    gameNode.AppendChild(gameDescriptionNode);

                    //Genre
                    XmlNode gameGenreNode = doc.CreateElement("genre");
                    gameGenreNode.InnerText = game.Genre;
                    gameNode.AppendChild(gameGenreNode);

                    //Company
                    XmlNode gameCompanyNode = doc.CreateElement("company");
                    gameCompanyNode.InnerText = game.Company;
                    gameNode.AppendChild(gameCompanyNode);

                    //Visible
                    XmlNode gameVisibleNode = doc.CreateElement("visible");
                    gameVisibleNode.InnerText = game.Visible.ToString();
                    gameNode.AppendChild(gameVisibleNode);

                    //Favourite
                    XmlNode gameFavouriteNode = doc.CreateElement("favourite");
                    gameFavouriteNode.InnerText = game.Favourite.ToString();
                    gameNode.AppendChild(gameFavouriteNode);

                    topGameNode.AppendChild(gameNode);
                }

                doc.Save(outputPath);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
