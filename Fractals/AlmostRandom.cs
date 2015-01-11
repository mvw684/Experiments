
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Fractals {
    public static class AlmostRandom {
        static private uint x = 123456789, y = 362436069, z = 77465321, w = 13579;
        const double RealUnitInt = 1.0/((double)int.MaxValue+1.0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public int Next() {
            // http://en.wikipedia.org/wiki/Xorshift
            uint t = x ^ (x << 11);
            x = y; y = z; z = w;
            w = w ^ (w >> 19) ^ t ^ (t >> 8);
            return (int)(0x7FFFFFFF&w);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public double NextDouble() {
            int val = Next();
            return (double) val * 1.0d / (double) Int32.MaxValue;

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public int Next(int upperBound) {
            // http://en.wikipedia.org/wiki/Xorshift
            uint t = x ^ (x << 11);
            x = y; y = z; z = w;
            w = w ^ (w >> 19) ^ t ^ (t >> 8);
            return (int)(RealUnitInt*(int)(0x7FFFFFFF&w)*upperBound);
        }

        private const int MaxIndex = 2000;
        private static int index;
        private static int[] values = Get();

        private static int[] Get() {
            int offset = Environment.TickCount % MaxIndex;
            int[] result = new int[MaxIndex];
            for (int i = 0; i < MaxIndex; i++) {
                result[(i + offset) % MaxIndex] = Next();
            }
            return result;
        }

        public static void Init() {
            values = Get();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int NextFast(int upperBound) {
            if (upperBound <= 1) {
                return 0;
            }
            int local = Interlocked.Increment(ref index);
            int val = values[local % MaxIndex];
            return (int)(RealUnitInt*val*upperBound);
        }
    }
}
