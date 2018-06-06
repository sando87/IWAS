using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IWAS
{
    class ExTrackListView : ListView
    {
        public class TaskTrack
        {
            public int taskID;
            public string time;
            public string editor;
            public int type;
            public string info;
        }
        public class ItemTask
        {
            public ICD.Task task;
            public List<TaskTrack> his;
        }

        public List<ItemTask> tasks = new List<ItemTask>();

        public void Init()
        {
            View = View.Details;
            GridLines = true;
            FullRowSelect = true;
            Sorting = SortOrder.None;

            Columns.Add("id");
            Columns[0].Width = 50;

            OwnerDraw = true;
            DrawColumnHeader += (s, ee) =>
            {
                ee.DrawDefault = true;
            };
            DrawSubItem += My_DrawSubItem;
        }

        private void My_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex != 3)
                e.DrawDefault = true;
            else
            {
                Rectangle rect = e.Bounds;
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
                //ee.Graphics.FillRectangle(Brushes.Red, rect.Left + 2, rect.Top + 2, rect.Width - 4, rect.Height - 4);
                e.Graphics.FillRectangle(Brushes.Blue, rect.Left + 2, rect.Top + 4, rect.Width - 4, rect.Height - 8);
            }
        }
    }
}
