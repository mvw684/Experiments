using System;

using SharedProject1;

namespace MultiTargetClassLibrary
{
    public class ModukeInitializer {
        public static void Run() {
            Console.WriteLine(
                "ModuleInitializer:" + typeof(ModukeInitializer).Assembly.GetName().Name + " " + SharedClass.Property
            );
        }
    }
}
