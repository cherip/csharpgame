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

        public List<int[]> gameResetStatus;
        public List<int> gameAnswer;
        public int[] gameRound = new int[] { 0, 0, 0, 0 };
        public int[] pairClean = new int[] { 0, 0, 0, 0 };

        public int totalRound;

        public TableInfo()
        {
            seats = new bool[4]{true,true,true,true};//初始化都能坐
            tabelEable = true;
            usercount = 0;
            readycount = 0;

            totalRound = 0;
        }

        public void GameStart()
        {
            for (int i = 0; i < 4; i++)
            {
                gameRound[i] = 0;
                pairClean[i] = 0;
            }
        }

        public int PlayerCleanPair(int seatIdx)
        {
            pairClean[seatIdx]++;
            if (pairClean[seatIdx] == gameAnswer[gameRound[seatIdx]])
            {
                gameRound[seatIdx]++;
                if (gameRound[seatIdx] == gameAnswer.Count)
                {
                    // 表示改玩家赢了
                    return 2;
                }
                else
                {
                    pairClean[seatIdx] = 0;
                    return 1;
                }
                
            }
            return 0;
        }

        public int[] GetGameForPlayer(int seatIdx)
        {
            return gameResetStatus[gameRound[seatIdx]];
        }
    }
}
