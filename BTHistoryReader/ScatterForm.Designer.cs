namespace BTHistoryReader
{
    partial class ScatterForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScatterForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.lbScatUsage = new System.Windows.Forms.Label();
            this.GetLegendInfo = new System.Windows.Forms.Timer(this.components);
            this.labelShowSeries = new System.Windows.Forms.Label();
            this.nudShowOnly = new System.Windows.Forms.NumericUpDown();
            this.tboxShowing = new System.Windows.Forms.TextBox();
            this.cboxUseLog = new System.Windows.Forms.CheckBox();
            this.ChartScatter = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbOffsetValue = new System.Windows.Forms.Label();
            this.cbUseOffset = new System.Windows.Forms.CheckBox();
            this.lviewSubSeries = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gboxOutlier = new System.Windows.Forms.GroupBox();
            this.nudHideXoutliers = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lblShowApp = new System.Windows.Forms.Label();
            this.btnInvSel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lbAdvFilter = new System.Windows.Forms.Label();
            this.tbWUcnt = new System.Windows.Forms.TextBox();
            this.LBtLwuS = new System.Windows.Forms.Label();
            this.lbGPUvis = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudShowOnly)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartScatter)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.gboxOutlier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHideXoutliers)).BeginInit();
            this.SuspendLayout();
            // 
            // lbScatUsage
            // 
            this.lbScatUsage.AutoSize = true;
            this.lbScatUsage.BackColor = System.Drawing.SystemColors.Info;
            this.lbScatUsage.Location = new System.Drawing.Point(655, 506);
            this.lbScatUsage.Name = "lbScatUsage";
            this.lbScatUsage.Size = new System.Drawing.Size(291, 52);
            this.lbScatUsage.TabIndex = 1;
            this.lbScatUsage.Text = resources.GetString("lbScatUsage.Text");
            // 
            // GetLegendInfo
            // 
            this.GetLegendInfo.Enabled = true;
            this.GetLegendInfo.Interval = 250;
            this.GetLegendInfo.Tick += new System.EventHandler(this.GetLegendInfo_Tick);
            // 
            // labelShowSeries
            // 
            this.labelShowSeries.AutoSize = true;
            this.labelShowSeries.Location = new System.Drawing.Point(18, 372);
            this.labelShowSeries.Name = "labelShowSeries";
            this.labelShowSeries.Size = new System.Drawing.Size(83, 13);
            this.labelShowSeries.TabIndex = 3;
            this.labelShowSeries.Text = "SHOW SERIES";
            // 
            // nudShowOnly
            // 
            this.nudShowOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudShowOnly.Location = new System.Drawing.Point(119, 361);
            this.nudShowOnly.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudShowOnly.Name = "nudShowOnly";
            this.nudShowOnly.ReadOnly = true;
            this.nudShowOnly.Size = new System.Drawing.Size(20, 38);
            this.nudShowOnly.TabIndex = 4;
            this.toolTip1.SetToolTip(this.nudShowOnly, "Click up or down to view another series, if any");
            this.nudShowOnly.ValueChanged += new System.EventHandler(this.nudShowOnly_ValueChanged);
            // 
            // tboxShowing
            // 
            this.tboxShowing.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tboxShowing.Location = new System.Drawing.Point(157, 369);
            this.tboxShowing.Name = "tboxShowing";
            this.tboxShowing.ReadOnly = true;
            this.tboxShowing.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.tboxShowing.Size = new System.Drawing.Size(222, 20);
            this.tboxShowing.TabIndex = 5;
            this.tboxShowing.Text = "Show All";
            // 
            // cboxUseLog
            // 
            this.cboxUseLog.AutoSize = true;
            this.cboxUseLog.Location = new System.Drawing.Point(106, 22);
            this.cboxUseLog.Name = "cboxUseLog";
            this.cboxUseLog.Size = new System.Drawing.Size(108, 17);
            this.cboxUseLog.TabIndex = 8;
            this.cboxUseLog.Text = "Make X log scale";
            this.cboxUseLog.UseVisualStyleBackColor = true;
            this.cboxUseLog.CheckedChanged += new System.EventHandler(this.cboxUseLog_CheckedChanged);
            // 
            // ChartScatter
            // 
            chartArea2.Name = "ChartArea1";
            this.ChartScatter.ChartAreas.Add(chartArea2);
            legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
            legend3.IsDockedInsideChartArea = false;
            legend3.Name = "Legend1";
            legend4.DockedToChartArea = "ChartArea1";
            legend4.Name = "SysLeg";
            this.ChartScatter.Legends.Add(legend3);
            this.ChartScatter.Legends.Add(legend4);
            this.ChartScatter.Location = new System.Drawing.Point(21, 35);
            this.ChartScatter.Name = "ChartScatter";
            this.ChartScatter.Size = new System.Drawing.Size(923, 320);
            this.ChartScatter.TabIndex = 0;
            this.ChartScatter.Text = "Scatter Plot";
            this.ChartScatter.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChartScatter_MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbOffsetValue);
            this.groupBox1.Controls.Add(this.cbUseOffset);
            this.groupBox1.Controls.Add(this.cboxUseLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 451);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 87);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "X-Axis scaling";
            // 
            // lbOffsetValue
            // 
            this.lbOffsetValue.AutoSize = true;
            this.lbOffsetValue.BackColor = System.Drawing.SystemColors.Info;
            this.lbOffsetValue.Location = new System.Drawing.Point(24, 66);
            this.lbOffsetValue.Name = "lbOffsetValue";
            this.lbOffsetValue.Size = new System.Drawing.Size(65, 13);
            this.lbOffsetValue.TabIndex = 10;
            this.lbOffsetValue.Text = "Offset Value";
            // 
            // cbUseOffset
            // 
            this.cbUseOffset.AutoSize = true;
            this.cbUseOffset.Enabled = false;
            this.cbUseOffset.Location = new System.Drawing.Point(107, 45);
            this.cbUseOffset.Name = "cbUseOffset";
            this.cbUseOffset.Size = new System.Drawing.Size(107, 17);
            this.cbUseOffset.TabIndex = 9;
            this.cbUseOffset.Text = "Offset each GPU";
            this.cbUseOffset.UseVisualStyleBackColor = true;
            this.cbUseOffset.CheckedChanged += new System.EventHandler(this.cbUseOffset_CheckedChanged);
            // 
            // lviewSubSeries
            // 
            this.lviewSubSeries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lviewSubSeries.FullRowSelect = true;
            this.lviewSubSeries.HideSelection = false;
            this.lviewSubSeries.Location = new System.Drawing.Point(411, 369);
            this.lviewSubSeries.MultiSelect = false;
            this.lviewSubSeries.Name = "lviewSubSeries";
            this.lviewSubSeries.ShowGroups = false;
            this.lviewSubSeries.Size = new System.Drawing.Size(179, 97);
            this.lviewSubSeries.TabIndex = 9;
            this.toolTip1.SetToolTip(this.lviewSubSeries, "Click on column header to restore default");
            this.lviewSubSeries.UseCompatibleStateImageBehavior = false;
            this.lviewSubSeries.View = System.Windows.Forms.View.Details;
            this.lviewSubSeries.SelectedIndexChanged += new System.EventHandler(this.lviewSubSeries_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Showing all unless listed below";
            this.columnHeader1.Width = 300;
            // 
            // gboxOutlier
            // 
            this.gboxOutlier.Controls.Add(this.nudHideXoutliers);
            this.gboxOutlier.Controls.Add(this.label3);
            this.gboxOutlier.Location = new System.Drawing.Point(296, 485);
            this.gboxOutlier.Name = "gboxOutlier";
            this.gboxOutlier.Size = new System.Drawing.Size(326, 73);
            this.gboxOutlier.TabIndex = 12;
            this.gboxOutlier.TabStop = false;
            this.gboxOutlier.Text = "Hide Outliers";
            // 
            // nudHideXoutliers
            // 
            this.nudHideXoutliers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudHideXoutliers.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudHideXoutliers.Location = new System.Drawing.Point(23, 19);
            this.nudHideXoutliers.Name = "nudHideXoutliers";
            this.nudHideXoutliers.ReadOnly = true;
            this.nudHideXoutliers.Size = new System.Drawing.Size(42, 34);
            this.nudHideXoutliers.TabIndex = 12;
            this.toolTip1.SetToolTip(this.nudHideXoutliers, "Click here to remove the largest value in the\r\ndata set to allow other values to " +
        "be seen");
            this.nudHideXoutliers.ValueChanged += new System.EventHandler(this.nudHideXoutliers_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label3.Location = new System.Drawing.Point(101, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 52);
            this.label3.TabIndex = 0;
            this.label3.Text = "Occasionally a GPU runs at lowest speed\r\nand a  task that normally takes minutes\r" +
    "\nstretches into days. These are outliers: and\r\nmay cause all other data to be ob" +
    "scured.";
            // 
            // lblShowApp
            // 
            this.lblShowApp.AutoSize = true;
            this.lblShowApp.Location = new System.Drawing.Point(29, 400);
            this.lblShowApp.Name = "lblShowApp";
            this.lblShowApp.Size = new System.Drawing.Size(51, 13);
            this.lblShowApp.TabIndex = 14;
            this.lblShowApp.Text = "appname";
            // 
            // btnInvSel
            // 
            this.btnInvSel.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInvSel.Location = new System.Drawing.Point(352, 423);
            this.btnInvSel.Name = "btnInvSel";
            this.btnInvSel.Size = new System.Drawing.Size(53, 41);
            this.btnInvSel.TabIndex = 15;
            this.btnInvSel.Text = "Invert\r\nSelection";
            this.toolTip1.SetToolTip(this.btnInvSel, "click here to invert the selectoins in the box");
            this.btnInvSel.UseVisualStyleBackColor = true;
            this.btnInvSel.Click += new System.EventHandler(this.btnInvSel_Click);
            // 
            // lbAdvFilter
            // 
            this.lbAdvFilter.AutoSize = true;
            this.lbAdvFilter.BackColor = System.Drawing.SystemColors.Info;
            this.lbAdvFilter.Location = new System.Drawing.Point(12, 559);
            this.lbAdvFilter.Name = "lbAdvFilter";
            this.lbAdvFilter.Size = new System.Drawing.Size(135, 13);
            this.lbAdvFilter.TabIndex = 16;
            this.lbAdvFilter.Text = "Reserve for filter info, if any";
            // 
            // tbWUcnt
            // 
            this.tbWUcnt.Location = new System.Drawing.Point(642, 397);
            this.tbWUcnt.Multiline = true;
            this.tbWUcnt.Name = "tbWUcnt";
            this.tbWUcnt.Size = new System.Drawing.Size(302, 86);
            this.tbWUcnt.TabIndex = 17;
            // 
            // LBtLwuS
            // 
            this.LBtLwuS.AutoSize = true;
            this.LBtLwuS.BackColor = System.Drawing.SystemColors.Info;
            this.LBtLwuS.Location = new System.Drawing.Point(679, 372);
            this.LBtLwuS.Name = "LBtLwuS";
            this.LBtLwuS.Size = new System.Drawing.Size(157, 13);
            this.LBtLwuS.TabIndex = 18;
            this.LBtLwuS.Text = "Work units in above scatter plot";
            // 
            // lbGPUvis
            // 
            this.lbGPUvis.AutoSize = true;
            this.lbGPUvis.BackColor = System.Drawing.SystemColors.Info;
            this.lbGPUvis.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbGPUvis.Location = new System.Drawing.Point(154, 423);
            this.lbGPUvis.Name = "lbGPUvis";
            this.lbGPUvis.Size = new System.Drawing.Size(167, 16);
            this.lbGPUvis.TabIndex = 19;
            this.lbGPUvis.Text = "GPUs shown as marker";
            this.lbGPUvis.Visible = false;
            // 
            // ScatterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 591);
            this.Controls.Add(this.lbGPUvis);
            this.Controls.Add(this.LBtLwuS);
            this.Controls.Add(this.tbWUcnt);
            this.Controls.Add(this.lbAdvFilter);
            this.Controls.Add(this.btnInvSel);
            this.Controls.Add(this.lblShowApp);
            this.Controls.Add(this.gboxOutlier);
            this.Controls.Add(this.lviewSubSeries);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tboxShowing);
            this.Controls.Add(this.nudShowOnly);
            this.Controls.Add(this.labelShowSeries);
            this.Controls.Add(this.lbScatUsage);
            this.Controls.Add(this.ChartScatter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "ScatterForm";
            this.Text = "Scatter Plot";
            ((System.ComponentModel.ISupportInitialize)(this.nudShowOnly)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartScatter)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gboxOutlier.ResumeLayout(false);
            this.gboxOutlier.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHideXoutliers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbScatUsage;
        private System.Windows.Forms.Timer GetLegendInfo;
        private System.Windows.Forms.Label labelShowSeries;
        private System.Windows.Forms.NumericUpDown nudShowOnly;
        private System.Windows.Forms.TextBox tboxShowing;
        private System.Windows.Forms.CheckBox cboxUseLog;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartScatter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lviewSubSeries;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox gboxOutlier;
        private System.Windows.Forms.NumericUpDown nudHideXoutliers;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblShowApp;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnInvSel;
        private System.Windows.Forms.Label lbAdvFilter;
        private System.Windows.Forms.CheckBox cbUseOffset;
        private System.Windows.Forms.Label lbOffsetValue;
        private System.Windows.Forms.TextBox tbWUcnt;
        private System.Windows.Forms.Label LBtLwuS;
        private System.Windows.Forms.Label lbGPUvis;
    }
}