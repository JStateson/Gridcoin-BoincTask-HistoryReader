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
            this.ChartScatter = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ChartScatter)).BeginInit();
            this.SuspendLayout();
            // 
            // ChartScatter
            // 
            chartArea1.Name = "ChartArea1";
            this.ChartScatter.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.ChartScatter.Legends.Add(legend1);
            this.ChartScatter.Location = new System.Drawing.Point(12, 12);
            this.ChartScatter.Name = "ChartScatter";
            this.ChartScatter.Size = new System.Drawing.Size(645, 354);
            this.ChartScatter.TabIndex = 0;
            this.ChartScatter.Text = "Scatter Plot";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(54, 386);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 39);
            this.label1.TabIndex = 1;
            this.label1.Text = "If any devices (GPU) were processing concurrent tasks then\r\nyou must enter the nu" +
    "mber of concurrent tasks for that\r\nproject and system and click on apply. ";
            // 
            // ScatterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ChartScatter);
            this.Name = "ScatterForm";
            this.Text = "Scatter Plot";
            ((System.ComponentModel.ISupportInitialize)(this.ChartScatter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart ChartScatter;
        private System.Windows.Forms.Label label1;
    }
}