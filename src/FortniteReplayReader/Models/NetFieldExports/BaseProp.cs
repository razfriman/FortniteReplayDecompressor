using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    public class BaseProp : INetFieldExportGroup
    {
        [NetFieldExport("bHidden", RepLayoutCmdType.Ignore)]
        public bool? bHidden { get; set; }

        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public int? RemoteRole { get; set; }

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public int? Role { get; set; }

        [NetFieldExport("bDestroyed", RepLayoutCmdType.PropertyBool)]
        public bool? bDestroyed { get; set; }

        [NetFieldExport("BuildTime", RepLayoutCmdType.Ignore)]
        public DebuggingObject BuildTime { get; set; }

        [NetFieldExport("RepairTime", RepLayoutCmdType.Ignore)]
        public DebuggingObject RepairTime { get; set; }

        [NetFieldExport("Health", RepLayoutCmdType.Ignore)]
        public DebuggingObject Health { get; set; }

        [NetFieldExport("MaxHealth", RepLayoutCmdType.Ignore)]
        public DebuggingObject MaxHealth { get; set; }
    }
}
