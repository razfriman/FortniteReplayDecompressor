using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches
{
    [NetFieldExportRPC("PlayerPawn_Athena_C_ClassNetCache")]
    public class PlayerPawnCache
    {
        [NetFieldExportRPCProperty("FastSharedReplication", "/Script/FortniteGame.FortPlayerPawnAthena:FastSharedReplication")]
        public object FastSharedReplication { get; set; }

        [NetFieldExportRPCProperty("ClientObservedStats", "/Script/FortniteGame.FortClientObservedStat", false)]
        public object ClientObservedStats { get; set; }

        [NetFieldExportRPCProperty("NetMulticast_Athena_BatchedDamageCues", "/Script/FortniteGame.FortPawn:NetMulticast_Athena_BatchedDamageCues")]
        public object BatchedDamage { get; set; }
    }

    [NetFieldExportRPC("BP_PlayerPawn_Athena_Phoebe_C_ClassNetCache")]
    public class AIPlayerPawnCache : PlayerPawnCache
    {
    }
}
