using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
	[NetFieldExportGroup("/Game/Athena/Deimos/Spawners/RiftSpawners/AthenaSupplyDrop_DeimosSpawner.AthenaSupplyDrop_DeimosSpawner_C")]
	public class SupplyDropDeimosSpawnerC : INetFieldExportGroup
	{
		[NetFieldExport("ReplicatedMovement", RepLayoutCmdType.RepMovementWholeNumber)]
		public FRepMovement ReplicatedMovement { get; set; } //Type: FRepMovement Bits: 83

		[NetFieldExport("bEditorPlaced", RepLayoutCmdType.PropertyBool)]
		public bool? bEditorPlaced { get; set; } //Type: uint8 Bits: 1


		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "ReplicatedMovement":
					ReplicatedMovement = (FRepMovement)value;
					break;
				case "bEditorPlaced":
					bEditorPlaced = (bool)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

	}
}
