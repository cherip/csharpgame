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
            this.label1.Location = new System.Drawing.Point(616, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "游戏副数：";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(687, 28);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(29, 16);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "1";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioClicked);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(722, 28);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(29, 16);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "2";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioClicked);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(757, 28);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(29, 16);
            this.radioButton3.TabIndex = 3;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "3";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioClicked);
            // 
            // hintbtn
            // 
            this.hintbtn.Location = new System.Drawing.Point(618, 55);
            this.hintbtn.Name = "hintbtn";
            this.hintbtn.Size = new System.Drawing.Size(75, 23);
            this.hintbtn.TabIndex = 4;
            this.hintbtn.TabStop = false;
            this.hintbtn.Text = "提示";
            this.hintbtn.UseVisualStyleBackColor = true;
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
            this.button2.Location = new System.Drawing.Point(618, 84);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "准备";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.Location = new System.Drawing.Point(757, 84);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(75, 23);
            this.exitBtn.TabIndex = 15;
            this.exitBtn.Text = "退出房间";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // sameBtn
            // 
            this.sameBtn.Location = new System.Drawing.Point(757, 55);
            this.sameBtn.Name = "sameBtn";
            this.sameBtn.Size = new System.Drawing.Size(75, 23);
            this.sameBtn.TabIndex = 16;
            this.sameBtn.Text = "显示相同";
            this.sameBtn.UseVisualStyleBackColor = true;
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
            this.radioButton4.Location = new System.Drawing.Point(793, 28);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(29, 16);
            this.radioButton4.TabIndex = 19;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "4";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(828, 28);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(29, 16);
            this.radioButton5.TabIndex = 20;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "5";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // CSharpGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 708);
            this.Controls.Add(this.radioButton5);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.panelOtherArea);
            this.Controls.Add(this.panelMyArea);
            this.Controls.Add(this.sameBtn);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.timeElapseBar);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.hintbtn);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label1);
            this.Name = "CSharpGame";
            this.Text = "找对对";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CSharpGame_FormClosed);
            this.Load += new System.EventHandler(this.CSharpGame_Load);
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
    }
}

