using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
	[NetFieldExportGroup("/Game/Athena/SafeZone/SafeZoneIndicator.SafeZoneIndicator_C")]
	public class SafeZoneIndicatorC : INetFieldExportGroup
	{
		[NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
		public object RemoteRole { get; set; } //Type:  Bits: 2

		[NetFieldExport("Role", RepLayoutCmdType.Ignore)]
		public object Role { get; set; } //Type:  Bits: 2

		[NetFieldExport("LastRadius", RepLayoutCmdType.PropertyFloat)]
		public float? LastRadius { get; set; } //Type: float Bits: 32

		[NetFieldExport("NextRadius", RepLayoutCmdType.PropertyFloat)]
		public float? NextRadius { get; set; } //Type: float Bits: 32

		[NetFieldExport("NextNextRadius", RepLayoutCmdType.PropertyFloat)]
		public float? NextNextRadius { get; set; } //Type: float Bits: 32

		[NetFieldExport("LastCenter", RepLayoutCmdType.PropertyVector100)]
		public FVector? LastCenter { get; set; } //Type: FVector_NetQuantize100 Bits: 74

		[NetFieldExport("NextCenter", RepLayoutCmdType.PropertyVector100)]
		public FVector? NextCenter { get; set; } //Type: FVector_NetQuantize100 Bits: 74

		[NetFieldExport("NextNextCenter", RepLayoutCmdType.PropertyVector100)]
		public FVector? NextNextCenter { get; set; } //Type: FVector_NetQuantize100 Bits: 74

		[NetFieldExport("SafeZoneStartShrinkTime", RepLayoutCmdType.PropertyFloat)]
		public float? SafeZoneStartShrinkTime { get; set; } //Type: float Bits: 32

		[NetFieldExport("SafeZoneFinishShrinkTime", RepLayoutCmdType.PropertyFloat)]
		public float? SafeZoneFinishShrinkTime { get; set; } //Type: float Bits: 32

		[NetFieldExport("bPausedForPreview", RepLayoutCmdType.PropertyBool)]
		public bool? bPausedForPreview { get; set; } //Type:  Bits: 1

		[NetFieldExport("MegaStormDelayTimeBeforeDestruction", RepLayoutCmdType.PropertyFloat)]
		public float? MegaStormDelayTimeBeforeDestruction { get; set; } //Type: float Bits: 32

		[NetFieldExport("Radius", RepLayoutCmdType.PropertyFloat)]
		public float? Radius { get; set; } //Type:  Bits: 32

		[NetFieldExport("TimeRemainingWhenPhasePaused", RepLayoutCmdType.PropertyFloat)]
		public float? TimeRemainingWhenPhasePaused { get; set; } //Type:  Bits: 32


		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "RemoteRole":
					RemoteRole = value;
					break;
				case "Role":
					Role = value;
					break;
				case "LastRadius":
					LastRadius = (float)value;
					break;
				case "NextRadius":
					NextRadius = (float)value;
					break;
				case "NextNextRadius":
					NextNextRadius = (float)value;
					break;
				case "LastCenter":
					LastCenter = (FVector)value;
					break;
				case "NextCenter":
					NextCenter = (FVector)value;
					break;
				case "NextNextCenter":
					NextNextCenter = (FVector)value;
					break;
				case "SafeZoneStartShrinkTime":
					SafeZoneStartShrinkTime = (float)value;
					break;
				case "SafeZoneFinishShrinkTime":
					SafeZoneFinishShrinkTime = (float)value;
					break;
				case "bPausedForPreview":
					bPausedForPreview = (bool)value;
					break;
				case "MegaStormDelayTimeBeforeDestruction":
					MegaStormDelayTimeBeforeDestruction = (float)value;
					break;
				case "Radius":
					Radius = (float)value;
					break;
				case "TimeRemainingWhenPhasePaused":
					TimeRemainingWhenPhasePaused = (float)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

	}
}
