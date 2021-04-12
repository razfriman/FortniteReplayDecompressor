using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteReplayReader.Models
{
    public enum RoundState { 
        Initializing = 2, 
        WaitingForPlayers = 3,
        Warmup = 4,
        RoundPlaying = 5,
        RoundOver = 6,

        GameoverScoreboard = 8,
        StartingNewRound = 10
    };

    public class MinigameInformation
    {
        public float TimeLimit { get; set; }
        public float TotalRounds { get; set; }
        public float WarmupDuration { get; set; }
        public float GameResetDelay { get; set; }
        public float RoundScoreDisplayTime { get; set; }
        public float GameScoreDisplayTime { get; set; }
        public float WinnerDisplayTime { get; set; }
        /// <summary>
        /// Should be an enum. Someone can manually figure the values out
        /// </summary>
        public int WinCondition { get; set; }

        public List<GameRound> Rounds { get; internal set; } = new List<GameRound>();

        internal RoundTeam[] TeamInfo { get; set; }
        internal RoundState State { get; set; }
        internal PlayerBucket[] PlayerBuckets { get; set; }
        internal HashSet<int> CurrentRoundTeams { get; set; } = new HashSet<int>();
        internal uint CurrentRound { get; set; } = 1;

        internal class PlayerBucket
        {
            public int TeamIndex { get; set; }
            public RoundTeam Team { get; set; }
        }
    }
}
