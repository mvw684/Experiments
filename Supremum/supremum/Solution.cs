using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace supremum {
    internal class Solution {

        private int[] positionValues = new int[256];
        private bool[] uniqueValues = new bool[2001];
        private int count;

        private HashSet<int> solutionsHelper = new HashSet<int>();
        private int countAms = 0;


        internal int CountAms {
            get {
                return countAms;
            }
        }

        internal void Sort() {
            Array.Sort(positionValues);
        }

        internal void Save() {
            Array.Sort(positionValues);
            if (countAms > 0) {
                StringBuilder report = new StringBuilder();
                report.Append("Nr of Add/Mul/Sub results,");
                report.Append(countAms);
                report.AppendLine();
                report.Append("Values,");
                for (int i = 0; i < positionValues.Length; i++) {
                    if (i > 0) {
                        report.Append(',');
                    }
                    report.Append(positionValues[i].ToString("G", CultureInfo.InvariantCulture));
                }
                report.AppendLine();
                string value = report.ToString();
                string fileName = Path.Combine(Constants.OutputDir, "Sup." + countAms + ".csv");
                using (TextWriter writer = new StreamWriter(fileName)) {
                    writer.Write(value);
                }
                // Process.Start(fileName);
            }
        }

        /// <summary>
        /// Update the count of combinations of multiplication, addition and subtraction
        /// </summary>
        internal bool UpdateCountAms(int best) {
            CheckNrOfValues();
            countAms = 0;
            HashSet<int> solutions = solutionsHelper;
            solutions.Clear();
            int[] input = positionValues;
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

        public int this[int position] {
            get {
                return positionValues[position];
            }
            set {
                int oldValue = positionValues[position];
                if (oldValue != 0) {
                    uniqueValues[oldValue] = false;
                    count--;
                }
                positionValues[position] = value;
                if (value != 0) {
                    uniqueValues[value] = true;
                    count++;
                }
            }
        }

        internal void Clear() {
            Array.Clear(positionValues,0,256);
            Array.Clear(uniqueValues,0,2001);
            count = 0;
        }

        internal void CheckNrOfValues() {
            if (count != 256) {
                InvariantViolated("Unique values is " + count);
            }
        }

        internal int NrOfUniqueValues {
            get {
                return count;
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
