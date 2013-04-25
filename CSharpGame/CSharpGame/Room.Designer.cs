namespace CSharpGame
{
    partial class Room
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelLogin = new System.Windows.Forms.Panel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.lblPwd = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.panelTables = new System.Windows.Forms.Panel();
            this.panelPlayers = new System.Windows.Forms.Panel();
            this.hallexit = new System.Windows.Forms.Button();
            this.listPlayers = new System.Windows.Forms.ListBox();
            this.lblPlayersList = new System.Windows.Forms.Label();
            this.panelLogin.SuspendLayout();
            this.panelPlayers.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLogin
            // 
            this.panelLogin.Controls.Add(this.btnLogin);
            this.panelLogin.Controls.Add(this.txtPwd);
            this.panelLogin.Controls.Add(this.lblPwd);
            this.panelLogin.Controls.Add(this.lblUserName);
            this.panelLogin.Controls.Add(this.txtUserName);
            this.panelLogin.Location = new System.Drawing.Point(423, 12);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(192, 115);
            this.panelLogin.TabIndex = 0;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(65, 82);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(54, 55);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(130, 21);
            this.txtPwd.TabIndex = 3;
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Location = new System.Drawing.Point(7, 58);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(29, 12);
            this.lblPwd.TabIndex = 2;
            this.lblPwd.Text = "密码";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(7, 27);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(41, 12);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "用户名";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(54, 24);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(130, 21);
            this.txtUserName.TabIndex = 0;
            // 
            // panelTables
            // 
            this.panelTables.Location = new System.Drawing.Point(2, 12);
            this.panelTables.Name = "panelTables";
            this.panelTables.Size = new System.Drawing.Size(419, 406);
            this.panelTables.TabIndex = 1;
            // 
            // panelPlayers
            // 
            this.panelPlayers.Controls.Add(this.hallexit);
            this.panelPlayers.Controls.Add(this.listPlayers);
            this.panelPlayers.Controls.Add(this.lblPlayersList);
            this.panelPlayers.Location = new System.Drawing.Point(424, 12);
            this.panelPlayers.Name = "panelPlayers";
            this.panelPlayers.Size = new System.Drawing.Size(191, 405);
            this.panelPlayers.TabIndex = 2;
            // 
            // hallexit
            // 
            this.hallexit.Location = new System.Drawing.Point(54, 340);
            this.hallexit.Name = "hallexit";
            this.hallexit.Size = new System.Drawing.Size(75, 23);
            this.hallexit.TabIndex = 2;
            this.hallexit.Text = "退出";
            this.hallexit.UseVisualStyleBackColor = true;
            this.hallexit.Click += new System.EventHandler(this.hallexit_Click);
            // 
            // listPlayers
            // 
            this.listPlayers.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.listPlayers.FormattingEnabled = true;
            this.listPlayers.ItemHeight = 12;
            this.listPlayers.Location = new System.Drawing.Point(9, 32);
            this.listPlayers.Name = "listPlayers";
            this.listPlayers.Size = new System.Drawing.Size(175, 268);
            this.listPlayers.TabIndex = 1;
            // 
            // lblPlayersList
            // 
            this.lblPlayersList.AutoSize = true;
            this.lblPlayersList.Location = new System.Drawing.Point(7, 12);
            this.lblPlayersList.Name = "lblPlayersList";
            this.lblPlayersList.Size = new System.Drawing.Size(53, 12);
            this.lblPlayersList.TabIndex = 0;
            this.lblPlayersList.Text = "在线玩家";
            // 
            // Room
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 430);
            this.Controls.Add(this.panelLogin);
            this.Controls.Add(this.panelPlayers);
            this.Controls.Add(this.panelTables);
            this.Name = "Room";
            this.Text = "联机版找对对";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Room_FormClosing);
            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            this.panelPlayers.ResumeLayout(false);
            this.panelPlayers.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLogin;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Label lblPwd;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Panel panelTables;
        private System.Windows.Forms.Panel panelPlayers;
        private System.Windows.Forms.ListBox listPlayers;
        private System.Windows.Forms.Label lblPlayersList;
        private System.Windows.Forms.Button hallexit;

    }
}