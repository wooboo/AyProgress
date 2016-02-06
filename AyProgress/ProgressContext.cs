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
            StateStart(min);
        }

        public ProgressState State { get; }

        public double Max { get; }
        public double Min { get; }

        public string Key { get; }
        public ProgressContext Parent { get; }
        private DateTime Start { get; }

        public void Dispose()
        {
            StateFinish(Max, DateTime.Now - Start);
            _returnParent?.Invoke(Parent);
        }

        public event EventHandler<ProgressEventArgs> ProgressChanged;

        private void OnProgressChanged(ProgressContext context, ProgressEventArgs e)
        {
            ProgressChanged?.Invoke(context, e);
        }

        private void StateStart(double min)
        {
            StateReport(min, TimeSpan.Zero);
        }

        private void StateFinish(double max, TimeSpan timeSpan)
        {
            StateReport(max, timeSpan);
        }

        public void Report(double value)
        {
            StateReport(Min + (Max - Min)*value, DateTime.Now - Start);
        }

        private void StateReport(double value, TimeSpan time)
        {
            var values = State.GetStates(Key);
            if (values.Count() >= 2)
            {
                var v = GetY(values, value);
                time = new TimeSpan((time + v).Ticks/2);
            }

            var exact = values.SingleOrDefault(o => Math.Abs(o.Value - value) < 0.01);
            if (exact == null)
            {
                exact = new State
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
            State.OnProgressChanged(this,
                new ProgressEventArgs(new ProgressInfo(value, time, values.Last().Time - time)));
        }

        public static TimeSpan GetY(IEnumerable<State> values, double x)
        {
            var states = values as State[] ?? values.ToArray();
            var point1 = states.First();
            var point2 = states.Last();
            var m = (long) ((point2.Time - point1.Time).Ticks/(point2.Value + point1.Value));
            var b = point1.Time.Ticks - (long) (m*point1.Value);

            return new TimeSpan((long) (m*x + b));
        }

        public ProgressInfo ToLocalProgressInfo(ProgressInfo info)
        {
            if (info.Progress < Min || info.Progress > Max)
            {
                throw new InvalidOperationException("This progress is outside the context");
            }
            var value1 = info.Progress - Min;
            var value2 = value1/(Max - Min);
            return new ProgressInfo(value2, TimeSpan.Zero, TimeSpan.Zero);
        }
    }
}