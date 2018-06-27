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
            public DateTime time;
            public string type;
            public string message;
        }

        class TrackingInfo
        {
            public Work workBase;
            public Work workCurrent;
            public WorkHistory[] his;
            public List<ReportInfo> reports = new List<ReportInfo>();

            public void Update(DateTime date)
            {
                TimeSpan span = new TimeSpan(VIEW_PERIOD, 0, 0, 0);
                long from = date.Ticks - span.Ticks;
                long to = date.Ticks + span.Ticks;
                long today = date.Ticks;
                long workCreate = DateTime.Parse(workBase.time).Ticks;
                long workOpen = DateTime.Parse(workBase.timeFirst).Ticks;
                long workClose = DateTime.Parse(workBase.timeDone).Ticks;
                if (to < workOpen || workClose < from || today < workCreate)
                {
                    workCurrent = null;
                    reports.Clear();
                    return;
                }

                if(his == null || his.Length == 0)
                {
                    workCurrent = workBase.Clone();
                    return;
                }

                reports.Clear();
                workCurrent = workBase.Clone();
                long current = DateTime.Parse(his[0].time).Ticks;
                int i = 0;
                while (current <= today && i < his.Length)
                {
                    WorkHistory curHis = his[i];
                    switch (curHis.columnName)
                    {
                        case "access": workCurrent.access = curHis.toInfo; break;
                        case "mainCate": workCurrent.mainCate = curHis.toInfo; break;
                        case "subCate": workCurrent.subCate = curHis.toInfo; break;
                        case "title": workCurrent.title = curHis.toInfo; break;
                        case "comment": workCurrent.comment = curHis.toInfo; break;
                        case "director": workCurrent.director = curHis.toInfo; break;
                        case "worker": workCurrent.worker = curHis.toInfo; break;
                        case "launch": workCurrent.launch = curHis.toInfo; break;
                        case "due": workCurrent.due = curHis.toInfo; break;
                        case "term": workCurrent.term = curHis.toInfo; break;
                        case "state": workCurrent.state = curHis.toInfo; break;
                        case "priority": workCurrent.priority = curHis.toInfo; break;
                        case "progress": workCurrent.progress = int.Parse(curHis.toInfo); break;
                        case "chatID": workCurrent.chatID = int.Parse(curHis.toInfo); break;
                        case "reportMid":
                        case "reportDone":
                        case "confirmOK":
                        case "confirmNO":
                            {
                                string[] data = curHis.toInfo.Split(',', (char)2);
                                ReportInfo rep = new ReportInfo();
                                rep.time = DateTime.Parse( data[0] );
                                rep.type = curHis.columnName;
                                rep.message = data[1];
                                reports.Add(rep);
                                UpdateWorkState(workCurrent, rep.type);
                            }
                            break;
                        default: break;
                    }
                    current = DateTime.Parse(curHis.time).Ticks;
                    i++;
                }

            }
            private void UpdateWorkState(Work work, string type)
            {
                switch (type)
                {
                    case "reportMid": work.state = "진행"; break;
                    case "reportDone": work.state = "완료대기"; break;
                    case "confirmOK": work.state = "완료"; break;
                    case "confirmNO": work.state = "진행"; break;
                }
            }
        }

        Dictionary<int, TrackingInfo> mTracks = new Dictionary<int, TrackingInfo>();
        private int mCount = 0;
        private int mCurFilterColumnIndex = 0;
        private DateTime mCurrentTime = new DateTime();

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

            mCurrentTime = DateTime.Now;
            TimeSpan span = new TimeSpan(VIEW_PERIOD, 0, 0, 0);
            DateTime from = mCurrentTime - span;
            DateTime to = mCurrentTime + span;
            RequestTaskList(from, to);
        }

        private void RequestTaskList(DateTime from, DateTime to)
        {
            HEADER msg = new HEADER();
            msg.FillClientHeader(DEF.CMD_TaskBaseList);

            msg.ext1 = from.ToString("yyyy-MM-dd HH:mm:ss");
            msg.ext2 = to.ToString("yyyy-MM-dd HH:mm:ss");

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
                case 0: return mTracks.OrderBy(num => num.Value.workCurrent?.recordID);
                case 1: return mTracks.OrderBy(num => num.Value.workCurrent?.type);
                case 2: return mTracks.OrderBy(num => num.Value.workCurrent?.time);
                case 3: return mTracks.OrderBy(num => num.Value.workCurrent?.creator);
                case 4: return mTracks.OrderBy(num => num.Value.workCurrent?.access);
                case 5: return mTracks.OrderBy(num => num.Value.workCurrent?.mainCate);
                case 6: return mTracks.OrderBy(num => num.Value.workCurrent?.subCate);
                case 7: return mTracks.OrderBy(num => num.Value.workCurrent?.title);
                case 8: return mTracks.OrderBy(num => num.Value.workCurrent?.comment);
                case 9: return mTracks.OrderBy(num => num.Value.workCurrent?.director);
                case 10: return mTracks.OrderBy(num => num.Value.workCurrent?.worker);
                case 11: return mTracks.OrderBy(num => num.Value.workCurrent?.state);
                case 12: return mTracks.OrderBy(num => num.Value.workCurrent?.priority);
                case COLUMN_COUNT-1: return mTracks.OrderBy(num => num.Value.workCurrent?.progress);
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

            HEADER msgHis = new HEADER();
            msgHis.FillClientHeader(DEF.CMD_TaskHistory);

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
            
            if (msg.workHistory.Length > 0)
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

        private void UpdateTaskTracking()
        {
            lbTodayDate.Text = mCurrentTime.ToString("yyyy-MM-dd HH:mm:ss");
            foreach (var item in mTracks)
            {
                item.Value.Update(mCurrentTime);
            }

            var orderedList = OrderItems(mCurFilterColumnIndex);
            lvTracking.Items.Clear();
            foreach (var item in orderedList)
            {
                if (item.Value.workCurrent==null || IsHide(item.Value.workCurrent))
                    continue;

                string[] row = ConvertToStringList(item.Value.workCurrent);
                lvTracking.Items.Add(new ListViewItem(row));
            }
            
        }

        private string[] ConvertToStringList(Work work)
        {
            string[] fields = new string[COLUMN_COUNT];

            fields[0] = work.recordID.ToString();
            fields[1] = work.type;
            fields[2] = work.time;
            fields[3] = work.creator;
            fields[4] = work.access;
            fields[5] = work.mainCate;
            fields[6] = work.subCate;
            fields[7] = work.title;
            fields[8] = work.comment;
            fields[9] = work.director;
            fields[10] = work.worker;
            fields[11] = work.state;
            fields[12] = work.priority;
            fields[COLUMN_COUNT - 1] = work.progress.ToString();

            return fields;
        }

        private void btnLater_Click(object sender, EventArgs e)
        {
            TimeSpan span = new TimeSpan(1, 0, 0, 0);
            mCurrentTime += span;
            UpdateTaskTracking();
        }

        private void btnAgo_Click(object sender, EventArgs e)
        {
            TimeSpan span = new TimeSpan(1, 0, 0, 0);
            mCurrentTime -= span;
            UpdateTaskTracking();
        }

    }
}
