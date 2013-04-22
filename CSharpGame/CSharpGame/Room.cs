using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSharpGame
{
    public partial class Room : Form
    {
        public List<gameTable> tables;
        MainLogic mainLogic;

        public Room()
        {
            InitializeComponent();
            InitControl();
        }

        public Room(MainLogic logic)
        {
            InitializeComponent();
            mainLogic = logic;
            InitControl();

            this.panelTables.Hide();
            this.panelPlayers.Hide();
        }

        public void InitControl() 
        {
            tables = new List<gameTable>();
            const int MaxTableNum = 9;
            const int TableNumInRow = 3;
            int startX = 2;
            int startY = 2;
            Size tableSize = new Size(150, 130);
            for (int i = 0; i < MaxTableNum; i++)
            {
                gameTable table = new gameTable(i);
                table.Location = new Point(startX + tableSize.Width * (i % TableNumInRow),
                                           startY + tableSize.Height * (i / TableNumInRow));
                table.Size = tableSize;

                if (mainLogic != null)
                {
                    table.clickSeatCallback += mainLogic.ClickSeat;
                }

                tables.Add(table);
                this.panelTables.Controls.Add(table);
                //this.afterLogin.Controls.Add(table);
            }
        }

        public delegate void addPlayers(List<string> playersName);
        public void AddPlayers(List<string> playersName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new addPlayers(AddPlayers),
                            new object[] { playersName });
            }
            else
            {
                foreach (string player in playersName)
                {
                    listPlayers.Items.Add(player);
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            // 这里的逻辑是： 点击登录，玩家直接登录，
            // 如果要做用户名和密码的话，则传递user和pwd给connect函数
            // 通过返回结果 判断是否进去下一个界面还是让用户重新登录

            if (this.mainLogic.PlayerLogin(this.txtUserName.Text,
                                           this.txtPwd.Text))
            {
                this.panelLogin.Hide();
                //this.InitControl();

                //// 测试代码
                //List<string> names = new List<string>();
                //names.Add("limian");
                //names.Add("xiaoyong");
                //AddPlayers(names);

                this.panelTables.Show();
                this.panelPlayers.Show();
            }
        }

        public void PlayerSeatDown(int tableIdx, int SeatIdx)
        {
            if (tableIdx < 0 || tableIdx >= tables.Count)
            {
                return;
            }

            tables[tableIdx].ClickSeat(SeatIdx);
        }

        public void PlayerSeatDown(int tableIdx, int SeatIdx, string user)
        {
            if (tableIdx < 0 || tableIdx >= tables.Count)
            {
                return;
            }

            if (tables[tableIdx].seatUser[SeatIdx] == "")
                tables[tableIdx].seatUser[SeatIdx] = user;
            else
                tables[tableIdx].seatUser[SeatIdx] = "";

            tables[tableIdx].ClickSeat(SeatIdx);
        }
    }
}
