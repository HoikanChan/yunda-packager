using PackageSupplier.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PackageSupplier
{
    public partial class Form1
    {
        int log_length = 1000;
        // 【线程】数据库写入数据的线程
        Thread LogThread = null;
        public void AddInfoLog(string title, string content)
        {
            string msg = "【" + title + "】" + content;
            LogsQueue.Enqueue(msg);
            string logMsg = String.Format("{{0}} {1}", Times.GetDateNow(), msg);

        }
        public void AddInfoLog(string title)
        {
            LogsQueue.Enqueue("【" + title + "】");
        }

        public void AddErrorLog(string title, Exception e)
        {
            LogsQueue.Enqueue("【" + title + "】" + e.Message);
        }
        // 【线程函数】数据库写入数据的线程
        private void LogThreadMethod()
        {
            Thread.Sleep(3000);
            while (true)
            {
                if (LogsQueue.Count != 0)
                {
                    try
                    {
                        string logstring = LogsQueue.Dequeue();
                        Logger.WriteLog_info(typeof(Form1), logstring);
                        bool scroll = false;

                        MessageListControl.Invoke(new Action(() =>
                        {
                            #region 写入到测试窗口
                            int FullIndex = MessageListControl.ItemHeight == 0 ?
                            0 :
                            MessageListControl.Items.Count - (int)(MessageListControl.Height / MessageListControl.ItemHeight);

                            if (MessageListControl.TopIndex == FullIndex)
                            {
                                scroll = true;
                            }
                            MessageListControl.Items.Add(string.Format("[{0}]-{1}", Times.GetDateNow(), logstring));
                            if (scroll)
                            {
                                MessageListControl.TopIndex = FullIndex;
                            }
                            if (MessageListControl.Items.Count > log_length)
                            {
                                MessageListControl.Items.Clear();
                            }
                            #endregion
                        }));
                    }
                    catch { }
                }
                Thread.Sleep(25);
            }
        }
    }
}
