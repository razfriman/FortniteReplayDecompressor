using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    [NetFieldExportGroup("/Game/Spectating/BP_ReplayPC_Athena.BP_ReplayPC_Athena_C", ParseType.Normal)]
    [PlayerController("BP_ReplayPC_Athena_C")]
    public class ReplayPC : INetFieldExportGroup
    {
        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public object RemoteRole { get; set; } //Type:  Bits: 2

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public object Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("PlayerState", RepLayoutCmdType.Property)]
        public NetworkGUID PlayerState { get; set; }

        [NetFieldExport("SpawnLocation", RepLayoutCmdType.PropertyVector)]
        public FVector SpawnLocation { get; set; }
    }
}
