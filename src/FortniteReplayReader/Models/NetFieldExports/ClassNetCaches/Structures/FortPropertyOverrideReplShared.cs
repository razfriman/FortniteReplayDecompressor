using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Structures
{
    [NetFieldExportGroup("/Script/FortniteGame.FortPropertyOverrideReplShared", ParseType.Debug)]
    public class FortPropertyOverrideReplShared : INetFieldExportGroup
    {
        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public object RemoteRole { get; set; } //Type:  Bits: 2

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public object Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("Owner", RepLayoutCmdType.Property)]
        public NetworkGUID Owner { get; set; }

        [NetFieldExport("PropertyScopedName", RepLayoutCmdType.PropertyString)]
        public string PropertyScopedName { get; set; }

        [NetFieldExport("PropertyData", RepLayoutCmdType.Ignore)]
        public DebuggingObject PropertyData { get; set; }
    }
}
