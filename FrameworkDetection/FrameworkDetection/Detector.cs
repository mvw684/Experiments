using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FrameworkDetection {
    public static class Detector {
        public static void Detect() {
            var objecType = typeof(Object);
            var objectAssembly = objecType.Assembly;
            var assemblyName = objectAssembly.FullName;
            var assemblyLocation = objectAssembly.Location;
            var assemblyCodeBase = objectAssembly.CodeBase;

            var desc = RuntimeInformation.FrameworkDescription;
            var osArchitecture = RuntimeInformation.OSArchitecture;
            var osDescription = RuntimeInformation.OSDescription;
            var processArchitecture = RuntimeInformation.OSArchitecture;
            var target = MultiTargetClassLibrary.MultiTargetClass.Property;
            Console.WriteLine("AssemblyName:         " + assemblyName);
            Console.WriteLine("Location:             " + assemblyLocation);
            Console.WriteLine("CodeBase:             " + assemblyCodeBase);
            Console.WriteLine("FrameworkDescription: " + desc);
            Console.WriteLine("OS Architecture:      " + osArchitecture);
            Console.WriteLine("OS Description:       " + osDescription);
            Console.WriteLine("ProcessArchitecture:  " + processArchitecture);
            Console.WriteLine("Detect.MultiTarget: " + target);
            Console.ReadLine();
        }
    }
}
