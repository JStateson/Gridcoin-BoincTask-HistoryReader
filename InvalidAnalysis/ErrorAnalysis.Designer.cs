namespace InvalidAnalysis
{
    partial class ErrorAnalysis
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
            this.ProjUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnViewData = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudRecsToRead = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecsToRead)).BeginInit();
            this.SuspendLayout();
            // 
            // ProjUrl
            // 
            this.ProjUrl.Location = new System.Drawing.Point(156, 9);
            this.ProjUrl.Name = "ProjUrl";
            this.ProjUrl.Size = new System.Drawing.Size(617, 20);
            this.ProjUrl.TabIndex = 0;
            this.ProjUrl.Text = "https://milkyway.cs.rpi.edu/milkyway/results.php?hostid=772622&offset=0&show_name" +
    "s=0&state=5&appid=";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "URL to project invalids";
            // 
            // btnViewData
            // 
            this.btnViewData.Location = new System.Drawing.Point(12, 49);
            this.btnViewData.Name = "btnViewData";
            this.btnViewData.Size = new System.Drawing.Size(75, 23);
            this.btnViewData.TabIndex = 2;
            this.btnViewData.Text = "View Page";
            this.btnViewData.UseVisualStyleBackColor = true;
            this.btnViewData.Click += new System.EventHandler(this.btnViewData_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 90);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbInfo
            // 
            this.tbInfo.Location = new System.Drawing.Point(483, 90);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.Size = new System.Drawing.Size(290, 175);
            this.tbInfo.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(496, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(250, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Informaton, warnings and error messages in this box";
            // 
            // nudRecsToRead
            // 
            this.nudRecsToRead.Location = new System.Drawing.Point(292, 59);
            this.nudRecsToRead.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudRecsToRead.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRecsToRead.Name = "nudRecsToRead";
            this.nudRecsToRead.Size = new System.Drawing.Size(55, 20);
            this.nudRecsToRead.TabIndex = 6;
            this.nudRecsToRead.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(135, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Number of record on page";
            // 
            // ErrorAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudRecsToRead);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbInfo);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnViewData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProjUrl);
            this.Name = "ErrorAnalysis";
            this.Text = "ErrorAnalysis";
            ((System.ComponentModel.ISupportInitialize)(this.nudRecsToRead)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ProjUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnViewData;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudRecsToRead;
        private System.Windows.Forms.Label label3;
    }
}

