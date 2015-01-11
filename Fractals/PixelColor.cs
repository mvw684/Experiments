using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fractals {
    public enum ColorPalette {
        None,
        Polynomial,
        Alternating,
        Sinus,
        Gray,
        RandomUpDown,
        RainBow

    }
    [StructLayout(LayoutKind.Explicit)]
    [DebuggerDisplay("Color: R={R}, G={G}, B={B}")]
    public struct PixelColor {

        [FieldOffset(0)]
        public int argb;
        //[FieldOffset(3)] public byte A;
        [FieldOffset(2)]
        public byte R;
        [FieldOffset(1)]
        public byte G;
        [FieldOffset(0)]
        public byte B;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PixelColor Average(PixelColor p, PixelColor q) {
            PixelColor r;
            r.argb = unchecked((int)0xff000000);
            // r.A = 0xff;
            r.R = (byte)((p.R + q.R)/2);
            r.G = (byte)((p.G + q.G)/2);
            r.B = (byte)((p.B + q.B)/2);
            return r;
        }

        public int Argb {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return argb;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PixelColor OffSetColor(int randomInput) {
            PixelColor result;
            int randomHalf = randomInput/2;
            int r = R + AlmostRandom.Next(randomInput) - randomHalf;
            int g = G + AlmostRandom.Next(randomInput) - randomHalf;
            int b = B + AlmostRandom.Next(randomInput) - randomHalf;
            if (r > 255) {
                r = 255;
            } else if (r < 0) {
                r = 0;
            }
            if (g > 255) {
                g = 255;
            } else if (g < 0) {
                g = 0;
            }
            if (b > 255) {
                b = 255;
            } else if (b < 0) {
                b = 0;
            }
            result.argb =unchecked((int)0xff000000);
            //result.A = 0xff;
            result.R = (byte)r;
            result.G = (byte)g;
            result.B = (byte)b;
            return result;
        }


        /// <summary>
        /// hue: color, 0 .. 360
        /// saturation: 0 .. 1
        /// brightness  : 0 .. 1
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="brightness"></param>
        /// <returns></returns>
        public static PixelColor ColorFromAhsb(double hue, double saturation, double brightness) {
            PixelColor result;
            //const int a = 255;
            result.argb = unchecked((int)0xff000000);
                
            const double tolerance = 0.00001d;
            
            while (hue < 0) {
                hue += 360;
            }
            while (hue >= 360) {
                hue -= 360;
            }

            if (0d > saturation || 1d < saturation) {
                throw new ArgumentOutOfRangeException("saturation", saturation,"Invalid Saturation");
            }
            if (0d > brightness || 1d < brightness) {
                throw new ArgumentOutOfRangeException("brightness", brightness,"Invalid Brightness");
            }


            if (Math.Abs(saturation) < tolerance) {
                result.R = Convert.ToByte(brightness * 255);
                result.G = result.R;
                result.B = result.R;
                return result;
            }

            hue /= 60;
            int iSextant = (int)Math.Floor(hue);
            double f = hue - iSextant;
            double p = brightness * (1 - saturation);
            double q = brightness * (1 - saturation * f);
            double t = brightness * (1 - saturation * (1 - f));
            
            
            switch (iSextant) {
                case 0:
                    result.R = Convert.ToByte(brightness * 255);
                    result.G = Convert.ToByte(t * 255);
                    result.B = Convert.ToByte(p * 255);
                    break;
                case 1:
                    result.R = Convert.ToByte(q * 255);
                    result.G = Convert.ToByte(brightness * 255);
                    result.B = Convert.ToByte(p * 255);
                    break;
                case 2:
                    result.R = Convert.ToByte(p * 255);
                    result.G = Convert.ToByte(brightness * 255);
                    result.B = Convert.ToByte(t * 255);
                    break;
                case 3:
                    result.R = Convert.ToByte(p * 255);
                    result.G = Convert.ToByte(q * 255);
                    result.B = Convert.ToByte(brightness * 255);
                    break;
                case 4:
                    result.R = Convert.ToByte(t * 255);
                    result.G = Convert.ToByte(p * 255);
                    result.B = Convert.ToByte(brightness * 255);
                    break;
                case 5:
                    result.R = Convert.ToByte(brightness * 255);
                    result.G = Convert.ToByte(p * 255);
                    result.B = Convert.ToByte(q * 255);
                    break;
                default:
                    Debugger.Break();
                    result.R = Convert.ToByte(brightness * 255);
                    result.G = Convert.ToByte(p * 255);
                    result.B = Convert.ToByte(q * 255);
                    break;
            }
            return result;
        }


        #region static stuff

        public const int Black = unchecked((int)0xff000000);
        const int BlackMark = 0x0f000000;
        public const int White = unchecked((int)0xffffffff);

        public const int ColorMax = 1000;
        public const int ColorLast = ColorMax - 1;
        public static int[] Colors = Palette();


        // for (int i = 0; i < ColorMax; i++) {
        //double t = (double) i / (double) ColorMax;

        // t: 0..1 ...

        

        //color.R = (byte) ((9d * (1d - t)*t * t * t)* 0xff);
        //color.G = (byte) ((15d * (1d - t) * 2 * t * t)* 0xff);
        //color.B = (byte) ((8.5d*(1 - t)*3d*t)* 0xff);
        //int c =  ColorMax - i;
        
        //result[i] =
        //    ColorFromAhsb(
        //        ((i * 4 * 360 / ColorMax) % 360),
        //        (0.8f + 0.2f * (float)i / (float)ColorMax),
        //        (0.5f + 0.5f * (float)i / (float)ColorMax)
        //    ).Argb;


        public static ColorPalette selectedPalet;
        public static ColorPalette PaletteSelection {
            get {
                if (selectedPalet == ColorPalette.None) {
                    selectedPalet = ColorPalette.Polynomial;
                }
                return selectedPalet;
            }
            set {
                selectedPalet = value;
                Colors = Palette();
            }
        }

        private static int[] Palette() {
            switch (PaletteSelection) {
                case ColorPalette.Polynomial:
                    return PalettePlynomial();
                case ColorPalette.Alternating:
                    return PaletteAlternating();
                case ColorPalette.Sinus:
                    return PaletteSinus();
                case ColorPalette.Gray:
                    return PaletteGray();
                case ColorPalette.RandomUpDown:
                    return PaletteRandomUpDown();
                case ColorPalette.RainBow:
                    return PaletteRainBow();
                default:
                    return PalettePlynomial();
            }
        }

        private static int[] PalettePlynomial() {
            var result = new int[ColorMax];

            PixelColor color;
            color.argb = Black;
            for (int i = 0; i < ColorMax; i++) {
                double t = (double)i / (double)ColorMax;
                color.argb = Black;
                color.R = (byte) ((9d * (1d - t)*t * t * t)* 0xff);
                color.G = (byte) ((15d * (1d - t) * 2 * t * t)* 0xff);
                color.B = (byte) ((8.5d*(1 - t)*3d*t)* 0xff);
                result[i] = color.argb;

            }
            result[0] = Black;
            result[ColorLast] = Black;
            return result;
        }

        private static int[] PaletteAlternating() {
            var result = new int[ColorMax];

            PixelColor color;
            color.argb = Black;
            for (int i = 0; i < ColorMax; i++) {
                color.argb = Black;
                double t = (double)i / (double)ColorMax;
                t /= 2;
                t += 0.5;
                byte v = (byte)(int)(t * 256);
                switch (i % 3) {
                    case 0:
                        color.R = v;
                        break;
                    case 1:
                        color.G = v;
                        break;
                    case 2:
                        color.B = v;
                        break;
                }   
                result[i] = color.argb;
                
            }
            result[0] = Black;
            result[ColorLast] = Black;
            return result;
        }

        private static int[] PaletteSinus() {
            var result = new int[ColorMax];

            PixelColor color;
            color.argb = Black;
            for (int i = 0; i < ColorMax; i++) {
                double t = (double)i / (double)ColorMax;
                color.argb = Black;
                color.R = (byte)((1+ Math.Sin(4 * t * Math.PI + Math.PI ))/2 * 0xff);
                color.G = (byte)((1+ Math.Sin(4 * t * Math.PI + Math.PI/2 ))/2 * 0xff);
                color.B = (byte)((1+ Math.Sin(4 * t * Math.PI))/2 * 0xff);
                result[i] = color.argb;

            }
            result[0] = Black;
            result[ColorLast] = Black;
            return result;
        }


        private static int[] PaletteGray() {
            var result = new int[ColorMax];

            PixelColor color;
            color.argb = Black;
            for (int i = 0; i < ColorMax; i++) {
                double t = (double)i / (double)ColorMax;
                color.argb = Black;
                color.R = (byte)(t * 0xff);
                color.G = (byte)(t * 0xff);
                color.B = (byte)(t * 0xff);
                result[i] = color.argb;

            }
            result[0] = Black;
            result[ColorLast] = Black;
            return result;
        }

        private static int[] PaletteRandomUpDown() {
            var result = new int[ColorMax];
            const int up = 2;
            const int down = -2;
            int dr = up;
            int dg = down;
            int db = up;

            int r = 0xff;
            int g = AlmostRandom.Next(0xff);
            int b = AlmostRandom.Next(0xff);

            PixelColor color;
            color.argb = Black;
            for (int i = ColorLast; i >= 0; i--) {
                color.argb = Black;
                color.R = (byte)r;
                color.G = (byte)g;
                color.B = (byte)b;
                result[i] = color.argb;

                if (r == 0) {
                    dr = up;
                }
                if (g == 0) {
                    dg = up;
                }
                if (b == 0) {
                    dg = up;
                }
                if (r == 0xff) {
                    dr = down;
                }
                if (g == 0xff) {
                    dg = down;
                }
                if (b == 0xff) {
                    db = down;
                }
                r += dr;
                g += dg;
                b += db;
            }
            result[0] = Black;
            result[ColorLast] = Black;
            return result;
        }



        private static int[] PaletteRainBow() {
            var result = new int[ColorMax];
            PixelColor[] rainbow = {
                PixelColor.ColorFromAhsb(0, 1, 0.5),
                PixelColor.ColorFromAhsb(30, 1, 0.5),
                PixelColor.ColorFromAhsb(60, 1, 0.5),
                PixelColor.ColorFromAhsb(90, 1, 0.5),
                PixelColor.ColorFromAhsb(120, 1, 0.5),
                PixelColor.ColorFromAhsb(150, 1, 0.5),
                PixelColor.ColorFromAhsb(180, 1, 0.5),
                PixelColor.ColorFromAhsb(210, 1, 0.5),
                PixelColor.ColorFromAhsb(240, 1, 0.5),
                PixelColor.ColorFromAhsb(270, 1, 0.5),
                PixelColor.ColorFromAhsb(300, 1, 0.5),
                PixelColor.ColorFromAhsb(330, 1, 0.5),
            };


            
            PixelColor color;
            color.argb = Black;

            for (int i = 1; i < ColorLast; i++) {
                color = ColorFromAhsb(i - 1 , 1.0d, 0.5d);
                result[i] = color.argb;
            }
            result[0] = Black;
            result[ColorLast] = Black;
            return result;
        }
        #endregion
    }
}
