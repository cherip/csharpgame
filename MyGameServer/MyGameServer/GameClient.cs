using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MyGameServer
{
    /// <summary>
    /// Client 的摘要说明。
    /// </summary>
    public class GameClient  // 该对象包含了客户端的一些相关信息，该信息被保存在一个数组列表中
    {
        private Thread clthread;

        private EndPoint endpoint;

        private string name;

        private Socket sock;

        private int tableIdx;
        public int SeatIdx;

        public GameClient(string _name, EndPoint _endpoint, Thread _thread, Socket _sock)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            clthread = _thread;

            endpoint = _endpoint;

            name = _name;

            sock = _sock;

            tableIdx = -1;
        }
        public override string ToString()
        {

            return endpoint.ToString() + " : " + name;

        }

        public Thread CLThread  // 获取和设置线程
        {

            get { return clthread; }

            set { clthread = value; }

        }

        public EndPoint Host // 获取和设置终端
        {

            get { return endpoint; }

            set { endpoint = value; }

        }

        public string Name  // 获取和设置client名称
        {

            get { return name; }

            set { name = value; }

        }

        public int TableIdx
        {
            get { return tableIdx; }

            set { tableIdx = value; }
        }

        public Socket Sock  // 获取和设置套接口
        {

            get { return sock; }

            set { sock = value; }

        }


    }
}
