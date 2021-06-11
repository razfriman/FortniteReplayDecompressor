using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Functions
{
    [NetFieldExportGroup("/Script/FortniteGame.FortPawn:NetMulticast_Athena_BatchedDamageCues")]
    public class BatchedDamage : INetFieldExportGroup
    {
        [NetFieldExport("Location", RepLayoutCmdType.PropertyVector10)]
        public FVector? Location { get; set; } //Type:  Bits: 1

        [NetFieldExport("Normal", RepLayoutCmdType.PropertyVectorNormal)]
        public FVector? Normal { get; set; } //Type:  Bits: 1

        [NetFieldExport("Magnitude", RepLayoutCmdType.PropertyFloat)]
        public float Magnitude { get; set; } //Type:  Bits: 1

        [NetFieldExport("bWeaponActivate", RepLayoutCmdType.PropertyBool)]
        public bool bWeaponActivate { get; set; } //Type:  Bits: 1

        [NetFieldExport("bIsFatal", RepLayoutCmdType.PropertyBool)]
        public bool bIsFatal { get; set; } //Type:  Bits: 1

        [NetFieldExport("bIsCritical", RepLayoutCmdType.PropertyBool)]
        public bool bIsCritical { get; set; } //Type:  Bits: 1

        [NetFieldExport("bIsShield", RepLayoutCmdType.PropertyBool)]
        public bool bIsShield { get; set; } //Type:  Bits: 1

        [NetFieldExport("bIsShieldDestroyed", RepLayoutCmdType.PropertyBool)]
        public bool bIsShieldDestroyed { get; set; } //Type:  Bits: 1

        [NetFieldExport("bIsBallistic", RepLayoutCmdType.PropertyBool)]
        public bool bIsBallistic { get; set; } //Type:  Bits: 1

        [NetFieldExport("bIsShieldApplied", RepLayoutCmdType.PropertyBool)]
        public bool bIsShieldApplied { get; set; } //Type:  Bits: 1

        [NetFieldExport("NonPlayerbIsFatal", RepLayoutCmdType.PropertyBool)]
        public bool NonPlayerbIsFatal { get; set; } //Type:  Bits: 1

        [NetFieldExport("NonPlayerbIsCritical", RepLayoutCmdType.PropertyBool)]
        public bool NonPlayerbIsCritical { get; set; } //Type:  Bits: 1

        [NetFieldExport("HitActor", RepLayoutCmdType.PropertyObject)]
        public uint HitActor { get; set; } //Type:  Bits: 1

        [NetFieldExport("NonPlayerHitActor", RepLayoutCmdType.PropertyObject)]
        public uint NonPlayerHitActor { get; set; } //Type:  Bits: 1

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "Location":
					Location = (FVector)value;
					break;
				case "Normal":
					Normal = (FVector)value;
					break;
				case "Magnitude":
					Magnitude = (float)value;
					break;
				case "bWeaponActivate":
					bWeaponActivate = (bool)value;
					break;
				case "bIsFatal":
					bIsFatal = (bool)value;
					break;
				case "bIsCritical":
					bIsCritical = (bool)value;
					break;
				case "bIsShield":
					bIsShield = (bool)value;
					break;
				case "bIsShieldDestroyed":
					bIsShieldDestroyed = (bool)value;
					break;
				case "bIsBallistic":
					bIsBallistic = (bool)value;
					break;
				case "bIsShieldApplied":
					bIsShieldApplied = (bool)value;
					break;
				case "NonPlayerbIsFatal":
					NonPlayerbIsFatal = (bool)value;
					break;
				case "NonPlayerbIsCritical":
					NonPlayerbIsCritical = (bool)value;
					break;
				case "HitActor":
					HitActor = (uint)value;
					break;
				case "NonPlayerHitActor":
					NonPlayerHitActor = (uint)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }
}
