namespace InvalidAnalysis
{
    partial class InfoForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tv_projapps = new System.Windows.Forms.TreeView();
            this.gb_Reveal = new System.Windows.Forms.GroupBox();
            this.rbExpandAll = new System.Windows.Forms.RadioButton();
            this.rbShowSuccess = new System.Windows.Forms.RadioButton();
            this.rbShowFailed = new System.Windows.Forms.RadioButton();
            this.rb_ShowAll = new System.Windows.Forms.RadioButton();
            this.gBoxDevices = new System.Windows.Forms.GroupBox();
            this.cbCPU = new System.Windows.Forms.CheckBox();
            this.cbINTEL = new System.Windows.Forms.CheckBox();
            this.cbNVIDIA = new System.Windows.Forms.CheckBox();
            this.cbATI = new System.Windows.Forms.CheckBox();
            this.gBoxPlatforms = new System.Windows.Forms.GroupBox();
            this.cbAND = new System.Windows.Forms.CheckBox();
            this.cbLINUX = new System.Windows.Forms.CheckBox();
            this.cbAPPLE = new System.Windows.Forms.CheckBox();
            this.cbWIN = new System.Windows.Forms.CheckBox();
            this.btnApplyFilter = new System.Windows.Forms.Button();
            this.gBoxFilters = new System.Windows.Forms.GroupBox();
            this.btnSaveFilters = new System.Windows.Forms.Button();
            this.btnClrFilter = new System.Windows.Forms.Button();
            this.btnSetFilters = new System.Windows.Forms.Button();
            this.rbIncon = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.gb_Reveal.SuspendLayout();
            this.gBoxDevices.SuspendLayout();
            this.gBoxPlatforms.SuspendLayout();
            this.gBoxFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tv_projapps);
            this.panel1.Location = new System.Drawing.Point(241, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(608, 572);
            this.panel1.TabIndex = 2;
            // 
            // tv_projapps
            // 
            this.tv_projapps.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tv_projapps.Location = new System.Drawing.Point(18, 19);
            this.tv_projapps.Name = "tv_projapps";
            this.tv_projapps.Size = new System.Drawing.Size(568, 533);
            this.tv_projapps.TabIndex = 0;
            // 
            // gb_Reveal
            // 
            this.gb_Reveal.Controls.Add(this.rbIncon);
            this.gb_Reveal.Controls.Add(this.rbExpandAll);
            this.gb_Reveal.Controls.Add(this.rbShowSuccess);
            this.gb_Reveal.Controls.Add(this.rbShowFailed);
            this.gb_Reveal.Controls.Add(this.rb_ShowAll);
            this.gb_Reveal.Location = new System.Drawing.Point(12, 23);
            this.gb_Reveal.Name = "gb_Reveal";
            this.gb_Reveal.Size = new System.Drawing.Size(210, 174);
            this.gb_Reveal.TabIndex = 1;
            this.gb_Reveal.TabStop = false;
            this.gb_Reveal.Text = "Display Options";
            // 
            // rbExpandAll
            // 
            this.rbExpandAll.AutoSize = true;
            this.rbExpandAll.Location = new System.Drawing.Point(18, 42);
            this.rbExpandAll.Name = "rbExpandAll";
            this.rbExpandAll.Size = new System.Drawing.Size(110, 17);
            this.rbExpandAll.TabIndex = 3;
            this.rbExpandAll.Tag = "1";
            this.rbExpandAll.Text = "Show All (expand)";
            this.rbExpandAll.UseVisualStyleBackColor = true;
            this.rbExpandAll.CheckedChanged += new System.EventHandler(this.rbExpandAll_CheckedChanged);
            // 
            // rbShowSuccess
            // 
            this.rbShowSuccess.AutoSize = true;
            this.rbShowSuccess.Location = new System.Drawing.Point(18, 88);
            this.rbShowSuccess.Name = "rbShowSuccess";
            this.rbShowSuccess.Size = new System.Drawing.Size(142, 17);
            this.rbShowSuccess.TabIndex = 2;
            this.rbShowSuccess.Tag = "3";
            this.rbShowSuccess.Text = "Show all that succeeded";
            this.rbShowSuccess.UseVisualStyleBackColor = true;
            this.rbShowSuccess.CheckedChanged += new System.EventHandler(this.rbShowUnk_CheckedChanged);
            // 
            // rbShowFailed
            // 
            this.rbShowFailed.AutoSize = true;
            this.rbShowFailed.Location = new System.Drawing.Point(18, 65);
            this.rbShowFailed.Name = "rbShowFailed";
            this.rbShowFailed.Size = new System.Drawing.Size(122, 17);
            this.rbShowFailed.TabIndex = 1;
            this.rbShowFailed.Tag = "2";
            this.rbShowFailed.Text = "Just those that failed";
            this.rbShowFailed.UseVisualStyleBackColor = true;
            this.rbShowFailed.CheckedChanged += new System.EventHandler(this.rbShowHis_CheckedChanged);
            // 
            // rb_ShowAll
            // 
            this.rb_ShowAll.AutoSize = true;
            this.rb_ShowAll.Checked = true;
            this.rb_ShowAll.Location = new System.Drawing.Point(18, 19);
            this.rb_ShowAll.Name = "rb_ShowAll";
            this.rb_ShowAll.Size = new System.Drawing.Size(120, 17);
            this.rb_ShowAll.TabIndex = 0;
            this.rb_ShowAll.TabStop = true;
            this.rb_ShowAll.Tag = "0";
            this.rb_ShowAll.Text = "Show All (collapsed)";
            this.rb_ShowAll.UseVisualStyleBackColor = true;
            this.rb_ShowAll.CheckedChanged += new System.EventHandler(this.rbShowAll_CheckedChanged);
            // 
            // gBoxDevices
            // 
            this.gBoxDevices.Controls.Add(this.cbCPU);
            this.gBoxDevices.Controls.Add(this.cbINTEL);
            this.gBoxDevices.Controls.Add(this.cbNVIDIA);
            this.gBoxDevices.Controls.Add(this.cbATI);
            this.gBoxDevices.Location = new System.Drawing.Point(10, 234);
            this.gBoxDevices.Name = "gBoxDevices";
            this.gBoxDevices.Size = new System.Drawing.Size(135, 124);
            this.gBoxDevices.TabIndex = 4;
            this.gBoxDevices.TabStop = false;
            this.gBoxDevices.Text = "Devices";
            // 
            // cbCPU
            // 
            this.cbCPU.AutoSize = true;
            this.cbCPU.Checked = true;
            this.cbCPU.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCPU.Location = new System.Drawing.Point(21, 93);
            this.cbCPU.Name = "cbCPU";
            this.cbCPU.Size = new System.Drawing.Size(48, 17);
            this.cbCPU.TabIndex = 3;
            this.cbCPU.Text = "CPU";
            this.cbCPU.UseVisualStyleBackColor = true;
            // 
            // cbINTEL
            // 
            this.cbINTEL.AutoSize = true;
            this.cbINTEL.Checked = true;
            this.cbINTEL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbINTEL.Location = new System.Drawing.Point(21, 70);
            this.cbINTEL.Name = "cbINTEL";
            this.cbINTEL.Size = new System.Drawing.Size(46, 17);
            this.cbINTEL.TabIndex = 2;
            this.cbINTEL.Text = "Intel";
            this.cbINTEL.UseVisualStyleBackColor = true;
            // 
            // cbNVIDIA
            // 
            this.cbNVIDIA.AutoSize = true;
            this.cbNVIDIA.Checked = true;
            this.cbNVIDIA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNVIDIA.Location = new System.Drawing.Point(21, 47);
            this.cbNVIDIA.Name = "cbNVIDIA";
            this.cbNVIDIA.Size = new System.Drawing.Size(55, 17);
            this.cbNVIDIA.TabIndex = 1;
            this.cbNVIDIA.Text = "nVidia";
            this.cbNVIDIA.UseVisualStyleBackColor = true;
            // 
            // cbATI
            // 
            this.cbATI.AutoSize = true;
            this.cbATI.Checked = true;
            this.cbATI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbATI.Location = new System.Drawing.Point(21, 24);
            this.cbATI.Name = "cbATI";
            this.cbATI.Size = new System.Drawing.Size(43, 17);
            this.cbATI.TabIndex = 0;
            this.cbATI.Text = "ATI";
            this.cbATI.UseVisualStyleBackColor = true;
            // 
            // gBoxPlatforms
            // 
            this.gBoxPlatforms.Controls.Add(this.cbAND);
            this.gBoxPlatforms.Controls.Add(this.cbLINUX);
            this.gBoxPlatforms.Controls.Add(this.cbAPPLE);
            this.gBoxPlatforms.Controls.Add(this.cbWIN);
            this.gBoxPlatforms.Location = new System.Drawing.Point(10, 92);
            this.gBoxPlatforms.Name = "gBoxPlatforms";
            this.gBoxPlatforms.Size = new System.Drawing.Size(135, 124);
            this.gBoxPlatforms.TabIndex = 5;
            this.gBoxPlatforms.TabStop = false;
            this.gBoxPlatforms.Text = "OS platform";
            // 
            // cbAND
            // 
            this.cbAND.AutoSize = true;
            this.cbAND.Checked = true;
            this.cbAND.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAND.Location = new System.Drawing.Point(21, 100);
            this.cbAND.Name = "cbAND";
            this.cbAND.Size = new System.Drawing.Size(62, 17);
            this.cbAND.TabIndex = 3;
            this.cbAND.Text = "Android";
            this.cbAND.UseVisualStyleBackColor = true;
            // 
            // cbLINUX
            // 
            this.cbLINUX.AutoSize = true;
            this.cbLINUX.Checked = true;
            this.cbLINUX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLINUX.Location = new System.Drawing.Point(21, 77);
            this.cbLINUX.Name = "cbLINUX";
            this.cbLINUX.Size = new System.Drawing.Size(51, 17);
            this.cbLINUX.TabIndex = 2;
            this.cbLINUX.Text = "Linux";
            this.cbLINUX.UseVisualStyleBackColor = true;
            // 
            // cbAPPLE
            // 
            this.cbAPPLE.AutoSize = true;
            this.cbAPPLE.Checked = true;
            this.cbAPPLE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAPPLE.Location = new System.Drawing.Point(21, 54);
            this.cbAPPLE.Name = "cbAPPLE";
            this.cbAPPLE.Size = new System.Drawing.Size(53, 17);
            this.cbAPPLE.TabIndex = 1;
            this.cbAPPLE.Text = "Apple";
            this.cbAPPLE.UseVisualStyleBackColor = true;
            // 
            // cbWIN
            // 
            this.cbWIN.AutoSize = true;
            this.cbWIN.Checked = true;
            this.cbWIN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWIN.Location = new System.Drawing.Point(21, 31);
            this.cbWIN.Name = "cbWIN";
            this.cbWIN.Size = new System.Drawing.Size(68, 17);
            this.cbWIN.TabIndex = 0;
            this.cbWIN.Text = "Wndows";
            this.cbWIN.UseVisualStyleBackColor = true;
            // 
            // btnApplyFilter
            // 
            this.btnApplyFilter.Location = new System.Drawing.Point(11, 19);
            this.btnApplyFilter.Name = "btnApplyFilter";
            this.btnApplyFilter.Size = new System.Drawing.Size(88, 23);
            this.btnApplyFilter.TabIndex = 6;
            this.btnApplyFilter.Text = "Apply Filters";
            this.btnApplyFilter.UseVisualStyleBackColor = true;
            this.btnApplyFilter.Click += new System.EventHandler(this.btnApplyFilter_Click);
            // 
            // gBoxFilters
            // 
            this.gBoxFilters.Controls.Add(this.btnSaveFilters);
            this.gBoxFilters.Controls.Add(this.btnClrFilter);
            this.gBoxFilters.Controls.Add(this.btnSetFilters);
            this.gBoxFilters.Controls.Add(this.btnApplyFilter);
            this.gBoxFilters.Controls.Add(this.gBoxPlatforms);
            this.gBoxFilters.Controls.Add(this.gBoxDevices);
            this.gBoxFilters.Location = new System.Drawing.Point(12, 219);
            this.gBoxFilters.Name = "gBoxFilters";
            this.gBoxFilters.Size = new System.Drawing.Size(210, 376);
            this.gBoxFilters.TabIndex = 3;
            this.gBoxFilters.TabStop = false;
            this.gBoxFilters.Text = "Filters";
            // 
            // btnSaveFilters
            // 
            this.btnSaveFilters.Location = new System.Drawing.Point(11, 48);
            this.btnSaveFilters.Name = "btnSaveFilters";
            this.btnSaveFilters.Size = new System.Drawing.Size(88, 23);
            this.btnSaveFilters.TabIndex = 9;
            this.btnSaveFilters.Text = "Save Settings";
            this.btnSaveFilters.UseVisualStyleBackColor = true;
            this.btnSaveFilters.Click += new System.EventHandler(this.btnSaveFilters_Click);
            // 
            // btnClrFilter
            // 
            this.btnClrFilter.Location = new System.Drawing.Point(128, 48);
            this.btnClrFilter.Name = "btnClrFilter";
            this.btnClrFilter.Size = new System.Drawing.Size(56, 23);
            this.btnClrFilter.TabIndex = 8;
            this.btnClrFilter.Text = "Clear";
            this.btnClrFilter.UseVisualStyleBackColor = true;
            this.btnClrFilter.Click += new System.EventHandler(this.btnClrFilter_Click);
            // 
            // btnSetFilters
            // 
            this.btnSetFilters.Location = new System.Drawing.Point(128, 19);
            this.btnSetFilters.Name = "btnSetFilters";
            this.btnSetFilters.Size = new System.Drawing.Size(56, 23);
            this.btnSetFilters.TabIndex = 7;
            this.btnSetFilters.Text = "Set";
            this.btnSetFilters.UseVisualStyleBackColor = true;
            this.btnSetFilters.Click += new System.EventHandler(this.btnSetFilters_Click);
            // 
            // rbIncon
            // 
            this.rbIncon.AutoSize = true;
            this.rbIncon.Location = new System.Drawing.Point(18, 111);
            this.rbIncon.Name = "rbIncon";
            this.rbIncon.Size = new System.Drawing.Size(85, 17);
            this.rbIncon.TabIndex = 4;
            this.rbIncon.TabStop = true;
            this.rbIncon.Tag = "4";
            this.rbIncon.Text = "Inconclusive";
            this.rbIncon.UseVisualStyleBackColor = true;
            this.rbIncon.CheckedChanged += new System.EventHandler(this.rbIncon_CheckedChanged);
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 607);
            this.Controls.Add(this.gBoxFilters);
            this.Controls.Add(this.gb_Reveal);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "InfoForm";
            this.Text = "InfoForm";
            this.panel1.ResumeLayout(false);
            this.gb_Reveal.ResumeLayout(false);
            this.gb_Reveal.PerformLayout();
            this.gBoxDevices.ResumeLayout(false);
            this.gBoxDevices.PerformLayout();
            this.gBoxPlatforms.ResumeLayout(false);
            this.gBoxPlatforms.PerformLayout();
            this.gBoxFilters.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView tv_projapps;
        private System.Windows.Forms.GroupBox gb_Reveal;
        private System.Windows.Forms.RadioButton rbShowSuccess;
        private System.Windows.Forms.RadioButton rbShowFailed;
        private System.Windows.Forms.RadioButton rb_ShowAll;
        private System.Windows.Forms.RadioButton rbExpandAll;
        private System.Windows.Forms.GroupBox gBoxPlatforms;
        private System.Windows.Forms.CheckBox cbAND;
        private System.Windows.Forms.CheckBox cbLINUX;
        private System.Windows.Forms.CheckBox cbAPPLE;
        private System.Windows.Forms.CheckBox cbWIN;
        private System.Windows.Forms.GroupBox gBoxDevices;
        private System.Windows.Forms.CheckBox cbCPU;
        private System.Windows.Forms.CheckBox cbINTEL;
        private System.Windows.Forms.CheckBox cbNVIDIA;
        private System.Windows.Forms.CheckBox cbATI;
        private System.Windows.Forms.Button btnApplyFilter;
        private System.Windows.Forms.GroupBox gBoxFilters;
        private System.Windows.Forms.Button btnSetFilters;
        private System.Windows.Forms.Button btnClrFilter;
        private System.Windows.Forms.Button btnSaveFilters;
        private System.Windows.Forms.RadioButton rbIncon;
    }
}