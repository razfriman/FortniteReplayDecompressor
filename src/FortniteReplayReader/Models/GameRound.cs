using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteReplayReader.Models
{
    public class GameRound
    {
        public float RoundTime => DeltaEndTime - DeltaStartTime;
        public float DeltaStartTime { get; set; }
        public float DeltaEndTime { get; set; }
        public ICollection<RoundTeam> Teams { get; set; }

    }
}
