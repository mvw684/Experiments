namespace Fractals {
    partial class PlasmaDisplay {
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
            if (root != null) {
                root.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlasmaDisplay));
            this.randomness = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.Button();
            this.stop = new System.Windows.Forms.Button();
            this.save = new System.Windows.Forms.Button();
            this.interpolationMode = new System.Windows.Forms.ComboBox();
            this.display = new Fractals.BitmapDisplay();
            this.close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.randomness)).BeginInit();
            this.SuspendLayout();
            // 
            // randomness
            // 
            this.randomness.Location = new System.Drawing.Point(1, 37);
            this.randomness.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.randomness.Name = "randomness";
            this.randomness.Size = new System.Drawing.Size(70, 20);
            this.randomness.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Randomness";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(1, 63);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(70, 23);
            this.start.TabIndex = 3;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(1, 92);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(70, 23);
            this.stop.TabIndex = 4;
            this.stop.Text = "Stop";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(1, 193);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(70, 23);
            this.save.TabIndex = 5;
            this.save.Text = "Save ...";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // interpolationMode
            // 
            this.interpolationMode.DropDownWidth = 200;
            this.interpolationMode.FormattingEnabled = true;
            this.interpolationMode.Location = new System.Drawing.Point(1, 141);
            this.interpolationMode.Name = "interpolationMode";
            this.interpolationMode.Size = new System.Drawing.Size(70, 21);
            this.interpolationMode.Sorted = true;
            this.interpolationMode.TabIndex = 6;
            // 
            // display
            // 
            this.display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.display.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.display.Changed = false;
            this.display.Location = new System.Drawing.Point(77, 1);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(583, 505);
            this.display.TabIndex = 0;
            this.display.NewSelection += new System.EventHandler<Fractals.NewSelectionEventArgs>(this.display_NewSelection);
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.close.Location = new System.Drawing.Point(1, 425);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(24, 23);
            this.close.TabIndex = 7;
            this.close.Text = "X";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // PlasmaDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(660, 506);
            this.Controls.Add(this.close);
            this.Controls.Add(this.interpolationMode);
            this.Controls.Add(this.save);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.start);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.randomness);
            this.Controls.Add(this.display);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlasmaDisplay";
            this.ShowInTaskbar = false;
            this.Text = "PlasmaDisplay";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.randomness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BitmapDisplay display;
        private System.Windows.Forms.NumericUpDown randomness;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.ComboBox interpolationMode;
        private System.Windows.Forms.Button close;
    }
}