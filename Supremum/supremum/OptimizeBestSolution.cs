using System;
using System.Threading;

namespace supremum {
    internal class OptimizeBestSolution {
        
        //int[] neverForgetti = { 1, 2, 64, 80, 96, 100, 112, 120, 125, 128, 140, 144, 150, 160, 168, 175, 176, 180, 192, 196, 200, 208, 210, 216, 220, 224, 225, 240, 245, 250, 252, 256, 260, 264, 270, 275, 280, 288, 294, 300, 304, 308, 312, 315, 320, 324, 325, 330, 336, 343, 344, 350, 352, 360, 364, 375, 378, 380, 384, 385, 390, 392, 396, 400, 405, 416, 420, 430, 432, 440, 441, 448, 450, 455, 456, 462, 468, 480, 484, 486, 490, 495, 500, 504, 512, 516, 520, 525, 528, 532, 539, 540, 546, 550, 560, 567, 572, 576, 585, 588, 594, 600, 602, 605, 608, 616, 624, 625, 630, 637, 640, 645, 648, 650, 660, 672, 675, 684, 686, 688, 693, 700, 702, 704, 715, 720, 726, 728, 729, 735, 750, 756, 760, 768, 770, 774, 780, 784, 792, 800, 810, 819, 825, 832, 840, 847, 858, 860, 864, 875, 880, 882, 891, 896, 900, 903, 910, 912, 924, 936, 945, 960, 968, 972, 975, 980, 990, 1000, 1001, 1008, 1024, 1029, 1032, 1040, 1050, 1053, 1056, 1064, 1078, 1080, 1089, 1092, 1100, 1120, 1125, 1134, 1140, 1144, 1152, 1155, 1161, 1170, 1176, 1188, 1200, 1204, 1210, 1215, 1216, 1225, 1232, 1248, 1250, 1260, 1274, 1280, 1287, 1290, 1296, 1300, 1320, 1323, 1344, 1350, 1365, 1368, 1372, 1376, 1386, 1400, 1404, 1408, 1430, 1440, 1452, 1456, 1458, 1470, 1485, 1500, 1512, 1520, 1536, 1540, 1548, 1560, 1568, 1584, 1600, 1620, 1638, 1650, 1664, 1680, 1716, 1728, 1760, 1764, 1782, 1800, 1824, 1848, 1872, 1920, 1944, 1980 };
        int[] neverForgetti = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 140, 144, 150, 160, 168, 175, 176, 180, 192, 196, 200, 208, 210, 216, 220, 224, 225, 240, 245, 250, 252, 256, 260, 264, 270, 275, 280, 288, 294, 300, 304, 308, 312, 315, 320, 324, 325, 330, 336, 343, 344, 350, 352, 360, 364, 375, 378, 380, 384, 385, 390, 392, 396, 400, 405, 416, 420, 430, 432, 440, 441, 448, 450, 455, 456, 462, 468, 480, 484, 486, 490, 495, 500, 504, 512, 516, 520, 525, 528, 532, 539, 540, 546, 550, 560, 567, 572, 576, 585, 588, 594, 600, 602, 605, 608, 616, 624, 625, 630, 637, 640, 645, 648, 650, 660, 672, 675, 684, 686, 688, 693, 700, 702, 704, 715, 720, 726, 728, 729, 735, 750, 756, 760, 768, 770, 774, 780, 784, 792, 800, 810, 819, 825, 832, 840, 847, 858, 860, 864, 875, 880, 882, 891, 896, 900, 903, 910, 912, 924, 936, 945, 960, 968, 972, 975, 980, 990, 1000, 1001, 1008, 1024, 1029, 1032, 1040, 1050, 1053, 1056, 1064, 1078, 1080, 1089, 1092, 1100, 1120, 1125, 1134, 1140, 1144, 1152, 1155, 1161, 1170, 1176, 1188, 1200, 1204, 1210, 1215, 1216, 1225, 1232, 1248, 1250, 1260, 1274, 1280, 1287, 1290, 1296, 1300, 1320, 1323, 1344, 1350, 1365, 1368, 1372, 1376, 1386, 1400, 1404, 1408, 1430, 1440, 1452, 1456, 1458, 1470, 1485, 1500, 1512, 1520, 1536, 1540, 1548, 1560, 1568, 1584, 1600, 1620, 1638, 1650, 1664, 1680, 1716, 1728, 1760, 1764, 1782, 1800, 1824, 1848, 1872, 1920, 1944, 1980 };
        // int[] bestNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 20, 21, 22, 24, 25, 26, 27, 28, 30, 32, 33, 34, 35, 36, 38, 39, 40, 42, 44, 45, 48, 49, 50, 51, 52, 54, 55, 56, 60, 63, 64, 65, 66, 68, 70, 72, 75, 77, 78, 80, 81, 84, 85, 88, 90, 91, 96, 98, 99, 100, 102, 104, 105, 108, 110, 112, 117, 119, 120, 125, 126, 128, 130, 132, 135, 136, 140, 144, 147, 150, 153, 154, 156, 160, 162, 165, 168, 170, 175, 176, 180, 182, 189, 192, 195, 196, 198, 200, 204, 208, 210, 216, 220, 224, 225, 231, 234, 238, 240, 243, 245, 250, 252, 255, 256, 260, 264, 270, 272, 273, 275, 280, 288, 294, 297, 300, 306, 308, 312, 315, 320, 324, 330, 336, 340, 350, 351, 352, 357, 360, 364, 375, 378, 384, 385, 390, 392, 396, 400, 405, 408, 416, 420, 432, 440, 441, 448, 450, 459, 462, 468, 476, 480, 486, 490, 495, 500, 504, 510, 512, 520, 525, 528, 540, 544, 546, 550, 560, 567, 576, 585, 588, 594, 600, 612, 616, 624, 630, 640, 648, 660, 672, 675, 680, 693, 700, 702, 704, 714, 720, 728, 735, 750, 756, 768, 770, 780, 784, 792, 800, 810, 816, 832, 840, 864, 880, 882, 896, 900, 924, 936, 945, 960, 972, 980, 990, 1000, 1008, 1020, 1040, 1050, 1056, 1080, 1092, 1120, 1134, 1152, 1176, 1188, 1200, 1224, 1248, 1260, 1296, 1320, 1344, 1440, 1512 };
        int[] coolArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 21, 22, 24, 25, 26, 27, 28, 30, 32, 33, 35, 36, 39, 40, 42, 44, 45, 48, 49, 50, 52, 54, 55, 56, 60, 63, 64, 65, 66, 70, 72, 75, 77, 78, 80, 81, 84, 88, 90, 91, 96, 98, 99, 100, 104, 105, 108, 110, 112, 117, 120, 121, 125, 126, 128, 130, 132, 135, 140, 143, 144, 147, 150, 154, 156, 160, 162, 165, 168, 169, 175, 176, 180, 182, 189, 192, 195, 196, 198, 200, 208, 210, 216, 220, 224, 225, 231, 234, 240, 242, 243, 245, 250, 252, 256, 260, 264, 270, 273, 275, 280, 286, 288, 294, 297, 300, 308, 312, 315, 320, 324, 325, 330, 336, 338, 343, 350, 351, 352, 360, 363, 364, 375, 378, 384, 385, 390, 392, 396, 400, 405, 416, 420, 429, 432, 440, 441, 448, 450, 455, 462, 468, 480, 484, 486, 490, 495, 500, 504, 507, 512, 520, 525, 528, 539, 540, 546, 550, 560, 567, 572, 576, 585, 588, 594, 600, 605, 616, 624, 630, 637, 640, 648, 650, 660, 672, 675, 676, 686, 693, 700, 702, 704, 715, 720, 726, 728, 729, 735, 750, 756, 768, 770, 780, 784, 792, 800, 810, 819, 825, 832, 840, 858, 864, 880, 882, 891, 896, 900, 910, 924, 936, 945, 960, 972, 980, 990, 1000, 1008, 1024, 1040, 1050, 1056, 1080, 1092, 1100, 1120, 1134, 1152, 1176, 1188, 1200, 1248, 1260, 1296, 1320, 1344, 1440 };
        internal Solution CreateInitialSolution(int initCounter) {
            Solution result = new Solution();
            int[] toUse;
            switch(initCounter) {
                case 0: toUse = coolArray;break;
                case 1: toUse = neverForgetti; break;
                case 2: toUse = neverForgetti; break;
                default: toUse = ExistingDataStatistics.allSolutions[initCounter - 3].Item2;break;
            }

            for (int i = 0; i < 256; i++) {
                int value = toUse[i];
                while (result.HasValue(value)) {
                    value++;
                }
                result[i] = value;
            }
            result.CheckNrOfValues();
            return result;
        }


        internal OptimizeBestSolution() {
            Console.Title = "Optimize best solution ...";
            for (int i = 0; i < Constants.NrOfCoresToUse; i++) {
                Thread optimizer =
                    new Thread(Optimize) {
                        Name = "Optimizer " + (i + 1),
                        IsBackground = true
                    };
                optimizer.Start(i);
            }
        }
        
        private void Optimize(object oIndex) {
            int index = (int)oIndex;
            Solution solution = CreateInitialSolution(index);
            solution.UpdateCountAms(int.MaxValue);
            CurrentDataStatistics.ReportBest(solution);
            int localBest = solution.CountAms;
            CurrentDataStatistics.localBest[index] = localBest;
            long notImprovedCount = 0;
            const int NotImprovedThresholdRandomize = 512*2000;
            const int SearchDepth = 1;
            const int MaxSearchDepth = 256;
            NumberPicker number = new NumberPicker();
            while (true) {
                int searchDepth = (int)(SearchDepth + notImprovedCount / 2000);
                if (searchDepth > MaxSearchDepth) {
                    searchDepth = MaxSearchDepth;
                }
                if (searchDepth < 50) {
                    if (TryUpdateMultipleValuesFromBestValues(number, solution, localBest, searchDepth)) {
                        CurrentDataStatistics.ReportBest(solution);
                        localBest = solution.CountAms;
                        CurrentDataStatistics.localBest[index] = localBest;
                        notImprovedCount = 0;
                    } else {
                        notImprovedCount++;
                    }
                }  else { 
                    if (notImprovedCount < NotImprovedThresholdRandomize) {
                        if (TryUpdateMultipleValuesFromOneToTwoThousand(number, solution, localBest, searchDepth)) {
                            CurrentDataStatistics.ReportBest(solution);
                            localBest = solution.CountAms;
                            CurrentDataStatistics.localBest[index] = localBest;
                            notImprovedCount = 0;
                        } else {
                            notImprovedCount++;
                        }
                    } else if (notImprovedCount > NotImprovedThresholdRandomize) {
                        // no improvements for some time /
                        // re- randomize the content
                        for (int i = 0; i < 50; i++) {
                            int position = number.PickIncluding(255);
                            // try to pick a new value
                            int value;
                            do {
                                value = number.PickIncluding(1, 2000);
                            } while (solution.HasValue(value));
                            // a new value
                            solution[position] = value;
                        }
                        Interlocked.Increment(ref CurrentDataStatistics.evaluated);
                        solution.UpdateCountAms(Int32.MaxValue);
                        CurrentDataStatistics.ReportBest(solution);
                        localBest = solution.CountAms;
                        CurrentDataStatistics.localBest[index] = localBest;
                        notImprovedCount = 0;
                    }
                }
            }
        }

/*
        private bool TryUpdateValueFirst(Solution solution, int localBest, int depth) {
            if (depth == 0) {
                return false;
            }

            int newValue;
            do {
                newValue = PickInteger(1, 2000);
            } while (solution.HasValue(newValue));

            for (int i = 0; i < 256; i++) {
                int oldValue = solution[i];
                solution[i] = newValue;
                Interlocked.Increment(ref CurrentDataStatistics.evaluated);
                if (solution.UpdateCountAms(localBest)) {
                    return true;
                } else {
                    if (!TryUpdateValueFirst(solution, localBest, depth - 1)) {
                        solution[i] = oldValue;
                        return false;
                    } else {
                        return true;
                    }
                }
            }
            return false;
        }
*/

        private bool TryUpdateMultipleValuesFromOneToTwoThousand(NumberPicker number, Solution solution, int localBest, int depth) {
            if (depth == 0) {
                return false;
            }
            int position = number.PickIncluding(0, 255);
            int oldValue = solution[position];
            int newValue;
            do {
                newValue = number.PickIncluding(1,2000);
            } while (solution.HasValue(newValue));
            solution[position] = newValue;
            Interlocked.Increment(ref CurrentDataStatistics.evaluated);
            if (solution.UpdateCountAms(localBest)) {
                return true;
            }
        
            if (!TryUpdateMultipleValuesFromOneToTwoThousand(number, solution, localBest, depth - 1)) {
                solution[position] = oldValue;
                return false;
            } else {
                return true;
            }
        }

        private bool TryUpdateMultipleValuesFromBestValues(NumberPicker number, Solution solution, int localBest, int depth) {
            if (depth == 0) {
                return false;
            }
            int position = number.PickIncluding(0, 255);
            int oldValue = solution[position];
            int[] values = ExistingDataStatistics.bestValuesPerPosition[position];
            int max = values.Length;
            int newValue;
            int retries = max + 1;
            do {
                int index = number.PickExcluding(max);
                newValue = values[index];
                retries--;
                if (retries == 0) {
                    return false;
                }
            } while (solution.HasValue(newValue));
            solution[position] = newValue;
            Interlocked.Increment(ref CurrentDataStatistics.evaluated);
            if (solution.UpdateCountAms(localBest)) {
                return true;
            }

            if (!TryUpdateMultipleValuesFromBestValues(number, solution, localBest, depth - 1)) {
                solution[position] = oldValue;
                return false;
            } else {
                return true;
            }
        }
    }
}
    