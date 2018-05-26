namespace IWAS
{
    partial class DlgEditChatUsers
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
            this.lvUserAll = new System.Windows.Forms.ListView();
            this.lvCurUsers = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvUserAll
            // 
            this.lvUserAll.Location = new System.Drawing.Point(12, 33);
            this.lvUserAll.Name = "lvUserAll";
            this.lvUserAll.Size = new System.Drawing.Size(178, 296);
            this.lvUserAll.TabIndex = 0;
            this.lvUserAll.UseCompatibleStateImageBehavior = false;
            this.lvUserAll.DoubleClick += new System.EventHandler(this.lvUserAll_DoubleClick);
            // 
            // lvCurUsers
            // 
            this.lvCurUsers.Location = new System.Drawing.Point(240, 32);
            this.lvCurUsers.Name = "lvCurUsers";
            this.lvCurUsers.Size = new System.Drawing.Size(178, 296);
            this.lvCurUsers.TabIndex = 1;
            this.lvCurUsers.UseCompatibleStateImageBehavior = false;
            this.lvCurUsers.DoubleClick += new System.EventHandler(this.lvCurUsers_DoubleClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(197, 132);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(37, 35);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = ">>";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(197, 173);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(37, 35);
            this.btnDel.TabIndex = 2;
            this.btnDel.Text = "<<";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "AllUsers";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(238, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "ChatUsers";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(343, 334);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // DlgEditChatUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 365);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvCurUsers);
            this.Controls.Add(this.lvUserAll);
            this.Name = "DlgEditChatUsers";
            this.Text = "DlgEditChatUsers";
            this.Load += new System.EventHandler(this.DlgEditChatUsers_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvUserAll;
        private System.Windows.Forms.ListView lvCurUsers;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnApply;
    }
}