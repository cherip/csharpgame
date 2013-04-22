using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpGame
{
    public enum MsgType
    {
        Sys = 1,
        Game,
        Chat
    }

    public enum ReciveType
    {
        ALL,
        Room,
        Player
    }

    [Serializable]
    public class Message
    {
        public MsgType msgType;
        public object msgContent;
        public ReciveType reciType; 
        public object Num;              // playername or roomidx
        //public MsgSys msgContent;

        public Message()
        {
            // 默认的消息是全局广播消息
            reciType = ReciveType.ALL;
            Num = null;
        }

        public Message(MsgGame msg) 
            : this()
        {
            msgType = MsgType.Game;
            msgContent = msg;
        }

        public Message(MsgChat msg)
            : this()
        {
            msgType = MsgType.Chat;
            msgContent = msg;
        }

        public Message(MsgSys msg)
            : this()
        {
            msgType = MsgType.Sys;
            msgContent = msg;
        }
    }

    class GameMessage
    {
        public MsgType msgType;
        public object msgContent;

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

    [Serializable]
    public class MsgGame 
    {
        public string userName;
        public int[] cleanPair;
        public MsgGame()
        {
            userName = null;
            cleanPair = new int[2];
        }
    }

    [Serializable]
    public class MsgChat
    {
        public List<string> onlineUser;
    }

    public enum MsgSysType
    {
        Begin = 1,
        End,
        Ready,
        Online,
        Offline,
        Join,
        List,
        Exit,
        CanStart,
        GameStart,

    }

    [Serializable]
    public class MsgSys
    {
        public MsgSysType sysType;
        //public int[] gameStart;
        //public string user;
        public object sysContent;
    }
}
