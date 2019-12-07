using System;
using System.Globalization;

namespace FizzBuzz {
    /// <summary>
    /// Write a program that prints the numbers from 1 to 100. 
    /// But for multiples of three print “Fizz” instead of the number 
    /// and for the multiples of five print “Buzz”. 
    /// 
    /// For numbers which are multiples of both three and five print “FizzBuzz”.
    /// </summary>
    internal class FB3 {
        internal static void Main() {
            for (int i = 1; i <= 100; i++) {
                FBW(i);
            }
        }

        private static void FBW(int number) {
            string fb = Fizz(number) + Buzz(number) + Woof(number);
            if (string.IsNullOrEmpty(fb)) {
                fb = number.ToString(CultureInfo.InvariantCulture);
            }
            Console.WriteLine(fb);
        }

        private static string Fizz(int number) {
            return number % 3 == 0 ? "Fizz" : string.Empty;
        }

        private static string Buzz(int number) {
            return number % 5 == 0 ? "Buzz" : String.Empty;
        }

        private static object Woof(int number) {
            return number % 7 == 0 ? "Woof" : string.Empty;
        }
    }
}
