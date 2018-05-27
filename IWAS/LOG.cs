using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IWAS
{
    class LOG
    {
        //디버깅용: 코드 흐름 추적을 위한 정보를 콘솔창에 남긴다.
        static public void trace(
            [CallerMemberName] string caller = null,
            [CallerLineNumber] int lineNumber = 0 )
        {
            Console.WriteLine("trace["+caller+"]["+lineNumber+"]");
        }

        //디버깅용: 간단한 정보와 함께 콘솔창에 남긴다.
        static public void echo<T>(T val,
            [CallerMemberName] string caller = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            Console.WriteLine("echo[" + caller + "][" + lineNumber + "]: " + val);
        }

        //경고 메세지박스를 띄어주어 개발자에게 강하게 알린다.
        static public void warn(
            [CallerMemberName] string caller = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            var st = new StackTrace();
            string msg = "";
            foreach(var frame in st.GetFrames())
            {
                msg += frame.GetMethod().ToString();
                msg += "\n";
            }
            MessageBox.Show("Warn!!\n" + msg);
        }

        //에러 메세지를 띄어주고 프로그램을 종료시킨다.(치명적 케이스인 경우 사용)
        static public void assert(
            [CallerMemberName] string caller = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            var st = new StackTrace();
            string msg = "";
            foreach (var frame in st.GetFrames())
            {
                msg += frame.GetMethod().ToString();
                msg += "\n";
            }
            MessageBox.Show("Assert!!\n" + msg);
            Process.GetCurrentProcess().Kill();
        }

    }
}
