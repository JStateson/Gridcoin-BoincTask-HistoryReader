namespace BTHistoryReader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoForm));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tv_projapps = new System.Windows.Forms.TreeView();
            this.gb_Reveal = new System.Windows.Forms.GroupBox();
            this.rbShowStats = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.rbShowUnk = new System.Windows.Forms.RadioButton();
            this.rbShowHis = new System.Windows.Forms.RadioButton();
            this.rb_ShowAll = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.gb_Reveal.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 247);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tv_projapps);
            this.panel1.Location = new System.Drawing.Point(335, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(426, 400);
            this.panel1.TabIndex = 2;
            // 
            // tv_projapps
            // 
            this.tv_projapps.Location = new System.Drawing.Point(33, 37);
            this.tv_projapps.Name = "tv_projapps";
            this.tv_projapps.Size = new System.Drawing.Size(363, 314);
            this.tv_projapps.TabIndex = 0;
            // 
            // gb_Reveal
            // 
            this.gb_Reveal.Controls.Add(this.rbShowStats);
            this.gb_Reveal.Controls.Add(this.radioButton1);
            this.gb_Reveal.Controls.Add(this.rbShowUnk);
            this.gb_Reveal.Controls.Add(this.rbShowHis);
            this.gb_Reveal.Controls.Add(this.rb_ShowAll);
            this.gb_Reveal.Location = new System.Drawing.Point(29, 274);
            this.gb_Reveal.Name = "gb_Reveal";
            this.gb_Reveal.Size = new System.Drawing.Size(203, 164);
            this.gb_Reveal.TabIndex = 1;
            this.gb_Reveal.TabStop = false;
            this.gb_Reveal.Text = "Reveal Project / Apps";
            // 
            // rbShowStats
            // 
            this.rbShowStats.AutoSize = true;
            this.rbShowStats.Location = new System.Drawing.Point(18, 111);
            this.rbShowStats.Name = "rbShowStats";
            this.rbShowStats.Size = new System.Drawing.Size(163, 17);
            this.rbShowStats.TabIndex = 4;
            this.rbShowStats.TabStop = true;
            this.rbShowStats.Tag = "4";
            this.rbShowStats.Text = "Show elapsed [num-Avg(std)]";
            this.rbShowStats.UseVisualStyleBackColor = true;
            this.rbShowStats.CheckedChanged += new System.EventHandler(this.rbShowStats_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(18, 42);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(110, 17);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.Tag = "1";
            this.radioButton1.Text = "Show All (expand)";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // rbShowUnk
            // 
            this.rbShowUnk.AutoSize = true;
            this.rbShowUnk.Location = new System.Drawing.Point(18, 88);
            this.rbShowUnk.Name = "rbShowUnk";
            this.rbShowUnk.Size = new System.Drawing.Size(125, 17);
            this.rbShowUnk.TabIndex = 2;
            this.rbShowUnk.Tag = "3";
            this.rbShowUnk.Text = "Show unknown apps";
            this.rbShowUnk.UseVisualStyleBackColor = true;
            this.rbShowUnk.CheckedChanged += new System.EventHandler(this.rbShowUnk_CheckedChanged);
            // 
            // rbShowHis
            // 
            this.rbShowHis.AutoSize = true;
            this.rbShowHis.Location = new System.Drawing.Point(18, 65);
            this.rbShowHis.Name = "rbShowHis";
            this.rbShowHis.Size = new System.Drawing.Size(117, 17);
            this.rbShowHis.TabIndex = 1;
            this.rbShowHis.Tag = "2";
            this.rbShowHis.Text = "Just those in history";
            this.rbShowHis.UseVisualStyleBackColor = true;
            this.rbShowHis.CheckedChanged += new System.EventHandler(this.rbShowHis_CheckedChanged);
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
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gb_Reveal);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "InfoForm";
            this.Text = "InfoForm";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.InfoForm_HelpButtonClicked);
            this.panel1.ResumeLayout(false);
            this.gb_Reveal.ResumeLayout(false);
            this.gb_Reveal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView tv_projapps;
        private System.Windows.Forms.GroupBox gb_Reveal;
        private System.Windows.Forms.RadioButton rbShowUnk;
        private System.Windows.Forms.RadioButton rbShowHis;
        private System.Windows.Forms.RadioButton rb_ShowAll;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton rbShowStats;
    }
}