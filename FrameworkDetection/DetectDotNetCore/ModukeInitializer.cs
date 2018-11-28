using System;
using System.Collections.Generic;
using System.Text;

using SharedProject1;

namespace DetectDotNetCore
{
    public class ModukeInitializer {
        public static void Run() {
            Console.WriteLine(
                "ModuleInitializer:" + typeof(ModukeInitializer).Assembly.GetName().Name + " " + SharedClass.Property
            );
        }
    }
}
