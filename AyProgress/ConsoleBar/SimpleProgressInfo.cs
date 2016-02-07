using System;

namespace AyProgress.ConsoleBar
{
    public class SimpleProgressInfo : IProgressInfo
    {
        public SimpleProgressInfo(double value, string text, TimeSpan timeToFinish)
        {
            Value = value;
            Text = text;
            TimeToFinish = timeToFinish;
        }

        public double Value { get; }
        public string Text { get; }
        public TimeSpan TimeToFinish { get; }
    }
}