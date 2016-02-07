using System;

namespace AyProgress.ConsoleBar
{
    public class DoubleProgressAdapter : ProgressAdapter<SimpleProgressInfo>
    {
        public DoubleProgressAdapter(IProgress<SimpleProgressInfo> progress, string key) : base(progress, key)
        {
        }

        public DoubleProgressAdapter(IProgress<SimpleProgressInfo> progress, Func<ProgressReportedEventArgs, bool> filter = null)
            : base(progress, filter)
        {
        }

        protected override void Report(ProgressReportedEventArgs e)
        {
            Progress.Report(new SimpleProgressInfo(e.Value));
        }
    }
}