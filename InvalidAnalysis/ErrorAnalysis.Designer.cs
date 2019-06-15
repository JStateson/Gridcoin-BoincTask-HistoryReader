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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvWorkUnits = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.btnShowUT = new System.Windows.Forms.Button();
            this.lbVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecsToRead)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            this.btnStart.Location = new System.Drawing.Point(11, 127);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbInfo
            // 
            this.tbInfo.Location = new System.Drawing.Point(11, 251);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbInfo.Size = new System.Drawing.Size(251, 175);
            this.tbInfo.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(250, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Informaton, warnings and error messages in this box";
            // 
            // nudRecsToRead
            // 
            this.nudRecsToRead.Location = new System.Drawing.Point(156, 90);
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
            this.label3.Location = new System.Drawing.Point(12, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Number of records to read";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvWorkUnits);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnShowUT);
            this.groupBox1.Location = new System.Drawing.Point(301, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(329, 377);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Treeview";
            // 
            // lvWorkUnits
            // 
            this.lvWorkUnits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
            this.lvWorkUnits.HideSelection = false;
            this.lvWorkUnits.Location = new System.Drawing.Point(39, 141);
            this.lvWorkUnits.MultiSelect = false;
            this.lvWorkUnits.Name = "lvWorkUnits";
            this.lvWorkUnits.Size = new System.Drawing.Size(214, 215);
            this.lvWorkUnits.TabIndex = 3;
            this.lvWorkUnits.UseCompatibleStateImageBehavior = false;
            this.lvWorkUnits.View = System.Windows.Forms.View.Details;
            this.lvWorkUnits.SelectedIndexChanged += new System.EventHandler(this.lvWorkUnits_SelectedIndexChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Click to view workunit at site";
            this.columnHeader4.Width = 194;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(188, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Click work unit to see the project page";
            // 
            // btnShowUT
            // 
            this.btnShowUT.Enabled = false;
            this.btnShowUT.Location = new System.Drawing.Point(39, 43);
            this.btnShowUT.Name = "btnShowUT";
            this.btnShowUT.Size = new System.Drawing.Size(109, 23);
            this.btnShowUT.TabIndex = 0;
            this.btnShowUT.Text = "Show User Tasks";
            this.btnShowUT.UseVisualStyleBackColor = true;
            this.btnShowUT.Click += new System.EventHandler(this.btnShowUT_Click);
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(12, 174);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(41, 13);
            this.lbVersion.TabIndex = 9;
            this.lbVersion.Text = "version";
            // 
            // ErrorAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 450);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudRecsToRead);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbInfo);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnViewData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProjUrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ErrorAnalysis";
            this.Text = "ErrorAnalysis";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ErrorAnalysis_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nudRecsToRead)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnShowUT;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView lvWorkUnits;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label lbVersion;
    }
}

