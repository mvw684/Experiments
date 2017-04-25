using System;
using System.Collections.Generic;
using System.Text;

namespace supremum {
    /// <summary>
    /// Provides suitable prime numbers to be used as size of bucket arrays.
    /// Using primes gives a far better distribution of values over the buckets,
    /// thereby significantly improving performance.
    /// </summary>
    /// <remarks>
    /// http://scottonwriting.net/sowblog/posts/1674.aspx
    /// http://blogs.msdn.com/kcwalina/archive/2004/08/06/210297.aspx
    /// </remarks>
    public static class Primes {

        /// <summary>
        /// Following table is created by starting with 3, 
        /// add 20% then find next larger prime number, 
        /// all primes below and including 107 are also added,
        /// till 256, steps of 10% are taken.
        /// On those numbers reduction of memory is important, considering DataObject requirements
        /// This yields a set of sizes which balances reallocations with overallocation.
        /// </summary>
        /// <remarks>
        /// You can regenerate this table with different number, do relevant  measurements
        /// and commit to the archive.
        /// Note that this table does NOT contain ALL the primes in the generated range.
        /// </remarks>
        private static readonly int[] primes = {
            3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71,
            73, 79, 83, 89, 97, 101, 103, 107, 127, 149, 167, 191, 211, 233, 257, 311,
            379, 457, 557, 673, 809, 971, 1171, 1409, 1693, 2039, 2447, 2939, 3527,
            4241, 5099, 6121, 7349, 8819, 10589, 12713, 15259, 18311, 21977, 26387,
            31667, 38011, 45631, 54767, 65729, 78877, 94687, 113647, 136379, 163661,
            196429, 235723, 282869, 339467, 407369, 488861, 586667, 704003, 844841,
            1013813, 1216577, 1459901, 1751891, 2102273, 2522743, 3027293, 3632753,
            4359307, 5231173, 6277409, 7532929
        };

        /// <summary>
        /// Consider all primes till this number (107).
        /// </summary>
        private const int LowerBound = 107;

        /// <summary>
        /// Increase with small steps (10%) from <see cref="LowerBound"/> 
        /// till this value (256)
        /// </summary>
        private const int ConservativeSize = 256;

        /// <summary>
        /// Between <see cref="ConservativeSize"/> and this number seek 
        /// next prime larger then 20% of previous.
        /// </summary>
        private const int UpperBound = 8 * 1024 * 1024;

        /// <summary>
        /// Biggest prime in our table, so if numbers are bigger,
        /// we need to do computation to find a suitable prime.
        /// </summary>
        /// <remarks>This needs to be copied from the generated table.</remarks>
        private const int lastInTable = 7532929;

        /// <summary>
        /// This function is called IsPrime, 
        /// that should be sufficient explanation to most.
        /// </summary>
        /// <remarks>
        /// http://en.wikipedia.org/wiki/Even_and_odd_numbers
        /// http://en.wikipedia.org/wiki/Prime_number
        /// http://en.wikipedia.org/wiki/Primality_test
        /// </remarks>
        public static bool IsPrime(int number) {
            // first read http://en.wikipedia.org/wiki/Primality_test
            // then read next code
            if ((number & 1) == 0) {
                // even, only two a prime
                return number == 2;
            }

            if ((number % 3) == 0) {
                // multiple of three
                // a prime is not a multiple of 3, except for 3 itself
                return (number == 3);
            }

            // did test for even and multiple of three
            // thereby ruling out many of potential checks.
            // now starting with numbers of form, 
            //      6 * i + K 
            //          with i > 0
            //          K  in -1, 0, 1, 2 , 3, 4
            //
            // observe 6 * i - 1 potential prime
            // observe 6 * i + 0 devides by 3 and 2
            // observe 6 * i + 1 potential prime
            // observe 6 * i + 2 devides by 2
            // observe 6 * i + 3 devides by 3
            // observe 6 * i + 4 devides by 2
            // (note K = 0,1,2,3,4 have been dealt with) (check on division by 2 and 3 above)
            // remains to test 6 * i - 1 and 6 * i + 1 where 
            // 6 * i + 1 smaller then or equal to sqrt(number)
            // note index == 6 * i.

            int limit = (int)Math.Sqrt(number);
            int indexMinusOne = 5; // ( 6 * 1 - 1)
            int indexPlusOne = 7;  // (6 * 1 + 1)
            while (indexMinusOne <= limit) {
                if ((number % indexMinusOne) == 0) {
                    return false;
                }
                if ((number % indexPlusOne) == 0) {
                    return false;
                }
                indexMinusOne += 6;
                indexPlusOne += 6;
            }
            return true;
        }

        /// <summary>
        /// This function returns next applicable prime to be used 
        /// as nr of buckets in a hashtable.
        /// </summary>
        public static int GetNextBucketsSize(int min) {
            if (min < lastInTable) {
                for (int i = 0; i < primes.Length; i++) {
                    int prime = primes[i];
                    if (prime >= min) {
                        return prime;
                    }
                }
            }
            // next code is a safeguard and rather harmless

            if (min < 5) {
                if (min <= 2) {
                    return 2;
                }
                if (min <= 3) {
                    return 3;
                }
                return 5;
            }
            //outside of our predefined table. 
            //compute the hard way. 
            // first read http://en.wikipedia.org/wiki/Primality_test
            // then read next code

            // min >= 5
            // index is first number with value 6 * i,
            // that satisfies 6 * i >= min, with i in 1..n.
            int index = 6 * ((min + 5) / 6);
            // now index is of form 6 * i and 6 * i >= min

            // candidate prime is nr of form (6 * i) + 1 or (6 * i) - 1 
            int indexPlusOne = index + 1;
            int indexMinusOne = index - 1;

            if (indexMinusOne < min) {
                if (IsPrime(indexPlusOne)) {
                    return indexPlusOne;
                }
                // if index - 1 smaller then min take next immediately.
                indexMinusOne += 6;
                indexPlusOne += 6;
            }
            do {
                if (IsPrime(indexMinusOne)) {
                    return indexMinusOne;
                }

                if (IsPrime(indexPlusOne)) {
                    return indexPlusOne;
                }

                unchecked {
                    indexPlusOne += 6;
                    indexMinusOne += 6;
                }
            } while (indexMinusOne > 0);
            // can't do better than this
            return min | 1;
        }

        /// <summary>
        /// Generate header part of this file, 
        /// especially the prims array
        /// </summary>
        public static void MainXxxx() {

            List<int> primes = new List<int>(UpperBound);
            for (int i = 2; i < UpperBound; i++) {
                if (IsPrime(i)) {
                    primes.Add(i);
                }
            }

            Console.WriteLine("       private static readonly int[] primes = {");

            int primeIndex = 0;
            int bottom = 2;
            do {
                PrimeLine(primes, ref primeIndex, ref bottom);
            } while (primeIndex < primes.Count);

            Console.WriteLine();
            Console.WriteLine("Copy the table to correct source file");
            Console.WriteLine("Then press enter.");
            Console.ReadLine();
        }

        private static void PrimeLine(List<int> primes, ref int primeIndex, ref int bottom) {
            StringBuilder sb = new StringBuilder(100);
            sb.Append("           ");
            bool notFirst = false;
            while ((primeIndex < primes.Count) && (sb.Length < 80)) {
                if (primes[primeIndex] > bottom) {
                    if (notFirst) {
                        sb.Append(", ");
                    } else {
                        notFirst = true;
                    }
                    sb.Append(primes[primeIndex]);
                    bottom = primes[primeIndex];
                    if (bottom < Primes.LowerBound) {
                        bottom++;
                    } else if (bottom < Primes.ConservativeSize) {
                        bottom = (int)(1.1 * bottom);
                    } else {
                        bottom = (int)(1.2 * bottom);
                    }
                }
                primeIndex++;
            }
            if (sb.Length >= 80) {
                sb.Append(",");
            }
            Console.WriteLine(sb);
        }
    }
}
