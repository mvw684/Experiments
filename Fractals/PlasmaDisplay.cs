using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Drawing.Drawing2D;

namespace Fractals {
    public partial class PlasmaDisplay : Form {

        private class Rect {
            public PixelColor TopLeft;
            public PixelColor TopRight;
            public PixelColor BottomLeft;
            public PixelColor BottomRight;
            public Rectangle Position;
        }

        private Task root;

        private int[,] bits;
        private Size bitsSize;
        private int bitsArea;
        private int randomOffset;
        
        public PlasmaDisplay() {
            InitializeComponent();
            var values = Enum.GetValues(typeof(InterpolationMode));
            for (int i = 0;i < values.Length;i++) {
                InterpolationMode mode = (InterpolationMode)values.GetValue(i);
                if (mode == InterpolationMode.Invalid) {
                    continue;
                }
                interpolationMode.Items.Add(mode);
                if (mode == InterpolationMode.HighQualityBicubic) {
                    interpolationMode.SelectedIndex = i;
                }
            }

            interpolationMode.SelectedValue = InterpolationMode.HighQualityBicubic;
            stop.Enabled = false;
        }

        protected override void OnShown(EventArgs e) {
            base.OnShown(e);
            BeginInvoke(new EventHandler(start_Click), this, new EventArgs());
        }

        private void start_Click(object sender, EventArgs e) {
            stop.Enabled = true;
            AlmostRandom.Init();
            start.Enabled = false;
            randomness.Enabled = false;
            randomOffset = (int)randomness.Value;
            bitsSize = display.BitsSize;
            bits = display.Bits;
            bitsArea = bitsSize.Height * bitsSize.Width;
            display.Changed = true;

            Rect initial = new Rect {
                TopLeft = { argb = unchecked((int)0xff000000), R = (byte) AlmostRandom.Next(255), G = (byte) AlmostRandom.Next(255), B = (byte) AlmostRandom.Next(255)},
                BottomLeft = { argb = unchecked((int)0xff000000), R = (byte)AlmostRandom.Next(255), G = (byte)AlmostRandom.Next(255), B = (byte)AlmostRandom.Next(255) },
                TopRight = { argb = unchecked((int)0xff000000), R = (byte)AlmostRandom.Next(255), G = (byte)AlmostRandom.Next(255), B = (byte)AlmostRandom.Next(255) },
                BottomRight = { argb = unchecked((int)0xff000000), R = (byte)AlmostRandom.Next(255), G = (byte)AlmostRandom.Next(255), B = (byte)AlmostRandom.Next(255) },
                Position = {X = 0, Y = 0, Width = bitsSize.Width, Height = bitsSize.Height }
            };

            Set(initial);

            display.Changed = true;

            root = new Task(() => Compute(initial, 2));
            var done = new Action<Task>(parent => BeginInvoke(new EventHandler(stop_Click), this, new EventArgs()));
            root.ContinueWith(done, TaskContinuationOptions.ExecuteSynchronously);
            root.Start(Scheduler.Instance);
        }

        private void stop_Click(object sender, EventArgs e) {
            root.Wait();
            stop.Enabled = false;
            start.Enabled = true;
            randomness.Enabled = true;
            display.Invalidate();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set(int x, int y, PixelColor color) {
            bits[y, x] = color.Argb;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set(Rect rect) {
            Rectangle p = rect.Position;
            int x1 = p.X;
            int y1 = p.Y;
            //int x2 = x1 + p.Width;
            //int y2 = y1 + p.Height;
            
            bits[y1, x1] = rect.TopLeft.Argb;
            //bits[y1, x2] = rect.TopRight.Argb;
            //bits[y2, x1] = rect.BottomLeft.Argb;
            //bits[y2, x2] = rect.BottomRight.Argb;
        }

        private void Compute(Rect item, int async) {

            Rectangle p = item.Position;
            
            int height = p.Height;
            int width = p.Width;
            if (height == 0 || width == 0 || (width <= 1 && height <= 1)) {
                return;
            }

            int offset = randomOffset + (height * width ) * (256-randomOffset) / bitsArea;
            
            if (height == 1) {

                #region horizontal
                // x1,y1 .. xc,y1 ..  x2,y1
                // x1,y2 .. xc,y2 ..  x2,y2

                int x1 = p.X;
                int y1 = p.Y;
                //int y2 = y1 + 1;
                int x2 = x1 + p.Width;
                int xc = x1 + p.Width/2;

                PixelColor topMid = PixelColor.Average(item.TopLeft, item.TopRight);
                PixelColor bottomMid = PixelColor.Average(item.BottomLeft, item.TopRight);
                topMid = topMid.OffSetColor(offset);
                bottomMid = bottomMid.OffSetColor(offset);
                Set(xc, y1, topMid);
                //Set(xc, y2, bottomMid);
                display.Changed = true;
                Rect left =
                    new Rect {
                        TopLeft = item.TopLeft,
                        BottomLeft = item.BottomLeft,
                        BottomRight = topMid,
                        TopRight = bottomMid,
                        Position = {X = x1, Y = y1, Height = height , Width = xc - x1}
                    };


                Rect right =
                    new Rect {
                        TopLeft = topMid,
                        BottomLeft = bottomMid,
                        TopRight = item.TopRight,
                        BottomRight = item.BottomRight,
                        Position = {X = xc, Y = y1, Height = height, Width = x2 - xc}
                    };
                
                Compute(left,0);
                Compute(right, 0);

                #endregion vertical

            } else if (width == 1) {

                #region vertical

                // x1,y1 ... x2,y1
                // .
                // . 
                // x1,yc ... x2,yc
                // .
                // .
                // x1,y2 ... x2,y2
                int x1 = p.X;
                //int x2 = x1 + 1;
                int y1 = p.Y;
                int y2 = y1 + p.Height;
                int yc = y1 + p.Height/2;

                PixelColor leftMid = PixelColor.Average(item.TopLeft, item.BottomLeft);
                PixelColor rightMid = PixelColor.Average(item.TopRight, item.BottomRight);

                leftMid = leftMid.OffSetColor(offset);
                rightMid = rightMid.OffSetColor(offset);
                Set(x1, yc, leftMid);
                //Set(x2, yc, rightMid);

                display.Changed = true;
                Rect top =
                    new Rect {
                        TopLeft = item.TopLeft,
                        TopRight = item.TopRight,
                        BottomRight = rightMid,
                        BottomLeft = leftMid,
                        Position = {X = x1, Y = y1, Width = 1, Height = yc - y1}
                    };

                Rect bottom =
                    new Rect {
                        TopLeft = leftMid,
                        BottomRight = item.BottomRight,
                        TopRight = rightMid,
                        BottomLeft = item.BottomLeft,
                        Position = {X = x1, Y = yc, Width = 1, Height = y2 - yc}
                    };

                Compute(top, 0);
                Compute(bottom, 0);

                #endregion vertical
            } else {

                // x1,y1 ... xc,y1 ... x2,y1
                //   .         .        .
                //   .         .        .
                // x1,yc ... xc,yc ... x2,yc
                //   .         .        .
                //   .         .        .
                // x1,y2 ... xc,y2 ... X2,y2

                int x1 = p.X;
                int y1 = p.Y;
                int x2 = x1 + p.Width;
                int y2 = y1 + p.Height;
                int xc = x1 + p.Width/2;
                int yc = y1 + p.Height/2;


                PixelColor topMid = PixelColor.Average(item.TopLeft, item.TopRight);
                PixelColor bottomMid = PixelColor.Average(item.BottomLeft, item.BottomRight);
                PixelColor leftMid = PixelColor.Average(item.TopLeft, item.BottomLeft);
                PixelColor rightMid = PixelColor.Average(item.TopRight, item.BottomRight);
                PixelColor center = PixelColor.Average(topMid, bottomMid).OffSetColor(offset);

                Set(xc, y1, topMid);
                Set(xc, yc, center);
                //Set(xc, y2, bottomMid);
                Set(x1, yc, leftMid);
                //(x2, yc, rightMid);

                display.Changed = true;

                Rect topLeft =
                    new Rect {
                        TopLeft = item.TopLeft,
                        TopRight = topMid,
                        BottomLeft = leftMid,
                        BottomRight = center,
                        Position = {X = x1, Y = y1, Width = xc - x1, Height = yc - y1}
                    };
                Rect topRight =
                    new Rect {
                        TopLeft = topMid,
                        TopRight = item.TopRight,
                        BottomLeft = center,
                        BottomRight = rightMid,
                        Position = {X = xc, Y = y1, Width = x2 - xc, Height = yc - y1}
                    };
                Rect bottomLeft =
                    new Rect {
                        TopLeft = leftMid,
                        TopRight = center,
                        BottomLeft = item.BottomLeft,
                        BottomRight = bottomMid,
                        Position = {X = x1, Y = yc, Width = xc - x1, Height = y2 - yc}
                    };
                Rect bottomRight =
                    new Rect {
                        TopLeft = center,
                        TopRight = rightMid,
                        BottomLeft = bottomMid,
                        BottomRight = item.BottomRight,
                        Position = {X = xc, Y = yc, Width = x2 - xc, Height = y2 - yc}
                    };
                if (async > 0) {
                    Task t1 = new Task(() => Compute(topLeft, async - 1));
                    Task t2 = new Task(() => Compute(topRight, async - 1));
                    Task t3 = new Task(() => Compute(bottomLeft, async - 1));
                    Task t4 = new Task(() => Compute(bottomRight, async - 1));
                    t1.Start(Scheduler.Instance);
                    t2.Start(Scheduler.Instance);
                    t3.Start(Scheduler.Instance);
                    t4.Start(Scheduler.Instance);
                    Task.WaitAll(t1, t2, t3, t4);
                } else {
                    Compute(topLeft, async - 1);
                    Compute(topRight, async - 1);
                    Compute(bottomLeft, async - 1);
                    Compute(bottomRight, async - 1);
                }
            }
        }

        private void save_Click(object sender, EventArgs e) {
            display.Save();
        }

        private void label1_Click(object sender, EventArgs e) {
            randomness.Value = AlmostRandom.Next(0xff);
        }

        private void display_NewSelection(object sender, NewSelectionEventArgs e) {
            var sourceRect = e.NewSelection;
            var targetRect = new Rectangle(0, 0, sourceRect.Width, sourceRect.Height);
            Bitmap source = display.Bitmap;
            Bitmap target = new Bitmap(targetRect.Width, targetRect.Height, source.PixelFormat);
            Graphics g = Graphics.FromImage(target);
            g.CompositingMode = CompositingMode.SourceCopy;
            g.InterpolationMode = InterpolationMode.Low;
                
            g.DrawImage(source, targetRect,sourceRect, GraphicsUnit.Pixel);
            g.Dispose();
            g = Graphics.FromImage(source);
            g.CompositingMode = CompositingMode.SourceCopy;
            g.InterpolationMode = (InterpolationMode)interpolationMode.SelectedItem;
            g.CompositingQuality = CompositingQuality.HighQuality;
            var destinationRect = new Rectangle(0, 0, source.Width, source.Height);
            g.DrawImage(target, destinationRect);
            display.Changed = true;
        }

        private void close_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
