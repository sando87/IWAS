using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWAS.ICD;

namespace IWAS
{
    public partial class DlgTaskTracking : Form
    {
        private const int VIEW_PERIOD = 15; //오늘 기준 15일 전후로 보여준다

        class ReportInfo
        {
            public string time;
            public string type;
            public string message;
        }

        class TrackingInfo
        {
            public Work workBase;
            public Work workCurrent;
            public WorkHistory[] his;
            public List<ReportInfo> reports = new List<ReportInfo>();

            private void Update(string fromtime, string totime, string curtime)
            {
                int from = int.Parse(fromtime);
                int to = int.Parse(totime);
                int today = int.Parse(curtime);
                int workCreate = int.Parse(workBase.createTime);
                int workOpen = int.Parse(workBase.timeOpen);
                int workClose = int.Parse(workBase.timeClose);
                if (to < workOpen || workClose < from || today < workCreate)
                {
                    workCurrent = null;
                    reports.Clear();
                    return;
                }

                reports.Clear();
                workCurrent = workBase;
                int current = workOpen;
                int i = 0;
                while (current <= today && i < his.Length)
                {
                    WorkHistory curHis = his[i];
                    switch (curHis.columnName)
                    {
                        case "title": workCurrent.title = curHis.toInfo; break;
                        case "launch": workCurrent.preLaunch = curHis.toInfo; break;
                        case "due": workCurrent.preDue = curHis.toInfo; break;
                        case "term": workCurrent.preterm = curHis.toInfo; break;
                        case "report":
                            {
                                ReportInfo rep = new ReportInfo();
                                rep.time = curHis.time;
                                rep.type = curHis.columnName;
                                rep.message = curHis.toInfo;
                                reports.Add(rep);
                            }
                            break;
                    }
                    current = int.Parse(curHis.time);
                    i++;
                }

            }
        }

        Dictionary<int, TrackingInfo> mTracks = new Dictionary<int, TrackingInfo>();
        private int mCount = 0;

        public DlgTaskTracking()
        {
            InitializeComponent();
        }

        private void DlgTaskTracking_Load(object sender, EventArgs e)
        {
            InitListViews();

            ICDPacketMgr.GetInst().OnRecv += OnProcTaskHistroy;
            FormClosed += delegate {
                ICDPacketMgr.GetInst().OnRecv -= OnProcTaskHistroy;
            };
            
            WorkList msg = new WorkList();
            msg.FillClientHeader(DEF.CMD_TaskListTime, 0);

            //default curDate +/-15 days
            msg.ext1 = "fromTime";
            msg.ext1 = "toTime";

            ICDPacketMgr.GetInst().sendMsgToServer(msg);
        }


        private void InitListViews()
        {
            lvTracking.View = View.Details;
            lvTracking.GridLines = true;
            lvTracking.FullRowSelect = true;
            lvTracking.Sorting = SortOrder.None;

            //lvTracking.DrawSubItem += My_DrawSubItem;

            /*
            lvTracking.Columns.Add("id");
            lvTracking.Columns[0].Width = 50;
            lvTracking.Columns.Add("user");
            lvTracking.Columns[1].Width = 100;
            lvTracking.Columns.Add("message");
            lvTracking.Columns[2].Width = 100;
            lvTracking.Columns.Add("state");
            lvTracking.Columns[3].Width = 50;
            */
        }

        private void My_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex != 9)
                e.DrawDefault = true;
            else
            {
                Rectangle rect = e.Bounds;
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
                //ee.Graphics.FillRectangle(Brushes.Red, rect.Left + 2, rect.Top + 2, rect.Width - 4, rect.Height - 4);
                e.Graphics.FillRectangle(Brushes.Blue, rect.Left + 2, rect.Top + 4, rect.Width - 4, rect.Height - 8);
            }
        }

        private void OnProcTaskHistroy(int clientID, HEADER obj)
        {
            switch (obj.msgID)
            {
                case ICD.DEF.CMD_TaskListTime:
                    ProcTaskListTime(obj);
                    break;
                case ICD.DEF.CMD_TaskHistory:
                    ProcTaskHistory(obj);
                    break;
                default:
                    break;
            }
        }

        private void ProcTaskListTime(HEADER obj)
        {
            mCount = 0;
            ICD.WorkList msg = (ICD.WorkList)obj;

            WorkHistoryList msgHis = new WorkHistoryList();
            msgHis.FillClientHeader(DEF.CMD_TaskHistory, 0);

            foreach (Work item in msg.works)
            {
                if (mTracks.ContainsKey(item.recordID))
                    continue;

                TrackingInfo info = new TrackingInfo();
                info.workBase = item;
                info.his = null;
                mTracks[item.recordID] = info;
                mCount++;

                msgHis.ext1 = item.recordID.ToString();
                ICDPacketMgr.GetInst().sendMsgToServer(msgHis);
            }
        }
        private void ProcTaskHistory(HEADER obj)
        {
            ICD.WorkHistoryList msg = (ICD.WorkHistoryList)obj;
            
            if(msg.workHistory.Length > 0)
            {
                int taskID = msg.workHistory[0].taskID;
                if (!mTracks.ContainsKey(taskID))
                {
                    LOG.warn();
                    return;
                }

                mTracks[taskID].his = msg.workHistory;
            }

            mCount--;
            if(mCount<=0)
            {
                UpdateListView();
            }
        }

        private void UpdateListView()
        {
            //Update WorkList
            //Sort Current List
            //filter records
            //update listview
        }
    }
}
