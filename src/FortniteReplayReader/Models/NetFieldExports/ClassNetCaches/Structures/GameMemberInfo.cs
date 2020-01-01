using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Structures
{
    [NetFieldExportGroup("/Script/FortniteGame.GameMemberInfo", ParseType.Debug)]
    public class GameMemberInfo : INetFieldExportGroup
    {
        [NetFieldExport("SquadId", RepLayoutCmdType.Property)]
        public DebuggingObject SquadId { get; set; }

        [NetFieldExport("TeamIndex", RepLayoutCmdType.Property)]
        public DebuggingObject TeamIndex { get; set; }

        [NetFieldExport("MemberUniqueId", RepLayoutCmdType.Property)]
        public DebuggingObject MemberUniqueId { get; set; }
    }
}
