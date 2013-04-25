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
        public string userSender;
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

    [Serializable]
    public class MsgGame 
    {
        public string userName;
        public int[] cleanPair;
        public int seatIdx;
        public int tableIdx;
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
        Login,
        Judge,
        Seat,
        Table,
        TableNoSeat,
        FreshGameArea,
        //EnterRoom,
        LeaveRoom,
        GameOver,
        GameOn,
        NewRound,
        ReadyMsg,
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
