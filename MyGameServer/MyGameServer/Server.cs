using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace MyGameServer
{
    public partial class Server : Form
    {
        private TcpListener listener;//用于服务器端编程，用于监听端口
        private int port = 8668;//用来记录端口号
        private Socket clientsocket;//用于接受来自客户端的连接请求
        private Thread clientservice;//定义一个线程，用于对应一个客户的请求 
        private Thread tdListen;//定义一个线程，用于监听客户的连接请求 
        private ArrayList clients; //申名一个一维数组，用来存储连接到服务器的客户信息
        public delegate void GetlbClientCall(string id, GameClient ipn);//不能在线程启动后又启动Windows窗体线程，这样是不安全的
        //因此要建立一个委托
        public Server()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clients = new ArrayList();
            tdListen = new Thread(new ThreadStart(StartListening));
            tdListen.Start();
        }

        private void StartListening()
        {
            IPAddress localip = IPAddress.Parse("127.0.0.1");
            IPEndPoint localendpoint = new IPEndPoint(localip, port);
            listener = new TcpListener(localendpoint);
            listener.Start();
            while (true)
            {
                try
                {
                    clientsocket = listener.AcceptSocket();
                    clientservice = new Thread(new ThreadStart(ServiceClient));
                    clientservice.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        private void ServiceClient()
        {
            Socket client = clientsocket;
            bool keepalive = true; //各个客户Socket线程存活的标识   
            Hashtable duizhan = new Hashtable();
            while (keepalive)
            {
               try
               {
                   Byte[] LInfor = new Byte[1024];
                   int msglen = client.Receive(LInfor, LInfor.Length, 0);
                   Byte[] realDate = new Byte[msglen];
                   System.Buffer.BlockCopy(LInfor, 0, realDate, 0, msglen);

                   CSharpGame.Message clientMsg = (CSharpGame.Message)(CSharpGame.SerializationUnit.DeserializeObject(realDate));
               }
               catch (System.Exception ex)
               {
                   client.Close();
               }
            }
        }

        private string GetUserNameList()
        {
            string chatters = "";
            for (int n = 0; n < clients.Count; n++)
            {
                GameClient gc = (GameClient)clients[n];
                chatters += gc.Name;
                chatters += "|";
            }
            chatters.Trim(new char[] { '|' });
            return chatters;
        }

        private int findGameClient(string userName)
        {
            int index = 0;

            bool found = false;

            for (int i = 0; i < clients.Count; i++)
            {
                GameClient cl = (GameClient)clients[i];
                Console.WriteLine(cl.Name);
               
                if (cl.Name.CompareTo(userName) == 0)
                {
                    index = i;
                    found = true;
                }
            }
            if (found)
                return index;
            return -1;
        }
        public void SendToClient(GameClient cl, string clientCommand)
        {
            try
            {
                Byte[] message = System.Text.Encoding.BigEndianUnicode.GetBytes(clientCommand);
                Socket s = cl.Sock;
                if (s.Connected)
                {
                    s.Send(message, message.Length, 0);
                }
            }

            catch (Exception)//如果有异常则退出
            {
                //clients.Remove(cl);
                //lbClients.Items.Remove(cl);
                ////lbClients.Items.Remove(cl.Name + " : " + cl.Host.ToString());
                //for (int n = 0; n < clients.Count; n++)
                //{
                //    Client cl1 = (Client)clients[n];
                //    SendToClient(cl1, "GONE|" + cl.Name);
                //}
                //cl.Sock.Close();
                //cl.CLThread.Abort();

            }

        }   
    }
}
