using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace IWAS
{
    class DatabaseMgr
    {
        /*
        string sql_a1 = "SELECT * FROM user WHERE id='sando' ORDER BY date DESC";
        string sql_a2 = "SELECT * FROM user WHERE auth IN(2,3)";
        string sql_a3 = "SELECT * FROM user WHERE auth BETWEEN 1 AND 3";
        string sql_a4 = "SELECT * FROM user WHERE date LIKE '2018%'";
        string sql_a5 = "SELECT * FROM user WHERE date LIKE '2018__05'";
        string sql_b1 = "INSERT INTO user (id, pw, date, auth) " +
            "VALUES('test', 'test', '20180507', 2)";
        string sql_b2 = "INSERT INTO user VALUES('test', 'test', '20180507', 2)";
        string sql_c = "DELETE FROM user WHERE id='sando'";
        string sql_d = "UPDATE user SET auth=1 WHERE id='sando'";
        */

        static private MySqlConnection mConn = null;
        public static void Open()
        {
            string strConn = "Server=localhost;Database=test;Uid=root;Pwd=root;SslMode=none;";
            mConn = new MySqlConnection(strConn);

            try { mConn.Open(); }
            catch (Exception e) { LOG.echo(e.ToString()); }
        }
        public static void Close()
        {
            if (mConn != null)
                mConn.Close();
        }
        public static DataRow GetUserInfo(string key)
        {
            string sql = "SELECT * FROM user WHERE recordID='" + key + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            int ret = adapter.Fill(ds, "USER");
            if (ret != 1)
                return null;

            //try { adapter.Fill(ds, "USER"); }
            //catch (Exception e) { LOG.echo(e.ToString()); return null; }

            return ds.Tables["USER"].Rows[0];
        }
        public static DataTable GetUsers()
        {
            string sql = "SELECT * FROM user";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            int ret = adapter.Fill(ds, "USERS");
            if (ret == 0)
                return null;

            return ds.Tables["USERS"];
        }
        public static int NewUser(ICD.User info)
        {
            string sql = string.Format(
                "INSERT INTO user " +       //user DataBase
                "(recordID, password, time, auth) " +   //Column name
                "VALUES ('{0}', '{1}', '{2}', {3})",     //values list
                info.userID,
                info.userPW,
                info.msgTime,
                2);


            MySqlCommand cmd = new MySqlCommand(sql, mConn);
            int ret = cmd.ExecuteNonQuery();
            return ret;
        }
        public static DataRow NewTask(ICD.WorkList msg)
        {
            if(msg.works == null || msg.works.Length != 1)
            {
                LOG.warn();
                return null;
            }
            ICD.Work info = msg.works[0];

            string sql = string.Format(
                "INSERT INTO task " +
                "(type, time, creator, access, mainCate, subCate, title, comment, director, worker, launch, due, term, state, priority, progress, chatID, timeFirst, timeDone) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}','{14}', '{15}', '{16}', '{17}', '{18}')",
                info.type,
                msg.msgTime,
                info.creator,
                info.access,
                info.mainCate,
                info.subCate,
                info.title,
                info.comment,
                info.director,
                info.worker,
                info.launch,
                info.due,
                info.term,
                info.state,
                info.priority,
                info.progress,
                info.chatID,
                info.timeFirst,
                info.timeDone);

            MySqlCommand cmd = new MySqlCommand(sql, mConn);
            cmd.ExecuteNonQuery();

            sql = string.Format("SELECT * FROM task WHERE time='{0}' AND creator='{1}'", msg.msgTime, info.creator);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            adapter.Fill(ds, "TASK");

            return ds.Tables["TASK"].Rows[0];
        }

        public static int EditTaskBase(int taskID, string columnName, string data)
        {
            string sql = string.Format(
                "UPDATE task SET {0}='{1}' WHERE recordID='{2}'",
                columnName,
                data,
                taskID.ToString());

            MySqlCommand cmd = new MySqlCommand(sql, mConn);
            int ret = cmd.ExecuteNonQuery();

            return ret;
        }

        public static int EditTask(ICD.WorkHistoryList msg)
        {
            int ret = 0;
            foreach (var item in msg.workHistory)
            {
                string sql = string.Format(
                    "INSERT INTO taskHistory " +
                    "(taskID, time, user, columnName, fromInfo, toInfo) " +
                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                    item.taskID,
                    msg.msgTime,
                    msg.msgUser,
                    item.columnName,
                    item.fromInfo,
                    item.toInfo);

                MySqlCommand cmd = new MySqlCommand(sql, mConn);
                ret += cmd.ExecuteNonQuery();
            }
            return ret;
        }
        public static DataTable GetTasks(long from, long to)
        {
            string sql = string.Format(
                "SELECT * FROM task WHERE timeDone>={0} AND time<={1}",
                from, to);

            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            int ret = adapter.Fill(ds, "TASK");
            if (ret == 0)
                return null;

            return ds.Tables["TASK"];
        }
        public static DataTable GetTasks(string user)
        {
            string sql = string.Format(
                "SELECT * FROM task WHERE worker='{0}' OR director='{1}'",
                user, user);

            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            int ret = adapter.Fill(ds, "TASK");
            if (ret == 0)
                return null;

            return ds.Tables["TASK"];
        }
        public static DataRow GetTaskRoot(int taskID)
        {
            string sql = "SELECT * FROM task WHERE recordID=" + taskID;
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            if (adapter.Fill(ds, "TASK") == 0)
                return null;

            return ds.Tables["TASK"].Rows[0];
        }
        public static DataTable GetTaskHistory(int taskID)
        {
            string sql = "SELECT * FROM taskHistory WHERE taskID=" + taskID +
                " ORDER BY time ASC";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            if (adapter.Fill(ds, "TASKHis") == 0)
                return null;

            return ds.Tables["TASKHis"];
        }
        public static void GetTaskLatest(int taskID, ref ICD.Work task)
        {
            DataRow taskRoot = GetTaskRoot(taskID);
            if (taskRoot == null)
                return;

            task.recordID = (int)taskRoot["recordID"];
            task.type = taskRoot["type"].ToString();
            task.time = (long)taskRoot["time"];
            task.creator = taskRoot["creator"].ToString();
            task.access = taskRoot["access"].ToString();
            task.mainCate = taskRoot["mainCate"].ToString();
            task.subCate = taskRoot["subCate"].ToString();
            task.title = taskRoot["title"].ToString();
            task.comment = taskRoot["comment"].ToString();
            task.director = taskRoot["director"].ToString();
            task.worker = taskRoot["worker"].ToString();
            task.launch = (long)taskRoot["launch"];
            task.due = (long)taskRoot["due"];
            task.term = (long)taskRoot["term"];
            task.state = taskRoot["state"].ToString();
            task.priority = taskRoot["priority"].ToString();
            task.progress = (int)taskRoot["progress"];
            task.chatID = (int)taskRoot["chatID"];
            task.timeFirst = (long)taskRoot["timeFirst"];
            task.timeDone = (long)taskRoot["timeDone"];

            DataTable taskHis = GetTaskHistory(taskID);
            if (taskHis == null)
                return;

            foreach (DataRow item in taskHis.Rows)
            {
                string name = item["columnName"].ToString();
                switch (name)
                {
                    case "access":      task.access = item["toInfo"].ToString(); break;
                    case "mainCate":    task.mainCate = item["toInfo"].ToString(); break;
                    case "subCate":     task.subCate = item["toInfo"].ToString(); break;
                    case "title":       task.title = item["toInfo"].ToString(); break;
                    case "comment":     task.comment = item["toInfo"].ToString(); break;
                    case "director":    task.director = item["toInfo"].ToString(); break;
                    case "worker":      task.worker = item["toInfo"].ToString(); break;
                    case "launch":      task.launch = (long)item["toInfo"]; break;
                    case "due":         task.due = (long)item["toInfo"]; break;
                    case "term":        task.term = (long)item["toInfo"]; break;
                    case "state":       task.state = item["toInfo"].ToString(); break;
                    case "priority":    task.priority = item["toInfo"].ToString(); break;
                    case "progress":    task.progress = (int)item["toInfo"]; break;
                    case "chatID":      task.chatID = (int)item["toInfo"]; break;
                    case "reportMid":      task.state = "진행"; break;
                    case "reportDone":      task.state = "완료대기"; break;
                    case "confirmOK":      task.state = "완료"; break;
                    case "confirmNO":      task.state = "진행"; break;
                    default:            LOG.warn(); break;
                }
            }
        }
        public static DataTable GetChatMessages(int chatID)
        {
            string sql = string.Format(
                "SELECT * FROM chatHistory WHERE chatID={0} AND type='msg'", chatID);
            sql += " ORDER BY time ASC";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            if (adapter.Fill(ds, "ChatHis") == 0)
                return null;

            return ds.Tables["ChatHis"];
        }
        public static DataTable GetChatUsers(int chatID)
        {
            string sql = string.Format(
                "SELECT * FROM chatHistory WHERE chatID={0} AND type LIKE '___User'", chatID);
            sql += " ORDER BY time ASC";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            if (adapter.Fill(ds, "ChatHis") == 0)
                return null;

            return ds.Tables["ChatHis"];
        }
        public static DataTable GetChatTasks(int chatID)
        {
            string sql = string.Format(
                "SELECT * FROM chatHistory WHERE chatID={0} AND type LIKE '___Task'", chatID);
            sql += " ORDER BY time ASC";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            if (adapter.Fill(ds, "ChatHis") == 0)
                return null;

            return ds.Tables["ChatHis"];
        }
        public static DataRow GetChatRoomInfo(int chatID)
        {
            string sql = string.Format(
                "SELECT * FROM chat WHERE recordID={0}", chatID);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            if (adapter.Fill(ds, "Chat") == 0)
                return null;

            return ds.Tables["Chat"].Rows[0];
        }
        public static DataTable GetChatRoomList()
        {
            string sql = string.Format("SELECT * FROM chat");
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            if (adapter.Fill(ds, "Chat") == 0)
                return null;

            return ds.Tables["Chat"];
        }

        public static DataRow PushNewChat(ICD.ChatRoomInfo info)
        {
            string sql = string.Format(
                "INSERT INTO chat " +
                "(time, creator, access) " +
                "VALUES ('{0}', '{1}', '{2}')",
                info.msgTime,
                info.msgUser,
                info.body.access);

            MySqlCommand cmd = new MySqlCommand(sql, mConn);
            cmd.ExecuteNonQuery();

            sql = string.Format("SELECT * FROM chat WHERE time='{0}' AND creator='{1}'", info.msgTime, info.msgUser);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            adapter.Fill(ds, "Chat");

            return ds.Tables["Chat"].Rows[0];

        }

        public static void AddChatUsers(ICD.ChatRoomInfo info)
        {
            foreach(var user in info.body.users)
            {
                string sql = string.Format(
                    "INSERT INTO chatHistory " +
                    "(chatID, time, user, type, info) " +
                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                    info.body.recordID,
                    info.msgTime,
                    info.msgUser,
                    "addUser",
                    user);

                MySqlCommand cmd = new MySqlCommand(sql, mConn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DelChatUsers(ICD.ChatRoomInfo info)
        {
            foreach (var user in info.body.users)
            {
                string sql = string.Format(
                    "INSERT INTO chatHistory " +
                    "(chatID, time, user, type, info) " +
                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                    info.body.recordID,
                    info.msgTime,
                    info.msgUser,
                    "delUser",
                    user);

                MySqlCommand cmd = new MySqlCommand(sql, mConn);
                cmd.ExecuteNonQuery();
            }

        }

        public static void AddChatTasks(ICD.ChatRoomInfo info)
        {
            foreach (int id in info.body.taskIDs)
            {
                string sql = string.Format(
                    "INSERT INTO chatHistory " +
                    "(chatID, time, user, type, info) " +
                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                    info.body.recordID,
                    info.msgTime,
                    info.msgUser,
                    "addTask",
                    id);

                MySqlCommand cmd = new MySqlCommand(sql, mConn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DelChatTasks(ICD.ChatRoomInfo info)
        {
            foreach(int id in info.body.taskIDs)
            {
                string sql = string.Format(
                    "INSERT INTO chatHistory " +
                    "(chatID, time, user, type, info) " +
                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                    info.body.recordID,
                    info.msgTime,
                    info.msgUser,
                    "delTask",
                    id);

                MySqlCommand cmd = new MySqlCommand(sql, mConn);
                cmd.ExecuteNonQuery();
            }
        }

        public static DataRow PushChatMessage(ICD.ChatRoomInfo info)
        {
            if (info.body.mesgs.Length != 1)
            {
                LOG.warn();
                return null;
            }

            string sql = string.Format(
                "INSERT INTO chatHistory " +
                "(chatID, time, user, type, info) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                info.body.recordID,
                info.msgTime,
                info.msgUser,
                "msg",
                info.body.mesgs[0].message);

            MySqlCommand cmd = new MySqlCommand(sql, mConn);
            cmd.ExecuteNonQuery();

            sql = string.Format("SELECT * FROM chatHistory WHERE time='{0}' AND user='{1}'", info.msgTime, info.msgUser);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            adapter.Fill(ds, "Chat");

            return ds.Tables["Chat"].Rows[0];

        }
    }
}
