using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace CSharpGame
{
    public partial class gameTable : UserControl
    {
        public delegate void playerClickSeat(int table, int seat);
        public event playerClickSeat clickSeatCallback;
        Hashtable btnIdx;
        public string[] seatUser = new string[4] { "", "", "", "" };
        public List<Button> seatList;
        private int tableIdx;
        bool Isgaming = false;

        public gameTable()
        {
            InitializeComponent();
            this.gamestartlabel.Visible = false;
        }

        public gameTable(int idx)
            : this()
        {
            this.tableNum.Text = System.Convert.ToString(idx + 1);
            tableIdx = idx;

            btnIdx = new Hashtable();
            btnIdx.Add(this.seatOne, 0);
            btnIdx.Add(this.seatTwo, 1);
            btnIdx.Add(this.seatThree, 2);
            btnIdx.Add(this.seatFour, 3);

            seatList = new List<Button>();
            seatList.Add(this.seatOne);
            seatList.Add(this.seatTwo);
            seatList.Add(this.seatThree);
            seatList.Add(this.seatFour);
        }


        public delegate void logicClickSeat(int seatIdx, string user);
        public void PlayerEnter(int seatIdx, string user)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new logicClickSeat(PlayerEnter),
                            new object[] { seatIdx, user });
            }
            else
            {
                Button btn = this.seatList[seatIdx];
                btn.Enabled = false;
                btn.FlatStyle = FlatStyle.Flat;
                seatUser[seatIdx] = user;
                btn.Text = user;
                btn.BackColor = System.Drawing.Color.WhiteSmoke;
            }
        }

        public void PlayerLeave(int seatIdx, string user)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new logicClickSeat(PlayerLeave),
                            new object[] { seatIdx, user });
            }
            else
            {
                Button btn = this.seatList[seatIdx];
                btn.Text = "";
                btn.BackColor = System.Drawing.Color.Tan;
                btn.FlatStyle = FlatStyle.Standard;
                seatUser[seatIdx] = "";
                if (Isgaming == false)
                    btn.Enabled = true;
            }
        }

        public delegate void gameFunc();
        public void GameOn()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new gameFunc(GameOn));
            }
            else
            {
                foreach (Button c in seatList)
                {
                    c.Enabled = false;
                    c.FlatStyle = FlatStyle.Flat;
                    gamestartlabel.Visible = true;
                }
                Isgaming = true;
            }
        }

        public void GameOver()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new gameFunc(GameOver));
            }
            else
            {
                foreach (Button c in seatList)
                {
                    if (c.Text == "")
                    {
                        c.Enabled = true;
                        c.BackColor = System.Drawing.Color.Tan;
                        c.FlatStyle = FlatStyle.Standard;
                        gamestartlabel.Visible = false;
                    }
                }
                Isgaming = false;
            }
        }

        private void seatOne_Click(object sender, EventArgs e)
        {
            // 然后再触发callback函数，通知上层用户点击了seat
            if (clickSeatCallback != null)
            {
                clickSeatCallback(tableIdx, (int)btnIdx[sender]);
            }
        }
    }
}
