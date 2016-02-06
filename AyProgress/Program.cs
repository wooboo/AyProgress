using System;
using System.Threading;

namespace AyProgress
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ProgressScope.ProgressReported += Progress_ProgressReported;
            using (ProgressScope.Start("1", 0, 1))
            {
                Thread.Sleep(1000);
                ProgressScope.Report(0.1);
                Thread.Sleep(1000);
                ProgressScope.Report(0.2);
                Thread.Sleep(1000);
                using (ProgressScope.Start("2", 0.1, 0.7))
                {
                    Thread.Sleep(1000);
                    ProgressScope.Report(0.33);
                    Thread.Sleep(1000);
                    ProgressScope.Report(0.66);
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
                ProgressScope.Report(0.6);
                Thread.Sleep(1000);
                ProgressScope.Report(0.8);
            }
            ProgressScope.ProgressReported -= Progress_ProgressReported;
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