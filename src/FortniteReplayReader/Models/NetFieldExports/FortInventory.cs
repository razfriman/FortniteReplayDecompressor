using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
	[NetFieldExportGroup("/Script/FortniteGame.FortInventory")]
	public class FortInventory : INetFieldExportGroup
	{
		[NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
		public object RemoteRole { get; set; } //Type:  Bits: 2

		[NetFieldExport("Owner", RepLayoutCmdType.PropertyObject)]
		public uint? Owner { get; set; } //Type: AActor* Bits: 8

		[NetFieldExport("Role", RepLayoutCmdType.Ignore)]
		public object Role { get; set; } //Type:  Bits: 2

		[NetFieldExport("ReplayPawn", RepLayoutCmdType.PropertyObject)]
		public uint? ReplayPawn { get; set; } //Type:  Bits: 16

	}
}
