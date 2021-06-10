using FortniteReplayReader.Models.NetFieldExports.Enums;
using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
	[NetFieldExportGroup("/Script/FortniteGame.FortInventory", ParseType.Normal)]
	public class FortInventory : INetFieldExportGroup
	{
		[NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
		public object RemoteRole { get; set; } //Type:  Bits: 2

		[NetFieldExport("Owner", RepLayoutCmdType.PropertyObject)]
		public uint? Owner { get; set; } //Type: AActor* Bits: 8

		[NetFieldExport("Role", RepLayoutCmdType.Ignore)]
		public object Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("Count", RepLayoutCmdType.PropertyInt)]
        public int? Count { get; set; } //Type:  Bits: 2

        [NetFieldExport("ItemDefinition", RepLayoutCmdType.Property)]
        public ItemDefinitionGUID ItemDefinition { get; set; } 

        [NetFieldExport("OrderIndex", RepLayoutCmdType.PropertyUInt16)]
        public ushort? OrderIndex { get; set; } 

        [NetFieldExport("Durability", RepLayoutCmdType.PropertyFloat)]
        public float? Durability { get; set; } 

        [NetFieldExport("Level", RepLayoutCmdType.PropertyInt)]
        public int? Level { get; set; } 

        [NetFieldExport("LoadedAmmo", RepLayoutCmdType.PropertyInt)]
        public int? LoadedAmmo { get; set; } 

        [NetFieldExport("A", RepLayoutCmdType.PropertyUInt32)]
        public uint? A { get; set; } 

        [NetFieldExport("B", RepLayoutCmdType.PropertyUInt32)]
        public uint? B { get; set; } 

        [NetFieldExport("C", RepLayoutCmdType.PropertyUInt32)]
        public uint? C { get; set; } 

        [NetFieldExport("D", RepLayoutCmdType.PropertyUInt32)]
        public uint? D { get; set; } 

        [NetFieldExport("inventoryoverflowdate", RepLayoutCmdType.PropertyBool)]
        public bool? InventoryOverflowDate { get; set; }

        [NetFieldExport("bIsReplicatedCopy", RepLayoutCmdType.PropertyBool)]
        public bool? bIsReplicatedCopy { get; set; } 

        [NetFieldExport("bIsDirty", RepLayoutCmdType.PropertyBool)]
        public bool? bIsDirty { get; set; }

        [NetFieldExport("bWasGifted", RepLayoutCmdType.PropertyBool)]
        public bool? bWasGifted { get; set; }

        [NetFieldExport("bUpdateStatsOnCollection", RepLayoutCmdType.PropertyBool)]
        public bool? bUpdateStatsOnCollection { get; set; }

        [NetFieldExport("StateValues", RepLayoutCmdType.DynamicArray)]
        public FortInventory[] StateValues { get; set; } 

        [NetFieldExport("IntValue", RepLayoutCmdType.PropertyInt)]
        public int? IntValue { get; set; } 

        [NetFieldExport("NameValue", RepLayoutCmdType.PropertyString)]
        public string NameValue { get; set; } 

        [NetFieldExport("StateType", RepLayoutCmdType.Enum)]
        public EFortItemEntryState StateType { get; set; }

        [NetFieldExport("ParentInventory", RepLayoutCmdType.PropertyObject)]
        public uint? ParentInventory { get; set; } 

        [NetFieldExport("Handle", RepLayoutCmdType.PropertyInt)]
        public int? Handle { get; set; } 

        [NetFieldExport("AlterationInstances", RepLayoutCmdType.Ignore)]
        public DebuggingObject[] AlterationInstances { get; set; } 

        [NetFieldExport("GenericAttributeValues", RepLayoutCmdType.Ignore)]
        public DebuggingObject[] GenericAttributeValues { get; set; } 

        [NetFieldExport("ReplayPawn", RepLayoutCmdType.PropertyObject)]
		public uint? ReplayPawn { get; set; } //Type:  Bits: 16

        [NetFieldExport("WrapOverride", RepLayoutCmdType.Ignore)]
        public DebuggingObject WrapOverride { get; set; } //Type:  Bits: 16

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "RemoteRole":
					RemoteRole = value;
					break;
				case "Owner":
					Owner = (uint)value;
					break;
				case "Role":
					Role = value;
					break;
				case "Count":
					Count = (int)value;
					break;
				case "ItemDefinition":
					ItemDefinition = (ItemDefinitionGUID)value;
					break;
				case "OrderIndex":
					OrderIndex = (ushort)value;
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
				case "inventoryoverflowdate":
				case "InventoryOverflowDate":
					InventoryOverflowDate = (bool)value;
					break;
				case "bIsReplicatedCopy":
					bIsReplicatedCopy = (bool)value;
					break;
				case "bIsDirty":
					bIsDirty = (bool)value;
					break;
				case "bWasGifted":
					bWasGifted = (bool)value;
					break;
				case "bUpdateStatsOnCollection":
					bUpdateStatsOnCollection = (bool)value;
					break;
				case "StateValues":
					StateValues = (FortInventory[])value;
					break;
				case "IntValue":
					IntValue = (int)value;
					break;
				case "NameValue":
					NameValue = (string)value;
					break;
				case "StateType":
					StateType = (EFortItemEntryState)value;
					break;
				case "ParentInventory":
					ParentInventory = (uint)value;
					break;
				case "Handle":
					Handle = (int)value;
					break;
				case "AlterationInstances":
					AlterationInstances = (DebuggingObject[])value;
					break;
				case "GenericAttributeValues":
					GenericAttributeValues = (DebuggingObject[])value;
					break;
				case "ReplayPawn":
					ReplayPawn = (uint)value;
					break;
				case "WrapOverride":
					WrapOverride = (DebuggingObject)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}
	}
}
