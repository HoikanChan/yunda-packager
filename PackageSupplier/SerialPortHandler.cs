using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace PackageSupplier
{
    public partial class Form1
    {
        Thread SerialPortThread = null;     //串口状态
        SerialPort serialPort = new SerialPort();
        bool canSerialPortRead = false;
        bool IsSerialPortOpened = false;
        List<string> WeightCacheList = new List<string>();
        #region 初始化串口
        void InitSerialPort()
        {
            serialPort.PortName = "com2";
            serialPort.BaudRate = int.Parse(ConfigurationManager.AppSettings["BaudRate"]);
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.ReadTimeout = 5000;
            //串口设置接收数据的回调方法
            serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(SerialPortDataReceiver);
            Thread.Sleep(10);
            try
            {
                serialPort.Open();
                AddInfoLog("串口连接成功");
                IsSerialPortOpened = true;
                UpdateSerialPortText(true);
            }
            catch (Exception ex)
            {
                AddErrorLog("串口连接失败", ex);
            }
        }
        private void SerialPortDataReceiver(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string weightString = serialPort.ReadExisting();
                Console.WriteLine(weightString);
                string weight = weightString.Replace("kg", "");
                WeightCacheList.Add(weight);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        #endregion
        //串口自动关闭的线程函数
        private void SerialPortReadingMethod()
        {
            while (true)
            {
                if (canSerialPortRead)
                {
                    //count_close = count_close + 1;
                    //if (count_close > 120)
                    {
                        canSerialPortRead = false;
                        serialPort.Close();
                        //show_serialport_text_status(false);
                        //logstring = "串口自动关闭";
                        //log_string.add(logstring);
                        //loghelper.writelog_info(typeof(form1), logstring);
                    }
                }
                Thread.Sleep(30000);
            }
        }
        void GetAveWeight()
        {
            if (WeightCacheList.Count >= 6)
            {
                try
                {
                    #region 取六个最新数据，去极值取均值
                    List<double> weightList = WeightCacheList.GetRange((WeightCacheList.Count) - 6, 6)
                   .Select(w => Convert.ToDouble(w)).ToList<double>();
                    double max = weightList.Max();
                    weightList.Remove(max);
                    //去除最小
                    double min = weightList.Min();
                    weightList.Remove(min);
                    //取平均值
                    double avg = weightList.Average();
                    AddInfoLog("收到称重指令，平均重量为:" + avg.ToString());
                    #endregion

                    WeightCacheList.Clear();

                    CarCacheGrid.Invoke(new Action(() =>
                    {
                        if(CacheTable.Rows.Count <= CachedCarsCount)
                        {
                            CacheTable.Rows.Add(new Object[] { });
                        }
                        else
                        {
                            try
                            {
                                tcpServer.SendMsg("Go", PLCIp + ":" + PLCPort);
                                AddInfoLog("发送到PLC指令：Go,成功");
                            }
                            catch (Exception ex)
                            {
                                AddErrorLog("发送到PLC指令：Go,失败", ex);
                            }
                        }
                        CacheTable.Rows[CachedCarsCount][2] = avg.ToString() + "kg";
                    }));
                }
                catch (Exception ex)
                {
                    CarCacheGrid.Invoke(new Action(() =>
                    {
                        CacheTable.Rows[CachedCarsCount][2] =  "空";
                    }));
                    AddErrorLog("获取平均称重失败", ex);
                }
            }
            else
            {
                AddInfoLog("错误！！收到称重指令，但还未称重");
            }
        }
    }
}
