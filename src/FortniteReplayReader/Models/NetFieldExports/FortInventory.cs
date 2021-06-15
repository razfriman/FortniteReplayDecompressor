using FortniteReplayReader.Models.NetFieldExports.Enums;
using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
	[NetFieldExportGroup("/Script/FortniteGame.FortInventory", ParseType.Normal)]
	public sealed class FortInventory : INetFieldExportGroup
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

        [NetFieldExport("inventory_overflow_date", RepLayoutCmdType.PropertyBool)]
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
	}
}
