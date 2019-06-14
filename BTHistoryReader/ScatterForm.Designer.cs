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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.lbScatUsage = new System.Windows.Forms.Label();
            this.GetLegendInfo = new System.Windows.Forms.Timer(this.components);
            this.labelShowSeries = new System.Windows.Forms.Label();
            this.nudShowOnly = new System.Windows.Forms.NumericUpDown();
            this.tboxShowing = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudXscale = new System.Windows.Forms.NumericUpDown();
            this.cboxUseLog = new System.Windows.Forms.CheckBox();
            this.ChartScatter = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lviewSubSeries = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gboxOutlier = new System.Windows.Forms.GroupBox();
            this.nudHideXoutliers = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSysHideUnhide = new System.Windows.Forms.Label();
            this.lblShowApp = new System.Windows.Forms.Label();
            this.btnInvSel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nudShowOnly)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXscale)).BeginInit();
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
            this.lbScatUsage.Location = new System.Drawing.Point(653, 361);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Stretch";
            // 
            // nudXscale
            // 
            this.nudXscale.Location = new System.Drawing.Point(105, 36);
            this.nudXscale.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudXscale.Name = "nudXscale";
            this.nudXscale.ReadOnly = true;
            this.nudXscale.Size = new System.Drawing.Size(38, 20);
            this.nudXscale.TabIndex = 6;
            this.nudXscale.ValueChanged += new System.EventHandler(this.nudXscale_ValueChanged_1);
            // 
            // cboxUseLog
            // 
            this.cboxUseLog.AutoSize = true;
            this.cboxUseLog.Location = new System.Drawing.Point(170, 39);
            this.cboxUseLog.Name = "cboxUseLog";
            this.cboxUseLog.Size = new System.Drawing.Size(108, 17);
            this.cboxUseLog.TabIndex = 8;
            this.cboxUseLog.Text = "Make X log scale";
            this.cboxUseLog.UseVisualStyleBackColor = true;
            this.cboxUseLog.CheckedChanged += new System.EventHandler(this.cboxUseLog_CheckedChanged);
            // 
            // ChartScatter
            // 
            chartArea1.Name = "ChartArea1";
            this.ChartScatter.ChartAreas.Add(chartArea1);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
            legend1.IsDockedInsideChartArea = false;
            legend1.Name = "Legend1";
            legend2.DockedToChartArea = "ChartArea1";
            legend2.Name = "SysLeg";
            this.ChartScatter.Legends.Add(legend1);
            this.ChartScatter.Legends.Add(legend2);
            this.ChartScatter.Location = new System.Drawing.Point(21, 22);
            this.ChartScatter.Name = "ChartScatter";
            this.ChartScatter.Size = new System.Drawing.Size(923, 320);
            this.ChartScatter.TabIndex = 0;
            this.ChartScatter.Text = "Scatter Plot";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboxUseLog);
            this.groupBox1.Controls.Add(this.nudXscale);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 423);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 75);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "X-Axis scaling";
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
            this.gboxOutlier.Location = new System.Drawing.Point(618, 425);
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
            // lblSysHideUnhide
            // 
            this.lblSysHideUnhide.AutoSize = true;
            this.lblSysHideUnhide.Location = new System.Drawing.Point(419, 485);
            this.lblSysHideUnhide.Name = "lblSysHideUnhide";
            this.lblSysHideUnhide.Size = new System.Drawing.Size(142, 13);
            this.lblSysHideUnhide.TabIndex = 13;
            this.lblSysHideUnhide.Text = "select system to hide/unhide";
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
            // ScatterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 510);
            this.Controls.Add(this.btnInvSel);
            this.Controls.Add(this.lblShowApp);
            this.Controls.Add(this.lblSysHideUnhide);
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
            ((System.ComponentModel.ISupportInitialize)(this.nudXscale)).EndInit();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudXscale;
        private System.Windows.Forms.CheckBox cboxUseLog;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartScatter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lviewSubSeries;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox gboxOutlier;
        private System.Windows.Forms.NumericUpDown nudHideXoutliers;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSysHideUnhide;
        private System.Windows.Forms.Label lblShowApp;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnInvSel;
    }
}