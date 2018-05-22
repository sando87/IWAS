namespace IWAS
{
    partial class TaskWindow
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
            this.btnWorker = new System.Windows.Forms.Button();
            this.btnDirector = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTerm = new System.Windows.Forms.TextBox();
            this.tbDue = new System.Windows.Forms.TextBox();
            this.tbLaunch = new System.Windows.Forms.TextBox();
            this.cbPriority = new System.Windows.Forms.ComboBox();
            this.cbSubCate = new System.Windows.Forms.ComboBox();
            this.cbMainCate = new System.Windows.Forms.ComboBox();
            this.cbAccess = new System.Windows.Forms.ComboBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.btnReqFix = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnWorker
            // 
            this.btnWorker.Enabled = false;
            this.btnWorker.Location = new System.Drawing.Point(261, 135);
            this.btnWorker.Name = "btnWorker";
            this.btnWorker.Size = new System.Drawing.Size(77, 25);
            this.btnWorker.TabIndex = 30;
            this.btnWorker.Text = "name";
            this.btnWorker.UseVisualStyleBackColor = true;
            // 
            // btnDirector
            // 
            this.btnDirector.Enabled = false;
            this.btnDirector.Location = new System.Drawing.Point(261, 104);
            this.btnDirector.Name = "btnDirector";
            this.btnDirector.Size = new System.Drawing.Size(77, 25);
            this.btnDirector.TabIndex = 29;
            this.btnDirector.Text = "name";
            this.btnDirector.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(273, 284);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(64, 25);
            this.btnEdit.TabIndex = 28;
            this.btnEdit.Text = "편집";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(43, 200);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "내용";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(43, 171);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "제목";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(191, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "예산소요일";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(191, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "예상종료일";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(191, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "예상시작일";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(212, 141);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "실무자";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 131);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "중요도";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(212, 109);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "관리자";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "보조분류";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "주요분류";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "접근권한";
            // 
            // tbComment
            // 
            this.tbComment.Enabled = false;
            this.tbComment.Location = new System.Drawing.Point(73, 197);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(265, 80);
            this.tbComment.TabIndex = 14;
            // 
            // tbTitle
            // 
            this.tbTitle.Enabled = false;
            this.tbTitle.Location = new System.Drawing.Point(73, 168);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(265, 20);
            this.tbTitle.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "구분";
            // 
            // tbTerm
            // 
            this.tbTerm.Enabled = false;
            this.tbTerm.Location = new System.Drawing.Point(264, 74);
            this.tbTerm.Name = "tbTerm";
            this.tbTerm.Size = new System.Drawing.Size(74, 20);
            this.tbTerm.TabIndex = 12;
            // 
            // tbDue
            // 
            this.tbDue.Enabled = false;
            this.tbDue.Location = new System.Drawing.Point(264, 44);
            this.tbDue.Name = "tbDue";
            this.tbDue.Size = new System.Drawing.Size(74, 20);
            this.tbDue.TabIndex = 11;
            // 
            // tbLaunch
            // 
            this.tbLaunch.Enabled = false;
            this.tbLaunch.Location = new System.Drawing.Point(264, 15);
            this.tbLaunch.Name = "tbLaunch";
            this.tbLaunch.Size = new System.Drawing.Size(74, 20);
            this.tbLaunch.TabIndex = 10;
            // 
            // cbPriority
            // 
            this.cbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPriority.Enabled = false;
            this.cbPriority.FormattingEnabled = true;
            this.cbPriority.Items.AddRange(new object[] {
            "매우긴급",
            "높음",
            "중간",
            "낮음"});
            this.cbPriority.Location = new System.Drawing.Point(82, 126);
            this.cbPriority.Name = "cbPriority";
            this.cbPriority.Size = new System.Drawing.Size(77, 21);
            this.cbPriority.TabIndex = 8;
            // 
            // cbSubCate
            // 
            this.cbSubCate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubCate.Enabled = false;
            this.cbSubCate.FormattingEnabled = true;
            this.cbSubCate.Location = new System.Drawing.Point(82, 98);
            this.cbSubCate.Name = "cbSubCate";
            this.cbSubCate.Size = new System.Drawing.Size(77, 21);
            this.cbSubCate.TabIndex = 7;
            // 
            // cbMainCate
            // 
            this.cbMainCate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMainCate.Enabled = false;
            this.cbMainCate.FormattingEnabled = true;
            this.cbMainCate.Location = new System.Drawing.Point(82, 69);
            this.cbMainCate.Name = "cbMainCate";
            this.cbMainCate.Size = new System.Drawing.Size(77, 21);
            this.cbMainCate.TabIndex = 6;
            // 
            // cbAccess
            // 
            this.cbAccess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAccess.Enabled = false;
            this.cbAccess.FormattingEnabled = true;
            this.cbAccess.Items.AddRange(new object[] {
            "관련자 제한",
            "모두 공개"});
            this.cbAccess.Location = new System.Drawing.Point(82, 41);
            this.cbAccess.Name = "cbAccess";
            this.cbAccess.Size = new System.Drawing.Size(77, 21);
            this.cbAccess.TabIndex = 9;
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.Enabled = false;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "개발업무",
            "이슈개선",
            "스터디",
            "기타"});
            this.cbType.Location = new System.Drawing.Point(82, 13);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(77, 21);
            this.cbType.TabIndex = 5;
            // 
            // btnReqFix
            // 
            this.btnReqFix.Location = new System.Drawing.Point(203, 284);
            this.btnReqFix.Name = "btnReqFix";
            this.btnReqFix.Size = new System.Drawing.Size(64, 25);
            this.btnReqFix.TabIndex = 28;
            this.btnReqFix.Text = "수정";
            this.btnReqFix.UseVisualStyleBackColor = true;
            this.btnReqFix.Visible = false;
            this.btnReqFix.Click += new System.EventHandler(this.btnReqFix_Click);
            // 
            // TaskWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 596);
            this.Controls.Add(this.btnWorker);
            this.Controls.Add(this.btnDirector);
            this.Controls.Add(this.btnReqFix);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.tbTitle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbTerm);
            this.Controls.Add(this.tbDue);
            this.Controls.Add(this.tbLaunch);
            this.Controls.Add(this.cbPriority);
            this.Controls.Add(this.cbSubCate);
            this.Controls.Add(this.cbMainCate);
            this.Controls.Add(this.cbAccess);
            this.Controls.Add(this.cbType);
            this.Name = "TaskWindow";
            this.Text = "TaskWindow";
            this.Load += new System.EventHandler(this.TaskWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnWorker;
        private System.Windows.Forms.Button btnDirector;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTerm;
        private System.Windows.Forms.TextBox tbDue;
        private System.Windows.Forms.TextBox tbLaunch;
        private System.Windows.Forms.ComboBox cbPriority;
        private System.Windows.Forms.ComboBox cbSubCate;
        private System.Windows.Forms.ComboBox cbMainCate;
        private System.Windows.Forms.ComboBox cbAccess;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Button btnReqFix;
    }
}