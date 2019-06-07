using MyEmulators2.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyEmulators2
{
    public class RomMatch
    {
        static Regex romCodeRegEx = new Regex(@"[(\[].*?[)\]]");
        static Regex theAppendRegEx = new Regex(@", the\b", RegexOptions.IgnoreCase);
        public RomMatch(Game game)
        {
            if (game != null)
            {
                this.game = game;
                if (!string.IsNullOrEmpty(game.SearchTitle))
                    Title = game.SearchTitle;
                else
                {
                    string title = romCodeRegEx.Replace(game.Title, "").Trim();
                    Match m = theAppendRegEx.Match(title);
                    if (m.Success)
                        title = "the " + title.Remove(m.Index, m.Length);
                    Title = title;
                }

                id = game.GameID;
                Path = game.Path;
            }
            PossibleGameDetails = new List<ScraperResult>();
        }

        Game game = null;
        public Game Game
        {
            get { return game; }
        }

        object syncRoot = new object();
        public object SyncRoot { get { return syncRoot; } }

        public ScraperResult GameDetails
        {
            get;
            set;
        }

        public List<ScraperResult> PossibleGameDetails
        {
            get;
            set;
        }

        public ScraperGame ScraperGame
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        bool? isMultiFile = null;
        public bool IsMultiFile
        {
            get
            {
                if (isMultiFile == null)
                    isMultiFile = game != null && game.GetDiscs().Count > 1;
                return isMultiFile == true;
            }
        }

        string displayFilename = null;
        public string DisplayFilename
        {
            get
            {
                if (displayFilename == null)
                {
                    if (IsMultiFile)
                        displayFilename = "Multiple files";
                    else
                        displayFilename = System.IO.Path.GetFileName(Path);
                }
                return displayFilename;
            }
        }

        string toolTip = null;
        public string ToolTip 
        { 
            get 
            {
                if (toolTip == null)
                {
                    List<GameDisc> discs = game.GetDiscs();
                    for (int x = 0; x < discs.Count; x++)
                    {
                        if (x > 0)
                            toolTip += ",\r\n";
                        toolTip += discs[x].Path;
                    }
                }
                return toolTip;
            } 
        }

        public void ResetDisplayInfo()
        {
            isMultiFile = null;
            displayFilename = null;
            toolTip = null;
        }

        int id = -1;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public bool HighPriority
        {
            get;
            set;
        }
        
        RomMatchStatus status = RomMatchStatus.PendingHash;
        public RomMatchStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        public int? CurrentThreadId
        {
            get;
            set;
        }

        public bool OwnedByThread()
        {
            bool result;
            lock (syncRoot)
                result = CurrentThreadId == System.Threading.Thread.CurrentThread.ManagedThreadId;
            return result;
        }

        public int BindingSourceIndex { get; set; }
    }

    public enum RomMatchStatus
    {
        PendingHash = 0,
        PendingServer = 1,
        PendingMatch = 2,
        Approved = 3,
        Committed = 4,
        NeedsInput = 5,
        Removed = 6,
        Ignored = 7
    }
}
