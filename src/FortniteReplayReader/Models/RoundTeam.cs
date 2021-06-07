using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteReplayReader.Models
{
    public class RoundTeam
    {
        public int MaxSize { get; set; }
        public int CurrentTeamSize { get; set; }
        public int TeamIndex { get; set; }
        public uint TeamColorIndex { get; set; }

        public int Eliminations { get; set; }

        public RoundPlayer[] PlayerIds { get; set; }

        public RoundTeam()
        {

        }

        public RoundTeam(RoundTeam team)
        {
            MaxSize = team.MaxSize;
            CurrentTeamSize = team.CurrentTeamSize;
            TeamIndex = team.TeamIndex;
            TeamColorIndex = team.TeamColorIndex;
            Eliminations = team.Eliminations;
            PlayerIds = team.PlayerIds?.Select(x => new RoundPlayer(x)).ToArray();

            if (PlayerIds != null)
            {
                for (int i = 0; i < PlayerIds.Length; i++)
                {
                    if (PlayerIds[i].HasLeft == true)
                    {
                        PlayerIds[i] = null;
                    }
                }
            }
        }
    }

    public class RoundPlayer
    {
        public Player Player { get; set; }
        public string PlayerId { get; set; }
        public bool HasLeft { get; set; }
        public int PlayerIndex { get; set; }

        public RoundPlayer()
        {

        }

        public RoundPlayer(RoundPlayer player)
        {
            Player = player?.Player;
            PlayerId = player?.PlayerId;
            HasLeft = player?.HasLeft ?? false;
            PlayerIndex = player?.PlayerIndex ?? 0;
        }
    }
}
