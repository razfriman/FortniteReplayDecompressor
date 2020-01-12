﻿using Unreal.Core.Attributes;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    public abstract class BaseWeapon
    {
        [NetFieldExport("bHidden", RepLayoutCmdType.Ignore)]
        public bool bHidden { get; set; }

        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public int RemoteRole { get; set; }

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public int Role { get; set; }

        [NetFieldExport("Owner", RepLayoutCmdType.Ignore)]
        public uint Owner { get; set; }

        [NetFieldExport("Instigator", RepLayoutCmdType.PropertyObject)]
        public uint Instigator { get; set; }

        [NetFieldExport("bIsEquippingWeapon", RepLayoutCmdType.PropertyBool)]
        public bool bIsEquippingWeapon { get; set; }

        [NetFieldExport("bIsReloadingWeapon", RepLayoutCmdType.PropertyBool)]
        public bool bIsReloadingWeapon { get; set; }

        [NetFieldExport("WeaponData", RepLayoutCmdType.Property)]
        public DebuggingObject WeaponData { get; set; }

        [NetFieldExport("LastFireTimeVerified", RepLayoutCmdType.Property)]
        public DebuggingObject LastFireTimeVerified { get; set; }

        [NetFieldExport("A", RepLayoutCmdType.Property)]
        public DebuggingObject A { get; set; }

        [NetFieldExport("B", RepLayoutCmdType.Property)]
        public DebuggingObject B { get; set; }

        [NetFieldExport("C", RepLayoutCmdType.Property)]
        public DebuggingObject C { get; set; }

        [NetFieldExport("D", RepLayoutCmdType.Property)]
        public DebuggingObject D { get; set; }

        [NetFieldExport("WeaponLevel", RepLayoutCmdType.PropertyInt)]
        public int WeaponLevel { get; set; }

        [NetFieldExport("AmmoCount", RepLayoutCmdType.PropertyInt)]
        public int AmmoCount { get; set; }

        [NetFieldExport("AppliedAlterations", RepLayoutCmdType.DynamicArray)]
        public DebuggingObject[] AppliedAlterations { get; set; }

        [NetFieldExport("bIsMuzzleTraceNearWall", RepLayoutCmdType.PropertyBool)]
        public bool bIsMuzzleTraceNearWall { get; set; }
    }
}