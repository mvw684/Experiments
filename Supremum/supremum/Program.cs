using System.Diagnostics;


namespace supremum {
    static class Program {

        static void Main() {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.BelowNormal;
            var optimizer = new OptimizeBestSolution();
            //var optimize = new IterateSolutions();
            //var optimize = new ConstructSolutions();
            CurrentDataStatistics.Report();
        }
    }
}
