using System;
using System.Collections.Generic;
using System.Threading;

namespace supremum {
    internal class IterateSolutions {

        static long startPoint = 16538864;
        static long prepared = 0;

        List<Solution> freeList = new List<Solution>();
        List<Solution> toEvaluate = new List<Solution>();

        internal IterateSolutions() {
            string title = "Iterating ... ";
            Console.Title = title;

            for (int i = 0; i < Constants.NrOfCoresToUse; i++) {
                // allow producer some slack to have max overlap, so create a queue > #threads
                freeList.Add(new Solution());
                freeList.Add(new Solution());
                Thread evaluator = new Thread(ParrallelEvaluate);
                evaluator.IsBackground = true;
                evaluator.Name = "Evaluator " + (i + 1);
                evaluator.Start(i);
            }
            Thread iterator = new Thread(Iterate);
            iterator.IsBackground = true;
            iterator.Name = "Iterator";
            iterator.Start();
        }

        private void Iterate() {
            var current = new int[256];
            PrepareSolutionsViaIteration(current,0,0);
        }

        private void PrepareSolutionsViaIteration(int[] current, int previousValue, int currentIndex) {
            if (currentIndex >= 256) {
                if (prepared >= startPoint) {
                    EvaluateSolutionAsync(current);
                }
                prepared++;
            } else {
                foreach (var value in ExistingDataStatistics.allBestValues[currentIndex]) {
                    if (value <= previousValue) {
                        continue;
                    }
                    current[currentIndex] = value;
                    PrepareSolutionsViaIteration(current, value, currentIndex + 1);
                }
                //int b = ExistingDataStatistics.Min[currentIndex];
                //if (b <= previousValue) {
                //    b = previousValue + 1;
                //}
                //int t = ExistingDataStatistics.Max[currentIndex];
                //if (b == t) {
                //    current[currentIndex] = b;
                //    PrepareSolutionsViaIteration(current, b, currentIndex + 1);
                //} else if (b < t) {
                //    for (int value = b; value <= t; value++) {
                //        current[currentIndex] = value;
                //        PrepareSolutionsViaIteration(current, value, currentIndex + 1);
                //    }
                //}
            }
        }

        private void EvaluateSolutionAsync(int[] current) {
            Solution toHandle = null;
            lock (freeList) {
                while (freeList.Count == 0) {
                    Monitor.Wait(freeList);
                }
                toHandle = freeList[0];
                freeList.RemoveAt(0);
            }
            toHandle.Clear();
            for (int i = 0; i < 256; i++) {
                toHandle[i] = current[i];
            }
            toHandle.CheckNrOfValues();
            lock (toEvaluate) {
                toEvaluate.Add(toHandle);
                Monitor.Pulse(toEvaluate);
            }
        }

        private void ParrallelEvaluate(object oindex) {
            int index = (int)oindex;
            int localBest = 10000;

            while (true) {
                Solution toHandle = null;
                lock (toEvaluate) {
                    while (toEvaluate.Count == 0) {
                        Monitor.Wait(toEvaluate);
                    }
                    toHandle = toEvaluate[0];
                    toEvaluate.RemoveAt(0);
                }
                if (toHandle.UpdateCountAms(localBest)) {
                    localBest = toHandle.CountAms;
                    CurrentDataStatistics.localBest[index] = localBest;
                    CurrentDataStatistics.ReportBest(toHandle);
                }
                Interlocked.Increment(ref CurrentDataStatistics.evaluated);
                lock(freeList) {
                    freeList.Add(toHandle);
                    Monitor.Pulse(freeList);
                }
            }
        }
    }
}
