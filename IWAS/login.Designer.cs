namespace IWAS
{
    partial class Login
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
            this.btnLogin = new System.Windows.Forms.Button();
            this.cbNewUser = new System.Windows.Forms.CheckBox();
            this.edPassword = new System.Windows.Forms.TextBox();
            this.edUserID = new System.Windows.Forms.TextBox();
            this.edPasswordCheck = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(140, 109);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "로그인";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbNewUser
            // 
            this.cbNewUser.AutoSize = true;
            this.cbNewUser.Location = new System.Drawing.Point(48, 116);
            this.cbNewUser.Name = "cbNewUser";
            this.cbNewUser.Size = new System.Drawing.Size(72, 16);
            this.cbNewUser.TabIndex = 1;
            this.cbNewUser.Text = "회원가입";
            this.cbNewUser.UseVisualStyleBackColor = true;
            this.cbNewUser.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // edPassword
            // 
            this.edPassword.Location = new System.Drawing.Point(115, 55);
            this.edPassword.Name = "edPassword";
            this.edPassword.Size = new System.Drawing.Size(100, 21);
            this.edPassword.TabIndex = 2;
            // 
            // edUserID
            // 
            this.edUserID.Location = new System.Drawing.Point(115, 28);
            this.edUserID.Name = "edUserID";
            this.edUserID.Size = new System.Drawing.Size(100, 21);
            this.edUserID.TabIndex = 2;
            // 
            // edPasswordCheck
            // 
            this.edPasswordCheck.Enabled = false;
            this.edPasswordCheck.Location = new System.Drawing.Point(115, 82);
            this.edPasswordCheck.Name = "edPasswordCheck";
            this.edPasswordCheck.Size = new System.Drawing.Size(100, 21);
            this.edPasswordCheck.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "사용자 ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "비밀번호";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "비밀번호 확인";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 153);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edPasswordCheck);
            this.Controls.Add(this.edUserID);
            this.Controls.Add(this.edPassword);
            this.Controls.Add(this.cbNewUser);
            this.Controls.Add(this.btnLogin);
            this.Name = "Login";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.CheckBox cbNewUser;
        private System.Windows.Forms.TextBox edPassword;
        private System.Windows.Forms.TextBox edUserID;
        private System.Windows.Forms.TextBox edPasswordCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

