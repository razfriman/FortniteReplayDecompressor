using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    [NetFieldExportGroup("/Game/Athena/AI/Phoebe/BP_PlayerPawn_Athena_Phoebe.BP_PlayerPawn_Athena_Phoebe_C", ParseType.Normal)]
    public class AIPlayerPawn : PlayerPawnC
    {
    }

    [NetFieldExportGroup("/Game/Athena/AI/MANG/BP_MangPlayerPawn_Default.BP_MangPlayerPawn_Default_C", ParseType.Full)]
    public class MangPlayerPawn : PlayerPawnC
    {
    }

    [NetFieldExportGroup("/Game/Athena/AI/MANG/BP_MangPlayerPawn_Boss_AdventureGirl.BP_MangPlayerPawn_Boss_AdventureGirl_C", ParseType.Full)]
    public class MangBossPlayerPawn : PlayerPawnC
    {
    }

    [NetFieldExportGroup("/Game/Athena/AI/MANG/MangDataTracker.MangDataTracker_C", ParseType.Full)]
    public class MangDataTracker : INetFieldExportGroup
    {
        [NetFieldExport("BotPawn", RepLayoutCmdType.Property)]
        public ActorGUID BotPawn { get; set; }

        [NetFieldExport("CurrentBotAlertLevel", RepLayoutCmdType.Enum)]
        public int? CurrentBotAlertLevel { get; set; }
    }
}
