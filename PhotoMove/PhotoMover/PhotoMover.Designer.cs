namespace PhotoMover {
    partial class PhotoMover {
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
            this.lSource = new System.Windows.Forms.Label();
            this.lTarget = new System.Windows.Forms.Label();
            this.source = new System.Windows.Forms.ComboBox();
            this.target = new System.Windows.Forms.ComboBox();
            this.execute = new System.Windows.Forms.Button();
            this.log = new System.Windows.Forms.TextBox();
            this.abort = new System.Windows.Forms.Button();
            this.fake = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lSource
            // 
            this.lSource.AutoSize = true;
            this.lSource.Location = new System.Drawing.Point(1, 9);
            this.lSource.Name = "lSource";
            this.lSource.Size = new System.Drawing.Size(41, 13);
            this.lSource.TabIndex = 0;
            this.lSource.Text = "Source";
            // 
            // lTarget
            // 
            this.lTarget.AutoSize = true;
            this.lTarget.Location = new System.Drawing.Point(2, 38);
            this.lTarget.Name = "lTarget";
            this.lTarget.Size = new System.Drawing.Size(38, 13);
            this.lTarget.TabIndex = 1;
            this.lTarget.Text = "Target";
            // 
            // source
            // 
            this.source.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.source.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.source.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.source.FormattingEnabled = true;
            this.source.Location = new System.Drawing.Point(44, 6);
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(671, 21);
            this.source.TabIndex = 2;
            // 
            // target
            // 
            this.target.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.target.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.target.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.target.FormattingEnabled = true;
            this.target.Location = new System.Drawing.Point(44, 35);
            this.target.Name = "target";
            this.target.Size = new System.Drawing.Size(671, 21);
            this.target.TabIndex = 3;
            // 
            // execute
            // 
            this.execute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.execute.Location = new System.Drawing.Point(719, 5);
            this.execute.Name = "execute";
            this.execute.Size = new System.Drawing.Size(54, 23);
            this.execute.TabIndex = 4;
            this.execute.Text = "&Execute";
            this.execute.UseVisualStyleBackColor = true;
            this.execute.Click += new System.EventHandler(this.Execute);
            // 
            // log
            // 
            this.log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.log.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.log.Location = new System.Drawing.Point(3, 69);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.log.Size = new System.Drawing.Size(798, 379);
            this.log.TabIndex = 5;
            // 
            // abort
            // 
            this.abort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.abort.Location = new System.Drawing.Point(719, 33);
            this.abort.Name = "abort";
            this.abort.Size = new System.Drawing.Size(54, 23);
            this.abort.TabIndex = 6;
            this.abort.Text = "&Abort";
            this.abort.UseVisualStyleBackColor = true;
            this.abort.Click += new System.EventHandler(this.Abort);
            // 
            // fake
            // 
            this.fake.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fake.Location = new System.Drawing.Point(779, 3);
            this.fake.Name = "fake";
            this.fake.Size = new System.Drawing.Size(22, 60);
            this.fake.TabIndex = 7;
            this.fake.Text = "&Fake";
            this.fake.UseVisualStyleBackColor = true;
            this.fake.Click += new System.EventHandler(this.Fake);
            // 
            // PhotoMover
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 450);
            this.Controls.Add(this.fake);
            this.Controls.Add(this.abort);
            this.Controls.Add(this.log);
            this.Controls.Add(this.execute);
            this.Controls.Add(this.target);
            this.Controls.Add(this.source);
            this.Controls.Add(this.lTarget);
            this.Controls.Add(this.lSource);
            this.Name = "PhotoMover";
            this.Text = "PhotoMover";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lSource;
        private System.Windows.Forms.Label lTarget;
        private System.Windows.Forms.ComboBox source;
        private System.Windows.Forms.ComboBox target;
        private System.Windows.Forms.Button execute;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.Button abort;
        private System.Windows.Forms.Button fake;
    }
}

