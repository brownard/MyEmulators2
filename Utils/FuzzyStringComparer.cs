using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cornerstone.Tools;
using System.Text.RegularExpressions;

namespace MyEmulators2
{
    class FuzzyStringComparer
    {        
        static Regex cleaner = new Regex(@"[^\w\s]", RegexOptions.IgnoreCase);
        public static int Score(string searchStr, string target)
        {
            int bestScore = int.MaxValue;
            int position = 0;
            int lastWhiteSpace = -1;

            searchStr = cleaner.Replace(searchStr, "").ToLower().Trim();
            string cleanTarget = cleaner.Replace(target, "").ToLower().Trim();

            // start off with a full string compare
            bestScore = AdvancedStringComparer.Levenshtein(searchStr, cleanTarget);

            // step through the movie title and try to match substrings of the same length as the search string
            while (position + searchStr.Length <= cleanTarget.Length)
            {
                string targetSubStr = cleanTarget.Substring(position, searchStr.Length);

                // base score
                int currScore = AdvancedStringComparer.Levenshtein(searchStr, targetSubStr);

                // penalty if the match starts mid word
                if (position - lastWhiteSpace > 1) currScore += 1;

                // penalty if the match ends mid word
                int trailingPos = position + searchStr.Length;
                if (trailingPos < cleanTarget.Length)
                {
                    char trailingChar = cleanTarget[trailingPos];
                    if (!char.IsWhiteSpace(trailingChar) && !char.IsPunctuation(trailingChar))
                        currScore++;
                }

                // penalty if it is a substring match
                currScore++;

                // store our new score as needed, upate state variables and move on
                if (bestScore > currScore) bestScore = currScore;
                if (targetSubStr.Length > 0 && (char.IsWhiteSpace(targetSubStr[0]) || char.IsPunctuation(targetSubStr[0]))) lastWhiteSpace = position;
                position++;
            }

            return bestScore;
        }

    }
}
