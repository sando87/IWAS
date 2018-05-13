namespace IWAS
{
    partial class NewTask
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
            this.cbType = new System.Windows.Forms.ComboBox();
            this.tbLaunch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbAccess = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbMainCate = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSubCate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbDue = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbTerm = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbPriority = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbDirector = new System.Windows.Forms.ComboBox();
            this.cbWorker = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnNewTask = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "개발업무",
            "이슈개선",
            "스터디",
            "기타"});
            this.cbType.Location = new System.Drawing.Point(76, 9);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(100, 20);
            this.cbType.TabIndex = 0;
            // 
            // tbLaunch
            // 
            this.tbLaunch.Location = new System.Drawing.Point(285, 11);
            this.tbLaunch.Name = "tbLaunch";
            this.tbLaunch.Size = new System.Drawing.Size(100, 21);
            this.tbLaunch.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "구분";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(214, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "예상시작일";
            // 
            // cbAccess
            // 
            this.cbAccess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAccess.FormattingEnabled = true;
            this.cbAccess.Items.AddRange(new object[] {
            "관련자 제한",
            "모두 공개"});
            this.cbAccess.Location = new System.Drawing.Point(76, 35);
            this.cbAccess.Name = "cbAccess";
            this.cbAccess.Size = new System.Drawing.Size(100, 20);
            this.cbAccess.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "접근권한";
            // 
            // cbMainCate
            // 
            this.cbMainCate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMainCate.FormattingEnabled = true;
            this.cbMainCate.Location = new System.Drawing.Point(76, 61);
            this.cbMainCate.Name = "cbMainCate";
            this.cbMainCate.Size = new System.Drawing.Size(100, 20);
            this.cbMainCate.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "주요분류";
            // 
            // cbSubCate
            // 
            this.cbSubCate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubCate.FormattingEnabled = true;
            this.cbSubCate.Location = new System.Drawing.Point(76, 87);
            this.cbSubCate.Name = "cbSubCate";
            this.cbSubCate.Size = new System.Drawing.Size(100, 20);
            this.cbSubCate.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "보조분류";
            // 
            // tbDue
            // 
            this.tbDue.Location = new System.Drawing.Point(285, 38);
            this.tbDue.Name = "tbDue";
            this.tbDue.Size = new System.Drawing.Size(100, 21);
            this.tbDue.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(214, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "예상종료일";
            // 
            // tbTerm
            // 
            this.tbTerm.Location = new System.Drawing.Point(285, 65);
            this.tbTerm.Name = "tbTerm";
            this.tbTerm.Size = new System.Drawing.Size(100, 21);
            this.tbTerm.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(214, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "예산소요일";
            // 
            // cbPriority
            // 
            this.cbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPriority.FormattingEnabled = true;
            this.cbPriority.Items.AddRange(new object[] {
            "매우긴급",
            "높음",
            "중간",
            "낮음"});
            this.cbPriority.Location = new System.Drawing.Point(76, 113);
            this.cbPriority.Name = "cbPriority";
            this.cbPriority.Size = new System.Drawing.Size(100, 20);
            this.cbPriority.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(29, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "중요도";
            // 
            // cbDirector
            // 
            this.cbDirector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirector.FormattingEnabled = true;
            this.cbDirector.Location = new System.Drawing.Point(285, 92);
            this.cbDirector.Name = "cbDirector";
            this.cbDirector.Size = new System.Drawing.Size(100, 20);
            this.cbDirector.TabIndex = 0;
            // 
            // cbWorker
            // 
            this.cbWorker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorker.FormattingEnabled = true;
            this.cbWorker.Location = new System.Drawing.Point(285, 118);
            this.cbWorker.Name = "cbWorker";
            this.cbWorker.Size = new System.Drawing.Size(100, 20);
            this.cbWorker.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(238, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "관리자";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(238, 121);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 2;
            this.label10.Text = "실무자";
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(76, 152);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(309, 21);
            this.tbTitle.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(41, 155);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "제목";
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(76, 179);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(309, 74);
            this.tbComment.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(41, 182);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 2;
            this.label12.Text = "내용";
            // 
            // btnNewTask
            // 
            this.btnNewTask.Location = new System.Drawing.Point(229, 259);
            this.btnNewTask.Name = "btnNewTask";
            this.btnNewTask.Size = new System.Drawing.Size(75, 23);
            this.btnNewTask.TabIndex = 3;
            this.btnNewTask.Text = "업무 생성";
            this.btnNewTask.UseVisualStyleBackColor = true;
            this.btnNewTask.Click += new System.EventHandler(this.btnNewTask_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(310, 259);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // NewTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 299);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNewTask);
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
            this.Controls.Add(this.cbWorker);
            this.Controls.Add(this.cbPriority);
            this.Controls.Add(this.cbDirector);
            this.Controls.Add(this.cbSubCate);
            this.Controls.Add(this.cbMainCate);
            this.Controls.Add(this.cbAccess);
            this.Controls.Add(this.cbType);
            this.Name = "NewTask";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.TextBox tbLaunch;
        private System.Windows.Forms.ComboBox cbAccess;
        private System.Windows.Forms.ComboBox cbMainCate;
        private System.Windows.Forms.ComboBox cbSubCate;
        private System.Windows.Forms.TextBox tbDue;
        private System.Windows.Forms.TextBox tbTerm;
        private System.Windows.Forms.ComboBox cbPriority;
        private System.Windows.Forms.ComboBox cbDirector;
        private System.Windows.Forms.ComboBox cbWorker;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Button btnNewTask;
        private System.Windows.Forms.Button btnCancel;
    }
}