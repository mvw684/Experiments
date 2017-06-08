using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace supremum {

    internal class SolutionComparer : IEqualityComparer<Solution> {

        internal static readonly SolutionComparer Instance = new SolutionComparer();

        public bool Equals(Solution x, Solution y) {
            if (ReferenceEquals(x,y)) {
                return true;
            }
            if (x.Hash != y.Hash) {
                return false;
            }
            x.Sort();
            y.Sort();
            for(int i = 0; i < Constants.SolutionSize; i++) {
                if (x[i] != y[i]) {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(Solution obj) {
            return obj.Hash;
        }
    }
    internal class Solution {

        internal static readonly string header = CreateHeader();

        private static string CreateHeader() {
            StringBuilder h = new StringBuilder(1000);
            h.Append("CountAms");
            for (int i = 1; i <= Constants.SolutionSize; i++) {
                h.Append(",V");
                h.Append(i);
            }
            return h.ToString();
        }

        internal class FileData {
            private static Dictionary<int, FileData> fileDataCache = new Dictionary<int, FileData>();

            internal static FileData Get(int countAms) {
                FileData result;
                lock (fileDataCache) {
                    if (!fileDataCache.TryGetValue(countAms, out result)) {
                        result = new FileData(countAms);
                        fileDataCache[countAms] = result;
                    }
                }
                return result;
            }

            internal readonly int CountAms;
            internal readonly string FileName;
            internal readonly string FullName;
            internal readonly Mutex Mutex;
            private bool exists;

            private FileData(int countAms) {
                CountAms = countAms;
                bool newMutex;
                FileName = "Sup." + countAms + ".combined.csv";
                Mutex = new Mutex(false, FileName, out newMutex, null);
                FullName = Path.Combine(Constants.OutputDir, FileName);
                exists = File.Exists(FullName);
            }


            internal bool Exists {
                get {
                    return exists;
                }
            }

            internal TextWriter Open() {
                var result = new StreamWriter(FullName, true);
                exists = true;
                return result;
            }
        }


        internal static FileData GetFileData(int countAms) {
            return FileData.Get(countAms);
        }

        private ushort[] positionValues = new ushort[Constants.SolutionSize];
        private bool[] uniqueValues = new bool[2001];
        private int count;
        private int hash;
        private int countAms;
        bool sorted;


        internal int CountAms {
            get {
                return countAms;
            }
        }

        internal void Sort() {
            if (!sorted) {
                Array.Sort(positionValues);
                sorted = true;
            }
        }

        public override string ToString() {
            return "Sup " + CountAms + "@" + Hash.ToString("X8");
        }

        internal void Save() {
            if (countAms > 0) {
                Sort();
                var fileData = GetFileData(countAms);
                var line = GetData();
                
                if (fileData.Mutex.WaitOne()) {
                    try {
                        bool exists = fileData.Exists;
                        using (var writer = fileData.Open()) {
                            if (!exists) {
                                writer.WriteLine(header);
                            }
                            writer.WriteLine(line);
                        }
                    } finally {
                        fileData.Mutex.ReleaseMutex();
                    }
                }
            }
        }

        /// <summary>
        /// Get a string count,v1,v2,..,v256
        /// </summary>
        internal string GetData() {
            StringBuilder lineBuilder = new StringBuilder();
            lineBuilder.Append(CountAms.ToString("G", CultureInfo.InvariantCulture));
            for (int i = 0; i < positionValues.Length; i++) {
                lineBuilder.Append(',');
                lineBuilder.Append(positionValues[i].ToString("G", CultureInfo.InvariantCulture));
            }
            string line = lineBuilder.ToString();
            return line;
        }

        internal static Solution LoadOld(FileInfo file) {
            var result = new Solution();
            int countAms;
            int[] numbers = ReadOld(file, out countAms);
            if (numbers == null) {
                return null;
            }
            for (int i = 0; i < numbers.Length; i++) {
                result[i] = numbers[i];
            }
            result.countAms = countAms;
            return result;
        }

        public static ReadOnlyCollection<Solution> Load(FileInfo file) {
            throw new NotImplementedException();xxx

        }
        private static int[] ReadOld(FileInfo file, out int countAms) {
            try {
                using (var reader = new StreamReader(file.FullName)) {
                    var items = reader.ReadLine();
                    countAms = items.Split(',').Skip(1).Select(a => Int32.Parse(a.Trim())).FirstOrDefault();
                    items = reader.ReadLine();
                    var parts = items.Split(',').Skip(1);
                    int[] result = parts.Select(a => Int32.Parse(a.Trim())).ToArray();
                    if (result.Length != Constants.SolutionSize) {
                        Console.WriteLine("Incorrect file: " + file.FullName);
                        countAms = 0;
                        return null;
                    }
                    return result;
                }
            } catch (Exception) {
                Console.WriteLine("Incorrect file: " + file.FullName);
                countAms = 0;
                return null;
            }
        }

        /// <summary>
        /// Update the count of combinations of multiplication, addition and subtraction
        /// </summary>
        internal bool UpdateCountAms(int best, HashSetInt solutionsHelper) {
            CheckNrOfValues();
            countAms = 0;
            HashSetInt solutions = solutionsHelper;
            solutions.Clear();
            ushort[] input = positionValues;
            for (int i = 0; i < input.Length; i++) {
                int x = input[i];
                for (int j = i; j < input.Length; j++) {
                    int y = input[j];
                    if (x < y) {
                        solutions.Add(y - x);
                    } else {
                        solutions.Add(x - y);
                    }
                    solutions.Add(x + y);
                    solutions.Add(x * y);
                    if (solutions.Count >= best) {
                        return false;
                    }
                }
            }
            countAms = solutions.Count;
            return true;
        }
        
        internal int Hash {
            get {
                if (hash == 0) {
                    Sort();
                    unchecked {
                        uint result = 0;
                        for (int i = 0; i < Constants.SolutionSize; i++) {
                            result += (uint)i * positionValues[i];
                        }
                        if (result > 131072000) {
                            Debugger.Break();
                        }
                        hash = (int)result;
                    }
                }
                return hash;
            }
        }

        public override int GetHashCode() {
            return Hash.GetHashCode();
        }

        public int this[int position] {
            get {
                return positionValues[position];
            }
            set {
                hash = 0;
                sorted = false;
                int oldValue = positionValues[position];
                if (oldValue > 0) {
                    uniqueValues[oldValue] = false;
                    count--;
                }
                if (value > 0) {
                    positionValues[position] = unchecked((ushort)value);
                    uniqueValues[value] = true;
                    count++;
                }
            }
        }

        internal void Clear() {
            sorted = false;
            hash = 0;
            Array.Clear(positionValues, 0, Constants.SolutionSize);
            Array.Clear(uniqueValues, 0, 2001);
            count = 0;
        }

        internal void CheckNrOfValues() {
            if (count != Constants.SolutionSize) {
                InvariantViolated("Unique values is " + count);
            }
        }

        internal int NrOfUniqueValues {
            get {
                return count;
            }
        }

        public ushort[] Data {
            get {
                return positionValues;
            }
        }

        private void InvariantViolated(string message) {
            throw new InvalidOperationException("Invariant Violated: " + message);
        }

        public bool HasValue(int value) {
            return uniqueValues[value];
        }
    }
}
