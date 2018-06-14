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
        private const int COLUMN_COUNT = 14;

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

            public void Update(string fromtime, string totime, string curtime)
            {
                int from = int.Parse(fromtime);
                int to = int.Parse(totime);
                int today = int.Parse(curtime);
                int workCreate = int.Parse(workBase.time);
                int workOpen = int.Parse(workBase.timeFirst);
                int workClose = int.Parse(workBase.timeDone);
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
                        case "launch": workCurrent.launch = curHis.toInfo; break;
                        case "due": workCurrent.due = curHis.toInfo; break;
                        case "term": workCurrent.term = curHis.toInfo; break;
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
        private int mCurFilterColumnIndex = 0;

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

            RequestTaskList("fromdate", "todate");
        }

        private void RequestTaskList(string from, string to)
        {
            WorkList msg = new WorkList();
            msg.FillClientHeader(DEF.CMD_TaskBaseList, 0);

            //default curDate +/-15 days
            msg.ext1 = from;
            msg.ext1 = to;

            ICDPacketMgr.GetInst().sendMsgToServer(msg);
        }


    private void InitListViews()
        {
            lvTracking.View = View.Details;
            lvTracking.GridLines = true;
            lvTracking.FullRowSelect = true;
            lvTracking.Sorting = SortOrder.None;

            lvTracking.ColumnClick += (ss, ee) => {
                mCurFilterColumnIndex = ee.Column;
            };
            //lvTracking.DrawSubItem += My_DrawSubItem;

            lvTracking.Columns.Add("id");
            lvTracking.Columns[0].Width = 50;
            lvTracking.Columns.Add("type");
            lvTracking.Columns[1].Width = 100;
            lvTracking.Columns.Add("time");
            lvTracking.Columns[2].Width = 0;
            lvTracking.Columns.Add("creator");
            lvTracking.Columns[3].Width = 100;
            lvTracking.Columns.Add("access");
            lvTracking.Columns[4].Width = 100;
            lvTracking.Columns.Add("mainCate");
            lvTracking.Columns[5].Width = 100;
            lvTracking.Columns.Add("subCate");
            lvTracking.Columns[6].Width = 100;
            lvTracking.Columns.Add("title");
            lvTracking.Columns[7].Width = 100;
            lvTracking.Columns.Add("comment");
            lvTracking.Columns[8].Width = 100;
            lvTracking.Columns.Add("director");
            lvTracking.Columns[9].Width = 100;
            lvTracking.Columns.Add("worker");
            lvTracking.Columns[10].Width = 100;
            lvTracking.Columns.Add("state");
            lvTracking.Columns[11].Width = 100;
            lvTracking.Columns.Add("priority");
            lvTracking.Columns[12].Width = 100;
            lvTracking.Columns.Add("progress");
            lvTracking.Columns[COLUMN_COUNT-1].Width = 100;


            
        }

        private IOrderedEnumerable<KeyValuePair<int,TrackingInfo>> OrderItems(int columnIndex)
        {
            switch(columnIndex)
            {
                case 0: return mTracks.OrderBy(num => num.Value.workCurrent.recordID);
                case 1: return mTracks.OrderBy(num => num.Value.workCurrent.type);
                case 2: return mTracks.OrderBy(num => num.Value.workCurrent.time);
                case 3: return mTracks.OrderBy(num => num.Value.workCurrent.creator);
                case 4: return mTracks.OrderBy(num => num.Value.workCurrent.access);
                case 5: return mTracks.OrderBy(num => num.Value.workCurrent.mainCate);
                case 6: return mTracks.OrderBy(num => num.Value.workCurrent.subCate);
                case 7: return mTracks.OrderBy(num => num.Value.workCurrent.title);
                case 8: return mTracks.OrderBy(num => num.Value.workCurrent.comment);
                case 9: return mTracks.OrderBy(num => num.Value.workCurrent.director);
                case 10: return mTracks.OrderBy(num => num.Value.workCurrent.worker);
                case 11: return mTracks.OrderBy(num => num.Value.workCurrent.state);
                case 12: return mTracks.OrderBy(num => num.Value.workCurrent.priority);
                case COLUMN_COUNT-1: return mTracks.OrderBy(num => num.Value.workCurrent.progress);
                //case 14: return mTracks.OrderBy(num => num.Value.workCurrent.createTime);
                //case 15: return mTracks.OrderBy(num => num.Value.workCurrent.createTime);
                //case 16: return mTracks.OrderBy(num => num.Value.workCurrent.createTime);
                default: return null;
            }
        }

        private bool IsHide(Work work)
        {
            if (work.worker == MyInfo.mMyInfo.userID)
                return false;
            else
                return true;
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
                case ICD.DEF.CMD_TaskBaseList:
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
                UpdateTaskTracking();
            }
        }

        private void AddItemToListView(Work work)
        {
            string[] field = new string[COLUMN_COUNT];

            field[0] = work.recordID.ToString();
            field[1] = work.type;
            field[2] = work.time;
            field[3] = work.creator;
            field[4] = work.access;
            field[5] = work.mainCate;
            field[6] = work.subCate;
            field[7] = work.title;
            field[8] = work.comment;
            field[9] = work.director;
            field[10] = work.worker;
            field[11] = work.state;
            field[12] = work.priority;
            field[COLUMN_COUNT-1] = work.progress.ToString();

            lvTracking.Items.Add(new ListViewItem(field));
        }

        private void UpdateTaskTracking()
        {
            foreach (var item in mTracks)
            {
                item.Value.Update("today-15", "today+15", "today");
            }

            var orderedList = OrderItems(mCurFilterColumnIndex);
            lvTracking.Items.Clear();
            foreach (var item in orderedList)
            {
                if (IsHide(item.Value.workCurrent))
                    continue;

                AddItemToListView(item.Value.workCurrent);
            }
            
        }
    }
}
