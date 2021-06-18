using FortniteReplayReader.Models.NetFieldExports.Enums;
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
        public float RoundScoreDisplayTime { get; set; }
        public float RoundWinnerDisplayTime { get; set; }
        public float GameScoreDisplayTime { get; set; }
        public float WinnerDisplayTime { get; set; }
        /// <summary>
        /// Should be an enum. Someone can manually figure the values out
        /// </summary>
        public EMinigameWinCondition WinCondition { get; set; } = EMinigameWinCondition.EMinigameWinCondition_MAX;

        public List<GameRound> Rounds { get; internal set; } = new List<GameRound>();

        internal RoundTeam[] TeamInfo { get; set; }
        internal EFortMinigameState State { get; set; }
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
