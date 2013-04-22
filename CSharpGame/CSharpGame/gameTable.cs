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

        private int tableIdx;

        public gameTable()
        {
            InitializeComponent();
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
        }

        // 提供给外面的 控制seat状态的方法
        // 不会触发 callback 行为
        public void ClickSeat(int idx)
        {
            // 不好的地方，用idx 控制一个table下的4个seat
            EventArgs param = null;
            switch (idx)
            {
                    // 处理seatOne
                case 0:
                    {
                        //seatOne_Click(this.seatOne, param);
                        this.Invoke(new EventHandler(changeSeatStatus), 
                                    new object[] { this.seatOne, param });
                    }
                    break;
                case 1:         // 处理seattwo
                    {
                        this.Invoke(new EventHandler(changeSeatStatus),
                                    new object[] { this.seatTwo, param });
                    }
                    break;
                case 2:         // 处理seatthree
                    {
                        this.Invoke(new EventHandler(changeSeatStatus),
                                    new object[] { this.seatThree, param });
                    }
                    break;
                case 3:         // 处理seatfour
                    {
                        this.Invoke(new EventHandler(changeSeatStatus),
                                    new object[] { this.seatFour, param });
                    }
                    break;
            }
        }

        private void changeSeatStatus(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Enabled == true)
            {
                btn.Enabled = false;
                btn.BackColor = System.Drawing.Color.WhiteSmoke;
            }
            else
            {
                btn.Enabled = true;
                btn.BackColor = System.Drawing.Color.Silver;
            }
        }

        private void seatOne_Click(object sender, EventArgs e)
        {
            // 首先改变 seat的状态
            changeSeatStatus(sender, e);
            // 然后再触发callback函数，通知上层用户点击了seat
            if (clickSeatCallback != null)
            {
                clickSeatCallback(tableIdx, (int)btnIdx[sender]);
            }
        }
    }
}
