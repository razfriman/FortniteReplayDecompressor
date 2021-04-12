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

        public List<RoundPlayer> PlayerIds { get; set; } = new List<RoundPlayer>();

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
            PlayerIds = team.PlayerIds.Where(x => !x.HasLeft).Select(x => new RoundPlayer(x)).ToList();
        }
    }

    public class RoundPlayer
    {
        public Player Player { get; set; }
        public string PlayerId { get; set; }
        public bool HasLeft { get; set; }

        public RoundPlayer()
        {

        }

        public RoundPlayer(RoundPlayer player)
        {
            Player = player.Player;
            PlayerId = player.PlayerId;
            HasLeft = player.HasLeft;
        }
    }
}
