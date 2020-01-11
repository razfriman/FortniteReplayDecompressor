using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    [NetFieldExportGroup("/Game/Spectating/BP_ReplayPC_Athena.BP_ReplayPC_Athena_C", ParseType.Normal)]
    [PlayerController("BP_ReplayPC_Athena_C")]
    public class ReplayPC : INetFieldExportGroup
    {
    }
}
