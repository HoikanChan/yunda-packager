using PackageSupplier.Tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PackageSupplier
{
    partial class Form1
    {
        bool IsConnectedWithServer = false;
        Thread TryConnectServerThread;
        private void TryConnectServerMethod()
        {
            Thread.Sleep(5000);  //等待5s后启动连接分拣机程序
            while (true)
            {
                if (!IsConnectedWithServer)
                {
                    tcpClient = null;
                    Thread.Sleep(50);
                    InitTcpClient();
                    Thread.Sleep(5000);
                }
                Thread.Sleep(10000);
            }
        }
        private void InitTcpClient()
        {
            #region  启动TCPClient 与服务器相连
            tcpClient = new TcpClient(ClientTargetIp, ClientTargetrPort);
            tcpClient.OnConnected += OnClientConneted;
            tcpClient.OnFaildConnect += OnClientFialdConnect;
            tcpClient.Start();
            #endregion
        }
        private void OnClientFialdConnect()
        {
            IsConnectedWithServer = false;
            UpdateServerStateText(false);
            AddInfoLog("与服务器连接失败");
        }

        private void OnClientConneted()
        {
            IsConnectedWithServer = true;
            UpdateServerStateText(true);
            AddInfoLog("与服务器连接成功");
        }
    }
}
