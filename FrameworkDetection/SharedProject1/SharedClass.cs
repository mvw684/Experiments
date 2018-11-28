using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject1 {
    internal class SharedClass {
        internal static string Property {
            get {
                #if NET461
                    return "Targeting .NET 461";
                #elif NETCOREAPP2_0
                    return "Targeting .NET Core 2.0";
                #elif NETSTANDARD2_0
                    return "Targeting .NET Standard 2.0";
                #else
                    return "Unknown target";
                #endif
            }
        }
        
    }
}
