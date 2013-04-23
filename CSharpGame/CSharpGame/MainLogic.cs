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
        //Logic myLogic;
        public int tableIdx;
        public int seatIdx;
        PlayerStatus myStatus;
        public string myClientName;

        // 其他玩家的Logic, 用于显示缩略图
        //List<Logic> otherPlayersLogic;

        public Hall hall;
        public Room gameRoom;
        public bool GameRoomIsWorking;

        public MainLogic()
        {
            myClientSoc = new MyClientSoc();
            keepalive = false;

            // 生成界面的form
            hall = new Hall(this);
            gameRoom = new Room(this);
            gameRoom.Draw();
            //gameRoom = CreateGameForm();
            GameRoomIsWorking = false;

            myStatus = PlayerStatus.OffLine;
            msgList = new List<Message>();
            thisLock = new Object();

            this.tableIdx = -1;
            this.seatIdx = -1;
        }

        private void InitGame(MsgSys msg)
        {
            //// 初始化所有玩家的状态
            //int[] gameStart = (int[])msg.sysContent;
            //StartPlayerShow(gameStart);
            //myLogic.Enable();
        }

        public void TestStart()
        {

        }

        //public void StartPlayerShow(int[] gameStatus)
        //{
        //    // 初始本地的
        //    myLogic.InitGame(gameStatus);
        //    //myLogic.startGame();
        //    // 其他玩家的
        //    foreach (Logic lg in otherPlayersLogic)
        //    {
        //        if (lg.myClientName != "")
        //        {
        //            lg.InitGame(gameStatus);
        //        }
        //    }
        //}

        //
        // 处理游戏的logic
        //

        // 提示功能
        //public void HintNext(object sender, EventArgs e)
        //{
        //    //myLogic.hintclicked();
        //}

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
                    // 如果登录不成功，主动结束socket
                    // 下次会重新连接。
                    myClientSoc.CloseConn();
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
                //myLogic.myClientName = user;
                this.myClientName = user;

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

                // 
                // 准备发送一个online的命令，服务器把当前所有的table的情况发回来。
                MsgSys s = new MsgSys();
                s.sysType = MsgSysType.Online;
                s.sysContent = this.myClientName;
                userSend(new Message(s));

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
                sysMsg.sysContent = this.myClientName;
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
                }
            }
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
                else
                {
                    ;
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
            //for (int i = 0; i < otherPlayersLogic.Count; i++)
            //{
            //    if (otherPlayersLogic[i].myClientName == gamMsg.userName && otherPlayersLogic[i].myClientName != null)
            //    {
            //        //更新指定玩家的界面
            //        otherPlayersLogic[i].CleanBtnPair(gamMsg.cleanPair[0],gamMsg.cleanPair[1]);
            //    }
            //}
            //if (gamMsg.tableIdx == this.tableIdx)
            //{
            gameRoom.PlayerCleanPair(gamMsg.seatIdx, gamMsg.cleanPair[0],
                                        gamMsg.cleanPair[1]);
            //}
        }

        private void ProcessSysMsg(Message _sysMsg)
        {
            MsgSys sysMsg = (MsgSys)_sysMsg.msgContent;
            switch (sysMsg.sysType)
            {
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
                        ;
                    }
                    break;
                case MsgSysType.Begin:
                    {
                        InitGame(sysMsg);
                    }
                    break;
                case MsgSysType.NewRound:
                    {
                        string userRefresh = (string)_sysMsg.userSender;
                        int[] gameStatus = (int[])sysMsg.sysContent;
                        gameRoom.RefreshGame(userRefresh, gameStatus);
                    }
                    break;
                case MsgSysType.Seat:
                    {
                        int[] seatInfo = (int[])sysMsg.sysContent;
                        string userSender = (string)_sysMsg.userSender;
                        //System.Console.WriteLine("seat {0} {1} {2}", userSender, seatInfo[0], seatInfo[1]);
                        hall.PlayerSeatDown(seatInfo[0], seatInfo[1], userSender);

                        // 如果当前桌子有人发来信息
                        // 则更新先进入者的信息
                        if (seatInfo[0] == this.seatIdx && userSender != this.myClientName)
                        {
                            gameRoom.PlayerEnterRoom(seatInfo[1], userSender);
                        }
                    }
                    break;
                    // 通知玩家有人进入某房间
                case MsgSysType.LeaveRoom:
                    {
                        int[] seatInfo = (int[])sysMsg.sysContent;
                        string userSender = (string)_sysMsg.userSender;
                        
                        hall.PlayerLeaveTable(seatInfo[0], seatInfo[1], userSender);
                        // 如果当前桌子有人发来信息
                        // 则更新先进入者的信息
                        if (seatInfo[0] == this.seatIdx)
                        {
                            gameRoom.PlayerLeaveRoom(seatInfo[1], userSender);
                        }
                    }
                    break;
                case MsgSysType.FreshGameArea:
                    {
                    }
                    break;
                    // 此消息是为了通知其他玩家，某个房间已经开始游戏，不能再坐下去了
                case MsgSysType.GameOn:
                    {
                        int tableIdx = (int)sysMsg.sysContent;
                        hall.TableGameOn(tableIdx);
                    }
                    break;
                // 此消息是为了通知其他玩家，某个房间已经结束游戏，可以继续参与了 
                case MsgSysType.GameOver:
                    {
                        int tableIdx = (int)sysMsg.sysContent;
                        hall.TableGameOver(tableIdx);

                        string user = _sysMsg.userSender;
                        if (tableIdx == this.tableIdx)
                        {
                            gameRoom.Win(user);
                        }
                    }
                    break;
            }
                   
        }

        public void SendGameData(MsgGame msg)
        {
            if (keepalive)
            {
                msg.tableIdx = this.tableIdx;
                msg.seatIdx = this.seatIdx;
                Message sendMsg = new Message(msg);
                sendMsg.reciType = ReciveType.Room;
                sendMsg.Num = this.tableIdx;
                userSend(sendMsg);
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
            msg.userSender = this.myClientName;
            myClientSoc.SendMsg(msg);
        }

        
        //
        //
        // 处理房间中的逻辑
        //
        //

        private void SendPlayerSitDown(int tableIdx, int seatIdx)
        {
            this.tableIdx = tableIdx;
            this.seatIdx = seatIdx;

            MsgSys seatMsg = new MsgSys();
            seatMsg.sysType = MsgSysType.Seat;
            seatMsg.sysContent = new int[] { tableIdx, seatIdx };
            Message msg = new Message(seatMsg);

            userSend(msg);
        }

        //
        // 玩家退出游戏房间
        //
        public void QuitGameArea()
        {
            // 模拟再次登录
            //SendPlayerSitDown(this.tableIdx, this.seatIdx);

            MsgSys seatMsg = new MsgSys();
            seatMsg.sysType = MsgSysType.LeaveRoom;
            seatMsg.sysContent = new int[] { tableIdx, seatIdx };
            Message msg = new Message(seatMsg);
            userSend(msg);

            gameRoom.PlayerLeaveRoom(this.seatIdx, this.myClientName);

            // 然后将当前的table信息清空
            this.tableIdx = -1;
            this.seatIdx = -1;
        }

        public delegate void showFun();
        private void showGameRoom(int tableIdx, int seatIdx)
        {
            gameTable tablesInfo = hall.tables[tableIdx];
            tablesInfo.seatUser[seatIdx] = this.myClientName;
            int mypos = seatIdx;
            // 玩家进入游戏房间时，将当前的所有的同桌人一起加入房间。
            // 第一个进入的人即本地玩家。
            for (int i = mypos; i < mypos + 4; i++)
            { 
                int k = i % 4;
                // 存在一个问题，自己的username可能传不过去了。
                if (tablesInfo.seatUser[k] != "")
                {
                    gameRoom.PlayerEnterRoom(i, tablesInfo.seatUser[k]);
                }
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

                SendPlayerSitDown(tableIdx, seatIdx);
                   
                // 这里为了简单起见，没有使用多线程，显示多界面了，每次只能有一个界面出现
                showGameRoom(tableIdx, seatIdx);

                gameRoom.Text = "连连看-房间" + System.Convert.ToString(tableIdx + 1);
                gameRoom.Text += "|" + this.myClientName;
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
