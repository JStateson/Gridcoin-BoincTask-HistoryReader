namespace BTHistoryReader
{
    partial class BTHistory
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnShowProjectTree = new System.Windows.Forms.Button();
            this.gb_filter = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_LocMax = new System.Windows.Forms.Label();
            this.lbTimeContinunity = new System.Windows.Forms.Label();
            this.btnContinunity = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lb_SelWorkUnits = new System.Windows.Forms.ListBox();
            this.btn_Filter = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tb_Results = new System.Windows.Forms.TextBox();
            this.rbThroughput = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbNDevices = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_AvgCredit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFetchHistory = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_AppNames = new System.Windows.Forms.ComboBox();
            this.cb_SelProj = new System.Windows.Forms.ComboBox();
            this.tb_Info = new System.Windows.Forms.TextBox();
            this.lb_history_loc = new System.Windows.Forms.Label();
            this.btn_OpenHistory = new System.Windows.Forms.Button();
            this.ofd_history = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rbElapsed = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.gb_filter.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnAbout);
            this.panel1.Controls.Add(this.btnShowProjectTree);
            this.panel1.Controls.Add(this.gb_filter);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.tb_Info);
            this.panel1.Controls.Add(this.lb_history_loc);
            this.panel1.Controls.Add(this.btn_OpenHistory);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(858, 527);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(725, 28);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(75, 23);
            this.btnAbout.TabIndex = 8;
            this.btnAbout.Text = "ABOUT";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnShowProjectTree
            // 
            this.btnShowProjectTree.Location = new System.Drawing.Point(555, 28);
            this.btnShowProjectTree.Name = "btnShowProjectTree";
            this.btnShowProjectTree.Size = new System.Drawing.Size(140, 23);
            this.btnShowProjectTree.TabIndex = 7;
            this.btnShowProjectTree.Text = "SHOW PROJECT TREE";
            this.btnShowProjectTree.UseVisualStyleBackColor = true;
            this.btnShowProjectTree.Click += new System.EventHandler(this.btnShowProjectTree_Click);
            // 
            // gb_filter
            // 
            this.gb_filter.Controls.Add(this.label4);
            this.gb_filter.Controls.Add(this.lb_LocMax);
            this.gb_filter.Controls.Add(this.lbTimeContinunity);
            this.gb_filter.Controls.Add(this.btnContinunity);
            this.gb_filter.Controls.Add(this.btnClear);
            this.gb_filter.Controls.Add(this.lb_SelWorkUnits);
            this.gb_filter.Controls.Add(this.btn_Filter);
            this.gb_filter.Location = new System.Drawing.Point(488, 77);
            this.gb_filter.Name = "gb_filter";
            this.gb_filter.Size = new System.Drawing.Size(344, 379);
            this.gb_filter.TabIndex = 6;
            this.gb_filter.TabStop = false;
            this.gb_filter.Text = "Filter";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Info;
            this.label4.Location = new System.Drawing.Point(139, 279);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(161, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Be sure to check continunity first";
            // 
            // lb_LocMax
            // 
            this.lb_LocMax.AutoSize = true;
            this.lb_LocMax.CausesValidation = false;
            this.lb_LocMax.Location = new System.Drawing.Point(139, 312);
            this.lb_LocMax.Name = "lb_LocMax";
            this.lb_LocMax.Size = new System.Drawing.Size(74, 13);
            this.lb_LocMax.TabIndex = 11;
            this.lb_LocMax.Text = "not known yet";
            // 
            // lbTimeContinunity
            // 
            this.lbTimeContinunity.AutoSize = true;
            this.lbTimeContinunity.CausesValidation = false;
            this.lbTimeContinunity.Location = new System.Drawing.Point(139, 339);
            this.lbTimeContinunity.Name = "lbTimeContinunity";
            this.lbTimeContinunity.Size = new System.Drawing.Size(91, 13);
            this.lbTimeContinunity.TabIndex = 10;
            this.lbTimeContinunity.Text = "not calculated yet";
            // 
            // btnContinunity
            // 
            this.btnContinunity.Location = new System.Drawing.Point(19, 334);
            this.btnContinunity.Name = "btnContinunity";
            this.btnContinunity.Size = new System.Drawing.Size(114, 23);
            this.btnContinunity.TabIndex = 9;
            this.btnContinunity.Text = "Check Continunity";
            this.toolTip1.SetToolTip(this.btnContinunity, "avoid any large change in time");
            this.btnContinunity.UseVisualStyleBackColor = true;
            this.btnContinunity.Click += new System.EventHandler(this.btnContinunity_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(19, 302);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(114, 23);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear Selections";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lb_SelWorkUnits
            // 
            this.lb_SelWorkUnits.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_SelWorkUnits.FormattingEnabled = true;
            this.lb_SelWorkUnits.ItemHeight = 14;
            this.lb_SelWorkUnits.Location = new System.Drawing.Point(19, 25);
            this.lb_SelWorkUnits.Name = "lb_SelWorkUnits";
            this.lb_SelWorkUnits.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lb_SelWorkUnits.Size = new System.Drawing.Size(293, 228);
            this.lb_SelWorkUnits.TabIndex = 7;
            this.toolTip1.SetToolTip(this.lb_SelWorkUnits, "Select a start and a stop");
            // 
            // btn_Filter
            // 
            this.btn_Filter.Location = new System.Drawing.Point(19, 269);
            this.btn_Filter.Name = "btn_Filter";
            this.btn_Filter.Size = new System.Drawing.Size(114, 23);
            this.btn_Filter.TabIndex = 6;
            this.btn_Filter.Text = "Filter Selected";
            this.toolTip1.SetToolTip(this.btn_Filter, "Select items in box then apply filter");
            this.btn_Filter.UseVisualStyleBackColor = true;
            this.btn_Filter.Click += new System.EventHandler(this.btn_Filter_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbElapsed);
            this.groupBox2.Controls.Add(this.tb_Results);
            this.groupBox2.Controls.Add(this.rbThroughput);
            this.groupBox2.Location = new System.Drawing.Point(16, 327);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(452, 185);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Type Analysis";
            // 
            // tb_Results
            // 
            this.tb_Results.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Results.Location = new System.Drawing.Point(151, 19);
            this.tb_Results.Multiline = true;
            this.tb_Results.Name = "tb_Results";
            this.tb_Results.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Results.Size = new System.Drawing.Size(268, 103);
            this.tb_Results.TabIndex = 4;
            // 
            // rbThroughput
            // 
            this.rbThroughput.AutoSize = true;
            this.rbThroughput.Checked = true;
            this.rbThroughput.Location = new System.Drawing.Point(6, 33);
            this.rbThroughput.Name = "rbThroughput";
            this.rbThroughput.Size = new System.Drawing.Size(80, 17);
            this.rbThroughput.TabIndex = 0;
            this.rbThroughput.Text = "Throughput";
            this.toolTip1.SetToolTip(this.rbThroughput, "Number of tasks and time interval from first to last");
            this.rbThroughput.UseVisualStyleBackColor = true;
            this.rbThroughput.CheckedChanged += new System.EventHandler(this.rbThroughput_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbNDevices);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tb_AvgCredit);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnFetchHistory);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cb_AppNames);
            this.groupBox1.Controls.Add(this.cb_SelProj);
            this.groupBox1.Location = new System.Drawing.Point(16, 159);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 162);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "History Selections";
            // 
            // tbNDevices
            // 
            this.tbNDevices.Location = new System.Drawing.Point(282, 129);
            this.tbNDevices.Name = "tbNDevices";
            this.tbNDevices.Size = new System.Drawing.Size(57, 20);
            this.tbNDevices.TabIndex = 8;
            this.tbNDevices.Text = "1";
            this.toolTip1.SetToolTip(this.tbNDevices, "Number of GPUs or CPU threads \r\nthat produce the data down");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(130, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Number of boards / cores";
            // 
            // tb_AvgCredit
            // 
            this.tb_AvgCredit.Location = new System.Drawing.Point(282, 100);
            this.tb_AvgCredit.Name = "tb_AvgCredit";
            this.tb_AvgCredit.Size = new System.Drawing.Size(57, 20);
            this.tb_AvgCredit.TabIndex = 6;
            this.tb_AvgCredit.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(130, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Estimated Avg Credit Per Unit";
            // 
            // btnFetchHistory
            // 
            this.btnFetchHistory.Location = new System.Drawing.Point(11, 119);
            this.btnFetchHistory.Name = "btnFetchHistory";
            this.btnFetchHistory.Size = new System.Drawing.Size(96, 23);
            this.btnFetchHistory.TabIndex = 4;
            this.btnFetchHistory.Text = "Display History";
            this.toolTip1.SetToolTip(this.btnFetchHistory, "Extract all app reslults  from history");
            this.btnFetchHistory.UseVisualStyleBackColor = true;
            this.btnFetchHistory.Click += new System.EventHandler(this.btnFetchHistory_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "App Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Project Name";
            // 
            // cb_AppNames
            // 
            this.cb_AppNames.FormattingEnabled = true;
            this.cb_AppNames.Location = new System.Drawing.Point(86, 67);
            this.cb_AppNames.Name = "cb_AppNames";
            this.cb_AppNames.Size = new System.Drawing.Size(279, 21);
            this.cb_AppNames.TabIndex = 1;
            this.cb_AppNames.SelectedIndexChanged += new System.EventHandler(this.cb_AppNames_SelectedIndexChanged);
            // 
            // cb_SelProj
            // 
            this.cb_SelProj.FormattingEnabled = true;
            this.cb_SelProj.Location = new System.Drawing.Point(86, 25);
            this.cb_SelProj.Name = "cb_SelProj";
            this.cb_SelProj.Size = new System.Drawing.Size(279, 21);
            this.cb_SelProj.TabIndex = 0;
            this.cb_SelProj.SelectedIndexChanged += new System.EventHandler(this.cb_SelProj_SelectedIndexChanged);
            // 
            // tb_Info
            // 
            this.tb_Info.Location = new System.Drawing.Point(16, 73);
            this.tb_Info.Multiline = true;
            this.tb_Info.Name = "tb_Info";
            this.tb_Info.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Info.Size = new System.Drawing.Size(452, 68);
            this.tb_Info.TabIndex = 2;
            // 
            // lb_history_loc
            // 
            this.lb_history_loc.AutoSize = true;
            this.lb_history_loc.Location = new System.Drawing.Point(3, 12);
            this.lb_history_loc.Name = "lb_history_loc";
            this.lb_history_loc.Size = new System.Drawing.Size(0, 13);
            this.lb_history_loc.TabIndex = 1;
            // 
            // btn_OpenHistory
            // 
            this.btn_OpenHistory.Location = new System.Drawing.Point(16, 28);
            this.btn_OpenHistory.Name = "btn_OpenHistory";
            this.btn_OpenHistory.Size = new System.Drawing.Size(97, 23);
            this.btn_OpenHistory.TabIndex = 0;
            this.btn_OpenHistory.Text = "Open History";
            this.btn_OpenHistory.UseVisualStyleBackColor = true;
            this.btn_OpenHistory.Click += new System.EventHandler(this.btn_OpenHistory_Click);
            // 
            // ofd_history
            // 
            this.ofd_history.Filter = "CVS Files|*.cvs";
            // 
            // rbElapsed
            // 
            this.rbElapsed.AutoSize = true;
            this.rbElapsed.Location = new System.Drawing.Point(6, 62);
            this.rbElapsed.Name = "rbElapsed";
            this.rbElapsed.Size = new System.Drawing.Size(111, 17);
            this.rbElapsed.TabIndex = 5;
            this.rbElapsed.Text = "Avg Elapsed Time";
            this.toolTip1.SetToolTip(this.rbElapsed, "Number of tasks and time interval from first to last");
            this.rbElapsed.UseVisualStyleBackColor = true;
            this.rbElapsed.CheckedChanged += new System.EventHandler(this.rbElapsed_CheckedChanged);
            // 
            // BTHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 541);
            this.Controls.Add(this.panel1);
            this.Name = "BTHistory";
            this.Text = "system";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gb_filter.ResumeLayout(false);
            this.gb_filter.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lb_history_loc;
        private System.Windows.Forms.Button btn_OpenHistory;
        private System.Windows.Forms.OpenFileDialog ofd_history;
        private System.Windows.Forms.TextBox tb_Info;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cb_SelProj;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_AppNames;
        private System.Windows.Forms.Button btnFetchHistory;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbThroughput;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox gb_filter;
        private System.Windows.Forms.Button btn_Filter;
        private System.Windows.Forms.TextBox tb_Results;
        private System.Windows.Forms.TextBox tb_AvgCredit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lb_SelWorkUnits;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnShowProjectTree;
        private System.Windows.Forms.Label lbTimeContinunity;
        private System.Windows.Forms.Button btnContinunity;
        private System.Windows.Forms.Label lb_LocMax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbNDevices;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.RadioButton rbElapsed;
    }
}

