using System;
using System.Threading;

namespace AyProgress
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Progress.ProgressReported += Progress_ProgressReported;
            using (Progress.Start("1", 0, 1))
            {
                Thread.Sleep(1000);
                Progress.Report(0.1);
                Thread.Sleep(1000);
                Progress.Report(0.2);
                Thread.Sleep(1000);
                using (Progress.Start("2", 0.3, 0.45))
                {
                    Thread.Sleep(1000);
                    Progress.Report(0.33);
                    Thread.Sleep(1000);
                    Progress.Report(0.66);
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
                Progress.Report(0.6);
                Thread.Sleep(1000);
                Progress.Report(0.8);
            }
            Progress.ProgressReported -= Progress_ProgressReported;
            Console.ReadKey();
        }

        private static void Progress_ProgressReported(object sender, ProgressReportedEventArgs e)
        {
            if (e.Key == "1")
            {
                var context = (ProgressContext) sender;

                Console.WriteLine(
                    $"key: {context.Key}\tvalue: {e.Value:0.000}\tto finish:{e.TimeToFinish}");
            }
        }
    }
}