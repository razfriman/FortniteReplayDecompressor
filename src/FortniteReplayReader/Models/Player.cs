using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class Player
    {
        public string EpicId { get; set; }
        public int Teamindex { get; set; }
        public bool IsBot { get; set; }
        public int Level { get; set; }

        //Extended information
        public List<PlayerLocation> Locations { get; private set; } = new List<PlayerLocation>();
    }

    public class PlayerLocation
    {
        public FVector Location { get; set; }
        public float? WorldTime { get; set; }
    }

    public class PlayerStateChange
    {
        public bool WasDowned { get; set; }
        public bool WasKilled { get; set; }
        public float? WorldTime { get; set; }
    }
}
