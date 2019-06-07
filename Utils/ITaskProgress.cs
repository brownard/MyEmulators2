using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2
{
    public delegate void BackgroundTaskProgress(int percent, string info);
    public interface ITaskProgress
    {
        event BackgroundTaskProgress OnTaskProgress;
        bool Start();
        void Stop();
        bool IsComplete { get; }
    }
}
