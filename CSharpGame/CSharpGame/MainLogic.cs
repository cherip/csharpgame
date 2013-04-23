using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace CSharpGame
{
    // 这里简单的用enum表示玩家的状态，
    // 要更复杂的话应该用 一个专门的class来表示玩家的状态
    // 应该放入到Logic内才对 ，这里简单搞搞了
    public enum PlayerStatus 
    {
        OnLine = 1,         // 已上线
        OffLine,            // 下线
        OnTable,            // 已经在桌子上做下了
        OnGame              // 已经开始游戏了
    }

    public class MainLogic
    {
        // 网络通信的提供者
        MyClientSoc myClientSoc;

        // 接受服务器消息，会改变界面的
        private Thread receiveThread;
        private Thread msgProcessor;
        List<Message> msgList;
        private Object thisLock;

        // 保持连接？
        public bool keepalive = true;

        // 当前client的玩家的Logic
        Logic myLogic;
        public int tableIdx;
        public int seatIdx;
        PlayerStatus myStatus;

        // 其他玩家的Logic, 用于显示缩略图
        List<Logic> otherPlayersLogic;

        public Room hall;
        public CSharpGame gameRoom;
        public bool GameRoomIsWorking;

        public MainLogic()
        {
            myClientSoc = new MyClientSoc();
            keepalive = false;

            // 生成界面的form
            hall = new Room(this);
            gameRoom = CreateGameForm();
            GameRoomIsWorking = false;

            myStatus = PlayerStatus.OffLine;
            msgList = new List<Message>();
            thisLock = new Object();
        }

        private CSharpGame CreateGameForm()
        {
            CSharpGame gameForm = new CSharpGame(this);

            // 首先创建4个logic
            myLogic = new Logic(1);
            myLogic.sendMsgEvent += SendGameData;

            otherPlayersLogic = new List<Logic>();
            for (int i = 0; i < 3; i++)
            {
                otherPlayersLogic.Add(new Logic(2));
            }

            // 将Logic的界面显示到gameForm中
            GameArea ga = myLogic.gameArea;
            gameForm.CreateMainArea(ga);
            List<GameArea> ret = new List<GameArea>();
            foreach (Logic lg in otherPlayersLogic)
            {
                ret.Add(lg.gameArea);
                lg.gameArea.Hide();
            }
            gameForm.CreateOppeArea(ret);

            return gameForm;
        }

        private void InitGame(MsgSys msg)
        {
            // 初始化所有玩家的状态
            int[] gameStart = (int[])msg.sysContent;
            StartPlayerShow(gameStart);
            myLogic.Enable();
        }

        public void TestStart()
        {
            int[] tester = new int[64];
            MyFormat.genPic(ref tester);
            // 初始本地的
            myLogic.InitGame(tester);
            //myLogic.startGame();
            // 其他玩家的
            foreach (Logic lg in otherPlayersLogic)
            {
                lg.InitGame(tester);
            }
            myLogic.Enable();
        }

        public void StartPlayerShow(int[] gameStatus)
        {
            // 初始本地的
            myLogic.InitGame(gameStatus);
            //myLogic.startGame();
            // 其他玩家的
            foreach (Logic lg in otherPlayersLogic)
            {
                if (lg.myClientName != "")
                {
                    lg.InitGame(gameStatus);
                }
            }
        }

        //
        // 处理游戏的logic
        //

        // 提示功能
        public void HintNext(object sender, EventArgs e)
        {
            myLogic.hintclicked();
        }

        //
        // 网络通信的功能
        //

        public bool ConnectNet(Message msg)
        {
            if (myClientSoc.connected == false)
            {
                myClientSoc.InitialSoc();
                // 如果网络连接失败
                if (myClientSoc.connected == false)
                {
                    return false;
                }
            }

            userSend(msg);
            Message serverMsg = myClientSoc.RecieveMsg();
            if (serverMsg.msgType == MsgType.Sys)
            {
                MsgSys loginMsg = (MsgSys)serverMsg.msgContent;
                if (loginMsg.sysType == MsgSysType.Judge)
                {

                    bool ret = (bool)loginMsg.sysContent;
                    // 登录失败 直接返回
                    if (ret != true)
                    {
                        MessageBox.Show("没有这个用户");
                        return false;
                    }

                    // 登录成功
                    keepalive = true;
                    myStatus = PlayerStatus.OnLine;
                }
                else
                {
                    return false;
                }
            }

            this.myStatus = PlayerStatus.OnLine;
            return true;
        }

        // 加密函数
        private string EncodePwd(string pwd)
        {
            return pwd;
        }

        public bool PlayerLogin(string user, string pwd)
        {
            // 现在do nothing，任何登录都能成功
            // ConnectNet() always return true;

            MsgSys sysMsg = new MsgSys();
            sysMsg.sysType = MsgSysType.Login;
            string[] user_pwd = new string[2] { user, EncodePwd(pwd) };
            sysMsg.sysContent = user_pwd;

            if (ConnectNet(new Message(sysMsg)))
            {
                myLogic.myClientName = user;

                // 登录成功
                keepalive = true;
                //启动后台线程接受服务器端发送的消息
                if (receiveThread == null)
                {
                    receiveThread = new Thread(new ThreadStart(NetRuning));
                    receiveThread.Start();

                    msgProcessor = new Thread(new ThreadStart(MsgProcessor));
                    msgProcessor.Start();
                }

                // 这里有 bug
                // 准备发送一个online的命令，服务器把当前所有的table的情况发回来。
                MsgSys s = new MsgSys();
                s.sysType = MsgSysType.Online;
                s.sysContent = myLogic.myClientName;
                Message conn = new Message(s);
                conn.userSender = myLogic.myClientName;
                myClientSoc.SendMsg(conn);

                return true;
            }
            return false;
        }

        public void CloseConn(string msg)
        {
            if (keepalive)
            {
                MsgSys sysMsg = new MsgSys();
                sysMsg.sysType = MsgSysType.Offline;
                sysMsg.sysContent = myLogic.myClientName;
                userSend(new Message(sysMsg));
                receiveThread = null;
                keepalive = false;
            }

        }

        public void NetRuning()
        {
            while (keepalive)
            {
                // 处理网络连接
                if (myClientSoc.connected)
                {
                    Message serverMsg = myClientSoc.RecieveMsg();
                    lock (thisLock)
                    {
                        this.msgList.Add(serverMsg);
                    }
                    //Thread cmdThread = new Thread(new ThreadStart(processMsg));
                    //processMsg(serverMsg);
                }
            }

            //  keepalive = false;
            myClientSoc.CloseConn();
        }

        public void MsgProcessor()
        {
            while (true)
            {
                if (this.msgList.Count != 0)
                {
                    Message msg;
                    lock (thisLock)
                    {
                        msg = this.msgList[0];
                        this.msgList.RemoveAt(0);
                    }

                    processMsg(msg);
                }
            }
        }

        //
        // 判断接收的数据，如果是退出消息且是本客户端发送的退出消息，
        // 则返回true，表示应该结束这个接收线程
        //
        public bool processMsg(Message msg)
        {
            // 这里客户端的接受服务器消息主要逻辑，
            // 为从服务器端发生的消息作出各种反应
            switch (msg.msgType)
            {
                case MsgType.Sys:
                    {
                        //MsgSys sysMsg = (MsgSys)msg.msgContent;

                        ProcessSysMsg(msg);
                    }
                    break;
                case MsgType.Game:
                    {
                        MsgGame gamMsg = (MsgGame)msg.msgContent;
                        ProcessGamMsg(gamMsg);
                    }
                    break;
            }
            return true;
        }

        private void ProcessGamMsg(MsgGame gamMsg)
        {
            //获得其他玩家数据，根据username更新界面
            for (int i = 0; i < otherPlayersLogic.Count; i++)
            {
                if (otherPlayersLogic[i].myClientName == gamMsg.userName && otherPlayersLogic[i].myClientName != null)
                {
                    //更新指定玩家的界面
                    otherPlayersLogic[i].CleanBtnPair(gamMsg.cleanPair[0],gamMsg.cleanPair[1]);
                }
            }
        }

        private void ProcessSysMsg(Message _sysMsg)
        {
            MsgSys sysMsg = (MsgSys)_sysMsg.msgContent;
            switch (sysMsg.sysType)
            {
                case MsgSysType.Judge:
                    {

                        bool judged = (bool)sysMsg.sysContent;
                        if (!judged)
                        {
                            MessageBox.Show("没有这个用户");
                        }
                        if (judged)
                        {
                            myStatus = PlayerStatus.OnLine;
                           // myLogic.myClientName = judged[1];
                            MsgSys s = new MsgSys();
                            s.sysType = MsgSysType.Online;
                           
                            s.sysContent = myLogic.myClientName;

                            Message conn = new Message(s);
                            userSend(conn);
                        }
                    }
                    break;
                case MsgSysType.Join:
                    {
                        //某玩家加入
                        string userName = (string)sysMsg.sysContent;
                       // Logic lg = new Logic(2);
                      //  lg.myClientName = userName;
                       // otherPlayersLogic.Add(new Logic(2));
                    }
                    break;
                case MsgSysType.List:
                    {
                        //在线玩家列表
                        List<string> userList = (List<string>)sysMsg.sysContent;
                    }
                    break;
                case MsgSysType.Exit:
                    {
                        //某玩家退出
                        string userName = (string)sysMsg.sysContent;
                    }
                    break;
                case MsgSysType.CanStart:
                    {
                        //通知 全部准备好
                        List<string> userList = (List<string>)sysMsg.sysContent;//所有准备好的玩家的名字
                        for (int i = 0; i < userList.Count; i++)
                        {
                            if (userList[i] != myLogic.myClientName)
                            {
                                Logic lg = new Logic(2);
                                lg.myClientName = userList[i];
                                otherPlayersLogic.Add(new Logic(2));
                            }
                        }
                        if (userList[0] == myLogic.myClientName)
                        {
                            MsgSys msgStart = new MsgSys();
                            msgStart.sysType = MsgSysType.GameStart;
                            msgStart.sysContent = null;
                         }
                    }
                    break;
                case MsgSysType.Begin:
                    {
                        InitGame(sysMsg);
                    }
                    break;
                case MsgSysType.Seat:
                    {
                        int[] seatInfo = (int[])sysMsg.sysContent;
                        string userSender = (string)_sysMsg.userSender;
                        System.Console.WriteLine("seat {0} {1} {2}", userSender, seatInfo[0], seatInfo[1]);
                        hall.PlayerSeatDown(seatInfo[0], seatInfo[1], userSender);

                      
                        // 如果seat信息的发送者不是服务器 或者 自己
                        // 则改变gamearea的界面，表示有人上线或者下线。
                        if (userSender != "Server" && userSender != myLogic.myClientName &&
                            seatInfo[0] == this.tableIdx)
                        {
                            int mypos = this.seatIdx;
                            int t = 0;
                            gameTable tablesInfo = hall.tables[tableIdx];
                            for (int i = mypos + 1; i < mypos + 4; i++)
                            {
                                int k = i % 4;
                                if (tablesInfo.seatUser[k] != "" &&
                                    tablesInfo.seatUser[k] != "Server")
                                {
                                    otherPlayersLogic[t].SetPlayer(tablesInfo.seatUser[k]);
                                    GameArea ga = otherPlayersLogic[t].gameArea;
                                    //ga.Show();
                                    ga.Invoke(new showFun(ga.Show));
                                }
                                t++;
                            }
                        }
                    }
                    break;
                case MsgSysType.FreshGameArea:
                    {
                        //int[] tableInfo = (int[])sysMsg.sysContent;
                        //if (tableInfo[0] == this.tableIdx)
                        //{
                       
                        //}
                    }
                    break;
            }
                   
        }

        public void SendGameData(Message msg)
        {
            if (keepalive)
            {
                msg.reciType = ReciveType.Room;
                msg.Num = this.tableIdx;
                userSend(msg);
            }
        }

        public void UserReady(int total)
        {
            if (keepalive)
            {
                MsgSys sysMsg = new MsgSys();
                sysMsg.sysType = MsgSysType.Ready;
                sysMsg.sysContent = tableIdx;
                Message conn = new Message(sysMsg);

                userSend(conn);
            }
        }

        public void StartGame()
        {
            if (keepalive)
            {
                MsgSys sysMsg = new MsgSys();
                sysMsg.sysType = MsgSysType.GameStart;
                sysMsg.sysContent = null;
                Message conn = new Message(sysMsg);
                userSend(conn);
            }
        }

        public void userSend(Message msg)
        {      
            msg.userSender = myLogic.myClientName;
            myClientSoc.SendMsg(msg);
        }

        
        //
        //
        // 处理房间中的逻辑
        //
        //
        public delegate void showFun();
        
        private void showGameRoom(int tableIdx, int seatIdx)
        {
            gameTable tablesInfo = hall.tables[tableIdx];
            
            int mypos = seatIdx;
            //foreach (string item in tablesInfo.seatUser)
            //{
            //    if (item == myLogic.myClientName)
            //        break;
            //    mypos++;
            //}

            int t = 0;
            for (int i = mypos + 1; i < mypos + 4; i++)
            { 
                int k = i % 4;
                if (tablesInfo.seatUser[k] != "" &&
                    tablesInfo.seatUser[k] != "Server")
                {
                    otherPlayersLogic[t].SetPlayer(tablesInfo.seatUser[k]);
                    GameArea ga = otherPlayersLogic[t].gameArea;
                    ga.Show();
                    //ga.Invoke(new showFun(ga.Show));
                }
                t++;
            }
        }

        public void ClickSeat(int tableIdx, int seatIdx)
        {
            // do nothing now...
            if (myStatus == PlayerStatus.OnLine)
            {
                // 
                // 添加一种网络消息，表示玩家做下了某个桌子
                // 其他玩家更加该消息，更新table的状态
                //
                // sendNetMsg(thisplayer on table)

                //gameRoom = CreateGameForm();
                hall.Hide();
                myStatus = PlayerStatus.OnTable;

                this.tableIdx = tableIdx;
                this.seatIdx = seatIdx;

                MsgSys seatMsg = new MsgSys();
                seatMsg.sysType = MsgSysType.Seat;
                seatMsg.sysContent = new int[] { tableIdx, seatIdx };
                Message msg = new Message(seatMsg);

                userSend(msg);

                // 这里为了简单起见，没有使用多线程，显示多界面了，每次只能有一个界面出现
                showGameRoom(tableIdx, seatIdx);

                GameRoomIsWorking = true;
                gameRoom.ShowDialog();
                GameRoomIsWorking = false;
//                showGameRoom(tableIdx);
                
                myStatus = PlayerStatus.OnLine;
                hall.Show();

                

                
            }
        }
    }
}
