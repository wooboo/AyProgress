using System;

namespace AyProgress
{
    public abstract class ProgressAdapter<T>
    {
        private readonly Func<ProgressReportedEventArgs, bool> _filter;
        protected readonly IProgress<T> Progress;

        protected ProgressAdapter(IProgress<T> progress, string key) : this(progress, p => p.Key == key)
        {
        }

        protected ProgressAdapter(IProgress<T> progress, Func<ProgressReportedEventArgs, bool> filter = null)
        {
            Progress = progress;
            _filter = filter ?? (p => true);
            AyProgress.ProgressScope.ProgressReported += Progress_ProgressReported;
        }

        private void Progress_ProgressReported(object sender, ProgressReportedEventArgs e)
        {
            if (_filter(e))
            {
                Report(e);
            }
        }

        protected abstract void Report(ProgressReportedEventArgs e);
    }
}