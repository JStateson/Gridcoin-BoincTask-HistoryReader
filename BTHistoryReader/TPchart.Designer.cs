namespace BTHistoryReader
{
    partial class TPchart
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPchart));
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labStartTime = new System.Windows.Forms.Label();
            this.cbHours = new System.Windows.Forms.ComboBox();
            this.gboxFilter = new System.Windows.Forms.GroupBox();
            this.labConcur = new System.Windows.Forms.Label();
            this.nudConcur = new System.Windows.Forms.NumericUpDown();
            this.tbSpinBinValue = new System.Windows.Forms.TextBox();
            this.SpinBin = new System.Windows.Forms.NumericUpDown();
            this.lbBinSize = new System.Windows.Forms.Label();
            this.lbSpinFilter = new System.Windows.Forms.Label();
            this.DetailFilter = new System.Windows.Forms.NumericUpDown();
            this.lbChart = new System.Windows.Forms.Label();
            this.lbl_sysname = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.gboxFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudConcur)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpinBin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetailFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.AxisX.Interval = 2D;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.MajorTickMark.Interval = 0D;
            chartArea1.AxisX.MajorTickMark.IntervalOffset = 0D;
            chartArea1.AxisX.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.Title = "Hours Back";
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(222, 25);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(537, 305);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labStartTime);
            this.groupBox1.Controls.Add(this.cbHours);
            this.groupBox1.Location = new System.Drawing.Point(12, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 131);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Max Lookback";
            // 
            // labStartTime
            // 
            this.labStartTime.AutoSize = true;
            this.labStartTime.Location = new System.Drawing.Point(15, 79);
            this.labStartTime.Name = "labStartTime";
            this.labStartTime.Size = new System.Drawing.Size(55, 13);
            this.labStartTime.TabIndex = 1;
            this.labStartTime.Text = "Start Time";
            // 
            // cbHours
            // 
            this.cbHours.FormattingEnabled = true;
            this.cbHours.Items.AddRange(new object[] {
            "24",
            "12",
            "8",
            "6",
            "4",
            "2",
            "1"});
            this.cbHours.Location = new System.Drawing.Point(17, 37);
            this.cbHours.Name = "cbHours";
            this.cbHours.Size = new System.Drawing.Size(57, 21);
            this.cbHours.TabIndex = 0;
            this.cbHours.Text = "24";
            this.toolTip1.SetToolTip(this.cbHours, "BoincTasks default is 24 hours maximum");
            this.cbHours.SelectedIndexChanged += new System.EventHandler(this.cbHours_SelectedIndexChanged);
            // 
            // gboxFilter
            // 
            this.gboxFilter.Controls.Add(this.labConcur);
            this.gboxFilter.Controls.Add(this.nudConcur);
            this.gboxFilter.Controls.Add(this.tbSpinBinValue);
            this.gboxFilter.Controls.Add(this.SpinBin);
            this.gboxFilter.Controls.Add(this.lbBinSize);
            this.gboxFilter.Controls.Add(this.lbSpinFilter);
            this.gboxFilter.Controls.Add(this.DetailFilter);
            this.gboxFilter.Location = new System.Drawing.Point(12, 187);
            this.gboxFilter.Name = "gboxFilter";
            this.gboxFilter.Size = new System.Drawing.Size(184, 143);
            this.gboxFilter.TabIndex = 4;
            this.gboxFilter.TabStop = false;
            this.gboxFilter.Text = "Detail Filter (0 is all)";
            // 
            // labConcur
            // 
            this.labConcur.AutoSize = true;
            this.labConcur.Location = new System.Drawing.Point(6, 111);
            this.labConcur.Name = "labConcur";
            this.labConcur.Size = new System.Drawing.Size(86, 13);
            this.labConcur.TabIndex = 10;
            this.labConcur.Text = "Concurrent WUs";
            // 
            // nudConcur
            // 
            this.nudConcur.Location = new System.Drawing.Point(115, 104);
            this.nudConcur.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudConcur.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudConcur.Name = "nudConcur";
            this.nudConcur.Size = new System.Drawing.Size(36, 20);
            this.nudConcur.TabIndex = 9;
            this.toolTip1.SetToolTip(this.nudConcur, "Number of concurrent GPU task (defult 1)");
            this.nudConcur.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudConcur.ValueChanged += new System.EventHandler(this.nudConcur_ValueChanged);
            // 
            // tbSpinBinValue
            // 
            this.tbSpinBinValue.Location = new System.Drawing.Point(92, 67);
            this.tbSpinBinValue.Name = "tbSpinBinValue";
            this.tbSpinBinValue.Size = new System.Drawing.Size(33, 20);
            this.tbSpinBinValue.TabIndex = 8;
            this.tbSpinBinValue.Text = "2";
            // 
            // SpinBin
            // 
            this.SpinBin.Location = new System.Drawing.Point(131, 68);
            this.SpinBin.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SpinBin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpinBin.Name = "SpinBin";
            this.SpinBin.Size = new System.Drawing.Size(20, 20);
            this.SpinBin.TabIndex = 7;
            this.toolTip1.SetToolTip(this.SpinBin, "Put more into bins");
            this.SpinBin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpinBin.ValueChanged += new System.EventHandler(this.SpinBin_ValueChanged);
            // 
            // lbBinSize
            // 
            this.lbBinSize.AutoSize = true;
            this.lbBinSize.Location = new System.Drawing.Point(6, 70);
            this.lbBinSize.Name = "lbBinSize";
            this.lbBinSize.Size = new System.Drawing.Size(75, 13);
            this.lbBinSize.TabIndex = 6;
            this.lbBinSize.Text = "Binning Factor";
            this.toolTip1.SetToolTip(this.lbBinSize, "Goes up or down power of 2");
            // 
            // lbSpinFilter
            // 
            this.lbSpinFilter.AutoSize = true;
            this.lbSpinFilter.Location = new System.Drawing.Point(6, 35);
            this.lbSpinFilter.Name = "lbSpinFilter";
            this.lbSpinFilter.Size = new System.Drawing.Size(68, 13);
            this.lbSpinFilter.TabIndex = 5;
            this.lbSpinFilter.Text = "Filter by STD";
            // 
            // DetailFilter
            // 
            this.DetailFilter.Location = new System.Drawing.Point(115, 35);
            this.DetailFilter.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.DetailFilter.Name = "DetailFilter";
            this.DetailFilter.Size = new System.Drawing.Size(36, 20);
            this.DetailFilter.TabIndex = 4;
            this.toolTip1.SetToolTip(this.DetailFilter, "Removes AVG plus STD from plot");
            this.DetailFilter.ValueChanged += new System.EventHandler(this.DetailFilter_ValueChanged);
            // 
            // lbChart
            // 
            this.lbChart.AutoSize = true;
            this.lbChart.Location = new System.Drawing.Point(466, 345);
            this.lbChart.Name = "lbChart";
            this.lbChart.Size = new System.Drawing.Size(278, 52);
            this.lbChart.TabIndex = 5;
            this.lbChart.Text = resources.GetString("lbChart.Text");
            // 
            // lbl_sysname
            // 
            this.lbl_sysname.AutoSize = true;
            this.lbl_sysname.Location = new System.Drawing.Point(12, 354);
            this.lbl_sysname.Name = "lbl_sysname";
            this.lbl_sysname.Size = new System.Drawing.Size(35, 13);
            this.lbl_sysname.TabIndex = 6;
            this.lbl_sysname.Text = "label1";
            // 
            // TPchart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 424);
            this.Controls.Add(this.lbl_sysname);
            this.Controls.Add(this.lbChart);
            this.Controls.Add(this.gboxFilter);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chart1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TPchart";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TPchart_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gboxFilter.ResumeLayout(false);
            this.gboxFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudConcur)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpinBin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetailFilter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gboxFilter;
        private System.Windows.Forms.NumericUpDown DetailFilter;
        private System.Windows.Forms.Label lbChart;
        private System.Windows.Forms.Label lbl_sysname;
        private System.Windows.Forms.ComboBox cbHours;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lbSpinFilter;
        private System.Windows.Forms.TextBox tbSpinBinValue;
        private System.Windows.Forms.NumericUpDown SpinBin;
        private System.Windows.Forms.Label lbBinSize;
        private System.Windows.Forms.Label labStartTime;
        private System.Windows.Forms.Label labConcur;
        private System.Windows.Forms.NumericUpDown nudConcur;
    }
}