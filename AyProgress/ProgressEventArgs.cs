using System;

namespace AyProgress
{
    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(ProgressInfo globalProgressInfo)
        {
            GlobalProgressInfo = globalProgressInfo;
        }

        public ProgressInfo GlobalProgressInfo { get; set; }
    }
}