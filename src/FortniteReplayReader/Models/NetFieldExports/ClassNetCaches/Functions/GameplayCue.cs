using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Functions
{
    [NetFieldExportGroup("/Script/FortniteGame.FortPawn:NetMulticast_InvokeGameplayCueAdded_WithParams", ParseType.Normal)]
    public class GameplayCue : INetFieldExportGroup
    {
        [NetFieldExport("GameplayCueTag", RepLayoutCmdType.Property)]
        public FGameplayTag GameplayCueTag { get; set; } //Type:  Bits: 1
    }

    [NetFieldExportGroup("/Script/FortniteGame.FortPawn:NetMulticast_InvokeGameplayCueExecuted_WithParams", ParseType.Normal)]
    public class GameplayCueWithParams : GameplayCue
    {
        [NetFieldExport("GameplayCueParameters", RepLayoutCmdType.Property)]
        public FGameplayCueParameters GameplayCueParameters { get; set; } //Type:  Bits: 1
    }
}
