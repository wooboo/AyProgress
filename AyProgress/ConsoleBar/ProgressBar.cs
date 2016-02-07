// thanks to https://gist.github.com/DanielSWolf/0ab6a96899cc5377bf54 MIT license

using System;
using System.Text;
using System.Threading;

namespace AyProgress.ConsoleBar
{
    public class ProgressBar<T> : IDisposable, IProgress<T>
        where T : IProgressInfo
    {
        private readonly char _empty;
        private readonly char _filled;
        private readonly Func<T, string> _format;
        private readonly int _textLength;
        private readonly string animation;
        private readonly TimeSpan animationInterval = TimeSpan.FromSeconds(1.0/8);
        private readonly int blockCount;

        private readonly Timer timer;
        private int animationIndex;

        private IProgressInfo currentProgress;
        private string currentText = string.Empty;
        private bool disposed;

        public ProgressBar(Func<T, string> format = null, string animation = @"▌▀▐▄", char empty = '░',
            char filled = '█',
            int blockCount = 30, int textLength = 30)
        {
            this.animation = animation;
            _empty = empty;
            _filled = filled;
            this.blockCount = blockCount;
            _textLength = textLength;
            _format = format ?? (o => $"{{text}} {{percent}} {{spinner}}╢{{bar}}╟ {{time}}");
            Console.OutputEncoding = Encoding.Unicode;

            timer = new Timer(TimerHandler);

            // A progress bar is only for temporary display in a console window.
            // If the console output is redirected to a file, draw nothing.
            // Otherwise, we'll end up with a lot of garbage in the target file.
            if (!Console.IsOutputRedirected)
            {
                ResetTimer();
            }
        }

        public void Dispose()
        {
            lock (timer)
            {
                disposed = true;
                var text = FormatProgress((T) currentProgress);
                UpdateText(text);
                Console.WriteLine();
            }
        }

        public void Report(T value)
        {
            Interlocked.Exchange(ref currentProgress, value);
        }

        private void TimerHandler(object state)
        {
            lock (timer)
            {
                if (disposed) return;
                if (currentProgress != null)
                {
                    var text = FormatProgress((T) currentProgress);
                    UpdateText(text);
                }
                ResetTimer();
            }
        }

        private string FormatProgress(T progressInfo)
        {
            var value = progressInfo.Value;
            // Make sure value is in [0..1] range
            value = Math.Max(0, Math.Min(1, value));

            var progressBlockCount = (int) (value*blockCount);
            var template = _format(progressInfo);
            var percent = (int) (value*100);

            var bar =
                $"{new string(_filled, progressBlockCount)}{new string(_empty, blockCount - progressBlockCount)}";
            var spinner = animation[animationIndex++%animation.Length];
            var text = template
                .Replace("{bar}", bar)
                .Replace("{spinner}", spinner.ToString())
                .Replace("{percent}", $"{percent,3}%")
                .Replace("{time}", FormatTimeLeft(progressInfo.TimeToFinish))
                .Replace("{text}", this.FormatText(progressInfo.Text, _textLength));
            return text;
        }

        private static string FormatTimeLeft(TimeSpan span)
        {
            var minutes = (int)Math.Abs(span.TotalMinutes);
            var seconds = Math.Abs(span.Seconds);
            if (span < TimeSpan.Zero)
            {
                return $"{minutes,3}:{seconds:00} overdue";
            }
            return $"{minutes,3}:{seconds:00} left";

        }

        private string FormatText(string text, int length)
        {
            return (text + "                                          ").Substring(0, length);
        }

        private void UpdateText(string text)
        {
            // Get length of common portion
            var commonPrefixLength = 0;
            var commonLength = Math.Min(currentText.Length, text.Length);
            while (commonPrefixLength < commonLength && text[commonPrefixLength] == currentText[commonPrefixLength])
            {
                commonPrefixLength++;
            }

            // Backtrack to the first differing character
            var outputBuilder = new StringBuilder();
            outputBuilder.Append('\b', currentText.Length - commonPrefixLength);

            // Output new suffix
            outputBuilder.Append(text.Substring(commonPrefixLength));

            // If the new text is shorter than the old one: delete overlapping characters
            var overlapCount = currentText.Length - text.Length;
            if (overlapCount > 0)
            {
                outputBuilder.Append(' ', overlapCount);
                outputBuilder.Append('\b', overlapCount);
            }

            Console.Write(outputBuilder);
            currentText = text;
        }

        private void ResetTimer()
        {
            timer.Change(animationInterval, TimeSpan.FromMilliseconds(-1));
        }
    }
}