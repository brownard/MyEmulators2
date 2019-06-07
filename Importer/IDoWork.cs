using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2
{
    public delegate bool DoWorkDelegate();
    interface IDoWork
    {
        event DoWorkDelegate DoWork;
    }
}
