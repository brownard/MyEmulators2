using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;

namespace MyEmulators2
{
    static class Logger
    {
        public static void LogInfo(string format, params object[] args)
        {
            Log.Info("Emulators 2: " + format, args);
        }
        public static void LogDebug(string format, params object[] args)
        {
            Log.Debug("Emulators 2: " + format, args);
        }
        public static void LogWarn(string format, params object[] args)
        {
            Log.Warn("Emulators 2: " + format, args);
        }
        public static void LogError(string format, params object[] args)
        {
            Log.Error("Emulators 2: " + format, args);
        }
        public static void LogError(Exception ex)
        {
            Log.Error("Emulators 2: Exception - {0}", ex.Message);
        }
    }
}
