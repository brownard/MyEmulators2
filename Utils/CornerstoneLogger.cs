using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;

namespace Cornerstone
{
    public class CornerstoneLogger
    {
        public CornerstoneLogger() { }
        const string logAppend = "Emulators2 Cornerstone: ";
        public void Debug(string format, params object[] args)
        {
            Log.Debug(logAppend + string.Format(format, args));
        }
        public void Error(string format, params object[] args)
        {
            Log.Error(logAppend + string.Format(format, args));
        }
        public void Error(string message, Exception e)
        {
            Log.Error(logAppend + message + " - Exception: " + e.Message);
        }
        public void Warn(string format, params object[] args)
        {
            Log.Warn(logAppend + string.Format(format, args));
        }

        internal void DebugException(string message, Exception e)
        {
            Log.Debug(logAppend + message + " - Exception: " + e.Message);
        }

        internal void ErrorException(string message, Exception e)
        {
            Log.Error(logAppend + message + " - Exception: " + e.Message);
        }
    }
}
