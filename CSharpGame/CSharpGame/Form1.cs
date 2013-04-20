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
    public partial class CSharpGame : Form
    {
        public const int MAX_PIC = 64;
        static System.Timers.Timer timeElapsed;//计时器
        int curTime = 0;//当前游戏剩余时间
        Button[] butArry = new Button[MAX_PIC];
        string username;
        Hashtable btnVal;
        private static Thread pthread;

        public delegate void PairBingoHandle(object sender, EventArgs e);//消除两张图代理
        public event PairBingoHandle pairBingoEvent;//消除事件
        public delegate void ListviewDeleg(string userName);
        public delegate void LableDeleg(string s);
        public delegate void ButtonDeleg(int i);

        Logic myLogic = new Logic();
        GameArea gameArea;

        public CSharpGame()
        {
            //int index = 0;
            //myLogic.InitLogic();

            InitializeComponent();
            initCreateControl();

            myInitial();

            //int[] tester = new int[] { 2, 1 };
            //byte[] ss = SerializationUnit.SerializeObject(tester);
            //int[] ob = (int[])(SerializationUnit.DeserializeObject(ss));
        }

        private void initCreateControl()
        {
            // 初始化 自己的游戏界面
            gameArea = new GameArea(new System.Drawing.Point(2, 238),
                                    new System.Drawing.Size(669, 600));

            gameArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            gameArea.Name = "myGameShower";
            gameArea.TabIndex = 0;
            gameArea.Enabled = false;

            this.Controls.Add(gameArea);

            // 初始化其他玩家的游戏界面， 这里应该由其他玩家控制。
            // 测试情况下 初始化一个小的

            OtherGameArea playerTesterArea = new OtherGameArea(new System.Drawing.Point(2, 10),
                                                     new System.Drawing.Size(200, 200));
            playerTesterArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            playerTesterArea.Name = "plTesterShower";
            playerTesterArea.TabIndex = 0;
            playerTesterArea.Enabled = false;
            this.Controls.Add(playerTesterArea);
        }

        private void myInitial()
        {           
            timeElapseBar.Maximum = 3;

            timeElapseBar.Value = timeElapseBar.Maximum;
            curTime = timeElapseBar.Value;

            if (timeElapsed == null)//唯一一个计时器，防止多次点击开始按钮，触发多次elapse事件
            {
                timeElapsed = new System.Timers.Timer(10000);
                timeElapsed.Elapsed += new System.Timers.ElapsedEventHandler(timeElapsed_Elapsed);
            }         
            timeElapsed.Start();                     
        }

        private void button_start(object sender, EventArgs e)
        {
            myInitial();
            myLogic.started = true;
            startBtn.Enabled = false;
            //MessageBox.Show("xxx" + pairPicCounts);
        }

        private void picBtn_Clicked(object sender, EventArgs e)              //点击图片按钮事件
        {
            //
            // 利用logic中的PushButton变量来得到是否消除某2个button
            // 但是我那边的logic没有写对，需要返回消除的点对。

            Button curr_click = (Button)sender;
            int pos = (int)btnVal[curr_click];
            int[] ret = myLogic.PushButton(pos);
            if (ret != null)
            {
                butArry[ret[0]].Visible = false;
                butArry[ret[1]].Visible = false;

                pairBingoEvent(sender, e);
            }
        }

        private void pairBingo(object sender, EventArgs e)    //两张图一样时，触发事件
        {
            //
            // 利用logic 完成逻辑判断，form只完成显示
            //
            //联机
            if (myLogic.keepalive)
            {
                myLogic.sendGameData();
            }
            
            int state = myLogic.ClearAnPair();

            if (state == 2)
            {
                MessageBox.Show("Win!");
                myLogic.started = false;
                startBtn.Enabled = true;
            }
            else if (state == 1)
            {
                button_start(null, e);
            }
        }

        private void radioClicked(object sender, EventArgs e)
        {
            if (!myLogic.started)
                   myLogic.totalTurns = int.Parse(((RadioButton)sender).Text);
        }

        private void hintclicked(object sender, EventArgs e)
        {
            //bool hinted = false;
            //for (int i = 0; i < MAX_PIC; i++)
            //{
            //    if (butArry[i].Visible)
            //    {
            //        for (int j = i + 1; j < MAX_PIC; j++)
            //        {
            //            if ((int)btnVal[butArry[i]] == (int)btnVal[butArry[j]] && butArry[j].Visible)
            //            {
            //                for (int k = 0; k < 3; k++)
            //                {
            //                    butArry[i].Visible = false;
            //                    butArry[j].Visible = false;
            //                    Thread.Sleep(200);
            //                    butArry[i].Visible = true;
            //                    butArry[j].Visible = true;
            //                    this.Refresh();
            //                    Thread.Sleep(200);
            //                }
            //                hinted = true;
            //                break;
            //            }
            //        }
            //    }
            //    if (hinted)
            //        break;
            //    else if (MAX_PIC - 1 == i)
            //        MessageBox.Show("no Hint!!!");
            //}

            PairPics pairpics = gerPairPics();
            if (pairpics != null)
            {
                hintbtn.Enabled = false;
                for (int k = 0; k < 3; k++)//相同图像的按钮闪烁
                {
                    butArry[pairpics.PicNO1].Visible = false;
                    butArry[pairpics.PicNO2].Visible = false;
                    Thread.Sleep(100);
                    butArry[pairpics.PicNO1].Visible = true;
                    butArry[pairpics.PicNO2].Visible = true;
                    this.Refresh();
                    Thread.Sleep(100);
                }
                hintbtn.Enabled = true;
            }
            else MessageBox.Show("NO hint!!!");
           
        }
        private PairPics gerPairPics()//提示功能中，获得两个相同图像的按钮
        {
            bool hinted = false;
            PairPics pairpics = new PairPics();
            for (int i = 0; i < MAX_PIC; i++)
            {
                if (butArry[i].Visible)
                {
                    for (int j = i + 1; j < MAX_PIC; j++)
                    {
                        int picType1 = myLogic.GetPicType(i);
                        int picType2 = myLogic.GetPicType(j);
                        if (picType1 == picType2 && butArry[j].Visible)
                        {
                            pairpics.PicNO1 = i;
                            pairpics.PicNO2 = j;
                            hinted = true;
                            break;
                        }
                    }
                }
                if (hinted)
                    break;
                else if (MAX_PIC - 1 == i)
                    return null;
            }
            return pairpics;
        }
    
        void  timeElapsed_Elapsed(object source, System.Timers.ElapsedEventArgs e)//计时事件响应，更新processbar
        {
            if (timeElapseBar.InvokeRequired)//线程安全
            {
                System.Timers.ElapsedEventHandler ed = timeElapsed_Elapsed;
                timeElapseBar.Invoke(ed,new object[]{source,e});
            }
            else
            {
                if (0 >= --curTime)
                {
                    timeElapsed.Enabled = false;
                    timeElapseBar.Value = 0;
                    timeElapseBar.Refresh();
                    //MessageBox.Show("You Lose!!");
                }
                else timeElapseBar.Value = curTime;                              
            }
        }

        private void conn_Click(object sender, EventArgs e)
        {
            //myClientSoc = new MyClientSoc();
            //myClientSoc.InitialSoc();
            //myClientSoc.SendStr("login","xxxx" + r.Next());
            //receiveStr = myClientSoc.RecieveStr();
            //pthread = new Thread(new ThreadStart(processStr));
            //pthread.Start();
         
            // 启动网络连接
           // connBtn.Enabled = false;
            //Random r = new Random();
            //username = "user" + r.Next(0, 1000);

            // 先绑定事件
            //myLogic.newtworkProcessor += updateForm;
            gameArea.connect();
           // myLogic.ConnectNet();
           // button2.Enabled = false;
            // 事件绑定
            //networkRun();

            //pthread = new Thread(new ThreadStart(networkRun));
            //pthread.Start();
            

            
        }

        private void networkRun()
        {
            // 添加事件
            myLogic.newtworkProcessor += updateForm;
            // 启动循环接受处理。
            //myLogic.NetRuning();
        }

        private void updateForm(object sender, int type)
        {    
            switch (type)
            {
                case 1:
                    {
                        string[] nameList = (string[])sender;
                        foreach (string s in nameList)
                        {
                            adduserList(s);
                        }
                        break;
                    }
                case 2:
                    {
                        removeExited((string)sender);
                        break;
                    }
                case 3:
                    {
                        adduserList((string)sender);
                        break;
                    }
                case 4:
                    {
                        showInviteRequst((string)sender);
                        break;
                    }
                case 5:
                    {
                        for (int i = 0; i < butArry.Length; i++)
                        {
                            refreshButton(i);
                        }
                        break;
                    }
            }
        }

        private void refreshButton(int i)
        {
            if (butArry[i].InvokeRequired)
            {
                ButtonDeleg bd = new ButtonDeleg(refreshButton);
                butArry[i].Invoke(bd, new object[]{i});
            }
            else
            {
                int type = myLogic.GetPicType(i);
                butArry[i].BackgroundImage = picList.Images[type];         
            }       
        }

        private void showInviteRequst(string param)
        {
            if (namlable.InvokeRequired)
            {
                LableDeleg ld = new LableDeleg(showInviteRequst);
                namlable.Invoke(ld, new object[] { param });
            }
            else
            {
                namlable.Text = param;
                button2.Enabled = true;
            }
            
        }

        private void removeExited(string userName)
        {
            if (listView1.InvokeRequired)
            {
                ListviewDeleg ld = new ListviewDeleg(removeExited);
                listView1.Invoke(ld, new object[] { userName });
            }
            else
            {
                foreach (ListViewItem lv in listView1.Items)
                {
                    if (lv.Text == userName)
                    {
                        listView1.Items.Remove(lv);
                    }
                }
            }
        }

        private void adduserList(string userName)
        {
            if (listView1.InvokeRequired)
            {
                ListviewDeleg ld = new ListviewDeleg(adduserList);
                listView1.Invoke(ld, new object[] { userName });
            }
            else
            {
                listView1.Items.Add(userName);
            }
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            myLogic.CloseConn(username);
            listView1.Clear();
            listView1.Items.Clear();

            myLogic.newtworkProcessor -= updateForm;
            //pthread.Abort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myLogic.inviteUser(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            myLogic.gameStart();
        }

        private void CSharpGame_Load(object sender, EventArgs e)
        {

        }
    }
}
