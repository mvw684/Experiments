using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Threading.Tasks;

namespace FizzBuzz {

    /// <summary>
    /// Write a program that prints the numbers from 1 to 100. 
    /// But for multiples of three print “Fizz” instead of the number 
    /// and for the multiples of five print “Buzz”. 
    /// 
    /// For numbers which are multiples of both three and five print “FizzBuzz”.
    /// </summary>
    /// <remarks>
    /// Expected future extension: Woof, multiples of 7, by another engineer
    /// </remarks>
    class Program {
        static void Main(string[] args) {

            Console.WriteLine("Execution started");
            string filePath = @"C:\test1.txt";
            string[] allLines = { "line1", "line2" };
            //Write into text file
            File.WriteAllLines(filePath, allLines);


            //Read from a text file
            Console.WriteLine("Reading from a file: {0}", File.ReadAllLines(filePath));


            Console.WriteLine("Execution Completed");

        }
    }
}
