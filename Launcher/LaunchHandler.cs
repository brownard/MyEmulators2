using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MediaPortal.GUI.Library;
using MediaPortal.Dialogs;

namespace MyEmulators2
{
    class LaunchHandler
    {
        #region Singleton

        protected static object instanceSync = new object();
        protected static LaunchHandler instance = null;
        public static LaunchHandler Instance
        {
            get
            {
                if (instance == null)
                    lock (instanceSync)
                        if (instance == null)
                            instance = new LaunchHandler();
                return instance;
            }
        }

        #endregion

        public void StartLaunch(Game game)
        {
            if (game == null)
                return;

            EmulatorProfile profile = game.GetSelectedProfile();
            bool isConfig = Emulators2Settings.Instance.IsConfig;
            BackgroundTaskHandler handler = new BackgroundTaskHandler();
            handler.ActionDelegate = () =>
            {
                string errorStr = null;
                string path = null;
                if (game.IsGoodmerge)
                {
                    try
                    {
                        path = Extractor.Instance.ExtractGame(game, profile, (l, p) =>
                        {
                            handler.ExecuteProgressHandler(p, l);
                            return true;
                        });
                    }
                    catch (ExtractException ex)
                    {
                        errorStr = ex.Message;
                    }
                }
                else
                    path = game.CurrentDisc.Path;

                GUIGraphicsContext.form.Invoke(new System.Windows.Forms.MethodInvoker(() =>
                {
                    if (path != null)
                    {
                        try
                        {
                            handler.ExecuteProgressHandler(50, "Launching " + game.Title);
                            Executor.Instance.LaunchGame(path, profile, (l, p) =>
                            {
                                handler.ExecuteProgressHandler(p, l);
                                return true;
                            });

                            if (!isConfig)
                                game.UpdateAndSaveGamePlayInfo();
                        }
                        catch (LaunchException ex)
                        {
                            Logger.LogError(ex.Message);
                            errorStr = ex.Message;
                        }
                    }

                    if (errorStr != null)
                        Emulators2Settings.Instance.ShowMPDialog("Error\r\n{0}", errorStr);
                }));
            };

            if (isConfig)
            {
                using (Conf_ProgressDialog dlg = new Conf_ProgressDialog(handler))
                    dlg.ShowDialog();
            }
            else
            {
                GUIProgressDialogHandler guiDlg = new GUIProgressDialogHandler(handler);
                guiDlg.ShowDialog();
            }
        }
    }
}
