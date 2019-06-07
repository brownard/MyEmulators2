using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace MyEmulators2
{
    class ExecutorItem
    {
        bool isPC = false;
        public bool IsPC { get { return isPC; } }
        string path = null;
        public string Path { get { return path; } set { path = value; } }
        string arguments = null;
        public string Arguments { get { return arguments; } set { arguments = value; } }
        string workingDirectory = null;
        public string WorkingDirectory { get { return workingDirectory; } set { path = workingDirectory; } }
        string romPath = null;
        public string RomPath { get { return romPath; } set { romPath = value; } }
        bool shouldReplaceWildcards = false;
        public bool ShouldReplaceWildcards { get { return shouldReplaceWildcards; } set { shouldReplaceWildcards = value; } }
        bool useQuotes = false;

        public bool UseQuotes { get { return useQuotes; } set { useQuotes = value; } }
        public bool Mount { get; set; }
        public bool Suspend { get; set; }
        public int ResumeDelay { get; set; }
        public string LaunchedExe { get; set; }
        public LaunchCommand PreCommand { get; set; }
        public LaunchCommand PostCommand { get; set; }
        public bool CheckController { get; set; }
        public bool StopEmulationOnKey { get; set; }
        public bool EscapeToExit { get; set; }

        public ExecutorItem(bool isPC)
        {
            this.isPC = isPC;
        }

        public void Init()
        {
            if (isPC)
            {
                if (System.IO.Path.GetExtension(path).ToLower() == ".lnk")
                    initShortcut();
                else
                    initPCGame();
            }
            else
                initRom();
        }

        void initRom()
        {
            if (!File.Exists(path))
                throw new LaunchException("Unable to locate emulator exe {0}", path);

            if (shouldReplaceWildcards)
                arguments = replaceWildcards(arguments, romPath, useQuotes);
            else
                arguments = removeWildcards(arguments);
            if (string.IsNullOrEmpty(workingDirectory) || !Directory.Exists(workingDirectory))
                workingDirectory = System.IO.Path.GetDirectoryName(path);
        }

        void initPCGame()
        {
            string parsedPath, parsedArgs;
            if (path.TryGetExecutablePath(out parsedPath, out parsedArgs))
            {
                path = parsedPath;
                if (string.IsNullOrEmpty(arguments))
                    arguments = parsedArgs;
            }

            if (!File.Exists(path))
                throw new LaunchException("Unable to locate PC game {0}", path);
                
            workingDirectory = System.IO.Path.GetDirectoryName(path);
        }

        void initShortcut()
        {
            if (!File.Exists(path))
                throw new LaunchException("Unable to locate shortcut {0}", path);                
                
            try
            {
                Logger.LogDebug("Reading shortcut {0}", path);
                IWshRuntimeLibrary.IWshShell ws = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut sc = (IWshRuntimeLibrary.IWshShortcut)ws.CreateShortcut(Path);
                Logger.LogDebug("\r\n\tShortcut target path: {0}\r\n\tShortcut arguments: {1}\r\n\tShortcut working directory: {2}", sc.TargetPath, sc.Arguments, sc.WorkingDirectory);
                if (!string.IsNullOrEmpty(sc.TargetPath))
                    path = sc.TargetPath;
                if (string.IsNullOrEmpty(arguments))
                    arguments = sc.Arguments;
                if (string.IsNullOrEmpty(workingDirectory))
                    workingDirectory = sc.WorkingDirectory;
            }
            catch (Exception ex)
            {
                throw new LaunchException("Error reading shortcut {0} - {1}", path, ex.Message);
            }
        }

        string replaceWildcards(string args, string romPath, bool useQuotes)
        {
            string fmt = useQuotes ? "\"{0}\"" : "{0}";
            bool foundWildcard = false;
            if (args.Contains(Emulators2Settings.GAME_WILDCARD))
            {
                foundWildcard = true;
                args = args.Replace(Emulators2Settings.GAME_WILDCARD, string.Format(fmt, romPath));
            }
            if (args.Contains(Emulators2Settings.GAME_WILDCARD_NO_EXT))
            {
                foundWildcard = true;
                string filename = System.IO.Path.GetFileNameWithoutExtension(romPath);
                args = args.Replace(Emulators2Settings.GAME_WILDCARD_NO_EXT, string.Format(fmt, filename));
            }
            if (!foundWildcard)
            {
                if (!args.EndsWith(" "))
                    args += " ";
                args += string.Format(fmt, romPath);
            }
            return args;
        }

        string removeWildcards(string args)
        {
            return args.Replace(Emulators2Settings.GAME_WILDCARD, "").Replace(Emulators2Settings.GAME_WILDCARD_NO_EXT, "").Trim();
        }
    }
}
