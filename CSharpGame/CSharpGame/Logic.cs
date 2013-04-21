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
    class Logic
    {
        MyClientSoc myClientSoc;
        static string myClientName;
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

        // 事件函数处理设置btn的图片
        public delegate bool SetBtnImage(int idx, int type);
        public event SetBtnImage btnImageSetFunc;

        // 事件函数处理 消除btn
        public delegate void CleanBtn(int a, int b);
        public event CleanBtn cleanBtnPair;

        // 非常2比的设计，因为logic要大量调用gamearea的界面显示函数，
        // 所以logic这里放入一个所控制的gamearea的变量
        // 其实应该把 gamearea放入logic之下， 然后回调logic的函数
        // 还做不了 应该不是一个线程。。囧
        public GameArea gameShower;

        public delegate void EnableGameArea();
        public event EnableGameArea startGame;
        

        public Logic()
        {
            keepalive = false;
            //myClientSoc = new MyClientSoc();
            pairPicCounts = -1;
        }

        public Logic(GameArea gameShow)
        {
            keepalive = false;
            //myClientSoc = new MyClientSoc();
            pairPicCounts = -1;
            this.gameShower = gameShow;
        }

        //public void InitLogic() {
        //    // 生成button对应的图像 和 统计总共消除的次数
        //    MyFormat.genPic(ref butArry);
        //    int[] tmp = (int[])butArry.Clone();
        //    pairPicCounts = MyFormat.countPairPic(tmp);
        //}

        // 通过传入的数组 初始化逻辑
        public void InitGame(int[] gameReset)
        {
            butArry = (int[])gameReset.Clone();
            int[] tmp = (int[])butArry.Clone();
            pairPicCounts = MyFormat.countPairPic(tmp);

            if (btnImageSetFunc != null) 
            {
                for (int i = 0; i < butArry.Length; i++)
                {
                    btnImageSetFunc(i, butArry[i]);
                }
            }

            // 激活gamearea界面。让玩家开始玩游戏
            // this.gameShower.Enabled = true;
            startGame();
            //System.Windows.Forms.pa
        }

        public int GetPicType(int pos) {
            if (pos < 0 || pos >= butArry.Length)
                return -1;
            return butArry[pos];
        }

        public void PushButton(int pos) {
            if (last_click == -1) {
                last_click = pos;
            } else {
                if (last_click != pos && butArry[last_click] == butArry[pos]) {
                    butArry[last_click] = -1;
                    butArry[pos] = -1; 
                    int ret = last_click;
                    last_click = -1;

                    // 调用绑定的事件 消除btn
                    cleanBtnPair(ret, pos);
                    // 再调用后续的处理逻辑
                    ClearAnPair();
                    //int[] r = new int[2] {ret, pos};
                } else {
                    last_click = pos;
                }
            }
        }

        public void ClearAnPair() {
            pairPicCounts--;

            // 如果此副牌接受
            if (pairPicCounts == 0) {

                // 如果完成所有牌
                if (--totalTurns == 0) {
                    // sendWin()
                    // showWin()...
                }
                else
                {
                    // sendFinish();
                    // resetGame();
                }
            }
        }

        //
        // 网络通信的功能
        //
        public void ConnectNet(Message msg)
        {
            //keepalive = true;
            myClientSoc = new MyClientSoc();
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
                myClientSoc.SendMsg(new Message(sysMsg));
                receiveThread = null;
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

            keepalive = false;
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
            }
            return true;
        }

        private void ProcessSysMsg(MsgSys sysMsg)
        {
            if(sysMsg.sysType == MsgSysType.Join)
            {
                //有玩家加入
                string userName = (string)sysMsg.sysContent;
            }
            if (sysMsg.sysType == MsgSysType.List)
            {
                //在线玩家列表
                List<string> userList = (List<string>)sysMsg.sysContent;
            }
            if (sysMsg.sysType == MsgSysType.Exit)
            {
                //某玩家退出
                string userName = (string)sysMsg.sysContent;
            }
            if (sysMsg.sysType == MsgSysType.Begin)
            {
                int[] gameStartStatus = (int[])sysMsg.sysContent;
                InitGame(gameStartStatus);
            }
        }

        // 
        // 下面都要改。。。
        // 下面都要改。。。
        //
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
