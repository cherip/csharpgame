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
        private Hashtable userandsocket;
        NetworkStream ns;
        private List<int[]> gameResetStatus;
        private List<TableInfo> tables;
        private static int total = 1;
        
        //因此要建立一个委托
        public Server()
        {
            InitializeComponent();
            button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            userandsocket = new Hashtable();
            clients = new ArrayList();
            readyUsers = new List<GameClient>();
            tables = new List<TableInfo>(9);
            for (int i = 0; i < tables.Capacity; i++)
                tables.Add(new TableInfo());

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
                    switch (clientMsg.msgType)
                    {
                        case MsgType.Sys:
                            {
                                ProcessSysMsg(clientMsg, client);
                            }
                            break;
                        case MsgType.Game:
                            {
                                ProcessGamMsg(clientMsg);
                            }
                            break;
                        case MsgType.Chat:
                            {

                            }
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    //用户意外掉线的处理，从hashtable中找到用户，然后在clients中干掉用户
                    string duanxian = (string)userandsocket[client];
                    if (duanxian != null)
                    {
                        userandsocket.Remove(client);

                        MsgSys sysBroadcast = new MsgSys();
                        sysBroadcast.sysType = MsgSysType.Exit;
                        sysBroadcast.sysContent = duanxian;
                        BroadcastClient(new CSharpGame.Message(sysBroadcast));
                        int remove = findGameClient((string)sysBroadcast.sysContent);
                        if (remove != -1)
                        {
                            clients.RemoveAt(remove);
                        }

                        client.Close();
                    }
                    
                    
                }
            }
        }

        private void ProcessGamMsg(CSharpGame.Message gamMsg)
        {
            // 指向同一房间内的玩家广播
            BroadcastRoom(gamMsg);

            // 服务器处理客户端的游戏情况
            MsgGame msgGame = (MsgGame)gamMsg.msgContent;
            int tabIdx = msgGame.tableIdx;
            int seatIdx = msgGame.seatIdx;

            int ret = tables[tabIdx].PlayerCleanPair(seatIdx);
            if (ret == 2)
            {
                // players win
                MsgSys sysGameOver = new MsgSys();
                sysGameOver.sysType = MsgSysType.GameOver;
                sysGameOver.sysContent = tabIdx;
                CSharpGame.Message m = new CSharpGame.Message(sysGameOver);
                m.userSender = msgGame.userName;
                BroadcastClient(m);

                // 一些房间的游戏信息清空
                tables[tabIdx].readycount = 0;
                //tables[tabIdx].usercount = 0;
                tables[tabIdx].tabelEable = false;
            }
            else if (ret == 1) 
            {
                // player 完成一幅牌
                MsgSys sysReset = new MsgSys();
                sysReset.sysType = MsgSysType.NewRound;
                sysReset.sysContent = tables[tabIdx].GetGameForPlayer(seatIdx);
                CSharpGame.Message m = new CSharpGame.Message(sysReset);
                m.userSender = msgGame.userName;
                m.Num = tabIdx;
                m.reciType = ReciveType.Room;
                BroadcastRoom(m);
                //SentToUser(m, msgGame.userName);
            }
        }

        private void ProcessSysMsg(CSharpGame.Message _sysMsg, Socket client)
        {
            MsgSys sysMsg = (MsgSys)_sysMsg.msgContent;
            string curr_user = _sysMsg.userSender;
            
            switch (sysMsg.sysType)
            {
                case MsgSysType.Login:
                    {
                        MsgSys s = new MsgSys();
                        s.sysType = MsgSysType.Judge;
                        string[] user_pwd = (string[])sysMsg.sysContent; 

                        
                        if (user_pwd == null || user_pwd.Length != 2 ||
                            JudgeUserLogin(user_pwd[0], user_pwd[1]) == false)
                        {
                            s.sysContent = false; 
                            GameClient newGC = new GameClient(user_pwd[0], null, clientservice, client);
                            SendToClient(newGC, new CSharpGame.Message(s));
                        }
                        else if (findGameClient(user_pwd[0]) != -1)//重名登录
                                              
                        {
                            
                            s.sysContent = false;
                            GameClient newGC = new GameClient(user_pwd[0], null, clientservice, client);
                           
                            SendToClient(newGC, new CSharpGame.Message(s));
                        }
                        else
                        {
                            s.sysContent = true;
                            GameClient newGC = new GameClient(user_pwd[0], null, clientservice, client);
                            clients.Add(newGC);
                            userandsocket[client] = user_pwd[0];
                            SendToClient(newGC, new CSharpGame.Message(s));
                        }
                    }
                    break;
                case MsgSysType.Online:
                    {

                        GameClient newGC = new GameClient((string)sysMsg.sysContent, null, clientservice, client);
                        for (int i = 0; i < tables.Count; i++)
                        {
                            if (tables[i].tabelEable)
                            {
                                MsgSys sysGameOn = new MsgSys();
                                sysGameOn.sysType = MsgSysType.GameOn;
                                sysGameOn.sysContent = i;
                                SendToClient(newGC, new CSharpGame.Message(sysGameOn));
                            }
                        }

                        
                        if (clients.Count != 0)
                        {
                            // 写个 广播函数 
                            // 因为会有很多 广播操作。。
                            MsgSys sysBroadcast = new MsgSys();
                            sysBroadcast.sysType = MsgSysType.Join;
                            sysBroadcast.sysContent = sysMsg.sysContent;
                            BroadcastClient(new CSharpGame.Message(sysBroadcast));
                           
                            Thread.Sleep(100);
                            MsgSys sendback = new MsgSys();
                            sendback.sysType = MsgSysType.List;
                            List<string> userList = GetUserNameList();
                            sendback.sysContent = userList;
                            SendToClient(newGC, new CSharpGame.Message(sendback));
                        }

                        


                        sendCurrentTables((string)sysMsg.sysContent);
                      
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
                        //客户端还要传桌子号过来
                        int find = findGameClient(curr_user);
                        GameClient gl = (GameClient)clients[find];
                        total = (int)sysMsg.sysContent;
                        //int tableIndex = (int)sysMsg.sysContent;
                        //gl.TableIdx = tableIndex;
                        int tableIndex = gl.TableIdx;
                        tables[tableIndex].readycount++;
                        if (tables[tableIndex].readycount == tables[tableIndex].usercount)
                        {
                            //首先发送消息通知客户端，某房间游戏已经开始
                            MsgSys sysGameOn = new MsgSys();
                            sysGameOn.sysType = MsgSysType.GameOn;
                            sysGameOn.sysContent = tableIndex;
                            BroadcastClient(new CSharpGame.Message(sysGameOn));

                            
                            // 完成牌数的功能 在InitGame前 设置tableInfo中的totalRound
                            tables[tableIndex].totalRound = total;
                            ;

                            //发送开始
                            InitTableGame(tableIndex);
                            MsgSys sysBroadcast = new MsgSys();
                            sysBroadcast.sysType = MsgSysType.Begin;
                            //sysBroadcast.sysContent = //gameResetStatus[0];
                            sysBroadcast.sysContent = tables[tableIndex].gameResetStatus[0];
                            CSharpGame.Message m = new CSharpGame.Message(sysBroadcast);
                            m.Num = tableIndex;
                            BroadcastRoom(m);
                        }
                    }
                    break;
                case MsgSysType.GameStart:
                    {
                        //服务器生成初始数据，图片数组，副数广播

                        InitGameStatus();
                        //MsgSys sysBegin = new MsgSys();
                        //sysBegin.sysType = MsgSysType.Begin;
                        //sysBegin.sysContent = gameResetStatus[0];
                        //SendToClient(newGC, new CSharpGame.Message(sysBegin));
                        MsgSys sysBroadcast = new MsgSys();
                        sysBroadcast.sysType = MsgSysType.Begin;
                        sysBroadcast.sysContent = gameResetStatus[0];
                        BroadcastClient(new CSharpGame.Message(sysBroadcast));
                    }
                    break;
                case MsgSysType.Seat:
                    {
                        // 
                        // 首先更新当前发生者gc的状态
                        //
                        int[] temp = (int[])sysMsg.sysContent;
                        tables[temp[0]].usercount++;
                        string userSend = (string)_sysMsg.userSender;
                        foreach (GameClient gc in clients)
                        {
                            if (gc.Name == userSend)
                            {
                                gc.TableIdx = temp[0];
                                gc.SeatIdx = temp[1];
                            }
                        }

                        // 把消息转发给所有人 通知客户端更新seat的信息
                        BroadcastClient(_sysMsg);
                    }
                    break;
                case MsgSysType.LeaveRoom:
                    {
                        // 
                        // 首先更新当前发生者gc的状态
                        //
                        int[] temp = (int[])sysMsg.sysContent;
                        if (tables[temp[0]].usercount != 0)
                        {
                            tables[temp[0]].usercount--;
                        }
                       
                        string userSend = (string)_sysMsg.userSender;
                        foreach (GameClient gc in clients)
                        {
                            if (gc.Name == userSend)
                            {
                                gc.TableIdx = -1;
                            }
                        }

                        // 把消息转发给所有人 通知客户端更新seat的信息
                        BroadcastClient(_sysMsg);
                        //if (tables[temp[0]].tabelEable)
                        //{
                        //    tables[temp[0]].readycount--;
                        //}
                        if (tables[temp[0]].readycount != 0)
                        {
                            tables[temp[0]].readycount--;
                        }
                        
                        // 如果是在游戏中退出的，且如果是该游戏的最后一名玩家，
                        // 则通知所有人该房间游戏结束
                        if (tables[temp[0]].readycount == 0 && tables[temp[0]].tabelEable)
                        {
                            MsgSys sysGameOver = new MsgSys();
                            sysGameOver.sysType = MsgSysType.GameOver;
                            sysGameOver.sysContent = temp[0];
                            CSharpGame.Message m = new CSharpGame.Message(sysGameOver);
                            m.userSender = "Server";
                            BroadcastClient(m);

                            tables[temp[0]].tabelEable = false;
                        }
                    }
                    break;
                case MsgSysType.TableNoSeat:
                    {
                        ;
                    }
                    break;

                case MsgSysType.FreshGameArea:
                    {
                        BroadcastRoom(_sysMsg);
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


        private void SentToUser(CSharpGame.Message msg, string user)
        {
            int find = findGameClient(user);
            GameClient gl = (GameClient)clients[find];

            SendToClient(gl, msg);
        }

        private void BroadcastClient(CSharpGame.Message msg)
        {
            foreach (GameClient cl in clients)
            {
                SendToClient(cl, msg);
            }
        }

        private void BroadcastRoom(CSharpGame.Message msg)
        {
            foreach (GameClient cl in clients)
            {
                if (cl.TableIdx == (int)msg.Num)
                {
                    SendToClient(cl, msg);
                }
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
                    System.Console.WriteLine("send seat to {0}, {1}", cl.Name, (string)mes.userSender);
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

        // 判断用户登录
        private bool JudgeUserLogin(string login, string pwd)
        {

            Console.WriteLine("user: {0} login error!", login);
            //return MyFile.Judge(login, pwd);
            return true;//测试用

         

        }

        private void sendCurrentTables(string username)
        {
            GameClient toUser = null;
            foreach (GameClient gc in clients)
            {
                if (gc.Name == username)
                {
                    toUser = gc;
                    break;
                }
            }
            if (toUser == null) return;

            foreach (GameClient gcc in clients)
            {
                if (gcc.Name == username) continue;
                if (gcc.TableIdx != -1)
                {
                    MsgSys sysMsg = new MsgSys();
                    sysMsg.sysType = MsgSysType.Seat;
                    sysMsg.sysContent = new int[] { gcc.TableIdx, gcc.SeatIdx };
                    CSharpGame.Message msg = new CSharpGame.Message(sysMsg);
                    msg.userSender = gcc.Name;

                    SendToClient(toUser, msg);
                    //BroadcastClient(msg);
                }
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

        public void InitTableGame(int tableIdx)
        {
            //gameResetStatus = new List<int[]>();
            tables[tableIdx].gameResetStatus = new List<int[]>();
            tables[tableIdx].gameAnswer = new List<int>();
            for (int i = 0; i < tables[tableIdx].totalRound; i++)
            {
                int[] game = new int[64];
                MyFormat.genPic(ref game);
                tables[tableIdx].gameResetStatus.Add((int[])game.Clone());
                tables[tableIdx].gameAnswer.Add(MyFormat.countPairPic(game));
            }
            tables[tableIdx].GameStart();
            tables[tableIdx].tabelEable = true;
        }
    }
}
