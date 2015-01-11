namespace Fractals {
    partial class LifeDisplay {
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
            this.close = new System.Windows.Forms.Button();
            this.generations = new System.Windows.Forms.Label();
            this.framesPerSecond = new System.Windows.Forms.Label();
            this.clear = new System.Windows.Forms.Button();
            this.fill = new System.Windows.Forms.Button();
            this.step = new System.Windows.Forms.Button();
            this.bitmap = new Fractals.BitmapDisplay();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(1, 2);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(51, 23);
            this.start.TabIndex = 0;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(58, 2);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(46, 23);
            this.stop.TabIndex = 1;
            this.stop.Text = "Stop";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.close.Location = new System.Drawing.Point(591, 2);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(31, 23);
            this.close.TabIndex = 2;
            this.close.Text = "X";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // generations
            // 
            this.generations.Location = new System.Drawing.Point(148, 3);
            this.generations.Name = "generations";
            this.generations.Size = new System.Drawing.Size(88, 22);
            this.generations.TabIndex = 4;
            this.generations.Text = "generations";
            this.generations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // framesPerSecond
            // 
            this.framesPerSecond.Location = new System.Drawing.Point(242, 3);
            this.framesPerSecond.Name = "framesPerSecond";
            this.framesPerSecond.Size = new System.Drawing.Size(68, 22);
            this.framesPerSecond.TabIndex = 5;
            this.framesPerSecond.Text = "framerate";
            this.framesPerSecond.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // clear
            // 
            this.clear.Location = new System.Drawing.Point(316, 3);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(75, 23);
            this.clear.TabIndex = 6;
            this.clear.Text = "Clear";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // fill
            // 
            this.fill.Location = new System.Drawing.Point(395, 3);
            this.fill.Name = "fill";
            this.fill.Size = new System.Drawing.Size(75, 23);
            this.fill.TabIndex = 7;
            this.fill.Text = "Fill";
            this.fill.UseVisualStyleBackColor = true;
            this.fill.Click += new System.EventHandler(this.fill_Click);
            // 
            // step
            // 
            this.step.Location = new System.Drawing.Point(111, 2);
            this.step.Name = "step";
            this.step.Size = new System.Drawing.Size(48, 23);
            this.step.TabIndex = 8;
            this.step.Text = "Step";
            this.step.UseVisualStyleBackColor = true;
            this.step.Click += new System.EventHandler(this.step_Click);
            // 
            // bitmap
            // 
            this.bitmap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bitmap.Changed = false;
            this.bitmap.Location = new System.Drawing.Point(1, 32);
            this.bitmap.Name = "bitmap";
            this.bitmap.Size = new System.Drawing.Size(633, 451);
            this.bitmap.TabIndex = 3;
            this.bitmap.NewSelection += new System.EventHandler<Fractals.NewSelectionEventArgs>(this.bitmap_NewSelection);
            this.bitmap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bitmap_MouseClick);
            this.bitmap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bitmap_MouseDown);
            this.bitmap.Resize += new System.EventHandler(this.bitmap_Resize);
            // 
            // LifeDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 486);
            this.Controls.Add(this.step);
            this.Controls.Add(this.fill);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.framesPerSecond);
            this.Controls.Add(this.generations);
            this.Controls.Add(this.bitmap);
            this.Controls.Add(this.close);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LifeDisplay";
            this.Text = "LifeDisplay";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.Button close;
        private BitmapDisplay bitmap;
        private System.Windows.Forms.Label generations;
        private System.Windows.Forms.Label framesPerSecond;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.Button fill;
        private System.Windows.Forms.Button step;
    }
}