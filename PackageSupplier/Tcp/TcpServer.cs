using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PackageSupplier.Tcp
{
    class TcpServer
    {
        public Dictionary<string, SocketInfo> SocketInfoDict = null;
        public Socket _socket = null;
        EndPoint _endPoint = null;
        public bool _islistening = false;
        int BACKLOG = 10;

        #region 线程全局变量
        public Thread socketConnectedThread = null;
        public Thread acceptServer = null;
        #endregion

        public delegate void OnConnectedHandler(string clientIP);
        public event OnConnectedHandler OnConnected;
        public delegate void OnReceiveMsgHandler(string ip);
        public event OnReceiveMsgHandler OnReceiveMsg;
        public event OnReceiveMsgHandler OnDisConnected;
        public event OnReceiveMsgHandler CloseConnected;

        public TcpServer(string ip, int port)
        {
            //使用Socket实例化一个_socket对象，以便来连接远程主机
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress _ip = IPAddress.Parse(ip);
            _endPoint = new IPEndPoint(_ip, port);
            SocketInfoDict = new Dictionary<string, SocketInfo>();
        }

        public void Start()
        {
            _socket.Bind(_endPoint);        //绑定端口
            _socket.Listen(BACKLOG);        //开启监听
            //Thread acceptServer = new Thread(AcceptWork);   //开启新的线程处理监听
            acceptServer = new Thread(AcceptWork);   //开启新的线程处理监听
            acceptServer.IsBackground = true;
            _islistening = true;
            acceptServer.Start();
        }

        public void Stop()
        {
            _islistening = false;
            foreach (SocketInfo s in SocketInfoDict.Values)
            {
                s.socket.Close();
            }
            SocketInfoDict.Clear();
            CloseConnected(SocketInfoDict.Count.ToString());
            _socket.Close();
            _socket = null;
        }

        //[J]:开启处理监听的线程
        public void AcceptWork()
        {
            while (_islistening)
            {
                Socket acceptSocket = null;
                //为新建连接创建新的Socket,负责跟客服端通信
                try
                {
                    acceptSocket = _socket.Accept();
                }
                catch
                {
                    acceptSocket = null;
                }
                if (acceptSocket != null && _islistening)
                {
                    SocketInfo sInfo = new SocketInfo();
                    sInfo.socket = acceptSocket;
                    //将远程连接的客服端的IP地址和Socket存入集合中
                    SocketInfoDict.Add(acceptSocket.RemoteEndPoint.ToString(), sInfo);
                    OnConnected(acceptSocket.RemoteEndPoint.ToString());
                    //开启一个新的线程，不断地接收客服端发来的信息
                    //Thread socketConnectedThread = new Thread(newSocketReceive);
                    socketConnectedThread = new Thread(newSocketReceive);
                    socketConnectedThread.IsBackground = true;
                    socketConnectedThread.Start(acceptSocket);
                }
                Thread.Sleep(200);
            }
        }

        //开启一个新的线程，不断地接收客服端发来的信息
        public void newSocketReceive(object obj)
        {
            if (_islistening)
            {
                Socket socket = obj as Socket;
                SocketInfo sInfo = SocketInfoDict[socket.RemoteEndPoint.ToString()];
                sInfo.isConnected = true;
                while (sInfo.isConnected)
                {
                    try
                    {
                        if (sInfo.socket == null)
                            return;
                        //这里向系统投递一个接收信息的请求，并为其指定ReceiveCallBack作为回调函数
                        sInfo.socket.BeginReceive(sInfo.buffer, 0, sInfo.buffer.Length,
                            SocketFlags.None, ReceiveCallBack, sInfo.socket.RemoteEndPoint);
                    }
                    catch
                    {
                        return;
                    }
                    Thread.Sleep(100);
                }
            }
        }

        //定义一个回调函数，当接收到数据的时候就调用
        private void ReceiveCallBack(IAsyncResult ar)
        {
            EndPoint ep = ar.AsyncState as IPEndPoint;
            //SocketInfo info = SocketInfoDict[ep.ToString()];
            SocketInfo info = null;
            if (SocketInfoDict.Keys.Contains(ep.ToString()))
            {
                info = SocketInfoDict[ep.ToString()];
            }
            else
            {
                info = null;
            }
            int readCount = 0;
            //[J]：下面这一段是做测试用的，当使用TCPIP通讯断开链接后
            //[J]:该回调函数会不断被调用，所以计数当没接收到数据超过100此就是断开了
            if (info != null)
            {
                info.disarm_count++;
                if (info.disarm_count > 100)
                {
                    info.isConnected = false;
                    SocketInfoDict.Remove(info.socket.RemoteEndPoint.ToString());
                    if (this.OnDisConnected != null)
                        OnDisConnected(info.socket.RemoteEndPoint.ToString());
                    info.socket.Close();
                    info.disarm_count = 0;
                    return;
                }
            }
            try
            {
                if (info.socket == null)
                    return;
                readCount = info.socket.EndReceive(ar);
            }
            catch
            {
                return;
            }
            if (readCount > 0)
            {
                info.disarm_count = 0;
                if (readCount < info.buffer.Length)
                {
                    byte[] newBuffer = new byte[readCount];
                    Buffer.BlockCopy(info.buffer, 0, newBuffer, 0, readCount);
                    info.msgBuffer = newBuffer;
                }
                else
                {
                    info.msgBuffer = info.buffer;
                }
                string msgTip = Encoding.ASCII.GetString(info.msgBuffer);
                //[J]:下面if内容中时使用配套的客服端参考代码时候才起作用的
                if (msgTip == "\0\0\0faild")
                {
                    info.isConnected = false;
                    if (this.OnDisConnected != null)
                        OnDisConnected(info.socket.RemoteEndPoint.ToString());
                    SocketInfoDict.Remove(info.socket.RemoteEndPoint.ToString());
                    info.socket.Close();
                    return;
                }
                //在这里注册调用了一个传参的OnReceiveMsg
                if (OnReceiveMsg != null)
                    //[J]:调用显示函数，我将在这里面添加一个调用查询数据库的功能
                    OnReceiveMsg(info.socket.RemoteEndPoint.ToString());
            }
        }

        //发送数据到客户端
        public void SendMsg(string text, string endPoint)
        {
            if (SocketInfoDict.Keys.Contains(endPoint) && SocketInfoDict[endPoint] != null)
            {
                SocketInfoDict[endPoint].socket.Send(Encoding.ASCII.GetBytes(text));
            }
        }

        public void SendMsg__ASCII(string text, string endPoint)
        {
            byte[] haha = { 0x10 };
            SocketInfoDict[endPoint].socket.Send(haha);
        }

        //[J]:定义一个数据结构类
        public class SocketInfo
        {
            public Socket socket = null;
            public byte[] buffer = null;
            public byte[] msgBuffer = null;
            public bool isConnected = false;
            public int disarm_count = 0;
            public SocketInfo()
            {
                buffer = new byte[1024 * 4];
            }
        }

    }
}
