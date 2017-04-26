using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace supremum {
    internal static class ExistingDataStatistics {

        internal static HashSet<int> allSolutionsCounts;
        internal static int[][] allBestSolutions;

        internal static int[] Min;
        internal static int[] Max;

        internal static int[] WidenedMin;
        internal static int[] WidenedMax;

        internal static List<int[]> best;

        
        internal static int[] allValues;

        static ExistingDataStatistics() {
            allSolutionsCounts = new HashSet<int>();
            Min = new int[256];
            Max = new int[256];
            WidenedMin = new int[256];
            WidenedMax = new int[256];
            best = ReadBest();
        }

    private static List<int[]> ReadBest() {
            DirectoryInfo resultDir = new DirectoryInfo(Constants.OutputDir);
            var files = resultDir.GetFiles("Sup*.csv");
            List<Tuple<int, FileInfo>> fileWithNumber = new List<Tuple<int, FileInfo>>(files.Length);
            lock (allSolutionsCounts) {
                foreach (var file in files) {
                    string fileName = file.Name;
                    string[] parts = fileName.Split('.');
                    int nrOfValues = Int32.Parse(parts[1]);
                    fileWithNumber.Add(new Tuple<int, FileInfo>(nrOfValues, file));
                    allSolutionsCounts.Add(nrOfValues);
                }
            }
            fileWithNumber.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            ComputeMinMax(fileWithNumber);
            return InitializeBest(fileWithNumber);
        }

        private static List<int[]> InitializeBest(List<Tuple<int, FileInfo>> fileWithNumber) {
            List<int[]> result = new List<int[]>();
            if (fileWithNumber.Count > 0) {
                Interlocked.Exchange(ref CurrentDataStatistics.bestSolution, fileWithNumber[0].Item1);
                for (int i = 0; i < fileWithNumber.Count; i++) {
                    if (fileWithNumber[i].Item1 > 6000) {
                        break;
                    }
                    result.Add(Read(fileWithNumber[i].Item2));
                }

            }
            return result;
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


        private static void ComputeMinMax(List<Tuple<int, FileInfo>> fileWithNumber) {
            if (fileWithNumber.Count == 0) {
                for (int j = 0; j < 256; j++) {
                    Min[j] = 1;
                    Max[j] = 2000;
                }
                return;
            }
            var allValuesTemp = new SortedSet<int>();
            var allBestSolutionsTemp = new SortedSet<int>[256];
            allBestSolutions = new int[256][];
            for (int i = 0; i < 256; i++) {
                allBestSolutionsTemp[i] = new SortedSet<int>();
            }

            int[] values = Read(fileWithNumber[0].Item2);
            for (int j = 0; j < values.Length; j++) {
                int v = values[j];
                Min[j] = v;
                Max[j] = v;
                allBestSolutionsTemp[j].Add(v);
                allValuesTemp.Add(v);
            }
            int lastItem = Math.Min(Constants.BestItemsToCheck, fileWithNumber.Count);
            
            for (int i = 1; i < lastItem; i++) { 
                var file = fileWithNumber[i];
                values = Read(file.Item2);
                for (int j = 0; j < values.Length; j++) {
                    int v = values[j];

                    if (v < Min[j]) {
                        Min[j] = v;
                    }
                    if (v > Max[j]) {
                        Max[j] = v;
                    }
                    allBestSolutionsTemp[j].Add(v);
                    allValuesTemp.Add(v);
                }
            }

            allValues = allValuesTemp.ToArray();

            // widen
            for (int j = 0; j < 256; j++) {
                allBestSolutions[j] = allBestSolutionsTemp[j].ToArray();
                Array.Sort(allBestSolutions[j]);
                WidenedMax[j] = (int)(Max[j] * 1.1);
                WidenedMin[j] = (int)(Min[j] * 0.9);
                if (WidenedMax[j] == 0) {
                    WidenedMax[j] = 1;
                }
                if (WidenedMin[j] == 0) {
                    WidenedMin[j] = 1;
                }
            }


            StringBuilder reportLine = new StringBuilder();
            string fileName = Path.Combine(Constants.OutputDir, "AllSup.csv");
            using (TextWriter writer = new StreamWriter(fileName)) {
                reportLine.Append("Count");
                for(int i = 1; i <=256; i++) {
                    reportLine.Append(", Nr" + i);
                }
                writer.WriteLine(reportLine);
                for (int i = 0; i < fileWithNumber.Count; i++) {
                    reportLine.Clear();
                    var file = fileWithNumber[i];
                    values = Read(file.Item2);
                    reportLine.Append(file.Item1);
                    for (int j = 0; j < values.Length; j++) {
                        reportLine.Append(',');
                        reportLine.Append(values[j].ToString("G", CultureInfo.InvariantCulture));
                    }
                    writer.WriteLine(reportLine);
                }
            }
        }
    }
}
