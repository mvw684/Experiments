﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace supremum {
    internal class Solution {

        private int[] positionValues = new int[256];
        private HashSet<int> uniqueValues = new HashSet<int>();
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
                uniqueValues.Remove(oldValue);
                positionValues[position] = value;
                if (value != 0) {
                    uniqueValues.Add(value);
                }
            }
        }

        internal void Clear() {
            Array.Clear(positionValues,0,256);
            uniqueValues.Clear();
        }

        internal void CheckNrOfValues() {
            if (uniqueValues.Count != 256) {
                InvariantViolated("Unique values is " + uniqueValues.Count);
            }
        }

        internal int NrOfUniqueValues {
            get {
                return uniqueValues.Count;
            }
            
        }

        private void InvariantViolated(string message) {
            throw new InvalidOperationException("Invariant Violated: " + message);
        }

        public bool HasValue(int value) {
            return uniqueValues.Contains(value);
        }
    }
}
