namespace IWAS
{
    partial class DlgChatRoom
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
            this.lvMsg = new System.Windows.Forms.ListView();
            this.tbMsg = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnUser = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvMsg
            // 
            this.lvMsg.Location = new System.Drawing.Point(12, 12);
            this.lvMsg.Name = "lvMsg";
            this.lvMsg.Size = new System.Drawing.Size(331, 421);
            this.lvMsg.TabIndex = 0;
            this.lvMsg.UseCompatibleStateImageBehavior = false;
            // 
            // tbMsg
            // 
            this.tbMsg.Location = new System.Drawing.Point(13, 440);
            this.tbMsg.Multiline = true;
            this.tbMsg.Name = "tbMsg";
            this.tbMsg.Size = new System.Drawing.Size(267, 44);
            this.tbMsg.TabIndex = 1;
            this.tbMsg.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbMsg_KeyUp);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(283, 439);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(36, 45);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "=>";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnUser
            // 
            this.btnUser.Location = new System.Drawing.Point(320, 440);
            this.btnUser.Name = "btnUser";
            this.btnUser.Size = new System.Drawing.Size(23, 22);
            this.btnUser.TabIndex = 3;
            this.btnUser.Text = "+";
            this.btnUser.UseVisualStyleBackColor = true;
            this.btnUser.Click += new System.EventHandler(this.btnUser_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(320, 462);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(23, 22);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "X";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // DlgChatRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 491);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnUser);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbMsg);
            this.Controls.Add(this.lvMsg);
            this.Name = "DlgChatRoom";
            this.Text = "DlgChatRoom";
            this.Load += new System.EventHandler(this.DlgChatRoom_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvMsg;
        private System.Windows.Forms.TextBox tbMsg;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnUser;
        private System.Windows.Forms.Button btnExit;
    }
}