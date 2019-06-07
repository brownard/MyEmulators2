using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MyEmulators2
{
    //The super class to all the content panels in the Configuration form

    class ContentPanel : UserControl
    {
        public virtual void UpdatePanel() { }
        public virtual void SavePanel() { }
        public virtual void ClosePanel() { }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public Form form
        {
            get;
            set;
        }
    }
}
