using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractals {
    public partial class LifeDisplay : Form {

        private int[,] field1, field2;
        private const int scaleFactor = 2;
        const double fps = 30;

        private Stopwatch stopwatch = new Stopwatch();
        private bool stopRequest = true;
        private object lockObject = new object();
        private long generation;
        private long frames;
        private int width;
        private int height;
        private ContextMenu addMenu;
        private readonly EventHandler _statistics;

        private void SetState(bool stopped) {
            if (stopped) {
                stopwatch.Stop();
            } else {
                stopwatch.Start();
            }
            start.Enabled = stopped;
            stop.Enabled = !stopped;
            clear.Enabled = stopped;
            fill.Enabled = stopped;
            step.Enabled = stopped;
        }

        public LifeDisplay() {
            _statistics = UpdateStatisticsUI;
            InitializeComponent();
            addMenu = new ContextMenu();
            addMenu.MenuItems.Add("Blinker Horizontal").Click += SetBlinkerHorizontal;
            addMenu.MenuItems.Add("Blinker Vertical").Click += SetBlinkerVertical;
            addMenu.MenuItems.Add("Glider Right Down").Click += SetGliderRightDown;
            addMenu.MenuItems.Add("Glider Left Down").Click += SetGliderLeftDown;
            addMenu.MenuItems.Add("Glider Right Up").Click += SetGliderRightUp;
            addMenu.MenuItems.Add("Glider Left Up").Click += SetGliderLeftUp;
            addMenu.MenuItems.Add("Gosper slider gun").Click += SetGosperSliderGun;
            
            bitmap.ContextMenu = addMenu;
            SetState(true);
            PixelColor.PaletteSelection = ColorPalette.RainBow;
            clear_Click(this, EventArgs.Empty);
        }

        private void close_Click(object sender, EventArgs e) {
            Close();
        }

        private void start_Click(object sender, EventArgs e) {
            SetState(false);
            lock (lockObject) {
                stopRequest = false;
                frames = 0;
                stopwatch.Restart();
                Task t = new Task(Compute);
                t.Start(Scheduler.Instance);
            }
        }

        private void stop_Click(object sender, EventArgs e) {
            lock (lockObject) {
                stopRequest = true;
                Monitor.Pulse(lockObject);
                Monitor.Wait(lockObject);
            }
            SetState(true);
        }

        private void clear_Click(object sender, EventArgs e) {
            int[,] bits = bitmap.Bits;
            Size s = bitmap.BitsSize;
            for (int y = 0; y < s.Height; y++) {
                for (int x = 0; x < s.Width; x++) {
                    bits[y, x] = PixelColor.Black;
                }
            }
            bitmap.Changed = true;
            width = s.Width / scaleFactor;
            height = s.Height / scaleFactor;

            field1 = new int[height, width];
            field2 = new int[height, width];
        }

        private void fill_Click(object sender, EventArgs e) {
            for (int i = 0; i < 1000; i++) {
                int x = AlmostRandom.Next(width);
                int y = AlmostRandom.Next(height);
                Set(x, y, false);
            }
            bitmap.Changed = true;
        }

        private void bitmap_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                int x = e.X / scaleFactor;
                int y = e.Y / scaleFactor;
                Set(x, y, ModifierKeys == Keys.Shift);
                bitmap.Changed = true;
            }
        }

        private void bitmap_NewSelection(object sender, NewSelectionEventArgs e) {
            int sx = e.NewSelection.Left / scaleFactor;
            int sy = e.NewSelection.Top / scaleFactor;
            int sWidth = (e.NewSelection.Width + scaleFactor -1) / scaleFactor;
            int sHeight = (e.NewSelection.Height + scaleFactor -1) / scaleFactor;

            for (int y = 0; y < sHeight; y++) {
                for (int x = 0; x < sWidth; x++) {
                    Set(sx+x, sy+y, ModifierKeys == Keys.Shift);
                }
            }
            bitmap.Changed = true;
        }

        private int[,] blinkerHorizontal = {
            {1, 1, 1}
        };

        private int[,] blinkerVertical = {
            {1},
            {1},
            {1}
        };

        private int[,] gliderRightDown = {
            {0, 1, 0},
            {0, 0, 1},
            {1, 1, 1}
        };

        private int[,] gliderLeftDown = {
            {0, 1, 0},
            {1, 0, 0},
            {1, 1, 1}
        };

        private int[,] gliderRightUp = {
            {1, 1, 1},
            {0, 0, 1},
            {0, 1, 0}
        };

        private int[,] gliderLeftUp = {
            {1, 1, 1},
            {1, 0, 0},
            {0, 1, 0}
        };

        private int[,] gosperGliderGun = {
         //  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5  6  7  8  9  0  1  1  1  1  2  3
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
            {1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };                                                                                                

        private int sx;
        private int sy;

        private void SetGosperSliderGun(object sender, EventArgs args) {
            DisplayItem(gosperGliderGun);
        }

        private void SetGliderLeftUp(object sender, EventArgs args) {
            DisplayItem(gliderLeftUp);
        }

        private void SetGliderRightUp(object sender, EventArgs args) {
            DisplayItem(gliderRightUp);
        }

        private void SetGliderRightDown(object sender, EventArgs args) {
            DisplayItem(gliderRightDown);
        }

        private void SetGliderLeftDown(object sender, EventArgs args) {
            DisplayItem(gliderLeftDown);
        }

        private void SetBlinkerHorizontal(object sender, EventArgs args) {
            DisplayItem(blinkerHorizontal);
        }

        private void SetBlinkerVertical(object sender, EventArgs args) {
            DisplayItem(blinkerVertical);
        }

        private void DisplayItem(int[,] item) {
            int itemWidth = item.GetLength(1);
            int itemHeight = item.GetLength(0);
            for (int y = 0; y < itemHeight; y++) {
                for (int x = 0; x < itemWidth; x++) {
                    Set(sx + x, sy + y, item[y,x] == 0);
                }
            }
            bitmap.Changed = true;
        }

        private void bitmap_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                sx = e.X / scaleFactor;
                sy = e.Y / scaleFactor;
                //var org = bitmap.ContextMenu;
                //bitmap.ContextMenu = m;
                addMenu.Show((Control) sender, e.Location);
            }
        }

        private void Set(int x, int y, bool reset) {
            if (x >= width) {
                return;
            }
            if (y >= height) {
                return;
            }
            int[,] bits = bitmap.Bits;
            int val = field1[y, x];
            if (reset) {
                val = 0;
            } else {
                val += 15;
                val %= PixelColor.ColorLast;
            }
            field1[y, x] = val;

            int color = PixelColor.Colors[val % PixelColor.ColorMax];
            int x2 = x * scaleFactor;
            int y2 = y * scaleFactor;
            for (int y1 = 0; y1 < scaleFactor; y1++) {
                for (int x1 = 0; x1 < scaleFactor; x1++) {
                    bits[y2 + y1, x2 + x1] = color;
                }
            }
        }

        private void Blit(int[,] field) {
            int[,] bits = bitmap.Bits;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int val = field[y, x];
                    int color = PixelColor.Colors[val % PixelColor.ColorLast];
                    int x2 = x * scaleFactor;
                    int y2 = y * scaleFactor;
                    for (int y1 = 0; y1 < scaleFactor; y1++) {
                        for (int x1 = 0; x1 < scaleFactor; x1++) {
                            bits[y2 + y1, x2 + x1] = color;
                        }
                    }
                }
            }
            bitmap.Changed = true;
        }

        private void bitmap_Resize(object sender, EventArgs e) {
            clear_Click(sender, e);
        }

        private void Compute() {
            try {
                Stopwatch fpsWatch = new Stopwatch();
                for (;;) {
                    fpsWatch.Restart();
                    lock (lockObject) {
                        if (stopRequest) {
                            stopRequest = false;
                            Monitor.Pulse(lockObject);
                            return;
                        }
                    }
                    ComputeStep();
                    UpdateStatistics();
                    TimeSpan towait = TimeSpan.FromSeconds(1.0 / fps) - fpsWatch.Elapsed;
                    if (towait.TotalMilliseconds > 0) {
                        Thread.Sleep(TimeSpan.FromSeconds(1.0 / fps) - fpsWatch.Elapsed);
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                string m = e.Message;
                Debugger.Launch();
            }

        }

        private void ComputeStep() {
            Interlocked.Increment(ref generation);
            Interlocked.Increment(ref frames);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    field2[y, x] = 0;
                }
            }

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int check = 0;
                    int colorSum = 0;
                    int val;

                    #region y1 = -1

                    #region x1 = -1

                    int xx = (x + -1 + width) % width;
                    int yy = (y + -1 + height) % height;
                    val = field1[yy, xx];
                    if (val > 0) {
                        check++;
                    }
                    colorSum += val;

                    #endregion

                    #region x1 = 0

                    xx = (x + 0 + width) % width;
                    yy = (y + -1 + height) % height;
                    val = field1[yy, xx];
                    if (val > 0) {
                        check++;
                    }
                    colorSum += val;

                    #endregion

                    #region x1 = 1

                    xx = (x + 1 + width) % width;
                    yy = (y + -1 + height) % height;
                    val = field1[yy, xx];
                    if (val > 0) {
                        check++;
                    }
                    colorSum += val;

                    #endregion

                    #endregion


                    #region y1 = 0

                    #region x1 = -1

                    xx = (x + -1 + width) % width;
                    yy = (y + 0 + height) % height;
                    val = field1[yy, xx];
                    if (val > 0) {
                        check++;
                    }
                    colorSum += val;

                    #endregion

                    #region x1 = 1

                    xx = (x + 1 + width) % width;
                    yy = (y + 0 + height) % height;
                    val = field1[yy, xx];
                    if (val > 0) {
                        check++;
                    }
                    colorSum += val;

                    #endregion

                    #endregion

                    #region y1 = 1

                    #region x1 = -1

                    xx = (x + -1 + width) % width;
                    yy = (y + 1 + height) % height;
                    val = field1[yy, xx];
                    if (val > 0) {
                        check++;
                    }
                    colorSum += val;

                    #endregion

                    #region x1 = 0

                    xx = (x + 0 + width) % width;
                    yy = (y + 1 + height) % height;
                    val = field1[yy, xx];
                    if (val > 0) {
                        check++;
                    }
                    colorSum += val;

                    #endregion

                    #region x1 = 1

                    xx = (x + 1 + width) % width;
                    yy = (y + 1 + height) % height;
                    val = field1[yy, xx];
                    if (val > 0) {
                        check++;
                    }
                    colorSum += val;

                    #endregion

                    #endregion

                    val = field1[y, x];
                    if (check == 3) {
                        if (val > 0) {
                            //val = (val + 1) % PixelColor.ColorLast;
                            //if (val == 0) {
                            //    val = 1;
                            //}
                            field2[y, x] = val;
                        } else {
                            field2[y, x] = 6 + (colorSum / check) % PixelColor.ColorLast;
                        }
                    } else if (check == 2) {
                        if (val > 0) {
                            //val = (val + 1) % PixelColor.ColorLast;
                            //if (val == 0) {
                            //    val = 1;
                            //}
                            field2[y, x] = val;
                        }
                    }
                }
            }
            var temp = field1;
            field1 = field2;
            field2 = temp;
            Blit(field1);
        }

        private volatile bool updating;

        private void UpdateStatistics() {
            if (updating) {
                return;
            }
            updating = true;
            BeginInvoke(_statistics, this, EventArgs.Empty);
        }

        private void UpdateStatisticsUI(object sender, EventArgs args) {
            long gen = Interlocked.Read(ref generation);
            long frame = Interlocked.Read(ref frames);
            generations.Text = gen.ToString("####,###,###");
            framesPerSecond.Text = (frame / stopwatch.Elapsed.TotalSeconds).ToString("####,##0.00");
            //generations.Invalidate();
            //framesPerSecond.Invalidate();
            generations.Update();
            framesPerSecond.Update();
            updating = false;
        }

        private void step_Click(object sender, EventArgs e) {
            ComputeStep();
        }

        
    }
}
