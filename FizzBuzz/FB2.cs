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
    internal class FB2 {
        internal static void Main() {
            for (int i = 1; i <= 100; i++) {
                FB(i);
            }
        }

        private static void FB(int number) {
            string fb;

            bool fizz = number % 3 == 0;
            bool buzz = number % 5 == 0;

            if (fizz && buzz) {
                fb = "FizzBuzz";
            } else if (fizz) {
                fb = "Fizz";
            } else if (buzz) {
                fb = "Buzz";
            } else {
                fb = number.ToString(CultureInfo.InvariantCulture);
            }
            Console.WriteLine(fb);

        }
    }
}
