using FortniteReplayReader.Models.NetFieldExports.Enums;
using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    [NetFieldExportGroup("/Script/FortniteGame.FortPickupAthena", ParseType.Full)]
    public class FortPickup : INetFieldExportGroup
    {
        [NetFieldExport("bReplicateMovement", RepLayoutCmdType.PropertyBool)]
        public bool? bReplicateMovement { get; set; } //Type: uint8 Bits: 1

        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public object RemoteRole { get; set; } //Type:  Bits: 2

        [NetFieldExport("ReplicatedMovement", RepLayoutCmdType.RepMovement)]
        public FRepMovement ReplicatedMovement { get; set; } //Type: FRepMovement Bits: 102

        [NetFieldExport("AttachParent", RepLayoutCmdType.Property)]
        public ActorGUID AttachParent { get; set; } //Type:  Bits: 16

        [NetFieldExport("LocationOffset", RepLayoutCmdType.PropertyVector100)]
        public FVector LocationOffset { get; set; } //Type:  Bits: 35

        [NetFieldExport("RelativeScale3D", RepLayoutCmdType.PropertyVector100)]
        public FVector RelativeScale3D { get; set; } //Type:  Bits: 23

        [NetFieldExport("RotationOffset", RepLayoutCmdType.PropertyRotator)]
        public FRotator RotationOffset { get; set; } //Type:  Bits: 51

        [NetFieldExport("AttachComponent", RepLayoutCmdType.PropertyObject)]
        public uint? AttachComponent { get; set; } //Type:  Bits: 16

        [NetFieldExport("Owner", RepLayoutCmdType.PropertyObject)]
        public uint? Owner { get; set; } //Type: AActor* Bits: 0

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public object Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("bRandomRotation", RepLayoutCmdType.PropertyBool)]
        public bool? bRandomRotation { get; set; } //Type: bool Bits: 1

        [NetFieldExport("Count", RepLayoutCmdType.PropertyInt)]
        public int? Count { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("ItemDefinition", RepLayoutCmdType.Property)]
        public ItemDefinitionGUID ItemDefinition { get; set; } //Type: UFortItemDefinition* Bits: 16

        [NetFieldExport("Durability", RepLayoutCmdType.PropertyFloat)]
        public float? Durability { get; set; } //Type: float Bits: 32

        [NetFieldExport("Level", RepLayoutCmdType.PropertyInt)]
        public int? Level { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("LoadedAmmo", RepLayoutCmdType.PropertyInt)]
        public int? LoadedAmmo { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("A", RepLayoutCmdType.PropertyUInt32)]
        public uint? A { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("B", RepLayoutCmdType.PropertyUInt32)]
        public uint? B { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("C", RepLayoutCmdType.PropertyUInt32)]
        public uint? C { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("D", RepLayoutCmdType.PropertyUInt32)]
        public uint? D { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("bUpdateStatsOnCollection", RepLayoutCmdType.PropertyBool)]
        public bool? bUpdateStatsOnCollection { get; set; } //Type:  Bits: 1

        [NetFieldExport("bIsDirty", RepLayoutCmdType.PropertyBool)]
        public bool? bIsDirty { get; set; } //Type: bool Bits: 1

        [NetFieldExport("StateValues", RepLayoutCmdType.DynamicArray)]
        public FortPickup[] StateValues { get; set; } //Type: TArray Bits: 343

		[NetFieldExport("StateType", RepLayoutCmdType.Enum)]
		public EFortItemEntryState StateType { get; set; } = EFortItemEntryState.EFortItemEntryState_MAX; //Type:  Bits: 0

        [NetFieldExport("IntValue", RepLayoutCmdType.PropertyInt)]
        public int? IntValue { get; set; } //Type: int32 Bits: 0

        [NetFieldExport("NameValue", RepLayoutCmdType.Property)]
        public FName NameValue { get; set; } //Type: FName Bits: 0

        [NetFieldExport("GenericAttributeValues", RepLayoutCmdType.DynamicArray)]
        public float[] GenericAttributeValues { get; set; } //Type:  Bits: 80

        [NetFieldExport("CombineTarget", RepLayoutCmdType.PropertyObject)]
        public uint? CombineTarget { get; set; } //Type:  Bits: 16

        [NetFieldExport("PickupTarget", RepLayoutCmdType.PropertyObject)]
        public uint? PickupTarget { get; set; } //Type: AFortPawn* Bits: 16

        [NetFieldExport("ItemOwner", RepLayoutCmdType.PropertyObject)]
        public uint? ItemOwner { get; set; } //Type: AFortPawn* Bits: 8

        [NetFieldExport("LootInitialPosition", RepLayoutCmdType.PropertyVector10)]
        public FVector LootInitialPosition { get; set; } //Type: FVector_NetQuantize10 Bits: 71

        [NetFieldExport("LootFinalPosition", RepLayoutCmdType.PropertyVector10)]
        public FVector LootFinalPosition { get; set; } //Type: FVector_NetQuantize10 Bits: 71

        [NetFieldExport("FlyTime", RepLayoutCmdType.PropertyFloat)]
        public float? FlyTime { get; set; } //Type: float Bits: 32

        [NetFieldExport("StartDirection", RepLayoutCmdType.PropertyVectorNormal)]
        public FVector StartDirection { get; set; } //Type: FVector_NetQuantizeNormal Bits: 48

        [NetFieldExport("FinalTossRestLocation", RepLayoutCmdType.PropertyVector10)]
        public FVector FinalTossRestLocation { get; set; } //Type: FVector_NetQuantize10 Bits: 71

		[NetFieldExport("TossState", RepLayoutCmdType.Enum)]
		public EFortPickupTossState TossState { get; set; } = EFortPickupTossState.EFortPickupTossState_MAX; //Type: EFortPickupTossState Bits: 2

        [NetFieldExport("bCombinePickupsWhenTossCompletes", RepLayoutCmdType.PropertyBool)]
        public bool? bCombinePickupsWhenTossCompletes { get; set; } //Type:  Bits: 1

        [NetFieldExport("OptionalOwnerID", RepLayoutCmdType.PropertyInt16)]
        public short? OptionalOwnerID { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("bPickedUp", RepLayoutCmdType.PropertyBool)]
        public bool? bPickedUp { get; set; } //Type: bool Bits: 1

        [NetFieldExport("bTossedFromContainer", RepLayoutCmdType.PropertyBool)]
        public bool? bTossedFromContainer { get; set; } //Type: bool Bits: 1

        [NetFieldExport("bServerStoppedSimulation", RepLayoutCmdType.PropertyBool)]
        public bool? bServerStoppedSimulation { get; set; } //Type: bool Bits: 1

        [NetFieldExport("ServerImpactSoundFlash", RepLayoutCmdType.PropertyByte)]
        public byte? ServerImpactSoundFlash { get; set; } //Type: uint8 Bits: 8

        [NetFieldExport("PawnWhoDroppedPickup", RepLayoutCmdType.PropertyObject)]
        public uint? PawnWhoDroppedPickup { get; set; } //Type: AFortPawn* Bits: 8

        [NetFieldExport("OrderIndex", RepLayoutCmdType.PropertyUInt16)]
        public ushort? OrderIndex { get; set; }

        [NetFieldExport("WrapOverride", RepLayoutCmdType.PropertyString)]
        public string WrapOverride { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "bReplicateMovement":
					bReplicateMovement = (bool)value;
					break;
				case "RemoteRole":
					RemoteRole = value;
					break;
				case "ReplicatedMovement":
					ReplicatedMovement = (FRepMovement)value;
					break;
				case "AttachParent":
					AttachParent = (ActorGUID)value;
					break;
				case "LocationOffset":
					LocationOffset = (FVector)value;
					break;
				case "RelativeScale3D":
					RelativeScale3D = (FVector)value;
					break;
				case "RotationOffset":
					RotationOffset = (FRotator)value;
					break;
				case "AttachComponent":
					AttachComponent = (uint)value;
					break;
				case "Owner":
					Owner = (uint)value;
					break;
				case "Role":
					Role = value;
					break;
				case "bRandomRotation":
					bRandomRotation = (bool)value;
					break;
				case "Count":
					Count = (int)value;
					break;
				case "ItemDefinition":
					ItemDefinition = (ItemDefinitionGUID)value;
					break;
				case "Durability":
					Durability = (float)value;
					break;
				case "Level":
					Level = (int)value;
					break;
				case "LoadedAmmo":
					LoadedAmmo = (int)value;
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
				case "bUpdateStatsOnCollection":
					bUpdateStatsOnCollection = (bool)value;
					break;
				case "bIsDirty":
					bIsDirty = (bool)value;
					break;
				case "StateValues":
					StateValues = (FortPickup[])value;
					break;
				case "StateType":
					StateType = (EFortItemEntryState)value;
					break;
				case "IntValue":
					IntValue = (int)value;
					break;
				case "NameValue":
					NameValue = (FName)value;
					break;
				case "GenericAttributeValues":
					GenericAttributeValues = (float[])value;
					break;
				case "CombineTarget":
					CombineTarget = (uint)value;
					break;
				case "PickupTarget":
					PickupTarget = (uint)value;
					break;
				case "ItemOwner":
					ItemOwner = (uint)value;
					break;
				case "LootInitialPosition":
					LootInitialPosition = (FVector)value;
					break;
				case "LootFinalPosition":
					LootFinalPosition = (FVector)value;
					break;
				case "FlyTime":
					FlyTime = (float)value;
					break;
				case "StartDirection":
					StartDirection = (FVector)value;
					break;
				case "FinalTossRestLocation":
					FinalTossRestLocation = (FVector)value;
					break;
				case "TossState":
					TossState = (EFortPickupTossState)value;
					break;
				case "bCombinePickupsWhenTossCompletes":
					bCombinePickupsWhenTossCompletes = (bool)value;
					break;
				case "OptionalOwnerID":
					OptionalOwnerID = (short)value;
					break;
				case "bPickedUp":
					bPickedUp = (bool)value;
					break;
				case "bTossedFromContainer":
					bTossedFromContainer = (bool)value;
					break;
				case "bServerStoppedSimulation":
					bServerStoppedSimulation = (bool)value;
					break;
				case "ServerImpactSoundFlash":
					ServerImpactSoundFlash = (byte)value;
					break;
				case "PawnWhoDroppedPickup":
					PawnWhoDroppedPickup = (uint)value;
					break;
				case "OrderIndex":
					OrderIndex = (ushort)value;
					break;
				case "WrapOverride":
					WrapOverride = (string)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }

}
