using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using MediaPortal.GUI.Library;
using MediaPortal.InputDevices;
using System.Windows.Forms;
using MediaPortal.Util;
using System.Text.RegularExpressions;

namespace MyEmulators2
{
    public delegate void ExecutorStatusChangedEventHandler(bool isRunning);
    public delegate bool ExecutorLaunchProgressHandler(string label, int perc);

    public class Executor : IDisposable
    {
        #region Singleton
        static object instanceSync = new object();
        static Executor instance = null;
        public static Executor Instance
        {
            get
            {
                if (instance == null)
                    lock (instanceSync)
                        if (instance == null)
                            instance = new Executor();
                return instance;
            }
        }

        #endregion
        object launchSync = new object();
        KeyboardHook keyHook = null;
        KeyEventHandler keyEvent = null;
        int keyData = 0;
        Process proc = null;
        public event ExecutorStatusChangedEventHandler StatusChanged;

        public static void LaunchDocument(DBItem item)
        {
            string manualPath = null;
            using (ThumbGroup thumbGroup = new ThumbGroup(item))
                manualPath = thumbGroup.ManualPath;

            if (string.IsNullOrEmpty(manualPath))
                return;

            //Execute
            using (Process proc = new Process())
            {
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.FileName = manualPath;
                proc.Start();
            }
        }
        
        /// <summary>
        /// Launches the specified game using the specified profile. If the specified profile is null
        /// the game's configured profile is used. If isConfig is true then any suspend settings are ignored
        /// and the specified game's playcount and playdate are not updated.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="isConfig">Whether the game is being launched by Configuration. Default false.</param>
        /// <param name="profile">The profile to use when launching the game.</param>
        public void LaunchGame(string path, EmulatorProfile profile, ExecutorLaunchProgressHandler launchProgressChanged = null)
        {
            if (profile == null)
                throw new LaunchException("Profile cannot be null");

            bool isPC = profile.EmulatorID == -1;
            bool isConfig = Emulators2Settings.Instance.IsConfig;

            ExecutorItem exeItem = new ExecutorItem(isPC);
            exeItem.Arguments = profile.Arguments;
            exeItem.Suspend = !isConfig && profile.SuspendMP == true;
            if (profile.DelayResume && profile.ResumeDelay > 0)
                exeItem.ResumeDelay = profile.ResumeDelay;

            if (isPC)
            {
                exeItem.Path = path;
                if (!string.IsNullOrEmpty(profile.LaunchedExe))
                {
                    try { exeItem.LaunchedExe = Path.GetFileNameWithoutExtension(profile.LaunchedExe); }
                    catch { exeItem.LaunchedExe = profile.LaunchedExe; }
                }
            }
            else
            {
                exeItem.Path = profile.EmulatorPath;
                exeItem.RomPath = path;
                exeItem.Mount = profile.MountImages && DaemonTools.IsImageFile(Path.GetExtension(path));
                exeItem.ShouldReplaceWildcards = !exeItem.Mount;
                exeItem.UseQuotes = profile.UseQuotes == true;
                exeItem.CheckController = profile.CheckController;
                exeItem.StopEmulationOnKey = profile.StopEmulationOnKey.HasValue ? profile.StopEmulationOnKey.Value : Options.Instance.GetBoolOption("domap");
                exeItem.EscapeToExit = profile.EscapeToExit;
            }
            exeItem.PreCommand = new LaunchCommand() { Command = profile.PreCommand, WaitForExit = profile.PreCommandWaitForExit, ShowWindow = profile.PreCommandShowWindow };
            exeItem.PostCommand = new LaunchCommand() { Command = profile.PostCommand, WaitForExit = profile.PostCommandWaitForExit, ShowWindow = profile.PostCommandShowWindow };
            exeItem.Init();

            //stop any media playing
            if (!isConfig && Options.Instance.GetBoolOption("stopmediaplayback") && MediaPortal.Player.g_Player.Playing)
            {
                Logger.LogDebug("Stopping playing media...");
                MediaPortal.Player.g_Player.Stop();
            }

            lock (launchSync)
            {
                launchGame(exeItem, launchProgressChanged);
            }
        }

        void launchGame(ExecutorItem exeItem, ExecutorLaunchProgressHandler launchProgressChanged)
        {
            Dispose();

            if (exeItem.Mount)
            {
                if (launchProgressChanged != null)
                    launchProgressChanged("Mounting...", 75);
                mountImage(exeItem.RomPath);
            }
            else if (launchProgressChanged != null)
                launchProgressChanged("Launching...", 75);

            List<int> pIds = new List<int>();
            initProc(exeItem, new EventHandler((o, e) => { onExit(exeItem, pIds); }));

            Logger.LogInfo("Launching with arguments '{1} {2}'", exeItem.Path, exeItem.Arguments);
            if (launchProgressChanged != null)
                launchProgressChanged("Launching...", 100);

            if (exeItem.CheckController && !ControllerHandler.CheckControllerState())
                Emulators2Settings.Instance.ShowMPDialog(Translator.Instance.nocontrollerconnected);

            if (exeItem.Suspend)
                suspendMP(true); //suspend MediaPortal rendering

            //run pre-command
            exeItem.PreCommand.Run();

            //if we're a launcher get id of all processes currently running with the same name so they can be excluded when looking for launched process
            if (!string.IsNullOrEmpty(exeItem.LaunchedExe))
                foreach (Process p in Process.GetProcessesByName(exeItem.LaunchedExe))
                    pIds.Add(p.Id);

            //start emulator
            bool result;
            try
            {
                result = proc.Start();
            }
            catch(Exception ex)
            {
                result = false;
                Logger.LogError("Exception starting {0} - {1}", exeItem.Path, ex.Message);
            }

            if (!result)
            {
                Logger.LogWarn("Failed to start {0}", exeItem.Path);
                onExit(exeItem, null, false);
                throw new LaunchException("{0} did not start", exeItem.Path);
            }

            //send status changed event
            if (StatusChanged != null)
                StatusChanged(true);

            //setup keyboard hook if configured and we're not launching a PC Game (they should have their own way of exiting)
            if (exeItem.StopEmulationOnKey)
                setupKeyboardHook(exeItem.EscapeToExit);
        }

        void waitForLaunchedProc(string p, List<int> pIds)
        {
            if (string.IsNullOrEmpty(p) || pIds == null)
                return;
            bool check = pIds.Count > 0;
            foreach (Process proc in System.Diagnostics.Process.GetProcessesByName(p))
            {
                if (check && pIds.Contains(proc.Id))
                    continue;
                proc.WaitForExit();
                break;
            }
        }

        void onExit(ExecutorItem exeItem, List<int> pIds, bool started = true)
        {
            Dispose();
            if (started)
                waitForLaunchedProc(exeItem.LaunchedExe, pIds);
            if (exeItem.Mount)
                unMount();
            if (exeItem.PostCommand != null)
                exeItem.PostCommand.Run();
            //resume Mediaportal rendering
            if (exeItem.Suspend)
            {
                if (started && exeItem.ResumeDelay > 0)
                    Thread.Sleep(exeItem.ResumeDelay);
                suspendMP(false);
            }
            if (StatusChanged != null)
                StatusChanged(false); 
        }

        void mountImage(string path)
        {
            Logger.LogInfo("Attempting to mount image file '{0}'", path);
            if (!DaemonTools.IsEnabled)
                throw new LaunchException("Attempt to mount image file '{0}' failed, Mediaportal's Virtual Drive is not enabled", path);

            string drive;
            DaemonTools.UnMount();
            if(!DaemonTools.Mount(path, out drive))
                throw new LaunchException("Attempt to mount image file '{0}' failed", path);
        }

        void initProc(ExecutorItem item, EventHandler procExit)
        {
            proc = new Process();
            proc.StartInfo = new ProcessStartInfo();
            proc.StartInfo.FileName = item.Path;
            proc.StartInfo.Arguments = item.Arguments;
            proc.StartInfo.WorkingDirectory = item.WorkingDirectory;
            proc.EnableRaisingEvents = true;
            proc.Exited += procExit;
        }

        //get key mapping and setup keyboard hook
        void setupKeyboardHook(bool escapeToExit)
        {
            Logger.LogInfo("Stop emulation on mapped key selected. Initialising keyboard hook...");
            keyData = Options.Instance.GetIntOption("mappedkeydata");
            if (keyData < 1)
            {
                Logger.LogError("Keyboard hook - No key has been mapped");
                return;
            }
            keyEvent = new KeyEventHandler((o, e) => { onMappedKey(e, escapeToExit); });
            keyHook = new KeyboardHook(proc.Id, keyEvent); //setup hook and attach to emu process
            Logger.LogDebug("Keyboard hook - Emulation will be stopped on '{0}'", Options.GetKeyDisplayString(keyData));
        }

        void onMappedKeyDoClose(object sender, KeyEventArgs e)
        {
            Logger.LogDebug("MAPPED KEY");
            onMappedKey(e, false);
        }
        void onMappedKeyDoEsc(object sender, KeyEventArgs e)
        {
            onMappedKey(e, true);
        }
        //Fired when a key press is detected by the keyboard hook
        void onMappedKey(KeyEventArgs e, bool escapeToExit)
        {
            if ((int)e.KeyData == keyData) //key pressed is mapped key
            {
                Logger.LogDebug("Keyboard hook - Mapped key pressed, stopping emulation");
                e.Handled = true;
                e.SuppressKeyPress = true;
                
                int Msg;
                uint wParam;
                if (escapeToExit)
                {
                    //set message to Esc key press
                    Msg = KeyboardHook.WM_KEYDOWN;
                    wParam = KeyboardHook.VK_ESCAPE;
                }
                else
                {
                    //Set message to window close
                    Msg = KeyboardHook.WM_QUIT;
                    wParam = 0;
                }

                try
                {
                    IntPtr wH = proc.MainWindowHandle;
                    if (wH != IntPtr.Zero)
                        KeyboardHook.PostMessage(wH, Msg, wParam, 0);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Keyboard hook - error sending close message to emulator - {0}", ex.Message);
                }
            }
        }

        void disposeHook()
        {
            if (keyHook != null)
            {
                Logger.LogDebug("Keyboard hook - Disposing...");
                keyHook.Dispose();
                keyHook = null;
            }
        }

        void disposeProc()
        {
            if (proc != null)
            {
                proc.Dispose();
                proc = null;
            }
        }
                
        void unMount()
        {
            if (DaemonTools.IsEnabled)
                DaemonTools.UnMount();
        }

        /* The following code was referenced from the Moving Pictures plugin */
        static void suspendMP(bool suspend)
        {
            if (suspend) //suspend and hide MediaPortal
            {
                Logger.LogDebug("Suspending MediaPortal...");
                // disable mediaportal input devices
                InputDevices.Stop();

                // hide mediaportal and suspend rendering to save resources for the pc game
                GUIGraphicsContext.BlankScreen = true;
                GUIGraphicsContext.form.Hide();
                GUIGraphicsContext.CurrentState = GUIGraphicsContext.State.SUSPENDING;
            }
            else //resume Mediaportal
            {
                Logger.LogDebug("Resuming MediaPortal...");

                InputDevices.Init();
                // Resume Mediaportal rendering
                GUIGraphicsContext.BlankScreen = false;
                GUIGraphicsContext.form.Show();
                GUIGraphicsContext.ResetLastActivity();
                GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_GETFOCUS, 0, 0, 0, 0, 0, null);
                GUIWindowManager.SendThreadMessage(msg);
                GUIGraphicsContext.CurrentState = GUIGraphicsContext.State.RUNNING;
            }
        }

        public void Dispose()
        {
            disposeHook();
            disposeProc();
        }        
    }
}
