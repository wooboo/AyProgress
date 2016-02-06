using System;

namespace AyProgress
{
    public class ProgressReportedEventArgs : EventArgs
    {
        public ProgressReportedEventArgs(string key, double value, TimeSpan timeToFinish)
        {
            Key = key;
            Value = value;
            TimeToFinish = timeToFinish;
        }

        public string Key { get; set; }
        public double Value { get; }
        public TimeSpan TimeToFinish { get; }
    }
}