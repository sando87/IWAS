using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWAS
{
    class ClientInfo
    {
        static public Dictionary<uint, ICD.Task> mTasks = new Dictionary<uint, ICD.Task>();
        static public Dictionary<string, ICD.User> mUsers = new Dictionary<string, ICD.User>();
        static public ICD.User mMyInfo = new ICD.User();
    }
}
