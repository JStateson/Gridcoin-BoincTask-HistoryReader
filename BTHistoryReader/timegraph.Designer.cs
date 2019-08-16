namespace BTHistoryReader
{
    partial class timegraph
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
            this.tgraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lbxaxis = new System.Windows.Forms.Label();
            this.lbyaxis = new System.Windows.Forms.Label();
            this.nudAvg = new System.Windows.Forms.DomainUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDoGraph = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tgraph)).BeginInit();
            this.SuspendLayout();
            // 
            // tgraph
            // 
            chartArea1.Name = "ChartArea";
            this.tgraph.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.tgraph.Legends.Add(legend1);
            this.tgraph.Location = new System.Drawing.Point(39, 12);
            this.tgraph.Name = "tgraph";
            this.tgraph.Size = new System.Drawing.Size(749, 375);
            this.tgraph.TabIndex = 0;
            this.tgraph.Text = "TimeGraph";
            // 
            // lbxaxis
            // 
            this.lbxaxis.AutoSize = true;
            this.lbxaxis.Location = new System.Drawing.Point(36, 414);
            this.lbxaxis.Name = "lbxaxis";
            this.lbxaxis.Size = new System.Drawing.Size(50, 13);
            this.lbxaxis.TabIndex = 1;
            this.lbxaxis.Text = "xaxis info";
            // 
            // lbyaxis
            // 
            this.lbyaxis.AutoSize = true;
            this.lbyaxis.Location = new System.Drawing.Point(36, 439);
            this.lbyaxis.Name = "lbyaxis";
            this.lbyaxis.Size = new System.Drawing.Size(50, 13);
            this.lbyaxis.TabIndex = 2;
            this.lbyaxis.Text = "yaxis info";
            // 
            // nudAvg
            // 
            this.nudAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudAvg.Items.Add("2");
            this.nudAvg.Items.Add("4");
            this.nudAvg.Items.Add("8");
            this.nudAvg.Items.Add("15");
            this.nudAvg.Items.Add("30");
            this.nudAvg.Items.Add("60");
            this.nudAvg.Location = new System.Drawing.Point(688, 409);
            this.nudAvg.Name = "nudAvg";
            this.nudAvg.Size = new System.Drawing.Size(59, 22);
            this.nudAvg.TabIndex = 3;
            this.nudAvg.Text = "15";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(561, 409);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Minutes To Average";
            // 
            // btnDoGraph
            // 
            this.btnDoGraph.Location = new System.Drawing.Point(564, 439);
            this.btnDoGraph.Name = "btnDoGraph";
            this.btnDoGraph.Size = new System.Drawing.Size(75, 23);
            this.btnDoGraph.TabIndex = 5;
            this.btnDoGraph.Text = "Re-Plot";
            this.btnDoGraph.UseVisualStyleBackColor = true;
            this.btnDoGraph.Click += new System.EventHandler(this.btnDoGraph_Click);
            // 
            // timegraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 474);
            this.Controls.Add(this.btnDoGraph);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudAvg);
            this.Controls.Add(this.lbyaxis);
            this.Controls.Add(this.lbxaxis);
            this.Controls.Add(this.tgraph);
            this.Name = "timegraph";
            this.Text = "timegraph";
            ((System.ComponentModel.ISupportInitialize)(this.tgraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart tgraph;
        private System.Windows.Forms.Label lbxaxis;
        private System.Windows.Forms.Label lbyaxis;
        private System.Windows.Forms.DomainUpDown nudAvg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDoGraph;
    }
}