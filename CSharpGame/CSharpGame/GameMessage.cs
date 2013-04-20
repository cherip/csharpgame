using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpGame
{
    class GameMessage
    {
 
        private int messageType;//1为系统消息，2为游戏信息      
        private string str_tpye;
      
        private int client_sender;
     
        private int game_info;

        private string[] users;

        private string user;
        public string User
        {
            get { return user; }
            set { user = value; }
        }
        public int Game_info
        {
            get { return game_info; }
            set { game_info = value; }
        }
        public int Client_sender
        {
            get { return client_sender; }
            set { client_sender = value; }
        }
        public int MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

        public string[] Para
        {
            get { return users; }
            set { users = value; }
        }

        public string Str_tpye
        {
            get { return str_tpye; }
            set { str_tpye = value; }
        }
    }
}
