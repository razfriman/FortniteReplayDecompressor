using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Structures
{
    [NetFieldExportGroup("/Script/FortniteGame.FortClientObservedStat", ParseType.Debug)]
    public class FortClientObservedStat : INetFieldExportGroup
    {
        [NetFieldExport("StatName", RepLayoutCmdType.PropertyName)]
        public string StatName { get; set; }

        [NetFieldExport("StatValue", RepLayoutCmdType.PropertyUInt32)]
        public uint? StatValue { get; set; }
    }
}
