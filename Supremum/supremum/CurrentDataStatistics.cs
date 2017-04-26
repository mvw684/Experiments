using System;
using System.Linq;
using System.Threading;


namespace supremum {
    internal static class CurrentDataStatistics {

        internal static long bestSolution;
        internal static long evaluated;
        internal static long improvements;
        internal static long[] localBest = new long[Constants.NrOfCoresToUse];
        internal static bool running = true;

        internal static void Report() {
            string title = Console.Title;
            DateTime start = DateTime.UtcNow;
            long oldEvaluated = 0;
            while (running) {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                DateTime now = DateTime.UtcNow;
                TimeSpan elapsed = now - start;
                long newBest =  Interlocked.Read(ref bestSolution);
                long newEvaluated = Interlocked.Read(ref evaluated);
                long newImprovements = Interlocked.Read(ref improvements);
                string message =
                    string.Format("{0:dd\\.hh\\:mm\\:ss}", elapsed) +
                    " Evaluated: " + newEvaluated.ToString("#,##0") + ", Delta: " + (newEvaluated - oldEvaluated).ToString("#,##0") +
                    ", Improvements: " + newImprovements.ToString("#,##0") +
                    ", Best: " + newBest.ToString("#,##0") + " --  Locals: " + localBest.Sum().ToString("#,##0") + " - " + String.Join("/", localBest.Select(a => a.ToString("#,##0")));
                oldEvaluated = newEvaluated;
                Console.WriteLine(message);
                Console.Title = title + " -- " + newEvaluated.ToString("#,##0") + " -- " + newBest.ToString("#,##0");
            }
        }


        internal static void ReportBest(Solution solution) {
            var allBests = ExistingDataStatistics.allSolutionsCounts;
            lock (allBests) {
                if (1 <= solution.CountAms) {
                    if (!allBests.Contains(solution.CountAms) && solution.CountAms < 10000) {
                        solution.Save();
                        allBests.Add(solution.CountAms);
                    }
                    if ((solution.CountAms < bestSolution) || (bestSolution == 0)) {
                        Interlocked.Increment(ref improvements);
                        Interlocked.Exchange(ref bestSolution, solution.CountAms);
                        Console.WriteLine("New best: " + solution.CountAms.ToString("#,##0"));
                    }
                }
            }
        }
    }
}
