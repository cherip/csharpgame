﻿using System;
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
    class GameArea : Panel
    {
        protected Hashtable btnVal;
        protected Button[] btnArry;
        public ImageList picList;

        public GameArea()
        {
            btnArry = new Button[64];
            btnVal = new Hashtable();
        }

        //在panel中绘制游戏button
        public void CreateBtn(Point local, Size areaSize)
        {
            int margin_x = (int)(areaSize.Width * 0.02 / 2);
            int margin_y = (int)(areaSize.Height * 0.02 / 2);

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

                local_x += btn_margin_x + local.X;
                local_y += btn_margin_y + local.Y;

                buttonNew.Location = new System.Drawing.Point(local_x, local_y);

                
                buttonNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;

                buttonNew.Name = "button" + System.Convert.ToString(i);
                buttonNew.Size = new System.Drawing.Size(w - btn_margin_x * 2, h - btn_margin_y * 2);
                buttonNew.TabIndex = 0;
                buttonNew.TabStop = false;
                buttonNew.UseVisualStyleBackColor = true;

                //buttonNew.Click += new System.EventHandler(this.picBtn_Clicked);
                
                btnVal.Add(buttonNew, i);
                btnArry[i] = buttonNew;
                this.Controls.Add(buttonNew);
            }
        }

        public delegate void resetFunc();
        public void Reset()//显示button
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new resetFunc(Reset));
            }
            else
            {
                foreach (Button b in btnArry)
                {
                    if (b.Image != null)
                    {
                        b.Image = null;
                    }
                    b.Visible = true;
                }
            }
        }

        public void UnGameStatus()//设置 没开始游戏状态
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new resetFunc(UnGameStatus));
            }
            else
            {
                foreach (Button b in btnArry)
                {
                    b.Visible = false;
                }
            }
        }

        public delegate void CleanBtnPairDele(int a, int b);
        public virtual void CleanBtnPair(int a, int b)//给LOGIC调用的方法，消除一对
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CleanBtnPairDele(CleanBtnPair), new object[] { a, b });
            }
            else
            {
                btnArry[a].Visible = false;
                btnArry[b].Visible = false;
            }
        }

        public bool SetBtnImage(int idx, int type)
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

        public virtual void Draw(Point local, Size size)
        {
            this.Location = local;
            this.Size = size;

            //this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TabIndex = 0;
            this.Enabled = false;
        }

        public virtual void EnableArea()
        {
            ;
        }
        public virtual void HintBlick(int pos_a, int pos_b, int times)
        {
            ;
        }

        public virtual void UpdateUser(string name)
        {
            ;
        }

        public virtual void ResetGameStatus()
        {
            ;
        }

        public virtual void ShowSameList(List<int> l)
        {
            ;
        }
    }

    class MyGameArea : GameArea//我的游戏区域
    {
        public delegate void btnClick(int idx);
        public event btnClick btnClickEvent;

        public MyGameArea()
            : base()
        {
            ;
        }

        public override void Draw(Point local, Size size)
        {
            base.Draw(local, size);
           
            // 本地的btn显示界面上面没有多的显示信息，直接用全部区域
            CreateBtn(new Point(0,0), size);
            
            foreach (Button b in btnArry)
            {
                b.Click += new EventHandler(picBtn_Clicked);
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

            btnClickEvent(pos);
        }

        public delegate void EnableDeleg();
        public override void EnableArea()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EnableDeleg(EnableArea), null);
            }
            else
            {
                this.Enabled = true;
            }
        }

        public override void HintBlick(int pos_a, int pos_b, int times)//提示时闪烁
        {
            for (int i = 0; i < times; i++)
            {
                btnArry[pos_a].Visible = false;
                btnArry[pos_b].Visible = false;
                Thread.Sleep(100);
                btnArry[pos_a].Visible = true;
                btnArry[pos_b].Visible = true;
                this.Refresh();
                Thread.Sleep(100);
            }
        }

        public override void ShowSameList(List<int> l)//找到相同图标后显示前景图片
        {
            base.ShowSameList(l);
            for (int i = 0; i < 64; i++ )
            {
                if (btnArry[i].Image != null)
                {
                    btnArry[i].Image = null;
                }
                
            }
            for (int i = 0; i < l.Count; i ++ )
            {
                btnArry[l[i]].Image = global::CSharpGame.Properties.Resources.yes;
            }
        }

    }

    class OtherGameArea : GameArea//对手的游戏区域
    {
        Label userName;
        Label gameStatus;

        public OtherGameArea()
            : base()
        {
            ;
        }

        public override void Draw(Point local, Size size)
        {
            base.Draw(local, size);
            
            userName = createLable(new Point(10, 0), new Size(70, 10), "userxxx");
            userName.ForeColor = Color.Black;
            userName.Font = new System.Drawing.Font("微软雅黑", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            gameStatus = createLable(new Point(85, 0), new Size(80, 10), "");

            CreateBtn(new Point(0, 12), new Size(size.Width, size.Height - 12));


            this.Controls.Add(userName);
            this.Controls.Add(gameStatus);
        }

        private Label createLable(Point p, Size s, string txt)
        {
            Label lbl = new Label();
            lbl.Font = new Font("微软雅黑", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            
            lbl.Location = p;
            lbl.Size = s;
            lbl.Text = txt;
            lbl.ForeColor = System.Drawing.Color.Black;
            return lbl;
        }

        private void UpdateStatus()
        {
        }

        public override void CleanBtnPair(int a, int b)
        {
            base.CleanBtnPair(a, b);
            UpdateStatus();
        }

        public override void UpdateUser(string name)
        {
            base.UpdateUser(name);
            this.userName.Text = "玩家：" + name;
//            this.Refresh();
        }

        public override void ResetGameStatus()
        {
            base.ResetGameStatus();
            
  
           // this.gameStatus.Text = "64/64";
            
//            this.Refresh();
        }
    }
}
