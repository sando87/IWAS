using System;
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
    public partial class MyInfo: Form
    {
        static public ICD.User mMyInfo = new ICD.User();
        public MyInfo()
        {
            InitializeComponent();
        }
    }
}
