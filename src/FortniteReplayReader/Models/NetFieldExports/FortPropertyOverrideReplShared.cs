using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    [NetFieldExportGroup("/Script/FortniteGame.FortPropertyOverrideReplShared")]
    public class FortPropertyOverrideReplShared : INetFieldExportGroup
    {
        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public object RemoteRole { get; set; } //Type:  Bits: 2

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public object Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("PropertyScopedName", RepLayoutCmdType.PropertyString)]
        public string PropertyScopedName { get; set; }

        [NetFieldExport("PropertyData", RepLayoutCmdType.Property)]
        public DebuggingObject PropertyData { get; set; }
    }
}
