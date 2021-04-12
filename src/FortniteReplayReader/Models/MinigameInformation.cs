using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteReplayReader.Models
{
    public class MinigameInformation
    {
        public float TimeLimit { get; set; }
        public float TotalRounds { get; set; }
        public float WarmupDuration { get; set; }
        public float GameResetDelay { get; set; }
        public float ScoreDisplayTime { get; set; }
        public float WinnerDisplayTime { get; set; }

        public List<GameRound> Rounds { get; internal set; } = new List<GameRound>();
    }
}
