using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    [NetFieldExportGroup("/Script/FortniteGame.FortTeamPrivateInfo")]
    public class FortTeamPrivateInfo : INetFieldExportGroup
    {
        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public int? RemoteRole { get; set; } //Type:  Bits: 2

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public int? Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("Owner", RepLayoutCmdType.PropertyObject)]
        public ActorGUID Owner { get; set; } //Type: AActor* Bits: 16

        [NetFieldExport("Value", RepLayoutCmdType.PropertyFloat)]
        public float? Value { get; set; }

        [NetFieldExport("PlayerID", RepLayoutCmdType.PropertyNetId)]
        public string PlayerID { get; set; }

        [NetFieldExport("PlayerState", RepLayoutCmdType.Property)]
        public ActorGUID PlayerState { get; set; }

        [NetFieldExport("LastRepLocation", RepLayoutCmdType.PropertyVector100)]
        public FVector LastRepLocation { get; set; }

        [NetFieldExport("LastRepYaw", RepLayoutCmdType.PropertyFloat)]
        public float? LastRepYaw { get; set; }

        [NetFieldExport("PawnStateMask", RepLayoutCmdType.Enum)]
        public int? PawnStateMask { get; set; }
    }
}
