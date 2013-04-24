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
        int[] btnArry = new int[MAX_PIC];
        int last_click = -1;

        int pairPicCounts = -1;//记录当前副图的总共消除次数
        public int totalTurns = 1;//总共需要完成的轮数，默认是1次
        public bool started = false;//游戏开始与否
        string username;


        //消除之后传给服务器
        public delegate void SendmsgDeleg(MsgGame msggame);
        public event SendmsgDeleg sendMsgEvent;

        public GameArea gameArea;

        // 其他玩家的Area;

        public Logic()
        {
            pairPicCounts = -1;
            myClientName = "";
        }

        public Logic(int type)
            : this()
        {
            switch (type)
            {
                case 1:
                    {
                        MyGameArea area = new MyGameArea();
                        area.btnClickEvent += PushButton;
                        gameArea = area;
                        gameArea.BackgroundImage  = global::CSharpGame.Properties.Resources.background1;
                    }
                    break;
                case 2:
                    {
                        gameArea = new OtherGameArea();
                    }
                    break;
            }
        }

        // 通过传入的数组 初始化逻辑
        public void InitGame(int[] gameReset)
        {
            btnArry = (int[])gameReset.Clone();
            int[] tmp = (int[])btnArry.Clone();
            pairPicCounts = MyFormat.countPairPic(tmp);

            for (int i = 0; i < btnArry.Length; i++)
            {
                gameArea.SetBtnImage(i, btnArry[i]);
            }
            gameArea.Reset();
        }

        public void Enable()
        {
            gameArea.EnableArea();
        }

        public int GetPicType(int pos)
        {
            if (pos < 0 || pos >= btnArry.Length)
                return -1;
            return btnArry[pos];
        }

        public void CleanBtnPair(int a, int b)
        {
            gameArea.CleanBtnPair(a,b);
        }


        public void PushButton(int pos)
        {
            if (last_click == -1)
            {
                last_click = pos;
            }
            else
            {
                if (last_click != pos && btnArry[last_click] == btnArry[pos])
                {
                    //传给mainlogic
                    MsgGame msggame = new MsgGame();
                    msggame.userName = myClientName;
                    msggame.cleanPair[0] = last_click;
                    msggame.cleanPair[1] = pos;
                    sendMsgEvent(msggame);

                    btnArry[last_click] = -1;
                    btnArry[pos] = -1;
                    int ret = last_click;
                    last_click = -1;

                    // 调用绑定的事件 消除btn
                    CleanBtnPair(ret, pos);
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

        //
        // 提示功能移到logic中来
        //
        public void hintclicked()
        {
            PairPics pairpics = gerPairPics();
            if (pairpics != null)
            {
                gameArea.HintBlick(pairpics.PicNO1, pairpics.PicNO2, 3);
            }
            else
            {
                MessageBox.Show("NO hint!!!");
            }
        }

        public void showSameList()
        {
            List<int> sameList = gerSameList();
            if (sameList != null)
            {
                gameArea.ShowSameList(sameList);
            }
            else MessageBox.Show("No Same Pictures!!!");
        }

        private PairPics gerPairPics()//提示功能中，获得两个相同图像的按钮
        {
            PairPics pairpics = new PairPics();
            for (int i = 0; i < MAX_PIC; i++)
            {
                if (btnArry[i] != -1)
                {
                    for (int j = i + 1; j < MAX_PIC; j++)
                    {
                        if (btnArry[j] != -1 && btnArry[i] == btnArry[j])
                        {
                                pairpics.PicNO1 = i;
                                pairpics.PicNO2 = j;
                                return pairpics;
                        }
                    }
                }
            }
            return null;
        }
        private List<int> gerSameList()//与i相同的所有的j都显示
        {
            List<int> sameList = new List<int>();
            for (int i = 0; i < MAX_PIC; i++)
            {
                if (btnArry[i] != -1)
                {
                    for (int j = i + 1; j < MAX_PIC; j++)
                    {
                        if (btnArry[j] != -1 && btnArry[i] == btnArry[j])
                        {
                            sameList.Add(j);                                                  
                        }
                    }
                    sameList.Add(i);
                    if (sameList.Count == 1)
                    {
                        sameList.Clear();
                        
                    }
                    else return sameList;
                    
                    
                   
                }
            }
            return null;
        }



        public void SetPlayer(string name)
        {
            this.myClientName = name;
            gameArea.UpdateUser(name);
            ShowArea();
        }

        public void UserQuit()
        {
            this.myClientName = "";
            gameArea.ResetGameStatus();
            HideArea();
        }

        public delegate void showFun();
        public void ShowArea()
        {
            if (gameArea.InvokeRequired)
            {
                gameArea.Invoke(new showFun(gameArea.Show));
            }
            else
            {
                gameArea.Show();
            }
        }

        public void HideArea()
        {
            if (gameArea.InvokeRequired)
            {
                gameArea.Invoke(new showFun(gameArea.Hide));
            }
            else
            {
                gameArea.Hide();
            }
        }
    }
}
