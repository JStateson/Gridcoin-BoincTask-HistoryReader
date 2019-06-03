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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScatterForm));
            this.ChartScatter = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudXscale = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ChartScatter)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudXscale)).BeginInit();
            this.SuspendLayout();
            // 
            // ChartScatter
            // 
            chartArea1.Name = "ChartArea1";
            this.ChartScatter.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.ChartScatter.Legends.Add(legend1);
            this.ChartScatter.Location = new System.Drawing.Point(224, 23);
            this.ChartScatter.Name = "ChartScatter";
            this.ChartScatter.Size = new System.Drawing.Size(640, 320);
            this.ChartScatter.TabIndex = 0;
            this.ChartScatter.Text = "Scatter Plot";
            this.ChartScatter.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChartScatter_MouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(578, 379);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 52);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nudXscale);
            this.groupBox1.Location = new System.Drawing.Point(12, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 179);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scaling";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "X-Factor";
            // 
            // nudXscale
            // 
            this.nudXscale.Location = new System.Drawing.Point(93, 37);
            this.nudXscale.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudXscale.Name = "nudXscale";
            this.nudXscale.ReadOnly = true;
            this.nudXscale.Size = new System.Drawing.Size(38, 20);
            this.nudXscale.TabIndex = 0;
            this.nudXscale.ValueChanged += new System.EventHandler(this.nudXscale_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.label3.Location = new System.Drawing.Point(233, 379);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(259, 39);
            this.label3.TabIndex = 3;
            this.label3.Text = "Clkick on the colored marker near the Legend above \r\nright to view just the desir" +
    "ed series. Click anywhere \r\na second time to redisplay all the eseries.";
            // 
            // ScatterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ChartScatter);
            this.MaximizeBox = false;
            this.Name = "ScatterForm";
            this.Text = "Scatter Plot";
            ((System.ComponentModel.ISupportInitialize)(this.ChartScatter)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudXscale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart ChartScatter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudXscale;
        private System.Windows.Forms.Label label3;
    }
}