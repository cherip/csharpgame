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
    class MainLogic
    {
        // 网络通信的提供者
        MyClientSoc myClientSoc;

        // 接受服务器消息，会改变界面的
        private Thread receiveThread;
        // 保持连接？
        public bool keepalive = true;

        // 当前client的玩家的Logic
        Logic myLogic;
        // 其他玩家的Logic, 用于显示缩略图
        List<Logic> otherPlayersLogic;

        public delegate bool CreateArea(object panel);
        public event CreateArea createMainArea;
        public event CreateArea createOppeArea;

        public MainLogic()
        {
            myClientSoc = new MyClientSoc();
            keepalive = false;
            myLogic = new Logic(1);
            otherPlayersLogic = new List<Logic>();

            // test codes
            for (int i = 0; i < 3; i++)
                otherPlayersLogic.Add(new Logic(2));
        }

        private void InitComponent()
        {
            // 绘制界面
            GameArea ga = myLogic.shower;
            createMainArea(ga);
            List<OtherGameArea> ret = new List<OtherGameArea>();
            foreach (Logic lg in otherPlayersLogic)
            {
                ret.Add(lg.oshower);
            }
            createOppeArea(ret);
        }

        private void InitGame(MsgSys msg)
        {
            // 绘制界面
            InitComponent();

            // 初始化所有玩家的状态
            int[] gameStart = (int[])msg.sysContent;
            
            // 初始本地的
            myLogic.InitGame(gameStart);
            myLogic.Enable();
            //myLogic.startGame();
            // 其他玩家的
            foreach (Logic lg in otherPlayersLogic)
            {
                lg.InitGame(gameStart);
            }
        }

        //
        // 网络通信的功能
        //
        public void ConnectNet(Message msg)
        {
            myClientSoc.InitialSoc();

            // 启动单独的线程用于接收服务器端发送来的消息
            if (receiveThread == null)
                receiveThread = new Thread(new ThreadStart(NetRuning));
            receiveThread.Start();

            //myClientSoc.SendStr("login", msg);
            myClientSoc.SendMsg(msg);

        }

        public void ConnectNet()
        {
            if (keepalive == false)
            {
                keepalive = true;
                Random r = new Random();

                MsgSys sysMsg = new MsgSys();
                sysMsg.sysType = MsgSysType.Online;
                sysMsg.sysContent = "user" + r.Next(0, 1000);
                
                myLogic.myClientName = (string)sysMsg.sysContent;

                Message conn = new Message(sysMsg);
                ConnectNet(conn);
            }
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
        }

        private void ProcessSysMsg(MsgSys sysMsg)
        {
            switch (sysMsg.sysType)
            {
                case MsgSysType.Join:
                    {
                        //某玩家加入
                        string userName = (string)sysMsg.sysContent;
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
                        List<string> readyList = (List<string>)sysMsg.sysContent;//房主名字
                    }
                    break;
                case MsgSysType.Begin:
                    {
                        InitGame(sysMsg);
                    }
                    break;
            }
        }

        public void UserReady(string userName)
        {
            if (keepalive)
            {
                MsgSys sysMsg = new MsgSys();
                sysMsg.sysType = MsgSysType.Ready;
                sysMsg.sysContent = userName;
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

    }
}
