namespace IWAS
{
    partial class MyTasks
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
            this.NewTask = new System.Windows.Forms.Button();
            this.TaskList = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // NewTask
            // 
            this.NewTask.Location = new System.Drawing.Point(13, 13);
            this.NewTask.Name = "NewTask";
            this.NewTask.Size = new System.Drawing.Size(75, 23);
            this.NewTask.TabIndex = 0;
            this.NewTask.Text = "NewTask";
            this.NewTask.UseVisualStyleBackColor = true;
            this.NewTask.Click += new System.EventHandler(this.NewTask_Click);
            // 
            // TaskList
            // 
            this.TaskList.Location = new System.Drawing.Point(13, 43);
            this.TaskList.Name = "TaskList";
            this.TaskList.Size = new System.Drawing.Size(259, 129);
            this.TaskList.TabIndex = 1;
            this.TaskList.UseCompatibleStateImageBehavior = false;
            this.TaskList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TaskList_MouseDoubleClick);
            // 
            // MyTasks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.TaskList);
            this.Controls.Add(this.NewTask);
            this.Name = "MyTasks";
            this.Text = "Tasks";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button NewTask;
        private System.Windows.Forms.ListView TaskList;
    }
}