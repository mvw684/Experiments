
using System.Diagnostics;

namespace supremum {
    internal static class Constants {
        internal const string OutputDir = @"D:\VIEWS\External\mvw\Experiments\Supremum";

        internal const int SolutionSize = 256;
        internal const int NotGoodEnough = 5900;
        internal static int GoodEnough = 5820;
#if DEBUG
        internal static int NrOfCoresToUse = Debugger.IsAttached ? 1 : 4;
        
#else
        internal const int NrOfCoresToUse = 8;
#endif

    }
}
