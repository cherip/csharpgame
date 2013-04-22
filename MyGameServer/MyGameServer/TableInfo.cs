using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGameServer
{
    class TableInfo
    {
        public bool[] seats;
        public TableInfo()
        {
            seats = new bool[4]{true,true,true,true};//初始化都能坐
            
        }
    }
}
