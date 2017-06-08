using System;
using System.Diagnostics;


namespace supremum {
    static class Program {

        static void Main() {
            try {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.BelowNormal;
                ExistingDataStatistics.Initialize();
                //var optimizer = new OptimizeBestSolution();
                //var optimizer = new IterateSolutions();
                //var optimizer = new ConstructSolutions();
                //CurrentDataStatistics.Report();
                //GC.KeepAlive(optimizer);
            } catch(Exception e) {
                Trace(e);
            }
        }

        private static void Trace(Exception e) {
            if (e != null) {
                Trace(e.InnerException);
                Console.WriteLine();
                Console.WriteLine(e.GetType().Name + " - " + e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine();
            }
        }
    }
}
