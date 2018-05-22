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
        string sql_c = "DELETE user WHERE id='sando'";
        string sql_d = "UPDATE user SET auth=1 WHERE id='sando'";
        */

        static private MySqlConnection mConn = null;
        public static void Open()
        {
            string strConn = "Server=localhost;Database=test;Uid=root;Pwd=root;";
            mConn = new MySqlConnection(strConn);
            mConn.Open();
        }
        public static void Close()
        {
            if(mConn!=null)
                mConn.Close();
        }
        public static DataRow GetUserInfo(string key)
        {
            string sql = "SELECT * FROM user WHERE id='" + key + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            int ret = adapter.Fill(ds, "USER");
            if (ret != 1)
                return null;

            return ds.Tables["USER"].Rows[0];
        }
        public static int NewUser(ICD.User info)
        {
            string sql = string.Format(
                "INSERT INTO user " +       //user DataBase
                "(id, pw, date, auth) " +   //Column name
                "VALUES ('{0}', '{1}', '{2}', {3}",     //values list
                info.userID,
                info.userPW,
                info.msgTime,
                2);


            MySqlCommand cmd = new MySqlCommand(sql, mConn);
            int ret = cmd.ExecuteNonQuery();
            return ret;
        }
        public static DataRow NewTask(ICD.Task info)
        {
            string sql = string.Format(
                "INSERT INTO task " +
                "(type, time, creator, access, title, director, worker, chatID) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}', '{7}')",
                info.msgType,
                info.msgTime,
                info.creator,
                info.access,
                info.title,
                info.director,
                info.worker,
                0);

            MySqlCommand cmd = new MySqlCommand(sql, mConn);
            int ret = cmd.ExecuteNonQuery();

            sql = string.Format("SELECT * FROM task WHERE time={0} AND creator={1}", info.msgTime, info.creator);
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            if (adapter.Fill(ds, "TASK") == 0)
                return null;

            return ds.Tables["TASK"].Rows[0];
        }
        public static int EditTask(ICD.TaskEdit info)
        {
            string sql = string.Format(
                "INSERT INTO taskHistory " +
                "(taskID, time, user, info) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}')",
                info.taskID,
                info.msgTime,
                info.msgUser,
                info.info );

            MySqlCommand cmd = new MySqlCommand(sql, mConn);
            int ret = cmd.ExecuteNonQuery();
            return ret;
        }
        public static DataTable GetTasks(string user)
        {
            string sql = "SELECT * FROM task WHERE worker='" + user + "'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mConn);

            DataSet ds = new DataSet();
            int ret = adapter.Fill(ds, "TASK");
            if (ret == 0)
                return null;

            return ds.Tables["TASK"];
        }
        public static DataRow GetTaskRoot(int taskID)
        {
            string sql = "SELECT * FROM task WHERE id=" + taskID;
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
            if(adapter.Fill(ds, "TASKHis") == 0)
                return null;

            return ds.Tables["TASKHis"];
        }
        public static void GetTaskLatest(int taskID, ref ICD.Task task)
        {
            DataRow taskRoot = GetTaskRoot(taskID);
            task.taskID = (uint)taskRoot["id"];
            task.cmdID = (uint)taskRoot["type"];
            task.createTime = taskRoot["time"].ToString();
            task.creator = taskRoot["creator"].ToString();
            task.access = taskRoot["access"].ToString();
            task.title = taskRoot["title"].ToString();
            task.director = taskRoot["director"].ToString();
            task.worker = taskRoot["woker"].ToString();

            DataTable taskHis = GetTaskHistory(taskID);
            foreach(DataRow item in taskHis.Rows)
            {
                string value = item["info"].ToString();
                string[] infos = value.Split(',');
                foreach(string info in infos)
                {
                    string[] data = info.Split(':');
                    switch (data[0])
                    {
                        case "title":
                            task.title = data[1];
                            break;
                        case "comment":
                            task.comment = data[1];
                            break;
                        case "launch":
                            task.preLaunch = data[1];
                            break;
                        case "access":
                            task.access = data[1];
                            break;
                        case "director":
                            task.director = data[1];
                            break;
                        case "woker":
                            task.worker = data[1];
                            break;
                        case "progress":
                            task.progress = uint.Parse(data[1]);
                            break;
                        default:
                            LOG.warn();
                            break;
                    }
                }
                
            }
        }
    }
}
