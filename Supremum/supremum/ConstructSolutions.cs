using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace supremum {
    internal class ConstructSolutions {

        //static long startPoint = 0;
        static long prepared = 0;

        List<Solution> freeList = new List<Solution>();
        List<Solution> toEvaluate = new List<Solution>();
        
        internal ConstructSolutions() {
            Console.Title = "Construct Solutions";
            for (int i = 0; i < Constants.NrOfCoresToUse; i++) {
                // allow producer some slack to have max overlap, so create a queue > #threads
                freeList.Add(new Solution());
                freeList.Add(new Solution());
                Thread evaluator = new Thread(ParrallelEvaluate);
                evaluator.IsBackground = true;
                evaluator.Name = "Evaluator " + (i + 1);
                evaluator.Start(i);
            }

            Thread iterator = new Thread(Construct);
            iterator.IsBackground = true;
            iterator.Name = "Constructor";
            iterator.Start();
        }

        private void Construct() {
            var current = new Solution();
            PrepareSolutionsUsingConstruction(current);
        }

        private void PrepareSolutionsUsingConstruction(Solution current) {
            int currentIndex = current.NrOfUniqueValues;
            if (currentIndex >= 256) {
                EvaluateSolutionAsync(current);
                prepared++;
            } else {
                int currentCount = currentIndex;
                if (currentIndex == 0) {
                    current[0] = 1;
                    current[1] = 2;
                } else {
                    for (int i = 0; i < currentIndex && currentCount < 256; i++) {
                        int x = current[i];
                        for (int j = i; j < currentIndex && currentCount < 256; j++) {
                            int y = current[j];
                            int value;
                            if (x != y) {
                                if (x < y) {
                                    value = y - x;
                                } else {
                                    value = x - y;
                                }
                                if (currentCount < 256 && !current.HasValue(value)) {
                                    current[currentCount++] = value;
                                }
                            }
                            value = x + y;
                            if (currentCount < 256 && !current.HasValue(value)) {
                                current[currentCount++] = value;
                            }
                            value = x * y;
                            if (currentCount < 256 && !current.HasValue(value)) {
                                current[currentCount++] = value;
                            }
                        }
                    }
                }
                PrepareSolutionsUsingConstruction(current);
                for (int i =  currentIndex; i < currentCount; i++) {
                    current[i] = 0;
                }
            }
        }

        private void EvaluateSolutionAsync(Solution current) {
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
            int localBest = Int32.MaxValue;

            while (true) {
                Solution toHandle = null;
                lock (toEvaluate) {
                    while (toEvaluate.Count == 0) {
                        Monitor.Wait(toEvaluate);
                    }
                    toHandle = toEvaluate[0];
                    toEvaluate.RemoveAt(0);
                }
                if (toHandle.UpdateCountAms(10000)) {
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
