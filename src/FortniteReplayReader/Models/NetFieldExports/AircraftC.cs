using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
	[NetFieldExportGroup("/Game/Athena/Aircraft/AthenaAircraft.AthenaAircraft_C")]
	public class AircraftC : INetFieldExportGroup
	{
		[NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
		public object RemoteRole { get; set; } //Type:  Bits: 2

		[NetFieldExport("Role", RepLayoutCmdType.Ignore)]
		public object Role { get; set; } //Type:  Bits: 2

		[NetFieldExport("JumpFlashCount", RepLayoutCmdType.PropertyInt)]
		public int? JumpFlashCount { get; set; } //Type: int32 Bits: 32

		[NetFieldExport("FlightStartLocation", RepLayoutCmdType.PropertyVector100)]
		public FVector FlightStartLocation { get; set; } //Type: FVector_NetQuantize100 Bits: 80

		[NetFieldExport("FlightStartRotation", RepLayoutCmdType.PropertyRotator)]
		public FRotator FlightStartRotation { get; set; } //Type: FRotator Bits: 19

		[NetFieldExport("FlightSpeed", RepLayoutCmdType.PropertyFloat)]
		public float? FlightSpeed { get; set; } //Type: float Bits: 32

		[NetFieldExport("TimeTillFlightEnd", RepLayoutCmdType.PropertyFloat)]
		public float? TimeTillFlightEnd { get; set; } //Type: float Bits: 32

		[NetFieldExport("TimeTillDropStart", RepLayoutCmdType.PropertyFloat)]
		public float? TimeTillDropStart { get; set; } //Type: float Bits: 32

		[NetFieldExport("TimeTillDropEnd", RepLayoutCmdType.PropertyFloat)]
		public float? TimeTillDropEnd { get; set; } //Type: float Bits: 32

		[NetFieldExport("FlightStartTime", RepLayoutCmdType.PropertyFloat)]
		public float? FlightStartTime { get; set; } //Type: float Bits: 32

		[NetFieldExport("FlightEndTime", RepLayoutCmdType.PropertyFloat)]
		public float? FlightEndTime { get; set; } //Type: float Bits: 32

		[NetFieldExport("DropStartTime", RepLayoutCmdType.PropertyFloat)]
		public float? DropStartTime { get; set; } //Type: float Bits: 32

		[NetFieldExport("DropEndTime", RepLayoutCmdType.PropertyFloat)]
		public float? DropEndTime { get; set; } //Type: float Bits: 32

		[NetFieldExport("ReplicatedFlightTimestamp", RepLayoutCmdType.PropertyFloat)]
		public float? ReplicatedFlightTimestamp { get; set; } //Type: float Bits: 32

		[NetFieldExport("AircraftIndex", RepLayoutCmdType.PropertyUInt32)]
		public uint? AircraftIndex { get; set; } //Type:  Bits: 32


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
				case "JumpFlashCount":
					JumpFlashCount = (int)value;
					break;
				case "FlightStartLocation":
					FlightStartLocation = (FVector)value;
					break;
				case "FlightStartRotation":
					FlightStartRotation = (FRotator)value;
					break;
				case "FlightSpeed":
					FlightSpeed = (float)value;
					break;
				case "TimeTillFlightEnd":
					TimeTillFlightEnd = (float)value;
					break;
				case "TimeTillDropStart":
					TimeTillDropStart = (float)value;
					break;
				case "TimeTillDropEnd":
					TimeTillDropEnd = (float)value;
					break;
				case "FlightStartTime":
					FlightStartTime = (float)value;
					break;
				case "FlightEndTime":
					FlightEndTime = (float)value;
					break;
				case "DropStartTime":
					DropStartTime = (float)value;
					break;
				case "DropEndTime":
					DropEndTime = (float)value;
					break;
				case "ReplicatedFlightTimestamp":
					ReplicatedFlightTimestamp = (float)value;
					break;
				case "AircraftIndex":
					AircraftIndex = (uint)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

	}
}
