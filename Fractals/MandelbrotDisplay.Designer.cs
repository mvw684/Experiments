namespace Fractals {
    partial class MandelbrotDisplay {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            if (disposing) {
                if (root != null) {
                    root.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.start = new System.Windows.Forms.Button();
            this.stop = new System.Windows.Forms.Button();
            this.reset = new System.Windows.Forms.Button();
            this.previous = new System.Windows.Forms.Button();
            this.save = new System.Windows.Forms.Button();
            this.up = new System.Windows.Forms.Button();
            this.left = new System.Windows.Forms.Button();
            this.right = new System.Windows.Forms.Button();
            this.down = new System.Windows.Forms.Button();
            this.zoomIn = new System.Windows.Forms.Button();
            this.zoomOut = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.palette = new System.Windows.Forms.ComboBox();
            this.juliaSelect = new System.Windows.Forms.NumericUpDown();
            this.mandelbrotSelected = new System.Windows.Forms.RadioButton();
            this.juliaSelection = new System.Windows.Forms.RadioButton();
            this.juliaX = new System.Windows.Forms.NumericUpDown();
            this.juliaY = new System.Windows.Forms.NumericUpDown();
            this.display = new Fractals.BitmapDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.juliaSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.juliaX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.juliaY)).BeginInit();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(3, 12);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 23);
            this.start.TabIndex = 1;
            this.start.TabStop = false;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(3, 42);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(75, 23);
            this.stop.TabIndex = 2;
            this.stop.TabStop = false;
            this.stop.Text = "Stop";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // reset
            // 
            this.reset.Location = new System.Drawing.Point(3, 107);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(75, 23);
            this.reset.TabIndex = 3;
            this.reset.TabStop = false;
            this.reset.Text = "Reset";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // previous
            // 
            this.previous.Location = new System.Drawing.Point(3, 72);
            this.previous.Name = "previous";
            this.previous.Size = new System.Drawing.Size(75, 23);
            this.previous.TabIndex = 4;
            this.previous.TabStop = false;
            this.previous.Text = "Previous";
            this.previous.UseVisualStyleBackColor = true;
            this.previous.Click += new System.EventHandler(this.previous_Click);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(3, 136);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 5;
            this.save.TabStop = false;
            this.save.Text = "Save ...";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // up
            // 
            this.up.Location = new System.Drawing.Point(27, 169);
            this.up.Name = "up";
            this.up.Size = new System.Drawing.Size(22, 23);
            this.up.TabIndex = 6;
            this.up.TabStop = false;
            this.up.Text = "↑";
            this.up.UseVisualStyleBackColor = true;
            this.up.Click += new System.EventHandler(this.up_Click);
            // 
            // left
            // 
            this.left.Location = new System.Drawing.Point(6, 191);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(22, 23);
            this.left.TabIndex = 7;
            this.left.TabStop = false;
            this.left.Text = "←";
            this.left.UseVisualStyleBackColor = true;
            this.left.Click += new System.EventHandler(this.left_Click);
            // 
            // right
            // 
            this.right.Location = new System.Drawing.Point(48, 191);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(22, 23);
            this.right.TabIndex = 8;
            this.right.TabStop = false;
            this.right.Text = "→";
            this.right.UseVisualStyleBackColor = true;
            this.right.Click += new System.EventHandler(this.right_Click);
            // 
            // down
            // 
            this.down.Location = new System.Drawing.Point(26, 212);
            this.down.Name = "down";
            this.down.Size = new System.Drawing.Size(22, 23);
            this.down.TabIndex = 9;
            this.down.TabStop = false;
            this.down.Text = "↓";
            this.down.UseVisualStyleBackColor = true;
            this.down.Click += new System.EventHandler(this.down_Click);
            // 
            // zoomIn
            // 
            this.zoomIn.Location = new System.Drawing.Point(48, 238);
            this.zoomIn.Name = "zoomIn";
            this.zoomIn.Size = new System.Drawing.Size(22, 23);
            this.zoomIn.TabIndex = 10;
            this.zoomIn.TabStop = false;
            this.zoomIn.Text = "+";
            this.zoomIn.UseVisualStyleBackColor = true;
            this.zoomIn.Click += new System.EventHandler(this.zoomIn_Click);
            // 
            // zoomOut
            // 
            this.zoomOut.Location = new System.Drawing.Point(6, 238);
            this.zoomOut.Name = "zoomOut";
            this.zoomOut.Size = new System.Drawing.Size(21, 23);
            this.zoomOut.TabIndex = 11;
            this.zoomOut.Text = "-";
            this.zoomOut.UseVisualStyleBackColor = true;
            this.zoomOut.Click += new System.EventHandler(this.zoomOut_Click);
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.close.Location = new System.Drawing.Point(51, 429);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(25, 23);
            this.close.TabIndex = 12;
            this.close.TabStop = false;
            this.close.Text = "X";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // palette
            // 
            this.palette.DropDownWidth = 150;
            this.palette.FormattingEnabled = true;
            this.palette.Location = new System.Drawing.Point(2, 269);
            this.palette.Name = "palette";
            this.palette.Size = new System.Drawing.Size(72, 21);
            this.palette.TabIndex = 13;
            this.palette.SelectedIndexChanged += new System.EventHandler(this.palette_SelectedIndexChanged);
            // 
            // juliaSelect
            // 
            this.juliaSelect.Location = new System.Drawing.Point(6, 346);
            this.juliaSelect.Name = "juliaSelect";
            this.juliaSelect.Size = new System.Drawing.Size(72, 20);
            this.juliaSelect.TabIndex = 15;
            this.juliaSelect.ValueChanged += new System.EventHandler(this.juliaSelect_ValueChanged);
            // 
            // mandelbrotSelected
            // 
            this.mandelbrotSelected.AutoSize = true;
            this.mandelbrotSelected.Location = new System.Drawing.Point(4, 304);
            this.mandelbrotSelected.Name = "mandelbrotSelected";
            this.mandelbrotSelected.Size = new System.Drawing.Size(78, 17);
            this.mandelbrotSelected.TabIndex = 16;
            this.mandelbrotSelected.TabStop = true;
            this.mandelbrotSelected.Text = "Mandelbrot";
            this.mandelbrotSelected.UseVisualStyleBackColor = true;
            this.mandelbrotSelected.CheckedChanged += new System.EventHandler(this.mandelbrotJuliaSelected_CheckedChanged);
            // 
            // juliaSelection
            // 
            this.juliaSelection.AutoSize = true;
            this.juliaSelection.Location = new System.Drawing.Point(4, 321);
            this.juliaSelection.Name = "juliaSelection";
            this.juliaSelection.Size = new System.Drawing.Size(46, 17);
            this.juliaSelection.TabIndex = 17;
            this.juliaSelection.TabStop = true;
            this.juliaSelection.Text = "Julia";
            this.juliaSelection.UseVisualStyleBackColor = true;
            this.juliaSelection.CheckedChanged += new System.EventHandler(this.mandelbrotJuliaSelected_CheckedChanged);
            // 
            // juliaX
            // 
            this.juliaX.DecimalPlaces = 5;
            this.juliaX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.juliaX.Location = new System.Drawing.Point(6, 375);
            this.juliaX.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.juliaX.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            -2147483648});
            this.juliaX.Name = "juliaX";
            this.juliaX.Size = new System.Drawing.Size(72, 20);
            this.juliaX.TabIndex = 18;
            this.juliaX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.juliaX.ValueChanged += new System.EventHandler(this.juliaX_ValueChanged);
            // 
            // juliaY
            // 
            this.juliaY.DecimalPlaces = 5;
            this.juliaY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.juliaY.Location = new System.Drawing.Point(6, 399);
            this.juliaY.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.juliaY.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            -2147483648});
            this.juliaY.Name = "juliaY";
            this.juliaY.Size = new System.Drawing.Size(72, 20);
            this.juliaY.TabIndex = 19;
            this.juliaY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.juliaY.ValueChanged += new System.EventHandler(this.juliaY_ValueChanged);
            // 
            // display
            // 
            this.display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.display.Changed = false;
            this.display.Location = new System.Drawing.Point(84, 0);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(534, 497);
            this.display.TabIndex = 0;
            this.display.NewSelection += new System.EventHandler<Fractals.NewSelectionEventArgs>(this.display_NewSelection);
            this.display.KeyUp += new System.Windows.Forms.KeyEventHandler(this.display_KeyUp);
            this.display.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.display_MouseDoubleClick);
            this.display.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.display_MouseWheel);
            // 
            // MandelbrotDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 495);
            this.Controls.Add(this.juliaY);
            this.Controls.Add(this.juliaX);
            this.Controls.Add(this.juliaSelection);
            this.Controls.Add(this.mandelbrotSelected);
            this.Controls.Add(this.juliaSelect);
            this.Controls.Add(this.palette);
            this.Controls.Add(this.close);
            this.Controls.Add(this.zoomOut);
            this.Controls.Add(this.zoomIn);
            this.Controls.Add(this.down);
            this.Controls.Add(this.right);
            this.Controls.Add(this.left);
            this.Controls.Add(this.up);
            this.Controls.Add(this.save);
            this.Controls.Add(this.previous);
            this.Controls.Add(this.reset);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.start);
            this.Controls.Add(this.display);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "MandelbrotDisplay";
            this.Text = "MandelbrotDisplay";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.SizeChanged += new System.EventHandler(this.MandelbrotDisplay_SizeChanged);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.display_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.juliaSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.juliaX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.juliaY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BitmapDisplay display;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.Button reset;
        private System.Windows.Forms.Button previous;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button up;
        private System.Windows.Forms.Button left;
        private System.Windows.Forms.Button right;
        private System.Windows.Forms.Button down;
        private System.Windows.Forms.Button zoomIn;
        private System.Windows.Forms.Button zoomOut;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.ComboBox palette;
        private System.Windows.Forms.NumericUpDown juliaSelect;
        private System.Windows.Forms.RadioButton mandelbrotSelected;
        private System.Windows.Forms.RadioButton juliaSelection;
        private System.Windows.Forms.NumericUpDown juliaX;
        private System.Windows.Forms.NumericUpDown juliaY;
    }
}