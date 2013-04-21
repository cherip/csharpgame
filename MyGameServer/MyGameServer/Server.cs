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
using CSharpGame;

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
        private List<GameClient> readyUsers;
        public delegate void GetlbClientCall(string id, GameClient ipn);//不能在线程启动后又启动Windows窗体线程，这样是不安全的
        NetworkStream ns;
        private List<int[]> gameResetStatus;
        
        //因此要建立一个委托
        public Server()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clients = new ArrayList();
            readyUsers = new List<GameClient>();
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
                   
                   string clientcommand = System.Text.Encoding.BigEndianUnicode.GetString(LInfor);
                   string[] tokens = clientcommand.Split(new Char[] { '|' });
                   
                   //
                   // 把msg的封装改了一下。这么用吧 简洁一些。
                   // 可以把后面的逻辑用函数提出去。 每种msg用一个函数处理，否则这一坨的代码太多了。
                   //
                   //if (clientMsg.msgType == CSharpGame.MsgType.Sys)
                   //{
                    switch (clientMsg.msgType)
                    {
                        case MsgType.Sys:
                            {
                                MsgSys sysMsg = (MsgSys)clientMsg.msgContent;
                                ProcessSysMsg(sysMsg, client);
                            }
                            break;
                        case MsgType.Game:
                            {
                                MsgGame gamMsg = (MsgGame)clientMsg.msgContent;
                                ProcessGamMsg(gamMsg);
                            }
                            break;
                        case MsgType.Chat:
                            {
                                
                            }
                            break;
                    }
                   switch(tokens[0])
                   {
                       case "login":
                           {
                               if (clients.Count != 0)
                               {
                                   for (int n = 0; n < clients.Count; n++)//将新用户的加入信息发送给其他用户
                                   {
                                       GameClient cl = (GameClient)clients[n];
                                       Console.WriteLine(cl.Name + "xxxjion|" + tokens[1]);
                                       SendToClient(cl, "xxxjion|" + tokens[1] + '|');
                                   }
                               }
                               GameClient newGC = new GameClient(tokens[1], null, clientservice, client);
                               clients.Add(newGC);
                               Console.WriteLine(newGC.Name + "list|" + GetUserNameList());
                               SendToClient(newGC, "list|" + GetUserNameList());
                               break;
                           }
                       case "exit":
                           {
                               //int remove = 0;

                               //bool found = false;

                               //for (int i = 0; i < clients.Count; i++)
                               //{
                               //    GameClient cl = (GameClient)clients[i];
                               //    Console.WriteLine(cl.Name + "xxxexit|" + tokens[1]);
                               //    SendToClient(cl, "xxxexit|" + tokens[1] + '|');
                               //    if (cl.Name.CompareTo(tokens[1]) == 0)
                               //    {
                               //        remove = i;
                               //        found = true;
                               //    }
                               //}
                               //if (found)
                               //    clients.RemoveAt(remove);
                               int remove = findGameClient(tokens[1]);
                               if (remove != -1)
                               {
                                   clients.RemoveAt(remove);
                               }
                               client.Close();
                               keepalive = false; 
                               break;
                           }
                       case "invite":
                           {
                               int answer = findGameClient(tokens[2]);
                               if (answer != -1)
                               {
                                   SendToClient((GameClient)clients[answer], "xxxinvite|" + tokens[1]);
                               }
                               break;
                           }
                       case "startgame":
                           {
                               int[] picArray = new int[64];
                               MyFormat.genPic(ref picArray);
                               string str = MyFormat.arrayToStr(picArray);
                               foreach (GameClient cl in clients)
                               {
                                   SendToClient(cl, "xxxstartgame|" + str);
                               }
                               break;
                           }
                       case "gamedata":
                           {
                               foreach (GameClient cl in clients)
                               {
                                   if (true)//!cl.Name.Equals(tokens[1]))
                                   {
                                       SendToClient(cl, "xxxgamedata|" + tokens[2]);
                                   }
                               }
                               break;
                           }
                   }

                   

               }
               catch (System.Exception ex)
               {
                   client.Close();
               }
            }
        }

        private void ProcessGamMsg(MsgGame gamMsg)
        {
            //广播玩家数据
            
            BroadcastClient(new CSharpGame.Message(gamMsg));
        }

        private void ProcessSysMsg(MsgSys sysMsg, Socket client)
        {
            switch (sysMsg.sysType)
            {
                case MsgSysType.Online:
                    {
                        if (clients.Count != 0)
                        {
                            // 写个 广播函数 
                            // 因为会有很多 广播操作。。
                            MsgSys sysBroadcast = new MsgSys();
                            sysBroadcast.sysType = MsgSysType.Join;
                            sysBroadcast.sysContent = sysMsg.sysContent;
                            BroadcastClient(new CSharpGame.Message(sysBroadcast));
                        }

                        GameClient newGC = new GameClient((string)sysMsg.sysContent, null, clientservice, client);
                        clients.Add(newGC);

                        CSharpGame.MsgSys sysSend2 = new CSharpGame.MsgSys();
                        sysSend2.sysType = CSharpGame.MsgSysType.List;
                        // 这里改了一下 list消息的content是个用户名的list
                        sysSend2.sysContent = GetUserNameList();
                        SendToClient(newGC, new CSharpGame.Message(sysSend2));

                        InitGameStatus();
                        MsgSys sysBegin = new MsgSys();
                        sysBegin.sysType = MsgSysType.Begin;
                        sysBegin.sysContent = gameResetStatus[0];
                        SendToClient(newGC, new CSharpGame.Message(sysBegin));
                    }
                    break;
                case MsgSysType.Offline:
                    {
                        MsgSys sysBroadcast = new MsgSys();
                        sysBroadcast.sysType = MsgSysType.Exit;
                        sysBroadcast.sysContent = sysMsg.sysContent;
                        BroadcastClient(new CSharpGame.Message(sysBroadcast));
                        int remove = findGameClient((string)sysMsg.sysContent);
                        if (remove != -1)
                        {
                            clients.RemoveAt(remove);
                        }
                        client.Close();
                    }
                    break;
                case MsgSysType.Ready:
                    {
                        int find = findGameClient((string)sysMsg.sysContent);
                        if (find != -1)
                        {
                            readyUsers.Add((GameClient)clients[find]);
                        }
                        if (readyUsers.Count == clients.Count)//全部准备
                        {
                            //等待房主确认开始
                            MsgSys sysBroadcast = new MsgSys();
                            sysBroadcast.sysType = MsgSysType.CanStart;
                            sysBroadcast.sysContent = GetUserNameList();//把所有玩家名字发给用户
                            BroadcastClient(new CSharpGame.Message(sysBroadcast));
                        }
                    }
                    break;
                case MsgSysType.GameStart:
                    {
                        //服务器生成初始数据，图片数组，副数广播
                        MsgSys sysBroadcast = new MsgSys();
                        sysBroadcast.sysType = MsgSysType.Begin;
                        sysBroadcast.sysContent = null;//把所有玩家名字发给用户
                        BroadcastClient(new CSharpGame.Message(sysBroadcast));
                    }
                    break;

            }

         
        }

        //
        //
        // 这里改了， name要list
        //
        private List<string> GetUserNameList()
        {
            List<string> chatters = new List<string>();
            for (int n = 0; n < clients.Count; n++)
            {
                GameClient gc = (GameClient)clients[n];
                chatters.Add(gc.Name);
            }
 
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

        private void BroadcastClient(CSharpGame.Message msg)
        {
            foreach (GameClient cl in clients)
            {
                SendToClient(cl, msg);
            }
        }

        public bool SendToClient(GameClient cl, CSharpGame.Message mes)
        {
            try
            {
                Byte[] outbytes = CSharpGame.SerializationUnit.SerializeObject(mes);
                Socket s = cl.Sock;

                if (s.Connected)
                {
                    s.Send(outbytes, outbytes.Length, 0);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
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

        public void InitGameStatus()
        {
            gameResetStatus = new List<int[]>();
            for (int i = 0; i < 3; i++)
            {
                int[] game = new int[64];
                MyFormat.genPic(ref game);
                gameResetStatus.Add((int[])game.Clone());
            }
        }
    }
}
