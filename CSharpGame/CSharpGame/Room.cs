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
    public partial class Room : Form
    {
        public const int MAX_PIC = 64;
        static System.Timers.Timer timeElapsed;//计时器
        int curTime = 0;//当前游戏剩余时间
        int total = 1;//默认是1副图

        bool Nobody;
        int mySeatIdx;
        int myTableIdx;

        public delegate void ListviewDeleg(string userName);
        public delegate void LableDeleg(string s);
        public delegate void ButtonDeleg(int i);

        MainLogic mainLogic;

        List<Logic> logicList;

        public Room()
        {
            InitializeComponent();
            myInitial();
        }

        public Room(MainLogic logic)
            : this()
        {
            mainLogic = logic;

            // 将闪烁的btn的点击事件和logic中的逻辑绑定
            //this.hintbtn.Click += new System.EventHandler(logic.HintNext);
            //this.hintbtn.Click += new System.EventHandler(logicList[0].hintclicked

            logicList = new List<Logic>();
            // 游戏房间内 有 1个 主logic
            logicList.Add(new Logic(1));

            // 下面3个 副logic，表示3个位子给对手
            
            for (int i = 0; i < 3; i++)
            {
                logicList.Add(new Logic(2));
            }

            mySeatIdx = -1;
            myTableIdx = -1;
            Nobody = true;
        }

        public void Draw()
        { 
            // 绘制自己的操作界面
            GameArea ga = logicList[0].gameArea;
            ga.Draw(new Point(2, 2), new Size(666, 600));
            ga.picList = this.picList;
            this.Controls.Add(ga);

            // 绘制其他玩家的界面
            int start_x = 2;
            int start_y = 612;
            for (int i = 1; i < logicList.Count; i++)
            {
                ga = logicList[i].gameArea;
                ga.Draw(new Point(start_x, start_y), new Size(222, 200));
                ga.picList = this.picList;
                start_x += 222;
                this.Controls.Add(ga);
            }

            foreach (Logic lg in logicList)
            {
                lg.HideArea();
            }
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

        //private void button_start(object sender, EventArgs e)
        //{
        //    myInitial();
        //}

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

        }

        private void removeExited(string userName)
        {
        }

        private void adduserList(string userName)
        {
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mainLogic.UserReady(total);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mainLogic.TestStart();
        }

        private void CSharpGame_Load(object sender, EventArgs e)
        {

        }

        private void CSharpGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Logic lg in logicList)
            {
                lg.UserQuit();
            }
            Nobody = true;
            mainLogic.QuitGameArea();
        }

        public void RefreshGame(string playerName, int[] gameStatus)
        {
            foreach (Logic lg in logicList)
            {
                if (lg.myClientName == playerName)
                {
                    lg.InitGame(gameStatus);
                    break;
                }
            }
        }

        public void StartGame(int[] gameStatus)
        {
            foreach (Logic lg in logicList)
            {
                lg.InitGame(gameStatus);
            }
        }

        public void ShowGameRoom()
        { 
            
        }

        public void PlayerCleanPair(int seatIdx, int pic1, int pic2)
        {
            if (seatIdx == this.mySeatIdx)
            {
                return;
            }
            else
            {
                int logicPos = (seatIdx - this.mySeatIdx + 4) % 4;
                logicList[logicPos].CleanBtnPair(pic1, pic2);
            }
        }

        public void PlayerEnterRoom(int seatIdx, string username)
        {
            if (Nobody == true)
            {
                Nobody = false;
                this.mySeatIdx = seatIdx;
                foreach (Logic lg in logicList)
                {
                    lg.HideArea();
                }
            }

            int logicPos = (seatIdx - this.mySeatIdx + 4) % 4;
            this.logicList[logicPos].SetPlayer(username);
            //this.logicList[logicPos].gameArea.UnGameStatus();
            //this.logicList[logicPos].ShowArea();
        }

        public void PlayerLeaveRoom(int seatIdx, string username)
        {
            int logicPos = (seatIdx - this.mySeatIdx + 4) % 4;
            this.logicList[logicPos].UserQuit();
        }

        public void Win(string username)
        { 
            
        }

        private void hintbtn_Click(object sender, EventArgs e)
        {
            this.logicList[0].hintclicked();
        }
    }
}
