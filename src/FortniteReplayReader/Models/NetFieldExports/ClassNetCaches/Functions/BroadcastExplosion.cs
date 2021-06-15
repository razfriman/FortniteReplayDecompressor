using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Functions
{
    [NetFieldExportGroup("/Script/FortniteGame.FortGameplayEffectDeliveryActor:BroadcastExplosion")]
    public class Explosion : INetFieldExportGroup
    {
        [NetFieldExport("HitActors", RepLayoutCmdType.DynamicArray)]
        public NetworkGUID[] HitActors { get; set; } //Type: bool Bits: 1

        [NetFieldExport("HitResults", RepLayoutCmdType.DynamicArray)]
        public FHitResult[] HitResults { get; set; } //Type: bool Bits: 1
    }
}
