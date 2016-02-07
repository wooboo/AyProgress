namespace AyProgress.ConsoleBar
{
    public class SimpleProgressInfo : IProgressInfo
    {
        public SimpleProgressInfo(double value)
        {
            Value = value;
        }

        public double Value { get; }
    }
}