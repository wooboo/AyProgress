using System;

namespace AyProgress
{
    public class DoubleProgressAdapter : ProgressAdapter<double>
    {
        public DoubleProgressAdapter(IProgress<double> progress, string key) : base(progress, key)
        {
        }

        public DoubleProgressAdapter(IProgress<double> progress, Func<ProgressReportedEventArgs, bool> filter = null)
            : base(progress, filter)
        {
        }

        protected override void Report(ProgressReportedEventArgs e)
        {
            Progress.Report(e.Value);
        }
    }
}