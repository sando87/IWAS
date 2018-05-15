namespace IWAS
{
    partial class UserList
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
            this.lvUserlist = new System.Windows.Forms.ListView();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvUserlist
            // 
            this.lvUserlist.Location = new System.Drawing.Point(13, 13);
            this.lvUserlist.Name = "lvUserlist";
            this.lvUserlist.Size = new System.Drawing.Size(259, 203);
            this.lvUserlist.TabIndex = 0;
            this.lvUserlist.UseCompatibleStateImageBehavior = false;
            this.lvUserlist.DoubleClick += new System.EventHandler(this.lvUserlist_DoubleClick);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(197, 226);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // UserList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.lvUserlist);
            this.Name = "UserList";
            this.Text = "UserList";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvUserlist;
        private System.Windows.Forms.Button btnUpdate;
    }
}