using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGameServer
{
    [Serializable]
    public class TableInfo
    {
        public bool[] seats;
        public bool tabelEable;//游戏如果已经开始，就不激活桌子了
        public int usercount;//桌子上的人数
        public int readycount;//准备了的人数
        public TableInfo()
        {
            seats = new bool[4]{true,true,true,true};//初始化都能坐
            tabelEable = true;
            usercount = 0;
            readycount = 0;
        }
    }
}
