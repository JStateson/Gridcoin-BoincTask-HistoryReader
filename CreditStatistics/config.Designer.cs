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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabCLient = new System.Windows.Forms.TabPage();
            this.tabStudy = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabCLient);
            this.tabControl1.Controls.Add(this.tabStudy);
            this.tabControl1.Location = new System.Drawing.Point(30, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(932, 576);
            this.tabControl1.TabIndex = 0;
            // 
            // tabCLient
            // 
            this.tabCLient.Location = new System.Drawing.Point(4, 22);
            this.tabCLient.Name = "tabCLient";
            this.tabCLient.Padding = new System.Windows.Forms.Padding(3);
            this.tabCLient.Size = new System.Drawing.Size(924, 550);
            this.tabCLient.TabIndex = 0;
            this.tabCLient.Text = "ClientList";
            this.tabCLient.UseVisualStyleBackColor = true;
            // 
            // tabStudy
            // 
            this.tabStudy.Location = new System.Drawing.Point(4, 22);
            this.tabStudy.Name = "tabStudy";
            this.tabStudy.Padding = new System.Windows.Forms.Padding(3);
            this.tabStudy.Size = new System.Drawing.Size(924, 550);
            this.tabStudy.TabIndex = 1;
            this.tabStudy.Text = "App Study";
            this.tabStudy.UseVisualStyleBackColor = true;
            // 
            // config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 612);
            this.Controls.Add(this.tabControl1);
            this.Name = "config";
            this.Text = "config";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabCLient;
        private System.Windows.Forms.TabPage tabStudy;
    }
}