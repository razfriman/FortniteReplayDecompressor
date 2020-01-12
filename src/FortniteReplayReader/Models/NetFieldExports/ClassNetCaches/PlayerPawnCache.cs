using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches
{
    [NetFieldExportRPC("PlayerPawn_Athena_C_ClassNetCache", ParseType.Normal)]
    public class PlayerPawnCache
    {
        [NetFieldExportRPCProperty("FastSharedReplication", "/Script/FortniteGame.FortPlayerPawnAthena:FastSharedReplication")]
        public object FastSharedReplication { get; set; }

        [NetFieldExportRPCProperty("ClientObservedStats", "/Script/FortniteGame.FortClientObservedStat", false)]
        public object ClientObservedStats { get; set; }

        [NetFieldExportRPCProperty("NetMulticast_Athena_BatchedDamageCues", "/Script/FortniteGame.FortPawn:NetMulticast_Athena_BatchedDamageCues")]
        public object BatchedDamage { get; set; }

        [NetFieldExportRPCProperty("NetMulticast_InvokeGameplayCueAdded_WithParams", "/Script/FortniteGame.FortPawn:NetMulticast_InvokeGameplayCueAdded_WithParams")]
        public object NetMulticastInvokeGameplayCueAddedWithParams { get; set; }

        [NetFieldExportRPCProperty("NetMulticast_InvokeGameplayCueExecuted_WithParams", "/Script/FortniteGame.FortPawn:NetMulticast_InvokeGameplayCueExecuted_WithParams")]
        public object NetMulticastInvokeGameplayCueExecutedWithParams { get; set; }

        [NetFieldExportRPCProperty("NetMulticast_InvokeGameplayCueExecuted_FromSpec", "/Script/FortniteGame.FortPawn:NetMulticast_InvokeGameplayCueExecuted_FromSpec")]
        public object NetMulticastInvokeGameplayCueExecuted { get; set; }
    }

    [NetFieldExportRPC("BP_PlayerPawn_Athena_Phoebe_C_ClassNetCache", ParseType.Normal)]
    public class AIPlayerPawnCache : PlayerPawnCache
    {
    }
}
