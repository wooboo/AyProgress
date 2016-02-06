using System;
using System.Collections.Generic;
using System.Linq;

namespace AyProgress
{
    public class ProgressContext : IDisposable
    {
        private readonly Action<ProgressContext> _returnParent;

        public ProgressContext(ProgressState state, string key, ProgressContext parent, double min, double max,
            Action<ProgressContext> returnParent)
        {
            _returnParent = returnParent;
            State = state;
            Key = key;
            Parent = parent;
            Start = DateTime.Now;
            Min = min;
            Max = max;
        }

        public ProgressState State { get; }

        public double Max { get; }
        public double Min { get; }
        public double Current { get; private set; }
        public DateTime ReportTime { get; private set; }

        public string Key { get; }
        public ProgressContext Parent { get; }
        public DateTime Start { get; }

        public void Dispose()
        {
            _returnParent?.Invoke(Parent);
        }

        public event EventHandler<ProgressReportedEventArgs> ProgressReported;

        private void OnProgressChanged(ProgressReportedEventArgs e)
        {
            ProgressReported?.Invoke(this, e);
        }

        public void Report(double value)
        {
            Current = value;
            ReportTime = DateTime.Now;
            TimeSpan timeToFinish;
            if (TryGetTimeToFinish(value, out timeToFinish))
            {
            }
            Parent?.Report(Min + (Max - Min)*value);
            OnProgressChanged(new ProgressReportedEventArgs(Key, value, timeToFinish));
        }

        private bool TryGetTimeToFinish(double value, out TimeSpan t)
        {
            var result = false;
            var time = DateTime.Now - Start;
            var values = State.GetStates(Key);
            t = TimeSpan.Zero;
            if (values.Count() >= 2)
            {
                var v = GetY(values, value);
                time = new TimeSpan((time + v).Ticks/2);
                t = values.Last().Time - time;
                result = true;
            }

            var exact = values.SingleOrDefault(o => Math.Abs(o.Value - value) < 0.01);
            if (exact == null)
            {
                exact = new StateItem
                {
                    Key = Key,
                    Value = value,
                    Time = time
                };
                State.Add(exact);
            }
            else
            {
                exact.Time = time;
            }

            return result;
        }

        public static TimeSpan GetY(IEnumerable<StateItem> values, double x)
        {
            var states = values as StateItem[] ?? values.ToArray();
            var point1 = states.First();
            var point2 = states.Last();
            var m = (long) ((point2.Time - point1.Time).Ticks/(point2.Value + point1.Value));
            var b = point1.Time.Ticks - (long) (m*point1.Value);

            return new TimeSpan((long) (m*x + b));
        }
    }
}