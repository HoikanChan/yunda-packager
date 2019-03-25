using PackageSupplier.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageSupplier
{
    partial class Form1
    {
        

        #region TcpServer 事件
        private void ServerDisConnected(string clientIP)
        {
            if (clientIP.Contains(PLCIp) && clientIP.Contains(PLCPort))
            {
                UpdatePlcStateText(false);
                AddInfoLog("与PLC断开连接");
            }
            //if (clientIP.Contains(CameraIp) && clientIP.Contains(CameraPort))
            //{
            //    UpdateCameraStateText(false);
            //    AddInfoLog("与相机断开连接");
            //}
        }

        private void ServerCloseConnection(string ip)
        {
            throw new NotImplementedException();
        }

        private void ServerConneted(string clientIP)
        {
            if (clientIP.Contains(PLCIp) && clientIP.Contains(PLCPort))
            {
                UpdatePlcStateText(true);
                AddInfoLog("与PLC成功连接");
               
            }
            //if (clientIP.Contains(CameraIp) && clientIP.Contains(CameraPort))
            //{
            //    UpdateCameraStateText(true);
            //    AddInfoLog("与相机成功连接");
            //}
        }

        private void ServerReceiveMsg(string clientIP)
        {
            byte[] msgBugger = tcpServer.SocketInfoDict[clientIP].msgBuffer;
            string msg = Encoding.ASCII.GetString(msgBugger).Replace("\0", "").Trim();
            AddInfoLog(string.Format("由IP:({0})收到TCP数据-({1}))", clientIP, msg));
            if (clientIP.Contains(PLCIp) && clientIP.Contains(PLCPort))
            {
                //if(msg == "M")
                //{
                //    GetAveWeight();
                //}
                //else 
                if(msg.Length == 4)
                {
                    ReceiveCarNumber(msg);
                }
                else
                {
                    ReceiveCode(msg);

                }
            }
            //if (clientIP.Contains(CameraIp) && clientIP.Contains(CameraPort))
            //{
            //    ReceiveCameraMsg(msg);
            //}
        }

        private void ReceiveCarNumber(string msg)
        {
            if(CacheTable.Rows.Count <= CachedCarsCount)
            {
                AddInfoLog("还未收到条形码，车号为：" + msg);
                return;
            }
            CarCacheGrid.Invoke(new Action(() =>
            {
                CacheTable.Rows[CachedCarsCount][0] = msg;
                AddInfoLog("收到车号：" + msg);
                var row = CacheTable.Rows[CachedCarsCount];
                string carId = row[0].ToString();
                string code = row[1].ToString();
                //string weight = row[2].ToString();
                string resultMsg = String.Format("[C{0}]{1}", carId, code);
                try
                {
                    tcpClient.SendMsg(resultMsg);
                    AddInfoLog(string.Format("发送给服务器数据=>车号：{0}，条形码：{1}", carId, code));
                }
                catch (Exception ex)
                {
                    AddErrorLog("发送到供包结果到TCP服务器失败：" + resultMsg, ex);

                }
                CarInfoGrid.Invoke(new Action(() =>
                {
                    InfoTable.Rows.Add(new Object[] {
                        CachedCarsCount.ToString().PadLeft(4, '0'),
                        Times.GetDateNow(),
                        carId,
                        code
                        //weight
                    });     
                }));
                CachedCarsCount++;
            }));
        }

        //private void ReceiveCameraMsg(string msg)
        //{
        //    if (NeedToWeight)
        //    {
        //        if (CarCacheGrid.InvokeRequired)
        //        {
        //            this.Invoke(new Action(() =>
        //            {
        //                CacheTable.Rows.Add(new object[] { });
        //                CacheTable.Rows[CacheTable.Rows.Count - 1][1] = msg;
        //            }));
        //        }
        //        else
        //        {
        //            CacheTable.Rows.Add(new object[] { });
        //            CacheTable.Rows[CacheTable.Rows.Count - 1][1] = msg;
        //        }
        //        if (!IsSerialPortOpened)
        //        {
        //            InitSerialPort();
        //        }
        //        AddInfoLog("收到相机信息",msg);
        //        NeedToWeight = true;
        //    }
        //}

        private void ReceiveCode(string msg)
        {
            AddInfoLog("收到条形码：" + msg);
            CarCacheGrid.Invoke(new Action(() =>
            {
                if (CacheTable.Rows.Count <= CachedCarsCount)
                {
                    CacheTable.Rows.Add(new Object[] { });
                }
                else
                {
                    try
                    {
                        tcpServer.SendMsg("Go", PLCIp + ":" + PLCPort);
                        AddInfoLog("发送到PLC指令：Go,成功");
                    }catch(Exception ex)
                    {
                        AddErrorLog("发送到PLC指令：Go,失败", ex);
                    }

                }
                CacheTable.Rows[CachedCarsCount][1] = msg;
            }));
        }
        #endregion
    }
}
