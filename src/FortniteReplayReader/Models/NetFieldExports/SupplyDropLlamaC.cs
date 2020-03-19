using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
	[NetFieldExportGroup("/Game/Athena/SupplyDrops/Llama/AthenaSupplyDrop_Llama.AthenaSupplyDrop_Llama_C")]
	public class SupplyDropLlamaC : INetFieldExportGroup
	{
		[NetFieldExport("bHidden", RepLayoutCmdType.PropertyBool)]
		public bool? bHidden { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
		public object RemoteRole { get; set; } //Type:  Bits: 2

		[NetFieldExport("ReplicatedMovement", RepLayoutCmdType.RepMovementWholeNumber)]
		public FRepMovement ReplicatedMovement { get; set; } //Type: FRepMovement Bits: 79

		[NetFieldExport("Role", RepLayoutCmdType.Ignore)]
		public object Role { get; set; } //Type:  Bits: 2

		[NetFieldExport("bDestroyed", RepLayoutCmdType.PropertyBool)]
		public bool? bDestroyed { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bEditorPlaced", RepLayoutCmdType.PropertyBool)]
		public bool? bEditorPlaced { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bInstantDeath", RepLayoutCmdType.PropertyBool)]
		public bool? bInstantDeath { get; set; } //Type:  Bits: 1

		[NetFieldExport("bHasSpawnedPickups", RepLayoutCmdType.PropertyBool)]
		public bool? bHasSpawnedPickups { get; set; } //Type: bool Bits: 1

		[NetFieldExport("Looted", RepLayoutCmdType.PropertyBool)]
		public bool? Looted { get; set; } //Type: bool Bits: 1

		[NetFieldExport("FinalDestination", RepLayoutCmdType.PropertyVector)]
		public FVector FinalDestination { get; set; } //Type: FVector Bits: 96

	}
}
