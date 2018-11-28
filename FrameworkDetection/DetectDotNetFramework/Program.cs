using System;
using FrameworkDetection;

namespace DetectDotNetFramework {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("DotNetFramework.MultiTarget: " + MultiTargetClassLibrary.MultiTargetClass.Property);
            Console.WriteLine("DotNetFramework.SharedTarget: " + SharedProject1.SharedClass.Property);
            
            Detector.Detect();
        }
    }
}
