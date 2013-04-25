namespace CSharpGame
{
    partial class gameTable
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableNum = new System.Windows.Forms.Label();
            this.seatFour = new System.Windows.Forms.Button();
            this.seatThree = new System.Windows.Forms.Button();
            this.seatTwo = new System.Windows.Forms.Button();
            this.seatOne = new System.Windows.Forms.Button();
            this.gamestartlabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tableNum
            // 
            this.tableNum.AutoSize = true;
            this.tableNum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tableNum.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableNum.Location = new System.Drawing.Point(48, 50);
            this.tableNum.Name = "tableNum";
            this.tableNum.Size = new System.Drawing.Size(20, 22);
            this.tableNum.TabIndex = 0;
            this.tableNum.Text = "1";
            // 
            // seatFour
            // 
            this.seatFour.BackColor = System.Drawing.Color.Tan;
            this.seatFour.Location = new System.Drawing.Point(0, 43);
            this.seatFour.Name = "seatFour";
            this.seatFour.Size = new System.Drawing.Size(36, 36);
            this.seatFour.TabIndex = 4;
            this.seatFour.UseVisualStyleBackColor = false;
            this.seatFour.Click += new System.EventHandler(this.seatOne_Click);
            // 
            // seatThree
            // 
            this.seatThree.BackColor = System.Drawing.Color.Tan;
            this.seatThree.Location = new System.Drawing.Point(40, 83);
            this.seatThree.Name = "seatThree";
            this.seatThree.Size = new System.Drawing.Size(36, 36);
            this.seatThree.TabIndex = 3;
            this.seatThree.UseVisualStyleBackColor = false;
            this.seatThree.Click += new System.EventHandler(this.seatOne_Click);
            // 
            // seatTwo
            // 
            this.seatTwo.BackColor = System.Drawing.Color.Tan;
            this.seatTwo.Location = new System.Drawing.Point(81, 43);
            this.seatTwo.Name = "seatTwo";
            this.seatTwo.Size = new System.Drawing.Size(36, 36);
            this.seatTwo.TabIndex = 2;
            this.seatTwo.UseVisualStyleBackColor = false;
            this.seatTwo.Click += new System.EventHandler(this.seatOne_Click);
            // 
            // seatOne
            // 
            this.seatOne.BackColor = System.Drawing.Color.Tan;
            this.seatOne.Location = new System.Drawing.Point(41, 0);
            this.seatOne.Name = "seatOne";
            this.seatOne.Size = new System.Drawing.Size(36, 36);
            this.seatOne.TabIndex = 1;
            this.seatOne.UseVisualStyleBackColor = false;
            this.seatOne.Click += new System.EventHandler(this.seatOne_Click);
            // 
            // gamestartlabel
            // 
            this.gamestartlabel.Font = new System.Drawing.Font("微软雅黑", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gamestartlabel.Location = new System.Drawing.Point(42, 52);
            this.gamestartlabel.Name = "gamestartlabel";
            this.gamestartlabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gamestartlabel.Size = new System.Drawing.Size(35, 29);
            this.gamestartlabel.TabIndex = 5;
            this.gamestartlabel.Text = "已开始";
            // 
            // gameTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.gamestartlabel);
            this.Controls.Add(this.seatFour);
            this.Controls.Add(this.seatThree);
            this.Controls.Add(this.seatTwo);
            this.Controls.Add(this.seatOne);
            this.Controls.Add(this.tableNum);
            this.Name = "gameTable";
            this.Size = new System.Drawing.Size(122, 122);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label tableNum;
        private System.Windows.Forms.Button seatOne;
        private System.Windows.Forms.Button seatTwo;
        private System.Windows.Forms.Button seatThree;
        private System.Windows.Forms.Button seatFour;
        private System.Windows.Forms.Label gamestartlabel;
    }
}
