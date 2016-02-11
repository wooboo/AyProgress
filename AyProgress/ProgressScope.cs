using System;

namespace AyProgress
{
    public class ProgressScope
    {
        private static readonly ProgressState State = new ProgressState("state.json");
        private static ProgressContext _current;

        public static event EventHandler<ProgressReportedEventArgs> ProgressReported;

        public static ProgressContext Start(string key, double min, double max, string text = null)
        {
            _current = new ProgressContext(State, key, _current, min, max, ReturnParent);
            _current.ProgressReported += OnProgressReported;
            _current.Report(0, text);
            return _current;
        }

        public static ProgressContext Start(string key, long min, long max, long of, string text = null)
        {
            return Start(key, (double) min/of, (double) max/of, text);
        }

        private static void ReturnParent(ProgressContext progressContext)
        {
            _current.Report(1);
            _current.ProgressReported -= OnProgressReported;
            _current = progressContext;
            State.Save();
        }

        public static void Report(double value, string text = null)
        {
            _current.Report(value, text);
        }
        public static void Report(long index, long of, string text = null)
        {
            _current.Report((double)index / of, text);
        }

        private static void OnProgressReported(object sender, ProgressReportedEventArgs e)
        {
            ProgressReported?.Invoke(sender, e);
        }
    }
}