namespace Fractals
{
    partial class FractalControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FractalControl));
            this.engines = new System.Windows.Forms.ListBox();
            this.launch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // engines
            // 
            this.engines.FormattingEnabled = true;
            this.engines.Location = new System.Drawing.Point(3, 3);
            this.engines.Name = "engines";
            this.engines.Size = new System.Drawing.Size(428, 212);
            this.engines.Sorted = true;
            this.engines.TabIndex = 0;
            this.engines.SelectedIndexChanged += new System.EventHandler(this.engines_SelectedIndexChanged);
            this.engines.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.engines_MouseDoubleClick);
            // 
            // launch
            // 
            this.launch.Location = new System.Drawing.Point(3, 233);
            this.launch.Name = "launch";
            this.launch.Size = new System.Drawing.Size(428, 34);
            this.launch.TabIndex = 1;
            this.launch.Text = "...";
            this.launch.UseVisualStyleBackColor = true;
            this.launch.Click += new System.EventHandler(this.launch_Click);
            // 
            // FractalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 313);
            this.Controls.Add(this.launch);
            this.Controls.Add(this.engines);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FractalControl";
            this.Text = "Fractalia";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox engines;
        private System.Windows.Forms.Button launch;

    }
}

