using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Custom;
using FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Structures;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches
{
    [NetFieldExportRPC("Athena_GameState_C_ClassNetCache", ParseType.Normal)]
    public class GameStateCache
    {
        [NetFieldExportRPCProperty("ActiveGameplayModifiers", "/Script/FortniteGame.ActiveGameplayModifier", false)]
        public object ActiveGameplayModifiers { get; set; }

        [NetFieldExportRPCProperty("GameMemberInfoArray", "/Script/FortniteGame.GameMemberInfo", false)]
        public object GameMemberInfoArray { get; set; }

        [NetFieldExportRPCProperty("CurrentPlaylistInfo", "CurrentPlaylistInfo", customStructure: true)]
        public CurrentPlaylistInfo CurrentPlaylistInfo { get; set; }
    }
}
