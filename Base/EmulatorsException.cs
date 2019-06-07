using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2
{
    class EmulatorsException : Exception
    {
        public EmulatorsException(string format, params object[] args)
        {
            this.message = string.Format(format, args);
        }

        protected string message;
        public override string Message
        {
            get
            {
                return message;
            }
        }
    }

    class LaunchException : EmulatorsException
    {
        public LaunchException(string format, params object[] args)
            : base(format, args)
        { }
    }

    class ExtractException : EmulatorsException
    {
        public ExtractException(string format, params object[] args)
            : base(format, args)
        { }
    }
}
