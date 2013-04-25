namespace CSharpGame
{
    partial class CSharpGame
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CSharpGame));
            this.picList = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.hintbtn = new System.Windows.Forms.Button();
            this.timeElapseBar = new System.Windows.Forms.ProgressBar();
            this.button2 = new System.Windows.Forms.Button();
            this.exitBtn = new System.Windows.Forms.Button();
            this.sameBtn = new System.Windows.Forms.Button();
            this.panelMyArea = new System.Windows.Forms.Panel();
            this.panelOtherArea = new System.Windows.Forms.Panel();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picList
            // 
            this.picList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("picList.ImageStream")));
            this.picList.TransparentColor = System.Drawing.Color.Transparent;
            this.picList.Images.SetKeyName(0, "0.png");
            this.picList.Images.SetKeyName(1, "1.png");
            this.picList.Images.SetKeyName(2, "2.png");
            this.picList.Images.SetKeyName(3, "3.png");
            this.picList.Images.SetKeyName(4, "4.png");
            this.picList.Images.SetKeyName(5, "5.png");
            this.picList.Images.SetKeyName(6, "6.png");
            this.picList.Images.SetKeyName(7, "7.png");
            this.picList.Images.SetKeyName(8, "8.png");
            this.picList.Images.SetKeyName(9, "9.png");
            this.picList.Images.SetKeyName(10, "10.png");
            this.picList.Images.SetKeyName(11, "11.png");
            this.picList.Images.SetKeyName(12, "12.png");
            this.picList.Images.SetKeyName(13, "13.png");
            this.picList.Images.SetKeyName(14, "14.png");
            this.picList.Images.SetKeyName(15, "15.png");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Goldenrod;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "游戏副数：";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.BackColor = System.Drawing.Color.Transparent;
            this.radioButton1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton1.ForeColor = System.Drawing.Color.Goldenrod;
            this.radioButton1.Location = new System.Drawing.Point(88, 5);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(33, 21);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "1";
            this.radioButton1.UseVisualStyleBackColor = false;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioClicked);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.BackColor = System.Drawing.Color.Transparent;
            this.radioButton2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton2.ForeColor = System.Drawing.Color.Goldenrod;
            this.radioButton2.Location = new System.Drawing.Point(123, 5);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(33, 21);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "2";
            this.radioButton2.UseVisualStyleBackColor = false;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioClicked);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.BackColor = System.Drawing.Color.Transparent;
            this.radioButton3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton3.ForeColor = System.Drawing.Color.Goldenrod;
            this.radioButton3.Location = new System.Drawing.Point(158, 5);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(33, 21);
            this.radioButton3.TabIndex = 3;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "3";
            this.radioButton3.UseVisualStyleBackColor = false;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioClicked);
            // 
            // hintbtn
            // 
            this.hintbtn.BackColor = System.Drawing.Color.Transparent;
            this.hintbtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.hintbtn.ForeColor = System.Drawing.Color.Goldenrod;
            this.hintbtn.Location = new System.Drawing.Point(5, 38);
            this.hintbtn.Name = "hintbtn";
            this.hintbtn.Size = new System.Drawing.Size(75, 23);
            this.hintbtn.TabIndex = 4;
            this.hintbtn.TabStop = false;
            this.hintbtn.Text = "提示";
            this.hintbtn.UseVisualStyleBackColor = false;
            // 
            // timeElapseBar
            // 
            this.timeElapseBar.Location = new System.Drawing.Point(-83, 646);
            this.timeElapseBar.Name = "timeElapseBar";
            this.timeElapseBar.Size = new System.Drawing.Size(668, 14);
            this.timeElapseBar.TabIndex = 5;
            this.timeElapseBar.Visible = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ForeColor = System.Drawing.Color.Goldenrod;
            this.button2.Location = new System.Drawing.Point(5, 67);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "准备";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.BackColor = System.Drawing.Color.Transparent;
            this.exitBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.exitBtn.ForeColor = System.Drawing.Color.Goldenrod;
            this.exitBtn.Location = new System.Drawing.Point(144, 67);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(75, 23);
            this.exitBtn.TabIndex = 15;
            this.exitBtn.Text = "退出房间";
            this.exitBtn.UseVisualStyleBackColor = false;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // sameBtn
            // 
            this.sameBtn.BackColor = System.Drawing.Color.Transparent;
            this.sameBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.sameBtn.ForeColor = System.Drawing.Color.Goldenrod;
            this.sameBtn.Location = new System.Drawing.Point(144, 38);
            this.sameBtn.Name = "sameBtn";
            this.sameBtn.Size = new System.Drawing.Size(75, 23);
            this.sameBtn.TabIndex = 16;
            this.sameBtn.Text = "显示相同";
            this.sameBtn.UseVisualStyleBackColor = false;
            // 
            // panelMyArea
            // 
            this.panelMyArea.BackColor = System.Drawing.Color.Transparent;
            this.panelMyArea.Location = new System.Drawing.Point(7, 122);
            this.panelMyArea.Name = "panelMyArea";
            this.panelMyArea.Size = new System.Drawing.Size(605, 574);
            this.panelMyArea.TabIndex = 17;
            // 
            // panelOtherArea
            // 
            this.panelOtherArea.BackColor = System.Drawing.Color.Transparent;
            this.panelOtherArea.Location = new System.Drawing.Point(618, 122);
            this.panelOtherArea.Name = "panelOtherArea";
            this.panelOtherArea.Size = new System.Drawing.Size(275, 574);
            this.panelOtherArea.TabIndex = 18;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.BackColor = System.Drawing.Color.Transparent;
            this.radioButton4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton4.ForeColor = System.Drawing.Color.Goldenrod;
            this.radioButton4.Location = new System.Drawing.Point(194, 5);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(33, 21);
            this.radioButton4.TabIndex = 19;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "4";
            this.radioButton4.UseVisualStyleBackColor = false;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.BackColor = System.Drawing.Color.Transparent;
            this.radioButton5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton5.ForeColor = System.Drawing.Color.Goldenrod;
            this.radioButton5.Location = new System.Drawing.Point(229, 5);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(33, 21);
            this.radioButton5.TabIndex = 20;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "5";
            this.radioButton5.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.sameBtn);
            this.panel1.Controls.Add(this.radioButton5);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Controls.Add(this.hintbtn);
            this.panel1.Controls.Add(this.exitBtn);
            this.panel1.Controls.Add(this.radioButton4);
            this.panel1.Controls.Add(this.radioButton3);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Location = new System.Drawing.Point(340, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(272, 104);
            this.panel1.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Goldenrod;
            this.label2.Location = new System.Drawing.Point(729, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 22);
            this.label2.TabIndex = 22;
            this.label2.Text = "击败他们";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Goldenrod;
            this.label3.Location = new System.Drawing.Point(39, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(279, 33);
            this.label3.TabIndex = 23;
            this.label3.Text = "欢迎来到找对对！";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Goldenrod;
            this.label4.Location = new System.Drawing.Point(193, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 22);
            this.label4.TabIndex = 24;
            this.label4.Text = "我的游戏";
            // 
            // CSharpGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(905, 708);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelOtherArea);
            this.Controls.Add(this.panelMyArea);
            this.Controls.Add(this.timeElapseBar);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Yellow;
            this.Name = "CSharpGame";
            this.Text = "找对对";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CSharpGame_FormClosed);
            this.Load += new System.EventHandler(this.CSharpGame_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList picList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Button hintbtn;
        private System.Windows.Forms.ProgressBar timeElapseBar;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.Button sameBtn;
        private System.Windows.Forms.Panel panelMyArea;
        private System.Windows.Forms.Panel panelOtherArea;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

