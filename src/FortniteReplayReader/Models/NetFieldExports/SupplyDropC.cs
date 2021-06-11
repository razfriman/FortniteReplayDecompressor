using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
	[NetFieldExportGroup("/Game/Athena/SupplyDrops/AthenaSupplyDrop.AthenaSupplyDrop_C")]
	public class SupplyDropC : INetFieldExportGroup
	{
		[NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
		public object RemoteRole { get; set; } //Type:  Bits: 2

		[NetFieldExport("ReplicatedMovement", RepLayoutCmdType.RepMovementWholeNumber)]
		public FRepMovement ReplicatedMovement { get; set; } //Type: FRepMovement Bits: 109

		[NetFieldExport("Role", RepLayoutCmdType.Ignore)]
		public object Role { get; set; } //Type:  Bits: 2

		[NetFieldExport("bDestroyed", RepLayoutCmdType.PropertyBool)]
		public bool? bDestroyed { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bEditorPlaced", RepLayoutCmdType.PropertyBool)]
		public bool? bEditorPlaced { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bInstantDeath", RepLayoutCmdType.PropertyBool)]
		public bool? bInstantDeath { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bHasSpawnedPickups", RepLayoutCmdType.PropertyBool)]
		public bool? bHasSpawnedPickups { get; set; } //Type: bool Bits: 1

		[NetFieldExport("Opened", RepLayoutCmdType.PropertyBool)]
		public bool? Opened { get; set; } //Type: bool Bits: 1

		[NetFieldExport("BalloonPopped", RepLayoutCmdType.PropertyBool)]
		public bool? BalloonPopped { get; set; } //Type: bool Bits: 1

		[NetFieldExport("FallSpeed", RepLayoutCmdType.PropertyFloat)]
		public float? FallSpeed { get; set; } //Type: float Bits: 32

		[NetFieldExport("LandingLocation", RepLayoutCmdType.PropertyVector)]
		public FVector? LandingLocation { get; set; } //Type: FVector Bits: 96

		[NetFieldExport("FallHeight", RepLayoutCmdType.PropertyFloat)]
		public float? FallHeight { get; set; } //Type: float Bits: 32


		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "RemoteRole":
					RemoteRole = value;
					break;
				case "ReplicatedMovement":
					ReplicatedMovement = (FRepMovement)value;
					break;
				case "Role":
					Role = value;
					break;
				case "bDestroyed":
					bDestroyed = (bool)value;
					break;
				case "bEditorPlaced":
					bEditorPlaced = (bool)value;
					break;
				case "bInstantDeath":
					bInstantDeath = (bool)value;
					break;
				case "bHasSpawnedPickups":
					bHasSpawnedPickups = (bool)value;
					break;
				case "Opened":
					Opened = (bool)value;
					break;
				case "BalloonPopped":
					BalloonPopped = (bool)value;
					break;
				case "FallSpeed":
					FallSpeed = (float)value;
					break;
				case "LandingLocation":
					LandingLocation = (FVector)value;
					break;
				case "FallHeight":
					FallHeight = (float)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

	}
}
