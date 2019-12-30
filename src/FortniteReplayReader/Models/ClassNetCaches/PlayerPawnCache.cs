using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;

namespace FortniteReplayReader.Models.ClassNetCaches
{
    [NetFieldExportRPC("PlayerPawn_Athena_C_ClassNetCache")]
    public class PlayerPawnCache
    {
        [NetFieldExportRPCProperty("ClientObservedStats", "/Script/FortniteGame.FortClientObservedStat", false)]
        public object ClientObservedStats { get; set; }
    }
}
