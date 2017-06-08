using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace supremum {
    internal class IterateSolutions {

        static long startPoint = 1184070;
        static long prepared = 0;

        Queue<Solution> freeList = new Queue<Solution>(Constants.NrOfCoresToUse * 8);
        Queue<Solution> toEvaluate = new Queue<Solution>(Constants.NrOfCoresToUse * 4);
        
        internal IterateSolutions() {
            
            BigInteger count = 1;
            for(int i = 0; i < Constants.SolutionSize; i++) {
                count *= ExistingDataStatistics.bestValuesPerPosition[i].Length;
            }

            string title = "Iterating " + count.ToString("G") + " ... ";
            Console.Title = title;

            // fill free list with amount that can be consumed
            // let consumers dictate the pace
            while(freeList.Count < Constants.NrOfCoresToUse * 4) {
                freeList.Enqueue(new Solution());
            }
            for (int i = 0; i < Constants.NrOfCoresToUse; i++) {
                Thread evaluator = 
                    new Thread(ParrallelEvaluate) {
                        IsBackground = true,
                        Name = "Evaluator " + (i + 1)
                    };
                evaluator.Start(i);
            }
            Thread iterator = new Thread(Iterate);
            iterator.IsBackground = true;
            iterator.Name = "Iterator";
            iterator.Start();
        }

        private void Iterate() {
            var current = new int[Constants.SolutionSize];
            PrepareSolutionsViaIteration(current,0,0);
        }

        private void PrepareSolutionsViaIteration(int[] current, int previousValue, int currentIndex) {
            if (currentIndex >= Constants.SolutionSize) {
                if (prepared >= startPoint) {
                    EvaluateSolutionAsync(current);
                } else {
                    Interlocked.Increment(ref CurrentDataStatistics.evaluated);
                }
                prepared++;
            } else {
                var values = ExistingDataStatistics.bestValuesPerPosition[currentIndex];
                for(int index = values.Length-1; index >= 0; index--) {
                    int value = values[index];
                    if (value <= previousValue) {
                        break;
                    }
                    current[currentIndex] = value;
                    PrepareSolutionsViaIteration(current, value, currentIndex + 1);
                }
            }
        }

        private void EvaluateSolutionAsync(int[] current) {
            Solution toHandle; 
            lock(freeList) {
                if (freeList.Count == 0) {
                    Monitor.Wait(freeList);
                }
                toHandle = freeList.Dequeue();
            }
            toHandle.Clear();
            for (int i = 0; i < Constants.SolutionSize; i++) {
                toHandle[i] = current[i];
            }
            toHandle.CheckNrOfValues();
            lock (toEvaluate) {
                toEvaluate.Enqueue(toHandle);
                Monitor.Pulse(toEvaluate);
            }
        }

        private void ParrallelEvaluate(object oindex) {
            int index = (int)oindex;
            const int MaxSolution = 7000;
            int localBest = MaxSolution;
            HashSetInt solutionsHelper = new HashSetInt(10000);
            while (true) {
                Solution toHandle;
                lock(toEvaluate) {
                    if (toEvaluate.Count == 0) {
                        Monitor.Wait(toEvaluate);
                    }
                    toHandle = toEvaluate.Dequeue();
                }
                if (toHandle.UpdateCountAms(10000, solutionsHelper)) {
                    localBest = toHandle.CountAms;
                    CurrentDataStatistics.ReportBest(toHandle);
                    CurrentDataStatistics.localBest[index] = localBest;

                }
                Interlocked.Increment(ref CurrentDataStatistics.evaluated);
                lock (freeList) {
                    freeList.Enqueue(toHandle);
                    Monitor.Pulse(freeList);
                }
            }
        }
    }
}
