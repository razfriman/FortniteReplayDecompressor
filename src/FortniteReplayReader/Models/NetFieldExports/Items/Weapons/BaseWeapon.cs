using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Weapons
{
    public class BaseWeapon : INetFieldExportGroup
    {
        [NetFieldExport("bHidden", RepLayoutCmdType.PropertyBool)]
        public bool? bHidden { get; set; } //Type:  Bits: 1

        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public object RemoteRole { get; set; } //Type:  Bits: 2

        [NetFieldExport("Owner", RepLayoutCmdType.Property)]
        public NetworkGUID Owner { get; set; } //Type:  Bits: 16

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public object Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("Instigator", RepLayoutCmdType.Property)]
        public ActorGUID Instigator { get; set; } //Type:  Bits: 16

        [NetFieldExport("bIsEquippingWeapon", RepLayoutCmdType.PropertyBool)]
        public bool? bIsEquippingWeapon { get; set; } //Type:  Bits: 1

        [NetFieldExport("bIsReloadingWeapon", RepLayoutCmdType.PropertyBool)]
        public bool? bIsReloadingWeapon { get; set; } //Type:  Bits: 1

        [NetFieldExport("bIsMuzzleTraceNearWall", RepLayoutCmdType.PropertyBool)]
        public bool? bIsMuzzleTraceNearWall { get; set; } //Type:  Bits: 1

        [NetFieldExport("WeaponData", RepLayoutCmdType.Property)]
        public ItemDefinitionGUID WeaponData { get; set; } //Type:  Bits: 16

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

        [NetFieldExport("AppliedAlterations", RepLayoutCmdType.DynamicArray)]
        public ItemDefinitionGUID[] AppliedAlterations { get; set; } //Type:  Bits: 64

        [NetFieldExport("LastFireTimeVerified", RepLayoutCmdType.PropertyFloat)]
        public float? LastFireTimeVerified { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "bHidden":
					bHidden = (bool)value;
					break;
				case "RemoteRole":
					RemoteRole = value;
					break;
				case "Owner":
					Owner = (NetworkGUID)value;
					break;
				case "Role":
					Role = value;
					break;
				case "Instigator":
					Instigator = (ActorGUID)value;
					break;
				case "bIsEquippingWeapon":
					bIsEquippingWeapon = (bool)value;
					break;
				case "bIsReloadingWeapon":
					bIsReloadingWeapon = (bool)value;
					break;
				case "bIsMuzzleTraceNearWall":
					bIsMuzzleTraceNearWall = (bool)value;
					break;
				case "WeaponData":
					WeaponData = (ItemDefinitionGUID)value;
					break;
				case "A":
					A = (uint)value;
					break;
				case "B":
					B = (uint)value;
					break;
				case "C":
					C = (uint)value;
					break;
				case "D":
					D = (uint)value;
					break;
				case "WeaponLevel":
					WeaponLevel = (uint)value;
					break;
				case "AmmoCount":
					AmmoCount = (uint)value;
					break;
				case "AppliedAlterations":
					AppliedAlterations = (ItemDefinitionGUID[])value;
					break;
				case "LastFireTimeVerified":
					LastFireTimeVerified = (float)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }
}
