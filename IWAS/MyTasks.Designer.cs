﻿namespace IWAS
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
            this.lvChat = new System.Windows.Forms.ListView();
            this.btnTracking = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NewTask
            // 
            this.NewTask.Location = new System.Drawing.Point(11, 14);
            this.NewTask.Name = "NewTask";
            this.NewTask.Size = new System.Drawing.Size(64, 25);
            this.NewTask.TabIndex = 0;
            this.NewTask.Text = "NewTask";
            this.NewTask.UseVisualStyleBackColor = true;
            this.NewTask.Click += new System.EventHandler(this.NewTask_Click);
            // 
            // TaskList
            // 
            this.TaskList.Location = new System.Drawing.Point(11, 47);
            this.TaskList.MultiSelect = false;
            this.TaskList.Name = "TaskList";
            this.TaskList.Size = new System.Drawing.Size(223, 139);
            this.TaskList.TabIndex = 1;
            this.TaskList.UseCompatibleStateImageBehavior = false;
            this.TaskList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TaskList_MouseDoubleClick);
            // 
            // lvChat
            // 
            this.lvChat.Location = new System.Drawing.Point(11, 222);
            this.lvChat.MultiSelect = false;
            this.lvChat.Name = "lvChat";
            this.lvChat.Size = new System.Drawing.Size(223, 170);
            this.lvChat.TabIndex = 2;
            this.lvChat.UseCompatibleStateImageBehavior = false;
            this.lvChat.DoubleClick += new System.EventHandler(this.lvChat_DoubleClick);
            // 
            // btnTracking
            // 
            this.btnTracking.Location = new System.Drawing.Point(159, 14);
            this.btnTracking.Name = "btnTracking";
            this.btnTracking.Size = new System.Drawing.Size(75, 23);
            this.btnTracking.TabIndex = 3;
            this.btnTracking.Text = "Tracking";
            this.btnTracking.UseVisualStyleBackColor = true;
            this.btnTracking.Click += new System.EventHandler(this.btnTracking_Click);
            // 
            // MyTasks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 405);
            this.Controls.Add(this.btnTracking);
            this.Controls.Add(this.lvChat);
            this.Controls.Add(this.TaskList);
            this.Controls.Add(this.NewTask);
            this.Name = "MyTasks";
            this.Text = "Tasks";
            this.Load += new System.EventHandler(this.MyTasks_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button NewTask;
        private System.Windows.Forms.ListView TaskList;
        private System.Windows.Forms.ListView lvChat;
        private System.Windows.Forms.Button btnTracking;
    }
}