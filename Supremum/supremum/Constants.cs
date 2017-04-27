using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace supremum {
    internal static class Constants {
        internal const string OutputDir = @"D:\VIEWS\External\mvw\Experiments\Supremum";

        

#if DEBUG
        internal const int NrOfCoresToUse = 1;
#else
        internal const int NrOfCoresToUse = 8;
#endif

    }
}
