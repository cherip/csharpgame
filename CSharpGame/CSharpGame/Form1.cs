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
        int total = 1;//默认是1副图
        //Button[] butArry = new Button[MAX_PIC];
        //string username;
        //Hashtable btnVal;
        //private static Thread pthread;

        //public delegate void PairBingoHandle(object sender, EventArgs e);//消除两张图代理
        //public event PairBingoHandle pairBingoEvent;//消除事件
        public delegate void ListviewDeleg(string userName);
        public delegate void LableDeleg(string s);
        public delegate void ButtonDeleg(int i);

        //Logic myLogic;
        //GameArea gameArea;
        MainLogic mainLogic;

        //List<GameArea> otherPlayerDisplay;

        public CSharpGame()
        {
            InitializeComponent();
            myInitial();
        }

        public CSharpGame(MainLogic logic)
            : this()
        {
            mainLogic = logic;

            // 将闪烁的btn的点击事件和logic中的逻辑绑定
            this.hintbtn.Click += new System.EventHandler(logic.HintNext);
        }

        public delegate bool CreateArea(object panel);
        public bool CreateMainArea(object panel)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CreateArea(CreateMainArea), panel);
            }
            else 
            {
                GameArea ga = (GameArea)panel;
                ga.Draw(new Point(2, 2), new Size(666, 600));
                ga.picList = this.picList;
                this.Controls.Add(ga);
            }
            return true;
        }

        public bool CreateOppeArea(object panel)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CreateArea(CreateOppeArea), panel);
            }
            else
            {
                // do something
                List<GameArea> ga_list = (List<GameArea>)panel;
                int start_x = 2;
                int start_y = 612;
                foreach (GameArea ga in ga_list)
                {
                    ga.Draw(new Point(start_x, start_y), new Size(222, 200));
                    ga.picList = this.picList;
                    start_x += 222;
                    this.Controls.Add(ga);
                }
            }
            return true;
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
        }

        private void radioClicked(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            total = int.Parse(rb.Text);
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


        private void showInviteRequst(string param)
        {
            //if (namlable.InvokeRequired)
            //{
            //    LableDeleg ld = new LableDeleg(showInviteRequst);
            //    namlable.Invoke(ld, new object[] { param });
            //}
            //else
            //{
            //    namlable.Text = param;
            //    button2.Enabled = true;
            //}

        }

        private void removeExited(string userName)
        {
            //if (listView1.InvokeRequired)
            //{
            //    ListviewDeleg ld = new ListviewDeleg(removeExited);
            //    listView1.Invoke(ld, new object[] { userName });
            //}
            //else
            //{
            //    foreach (ListViewItem lv in listView1.Items)
            //    {
            //        if (lv.Text == userName)
            //        {
            //            listView1.Items.Remove(lv);
            //        }
            //    }
            //}
        }

        private void adduserList(string userName)
        {
            //if (listView1.InvokeRequired)
            //{
            //    ListviewDeleg ld = new ListviewDeleg(adduserList);
            //    listView1.Invoke(ld, new object[] { userName });
            //}
            //else
            //{
            //    listView1.Items.Add(userName);
            //}
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            //gameArea.logout();
            //listView1.Clear();
            //listView1.Items.Clear();

            //myLogic.newtworkProcessor -= updateForm;
            //pthread.Abort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //myLogic.inviteUser(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mainLogic.UserReady(total);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //myLogic.gameStart();
            mainLogic.TestStart();
        }

        private void CSharpGame_Load(object sender, EventArgs e)
        {

        }

        private void CSharpGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 用户退出相当于 用户从模拟重新点击了一次seat
            mainLogic.QuitGameArea();
        }

        public void ControlAdjustNO()
        {
            this.ControlBox = false;
        }

        public void ControlAdjustYes()
        {
            this.ControlBox = true;
        }
    }
}
