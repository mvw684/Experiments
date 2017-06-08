using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using OfficeOpenXml;

namespace supremum {
    internal static class ExistingDataStatistics {

        internal static object Lock = new object();
        internal static int BestItemsToCheck = -1;

        private static Dictionary<int, HashSetInt> allSolutionsCounts = new Dictionary<int, HashSetInt>();

        internal static List<Solution> allSolutions;
        internal static List<Solution> bestSolutions;

        internal static int[] bestMin = new int[Constants.SolutionSize];
        internal static int[] bestMax = new int[Constants.SolutionSize];

        internal static int[] bestWidenedMin = new int[Constants.SolutionSize];
        internal static int[] bestWidenedMax = new int[Constants.SolutionSize];

        internal static int[][] bestValuesPerPosition;
        internal static int[][] badValuesPerPosition;
        internal static int[][] allValuesPerPosition;

        internal static int[] allValuesPlain;
        internal static int[] allValuesByPriority;

        internal static Dictionary<int, int> valueToPriority = new Dictionary<int, int>();
        internal static int totalPriority;

        internal static void Initialize() {
            CondenseAllSolutions();
            //ReadAllSolutions();
            //WriteCombinedExcel();
            //WriteAllSolutions();
            //ComputeNumberStatistics();
            //BestItemsToCheck = allSolutions.Count(a => a.CountAms < Constants.GoodEnough);
            //ComputeMinMaxFromBest();
            //ComputePerPosition();
        }

        private static void WriteAllSolutions() {
            foreach (var solution in allSolutions) {
                solution.Save();
            }
        }

        private static void ComputePerPosition() {
            var byPosition = new HashSetInt[Constants.SolutionSize];
            var bestByPosition = new HashSetInt[Constants.SolutionSize];
            var badByPosition = new HashSetInt[Constants.SolutionSize];
            for (int j = 0; j < Constants.SolutionSize; j++) {
                byPosition[j] = new HashSetInt(2000);
                bestByPosition[j] = new HashSetInt(2000);
                badByPosition[j] = new HashSetInt(2000);
            }
            for(int i = 0; i < allSolutions.Count; i++) {
                var items = allSolutions[i];
                bool best = i < BestItemsToCheck;
                for (int j = 0; j < Constants.SolutionSize; j++) {
                    var value = items[j];
                    byPosition[j].Add(value);
                    if (best) {
                        bestByPosition[j].Add(value);
                    }
                }
            }

            for (int j = 0; j < Constants.SolutionSize; j++) {
                var best = bestByPosition[j];
                var all = byPosition[j];
                var bad = badByPosition[j];
                foreach (var value in all) {
                    if (!best.Contains(value)) {
                        bad.Add(value);
                    }
                }
            }

            allValuesPerPosition = new int[Constants.SolutionSize][];
            bestValuesPerPosition = new int[Constants.SolutionSize][];
            badValuesPerPosition = new int[Constants.SolutionSize][];
            for (int j = 0; j < Constants.SolutionSize; j++) {

                int[] temp = byPosition[j].ToArray();
                Array.Sort(temp);
                allValuesPerPosition[j] = temp;

                temp = bestByPosition[j].ToArray();
                Array.Sort(temp);
                bestValuesPerPosition[j] = temp;

                temp = badByPosition[j].ToArray();
                Array.Sort(temp);
                badValuesPerPosition[j] = temp;
            }
        }

        private static int CompareSolution(Solution a, Solution b) {
            int result = a.CountAms - b.CountAms;
            if (result == 0) {
                result = a.Hash.CompareTo(b.Hash);
            }
            return result;
        }

        private static void CondenseAllSolutions() {
            DirectoryInfo resultDir = new DirectoryInfo(Constants.OutputDir);
            var files = resultDir.EnumerateFiles("Sup.*@*.csv");
            Dictionary<int, List<FileInfo>> grouped = new Dictionary<int, List<FileInfo>>();
            foreach (var file in files) {
                string[] parts = file.Name.Split('.');
                string[] nrs = parts[1].Split('@');
                int nr = int.Parse(nrs[0]);
                List<FileInfo> group;
                if (!grouped.TryGetValue(nr, out group)) {
                    group = new List<FileInfo>();
                    grouped[nr] = group;
                }
                group.Add(file);
            }
            int[] keys = grouped.Keys.ToArray();
            Array.Sort(keys);
            var result = Parallel.ForEach(keys, key => CondenseAllSolutions(key, grouped[key]));
            if (result.IsCompleted) {
                Console.WriteLine("Consolidated");
            } else {
                Console.WriteLine("Consolidated failed");
            }
        }

        private static void CondenseAllSolutions(int key, List<FileInfo> list) {

            var fileData = Solution.GetFileData(key);

            HashSet<Solution> checkForDuplicates = new HashSet<Solution>(SolutionComparer.Instance);
            bool exists = fileData.Exists;
            string header = Solution.header;
            if (fileData.Mutex.WaitOne()) {
                if (exists) {
                    var oldData =Solution.Load(new FileInfo(fileData.FullName));
                    foreach (var solution in oldData) {
                        if (!checkForDuplicates.Contains(solution)) {
                            checkForDuplicates.Add(solution);
                        } else {
                            if (Debugger.IsAttached) {
                                Debugger.Break();
                            }
                        }
                    }
                }

                try {
                    using (var writer = fileData.Open()) {
                        if (!exists) {
                            writer.WriteLine(header);
                        }
                        foreach (var file in list) {
                            var solution = Solution.LoadOld(file);
                            if (!checkForDuplicates.Contains(solution)) {
                                var line = solution.GetData();
                                writer.WriteLine(line);
                                writer.Flush();
                            }
                            // file.Delete();
                        }
                    }
                } finally {
                    fileData.Mutex.ReleaseMutex();
                }
            }
            //int current = 0;
            //int oldPercentage = -1;
            //Console.Write("Reading 0%");
            //foreach (var file in files) {
            //    if (file.Length == 0) {
            //        file.Delete();
            //        continue;
            //    }
            //    var solution = Solution.Load(file);
            //    if (solution != null) {
            //        if (solution.CountAms < Constants.NotGoodEnough) {
            //            allSolutions.Add(solution);
            //            TryAddSolutionCounts(solution);
            //        }
            //        current++;
            //    }
            //    int percentage = (100 * current) / count;
            //    if (percentage != oldPercentage) {
            //        Console.CursorLeft = 8;
            //        Console.Write("{0}%", percentage);
            //        oldPercentage = percentage;
            //    }
            //}
            //Console.WriteLine();
            //allSolutions.Sort(CompareSolution);
            //if (allSolutions.Count > 0) {
            //    Interlocked.Exchange(ref CurrentDataStatistics.bestSolution, allSolutions.First().CountAms);
            //}
        }

        private static void ReadAllSolutions() {
            DirectoryInfo resultDir = new DirectoryInfo(Constants.OutputDir);
            var files = resultDir.GetFiles("Sup*.csv");
            Array.Sort(files, (a,b) => StringComparer.OrdinalIgnoreCase.Compare(a.Name,b.Name));
            int count = files.Length;
            allSolutions = new List<Solution>(count);
            int current = 0;
            int oldPercentage = -1;
            Console.Write("Reading 0%");
            foreach (var file in files) {
                if (file.Length == 0) {
                    file.Delete();
                    continue;
                }
                var solution = Solution.LoadOld(file);
                if (solution != null) {
                    if (solution.CountAms < Constants.NotGoodEnough) {
                        allSolutions.Add(solution);
                        TryAddSolutionCounts(solution);
                    }
                    current++;
                }
                int percentage = (100 * current) / count;
                if (percentage != oldPercentage) {
                    Console.CursorLeft = 8;
                    Console.Write("{0}%", percentage);
                    oldPercentage = percentage;
                }
            }
            Console.WriteLine();
            allSolutions.Sort(CompareSolution);
            if (allSolutions.Count > 0) {
                Interlocked.Exchange(ref CurrentDataStatistics.bestSolution, allSolutions.First().CountAms);
            }
        }

        internal static bool TryAddSolutionCounts(Solution solution) {
            if (solution.CountAms > Constants.NotGoodEnough) {
                return false;
            }
            HashSetInt hashes;
            if ( !allSolutionsCounts.TryGetValue(solution.CountAms, out hashes)) {
                hashes = new HashSetInt(500);
                allSolutionsCounts[solution.CountAms] = hashes;
                hashes.Add(solution.Hash);
                return true;
            }
            if (!hashes.Contains(solution.Hash)) {
                hashes.Add(solution.Hash);
                return true;
            }
            return false;
        }

        private static void WriteCombinedExcel() {
            StringBuilder reportLine = new StringBuilder();
            string fileName = Path.Combine(Constants.OutputDir, "AllSup.xlsx");
            FileInfo file = new FileInfo(fileName);
            if (file.Exists) {
                file.Delete();
                file.Refresh();
            }

            int count = allSolutions.Count;
            int current = 0;
            int oldPercentage = -1;

            using (var package = new ExcelPackage(file)) {
                var sheet = package.Workbook.Worksheets.Add("Supremum");
                var cells = sheet.Cells;
                const int headerRow = 4;
                const int nrsStartColumn = 3;
                cells[headerRow, 1].Value = "Count";
                cells[headerRow, 2].Value = "Hash";
                for (int i = 1; i <= Constants.SolutionSize; i++) {
                    cells[headerRow, i + nrsStartColumn - 1].Value = "Nr" + i;
                }
                int dataRow = 0;
                Console.Write("Creating Excel 0%");
                for (int i = 0; i < allSolutions.Count; i++) {
                    dataRow = headerRow + 1 + i;
                    var solution = allSolutions[i];
                    cells[dataRow, 1].Value = solution.CountAms;
                    cells[dataRow, 2].Value = solution.Hash.ToString("X8", CultureInfo.InvariantCulture);
                    for (int j = 0; j < Constants.SolutionSize; j++) {
                        cells[dataRow, j + nrsStartColumn].Value = solution[j];
                    }
                    current++;
                    int percentage = (100 * current) / count;
                    if (percentage != oldPercentage) {
                        Console.CursorLeft = 15;
                        Console.Write("{0}%", percentage);
                        oldPercentage = percentage;
                    }
                }
                int firstDataRow = headerRow + 1;
                int lastDataRow = dataRow;
                
                for (int i = 1; i <= Constants.SolutionSize; i++) {
                    int column = i + nrsStartColumn - 1;
                    cells[headerRow - 3, column].Formula = "SUBTOTAL(105," + new ExcelAddress(firstDataRow, column, lastDataRow, column) + ")";
                    cells[headerRow - 2, column].Formula = "SUBTOTAL(101," + new ExcelAddress(firstDataRow, column, lastDataRow, column) + ")";
                    cells[headerRow - 1, column].Formula = "SUBTOTAL(104," + new ExcelAddress(firstDataRow, column, lastDataRow, column) + ")";
                }
                sheet.Cells[headerRow, 1, headerRow, 2 + Constants.SolutionSize].AutoFilter = true;
                sheet.Cells[1, 1, lastDataRow, nrsStartColumn + Constants.SolutionSize].Style.Numberformat.Format = "###0";
                sheet.View.FreezePanes(5,3);
                Console.WriteLine();
                Console.WriteLine("Saving excel ...");
                package.Save();
                Console.WriteLine("Saved " + file.FullName);
                Console.WriteLine();
            }
        }

        private static void ComputeNumberStatistics() {
            HashSetInt allValues = new HashSetInt(2000);
            int worst = allSolutions.Last().CountAms;

            for (int i = 0; i < allSolutions.Count; i++) {
                var solution = allSolutions[i];
                int count = solution.CountAms;
                int weight = worst - count;
                for (int j = 0; j < Constants.SolutionSize; j++) {
                    int number = solution[j];
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
            int[] result = new int[Constants.SolutionSize];
            for (int i = 0; i < Constants.SolutionSize; i++) {
                result[i] = InitialValue(i);
            }
            return result;
        }
        
        private static void ComputeMinMaxFromBest() {
            bestSolutions = allSolutions.GetRange(0, Math.Min(BestItemsToCheck, allSolutions.Count));
            for (int j = 0; j < Constants.SolutionSize; j++) {
                bestMin[j] = Int32.MaxValue;
                bestMax[j] = Int32.MinValue;
            }
            for (int i = 0; i < bestSolutions.Count; i++) { 
                var solution = bestSolutions[i];
                for (int j = 0; j < Constants.SolutionSize; j++) {
                    int v = solution[j];
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
            for (int j = 0; j < Constants.SolutionSize; j++) {
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
