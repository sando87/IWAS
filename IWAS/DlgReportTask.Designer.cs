namespace IWAS
{
    partial class DlgReportTask
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
            this.btnReport = new System.Windows.Forms.Button();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.tbDate = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(197, 144);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 23);
            this.btnReport.TabIndex = 0;
            this.btnReport.Text = "Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "중간보고",
            "최종보고"});
            this.cbType.Location = new System.Drawing.Point(12, 12);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(121, 20);
            this.cbType.TabIndex = 1;
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(12, 38);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(260, 100);
            this.tbComment.TabIndex = 2;
            // 
            // tbDate
            // 
            this.tbDate.Location = new System.Drawing.Point(139, 11);
            this.tbDate.Name = "tbDate";
            this.tbDate.Size = new System.Drawing.Size(133, 21);
            this.tbDate.TabIndex = 3;
            // 
            // DlgReportTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 176);
            this.Controls.Add(this.tbDate);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.btnReport);
            this.Name = "DlgReportTask";
            this.Text = "DlgReportTask";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.TextBox tbDate;
    }
}