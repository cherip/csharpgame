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
            this.startBtn = new System.Windows.Forms.Button();
            this.picList = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.hintbtn = new System.Windows.Forms.Button();
            this.timeElapseBar = new System.Windows.Forms.ProgressBar();
            this.connBtn = new System.Windows.Forms.Button();
            this.lable1 = new System.Windows.Forms.Label();
            this.namlable = new System.Windows.Forms.Label();
            this.logoutBtn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(712, 579);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(75, 23);
            this.startBtn.TabIndex = 1;
            this.startBtn.TabStop = false;
            this.startBtn.Text = "开始";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.button_start);
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
            this.label1.Location = new System.Drawing.Point(687, 182);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "游戏副数：";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(758, 180);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(29, 16);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "1";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioClicked);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(793, 180);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(29, 16);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "3";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioClicked);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(828, 180);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(29, 16);
            this.radioButton3.TabIndex = 3;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "5";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioClicked);
            // 
            // hintbtn
            // 
            this.hintbtn.Location = new System.Drawing.Point(813, 579);
            this.hintbtn.Name = "hintbtn";
            this.hintbtn.Size = new System.Drawing.Size(75, 23);
            this.hintbtn.TabIndex = 4;
            this.hintbtn.TabStop = false;
            this.hintbtn.Text = "提示";
            this.hintbtn.UseVisualStyleBackColor = true;
            this.hintbtn.Click += new System.EventHandler(this.hintclicked);
            // 
            // timeElapseBar
            // 
            this.timeElapseBar.Location = new System.Drawing.Point(3, 218);
            this.timeElapseBar.Name = "timeElapseBar";
            this.timeElapseBar.Size = new System.Drawing.Size(668, 14);
            this.timeElapseBar.TabIndex = 5;
            // 
            // connBtn
            // 
            this.connBtn.Location = new System.Drawing.Point(712, 397);
            this.connBtn.Name = "connBtn";
            this.connBtn.Size = new System.Drawing.Size(75, 23);
            this.connBtn.TabIndex = 6;
            this.connBtn.Text = "连接";
            this.connBtn.UseVisualStyleBackColor = true;
            this.connBtn.Click += new System.EventHandler(this.conn_Click);
            // 
            // lable1
            // 
            this.lable1.AutoSize = true;
            this.lable1.Location = new System.Drawing.Point(677, 344);
            this.lable1.Name = "lable1";
            this.lable1.Size = new System.Drawing.Size(41, 12);
            this.lable1.TabIndex = 7;
            this.lable1.Text = "姓名：";
            // 
            // namlable
            // 
            this.namlable.AutoSize = true;
            this.namlable.Location = new System.Drawing.Point(724, 344);
            this.namlable.Name = "namlable";
            this.namlable.Size = new System.Drawing.Size(41, 12);
            this.namlable.TabIndex = 8;
            this.namlable.Text = "label2";
            // 
            // logoutBtn
            // 
            this.logoutBtn.Location = new System.Drawing.Point(811, 397);
            this.logoutBtn.Name = "logoutBtn";
            this.logoutBtn.Size = new System.Drawing.Size(75, 23);
            this.logoutBtn.TabIndex = 9;
            this.logoutBtn.Text = "退出";
            this.logoutBtn.UseVisualStyleBackColor = true;
            this.logoutBtn.Click += new System.EventHandler(this.logoutBtn_Click);
            // 
            // listView1
            // 
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(712, 426);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(176, 128);
            this.listView1.TabIndex = 10;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(813, 369);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "邀请";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(712, 370);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 12;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(712, 295);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "准备";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(811, 295);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "对战";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // CSharpGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 782);
            this.Controls.Add(this.timeElapseBar);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.logoutBtn);
            this.Controls.Add(this.namlable);
            this.Controls.Add(this.lable1);
            this.Controls.Add(this.connBtn);
            this.Controls.Add(this.hintbtn);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startBtn);
            this.Name = "CSharpGame";
            this.Text = "找对对";
            this.Load += new System.EventHandler(this.CSharpGame_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.ImageList picList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Button hintbtn;
        private System.Windows.Forms.ProgressBar timeElapseBar;
        private System.Windows.Forms.Button connBtn;
        private System.Windows.Forms.Label lable1;
        private System.Windows.Forms.Label namlable;
        private System.Windows.Forms.Button logoutBtn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

