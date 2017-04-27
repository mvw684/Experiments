using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace supremum {
    internal static class ExistingDataStatistics {

        internal static int BestItemsToCheck = 50;

        internal static HashSet<int> allSolutionsCounts = new HashSet<int>();
        internal static List<Tuple<int, int[]>> allSolutions = new List<Tuple<int, int[]>>(5000);

        internal static List<Tuple<int, int[]>> bestSolutions = new List<Tuple<int, int[]>>(BestItemsToCheck);
        
        internal static int[] bestMin = new int[256];
        internal static int[] bestMax = new int[256];

        internal static int[] bestWidenedMin = new int[256];
        internal static int[] bestWidenedMax = new int[256];

        internal static int[][] bestValuesPerPosition;
        internal static int[][] allValuesPerPosition;


        internal static int[] allValuesPlain;
        internal static int[] allValuesByPriority;

        internal static Dictionary<int, int> valueToPriority = new Dictionary<int, int>();
        internal static int totalPriority;

        static ExistingDataStatistics() {
            ReadAllSolutions();
            WriteCombinedCsv();
            ComputeNumberStatistics();
            ComputeMinMaxFromBest();
            ComputePerPosition();
        }

        private static void ComputePerPosition() {
            var byPosition = new HashSet<int>[256];
            var bestByPosition = new HashSet<int>[256];
            for(int j = 0; j < 256; j++) {
                byPosition[j] = new HashSet<int>();
                bestByPosition[j] = new HashSet<int>();
            }
            for(int i = 0; i < allSolutions.Count; i++) {
                var items = allSolutions[i].Item2;
                bool best = i < BestItemsToCheck;
                for (int j = 0; j < 256; j++) {
                    var value = items[j];
                    byPosition[j].Add(value);
                    if (best) {
                        bestByPosition[j].Add(value);
                    }
                }
            }
            allValuesPerPosition = new int[256][];
            bestValuesPerPosition = new int[256][];
            for (int j = 0; j < 256; j++) {
                int[] temp = byPosition[j].ToArray();
                Array.Sort(temp);
                allValuesPerPosition[j] = temp;

                temp = bestByPosition[j].ToArray();
                Array.Sort(temp);
                bestValuesPerPosition[j] = temp;
            }
        }

        private static void ReadAllSolutions() {
            DirectoryInfo resultDir = new DirectoryInfo(Constants.OutputDir);
            var files = resultDir.GetFiles("Sup*.csv");
            foreach (var file in files) {
                string fileName = file.Name;
                string[] parts = fileName.Split('.');
                int nrOfValues = Int32.Parse(parts[1]);
                allSolutionsCounts.Add(nrOfValues);
                int[] solution = Read(file);
                allSolutions.Add(new Tuple<int, int[]>(nrOfValues, solution));
            }
            allSolutions.Sort((a,b) => a.Item1.CompareTo(b.Item1));
            if (allSolutions.Count > 0) {
                Interlocked.Exchange(ref CurrentDataStatistics.bestSolution, allSolutions.First().Item1);
            }
        }

        private static void WriteCombinedCsv() {
            StringBuilder reportLine = new StringBuilder();
            string fileName = Path.Combine(Constants.OutputDir, "AllSup.csv");
            using (TextWriter writer = new StreamWriter(fileName)) {
                reportLine.Append("Count");
                for (int i = 1; i <= 256; i++) {
                    reportLine.Append(", Nr" + i);
                }
                writer.WriteLine(reportLine);
                for (int i = 0; i < allSolutions.Count; i++) {
                    reportLine.Clear();
                    var solution = allSolutions[i];
                    reportLine.Append(solution.Item1);
                    for (int j = 0; j < 256; j++) {
                        reportLine.Append(',');
                        reportLine.Append(solution.Item2[j].ToString("G", CultureInfo.InvariantCulture));
                    }
                    writer.WriteLine(reportLine);
                }
            }
        }

        private static void ComputeNumberStatistics() {
            HashSet<int> allValues = new HashSet<int>();
            int worst = allSolutions.Last().Item1;

            for (int i = 0; i < allSolutions.Count; i++) {
                var solution = allSolutions[i];
                int count = solution.Item1;
                int weight = worst - count;
                int[] numbers = solution.Item2;
                for (int j = 0; j < 256; j++) {
                    int number = numbers[j];
                    allValues.Add(number);
                    int oldWeight;
                    valueToPriority.TryGetValue(number, out oldWeight);
                    oldWeight += weight;
                    valueToPriority[number] = oldWeight;
                }
            }
            allValuesPlain = allValues.ToArray();
            allValuesByPriority = (int[])allValuesPlain.Clone();

            var weights = valueToPriority.Values;
            int min = weights.Min();
            int max = weights.Max();
            max -= min;
            var keys = valueToPriority.Keys.ToList();
            for (int i = 0; i < keys.Count; i++) {
                int key = keys[i];
                int weight = valueToPriority[key];
                weight -= min;
                weight = (int)((weight * 1000.0) / max);
                valueToPriority[key] = weight;
            }

            weights = valueToPriority.Values;
            min = weights.Min();
            max = weights.Max();

            Array.Sort(allValuesPlain);
            Array.Sort(
                allValuesByPriority,
                (a, b) => valueToPriority[a].CompareTo(valueToPriority[b])
            );
            totalPriority = valueToPriority.Values.Sum();
        }
        
        
        
        /// <summary>
        /// Initial value based on an appoximate curve
        /// </summary>
        internal static int InitialValue(double x) {
            double y = 9.0e-11 * Math.Pow(x, 6) - 6.0e-8 * Math.Pow(x, 5) + 2.0e-5 * Math.Pow(x, 4) + 0.1204 * x * x - 1.6623 * x + 14.129;
            return (int)Math.Round(y, 0, MidpointRounding.AwayFromZero);
        }

        private static int[] InitialValue() {
            int[] result = new int[256];
            for (int i = 0; i < 256; i++) {
                result[i] = InitialValue(i);
            }
            return result;
        }


        private static int[] Read(FileInfo file) {
            using (var reader = new StreamReader(file.FullName)) {
                reader.ReadLine();
                var items = reader.ReadLine();
                var parts = items.Split(',').Skip(1);
                int[] result = parts.Select(a => Int32.Parse(a.Trim())).ToArray();
                return result;
            }
        }


        private static void ComputeMinMaxFromBest() {
            bestSolutions = allSolutions.GetRange(0, Math.Min(BestItemsToCheck, allSolutions.Count));
            for (int j = 0; j < 256; j++) {
                bestMin[j] = Int32.MaxValue;
                bestMax[j] = Int32.MinValue;
            }
            for (int i = 0; i < bestSolutions.Count; i++) { 
                var solution = bestSolutions[i];
                var values = solution.Item2;
                for (int j = 0; j < values.Length; j++) {
                    int v = values[j];
                    if (v < bestMin[j]) {
                        bestMin[j] = v;
                    }
                    if (v > bestMax[j]) {
                        bestMax[j] = v;
                    }
                }
            }

            // widen
            const double factor = 0.2d;
            for (int j = 0; j < 256; j++) {
                bestWidenedMax[j] = (int)(bestMax[j] * (1+factor));
                bestWidenedMin[j] = (int)(bestMin[j] * (1-factor));
                if (bestWidenedMax[j] == 0) {
                    bestWidenedMax[j] = 1;
                }
                if (bestWidenedMin[j] == 0) {
                    bestWidenedMin[j] = 1;
                }
            }
        }
    }
}
