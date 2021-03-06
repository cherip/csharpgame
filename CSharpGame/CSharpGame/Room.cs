﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace CSharpGame
{
    public partial class Room : Form
    {
        public List<gameTable> tables;
        MainLogic mainLogic;
        public delegate void btnClick(object sender, EventArgs e);
        public event btnClick btnClickEvent;

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
        //初始化所有桌子
        public void InitControl() 
        {
            //this.ControlBox = false;
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
        //用户坐下，该桌子添加用户
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
        //移除桌子上用户
        public delegate void removePlayer(string playersName);
        public void RemovePlayers(string playersName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new removePlayer(RemovePlayers),
                           new object[] { playersName });
            }
            else
            {
                
                    listPlayers.Items.Remove(playersName);
                
            }
        }

        public delegate void addOnePlayers(string playersName);
        public void AddOnePlayers(string playersName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new addOnePlayers(AddOnePlayers),
                            new object[] { playersName });
            }
            else
            {
                 listPlayers.Items.Add(playersName);       
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            // 点击登录，玩家直接登录，
            // 如果要做用户名和密码的话，则传递user和pwd给connect函数
            // 通过返回结果 判断是进去下一个界面还是让用户重新登录
           // this.ControlBox = false;

            if (txtUserName.Text.Trim().Equals(""))
            {
                MessageBox.Show("谁都可以登录，但也不要输入为空嘛！");
            }
            
            else if (this.mainLogic.PlayerLogin(this.txtUserName.Text,
                                           this.txtPwd.Text))
            {
                Thread.Sleep(300);
                this.panelLogin.Hide();
                this.Text = "消灭对对游戏大厅 | " + "欢迎您：" + this.txtUserName.Text;
                
                this.panelTables.Show();
                this.panelPlayers.Show();
            }
        }
        //用户坐下，调用gametable显示
        public void PlayerSeatDown(int tableIdx, int SeatIdx, string user)
        {
            if (tableIdx < 0 || tableIdx >= tables.Count)
            {
                return;
            }

            tables[tableIdx].PlayerEnter(SeatIdx, user);
        }
        //用户离开桌子
        public void PlayerLeaveTable(int tableIdx, int SeatIdx, string user)
        {
            if (tableIdx < 0 || tableIdx >= tables.Count)
            {
                return;
            }

            tables[tableIdx].PlayerLeave(SeatIdx, user);
        }
        //该桌子开始游戏
        public void TableGameOn(int tableIdx)
        {
            if (tableIdx < 0 || tableIdx >= tables.Count)
            {
                return;
            }
            tables[tableIdx].GameOn();
        }
        //桌子游戏结束
        public void TableGameOver(int tableIdx)
        {
            if (tableIdx < 0 || tableIdx >= tables.Count)
            {
                return;
            }
            tables[tableIdx].GameOver();
        }

        private void hallexit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("真的要退出游戏吗？", "提示消息！", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                btnClickEvent(sender, e);
            }
        }

        private void Room_FormClosing(object sender, FormClosingEventArgs e)
        {         
                btnClickEvent(sender, e);           
        }
           

        


    }
}
