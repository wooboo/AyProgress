using System;
using System.Threading;

namespace AyProgress
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Progress.ProgressChanged += Progress_ProgressChanged;
            using (Progress.Start("1", 0, 1))
            {
                Thread.Sleep(10);
                Progress.Report(0.1);
                Thread.Sleep(10);
                Progress.Report(0.2);
                Thread.Sleep(10);
                using (Progress.Start("2", 0.3, 0.45))
                {
                    Thread.Sleep(10);
                    Progress.Report(0.33);
                    Thread.Sleep(10);
                    Progress.Report(0.66);
                    Thread.Sleep(10);
                }
                Thread.Sleep(10);
                Progress.Report(0.6);
                Thread.Sleep(10);
                Progress.Report(0.8);
            }
            Console.ReadKey();
        }

        private static void Progress_ProgressChanged(object sender, ProgressEventArgs e)
        {
            var info = e.GlobalProgressInfo;
            var context = (ProgressContext) sender;
            var info2 = context.ToLocalProgressInfo(info);
            Console.WriteLine(
                $"key: {context.Key}\tvalue: {info.Progress:0.00}\ttime: {info.TimeFromStart}\tto finish:{info.TimeToFinish}\tlocal: {info2.Progress:0.00}");
        }
    }
}