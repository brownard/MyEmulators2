using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MyEmulators2
{
    public class ScraperGame : IDisposable
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public string Year { get; set; }
        public string Grade { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public Bitmap BoxFront { get; set; }
        public Bitmap BoxBack { get; set; }
        public Bitmap TitleScreen { get; set; }
        public Bitmap InGame { get; set; }
        public Bitmap Fanart { get; set; }

        public string BoxFrontUrl { get; set; }
        public string BoxBackUrl { get; set; }
        public string TitleScreenUrl { get; set; }
        public string InGameUrl { get; set; }
        public string FanartUrl { get; set; }

        public ScraperGame(string title, string company, string year, string grade, string description, string genre)
        {

            Title = checkString(title);
            Company = checkString(company);
            Year = checkString(year);
            Grade = checkString(grade);
            Description = checkString(description);
            Genre = checkString(genre);
            BoxFront = null;
            BoxBack = null;
            TitleScreen = null;
            InGame = null;
            Fanart = null;
            BoxFrontUrl = null;
            BoxBackUrl = null;
            TitleScreenUrl = null;
            InGameUrl = null;
            FanartUrl = null;
        }
        
        string checkString(string s)
        {
            return string.IsNullOrEmpty(s) ? "" : s;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (BoxFront != null)
            {
                try
                { BoxFront.Dispose(); }
                catch { }
                BoxFront = null;
            }
            if (BoxBack != null)
            {
                try
                { BoxBack.Dispose(); }
                catch { }
                BoxBack = null;
            }
            if (TitleScreen != null)
            {
                try
                { TitleScreen.Dispose(); }
                catch { }
                TitleScreen = null;
            }
            if (InGame != null)
            {
                try
                { InGame.Dispose(); }
                catch { }
                InGame = null;
            }
            if (Fanart != null)
            {
                try
                { Fanart.Dispose(); }
                catch { }
                Fanart = null;
            }
        }

        #endregion
    }
}
