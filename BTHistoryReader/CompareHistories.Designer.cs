namespace BTHistoryReader
{
    partial class CompareHistories
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rbScatProj = new System.Windows.Forms.RadioButton();
            this.rbScatApps = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.BtnInvSelApp = new System.Windows.Forms.Button();
            this.BtnClrAllApp = new System.Windows.Forms.Button();
            this.btnSelAllApp = new System.Windows.Forms.Button();
            this.LBoxApps = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.LBoxProjects = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbElapsedTime = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbSelect = new System.Windows.Forms.RadioButton();
            this.lbEditTab = new System.Windows.Forms.Label();
            this.TBoxStats = new System.Windows.Forms.TextBox();
            this.LViewConc = new System.Windows.Forms.ListView();
            this.colUSE = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSysName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnApply = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.BtnCmpSave = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnReset = new System.Windows.Forms.Button();
            this.btnShowScatter = new System.Windows.Forms.Button();
            this.tbSumCnt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblWarnApply = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboxFL = new System.Windows.Forms.CheckBox();
            this.TBoxResults = new System.Windows.Forms.TextBox();
            this.lblWarnApps = new System.Windows.Forms.Label();
            this.lbAdvFilter = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Ivory;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lbAdvFilter);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.ForeColor = System.Drawing.Color.Blue;
            this.panel1.Location = new System.Drawing.Point(12, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(937, 203);
            this.panel1.TabIndex = 1;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rbScatProj);
            this.groupBox5.Controls.Add(this.rbScatApps);
            this.groupBox5.Location = new System.Drawing.Point(749, 18);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(159, 127);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Stats and Plots";
            // 
            // rbScatProj
            // 
            this.rbScatProj.AutoSize = true;
            this.rbScatProj.Location = new System.Drawing.Point(12, 82);
            this.rbScatProj.Name = "rbScatProj";
            this.rbScatProj.Size = new System.Drawing.Size(101, 17);
            this.rbScatProj.TabIndex = 1;
            this.rbScatProj.Text = "Scatter Systems";
            this.toolTip1.SetToolTip(this.rbScatProj, "Only useful with ore than 1 system ");
            this.rbScatProj.UseVisualStyleBackColor = true;
            this.rbScatProj.CheckedChanged += new System.EventHandler(this.rbScatProj_CheckedChanged);
            // 
            // rbScatApps
            // 
            this.rbScatApps.AutoSize = true;
            this.rbScatApps.Checked = true;
            this.rbScatApps.Location = new System.Drawing.Point(12, 35);
            this.rbScatApps.Name = "rbScatApps";
            this.rbScatApps.Size = new System.Drawing.Size(118, 17);
            this.rbScatApps.TabIndex = 0;
            this.rbScatApps.TabStop = true;
            this.rbScatApps.Text = "Scatter Thsse Apps";
            this.toolTip1.SetToolTip(this.rbScatApps, "Only useful with more than one app");
            this.rbScatApps.UseVisualStyleBackColor = true;
            this.rbScatApps.CheckedChanged += new System.EventHandler(this.rbScatApps_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(714, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "FOR";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.BtnInvSelApp);
            this.groupBox4.Controls.Add(this.BtnClrAllApp);
            this.groupBox4.Controls.Add(this.btnSelAllApp);
            this.groupBox4.Controls.Add(this.LBoxApps);
            this.groupBox4.Location = new System.Drawing.Point(483, 14);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(225, 174);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Applications";
            // 
            // BtnInvSelApp
            // 
            this.BtnInvSelApp.Location = new System.Drawing.Point(150, 145);
            this.BtnInvSelApp.Name = "BtnInvSelApp";
            this.BtnInvSelApp.Size = new System.Drawing.Size(68, 23);
            this.BtnInvSelApp.TabIndex = 5;
            this.BtnInvSelApp.Text = "Invert Sel";
            this.BtnInvSelApp.UseVisualStyleBackColor = true;
            this.BtnInvSelApp.Click += new System.EventHandler(this.BtnInvSelApp_Click);
            // 
            // BtnClrAllApp
            // 
            this.BtnClrAllApp.Location = new System.Drawing.Point(68, 145);
            this.BtnClrAllApp.Name = "BtnClrAllApp";
            this.BtnClrAllApp.Size = new System.Drawing.Size(44, 23);
            this.BtnClrAllApp.TabIndex = 4;
            this.BtnClrAllApp.Text = "Clr All";
            this.BtnClrAllApp.UseVisualStyleBackColor = true;
            this.BtnClrAllApp.Click += new System.EventHandler(this.BtnClrAllApp_Click);
            // 
            // btnSelAllApp
            // 
            this.btnSelAllApp.Location = new System.Drawing.Point(6, 145);
            this.btnSelAllApp.Name = "btnSelAllApp";
            this.btnSelAllApp.Size = new System.Drawing.Size(44, 23);
            this.btnSelAllApp.TabIndex = 3;
            this.btnSelAllApp.Text = "Sel All";
            this.btnSelAllApp.UseVisualStyleBackColor = true;
            this.btnSelAllApp.Click += new System.EventHandler(this.btnSelAllApp_Click);
            // 
            // LBoxApps
            // 
            this.LBoxApps.BackColor = System.Drawing.Color.Cornsilk;
            this.LBoxApps.FormattingEnabled = true;
            this.LBoxApps.HorizontalScrollbar = true;
            this.LBoxApps.Location = new System.Drawing.Point(6, 19);
            this.LBoxApps.Name = "LBoxApps";
            this.LBoxApps.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.LBoxApps.Size = new System.Drawing.Size(212, 108);
            this.LBoxApps.Sorted = true;
            this.LBoxApps.TabIndex = 2;
            this.LBoxApps.SelectedIndexChanged += new System.EventHandler(this.LBoxApps_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(441, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "WITH";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.LBoxProjects);
            this.groupBox3.Location = new System.Drawing.Point(297, 14);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(138, 143);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Projects";
            // 
            // LBoxProjects
            // 
            this.LBoxProjects.BackColor = System.Drawing.Color.Cornsilk;
            this.LBoxProjects.FormattingEnabled = true;
            this.LBoxProjects.Location = new System.Drawing.Point(6, 19);
            this.LBoxProjects.Name = "LBoxProjects";
            this.LBoxProjects.Size = new System.Drawing.Size(124, 108);
            this.LBoxProjects.Sorted = true;
            this.LBoxProjects.TabIndex = 2;
            this.LBoxProjects.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.LBoxProjects_DrawItem);
            this.LBoxProjects.SelectedIndexChanged += new System.EventHandler(this.LBoxProjects_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(253, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "FROM";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbElapsedTime);
            this.groupBox2.Location = new System.Drawing.Point(130, 18);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(117, 120);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Results";
            // 
            // rbElapsedTime
            // 
            this.rbElapsedTime.AutoSize = true;
            this.rbElapsedTime.Checked = true;
            this.rbElapsedTime.Location = new System.Drawing.Point(22, 31);
            this.rbElapsedTime.Name = "rbElapsedTime";
            this.rbElapsedTime.Size = new System.Drawing.Size(89, 17);
            this.rbElapsedTime.TabIndex = 1;
            this.rbElapsedTime.TabStop = true;
            this.rbElapsedTime.Text = "Elapsed Time";
            this.rbElapsedTime.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSelect);
            this.groupBox1.Location = new System.Drawing.Point(21, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(85, 120);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operations";
            // 
            // rbSelect
            // 
            this.rbSelect.AutoSize = true;
            this.rbSelect.Checked = true;
            this.rbSelect.Location = new System.Drawing.Point(6, 31);
            this.rbSelect.Name = "rbSelect";
            this.rbSelect.Size = new System.Drawing.Size(66, 17);
            this.rbSelect.TabIndex = 0;
            this.rbSelect.TabStop = true;
            this.rbSelect.Text = "SELECT";
            this.rbSelect.UseVisualStyleBackColor = true;
            // 
            // lbEditTab
            // 
            this.lbEditTab.AutoSize = true;
            this.lbEditTab.ForeColor = System.Drawing.Color.Crimson;
            this.lbEditTab.Location = new System.Drawing.Point(140, 235);
            this.lbEditTab.Name = "lbEditTab";
            this.lbEditTab.Size = new System.Drawing.Size(204, 26);
            this.lbEditTab.TabIndex = 7;
            this.lbEditTab.Text = "ASSUMES ONE WU PER GPU UNLESS\r\nYOU EDIT VALUE IN FIRST COLUMN";
            // 
            // TBoxStats
            // 
            this.TBoxStats.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBoxStats.Location = new System.Drawing.Point(251, 19);
            this.TBoxStats.Multiline = true;
            this.TBoxStats.Name = "TBoxStats";
            this.TBoxStats.ReadOnly = true;
            this.TBoxStats.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TBoxStats.Size = new System.Drawing.Size(178, 150);
            this.TBoxStats.TabIndex = 9;
            this.TBoxStats.WordWrap = false;
            // 
            // LViewConc
            // 
            this.LViewConc.CheckBoxes = true;
            this.LViewConc.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUSE,
            this.colSysName,
            this.colCount});
            this.LViewConc.FullRowSelect = true;
            this.LViewConc.GridLines = true;
            this.LViewConc.HideSelection = false;
            this.LViewConc.LabelEdit = true;
            this.LViewConc.LabelWrap = false;
            this.LViewConc.Location = new System.Drawing.Point(143, 273);
            this.LViewConc.MultiSelect = false;
            this.LViewConc.Name = "LViewConc";
            this.LViewConc.Size = new System.Drawing.Size(338, 142);
            this.LViewConc.TabIndex = 10;
            this.LViewConc.UseCompatibleStateImageBehavior = false;
            this.LViewConc.View = System.Windows.Forms.View.Details;
            this.LViewConc.SelectedIndexChanged += new System.EventHandler(this.LViewConc_SelectedIndexChanged);
            // 
            // colUSE
            // 
            this.colUSE.Text = "UseInStats / NumConcurrent";
            this.colUSE.Width = 155;
            // 
            // colSysName
            // 
            this.colSysName.Text = "System";
            this.colSysName.Width = 115;
            // 
            // colCount
            // 
            this.colCount.Text = "Count";
            this.colCount.Width = 57;
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.ForeColor = System.Drawing.Color.Black;
            this.btnApply.Location = new System.Drawing.Point(12, 316);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(107, 23);
            this.btnApply.TabIndex = 11;
            this.btnApply.Text = "Apply Changes";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Visible = false;
            this.btnApply.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.ForeColor = System.Drawing.Color.Black;
            this.btnHelp.Location = new System.Drawing.Point(12, 273);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 12;
            this.btnHelp.Text = "HELP";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // BtnCmpSave
            // 
            this.BtnCmpSave.Location = new System.Drawing.Point(730, 420);
            this.BtnCmpSave.Name = "BtnCmpSave";
            this.BtnCmpSave.Size = new System.Drawing.Size(75, 23);
            this.BtnCmpSave.TabIndex = 13;
            this.BtnCmpSave.Text = "Save Results";
            this.BtnCmpSave.UseVisualStyleBackColor = true;
            this.BtnCmpSave.Click += new System.EventHandler(this.BtnCmpSave_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnReset.ForeColor = System.Drawing.Color.DarkRed;
            this.btnReset.Location = new System.Drawing.Point(12, 392);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 15;
            this.btnReset.Text = "Reset All";
            this.toolTip1.SetToolTip(this.btnReset, "check all and set number concurrent to 1\r\nincludeing those not shown in the stats" +
        " box");
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnShowScatter
            // 
            this.btnShowScatter.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnShowScatter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowScatter.ForeColor = System.Drawing.Color.Black;
            this.btnShowScatter.Location = new System.Drawing.Point(12, 354);
            this.btnShowScatter.Name = "btnShowScatter";
            this.btnShowScatter.Size = new System.Drawing.Size(107, 23);
            this.btnShowScatter.TabIndex = 16;
            this.btnShowScatter.Text = "Show Scatter";
            this.toolTip1.SetToolTip(this.btnShowScatter, "Scatter plot of apps or projects");
            this.btnShowScatter.UseVisualStyleBackColor = false;
            this.btnShowScatter.Visible = false;
            this.btnShowScatter.Click += new System.EventHandler(this.btnShowScatter_Click);
            // 
            // tbSumCnt
            // 
            this.tbSumCnt.ForeColor = System.Drawing.Color.Blue;
            this.tbSumCnt.Location = new System.Drawing.Point(132, 19);
            this.tbSumCnt.Name = "tbSumCnt";
            this.tbSumCnt.Size = new System.Drawing.Size(45, 20);
            this.tbSumCnt.TabIndex = 11;
            this.tbSumCnt.Text = "100";
            this.toolTip1.SetToolTip(this.tbSumCnt, "If over 1000 your system may not repond for a while");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(493, 441);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(228, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "All time values are in minutes or fraction thereof";
            // 
            // lblWarnApply
            // 
            this.lblWarnApply.AutoSize = true;
            this.lblWarnApply.ForeColor = System.Drawing.Color.Red;
            this.lblWarnApply.Location = new System.Drawing.Point(140, 424);
            this.lblWarnApply.Name = "lblWarnApply";
            this.lblWarnApply.Size = new System.Drawing.Size(122, 13);
            this.lblWarnApply.TabIndex = 17;
            this.lblWarnApply.Text = "Be sure to apply change";
            this.lblWarnApply.Visible = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.tbSumCnt);
            this.groupBox6.Controls.Add(this.cboxFL);
            this.groupBox6.Controls.Add(this.TBoxResults);
            this.groupBox6.Controls.Add(this.TBoxStats);
            this.groupBox6.Location = new System.Drawing.Point(502, 240);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(447, 175);
            this.groupBox6.TabIndex = 18;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Values / Summery and Results";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "(Clck apply if changed)";
            // 
            // cboxFL
            // 
            this.cboxFL.AutoSize = true;
            this.cboxFL.Checked = true;
            this.cboxFL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxFL.ForeColor = System.Drawing.Color.Blue;
            this.cboxFL.Location = new System.Drawing.Point(17, 19);
            this.cboxFL.Name = "cboxFL";
            this.cboxFL.Size = new System.Drawing.Size(109, 17);
            this.cboxFL.TabIndex = 10;
            this.cboxFL.Text = "Summarize if over";
            this.cboxFL.UseVisualStyleBackColor = true;
            // 
            // TBoxResults
            // 
            this.TBoxResults.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBoxResults.Location = new System.Drawing.Point(17, 65);
            this.TBoxResults.Multiline = true;
            this.TBoxResults.Name = "TBoxResults";
            this.TBoxResults.ReadOnly = true;
            this.TBoxResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TBoxResults.Size = new System.Drawing.Size(202, 104);
            this.TBoxResults.TabIndex = 9;
            this.TBoxResults.WordWrap = false;
            // 
            // lblWarnApps
            // 
            this.lblWarnApps.AutoSize = true;
            this.lblWarnApps.ForeColor = System.Drawing.Color.Red;
            this.lblWarnApps.Location = new System.Drawing.Point(140, 444);
            this.lblWarnApps.Name = "lblWarnApps";
            this.lblWarnApps.Size = new System.Drawing.Size(230, 13);
            this.lblWarnApps.TabIndex = 19;
            this.lblWarnApps.Text = "Cannot edit table if more than one app selected";
            // 
            // lbAdvFilter
            // 
            this.lbAdvFilter.AutoSize = true;
            this.lbAdvFilter.BackColor = System.Drawing.SystemColors.Info;
            this.lbAdvFilter.Location = new System.Drawing.Point(24, 169);
            this.lbAdvFilter.Name = "lbAdvFilter";
            this.lbAdvFilter.Size = new System.Drawing.Size(135, 13);
            this.lbAdvFilter.TabIndex = 17;
            this.lbAdvFilter.Text = "Reserve for filter info, if any";
            // 
            // CompareHistories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(961, 463);
            this.Controls.Add(this.lblWarnApps);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.lblWarnApply);
            this.Controls.Add(this.btnShowScatter);
            this.Controls.Add(this.lbEditTab);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BtnCmpSave);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.LViewConc);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.CadetBlue;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "CompareHistories";
            this.Text = "CompareHistories";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox LBoxApps;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox LBoxProjects;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbElapsedTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbSelect;
        private System.Windows.Forms.TextBox TBoxStats;
        private System.Windows.Forms.Label lbEditTab;
        private System.Windows.Forms.ListView LViewConc;
        private System.Windows.Forms.ColumnHeader colUSE;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.ColumnHeader colSysName;
        private System.Windows.Forms.Button BtnCmpSave;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rbScatProj;
        private System.Windows.Forms.RadioButton rbScatApps;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnShowScatter;
        private System.Windows.Forms.Label lblWarnApply;
        private System.Windows.Forms.ColumnHeader colCount;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox TBoxResults;
        private System.Windows.Forms.CheckBox cboxFL;
        private System.Windows.Forms.TextBox tbSumCnt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BtnInvSelApp;
        private System.Windows.Forms.Button BtnClrAllApp;
        private System.Windows.Forms.Button btnSelAllApp;
        private System.Windows.Forms.Label lblWarnApps;
        private System.Windows.Forms.Label lbAdvFilter;
    }
}