using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaWars_try2
{
    [Serializable]
    public class GameSaveData
    {
        public int PlayerScore { get; set; }
        public int BotScore { get; set; }
        public TimeSpan BestTime { get; set; }

        public GameSaveData(int playerScore, int botScore, TimeSpan bestTime)
        {
            PlayerScore = playerScore;
            BotScore = botScore;
            BestTime = bestTime;    
        }
    }
}
