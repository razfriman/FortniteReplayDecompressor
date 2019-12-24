using System.Collections.Generic;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class FortniteReplay : Replay
    {
        public IList<PlayerElimination> Eliminations { get; internal set; } = new List<PlayerElimination>();
        public Stats Stats { get; internal set; }
        public TeamStats TeamStats { get; internal set; }
        public GameInformation GameInformation { get; internal set; } = new GameInformation();
    }
}
