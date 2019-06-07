using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using MyEmulators2.Import;

namespace MyEmulators2
{
    partial class GUIPresenter
    {
        object importControllerSync = new object();
        Importer importer = null;
        bool? autoimport = null;
        volatile bool restarting = false;

        void initImporter()
        {
            GUIPropertyManager.SetProperty("#Emulators2.Importer.working", "no");

            if (Options.Instance.GetBoolOption("autoimportgames"))
                autoimport = true;
            else if (Options.Instance.GetBoolOption("autorefreshgames"))
                autoimport = false;
            else
                autoimport = null;
                        
            importer = new Importer(true, autoimport == false);
            importer.ImportStatusChanged += new ImportStatusChangedHandler(importerStatusChangedHandler);
            importer.RomStatusChanged += new RomStatusChangedHandler(romStatusChangedHandler);
            //pause importer during game execution
            Executor.Instance.StatusChanged += new ExecutorStatusChangedEventHandler(executorStatusChangedHandler);
            if (autoimport != null)
                importer.Start();
        }

        void resumeImporter()
        {
            if (importer == null)
                return;

            importer.UnPause();
        }

        public void RestartImporter()
        {
            if (restarting)
                return;

            lock (importControllerSync)
            {                
                importer.JustRefresh = false;
                restarting = true;
                importer.Restart();
            }
        }

        public void AddToImporter(Game game)
        {
            if (game != null)
                importer.AddGames(new Game[] { game });
        }

        void importerStatusChangedHandler(object sender, ImportStatusChangedEventArgs e)
        {
            bool working = false;
            switch (e.Action)
            {
                case ImportAction.ImportStarted:
                    restarting = false;
                    working = true;
                    break;
                case ImportAction.ImportResumed:
                    working = true;
                    break;
                case ImportAction.NewFilesFound:
                    working = true;
                    UpdateFacade();
                    break;
                case ImportAction.ImportFinished:
                    UpdateFacade();
                    break;
                case ImportAction.ImportPaused:
                    break;
                default:
                    return;
            }
            GUIPropertyManager.SetProperty("#Emulators2.Importer.working", working ? "yes" : "no");
            Logger.LogDebug("Importer action: {0}", e.Action.ToString());
        }

        void romStatusChangedHandler(object sender, RomStatusChangedEventArgs e)
        {
            if (e.Status == RomMatchStatus.Committed)
            {
                Game game = DB.Instance.GetGame(e.RomMatch.ID);
                Logger.LogDebug("Importer action: {0} updated", game.Title);
                UpdateGame(game);
            }
        }

        void executorStatusChangedHandler(bool isRunning)
        {
            lock (importControllerSync)
            {
                if (importer == null)
                    return;
                //toggle pause state
                if (isRunning)
                    importer.Pause();
                else
                    importer.UnPause();
            }
        }
    }
}
