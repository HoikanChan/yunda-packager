using PackageSupplier.Tcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PackageSupplier
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Queue<string> LogsQueue = new Queue<string>();
        DataTable InfoTable = new DataTable();
        DataTable CacheTable = new DataTable();

        TcpServer tcpServer;
        TcpClient tcpClient;

        #region 端口和IP  
        string ServerIp = ConfigurationManager.AppSettings["ServerIp"];
        int ServerPort = int.Parse(ConfigurationManager.AppSettings["ServerPort"]);
        string ClientTargetIp = ConfigurationManager.AppSettings["ClientTargetIp"];
        int ClientTargetrPort = int.Parse(ConfigurationManager.AppSettings["ClientTargetPort"]);
        string PLCIp = ConfigurationManager.AppSettings["PLCIp"];
        string PLCPort = ConfigurationManager.AppSettings["PLCPort"];
        string CameraIp = ConfigurationManager.AppSettings["CameraIp"];
        string CameraPort = ConfigurationManager.AppSettings["CameraPort"];

        #endregion

        public bool NeedToWeight = false;
        // 已缓存小车数量
        public int CachedCarsCount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void FormLoaded(object sender, EventArgs e)
        {
            InitTable();
            LogThread = new Thread(LogThreadMethod);
            LogThread.Start();
            TryConnectServerThread = new Thread(TryConnectServerMethod);
            TryConnectServerThread.Start();
            #region 初始化串口
            InitSerialPort();
            SerialPortThread = new Thread(SerialPortReadingMethod);
            SerialPortThread.IsBackground = true;
            SerialPortThread.Start();
            #endregion

            #region 启动TCPserver 等待相机和PLC连接
            tcpServer = new TcpServer(ServerIp, ServerPort);
            tcpServer.OnConnected += ServerConneted;
            tcpServer.CloseConnected += ServerCloseConnection;
            tcpServer.OnDisConnected += ServerDisConnected;
            tcpServer.OnReceiveMsg += ServerReceiveMsg;
            tcpServer.Start();
            #endregion
            InitTcpClient();
        }

       

        private void InitTable()
        {
            InfoTable.Columns.Add("序号", typeof(string));
            InfoTable.Columns.Add("时间", typeof(string));
            InfoTable.Columns.Add("小车", typeof(string));
            InfoTable.Columns.Add("条形码", typeof(string));
            InfoTable.Columns.Add("重量", typeof(string));
            InfoTable.Columns.Add("备注", typeof(string));
            CarInfoGrid.DataSource = InfoTable;
            CacheTable.Columns.Add("小车", typeof(string));
            CacheTable.Columns.Add("条形码", typeof(string));
            CacheTable.Columns.Add("重量", typeof(string));
            CarCacheGrid.DataSource = CacheTable;
        }

        //显示PLC1200连接状态
        private void UpdatePlcStateText(bool status)
        {
            if (status)
            {
                #region PlcStateText.Text = "主PLC连接成功";
                if (PlcStateText.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        PlcStateText.Text = "主PLC连接成功";
                    }));
                }
                else
                {
                    PlcStateText.Text = "主PLC连接成功";
                }
                #endregion
            }
            else
            {
                #region PlcStateText.Text = "主PLC断开连接";
                if (PlcStateText.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        PlcStateText.Text = "主PLC断开连接";
                    }));
                }
                else
                {
                    PlcStateText.Text = "主PLC断开连接";
                }
                #endregion
            }

        }

        private void UpdateServerStateText(bool status)
        {
            if (status)
            {
                #region serverStateText.Text = "与服务器连接成功";
                if (serverStateText.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        serverStateText.Text = "与服务器连接成功";
                    }));
                }
                else
                {
                    serverStateText.Text = "与服务器连接成功";
                }
                #endregion
            }
            else
            {
                #region CameraStateText.Text = "与服务器断开连接";
                if (serverStateText.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        serverStateText.Text = "与服务器断开连接";
                    }));
                }
                else
                {
                    serverStateText.Text = "与服务器断开连接";
                }
                #endregion
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CarCacheGrid_Click(object sender, EventArgs e)
        {

        }
    }
}
