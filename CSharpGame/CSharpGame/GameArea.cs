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
    class GameArea: Panel
    {
        Hashtable btnVal;
        Button[] btnArry;
        Logic myLogic;
        bool gameStart;
        public System.Windows.Forms.ImageList picList;

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
            
            this.Location = areaLocat;
            this.Size = areaSize;
            createButton(areaSize);

            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TabIndex = 0;
            this.Enabled = false;

            // 处理逻辑关系
            myLogic = new Logic(this);
            myLogic.btnImageSetFunc += SetBtnImage;
            myLogic.startGame += EnableArea;
            myLogic.cleanBtnPair += CleanBtnPair;

            gameStart = false;
            //myLogic.InitLogic();
        }

        // 废函数
        public bool GameStart(int[] gameReset)
        {
//            myLogic.btnImageSetFunc += SetBtnImage;
            myLogic.InitGame(gameReset);
//            myLogic.btnImageSetFunc -= SetBtnImage;
            return true;
        }

        private bool SetBtnImage(int idx, int type)
        {
            if (idx < 0 || idx >= btnArry.Length)
            {
                return false;
            }

            if (type < 0 || type >= picList.Images.Count)
            {
                return false;
            }

            Button btn = btnArry[idx];
            btn.BackgroundImage = picList.Images[type];

            return true;
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
            // logic会回调cleanBtnPair 完成界面的更新 
            //

            Button curr_click = (Button)sender;
            int pos = (int)btnVal[curr_click];
            myLogic.PushButton(pos);

            //int[] ret = myLogic.PushButton(pos);
            //if (ret != null)
            //{
            //    CleanBtnPair(ret[0], ret[1]);
            //    pairBingo(sender, e);
            //}
        }

        public void CleanBtnPair(int a, int b)
        {
            btnArry[a].Visible = false;
            btnArry[b].Visible = false;
        }

        public void connect()
        {
            myLogic.ConnectNet();
        }

        public delegate void EnableDeleg();

        public void EnableArea()
        {
            if (this.InvokeRequired)
            {
                EnableDeleg ld = new EnableDeleg(EnableArea);
                this.Invoke(ld, null);
            }
            else
            {
                this.Enabled = true;
            }
        }

        //private void pairBingo(object sender, EventArgs e)    //两张图一样时，触发事件
        //{
        //    //
        //    // 利用logic 完成逻辑判断，form只完成显示
        //    //
        //    //联机
        //    if (myLogic.keepalive)
        //    {
        //        myLogic.sendGameData();
        //    }

        //    int state = myLogic.ClearAnPair();

        //    if (state == 2)
        //    {
        //        MessageBox.Show("Win!");
        //        myLogic.started = false;
        //        //startBtn.Enabled = true;
        //    }
        //    else if (state == 1)
        //    {
        //        //button_start(null, e);
        //    }
        //}
    }
}
