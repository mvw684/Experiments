using System;

namespace MultiTargetClassLibrary
{
    public static class MultiTargetClass {
        public static string Property {
            get {
                #if NET461
                    return "Targeting .NET 461";
                #elif NETCOREAPP2_0
                    return "Targeting .NET Core 2.0";
                #elif NETSTANDARD2_0
                    return "Targeting .NET standard 2.0";
                #else
                    #error this should not compile!
                #endif
            }
        }
    }
}
