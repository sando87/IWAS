﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IWAS
{
    public partial class TaskWindow : Form
    {
        private ICD.Task mTask;
        public TaskWindow(ICD.Task task)
        {
            mTask = task;
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            
        }
    }
}
