using System;

namespace AyProgress
{
    public class ProgressReportedEventArgs : EventArgs
    {
        public ProgressReportedEventArgs(string key, double value, TimeSpan timeToFinish, string text)
        {
            Key = key;
            Value = value;
            TimeToFinish = timeToFinish;
            Text = text;
        }

        public string Key { get; }
        public double Value { get; }
        public TimeSpan TimeToFinish { get; }
        public string Text { get; }
    }
}