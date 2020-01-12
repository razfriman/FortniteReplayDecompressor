using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    public class BaseContainer : INetFieldExportGroup
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
        public FQuantizedBuildingAttribute BuildTime { get; set; }

        [NetFieldExport("RepairTime", RepLayoutCmdType.Property)]
        public FQuantizedBuildingAttribute RepairTime { get; set; }

        [NetFieldExport("Health", RepLayoutCmdType.PropertyInt16)]
        public short? Health { get; set; }

        [NetFieldExport("MaxHealth", RepLayoutCmdType.PropertyInt16)]
        public short? MaxHealth { get; set; }

        [NetFieldExport("ReplicatedLootTier", RepLayoutCmdType.PropertyInt)]
        public int? ReplicatedLootTier { get; set; }

        [NetFieldExport("SearchAnimationCount", RepLayoutCmdType.PropertyUInt32)]
        public uint? SearchAnimationCount { get; set; }

        [NetFieldExport("bAlreadySearched", RepLayoutCmdType.PropertyBool)]
        public bool? bAlreadySearched { get; set; }

        [NetFieldExport("ResourceType", RepLayoutCmdType.Enum)]
        public int? ResourceType { get; set; }

        [NetFieldExport("BounceNormal", RepLayoutCmdType.PropertyVector)]
        public FVector BounceNormal { get; set; }
    }
}
