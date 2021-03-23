using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class PrivateTeamInfo
    {
        public float Value { get; set; }
        public string PlayerId { get; set; }
        public Player PlayerState { get; set; }
        public FVector LastLocation { get; set; }
        public float LastYaw { get; set; }
        public int PawnStateMask { get; set; }
    }
}
