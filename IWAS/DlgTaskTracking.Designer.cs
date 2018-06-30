namespace IWAS
{
    partial class DlgTaskTracking
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
            this.lvTracking = new System.Windows.Forms.ListView();
            this.btnAgoFar = new System.Windows.Forms.Button();
            this.btnAgo = new System.Windows.Forms.Button();
            this.btnLater = new System.Windows.Forms.Button();
            this.btnLaterFar = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvTracking
            // 
            this.lvTracking.Location = new System.Drawing.Point(12, 51);
            this.lvTracking.Name = "lvTracking";
            this.lvTracking.Size = new System.Drawing.Size(1036, 432);
            this.lvTracking.TabIndex = 0;
            this.lvTracking.UseCompatibleStateImageBehavior = false;
            // 
            // btnAgoFar
            // 
            this.btnAgoFar.Location = new System.Drawing.Point(12, 22);
            this.btnAgoFar.Name = "btnAgoFar";
            this.btnAgoFar.Size = new System.Drawing.Size(75, 23);
            this.btnAgoFar.TabIndex = 1;
            this.btnAgoFar.Text = "<<";
            this.btnAgoFar.UseVisualStyleBackColor = true;
            // 
            // btnAgo
            // 
            this.btnAgo.Location = new System.Drawing.Point(93, 22);
            this.btnAgo.Name = "btnAgo";
            this.btnAgo.Size = new System.Drawing.Size(75, 23);
            this.btnAgo.TabIndex = 1;
            this.btnAgo.Text = "<";
            this.btnAgo.UseVisualStyleBackColor = true;
            this.btnAgo.Click += new System.EventHandler(this.btnAgo_Click);
            // 
            // btnLater
            // 
            this.btnLater.Location = new System.Drawing.Point(174, 22);
            this.btnLater.Name = "btnLater";
            this.btnLater.Size = new System.Drawing.Size(75, 23);
            this.btnLater.TabIndex = 1;
            this.btnLater.Text = ">";
            this.btnLater.UseVisualStyleBackColor = true;
            this.btnLater.Click += new System.EventHandler(this.btnLater_Click);
            // 
            // btnLaterFar
            // 
            this.btnLaterFar.Location = new System.Drawing.Point(255, 22);
            this.btnLaterFar.Name = "btnLaterFar";
            this.btnLaterFar.Size = new System.Drawing.Size(75, 23);
            this.btnLaterFar.TabIndex = 1;
            this.btnLaterFar.Text = ">>";
            this.btnLaterFar.UseVisualStyleBackColor = true;
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(468, 22);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(75, 23);
            this.btnSetting.TabIndex = 1;
            this.btnSetting.Text = "Setting";
            this.btnSetting.UseVisualStyleBackColor = true;
            // 
            // DlgTaskTracking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 496);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.btnLaterFar);
            this.Controls.Add(this.btnLater);
            this.Controls.Add(this.btnAgo);
            this.Controls.Add(this.btnAgoFar);
            this.Controls.Add(this.lvTracking);
            this.Name = "DlgTaskTracking";
            this.Text = "DlgTaskTracking";
            this.Load += new System.EventHandler(this.DlgTaskTracking_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvTracking;
        private System.Windows.Forms.Button btnAgoFar;
        private System.Windows.Forms.Button btnAgo;
        private System.Windows.Forms.Button btnLater;
        private System.Windows.Forms.Button btnLaterFar;
        private System.Windows.Forms.Button btnSetting;
    }
}