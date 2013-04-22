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
        public string myClientName;
        public const int MAX_PIC = 64;
        int[] butArry = new int[MAX_PIC];
        int last_click = -1;

        int pairPicCounts = -1;//记录当前副图的总共消除次数
        public int totalTurns = 1;//总共需要完成的轮数，默认是1次
        public bool started = false;//游戏开始与否
        string username;

        // 事件函数处理设置btn的图片
        public delegate bool SetBtnImage(int idx, int type);
        public event SetBtnImage btnImageSetFunc;

        // 事件函数处理 消除btn
        public delegate void CleanBtn(int a, int b);
        public event CleanBtn cleanBtnPair;
        //消除之后传给服务器
        public delegate void SendmsgDeleg(Message msggame);
        public event SendmsgDeleg sendMsgEvent;

        // 非常2比的设计，因为logic要大量调用gamearea的界面显示函数，
        // 所以logic这里放入一个所控制的gamearea的变量
        // 其实应该把 gamearea放入logic之下， 然后回调logic的函数
        // 还做不了 应该不是一个线程。。囧
        public GameArea shower;
        public OtherGameArea oshower;
        //public OtherGameArea gameShower;

        public delegate void EnableGameArea();
        public event EnableGameArea startGame;
        
        // 其他玩家的Area;

        public Logic()
        {
            pairPicCounts = -1;
        }

        public Logic(GameArea gameShow)
        {
            //myClientSoc = new MyClientSoc();
            pairPicCounts = -1;
        }

        public Logic(int type)
            : this()
        {
            shower = null;
            oshower = null;

            switch (type)
            {
                case 1:
                    {
                        shower = new GameArea();
                        shower.btnClickEvent += PushButton;

                        // 不好的设计
                        this.cleanBtnPair += shower.CleanBtnPair;
                        this.btnImageSetFunc += shower.SetBtnImage;
                        this.startGame += shower.EnableArea;
                    }
                    break;
                case 2:
                    {
                        oshower = new OtherGameArea();
                        GameArea vshower = oshower.gameArea;
                        vshower.btnClickEvent += PushButton;

                        // 不好的设计
                        this.cleanBtnPair += oshower.CleanButton;
                        this.btnImageSetFunc += vshower.SetBtnImage;
                        this.startGame += vshower.EnableArea;
                    }
                    break;
            }
        }

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

            //startGame();
            //System.Windows.Forms.pa
        }

        public void Enable()
        {
            startGame();
        }

        public int GetPicType(int pos)
        {
            if (pos < 0 || pos >= butArry.Length)
                return -1;
            return butArry[pos];
        }

        public void CleanBtnPair(int a, int b)
        {
            cleanBtnPair(a,b);
        }

        public void PushButton(int pos)
        {
            if (last_click == -1)
            {
                last_click = pos;
            }
            else
            {
                if (last_click != pos && butArry[last_click] == butArry[pos])
                {
                    //传给mainlogic
                    MsgGame msggame = new MsgGame();
                    msggame.userName = myClientName;
                    msggame.cleanPair[0] = last_click;
                    msggame.cleanPair[1] = pos;
                    Message msgtosend = new Message(msggame);
                    sendMsgEvent(msgtosend);

                    butArry[last_click] = -1;
                    butArry[pos] = -1;
                    int ret = last_click;
                    last_click = -1;

                    // 调用绑定的事件 消除btn
                    cleanBtnPair(ret, pos);
                    // 再调用后续的处理逻辑
                    ClearAnPair();
                    //int[] r = new int[2] {ret, pos};
                   
                }
                else
                {
                    last_click = pos;
                }
            }
        }

        public void ClearAnPair()
        {
            pairPicCounts--;

            // 如果此副牌接受
            if (pairPicCounts == 0)
            {

                // 如果完成所有牌
                if (--totalTurns == 0)
                {
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

    }
}
