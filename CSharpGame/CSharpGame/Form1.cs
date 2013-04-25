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
        //进入游戏界面
        public const int MAX_PIC = 64;
        static System.Timers.Timer timeElapsed;//计时器
        int curTime = 0;//当前游戏剩余时间
        int total = 1;//游戏副数，默认是1副图
        

        //public delegate void PairBingoHandle(object sender, EventArgs e);//消除两张图代理
        //public event PairBingoHandle pairBingoEvent;//消除事件
        public delegate void ListviewDeleg(string userName);
        public delegate void LableDeleg(string s);
        public delegate void ButtonDeleg(int i);

        //Logic myLogic;
        //GameArea gameArea;
        MainLogic mainLogic;// 主逻辑

       
        public CSharpGame()
        {
            InitializeComponent();
            this.ControlBox = false;
            myInitial();
        }

        public CSharpGame(MainLogic logic)
            : this()
        {
            mainLogic = logic;

            // 将闪烁的btn的点击事件和logic中的逻辑绑定
            this.hintbtn.Click += new System.EventHandler(logic.HintNext);
            this.sameBtn.Click += new System.EventHandler(logic.SameBtn);

            this.hintbtn.Visible = false;
            this.sameBtn.Visible = false;
        }

        public delegate bool CreateArea(object panel);
        public bool CreateMainArea(object panel)//创建我的游戏区域
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CreateArea(CreateMainArea), panel);
            }
            else 
            {
                GameArea ga = (GameArea)panel;
                ga.Draw(new Point(0, 0), this.panelMyArea.Size);
                ga.picList = this.picList;
                //this.Controls.Add(ga);
                this.panelMyArea.Controls.Add(ga);
            }
            return true;
        }

        public bool CreateOppeArea(object panel)//创建对手的游戏界面
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CreateArea(CreateOppeArea), panel);
            }
            else
            {
                // do something
                List<GameArea> ga_list = (List<GameArea>)panel;
                int start_x = 0;
                int start_y = 0;
                int w = this.panelOtherArea.Size.Width;
                int h = this.panelOtherArea.Size.Height / 3;
                foreach (GameArea ga in ga_list)
                {
                    ga.Draw(new Point(start_x, start_y), new Size(w, h));
                    ga.picList = this.picList;
                    start_y += h;
                    this.panelOtherArea.Controls.Add(ga);
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

        private void button2_Click(object sender, EventArgs e)//用户点击准备，交予主逻辑处理
        {
            mainLogic.UserReady(total);
            ((Button)sender).Visible = false;
            this.hintbtn.Visible = true;
            this.sameBtn.Visible = true;
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

            mainLogic.QuitGameArea();//用户退出游戏画面
        }

        public void ControlAdjustNO()
        {
            exitBtn.Enabled = false;    //按钮处理
        }

        public void ControlAdjustYes()
        {
            exitBtn.Enabled = true;
            this.button2.Visible = true;
            hintbtn.Visible = false;
            sameBtn.Visible = false;
        }

        private void exitBtn_Click(object sender, EventArgs e)//退出按钮处理
        {
            if (MessageBox.Show("您确定要退出吗？", "游戏提示消息！", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {

                this.hintbtn.Visible = false;
                this.sameBtn.Visible = false;
                this.button2.Visible = true;
                this.Hide();
                mainLogic.QuitGameArea();
            }
           
        }
    }
}
