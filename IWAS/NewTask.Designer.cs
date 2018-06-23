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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbAccess = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbMainCate = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSubCate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbPriority = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnNewTask = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDirector = new System.Windows.Forms.Button();
            this.btnWorker = new System.Windows.Forms.Button();
            this.dtLaunch = new System.Windows.Forms.DateTimePicker();
            this.dtDue = new System.Windows.Forms.DateTimePicker();
            this.dtTerm = new System.Windows.Forms.DateTimePicker();
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
            "채팅",
            "기타"});
            this.cbType.Location = new System.Drawing.Point(65, 10);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(86, 21);
            this.cbType.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "구분";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(183, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
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
            this.cbAccess.Location = new System.Drawing.Point(65, 38);
            this.cbAccess.Name = "cbAccess";
            this.cbAccess.Size = new System.Drawing.Size(86, 21);
            this.cbAccess.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "접근권한";
            // 
            // cbMainCate
            // 
            this.cbMainCate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMainCate.FormattingEnabled = true;
            this.cbMainCate.Location = new System.Drawing.Point(65, 66);
            this.cbMainCate.Name = "cbMainCate";
            this.cbMainCate.Size = new System.Drawing.Size(86, 21);
            this.cbMainCate.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "주요분류";
            // 
            // cbSubCate
            // 
            this.cbSubCate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubCate.FormattingEnabled = true;
            this.cbSubCate.Location = new System.Drawing.Point(65, 94);
            this.cbSubCate.Name = "cbSubCate";
            this.cbSubCate.Size = new System.Drawing.Size(86, 21);
            this.cbSubCate.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "보조분류";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(183, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "예상종료일";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(183, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
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
            this.cbPriority.Location = new System.Drawing.Point(65, 122);
            this.cbPriority.Name = "cbPriority";
            this.cbPriority.Size = new System.Drawing.Size(86, 21);
            this.cbPriority.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 128);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "중요도";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(204, 106);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "관리자";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(204, 138);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "실무자";
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(65, 165);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(265, 20);
            this.tbTitle.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(35, 168);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "제목";
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(65, 194);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(265, 80);
            this.tbComment.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(35, 197);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "내용";
            // 
            // btnNewTask
            // 
            this.btnNewTask.Location = new System.Drawing.Point(196, 281);
            this.btnNewTask.Name = "btnNewTask";
            this.btnNewTask.Size = new System.Drawing.Size(64, 25);
            this.btnNewTask.TabIndex = 3;
            this.btnNewTask.Text = "업무 생성";
            this.btnNewTask.UseVisualStyleBackColor = true;
            this.btnNewTask.Click += new System.EventHandler(this.btnNewTask_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(266, 281);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDirector
            // 
            this.btnDirector.Location = new System.Drawing.Point(244, 101);
            this.btnDirector.Name = "btnDirector";
            this.btnDirector.Size = new System.Drawing.Size(86, 25);
            this.btnDirector.TabIndex = 4;
            this.btnDirector.Text = "button";
            this.btnDirector.UseVisualStyleBackColor = true;
            this.btnDirector.Click += new System.EventHandler(this.btnDirector_Click);
            // 
            // btnWorker
            // 
            this.btnWorker.Location = new System.Drawing.Point(244, 132);
            this.btnWorker.Name = "btnWorker";
            this.btnWorker.Size = new System.Drawing.Size(86, 25);
            this.btnWorker.TabIndex = 4;
            this.btnWorker.Text = "button";
            this.btnWorker.UseVisualStyleBackColor = true;
            this.btnWorker.Click += new System.EventHandler(this.btnWorker_Click);
            // 
            // dtLaunch
            // 
            this.dtLaunch.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtLaunch.Location = new System.Drawing.Point(238, 15);
            this.dtLaunch.Name = "dtLaunch";
            this.dtLaunch.Size = new System.Drawing.Size(92, 20);
            this.dtLaunch.TabIndex = 5;
            // 
            // dtDue
            // 
            this.dtDue.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDue.Location = new System.Drawing.Point(238, 41);
            this.dtDue.Name = "dtDue";
            this.dtDue.Size = new System.Drawing.Size(92, 20);
            this.dtDue.TabIndex = 5;
            // 
            // dtTerm
            // 
            this.dtTerm.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTerm.Location = new System.Drawing.Point(238, 67);
            this.dtTerm.Name = "dtTerm";
            this.dtTerm.Size = new System.Drawing.Size(92, 20);
            this.dtTerm.TabIndex = 5;
            // 
            // NewTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 324);
            this.Controls.Add(this.dtTerm);
            this.Controls.Add(this.dtDue);
            this.Controls.Add(this.dtLaunch);
            this.Controls.Add(this.btnWorker);
            this.Controls.Add(this.btnDirector);
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
            this.Controls.Add(this.cbPriority);
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
        private System.Windows.Forms.ComboBox cbAccess;
        private System.Windows.Forms.ComboBox cbMainCate;
        private System.Windows.Forms.ComboBox cbSubCate;
        private System.Windows.Forms.ComboBox cbPriority;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Button btnNewTask;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDirector;
        private System.Windows.Forms.Button btnWorker;
        private System.Windows.Forms.DateTimePicker dtLaunch;
        private System.Windows.Forms.DateTimePicker dtDue;
        private System.Windows.Forms.DateTimePicker dtTerm;
    }
}