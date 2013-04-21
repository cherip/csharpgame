using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;

namespace CSharpGame
{
    class GameArea: Panel
    {
        Hashtable btnVal;
        Button[] btnArry;
        Logic myLogic;

        //public delegate void PairBingoHandle(object sender, EventArgs e);//消除两张图代理
        //public event PairBingoHandle pairBingoEvent;

        public GameArea()
        {
            btnArry = new Button[64];
            btnVal = new Hashtable();
            myLogic = new Logic();

            //createButton();
        }

        public GameArea(Point areaLocat, Size areaSize) {
            btnArry = new Button[64];
            btnVal = new Hashtable();
            
            myLogic = new Logic();
            myLogic.InitLogic();

            this.Location = areaLocat;
            this.Size = areaSize;
            createButton(areaSize);

            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //this.Name = "myGameShower";
            this.TabIndex = 0;
            this.Enabled = false;
        }

        private void createButton(Size areaSize)
        {
            int margin_x = (int)(areaSize.Width * 0.05 / 2);
            int margin_y = (int)(areaSize.Height * 0.05 / 2);

            int w = (areaSize.Width - margin_x * 2) / 8;
            int h = (areaSize.Height - margin_y * 2) / 8;

            int btn_margin_x = (int)(w * 0.1) / 2;
            int btn_margin_y = (int)(h * 0.1) / 2;

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CSharpGame));
            for (int i = 0; i < 64; i++)
            {
                System.Windows.Forms.Button buttonNew = new System.Windows.Forms.Button();

                int local_x = margin_x + (w) * (i % 8);
                int local_y = margin_y + (h) * (i / 8);

                local_x += btn_margin_x;
                local_y += btn_margin_y;

                buttonNew.Location = new System.Drawing.Point(local_x, local_y);

                buttonNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
                buttonNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;

                buttonNew.Name = "button" + System.Convert.ToString(i);
                buttonNew.Size = new System.Drawing.Size(w - btn_margin_x * 2, h - btn_margin_y * 2);
                buttonNew.TabIndex = 0;
                buttonNew.TabStop = false;
                buttonNew.UseVisualStyleBackColor = true;
                
                buttonNew.Click += new System.EventHandler(this.picBtn_Clicked);

                btnVal.Add(buttonNew, i);
                btnArry[i] = buttonNew;
                this.Controls.Add(buttonNew);
            }
        }

        private void picBtn_Clicked(object sender, EventArgs e)
        {
            //
            // 利用logic中的PushButton变量来得到是否消除某2个button
            // 但是我那边的logic没有写对，需要返回消除的点对。

            Button curr_click = (Button)sender;
            int pos = (int)btnVal[curr_click];
            int[] ret = myLogic.PushButton(pos);
            if (ret != null)
            {
                CleanPair(ret[0], ret[1]);
                pairBingo(sender, e);
            }
        }

        public void CleanPair(int a, int b)
        {
            btnArry[a].Visible = false;
            btnArry[b].Visible = false;
            //a.Visible = false;
            //b.Visible = false;
        }

        public void connect()
        {
            myLogic.ConnectNet();
        }

        public void logout()
        {
            myLogic.CloseConn(null);
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
                //startBtn.Enabled = true;
            }
            else if (state == 1)
            {
                //button_start(null, e);
            }
        }
    }
}
