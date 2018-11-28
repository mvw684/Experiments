using System;
using FrameworkDetection;

namespace DetectDotNetCore {
    class Program {
        static void Main(string[] args) {

            Console.WriteLine("DotNetCore.MultiTarget: " + MultiTargetClassLibrary.MultiTargetClass.Property);
            Console.WriteLine("DotNetCore.SharedTarget: " + SharedProject1.SharedClass.Property);
            Console.WriteLine();
            Detector.Detect();
        }
    }
}
