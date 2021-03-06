﻿using System;
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
    // 用enum表示玩家的状态，
       
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

        // 保持连接
        public bool keepalive = true;

        // 当前client的玩家的Logic
        Logic myLogic;
        public int tableIdx;
        public int seatIdx;
        PlayerStatus myStatus;

        // 其他玩家的Logic, 用于显示缩略图
        List<Logic> otherPlayersLogic;

        public Room hall;//游戏大厅
        public CSharpGame gameRoom;//游戏界面
        public bool GameRoomIsWorking;//游戏大厅桌子是否已经开始玩



        public MainLogic()
        {
            myClientSoc = new MyClientSoc();
            keepalive = false;
            //初始化数据
            // 生成界面的form
            hall = new Room(this);
            gameRoom = CreateGameForm();
            GameRoomIsWorking = false;

            
            myStatus = PlayerStatus.OffLine;
            msgList = new List<Message>();
            thisLock = new Object();

            hall.btnClickEvent += ExitHall;
            this.tableIdx = -1;
            this.seatIdx = -1;
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
            // 游戏开始前隐藏btn
            ga.UnGameStatus();
            List<GameArea> ret = new List<GameArea>();
            foreach (Logic lg in otherPlayersLogic)
            {
                ret.Add(lg.gameArea);
                lg.gameArea.Hide();
            }
            gameForm.CreateOppeArea(ret);
            foreach (Logic lg in otherPlayersLogic)
            {
                lg.gameArea.UnGameStatus();
            }

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
            //int[] tester = new int[64];
            //MyFormat.genPic(ref tester);
            //// 初始本地的
            //myLogic.InitGame(tester);
            ////myLogic.startGame();
            //// 其他玩家的
            //foreach (Logic lg in otherPlayersLogic)
            //{
            //    lg.InitGame(tester);
            //}
            //myLogic.Enable();
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
        public void ExitHall(object sender, EventArgs e)
        {
            if (myLogic != null && myClientSoc != null && myLogic.myClientName != null)
            {
                CloseConn();
                Environment.Exit(0);
            }
        }

        public void HintNext(object sender, EventArgs e)
        {
            myLogic.hintclicked();
        }

        public void SameBtn(object sender, EventArgs e)
        {
            myLogic.showSameList();
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
                        MessageBox.Show("登录失败！");
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

                // 
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

        public void CloseConn()
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
            try
            {
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
                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("服务器没响应，休息一会吧~亲~");
                Environment.Exit(0);
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
                case MsgSysType.Judge://登录请求响应
                    {

                        bool judged = (bool)sysMsg.sysContent;
                        if (!judged)
                        {
                            MessageBox.Show("登录失败！");
                            myClientSoc.CloseConn();
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
                case MsgSysType.Join://有玩家上线
                    {
                        //某玩家加入，大厅中在线列表更新
                        string userName = (string)sysMsg.sysContent;
                        if (!userName.Equals(myLogic.myClientName))
                        {
                            hall.AddOnePlayers(userName);
                        }
                        
                       // Logic lg = new Logic(2);
                      //  lg.myClientName = userName;
                       // otherPlayersLogic.Add(new Logic(2));
                    }
                    break;
                case MsgSysType.List://在线玩家列表
                    {
                        //在线玩家列表
                        List<string> userList = (List<string>)sysMsg.sysContent;
                        hall.AddPlayers(userList);
                    }
                    break;
                case MsgSysType.Exit:
                    {
                        //某玩家退出
                        string userName = (string)sysMsg.sysContent;
                        //MessageBox.Show("玩家-" + userName + "-退出！");
                        hall.RemovePlayers(userName);
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
                        InitGame(sysMsg);   //游戏开始
                    }
                    break;
                case MsgSysType.NewRound://完成一幅新的一轮开始
                    {
                        string userRefresh = (string)_sysMsg.userSender;
                        int[] gameStatus = (int[])sysMsg.sysContent;
                        if (userRefresh == myLogic.myClientName)
                        {
                            myLogic.InitGame(gameStatus);
                        }
                        else
                        {
                            foreach (Logic lg in otherPlayersLogic)
                            {
                                if (lg.myClientName == userRefresh)
                                {
                                    lg.InitGame(gameStatus);
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case MsgSysType.Seat://游戏座位响应
                    {
                        int[] seatInfo = (int[])sysMsg.sysContent;
                        string userSender = (string)_sysMsg.userSender;
                        //System.Console.WriteLine("seat {0} {1} {2}", userSender, seatInfo[0], seatInfo[1]);
                        hall.PlayerSeatDown(seatInfo[0], seatInfo[1], userSender);

                        // 如果当前桌子有人发来信息，且不是本人说明有后来的玩家进入
                        // 则更新先进入者的信息
                        if (userSender != myLogic.myClientName && seatInfo[0] == this.tableIdx)
                        {
                            int otherLogicPos = (seatInfo[1] - seatIdx + 4) % 4 - 1;
                            otherPlayersLogic[otherLogicPos].SetPlayer(userSender);
                        }
                    }
                    break;
                    // 通知玩家有人进入某房间
                case MsgSysType.LeaveRoom:
                    {
                        int[] seatInfo = (int[])sysMsg.sysContent;
                        string userSender = (string)_sysMsg.userSender;
                        
                        hall.PlayerLeaveTable(seatInfo[0], seatInfo[1], userSender);
                        if (userSender != myLogic.myClientName && seatInfo[0] == this.tableIdx)
                        {
                            int otherLogicPos = (seatInfo[1] - seatIdx + 4) % 4 - 1;
                            otherPlayersLogic[otherLogicPos].UserQuit();
                        }
                    }
                    break;
                case MsgSysType.ReadyMsg:
                    {
                        
                        //gameRoom.AddSysMsg(userSender);
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
                           
                            myLogic.gameArea.UnGameStatus();
                            foreach (Logic lg in otherPlayersLogic)
                            {
                                lg.gameArea.UnGameStatus();
                            }

                            if (gameRoom.InvokeRequired)
                            {
                                gameRoom.Invoke(new showFun(gameRoom.ControlAdjustNO));
                            }
                            else gameRoom.ControlAdjustNO();
                           
                           
                                if (MessageBox.Show(user + "获胜！", "游戏提示消息！", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
                                {
                                    
                                    if (gameRoom.InvokeRequired)
                                    {
                                        gameRoom.Invoke(new showFun(gameRoom.ControlAdjustYes));
                                    }
                                    else gameRoom.ControlAdjustYes();
                                }
                                else
                                {
                                    if (gameRoom.InvokeRequired)
                                    {
                                        gameRoom.Invoke(new showFun(gameRoom.ControlAdjustYes));
                                    }
                                    else gameRoom.ControlAdjustYes();
                                }
                            
                           
                        }
                    }
                    break;
            }
                   
        }

        public void SendGameData(MsgGame msg)//发送游戏数据
        {
            if (keepalive)
            {
                msg.tableIdx = this.tableIdx;
                msg.seatIdx = this.seatIdx;
                //msg.userName = myLogic.myClientName;

                Message sendMsg = new Message(msg);
                sendMsg.reciType = ReciveType.Room;
                sendMsg.Num = this.tableIdx;
                //MsgGame msg
                userSend(sendMsg);
            }
        }

        public void UserReady(int total)//发送用户准备请求数据
        {
            if (keepalive)
            {
                MsgSys sysMsg = new MsgSys();
                sysMsg.sysType = MsgSysType.Ready;
                sysMsg.sysContent = total;
                Message conn = new Message(sysMsg);

                userSend(conn);
            }
        }

        public void StartGame()//发送游戏开始数据
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

        public void userSend(Message msg)//封装网络层发送方法
        {      
            msg.userSender = myLogic.myClientName;
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

            foreach (Logic lg in otherPlayersLogic)
            {
                lg.UserQuit();
                lg.HideArea();
            }


            // 然后将当前的table信息清空
            this.tableIdx = -1;
            this.seatIdx = -1;
        }

        public delegate void showFun();
        public delegate void addListMsgDeleg(string msg);
        public void addListMsg(string msg)
        {
            //gameRoom.AddSysMsg(msg);
        }
       // public delegate void showFun(bool b);
        //显示游戏界面中其他玩家界面
        private void showGameRoom(int tableIdx, int seatIdx)
        {
            myLogic.gameArea.UnGameStatus();
            gameTable tablesInfo = hall.tables[tableIdx];
            int mypos = seatIdx;
            int t = 0;
            for (int i = mypos + 1; i < mypos + 4; i++)
            { 
                int k = i % 4;
                if (tablesInfo.seatUser[k] != "")
                {
                    otherPlayersLogic[t].SetPlayer(tablesInfo.seatUser[k]);
                    GameArea ga = otherPlayersLogic[t].gameArea;
                    ga.UnGameStatus();
                    ga.Show();
                }
                else
                {
                    GameArea ga = otherPlayersLogic[t].gameArea;
                    ga.UnGameStatus();
                    //ga.Show();
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
                // 需要添加一种网络消息，表示玩家做下了某个桌子
                // 其他玩家更加该消息，更新table的状态
                //
                // sendNetMsg(thisplayer on table)

                //gameRoom = CreateGameForm();
               // hall.Hide();
                myStatus = PlayerStatus.OnTable;

                SendPlayerSitDown(tableIdx, seatIdx);
                   
                
                showGameRoom(tableIdx, seatIdx);

                gameRoom.Text = "连连看-房间" + System.Convert.ToString(tableIdx + 1);
                gameRoom.Text += "|" + myLogic.myClientName;
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
