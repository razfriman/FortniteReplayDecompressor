using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class SafeZone
    {
        public float Radius { get; set; }
        public float ShrinkStartTime { get; set; }
        public float ShringEndTime { get; set; }

        public float NextRadius { get; set; }
        public float NextNextRadius { get; set; }
        public FVector LastCenter { get; set; }
        public FVector NextCenter { get; set; }
        public FVector NextNextCenter { get; set; }
    }
}
