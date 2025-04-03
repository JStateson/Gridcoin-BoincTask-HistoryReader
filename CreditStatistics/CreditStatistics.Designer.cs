namespace CreditStatistics
{
    partial class CreditStatistics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreditStatistics));
            this.btnViewData = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.lbVersion = new System.Windows.Forms.Label();
            this.gbGetStats = new System.Windows.Forms.GroupBox();
            this.btCancel = new System.Windows.Forms.Button();
            this.pbTask = new System.Windows.Forms.ProgressBar();
            this.btnClear = new System.Windows.Forms.Button();
            this.lbHdr = new System.Windows.Forms.Label();
            this.cbfilterSTD = new System.Windows.Forms.CheckBox();
            this.ProjUrl = new System.Windows.Forms.TextBox();
            this.gbSamURL = new System.Windows.Forms.GroupBox();
            this.btnApplyName = new System.Windows.Forms.Button();
            this.tbHOSTID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbHdrInfo = new System.Windows.Forms.TextBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tcProj = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnClearURL = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnExtract = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tbProjHostList = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnReadBoinc = new System.Windows.Forms.Button();
            this.tbBoincLoc = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnSetSel = new System.Windows.Forms.Button();
            this.btnCleSel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btApplyDemo = new System.Windows.Forms.Button();
            this.tbSelected = new System.Windows.Forms.TextBox();
            this.lbSelectDemo = new System.Windows.Forms.ListBox();
            this.TaskTimer = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbGetStats.SuspendLayout();
            this.gbSamURL.SuspendLayout();
            this.tcProj.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnViewData
            // 
            this.btnViewData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewData.ForeColor = System.Drawing.Color.Blue;
            this.btnViewData.Location = new System.Drawing.Point(431, 33);
            this.btnViewData.Name = "btnViewData";
            this.btnViewData.Size = new System.Drawing.Size(133, 47);
            this.btnViewData.TabIndex = 2;
            this.btnViewData.Text = "Click to view\r\nthe below page";
            this.btnViewData.UseVisualStyleBackColor = true;
            this.btnViewData.Click += new System.EventHandler(this.btnViewData_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.Blue;
            this.btnStart.Location = new System.Drawing.Point(186, 33);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Next";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbInfo
            // 
            this.tbInfo.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbInfo.Location = new System.Drawing.Point(10, 138);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbInfo.Size = new System.Drawing.Size(692, 480);
            this.tbInfo.TabIndex = 4;
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(7, 80);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(41, 13);
            this.lbVersion.TabIndex = 9;
            this.lbVersion.Text = "version";
            // 
            // gbGetStats
            // 
            this.gbGetStats.Controls.Add(this.btCancel);
            this.gbGetStats.Controls.Add(this.pbTask);
            this.gbGetStats.Controls.Add(this.btnClear);
            this.gbGetStats.Controls.Add(this.lbHdr);
            this.gbGetStats.Controls.Add(this.cbfilterSTD);
            this.gbGetStats.Controls.Add(this.tbInfo);
            this.gbGetStats.Controls.Add(this.lbVersion);
            this.gbGetStats.Controls.Add(this.btnStart);
            this.gbGetStats.Location = new System.Drawing.Point(634, 54);
            this.gbGetStats.Name = "gbGetStats";
            this.gbGetStats.Size = new System.Drawing.Size(731, 657);
            this.gbGetStats.TabIndex = 10;
            this.gbGetStats.TabStop = false;
            this.gbGetStats.Text = "WU / Credit statistics";
            // 
            // btCancel
            // 
            this.btCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCancel.ForeColor = System.Drawing.Color.Blue;
            this.btCancel.Location = new System.Drawing.Point(186, 72);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 14;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // pbTask
            // 
            this.pbTask.Location = new System.Drawing.Point(307, 33);
            this.pbTask.Maximum = 60;
            this.pbTask.Name = "pbTask";
            this.pbTask.Size = new System.Drawing.Size(327, 23);
            this.pbTask.TabIndex = 13;
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.Blue;
            this.btnClear.Location = new System.Drawing.Point(10, 33);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lbHdr
            // 
            this.lbHdr.AutoSize = true;
            this.lbHdr.BackColor = System.Drawing.SystemColors.Info;
            this.lbHdr.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHdr.Location = new System.Drawing.Point(7, 106);
            this.lbHdr.Name = "lbHdr";
            this.lbHdr.Size = new System.Drawing.Size(49, 14);
            this.lbHdr.TabIndex = 11;
            this.lbHdr.Text = "label2";
            // 
            // cbfilterSTD
            // 
            this.cbfilterSTD.AutoSize = true;
            this.cbfilterSTD.Checked = true;
            this.cbfilterSTD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbfilterSTD.ForeColor = System.Drawing.Color.Red;
            this.cbfilterSTD.Location = new System.Drawing.Point(545, 76);
            this.cbfilterSTD.Name = "cbfilterSTD";
            this.cbfilterSTD.Size = new System.Drawing.Size(93, 17);
            this.cbfilterSTD.TabIndex = 10;
            this.cbfilterSTD.Text = "Filter out 2 SD";
            this.cbfilterSTD.UseVisualStyleBackColor = true;
            this.cbfilterSTD.CheckedChanged += new System.EventHandler(this.cbfilterSTD_CheckedChanged);
            // 
            // ProjUrl
            // 
            this.ProjUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjUrl.Location = new System.Drawing.Point(10, 103);
            this.ProjUrl.Multiline = true;
            this.ProjUrl.Name = "ProjUrl";
            this.ProjUrl.Size = new System.Drawing.Size(554, 47);
            this.ProjUrl.TabIndex = 0;
            this.ProjUrl.Text = "https://milkyway.cs.rpi.edu/milkyway/results.php?hostid=1046337&offset=0&show_nam" +
    "es=0&state=4&appid=";
            this.ProjUrl.TextChanged += new System.EventHandler(this.ProjUrl_TextChanged);
            // 
            // gbSamURL
            // 
            this.gbSamURL.Controls.Add(this.btnApplyName);
            this.gbSamURL.Controls.Add(this.tbHOSTID);
            this.gbSamURL.Controls.Add(this.label2);
            this.gbSamURL.Location = new System.Drawing.Point(23, 319);
            this.gbSamURL.Name = "gbSamURL";
            this.gbSamURL.Size = new System.Drawing.Size(428, 324);
            this.gbSamURL.TabIndex = 10;
            this.gbSamURL.TabStop = false;
            this.gbSamURL.Text = "Select Project";
            // 
            // btnApplyName
            // 
            this.btnApplyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplyName.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnApplyName.Location = new System.Drawing.Point(313, 33);
            this.btnApplyName.Name = "btnApplyName";
            this.btnApplyName.Size = new System.Drawing.Size(75, 23);
            this.btnApplyName.TabIndex = 9;
            this.btnApplyName.Text = "Apply";
            this.btnApplyName.UseVisualStyleBackColor = true;
            this.btnApplyName.Click += new System.EventHandler(this.btnApplyName_Click);
            // 
            // tbHOSTID
            // 
            this.tbHOSTID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbHOSTID.Location = new System.Drawing.Point(223, 34);
            this.tbHOSTID.Name = "tbHOSTID";
            this.tbHOSTID.Size = new System.Drawing.Size(84, 22);
            this.tbHOSTID.TabIndex = 8;
            this.tbHOSTID.Text = "12345";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Info;
            this.label2.Location = new System.Drawing.Point(16, 29);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 32);
            this.label2.TabIndex = 7;
            this.label2.Text = "Enter HOST ID and select\r\nproject then click apply";
            // 
            // tbHdrInfo
            // 
            this.tbHdrInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbHdrInfo.Location = new System.Drawing.Point(336, 200);
            this.tbHdrInfo.Multiline = true;
            this.tbHdrInfo.Name = "tbHdrInfo";
            this.tbHdrInfo.Size = new System.Drawing.Size(135, 82);
            this.tbHdrInfo.TabIndex = 8;
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.ForeColor = System.Drawing.Color.Blue;
            this.btnFind.Location = new System.Drawing.Point(485, 236);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(65, 23);
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "FIND";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Info;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 16);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(297, 52);
            this.label5.TabIndex = 6;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // tcProj
            // 
            this.tcProj.Controls.Add(this.tabPage1);
            this.tcProj.Controls.Add(this.tabPage2);
            this.tcProj.Controls.Add(this.tabPage3);
            this.tcProj.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcProj.Location = new System.Drawing.Point(12, 29);
            this.tcProj.Name = "tcProj";
            this.tcProj.SelectedIndex = 0;
            this.tcProj.Size = new System.Drawing.Size(592, 689);
            this.tcProj.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnPaste);
            this.tabPage1.Controls.Add(this.btnClearURL);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.tbPage);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btnExtract);
            this.tabPage1.Controls.Add(this.gbSamURL);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.tbHdrInfo);
            this.tabPage1.Controls.Add(this.ProjUrl);
            this.tabPage1.Controls.Add(this.btnFind);
            this.tabPage1.Controls.Add(this.btnViewData);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(584, 660);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Projects";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnPaste
            // 
            this.btnPaste.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPaste.ForeColor = System.Drawing.Color.Blue;
            this.btnPaste.Location = new System.Drawing.Point(260, 156);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(70, 20);
            this.btnPaste.TabIndex = 18;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnClearURL
            // 
            this.btnClearURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearURL.ForeColor = System.Drawing.Color.Blue;
            this.btnClearURL.Location = new System.Drawing.Point(170, 156);
            this.btnClearURL.Name = "btnClearURL";
            this.btnClearURL.Size = new System.Drawing.Size(70, 20);
            this.btnClearURL.TabIndex = 17;
            this.btnClearURL.Text = "CLEAR";
            this.btnClearURL.UseVisualStyleBackColor = true;
            this.btnClearURL.Click += new System.EventHandler(this.btnClearURL_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(230, 225);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 48);
            this.label1.TabIndex = 16;
            this.label1.Text = "Amount of data\r\nif project has a\r\nheader or empty";
            // 
            // tbPage
            // 
            this.tbPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPage.ForeColor = System.Drawing.Color.Blue;
            this.tbPage.Location = new System.Drawing.Point(18, 269);
            this.tbPage.Name = "tbPage";
            this.tbPage.Size = new System.Drawing.Size(79, 22);
            this.tbPage.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Info;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 239);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Selected Offset or Page";
            // 
            // btnExtract
            // 
            this.btnExtract.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExtract.ForeColor = System.Drawing.Color.Blue;
            this.btnExtract.Location = new System.Drawing.Point(18, 157);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(126, 45);
            this.btnExtract.TabIndex = 11;
            this.btnExtract.Text = "Click to verify\r\nthe above URL";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tbProjHostList);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(584, 660);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Host IDs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tbProjHostList
            // 
            this.tbProjHostList.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbProjHostList.ForeColor = System.Drawing.Color.Blue;
            this.tbProjHostList.Location = new System.Drawing.Point(17, 229);
            this.tbProjHostList.Multiline = true;
            this.tbProjHostList.Name = "tbProjHostList";
            this.tbProjHostList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbProjHostList.Size = new System.Drawing.Size(422, 385);
            this.tbProjHostList.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnReadBoinc);
            this.groupBox1.Controls.Add(this.tbBoincLoc);
            this.groupBox1.Location = new System.Drawing.Point(17, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 175);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "BOINC location, change if necessary";
            // 
            // btnReadBoinc
            // 
            this.btnReadBoinc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadBoinc.ForeColor = System.Drawing.Color.Blue;
            this.btnReadBoinc.Location = new System.Drawing.Point(105, 80);
            this.btnReadBoinc.Name = "btnReadBoinc";
            this.btnReadBoinc.Size = new System.Drawing.Size(122, 50);
            this.btnReadBoinc.TabIndex = 1;
            this.btnReadBoinc.Text = "Click to read\r\nyour Host IDs";
            this.btnReadBoinc.UseVisualStyleBackColor = true;
            this.btnReadBoinc.Click += new System.EventHandler(this.btnReadBoinc_Click);
            // 
            // tbBoincLoc
            // 
            this.tbBoincLoc.Location = new System.Drawing.Point(21, 31);
            this.tbBoincLoc.Name = "tbBoincLoc";
            this.tbBoincLoc.Size = new System.Drawing.Size(386, 22);
            this.tbBoincLoc.TabIndex = 0;
            this.tbBoincLoc.Text = "c:\\programdata\\boinc";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnSetSel);
            this.tabPage3.Controls.Add(this.btnCleSel);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.btApplyDemo);
            this.tabPage3.Controls.Add(this.tbSelected);
            this.tabPage3.Controls.Add(this.lbSelectDemo);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(584, 660);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Top PCs";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnSetSel
            // 
            this.btnSetSel.Enabled = false;
            this.btnSetSel.ForeColor = System.Drawing.Color.Blue;
            this.btnSetSel.Location = new System.Drawing.Point(297, 163);
            this.btnSetSel.Name = "btnSetSel";
            this.btnSetSel.Size = new System.Drawing.Size(87, 23);
            this.btnSetSel.TabIndex = 5;
            this.btnSetSel.Text = "Select All";
            this.btnSetSel.UseVisualStyleBackColor = true;
            this.btnSetSel.Click += new System.EventHandler(this.btnSetSel_Click);
            // 
            // btnCleSel
            // 
            this.btnCleSel.Enabled = false;
            this.btnCleSel.ForeColor = System.Drawing.Color.Blue;
            this.btnCleSel.Location = new System.Drawing.Point(201, 163);
            this.btnCleSel.Name = "btnCleSel";
            this.btnCleSel.Size = new System.Drawing.Size(75, 23);
            this.btnCleSel.TabIndex = 4;
            this.btnCleSel.Text = "Clear";
            this.btnCleSel.UseVisualStyleBackColor = true;
            this.btnCleSel.Click += new System.EventHandler(this.btnCleSel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Coral;
            this.label4.Location = new System.Drawing.Point(12, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 64);
            this.label4.TabIndex = 3;
            this.label4.Text = "Not Programmed\r\n denis, yoyo, wcg\r\nIf you select a url it\r\ngoes into clipboard";
            // 
            // btApplyDemo
            // 
            this.btApplyDemo.Enabled = false;
            this.btApplyDemo.ForeColor = System.Drawing.Color.Blue;
            this.btApplyDemo.Location = new System.Drawing.Point(15, 16);
            this.btApplyDemo.Name = "btApplyDemo";
            this.btApplyDemo.Size = new System.Drawing.Size(75, 23);
            this.btApplyDemo.TabIndex = 2;
            this.btApplyDemo.Text = "Apply";
            this.btApplyDemo.UseVisualStyleBackColor = true;
            this.btApplyDemo.Click += new System.EventHandler(this.btApplyDemo_Click);
            // 
            // tbSelected
            // 
            this.tbSelected.ForeColor = System.Drawing.Color.Blue;
            this.tbSelected.Location = new System.Drawing.Point(201, 16);
            this.tbSelected.Multiline = true;
            this.tbSelected.Name = "tbSelected";
            this.tbSelected.Size = new System.Drawing.Size(337, 131);
            this.tbSelected.TabIndex = 1;
            // 
            // lbSelectDemo
            // 
            this.lbSelectDemo.FormattingEnabled = true;
            this.lbSelectDemo.ItemHeight = 16;
            this.lbSelectDemo.Items.AddRange(new object[] {
            "https://numberfields.asu.edu/NumberFields/results.php?hostid=2883430&state=4&offs" +
                "et=0",
            "https://main.cpdn.org/results.php?hostid=1556725&state=4&offset=0",
            "https://lhcathome.cern.ch/lhcathome/results.php?hostid=10835483&offset=0&state=4",
            "https://sech.me/boinc/Amicable/results.php?hostid=220390&state=4&offset=0",
            "https://asteroidsathome.net/boinc/results.php?hostid=726869&state=4&offset=0",
            "https://einsteinathome.org/host/12850601/tasks/4/0",
            "https://milkyway.cs.rpi.edu/milkyway/results.php?hostid=899300&offset=0&state=4",
            "https://moowrap.net/results.php?hostid=1324423&state=4&offset=0",
            "https://escatter11.fullerton.edu/nfs/results.php?hostid=7237359&state=4&offset=0",
            "https://srbase.my-firewall.org/sr5/results.php?hostid=226750&state=4&offset=0",
            "https://yafu.myfirewall.org/yafu/results.php?hostid=60890&state=4&offset=0",
            "https://boinc.loda-lang.org/loda/results.php?hostid=125&state=4&offset=0",
            "https://rake.boincfast.ru/rakesearch/results.php?hostid=21600&state=4&offset=0",
            "http://radioactiveathome.org/boinc/results.php?hostid=45736&state=3&offset=0",
            "https://www.sidock.si/sidock/results.php?hostid=51476&offset=0&show_names=0&state" +
                "=4&appid=",
            "https://rnma.xyz/boinc/results.php?hostid=23250&state=4&offset=0",
            "http://radioactiveathome.org/boinc/results.php?hostid=45736&offset=0&state=3",
            "https://boinc.progger.info/odlk/results.php?hostid=29350&state=4&offset=0",
            "https://www.primegrid.com/results.php?hostid=905516&offset=0&state=4",
            "https://gerasim.boinc.ru/users/viewHostResults.aspx?hostid=70676&opt=2"});
            this.lbSelectDemo.Location = new System.Drawing.Point(15, 192);
            this.lbSelectDemo.Name = "lbSelectDemo";
            this.lbSelectDemo.Size = new System.Drawing.Size(536, 452);
            this.lbSelectDemo.TabIndex = 0;
            this.lbSelectDemo.SelectedIndexChanged += new System.EventHandler(this.lbSelectDemo_SelectedIndexChanged);
            // 
            // TaskTimer
            // 
            this.TaskTimer.Interval = 1000;
            this.TaskTimer.Tick += new System.EventHandler(this.TaskTimer_Tick);
            // 
            // CreditStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1377, 744);
            this.Controls.Add(this.tcProj);
            this.Controls.Add(this.gbGetStats);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CreditStatistics";
            this.Text = "Credit Statistics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ErrorAnalysis_FormClosing);
            this.gbGetStats.ResumeLayout(false);
            this.gbGetStats.PerformLayout();
            this.gbSamURL.ResumeLayout(false);
            this.gbSamURL.PerformLayout();
            this.tcProj.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnViewData;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbInfo;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.GroupBox gbGetStats;
        private System.Windows.Forms.TextBox ProjUrl;
        private System.Windows.Forms.TextBox tbHdrInfo;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbfilterSTD;
        private System.Windows.Forms.Label lbHdr;
        private System.Windows.Forms.GroupBox gbSamURL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnApplyName;
        private System.Windows.Forms.TextBox tbHOSTID;
        private System.Windows.Forms.TabControl tcProj;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbProjHostList;
        private System.Windows.Forms.Button btnReadBoinc;
        private System.Windows.Forms.TextBox tbBoincLoc;
        private System.Windows.Forms.TextBox tbPage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ProgressBar pbTask;
        private System.Windows.Forms.Timer TaskTimer;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListBox lbSelectDemo;
        private System.Windows.Forms.Button btApplyDemo;
        private System.Windows.Forms.TextBox tbSelected;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSetSel;
        private System.Windows.Forms.Button btnCleSel;
        private System.Windows.Forms.Button btnClearURL;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

