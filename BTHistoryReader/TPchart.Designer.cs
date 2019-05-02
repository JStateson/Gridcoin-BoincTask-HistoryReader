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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPchart));
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DetailFilter = new System.Windows.Forms.NumericUpDown();
            this.lbChart = new System.Windows.Forms.Label();
            this.lbl_sysname = new System.Windows.Forms.Label();
            this.cbHours = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetailFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea4.AxisX.Interval = 2D;
            chartArea4.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea4.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea4.AxisX.MajorGrid.Enabled = false;
            chartArea4.AxisX.MajorTickMark.Enabled = false;
            chartArea4.AxisX.MajorTickMark.Interval = 0D;
            chartArea4.AxisX.MajorTickMark.IntervalOffset = 0D;
            chartArea4.AxisX.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea4.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea4.AxisX.Minimum = 0D;
            chartArea4.AxisX.Title = "Hours Back";
            chartArea4.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart1.Legends.Add(legend4);
            this.chart1.Location = new System.Drawing.Point(207, 25);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(537, 305);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbHours);
            this.groupBox1.Location = new System.Drawing.Point(12, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 131);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Max Lookback";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DetailFilter);
            this.groupBox2.Location = new System.Drawing.Point(12, 187);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 116);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Detail Filter (0 is all)";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // DetailFilter
            // 
            this.DetailFilter.Location = new System.Drawing.Point(38, 48);
            this.DetailFilter.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.DetailFilter.Name = "DetailFilter";
            this.DetailFilter.Size = new System.Drawing.Size(47, 20);
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
            this.cbHours.Location = new System.Drawing.Point(38, 40);
            this.cbHours.Name = "cbHours";
            this.cbHours.Size = new System.Drawing.Size(57, 21);
            this.cbHours.TabIndex = 0;
            this.cbHours.Text = "24";
            this.toolTip1.SetToolTip(this.cbHours, "BoincTasks default is 24 hours maximum");
            this.cbHours.SelectedIndexChanged += new System.EventHandler(this.cbHours_SelectedIndexChanged);
            // 
            // TPchart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 424);
            this.Controls.Add(this.lbl_sysname);
            this.Controls.Add(this.lbChart);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chart1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TPchart";
            this.Text = "Throughput Chart";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TPchart_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DetailFilter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown DetailFilter;
        private System.Windows.Forms.Label lbChart;
        private System.Windows.Forms.Label lbl_sysname;
        private System.Windows.Forms.ComboBox cbHours;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}