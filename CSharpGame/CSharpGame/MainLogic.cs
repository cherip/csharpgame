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
        // 保持连接？
        public bool keepalive = true;

        // 当前client的玩家的Logic
        Logic myLogic;
        PlayerStatus myStatus;

        // 其他玩家的Logic, 用于显示缩略图
        List<Logic> otherPlayersLogic;

        public Room hall;
        public CSharpGame gameRoom;

        public MainLogic()
        {
            myClientSoc = new MyClientSoc();
            keepalive = false;

            // 生成界面的form
            hall = new Room(this);
            gameRoom = CreateGameForm();

            myStatus = PlayerStatus.OffLine;
        }

        private CSharpGame CreateGameForm()
        {
            CSharpGame gameForm = new CSharpGame(this);

            // 首先创建4个logic
            myLogic = new Logic(1);
            myLogic.sendMsgEvent += SendCurrentData;

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
                if (lg.myClientName != null)
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

            myClientSoc.SendMsg(msg);
            Message serverMsg = myClientSoc.RecieveMsg();
            if (serverMsg.msgType == MsgType.Sys)
            {
                MsgSys loginMsg = (MsgSys)serverMsg.msgContent;
                if (loginMsg.sysType == MsgSysType.Judge)
                {
                    bool ret = (bool)loginMsg.sysContent;
                    // 登录失败 直接返回
                    if (ret != true)
                        return false;

                    // 登录成功
                    keepalive = true;
                    //启动后台线程接受服务器端发送的消息
                    if (receiveThread == null)
                    {
                        receiveThread = new Thread(new ThreadStart(NetRuning));
                        receiveThread.Start();
                    }
                }
                else
                {
                    return false;
                }
            }
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

            return ConnectNet(new Message(sysMsg));
        }

        public void CloseConn(string msg)
        {
            if (keepalive)
            {
                MsgSys sysMsg = new MsgSys();
                sysMsg.sysType = MsgSysType.Offline;
                sysMsg.sysContent = myLogic.myClientName;
                myClientSoc.SendMsg(new Message(sysMsg));
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
                    processMsg(serverMsg);
                }
            }

            //  keepalive = false;
            myClientSoc.CloseConn();
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
                        MsgSys sysMsg = (MsgSys)msg.msgContent;

                        ProcessSysMsg(sysMsg);
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

        private void ProcessSysMsg(MsgSys sysMsg)
        {
            switch (sysMsg.sysType)
            {
                case MsgSysType.Judge:
                    {
                        string[] judged = (string[])sysMsg.sysContent;
                        if (judged[0].Equals("false"))
                        {
                            MessageBox.Show("没有这个用户");
                        }
                        if (judged[0].Equals("true"))
                        {
                            myLogic.myClientName = judged[1];
                            MsgSys s = new MsgSys();
                            s.sysType = MsgSysType.Online;
                           
                            s.sysContent = myLogic.myClientName;

                            Message conn = new Message(s);
                            myClientSoc.SendMsg(conn);
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
            }
        }

        public void SendCurrentData(Message msg)
        {
            if (keepalive)
            {              
                myClientSoc.SendMsg(msg);
            }
        }

        public void UserReady()
        {
            if (keepalive)
            {
                MsgSys sysMsg = new MsgSys();
                sysMsg.sysType = MsgSysType.Ready;
                sysMsg.sysContent = myLogic.myClientName;
                Message conn = new Message(sysMsg);
                myClientSoc.SendMsg(conn);
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
                myClientSoc.SendMsg(conn);
            }
        }

        private void ProcessGameMsg(MsgGame gameMsg)
        {
            int[] pair = gameMsg.cleanPair;
            if (pair == null || pair.Length != 2)
            {
                return;
            }

        }


        //
        //
        // 处理房间中的逻辑
        //
        //
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

                // 这里为了简单起见，没有使用多线程，显示多界面了，每次只能有一个界面出现
                gameRoom.ShowDialog();

                myStatus = PlayerStatus.OnLine;
                hall.Show();
            }
        }
    }
}
