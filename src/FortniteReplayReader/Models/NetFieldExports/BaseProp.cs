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

        [NetFieldExport("BuildTime", RepLayoutCmdType.Property)]
        public DebuggingObject BuildTime { get; set; }

        [NetFieldExport("RepairTime", RepLayoutCmdType.Property)]
        public DebuggingObject RepairTime { get; set; }

        [NetFieldExport("Health", RepLayoutCmdType.Property)]
        public DebuggingObject Health { get; set; }

        [NetFieldExport("MaxHealth", RepLayoutCmdType.Property)]
        public DebuggingObject MaxHealth { get; set; }
    }
}
