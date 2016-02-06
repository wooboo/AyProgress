using System;

namespace AyProgress
{
    public class ProgressInfo
    {
        public ProgressInfo(double progress, TimeSpan timeFromStart, TimeSpan timeToFinish)
        {
            Progress = progress;
            TimeFromStart = timeFromStart;
            TimeToFinish = timeToFinish;
        }

        public double Progress { get; }
        public TimeSpan TimeFromStart { get; }
        public TimeSpan TimeToFinish { get; }
    }
}