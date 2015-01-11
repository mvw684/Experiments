using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractals {
    public partial class MandelbrotDisplay : Form {

        private volatile bool running;
        private RectangleD original;
        private RectangleD range;
        private Stack<RectangleD> history = new Stack<RectangleD>(30);
        private PointD juliaStart;
        private Task root;
        private int[,] bits;
        
        private int bitsWidth;
        private int bitsHeight;

        private readonly EventHandler _start;
        
        public MandelbrotDisplay() {
            _start = start_Click;
            InitializeComponent();
            Array values = Enum.GetValues(typeof (ColorPalette));
            foreach (ColorPalette p in values) {
                if (p != ColorPalette.None) {
                    palette.Items.Add(p);
                }
            }
            palette.SelectedValue = values.GetValue(0);
            palette.SelectedItem = values.GetValue(0);
            palette.SelectedIndex = 0;
            juliaSelect.Minimum = 0;
            juliaSelect.Maximum = JuliaStarts.Length - 1;
            mandelbrotSelected.Checked = !julia;
            juliaSelect.Value = 0;
            juliaStart = JuliaStarts[0];
            juliaX.Value = (Decimal)juliaStart.X;
            juliaY.Value = (Decimal)juliaStart.Y;
        }

        protected override void OnShown(EventArgs e) {
            base.OnShown(e);

            bitsWidth = display.BitsSize.Width;
            bitsHeight = display.BitsSize.Height;
           
            original = new RectangleD(-2.6d, -2.1d, 5.2d, 5.2d);
            original.Height = original.Width * (double) bitsHeight / (double) bitsWidth;
            range = original;
            InvokeStart();
        }

        private void MandelbrotDisplay_SizeChanged(object sender, EventArgs e) {
            original = new RectangleD(-2.6d, -2.1d, 5.2d, 5.2d);
            original.Height = original.Width * (double)bitsHeight / (double)bitsWidth;
            range = original;history.Clear();
        }

        private void start_Click(object sender, EventArgs e) {
            running = true;
            bits = display.Bits;
            bitsWidth = display.BitsSize.Width;
            bitsHeight = display.BitsSize.Height;
           
            int count = bitsHeight / 10;
            Task[] tasks = new Task[count];
            int y1 = 0;
            int y2 = 0;
            for (int i = 1; i <= count; i++) {
                y2 = bitsHeight * i / count;
                
                int y3 = y1;
                int y4 = y2;
                tasks[i - 1] = new Task( () => Compute(y3,y4));
                y1 = y2;
            }
            Shuffle(tasks);
            root = new Task(() => Task.WaitAll(tasks));

            root.ContinueWith(
                parent => {
                    BeginInvoke(new EventHandler(stop_Click), this, new EventArgs());
                }
            );

            for (int i = 0; i < count; i++) {
                tasks[i].Start(Scheduler.Instance);
            }
            root.Start(Scheduler.Instance);
            start.Enabled = false;
            stop.Enabled = true;
            reset.Enabled = false;
            previous.Enabled = false;
            display.Enabled = false;
        }

        private void Shuffle(Task[] tasks) {
            for (int i = 0; i < tasks.Length; i++) {
                int p1 = AlmostRandom.Next(tasks.Length);
                int p2 = AlmostRandom.Next(tasks.Length);
                Task t = tasks[p1];
                tasks[p1] = tasks[p2];
                tasks[p2] = t;
            }
        }

        private void stop_Click(object sender, EventArgs e) {
            running = false;
            if (root != null) {
                root.Wait();
            }
            stop.Enabled = false;
            display.Enabled = true;
            start.Enabled = true;
            reset.Enabled = true;
            previous.Enabled = true;
            display.Focus();
            display.Select();
        }

        public bool julia = false;

        private readonly static PointD[] JuliaStarts = {
            new PointD {X = 0.233, Y = 0.53780},
            new PointD {X = 0.03515, Y = -0.7467},
            new PointD {X = -0.62772, Y = 0.42193},
            new PointD {X = -0.67319, Y = 0.34442},
            new PointD {X = -0.74173, Y = 0.15518},
            new PointD {X = -0.74434, Y = 0.10772},
            new PointD {X = -0.74543, Y = 0.11301}
        };

        private void Compute(int y1, int y2) {
            double xFactor = range.Width / (double)bitsWidth;
            double yFactor = range.Height / (double) bitsHeight;

            double cy = range.Y + y1 * yFactor;
            for (int y = y1; y < y2; y++) {
                double cx = range.X;
                for (int x = 0; x < bitsWidth; x++) {
                    int color;
                    if (julia) {
                        color = Compute(cx, cy, juliaStart.X, juliaStart.Y);
                    } else {
                        color = Compute(0d, 0d, cx, cy);
                    }
                    bits[y, x] = PixelColor.Colors[color];
                    display.Changed = true;
                    if (!running) {
                        return;
                    }
                    cx += xFactor;
                }
                cy += yFactor;
            }
        }

        static double escapeVal = 2;
        static double log2 = Math.Log(2);
        static double escapeValSquare = escapeVal * escapeVal;

        private int Compute(double xi, double yi, double cx, double cy) {
            int i = 0;

            double x = xi;
            double y = yi;
            double square;
            // x,y = (x,y)*(x,y) + (cx,cy)
            // x + yi =  (x+yi)*(x+yi) + cx + cyi
            // x + yi =  xx + xyi + yix -yy + cx +cyi
            // x + yi =  xx + xyi + xyi - yy + cx + cyi
            // x + yi =  xx + - yy + cx + 2xyi + cyi
            // x = xx - yy + cx
            // y = 2xyi + cyi
            for (;;) {
                double xs = x * x;
                double ys = y * y;
                square = xs + ys;
                if (
                    x > escapeVal ||
                    y > escapeVal ||
                    (square > escapeValSquare)
                ) {
                    break;
                }
                double xn = xs - ys + cx;
                y = 2 * x * y + cy;
                x = xn;
                i++;
                if (i == PixelColor.ColorLast) {
                    break;
                }
            }
            // return i; // banding ..

            if (i == PixelColor.ColorLast) {
                // max iterations always color as black
                return i;
            }
            i++;

            double val = 
                Math.Log(
                    Math.Log(
                        Math.Sqrt(square)
                    ) / log2
                ) / log2;
            //double val =
            //    Math.Log(
            //        Math.Log(
            //            Math.Sqrt(square)
            //            )
            //        ) / log2;


            int result = i  - (int) val;
            if (result < 0) {
                result = 0;
            }
            if (result >= PixelColor.ColorMax) {
                result = PixelColor.ColorLast;
            }
            return result;
        }

        private void display_NewSelection(object sender, NewSelectionEventArgs e) {
            history.Push(range);
            RectangleD oldRange = range;
            int x = e.NewSelection.Left;
            int y = e.NewSelection.Top;
            int width = e.NewSelection.Width;
            int height = e.NewSelection.Height;

            double xFactor = (double) width / (double) bitsWidth;
            double yFactor = (double) height / (double)bitsHeight;

            double factor = Math.Max(xFactor, yFactor);

            range.Width = oldRange.Width * factor;
            range.Height = oldRange.Height * factor;

            xFactor = (double) x / (double) bitsWidth;
            yFactor = (double) y / (double) bitsHeight;
            range.X = oldRange.X + oldRange.Width * xFactor;
            range.Y = oldRange.Y + oldRange.Height * yFactor;

            InvokeStart();
        }

        private void display_MouseWheel(object sender, MouseEventArgs e) {

            history.Push(range);
            
            double factor = e.Delta < 0 ? 1.2 : 0.8;

            double x = range.X + range.Width * e.X / bitsWidth;
            double y = range.Y + range.Height * e.Y / bitsHeight;

            range.Width *= factor;
            range.Height *= factor;

            range.X = x - range.Width * e.X / bitsWidth;
            range.Y = y - range.Height * e.Y / bitsHeight;
 
            
            InvokeStart();
        }

        private void display_MouseDoubleClick(object sender, MouseEventArgs e) {
            history.Push(range);
            double x = range.X + range.Width * e.X / bitsWidth;
            double y = range.Y + range.Height * e.Y / bitsHeight;
            range.X  = x - range.Width /2 ;
            range.Y = y - range.Height / 2;

            InvokeStart();
        }

        private void reset_Click(object sender, EventArgs e) {
            range = original;
            InvokeStart();
        }

        private void previous_Click(object sender, EventArgs e) {
            if (history.Count > 0) {
                range = history.Pop();
            } else {
                range = original;
            }
            InvokeStart();
        }

        private void save_Click(object sender, EventArgs e) {
            display.Save();
        }

        private void up_Click(object sender, EventArgs e) {
            history.Push(range);
            range.Y -= range.Height / 20;
            InvokeStart();
        }

        private void down_Click(object sender, EventArgs e) {
            history.Push(range);
            range.Y += range.Height / 20;
            InvokeStart();
        }

        private void right_Click(object sender, EventArgs e) {
            history.Push(range);
            range.X += range.Width / 20;
            InvokeStart();
        }

        private void left_Click(object sender, EventArgs e) {
            history.Push(range);
            double oldX = range.X;
            range.X -= range.Width / 20;
            InvokeStart();
        }

        private void zoomOut_Click(object sender, EventArgs e) {
            history.Push(range);
            double cx = range.X + range.Width / 2;
            double cy = range.Y + range.Height / 2;
            range.Width *= 1.2f;
            range.Height *= 1.2f;
            range.X = cx - range.Width / 2;
            range.Y = cy - range.Height / 2;
            InvokeStart();
        }

        private void zoomIn_Click(object sender, EventArgs e) {
            history.Push(range);
            double cx = range.X + range.Width / 2;
            double cy = range.Y + range.Height / 2;
            range.Width *= 0.8f;
            range.Height *= 0.8f;
            range.X = cx - range.Width / 2;
            range.Y = cy - range.Height / 2;
            InvokeStart();
        }

        private void close_Click(object sender, EventArgs e) {
            Close();
        }

        private void display_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Up) {
                up_Click(sender, EventArgs.Empty);
            } else if (e.KeyCode == Keys.Down) {
                down_Click(sender, EventArgs.Empty);
            } else if (e.KeyCode == Keys.Left) {
                left_Click(sender, EventArgs.Empty);
            } else if (e.KeyCode == Keys.Right) {
                right_Click(sender, EventArgs.Empty);
            } else if (e.KeyCode == Keys.Add) {
                zoomIn_Click(sender, EventArgs.Empty);
            } else if (e.KeyCode == Keys.Subtract) {
                zoomOut_Click(sender, EventArgs.Empty);
            } else if (e.KeyCode == Keys.X) {
                close_Click(sender, EventArgs.Empty);
            }
        }

        private void palette_SelectedIndexChanged(object sender, EventArgs e) {
            var p = (ColorPalette)palette.SelectedItem;
            PixelColor.PaletteSelection = p;
            InvokeStart();
        }

        private void juliaSelect_ValueChanged(object sender, EventArgs e) {
            int item = (int)juliaSelect.Value;
            juliaStart = JuliaStarts[item];
            juliaX.Value = (Decimal)juliaStart.X;
            juliaY.Value = (Decimal)juliaStart.Y;
            InvokeStart();
        }

        private void mandelbrotJuliaSelected_CheckedChanged(object sender, EventArgs e) {
            julia = !mandelbrotSelected.Checked;
            juliaSelect.Enabled = julia;
            juliaX.Enabled = julia;
            juliaY.Enabled = julia;
            InvokeStart();
        }

        private void InvokeStart() {
            if (IsHandleCreated) {
                BeginInvoke(_start, EventArgs.Empty);
            }
        }

        private void juliaX_ValueChanged(object sender, EventArgs e) {
            juliaStart.X = (Double)juliaX.Value;
            InvokeStart();
        }

        private void juliaY_ValueChanged(object sender, EventArgs e) {
            juliaStart.Y = (Double)juliaY.Value;
            InvokeStart();
        }

        
    }
}
