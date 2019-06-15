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
            this.lblBuildDate = new System.Windows.Forms.Label();
            this.pbarLoading = new System.Windows.Forms.ProgressBar();
            this.gboxOPFsettings = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tboxLimit = new System.Windows.Forms.TextBox();
            this.cboxStopLoad = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbUseCVS = new System.Windows.Forms.RadioButton();
            this.rbUseCVS1 = new System.Windows.Forms.RadioButton();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnShowProjectTree = new System.Windows.Forms.Button();
            this.gb_filter = new System.Windows.Forms.GroupBox();
            this.btnScatSets = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnPlotET = new System.Windows.Forms.Button();
            this.btnPlot = new System.Windows.Forms.Button();
            this.btn_Filter = new System.Windows.Forms.Button();
            this.btnCheckPrev = new System.Windows.Forms.Button();
            this.lbSeriesTime = new System.Windows.Forms.Label();
            this.btnCheckNext = new System.Windows.Forms.Button();
            this.lb_NumSel = new System.Windows.Forms.Label();
            this.bt_all = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_LocMax = new System.Windows.Forms.Label();
            this.lbTimeContinunity = new System.Windows.Forms.Label();
            this.btnContinunity = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lb_SelWorkUnits = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbIdle = new System.Windows.Forms.RadioButton();
            this.rbElapsed = new System.Windows.Forms.RadioButton();
            this.tb_Results = new System.Windows.Forms.TextBox();
            this.rbThroughput = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_SelProj = new System.Windows.Forms.ComboBox();
            this.lb_nApps = new System.Windows.Forms.Label();
            this.lb_nProj = new System.Windows.Forms.Label();
            this.tbNDevices = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_AvgCredit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_AppNames = new System.Windows.Forms.ComboBox();
            this.tb_Info = new System.Windows.Forms.TextBox();
            this.lb_history_loc = new System.Windows.Forms.Label();
            this.btn_OpenHistory = new System.Windows.Forms.Button();
            this.ofd_history = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.TimerShowBuild = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.gboxOPFsettings.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.gb_filter.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblBuildDate);
            this.panel1.Controls.Add(this.pbarLoading);
            this.panel1.Controls.Add(this.gboxOPFsettings);
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
            this.panel1.Size = new System.Drawing.Size(937, 534);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lblBuildDate
            // 
            this.lblBuildDate.AutoSize = true;
            this.lblBuildDate.Location = new System.Drawing.Point(19, 63);
            this.lblBuildDate.Name = "lblBuildDate";
            this.lblBuildDate.Size = new System.Drawing.Size(56, 13);
            this.lblBuildDate.TabIndex = 12;
            this.lblBuildDate.Text = "Build Date";
            // 
            // pbarLoading
            // 
            this.pbarLoading.Location = new System.Drawing.Point(16, 90);
            this.pbarLoading.Maximum = 20;
            this.pbarLoading.Name = "pbarLoading";
            this.pbarLoading.Size = new System.Drawing.Size(163, 11);
            this.pbarLoading.Step = 1;
            this.pbarLoading.TabIndex = 11;
            this.pbarLoading.Visible = false;
            // 
            // gboxOPFsettings
            // 
            this.gboxOPFsettings.Controls.Add(this.label7);
            this.gboxOPFsettings.Controls.Add(this.label6);
            this.gboxOPFsettings.Controls.Add(this.tboxLimit);
            this.gboxOPFsettings.Controls.Add(this.cboxStopLoad);
            this.gboxOPFsettings.Controls.Add(this.groupBox4);
            this.gboxOPFsettings.Location = new System.Drawing.Point(210, 18);
            this.gboxOPFsettings.Name = "gboxOPFsettings";
            this.gboxOPFsettings.Size = new System.Drawing.Size(306, 86);
            this.gboxOPFsettings.TabIndex = 10;
            this.gboxOPFsettings.TabStop = false;
            this.gboxOPFsettings.Text = "Open File Settings";
            this.toolTip1.SetToolTip(this.gboxOPFsettings, "Defalt is CVS1 and no longs");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(64, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "...records/app";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "No more than ...";
            // 
            // tboxLimit
            // 
            this.tboxLimit.Location = new System.Drawing.Point(9, 60);
            this.tboxLimit.Name = "tboxLimit";
            this.tboxLimit.Size = new System.Drawing.Size(49, 20);
            this.tboxLimit.TabIndex = 13;
            this.tboxLimit.Text = "20000";
            this.toolTip1.SetToolTip(this.tboxLimit, "use smaller number if out of memory or system stops responding");
            // 
            // cboxStopLoad
            // 
            this.cboxStopLoad.AutoSize = true;
            this.cboxStopLoad.Checked = true;
            this.cboxStopLoad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxStopLoad.Location = new System.Drawing.Point(21, 19);
            this.cboxStopLoad.Name = "cboxStopLoad";
            this.cboxStopLoad.Size = new System.Drawing.Size(95, 17);
            this.cboxStopLoad.TabIndex = 12;
            this.cboxStopLoad.Text = "Limit App Data";
            this.toolTip1.SetToolTip(this.cboxStopLoad, "This limit does NOT apply to comparions");
            this.cboxStopLoad.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbUseCVS);
            this.groupBox4.Controls.Add(this.rbUseCVS1);
            this.groupBox4.Location = new System.Drawing.Point(146, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(135, 66);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            // 
            // rbUseCVS
            // 
            this.rbUseCVS.AutoSize = true;
            this.rbUseCVS.Location = new System.Drawing.Point(30, 33);
            this.rbUseCVS.Name = "rbUseCVS";
            this.rbUseCVS.Size = new System.Drawing.Size(68, 17);
            this.rbUseCVS.TabIndex = 14;
            this.rbUseCVS.Text = "Use CVS";
            this.toolTip1.SetToolTip(this.rbUseCVS, "Files with this extension will contain active tasks that\r\nhave not completed and " +
        "thus will generate various\r\nwarning");
            this.rbUseCVS.UseVisualStyleBackColor = true;
            // 
            // rbUseCVS1
            // 
            this.rbUseCVS1.AutoSize = true;
            this.rbUseCVS1.Checked = true;
            this.rbUseCVS1.Location = new System.Drawing.Point(30, 17);
            this.rbUseCVS1.Name = "rbUseCVS1";
            this.rbUseCVS1.Size = new System.Drawing.Size(74, 17);
            this.rbUseCVS1.TabIndex = 13;
            this.rbUseCVS1.TabStop = true;
            this.rbUseCVS1.Text = "Use CVS1";
            this.toolTip1.SetToolTip(this.rbUseCVS1, "This contains completed work units but\r\nmay not have the most recent depending\r\no" +
        "n how often history is updated");
            this.rbUseCVS1.UseVisualStyleBackColor = true;
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(747, 12);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(99, 23);
            this.btnAbout.TabIndex = 8;
            this.btnAbout.Text = "ABOUT / HELP";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnShowProjectTree
            // 
            this.btnShowProjectTree.Location = new System.Drawing.Point(552, 12);
            this.btnShowProjectTree.Name = "btnShowProjectTree";
            this.btnShowProjectTree.Size = new System.Drawing.Size(140, 23);
            this.btnShowProjectTree.TabIndex = 7;
            this.btnShowProjectTree.Text = "SHOW PROJECT TREE";
            this.btnShowProjectTree.UseVisualStyleBackColor = true;
            this.btnShowProjectTree.Click += new System.EventHandler(this.btnShowProjectTree_Click);
            // 
            // gb_filter
            // 
            this.gb_filter.Controls.Add(this.btnScatSets);
            this.gb_filter.Controls.Add(this.groupBox5);
            this.gb_filter.Controls.Add(this.btn_Filter);
            this.gb_filter.Controls.Add(this.btnCheckPrev);
            this.gb_filter.Controls.Add(this.lbSeriesTime);
            this.gb_filter.Controls.Add(this.btnCheckNext);
            this.gb_filter.Controls.Add(this.lb_NumSel);
            this.gb_filter.Controls.Add(this.bt_all);
            this.gb_filter.Controls.Add(this.label4);
            this.gb_filter.Controls.Add(this.lb_LocMax);
            this.gb_filter.Controls.Add(this.lbTimeContinunity);
            this.gb_filter.Controls.Add(this.btnContinunity);
            this.gb_filter.Controls.Add(this.btnClear);
            this.gb_filter.Controls.Add(this.lb_SelWorkUnits);
            this.gb_filter.Location = new System.Drawing.Point(534, 41);
            this.gb_filter.Name = "gb_filter";
            this.gb_filter.Size = new System.Drawing.Size(382, 478);
            this.gb_filter.TabIndex = 6;
            this.gb_filter.TabStop = false;
            this.gb_filter.Text = "Filter";
            // 
            // btnScatSets
            // 
            this.btnScatSets.Enabled = false;
            this.btnScatSets.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnScatSets.Location = new System.Drawing.Point(265, 442);
            this.btnScatSets.Name = "btnScatSets";
            this.btnScatSets.Size = new System.Drawing.Size(111, 23);
            this.btnScatSets.TabIndex = 24;
            this.btnScatSets.Text = "Plot All Datasets";
            this.toolTip1.SetToolTip(this.btnScatSets, "Scatter elapsed over all datsets for project selected");
            this.btnScatSets.UseVisualStyleBackColor = true;
            this.btnScatSets.Click += new System.EventHandler(this.btnScatSets_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnPlotET);
            this.groupBox5.Controls.Add(this.btnPlot);
            this.groupBox5.Location = new System.Drawing.Point(269, 311);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(107, 118);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Above Selections";
            // 
            // btnPlotET
            // 
            this.btnPlotET.Enabled = false;
            this.btnPlotET.Location = new System.Drawing.Point(18, 72);
            this.btnPlotET.Name = "btnPlotET";
            this.btnPlotET.Size = new System.Drawing.Size(75, 23);
            this.btnPlotET.TabIndex = 22;
            this.btnPlotET.Text = "Plot Elapsed";
            this.toolTip1.SetToolTip(this.btnPlotET, "Histogram of elapsed time");
            this.btnPlotET.UseVisualStyleBackColor = true;
            this.btnPlotET.Click += new System.EventHandler(this.btnPlotET_Click);
            // 
            // btnPlot
            // 
            this.btnPlot.Enabled = false;
            this.btnPlot.Location = new System.Drawing.Point(18, 33);
            this.btnPlot.Name = "btnPlot";
            this.btnPlot.Size = new System.Drawing.Size(75, 23);
            this.btnPlot.TabIndex = 21;
            this.btnPlot.Text = "Plot Idle";
            this.toolTip1.SetToolTip(this.btnPlot, "Plots completion time of items selected");
            this.btnPlot.UseVisualStyleBackColor = true;
            this.btnPlot.Click += new System.EventHandler(this.btnPlot_Click);
            // 
            // btn_Filter
            // 
            this.btn_Filter.Enabled = false;
            this.btn_Filter.Location = new System.Drawing.Point(19, 269);
            this.btn_Filter.Name = "btn_Filter";
            this.btn_Filter.Size = new System.Drawing.Size(98, 23);
            this.btn_Filter.TabIndex = 6;
            this.btn_Filter.Text = "Run Filter";
            this.toolTip1.SetToolTip(this.btn_Filter, "Must select items first then apply filter");
            this.btn_Filter.UseVisualStyleBackColor = true;
            this.btn_Filter.Click += new System.EventHandler(this.btn_Filter_Click);
            // 
            // btnCheckPrev
            // 
            this.btnCheckPrev.BackColor = System.Drawing.SystemColors.Control;
            this.btnCheckPrev.Enabled = false;
            this.btnCheckPrev.Location = new System.Drawing.Point(19, 442);
            this.btnCheckPrev.Name = "btnCheckPrev";
            this.btnCheckPrev.Size = new System.Drawing.Size(125, 23);
            this.btnCheckPrev.TabIndex = 19;
            this.btnCheckPrev.Text = "Check Prev";
            this.toolTip1.SetToolTip(this.btnCheckPrev, "avoid any large change in time");
            this.btnCheckPrev.UseVisualStyleBackColor = true;
            this.btnCheckPrev.Click += new System.EventHandler(this.btnCheckPrev_Click);
            // 
            // lbSeriesTime
            // 
            this.lbSeriesTime.AutoSize = true;
            this.lbSeriesTime.Location = new System.Drawing.Point(59, 16);
            this.lbSeriesTime.Name = "lbSeriesTime";
            this.lbSeriesTime.Size = new System.Drawing.Size(145, 13);
            this.lbSeriesTime.TabIndex = 17;
            this.lbSeriesTime.Text = "Selected Series Length (time)";
            // 
            // btnCheckNext
            // 
            this.btnCheckNext.BackColor = System.Drawing.SystemColors.Control;
            this.btnCheckNext.Enabled = false;
            this.btnCheckNext.Location = new System.Drawing.Point(19, 406);
            this.btnCheckNext.Name = "btnCheckNext";
            this.btnCheckNext.Size = new System.Drawing.Size(125, 23);
            this.btnCheckNext.TabIndex = 15;
            this.btnCheckNext.Text = "Check Next";
            this.toolTip1.SetToolTip(this.btnCheckNext, "avoid any large change in time");
            this.btnCheckNext.UseVisualStyleBackColor = true;
            this.btnCheckNext.Click += new System.EventHandler(this.btnCheckNext_Click);
            // 
            // lb_NumSel
            // 
            this.lb_NumSel.AutoSize = true;
            this.lb_NumSel.Location = new System.Drawing.Point(168, 373);
            this.lb_NumSel.Name = "lb_NumSel";
            this.lb_NumSel.Size = new System.Drawing.Size(78, 13);
            this.lb_NumSel.TabIndex = 14;
            this.lb_NumSel.Text = "None Selected";
            this.lb_NumSel.Visible = false;
            // 
            // bt_all
            // 
            this.bt_all.Enabled = false;
            this.bt_all.Location = new System.Drawing.Point(19, 303);
            this.bt_all.Name = "bt_all";
            this.bt_all.Size = new System.Drawing.Size(98, 23);
            this.bt_all.TabIndex = 13;
            this.bt_all.Text = "Select All";
            this.bt_all.UseVisualStyleBackColor = true;
            this.bt_all.Click += new System.EventHandler(this.bt_all_Click);
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
            this.lb_LocMax.Location = new System.Drawing.Point(170, 313);
            this.lb_LocMax.Name = "lb_LocMax";
            this.lb_LocMax.Size = new System.Drawing.Size(74, 13);
            this.lb_LocMax.TabIndex = 11;
            this.lb_LocMax.Text = "not known yet";
            this.lb_LocMax.Visible = false;
            // 
            // lbTimeContinunity
            // 
            this.lbTimeContinunity.AutoSize = true;
            this.lbTimeContinunity.CausesValidation = false;
            this.lbTimeContinunity.Location = new System.Drawing.Point(169, 339);
            this.lbTimeContinunity.Name = "lbTimeContinunity";
            this.lbTimeContinunity.Size = new System.Drawing.Size(91, 13);
            this.lbTimeContinunity.TabIndex = 10;
            this.lbTimeContinunity.Text = "not calculated yet";
            // 
            // btnContinunity
            // 
            this.btnContinunity.BackColor = System.Drawing.SystemColors.Control;
            this.btnContinunity.Enabled = false;
            this.btnContinunity.Location = new System.Drawing.Point(19, 373);
            this.btnContinunity.Name = "btnContinunity";
            this.btnContinunity.Size = new System.Drawing.Size(125, 23);
            this.btnContinunity.TabIndex = 9;
            this.btnContinunity.Text = "Check Continunity";
            this.toolTip1.SetToolTip(this.btnContinunity, "avoid any large change in time");
            this.btnContinunity.UseVisualStyleBackColor = true;
            this.btnContinunity.Click += new System.EventHandler(this.btnContinunity_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(19, 339);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(98, 23);
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
            this.lb_SelWorkUnits.Location = new System.Drawing.Point(19, 39);
            this.lb_SelWorkUnits.Name = "lb_SelWorkUnits";
            this.lb_SelWorkUnits.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lb_SelWorkUnits.Size = new System.Drawing.Size(317, 214);
            this.lb_SelWorkUnits.TabIndex = 7;
            this.toolTip1.SetToolTip(this.lb_SelWorkUnits, "Select a start and a stop");
            this.lb_SelWorkUnits.SelectedIndexChanged += new System.EventHandler(this.lb_SelWorkUnits_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbIdle);
            this.groupBox2.Controls.Add(this.rbElapsed);
            this.groupBox2.Controls.Add(this.tb_Results);
            this.groupBox2.Controls.Add(this.rbThroughput);
            this.groupBox2.Location = new System.Drawing.Point(16, 352);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(500, 167);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Type Analysis";
            // 
            // rbIdle
            // 
            this.rbIdle.AutoSize = true;
            this.rbIdle.Location = new System.Drawing.Point(6, 83);
            this.rbIdle.Name = "rbIdle";
            this.rbIdle.Size = new System.Drawing.Size(68, 17);
            this.rbIdle.TabIndex = 6;
            this.rbIdle.Text = "Idle Time";
            this.toolTip1.SetToolTip(this.rbIdle, "Average and std of completion time");
            this.rbIdle.UseVisualStyleBackColor = true;
            // 
            // rbElapsed
            // 
            this.rbElapsed.AutoSize = true;
            this.rbElapsed.Location = new System.Drawing.Point(6, 52);
            this.rbElapsed.Name = "rbElapsed";
            this.rbElapsed.Size = new System.Drawing.Size(111, 17);
            this.rbElapsed.TabIndex = 5;
            this.rbElapsed.Text = "Avg Elapsed Time";
            this.toolTip1.SetToolTip(this.rbElapsed, "Average and std of completion time");
            this.rbElapsed.UseVisualStyleBackColor = true;
            this.rbElapsed.CheckedChanged += new System.EventHandler(this.rbElapsed_CheckedChanged);
            // 
            // tb_Results
            // 
            this.tb_Results.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Results.Location = new System.Drawing.Point(145, 19);
            this.tb_Results.Multiline = true;
            this.tb_Results.Name = "tb_Results";
            this.tb_Results.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Results.Size = new System.Drawing.Size(349, 134);
            this.tb_Results.TabIndex = 4;
            // 
            // rbThroughput
            // 
            this.rbThroughput.AutoSize = true;
            this.rbThroughput.Checked = true;
            this.rbThroughput.Location = new System.Drawing.Point(6, 23);
            this.rbThroughput.Name = "rbThroughput";
            this.rbThroughput.Size = new System.Drawing.Size(80, 17);
            this.rbThroughput.TabIndex = 0;
            this.rbThroughput.TabStop = true;
            this.rbThroughput.Text = "Throughput";
            this.toolTip1.SetToolTip(this.rbThroughput, "Number of tasks and time interval from first to last");
            this.rbThroughput.UseVisualStyleBackColor = true;
            this.rbThroughput.CheckedChanged += new System.EventHandler(this.rbThroughput_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_SelProj);
            this.groupBox1.Controls.Add(this.lb_nApps);
            this.groupBox1.Controls.Add(this.lb_nProj);
            this.groupBox1.Controls.Add(this.tbNDevices);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tb_AvgCredit);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cb_AppNames);
            this.groupBox1.Location = new System.Drawing.Point(16, 184);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 162);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "History Selections";
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
            // lb_nApps
            // 
            this.lb_nApps.AutoSize = true;
            this.lb_nApps.Location = new System.Drawing.Point(384, 75);
            this.lb_nApps.Name = "lb_nApps";
            this.lb_nApps.Size = new System.Drawing.Size(25, 13);
            this.lb_nApps.TabIndex = 10;
            this.lb_nApps.Text = "unk";
            this.lb_nApps.Visible = false;
            // 
            // lb_nProj
            // 
            this.lb_nProj.AutoSize = true;
            this.lb_nProj.Location = new System.Drawing.Point(384, 33);
            this.lb_nProj.Name = "lb_nProj";
            this.lb_nProj.Size = new System.Drawing.Size(25, 13);
            this.lb_nProj.TabIndex = 9;
            this.lb_nProj.Text = "unk";
            this.lb_nProj.Visible = false;
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
            // tb_Info
            // 
            this.tb_Info.Location = new System.Drawing.Point(16, 110);
            this.tb_Info.Multiline = true;
            this.tb_Info.Name = "tb_Info";
            this.tb_Info.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Info.Size = new System.Drawing.Size(500, 68);
            this.tb_Info.TabIndex = 2;
            // 
            // lb_history_loc
            // 
            this.lb_history_loc.AutoSize = true;
            this.lb_history_loc.Location = new System.Drawing.Point(13, 2);
            this.lb_history_loc.Name = "lb_history_loc";
            this.lb_history_loc.Size = new System.Drawing.Size(332, 13);
            this.lb_history_loc.TabIndex = 1;
            this.lb_history_loc.Text = "Select multiple files to do a comparison (app data limit does not apply)";
            // 
            // btn_OpenHistory
            // 
            this.btn_OpenHistory.Location = new System.Drawing.Point(16, 28);
            this.btn_OpenHistory.Name = "btn_OpenHistory";
            this.btn_OpenHistory.Size = new System.Drawing.Size(97, 23);
            this.btn_OpenHistory.TabIndex = 0;
            this.btn_OpenHistory.Text = "Open History";
            this.toolTip1.SetToolTip(this.btn_OpenHistory, "After opening, perform \"Dkisplay History\" then select and run filter");
            this.btn_OpenHistory.UseVisualStyleBackColor = true;
            this.btn_OpenHistory.Click += new System.EventHandler(this.btn_OpenHistory_Click);
            // 
            // ofd_history
            // 
            this.ofd_history.Filter = "CVS Files|*.cvs";
            this.ofd_history.Multiselect = true;
            // 
            // TimerShowBuild
            // 
            this.TimerShowBuild.Enabled = true;
            this.TimerShowBuild.Interval = 250;
            this.TimerShowBuild.Tick += new System.EventHandler(this.TimerShowBuild_Tick);
            // 
            // BTHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 558);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "BTHistory";
            this.Text = "system";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BTHistory_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gboxOPFsettings.ResumeLayout(false);
            this.gboxOPFsettings.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.gb_filter.ResumeLayout(false);
            this.gb_filter.PerformLayout();
            this.groupBox5.ResumeLayout(false);
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
        private System.Windows.Forms.Button bt_all;
        private System.Windows.Forms.Label lb_NumSel;
        private System.Windows.Forms.Label lb_nApps;
        private System.Windows.Forms.Label lb_nProj;
        private System.Windows.Forms.Button btnCheckNext;
        private System.Windows.Forms.RadioButton rbIdle;
        private System.Windows.Forms.Label lbSeriesTime;
        private System.Windows.Forms.Button btnCheckPrev;
        private System.Windows.Forms.GroupBox gboxOPFsettings;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbUseCVS;
        private System.Windows.Forms.RadioButton rbUseCVS1;
        private System.Windows.Forms.ProgressBar pbarLoading;
        private System.Windows.Forms.Timer TimerShowBuild;
        private System.Windows.Forms.Label lblBuildDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tboxLimit;
        private System.Windows.Forms.CheckBox cboxStopLoad;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnPlotET;
        private System.Windows.Forms.Button btnPlot;
        private System.Windows.Forms.Button btnScatSets;
    }
}

