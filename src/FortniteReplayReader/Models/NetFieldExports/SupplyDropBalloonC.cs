using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
	[NetFieldExportGroup("/Game/Athena/SupplyDrops/AthenaSupplyDropBalloon.AthenaSupplyDropBalloon_C")]
	public class SupplyDropBalloonC : INetFieldExportGroup
	{
		[NetFieldExport("bHidden", RepLayoutCmdType.PropertyBool)]
		public bool? bHidden { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bCanBeDamaged", RepLayoutCmdType.PropertyBool)]
		public bool? bCanBeDamaged { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
		public object RemoteRole { get; set; } //Type:  Bits: 2

		[NetFieldExport("AttachParent", RepLayoutCmdType.PropertyObject)]
		public uint? AttachParent { get; set; } //Type: AActor* Bits: 16

		[NetFieldExport("LocationOffset", RepLayoutCmdType.Ignore)]
		public object LocationOffset { get; set; } //Type:  Bits: 41

		[NetFieldExport("RelativeScale3D", RepLayoutCmdType.PropertyVector100)]
		public FVector? RelativeScale3D { get; set; } //Type: FVector_NetQuantize100 Bits: 29

		[NetFieldExport("AttachComponent", RepLayoutCmdType.PropertyObject)]
		public uint? AttachComponent { get; set; } //Type: USceneComponent* Bits: 16

		[NetFieldExport("Role", RepLayoutCmdType.Ignore)]
		public object Role { get; set; } //Type:  Bits: 2

		[NetFieldExport("A", RepLayoutCmdType.PropertyInt)]
		public int? A { get; set; } //Type: int32 Bits: 32

		[NetFieldExport("B", RepLayoutCmdType.PropertyInt)]
		public int? B { get; set; } //Type: int32 Bits: 32

		[NetFieldExport("C", RepLayoutCmdType.PropertyInt)]
		public int? C { get; set; } //Type: int32 Bits: 32

		[NetFieldExport("D", RepLayoutCmdType.PropertyInt)]
		public int? D { get; set; } //Type: int32 Bits: 32

		[NetFieldExport("ReplicatedBuildingAttributeSet", RepLayoutCmdType.PropertyObject)]
		public uint? ReplicatedBuildingAttributeSet { get; set; } //Type: UFortBuildingActorSet* Bits: 16

		[NetFieldExport("ReplicatedAbilitySystemComponent", RepLayoutCmdType.PropertyObject)]
		public uint? ReplicatedAbilitySystemComponent { get; set; } //Type: UFortAbilitySystemComponent* Bits: 16

		[NetFieldExport("bDestroyed", RepLayoutCmdType.PropertyBool)]
		public bool? bDestroyed { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bEditorPlaced", RepLayoutCmdType.PropertyBool)]
		public bool? bEditorPlaced { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bInstantDeath", RepLayoutCmdType.PropertyBool)]
		public bool? bInstantDeath { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("AttachmentPlacementBlockingActors", RepLayoutCmdType.DynamicArray)]
		public object[] AttachmentPlacementBlockingActors { get; set; } //Type: TArray Bits: 16


		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "bHidden":
					bHidden = (bool)value;
					break;
				case "bCanBeDamaged":
					bCanBeDamaged = (bool)value;
					break;
				case "RemoteRole":
					RemoteRole = value;
					break;
				case "AttachParent":
					AttachParent = (uint)value;
					break;
				case "LocationOffset":
					LocationOffset = value;
					break;
				case "RelativeScale3D":
					RelativeScale3D = (FVector)value;
					break;
				case "AttachComponent":
					AttachComponent = (uint)value;
					break;
				case "Role":
					Role = value;
					break;
				case "A":
					A = (int)value;
					break;
				case "B":
					B = (int)value;
					break;
				case "C":
					C = (int)value;
					break;
				case "D":
					D = (int)value;
					break;
				case "ReplicatedBuildingAttributeSet":
					ReplicatedBuildingAttributeSet = (uint)value;
					break;
				case "ReplicatedAbilitySystemComponent":
					ReplicatedAbilitySystemComponent = (uint)value;
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
				case "AttachmentPlacementBlockingActors":
					AttachmentPlacementBlockingActors = (object[])value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

	}
}
