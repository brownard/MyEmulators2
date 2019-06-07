using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;

namespace MyEmulators2
{
    class GUIProgressDialogHandler : IDisposable
    {
        ITaskProgress handler;
        GUIDialogProgress dlgPrgrs = null;
        public GUIProgressDialogHandler(ITaskProgress handler)
        {
            this.handler = handler;
        }

        public void ShowDialog()
        {
            if (handler == null)
                return;
            handler.OnTaskProgress += new BackgroundTaskProgress(setProgress);
            closeProgDialog();
            dlgPrgrs = (GUIDialogProgress)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_PROGRESS);
            if (dlgPrgrs != null)
            {
                dlgPrgrs.Reset();
                dlgPrgrs.DisplayProgressBar = true;
                dlgPrgrs.ShowWaitCursor = false;
                dlgPrgrs.DisableCancel(true);
                dlgPrgrs.SetHeading("");
                dlgPrgrs.SetLine(1, "");
                dlgPrgrs.StartModal(GUIWindowManager.ActiveWindow);
            }
            else
            {
                GUIWaitCursor.Init(); GUIWaitCursor.Show();
            }
            if (!handler.Start())
            {
                closeProgDialog();
                return;
            }
        }

        void setProgress(int percent, string info)
        {
            if (dlgPrgrs != null)
            {
                dlgPrgrs.SetPercentage(percent);
                dlgPrgrs.SetLine(1, info);
            }
            if (handler.IsComplete)
                closeProgDialog();
        }

        void closeProgDialog()
        {
            if (dlgPrgrs != null)
            {
                dlgPrgrs.Close();
                dlgPrgrs.Dispose();
                dlgPrgrs = null;
            }
            else
                GUIWaitCursor.Hide();
        }

        #region IDisposable Members

        public void Dispose()
        {
            closeProgDialog();
        }

        #endregion
    }
}
