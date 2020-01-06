using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    public class BaseWeapon : INetFieldExportGroup
    {
        [NetFieldExport("bHidden", RepLayoutCmdType.PropertyBool)]
        public bool? bHidden { get; set; } //Type:  Bits: 1

        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public object RemoteRole { get; set; } //Type:  Bits: 2

        [NetFieldExport("Owner", RepLayoutCmdType.PropertyObject)]
        public uint? Owner { get; set; } //Type:  Bits: 16

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public object Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("Instigator", RepLayoutCmdType.PropertyObject)]
        public uint? Instigator { get; set; } //Type:  Bits: 16

        [NetFieldExport("bIsEquippingWeapon", RepLayoutCmdType.PropertyBool)]
        public bool? bIsEquippingWeapon { get; set; } //Type:  Bits: 1

        [NetFieldExport("bIsReloadingWeapon", RepLayoutCmdType.PropertyBool)]
        public bool? bIsReloadingWeapon { get; set; } //Type:  Bits: 1

        [NetFieldExport("WeaponData", RepLayoutCmdType.PropertyObject)]
        public uint? WeaponData { get; set; } //Type:  Bits: 16

        [NetFieldExport("A", RepLayoutCmdType.PropertyUInt32)]
        public uint? A { get; set; } //Type:  Bits: 32

        [NetFieldExport("B", RepLayoutCmdType.PropertyUInt32)]
        public uint? B { get; set; } //Type:  Bits: 32

        [NetFieldExport("C", RepLayoutCmdType.PropertyUInt32)]
        public uint? C { get; set; } //Type:  Bits: 32

        [NetFieldExport("D", RepLayoutCmdType.PropertyUInt32)]
        public uint? D { get; set; } //Type:  Bits: 32

        [NetFieldExport("WeaponLevel", RepLayoutCmdType.PropertyUInt32)]
        public uint? WeaponLevel { get; set; } //Type:  Bits: 32

        [NetFieldExport("AmmoCount", RepLayoutCmdType.PropertyUInt32)]
        public uint? AmmoCount { get; set; } //Type:  Bits: 32

        [NetFieldExport("AppliedAlterations", RepLayoutCmdType.Ignore)]
        public object AppliedAlterations { get; set; } //Type:  Bits: 64
    }
}
