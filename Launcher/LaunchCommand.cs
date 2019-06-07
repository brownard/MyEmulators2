using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MyEmulators2
{
    class LaunchCommand
    {
        public string Command { get; set; }
        public bool WaitForExit { get; set; }
        public bool ShowWindow { get; set; }
        public void Run()
        {
            run(Command, WaitForExit, ShowWindow);
        }

        static void run(string command, bool waitForExit, bool showWindow)
        {
            if (string.IsNullOrEmpty(command))
                return;
            using (Process cmd = new Process())
            {
                cmd.StartInfo = new ProcessStartInfo("cmd.exe", string.Format("/C {0}", command));
                cmd.StartInfo.WindowStyle = showWindow ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
                try
                {
                    cmd.Start();
                    if (waitForExit)
                        cmd.WaitForExit();
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error running command line '{0}' - {1}", command, ex.Message);
                }
            }
        }
    }
}
