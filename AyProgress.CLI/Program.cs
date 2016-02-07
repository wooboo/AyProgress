using System;
using System.Threading;
using AyProgress.ConsoleBar;

namespace AyProgress.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Performing some task... ");
            using (var progress = new ProgressBar<SimpleProgressInfo>())
            using (new DoubleProgressAdapter(progress, "1"))
            using (ProgressScope.Start("1", 0, 1, "Starting"))
            {
                Thread.Sleep(1000);
                ProgressScope.Report(0.1, "One");
                Thread.Sleep(1000);
                ProgressScope.Report(0.2);
                Thread.Sleep(1000);
                using (ProgressScope.Start("2", 0.3, 0.45))
                {
                    Thread.Sleep(1000);
                    ProgressScope.Report(0.33);
                    Thread.Sleep(1000);
                    ProgressScope.Report(0.66, "tu");
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
                ProgressScope.Report(0.6, "Almost");
                Thread.Sleep(1000);
                ProgressScope.Report(0.8);
                Thread.Sleep(1000);
            }
            Console.WriteLine("Done.");

            Console.ReadKey();
        }
    }
}