using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports.Sets;

namespace FortniteReplayReader.Models
{
    public class HealthUpdate
    {
        public HealthSet Health { get; internal set; }
        public float DeltaGameTimeSeconds { get; internal set; }
    }
}
