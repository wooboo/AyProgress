using System;

namespace AyProgress
{
    public class Progress
    {
        private static readonly ProgressState State = new ProgressState("state.json");
        private static ProgressContext _current;

        public static event EventHandler<ProgressReportedEventArgs> ProgressReported;

        public static ProgressContext Start(string key, double min, double max)
        {
            _current = new ProgressContext(State, key, _current, min, max, ReturnParent);
            _current.ProgressReported += OnProgressReported;
            _current.Report(0);
            return _current;
        }

        private static void ReturnParent(ProgressContext progressContext)
        {
            _current.Report(1);
            _current.ProgressReported -= OnProgressReported;
            _current = progressContext;
            State.Save();
        }

        public static void Report(double value)
        {
            _current.Report(value);
        }

        private static void OnProgressReported(object sender, ProgressReportedEventArgs e)
        {
            ProgressReported?.Invoke(sender, e);
        }
    }
}