using dnSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X2292_ChassisFinalAssyAOI
{
   

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //处理未捕获的异常
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


            bool createNew;
            // createdNew:  
            // 在此方法返回时，如果创建了局部互斥体（即，如果 name 为 null 或空字符串）或指定的命名系统互斥体，则包含布尔值 true；  
            // 如果指定的命名系统互斥体已存在，则为false  
            using (Mutex mutex = new Mutex(true, Application.ProductName, out createNew))
            {
                if (createNew)
                {
                    //SP.ShowSplash("DNb.png");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frm_Main());
                }
                // 程序已经运行的情况，则弹出消息提示并终止此次运行  
                else
                {
                    MessageBox.Show("应用程序已经在运行中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    System.Threading.Thread.Sleep(1000);

                    // 终止此进程并为基础操作系统提供指定的退出代码。  
                    System.Environment.Exit(1);
                }
            }     
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = "Application_ThreadException:" + DateTime.Now.ToString() + "\r\n";
            Exception error = e.Exception as Exception;
            if (error != null)
            {
                str += string.Format("Name:{0}\r\nMessage:{1}\r\nStackTrace:{2}\r\n",
                     error.GetType().Name, error.Message, error.StackTrace);
            }
            else
            {
                str += string.Format("Exception:{0}", e);
            }

            MessageBox.Show(str);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = "CurrentDomain_UnhandledException:" + DateTime.Now.ToString() + "\r\n";
            Exception error = e.ExceptionObject as Exception;
            if (error != null)
            {
                str += string.Format("Message:{0}\r\nStackTrace:{1}\r\n", error.Message, error.StackTrace);
            }
            else
            {
                str += string.Format("Exception:{0}", e);
            }
            MessageBox.Show(str);           
        }

    }
}
