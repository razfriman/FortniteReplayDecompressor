using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches
{
    public class Explosive
    {
        [NetFieldExportRPCProperty("BroadcastExplosion", "/Script/FortniteGame.FortGameplayEffectDeliveryActor:BroadcastExplosion")]
        public object BroadcastExplosion { get; set; }
    }

    [NetFieldExportRPC("B_Prj_Athena_FragGrenade_C_ClassNetCache", ParseType.Full)]
    public class FragExplosion : Explosive
    {
    }

    [NetFieldExportRPC("B_Prj_Ranged_Rocket_Athena_C_ClassNetCache", ParseType.Full)]
    public class RocketExplosion : Explosive
    {
    }
}
