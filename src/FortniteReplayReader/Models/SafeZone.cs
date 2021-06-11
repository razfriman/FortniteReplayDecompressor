using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class SafeZone
    {
        public float Radius { get; internal set; }
        public float ShrinkStartTime { get; internal set; }
        public float ShringEndTime { get; internal set; }
        public float CurrentRadius { get; internal set; }

        public float NextRadius { get; internal set; }
        public float NextNextRadius { get; internal set; }
        public FVector? LastCenter { get; internal set; }
        public FVector? NextCenter { get; internal set; }
        public FVector? NextNextCenter { get; internal set; }
    }
}
