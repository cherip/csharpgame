﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

namespace CSharpGame
{
    class Logic
    {
        MyClientSoc myClientSoc;
        string myClientName;
        GameClient other;
        public const int MAX_PIC = 64;
        static System.Timers.Timer timeElapsed;                 //计时器
        int curTime = 0;                                        //当前游戏剩余时间
        //Button[] butArry = new Button[MAX_PIC];
        int[] butArry = new int[MAX_PIC];

        int[] picArry = new int[MAX_PIC];
        int last_click = -1;
        int cur_click = -1;
        //Button last_click = null;                             //记录上一次点击的按钮
        //Button curr_click = null;                             //当前点击的按钮
        //Hashtable btnVal;

        int pairPicCounts = -1;//记录当前副图的总共消除次数
        public int totalTurns = 1;//总共需要完成的轮数，默认是1次
        public bool started = false;//游戏开始与否
        Random r;
        private Thread receiveThread;
        string[] receiveStr;
        public bool keepalive = true;

        public delegate void NetworkProcess(object param, int type);    //处理网络消息
        public event NetworkProcess newtworkProcessor;                  //form传递函数，直接操作form中的控件

        public Logic()
        {
            keepalive = false;
            //myClientSoc = new MyClientSoc();
            pairPicCounts = -1;
        }

        public void InitLogic() {
            // 生成button对应的图像 和 统计总共消除的次数
            MyFormat.genPic(ref butArry);
            int[] tmp = (int[])butArry.Clone();
            pairPicCounts = MyFormat.countPairPic(tmp);
        }

        public int GetPicType(int pos) {
            if (pos < 0 || pos >= butArry.Length)
                return -1;
            return butArry[pos];
        }

        public int[] PushButton(int pos) {
            if (last_click == -1) {
                last_click = pos;
                return null;
            } else {
                if (last_click != pos && butArry[last_click] == butArry[pos]) {
                    butArry[last_click] = -1;
                    butArry[pos] = -1; 
                    int ret = last_click;
                    last_click = -1;
                    int[] r = new int[2] {ret, pos};
                    return r;
                } else {
                    last_click = pos;
                    return null;
                }
            }
        }

        public int ClearAnPair() {
            pairPicCounts--;
            if (pairPicCounts == 0) {
                if (--totalTurns == 0) {
                    return 2;
                }
                return 1;
            }
            return 0;
        }

        //
        // 网络通信的功能
        //
        public void ConnectNet(Message msg)
        {
            keepalive = true;
            myClientSoc = new MyClientSoc();
	        myClientSoc.InitialSoc();

            // 启动单独的线程用于接收服务器端发送来的消息
            //receiveThread = new Thread(new ThreadStart(NetRuning));
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
                myClientName = (string)sysMsg.sysContent;

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
                sysMsg.sysContent = myClientName;
                Message conn = new Message(sysMsg);
                myClientSoc.SendMsg(conn);
	            //myClientSoc.SendStr("exit", myClientName);
	            //... someting to do
	            //myClientSoc.CloseConn();

                //这里选择等待receive线程的结束，否则可能在断时间内，多次
                //receiveThread.Join();
                //myClientSoc.CloseConn();
                //receiveThread.Abort();

                //receiveThread.Join();
                
	            //keepalive = false;
                receiveThread = null;
                keepalive = false;
            }
            
        }
        public void NetRuning() {
            while (keepalive) {
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
                        List<string> readyList= (List<string>)sysMsg.sysContent;//房主名字
                    }
                    break;
                case MsgSysType.Begin:
                    {
                        //获得服务器初始数据，更新游戏界面
                    }
                    break;
            
            }
                         
        }

        // 
        // 下面都要改。。。
        // 下面都要改。。。
        //
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

        public void sendGameData()
        {
            myClientSoc.SendStr("gamedata", myClientName + '|' + MyFormat.arrayToStr(butArry));
        }

        public void inviteUser(string answer)
        {
            myClientSoc.SendStr("invite",myClientName + '|' + answer);
            //other.Add(new GameClient(answer, null, null, null));
        }

        public void gameStart()
        {            
           // myClientSoc.SendStr("gamestart", str);
            myClientSoc.SendStr("startgame", myClientName);
        }

        public void acceptInvite()
        {

        }


    }
}
