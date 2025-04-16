namespace CreditStatistics
{
    partial class config
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(config));
            this.TabCA = new System.Windows.Forms.TabControl();
            this.tabStudy = new System.Windows.Forms.TabPage();
            this.btnGetMoreIDs = new System.Windows.Forms.Button();
            this.btnLoadAppIDs = new System.Windows.Forms.Button();
            this.tbApp = new System.Windows.Forms.TextBox();
            this.btnSaveAppIDs = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPC = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReadBoinc = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbWhereBoinc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.BoincTaskFolder = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.WorkingFolderLoc = new System.Windows.Forms.TextBox();
            this.btnSavePClist = new System.Windows.Forms.Button();
            this.rtbLocalHostsBT = new System.Windows.Forms.RichTextBox();
            this.tabCLient = new System.Windows.Forms.TabPage();
            this.btnUseDemo = new System.Windows.Forms.Button();
            this.btnRunScheduler = new System.Windows.Forms.Button();
            this.btnLoadClient = new System.Windows.Forms.Button();
            this.btnSaveClient = new System.Windows.Forms.Button();
            this.rtfClient = new System.Windows.Forms.RichTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SchTimer = new System.Windows.Forms.Timer(this.components);
            this.pbTask = new System.Windows.Forms.ProgressBar();
            this.TabCA.SuspendLayout();
            this.tabStudy.SuspendLayout();
            this.tabPC.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabCLient.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabCA
            // 
            this.TabCA.Controls.Add(this.tabStudy);
            this.TabCA.Controls.Add(this.tabPC);
            this.TabCA.Controls.Add(this.tabCLient);
            this.TabCA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabCA.Location = new System.Drawing.Point(12, 97);
            this.TabCA.Name = "TabCA";
            this.TabCA.SelectedIndex = 0;
            this.TabCA.Size = new System.Drawing.Size(1018, 560);
            this.TabCA.TabIndex = 0;
            // 
            // tabStudy
            // 
            this.tabStudy.Controls.Add(this.btnGetMoreIDs);
            this.tabStudy.Controls.Add(this.btnLoadAppIDs);
            this.tabStudy.Controls.Add(this.tbApp);
            this.tabStudy.Controls.Add(this.btnSaveAppIDs);
            this.tabStudy.Controls.Add(this.label5);
            this.tabStudy.Location = new System.Drawing.Point(4, 25);
            this.tabStudy.Name = "tabStudy";
            this.tabStudy.Padding = new System.Windows.Forms.Padding(3);
            this.tabStudy.Size = new System.Drawing.Size(1010, 531);
            this.tabStudy.TabIndex = 1;
            this.tabStudy.Text = "App Study";
            this.tabStudy.UseVisualStyleBackColor = true;
            // 
            // btnGetMoreIDs
            // 
            this.btnGetMoreIDs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetMoreIDs.ForeColor = System.Drawing.Color.Blue;
            this.btnGetMoreIDs.Location = new System.Drawing.Point(553, 206);
            this.btnGetMoreIDs.Name = "btnGetMoreIDs";
            this.btnGetMoreIDs.Size = new System.Drawing.Size(94, 23);
            this.btnGetMoreIDs.TabIndex = 24;
            this.btnGetMoreIDs.Text = "Get IDs";
            this.toolTip1.SetToolTip(this.btnGetMoreIDs, "This find the BOINC\r\nfolder to get app IDs");
            this.btnGetMoreIDs.UseVisualStyleBackColor = true;
            this.btnGetMoreIDs.Click += new System.EventHandler(this.btnGetMoreIDs_Click);
            // 
            // btnLoadAppIDs
            // 
            this.btnLoadAppIDs.ForeColor = System.Drawing.Color.Blue;
            this.btnLoadAppIDs.Location = new System.Drawing.Point(692, 317);
            this.btnLoadAppIDs.Name = "btnLoadAppIDs";
            this.btnLoadAppIDs.Size = new System.Drawing.Size(94, 23);
            this.btnLoadAppIDs.TabIndex = 22;
            this.btnLoadAppIDs.Text = "Load IDs";
            this.btnLoadAppIDs.UseVisualStyleBackColor = true;
            this.btnLoadAppIDs.Click += new System.EventHandler(this.btnLoadAppIDs_Click);
            // 
            // tbApp
            // 
            this.tbApp.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbApp.Location = new System.Drawing.Point(69, 69);
            this.tbApp.Multiline = true;
            this.tbApp.Name = "tbApp";
            this.tbApp.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbApp.Size = new System.Drawing.Size(432, 413);
            this.tbApp.TabIndex = 1;
            // 
            // btnSaveAppIDs
            // 
            this.btnSaveAppIDs.ForeColor = System.Drawing.Color.Blue;
            this.btnSaveAppIDs.Location = new System.Drawing.Point(692, 264);
            this.btnSaveAppIDs.Name = "btnSaveAppIDs";
            this.btnSaveAppIDs.Size = new System.Drawing.Size(94, 23);
            this.btnSaveAppIDs.TabIndex = 21;
            this.btnSaveAppIDs.Text = "Save IDs";
            this.btnSaveAppIDs.UseVisualStyleBackColor = true;
            this.btnSaveAppIDs.Click += new System.EventHandler(this.btnSaveAppIDs_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Info;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(550, 71);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(236, 112);
            this.label5.TabIndex = 7;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // tabPC
            // 
            this.tabPC.Controls.Add(this.label2);
            this.tabPC.Controls.Add(this.btnReadBoinc);
            this.tabPC.Controls.Add(this.groupBox1);
            this.tabPC.Controls.Add(this.btnSavePClist);
            this.tabPC.Controls.Add(this.rtbLocalHostsBT);
            this.tabPC.Location = new System.Drawing.Point(4, 25);
            this.tabPC.Name = "tabPC";
            this.tabPC.Size = new System.Drawing.Size(1010, 531);
            this.tabPC.TabIndex = 2;
            this.tabPC.Text = "PC list";
            this.tabPC.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Info;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(191, 23);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(476, 96);
            this.label2.TabIndex = 31;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // btnReadBoinc
            // 
            this.btnReadBoinc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadBoinc.ForeColor = System.Drawing.Color.Blue;
            this.btnReadBoinc.Location = new System.Drawing.Point(30, 18);
            this.btnReadBoinc.Name = "btnReadBoinc";
            this.btnReadBoinc.Size = new System.Drawing.Size(123, 51);
            this.btnReadBoinc.TabIndex = 1;
            this.btnReadBoinc.Text = "Click to get your\r\nremote PC names\r\nfrom BoincTasks";
            this.btnReadBoinc.UseVisualStyleBackColor = true;
            this.btnReadBoinc.Click += new System.EventHandler(this.btnReadBoinc_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbWhereBoinc);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.BoincTaskFolder);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.WorkingFolderLoc);
            this.groupBox1.Location = new System.Drawing.Point(470, 184);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 327);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Folder Locations";
            // 
            // tbWhereBoinc
            // 
            this.tbWhereBoinc.Location = new System.Drawing.Point(24, 224);
            this.tbWhereBoinc.Name = "tbWhereBoinc";
            this.tbWhereBoinc.Size = new System.Drawing.Size(386, 22);
            this.tbWhereBoinc.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 200);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 14);
            this.label1.TabIndex = 16;
            this.label1.Text = "Boinc data folder";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.SystemColors.Info;
            this.label15.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(30, 109);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(357, 14);
            this.label15.TabIndex = 14;
            this.label15.Text = "Boinctask folder: AppData\\Roaming\\eFMer\\BoincTasks\r\n";
            // 
            // BoincTaskFolder
            // 
            this.BoincTaskFolder.Location = new System.Drawing.Point(24, 133);
            this.BoincTaskFolder.Name = "BoincTaskFolder";
            this.BoincTaskFolder.Size = new System.Drawing.Size(386, 22);
            this.BoincTaskFolder.TabIndex = 13;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.SystemColors.Info;
            this.label14.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(30, 31);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(105, 14);
            this.label14.TabIndex = 12;
            this.label14.Text = "Working folder";
            // 
            // WorkingFolderLoc
            // 
            this.WorkingFolderLoc.Location = new System.Drawing.Point(24, 55);
            this.WorkingFolderLoc.Name = "WorkingFolderLoc";
            this.WorkingFolderLoc.ReadOnly = true;
            this.WorkingFolderLoc.Size = new System.Drawing.Size(386, 22);
            this.WorkingFolderLoc.TabIndex = 0;
            // 
            // btnSavePClist
            // 
            this.btnSavePClist.ForeColor = System.Drawing.Color.Blue;
            this.btnSavePClist.Location = new System.Drawing.Point(31, 96);
            this.btnSavePClist.Name = "btnSavePClist";
            this.btnSavePClist.Size = new System.Drawing.Size(94, 23);
            this.btnSavePClist.TabIndex = 27;
            this.btnSavePClist.Text = "Save PC list";
            this.btnSavePClist.UseVisualStyleBackColor = true;
            this.btnSavePClist.Click += new System.EventHandler(this.btnSavePClist_Click);
            // 
            // rtbLocalHostsBT
            // 
            this.rtbLocalHostsBT.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLocalHostsBT.Location = new System.Drawing.Point(30, 184);
            this.rtbLocalHostsBT.Name = "rtbLocalHostsBT";
            this.rtbLocalHostsBT.Size = new System.Drawing.Size(390, 327);
            this.rtbLocalHostsBT.TabIndex = 26;
            this.rtbLocalHostsBT.Text = "";
            // 
            // tabCLient
            // 
            this.tabCLient.Controls.Add(this.btnUseDemo);
            this.tabCLient.Controls.Add(this.btnRunScheduler);
            this.tabCLient.Controls.Add(this.btnLoadClient);
            this.tabCLient.Controls.Add(this.btnSaveClient);
            this.tabCLient.Controls.Add(this.rtfClient);
            this.tabCLient.Location = new System.Drawing.Point(4, 25);
            this.tabCLient.Name = "tabCLient";
            this.tabCLient.Padding = new System.Windows.Forms.Padding(3);
            this.tabCLient.Size = new System.Drawing.Size(1010, 531);
            this.tabCLient.TabIndex = 0;
            this.tabCLient.Text = "ClientList";
            this.tabCLient.UseVisualStyleBackColor = true;
            // 
            // btnUseDemo
            // 
            this.btnUseDemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUseDemo.ForeColor = System.Drawing.Color.Blue;
            this.btnUseDemo.Location = new System.Drawing.Point(842, 44);
            this.btnUseDemo.Name = "btnUseDemo";
            this.btnUseDemo.Size = new System.Drawing.Size(139, 43);
            this.btnUseDemo.TabIndex = 26;
            this.btnUseDemo.Text = "Put in demo mode";
            this.toolTip1.SetToolTip(this.btnUseDemo, "Click this to put into demo mode");
            this.btnUseDemo.UseVisualStyleBackColor = true;
            this.btnUseDemo.Click += new System.EventHandler(this.btnUseDemo_Click);
            // 
            // btnRunScheduler
            // 
            this.btnRunScheduler.Enabled = false;
            this.btnRunScheduler.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunScheduler.ForeColor = System.Drawing.Color.Blue;
            this.btnRunScheduler.Location = new System.Drawing.Point(842, 126);
            this.btnRunScheduler.Name = "btnRunScheduler";
            this.btnRunScheduler.Size = new System.Drawing.Size(139, 67);
            this.btnRunScheduler.TabIndex = 25;
            this.btnRunScheduler.Text = "Query to get data\r\nfrom every PC";
            this.toolTip1.SetToolTip(this.btnRunScheduler, "Obtain from each PC client data: \r\nProject name and identifier\r\nDemo mode is disa" +
        "bled.");
            this.btnRunScheduler.UseVisualStyleBackColor = true;
            this.btnRunScheduler.Click += new System.EventHandler(this.btnRunScheduler_Click);
            // 
            // btnLoadClient
            // 
            this.btnLoadClient.ForeColor = System.Drawing.Color.Blue;
            this.btnLoadClient.Location = new System.Drawing.Point(867, 295);
            this.btnLoadClient.Name = "btnLoadClient";
            this.btnLoadClient.Size = new System.Drawing.Size(94, 23);
            this.btnLoadClient.TabIndex = 24;
            this.btnLoadClient.Text = "Load Client";
            this.btnLoadClient.UseVisualStyleBackColor = true;
            this.btnLoadClient.Click += new System.EventHandler(this.btnLoadClient_Click);
            // 
            // btnSaveClient
            // 
            this.btnSaveClient.ForeColor = System.Drawing.Color.Blue;
            this.btnSaveClient.Location = new System.Drawing.Point(867, 240);
            this.btnSaveClient.Name = "btnSaveClient";
            this.btnSaveClient.Size = new System.Drawing.Size(94, 23);
            this.btnSaveClient.TabIndex = 23;
            this.btnSaveClient.Text = "Save Client";
            this.btnSaveClient.UseVisualStyleBackColor = true;
            this.btnSaveClient.Click += new System.EventHandler(this.btnSaveClient_Click);
            // 
            // rtfClient
            // 
            this.rtfClient.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfClient.Location = new System.Drawing.Point(37, 21);
            this.rtfClient.Name = "rtfClient";
            this.rtfClient.Size = new System.Drawing.Size(786, 423);
            this.rtfClient.TabIndex = 0;
            this.rtfClient.Text = "";
            // 
            // SchTimer
            // 
            this.SchTimer.Interval = 800;
            this.SchTimer.Tick += new System.EventHandler(this.SchTimer_Tick);
            // 
            // pbTask
            // 
            this.pbTask.Location = new System.Drawing.Point(371, 26);
            this.pbTask.Maximum = 60;
            this.pbTask.Name = "pbTask";
            this.pbTask.Size = new System.Drawing.Size(327, 23);
            this.pbTask.TabIndex = 24;
            // 
            // config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 669);
            this.Controls.Add(this.pbTask);
            this.Controls.Add(this.TabCA);
            this.Name = "config";
            this.Text = "config";
            this.TabCA.ResumeLayout(false);
            this.tabStudy.ResumeLayout(false);
            this.tabStudy.PerformLayout();
            this.tabPC.ResumeLayout(false);
            this.tabPC.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabCLient.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabCA;
        private System.Windows.Forms.TabPage tabCLient;
        private System.Windows.Forms.TabPage tabStudy;
        private System.Windows.Forms.TextBox tbApp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnGetMoreIDs;
        private System.Windows.Forms.Button btnLoadAppIDs;
        private System.Windows.Forms.Button btnSaveAppIDs;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RichTextBox rtfClient;
        private System.Windows.Forms.Button btnLoadClient;
        private System.Windows.Forms.Button btnSaveClient;
        private System.Windows.Forms.Button btnRunScheduler;
        private System.Windows.Forms.TabPage tabPC;
        private System.Windows.Forms.Button btnSavePClist;
        private System.Windows.Forms.RichTextBox rtbLocalHostsBT;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnReadBoinc;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox BoincTaskFolder;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox WorkingFolderLoc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbWhereBoinc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUseDemo;
        private System.Windows.Forms.Timer SchTimer;
        private System.Windows.Forms.ProgressBar pbTask;
    }
}