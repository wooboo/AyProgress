using System;

namespace AyProgress
{
    public class Progress
    {
        private static readonly ProgressState State = new ProgressState("state.json");
        private static ProgressContext _current;

        public static event EventHandler<ProgressEventArgs> ProgressChanged
        {
            add { State.ProgressChanged += value; }
            remove { State.ProgressChanged -= value; }
        }

        public static ProgressContext Start(string key, double min, double max)
        {
            _current = new ProgressContext(State, key, _current, min, max, ReturnParent);
            return _current;
        }

        private static void ReturnParent(ProgressContext progressContext)
        {
            _current = progressContext;
        }

        public static void Report(double value)
        {
            _current.Report(value);
        }
    }
}