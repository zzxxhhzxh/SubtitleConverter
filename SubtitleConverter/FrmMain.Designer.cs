namespace SubtitleConverter
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtSFile = new System.Windows.Forms.TextBox();
            this.txtDFile = new System.Windows.Forms.TextBox();
            this.lblTxt = new System.Windows.Forms.Label();
            this.lvwSub = new System.Windows.Forms.ListView();
            this.colIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colETime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nudDelay = new System.Windows.Forms.NumericUpDown();
            this.lblMs = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(547, 7);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(547, 46);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // txtSFile
            // 
            this.txtSFile.Location = new System.Drawing.Point(79, 9);
            this.txtSFile.Name = "txtSFile";
            this.txtSFile.Size = new System.Drawing.Size(462, 20);
            this.txtSFile.TabIndex = 3;
            // 
            // txtDFile
            // 
            this.txtDFile.Location = new System.Drawing.Point(79, 48);
            this.txtDFile.Name = "txtDFile";
            this.txtDFile.Size = new System.Drawing.Size(462, 20);
            this.txtDFile.TabIndex = 4;
            // 
            // lblTxt
            // 
            this.lblTxt.AutoSize = true;
            this.lblTxt.Location = new System.Drawing.Point(13, 12);
            this.lblTxt.Name = "lblTxt";
            this.lblTxt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblTxt.Size = new System.Drawing.Size(60, 91);
            this.lblTxt.TabIndex = 5;
            this.lblTxt.Text = "Open File\r\n\r\n\r\nSave File\r\n\r\n\r\nDelay Time";
            this.lblTxt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lvwSub
            // 
            this.lvwSub.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIndex,
            this.colSTime,
            this.colETime,
            this.colContent});
            this.lvwSub.GridLines = true;
            this.lvwSub.HideSelection = false;
            this.lvwSub.Location = new System.Drawing.Point(12, 111);
            this.lvwSub.Name = "lvwSub";
            this.lvwSub.Size = new System.Drawing.Size(610, 438);
            this.lvwSub.TabIndex = 6;
            this.lvwSub.UseCompatibleStateImageBehavior = false;
            this.lvwSub.View = System.Windows.Forms.View.Details;
            // 
            // colIndex
            // 
            this.colIndex.Text = "Index";
            // 
            // colSTime
            // 
            this.colSTime.Text = "Start Time";
            this.colSTime.Width = 80;
            // 
            // colETime
            // 
            this.colETime.Text = "End Time";
            this.colETime.Width = 80;
            // 
            // colContent
            // 
            this.colContent.Text = "Content";
            this.colContent.Width = 380;
            // 
            // nudDelay
            // 
            this.nudDelay.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudDelay.Location = new System.Drawing.Point(80, 85);
            this.nudDelay.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.nudDelay.Minimum = new decimal(new int[] {
            600000,
            0,
            0,
            -2147483648});
            this.nudDelay.Name = "nudDelay";
            this.nudDelay.Size = new System.Drawing.Size(120, 20);
            this.nudDelay.TabIndex = 7;
            this.nudDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudDelay.ThousandsSeparator = true;
            // 
            // lblMs
            // 
            this.lblMs.AutoSize = true;
            this.lblMs.Location = new System.Drawing.Point(206, 90);
            this.lblMs.Name = "lblMs";
            this.lblMs.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMs.Size = new System.Drawing.Size(20, 13);
            this.lblMs.TabIndex = 8;
            this.lblMs.Text = "ms";
            this.lblMs.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 561);
            this.Controls.Add(this.lblMs);
            this.Controls.Add(this.nudDelay);
            this.Controls.Add(this.lvwSub);
            this.Controls.Add(this.lblTxt);
            this.Controls.Add(this.txtDFile);
            this.Controls.Add(this.txtSFile);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOpen);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.Text = "Subtitle Converter";
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtSFile;
        private System.Windows.Forms.TextBox txtDFile;
        private System.Windows.Forms.Label lblTxt;
        private System.Windows.Forms.ListView lvwSub;
        private System.Windows.Forms.NumericUpDown nudDelay;
        private System.Windows.Forms.Label lblMs;
        private System.Windows.Forms.ColumnHeader colIndex;
        private System.Windows.Forms.ColumnHeader colSTime;
        private System.Windows.Forms.ColumnHeader colETime;
        private System.Windows.Forms.ColumnHeader colContent;
    }
}