using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Fractals {

    public partial class BitmapDisplay : UserControl {

        private volatile bool changed;
        private Action invalidateSoon;
        private int[,] bits;
        private Size bitsSize;
        private GCHandle handle;
        private Bitmap bitmap;
        
        static int offset = 0;
        
        public void ResetBitmap() {
            if (bitmap != null) {
                bitmap.Dispose();
            }
            bitmap = null;
            if (bits != null) {
                handle.Free();
                bits = null;
            }
        }

        /// <summary>
        /// The bitmap on which the algoritms will be drawing.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int[,] Bits {
            get {
                if (bits == null) {
                    bitsSize  = Size;
                    bits = new int[bitsSize.Height, bitsSize.Width];
                    handle = GCHandle.Alloc(bits, GCHandleType.Pinned);
                    for (int y = 0; y < bitsSize.Height; y++) {
                        for (int x = 0; x < bitsSize.Width; x++) {
                            int index = (offset + (x /4) + (y/4)) % PixelColor.ColorMax;
                            bits[y,x] = PixelColor.Colors[index];
                        }
                    }
                    offset++;
                    Changed = true;
                }
                return bits;
            }
        }

        public Bitmap Bitmap {
            get {
                if (bitmap == null) {
                    var b = Bits;
                    if (b != null) {
                        bitmap =
                            new Bitmap(
                                Size.Width,
                                Size.Height,
                                Size.Width*4,
                                PixelFormat.Format32bppArgb,
                                handle.AddrOfPinnedObject()
                                );
                    }
                }
                return bitmap;
            }
        }

        public Size BitsSize {
            get {
                var b = Bits;
                GC.KeepAlive(b);
                return bitsSize;
            }
        }
                              
        public BitmapDisplay() {
            InitializeComponent();
            invalidateSoon = InvalidateSoon;
        }

        public void Save() {
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Fractals");
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".bmp";
            dialog.InitialDirectory = dir;
            dialog.Filter = "Bitmap files|(*.bmp)";
            if (dialog.ShowDialog() == DialogResult.OK) {
                string file = dialog.FileName;
                file = Path.ChangeExtension(file, ".bmp");
                Bitmap.Save(file, ImageFormat.Bmp);
                Process.Start(file);
            }
        }
        /// <summary>
        /// Indication that bitmap is changed w.r.t. display and a repaint is required
        /// </summary>
        public bool Changed {
            get {
                return changed;
            }
            set {
                if (changed != value) {
                    changed  = value;
                    if (changed) {
                        if (InvokeRequired) {
                            BeginInvoke(invalidateSoon);
                        } else {
                            InvalidateSoon();
                        }
                    }
                }
            }
        }

        public bool Selecting {
            get { return selection; }
        }

        private void InvalidateSoon() {
            Invalidate();
            Update();
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
        }

        private static Pen selectionPen = new Pen(new HatchBrush(HatchStyle.Weave, Color.Red, Color.Blue), 4);
        protected override void OnPaint(PaintEventArgs e) {
            changed = false; 
            var b = Bitmap;
            // http://stackoverflow.com/questions/11020710/is-graphics-drawimage-too-slow-for-bigger-images
            if (b != null) {
                e.Graphics.CompositingMode = CompositingMode.SourceCopy;
                e.Graphics.InterpolationMode = InterpolationMode.Low;
                e.Graphics.DrawImageUnscaled(b, 0, 0);
            }
            if (selection) {
                e.Graphics.DrawRectangle(selectionPen, selectedRange);
            }
            base.OnPaint(e);
        }

        protected override void OnSizeChanged(EventArgs e) {
            //
            ResetBitmap();
            //
            base.OnSizeChanged(e);
        }

        //
        // Allow selecting a rectangle of interest to zoom into..
        //
        private bool selection;
        Point start;
        Rectangle selectedRange;

        protected override void OnKeyUp(KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                selection = false;
                Invalidate();
            }
            base.OnKeyUp(e);
        }
        
        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                selection = true;
                start.X = e.X;
                start.Y = e.Y;
                selectedRange.Location = start;
                selectedRange.Width = 0;
                selectedRange.Height = 0;
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (selection) {
                    RaiseNewSelection(selectedRange);
                    selection = false;
                    Invalidate();
                }
            }
            base.OnMouseUp(e);
        }

        public event EventHandler<NewSelectionEventArgs> NewSelection;

        private void RaiseNewSelection(Rectangle range) {
            if (range.Width > 0 && range.Height > 0) {
                var local = NewSelection;
                if (local != null) {
                    local(this, new NewSelectionEventArgs {NewSelection = range});
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (selection) {
                selectedRange.X = Math.Min(start.X, e.X);
                selectedRange.Y = Math.Min(start.Y, e.Y);
                selectedRange.Width  = Math.Abs(start.X - e.X);
                selectedRange.Height = Math.Abs(start.Y - e.Y);
                Invalidate();
            }
            base.OnMouseMove(e);
        }
    }

    public class NewSelectionEventArgs : EventArgs {
        public Rectangle NewSelection { get; set; }
    }

    
}
