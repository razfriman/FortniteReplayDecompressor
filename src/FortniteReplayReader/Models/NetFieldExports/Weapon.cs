using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    public class Weapon : INetFieldExportGroup
    {
        [NetFieldExport("bHidden", RepLayoutCmdType.PropertyBool, 0, "bHidden", "", 1)]
        public bool? bHidden { get; set; } //Type:  Bits: 1

        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore, 4, "RemoteRole", "", 2)]
        public object RemoteRole { get; set; } //Type:  Bits: 2

        [NetFieldExport("Owner", RepLayoutCmdType.PropertyObject, 12, "Owner", "", 16)]
        public uint? Owner { get; set; } //Type:  Bits: 16

        [NetFieldExport("Role", RepLayoutCmdType.Ignore, 13, "Role", "", 2)]
        public object Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("Instigator", RepLayoutCmdType.PropertyObject, 14, "Instigator", "", 16)]
        public uint? Instigator { get; set; } //Type:  Bits: 16

        [NetFieldExport("bIsEquippingWeapon", RepLayoutCmdType.PropertyBool, 15, "bIsEquippingWeapon", "", 1)]
        public bool? bIsEquippingWeapon { get; set; } //Type:  Bits: 1

        [NetFieldExport("bIsReloadingWeapon", RepLayoutCmdType.PropertyBool, 16, "bIsReloadingWeapon", "", 1)]
        public bool? bIsReloadingWeapon { get; set; } //Type:  Bits: 1

        [NetFieldExport("WeaponData", RepLayoutCmdType.PropertyObject, 18, "WeaponData", "", 16)]
        public uint? WeaponData { get; set; } //Type:  Bits: 16

        [NetFieldExport("A", RepLayoutCmdType.PropertyUInt32, 21, "A", "", 32)]
        public uint? A { get; set; } //Type:  Bits: 32

        [NetFieldExport("B", RepLayoutCmdType.PropertyUInt32, 22, "B", "", 32)]
        public uint? B { get; set; } //Type:  Bits: 32

        [NetFieldExport("C", RepLayoutCmdType.PropertyUInt32, 23, "C", "", 32)]
        public uint? C { get; set; } //Type:  Bits: 32

        [NetFieldExport("D", RepLayoutCmdType.PropertyUInt32, 24, "D", "", 32)]
        public uint? D { get; set; } //Type:  Bits: 32

        [NetFieldExport("WeaponLevel", RepLayoutCmdType.PropertyUInt32, 25, "WeaponLevel", "", 32)]
        public uint? WeaponLevel { get; set; } //Type:  Bits: 32

        [NetFieldExport("AmmoCount", RepLayoutCmdType.PropertyUInt32, 26, "AmmoCount", "", 32)]
        public uint? AmmoCount { get; set; } //Type:  Bits: 32

        [NetFieldExport("AppliedAlterations", RepLayoutCmdType.Ignore, 32, "AppliedAlterations", "", 64)]
        public object AppliedAlterations { get; set; } //Type:  Bits: 64
    }
}
