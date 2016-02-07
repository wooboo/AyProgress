using System;

namespace AyProgress.ConsoleBar
{
    public interface IProgressInfo
    {
        double Value { get; }
        string Text { get; }
        TimeSpan TimeToFinish { get; }
    }
}