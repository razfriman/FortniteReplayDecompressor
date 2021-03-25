using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Vehicles
{
	public abstract class BaseVehicle : INetFieldExportGroup
	{
		[NetFieldExport("RemoteRole", RepLayoutCmdType.Enum)]
		public int? RemoteRole { get; set; }

		[NetFieldExport("Role", RepLayoutCmdType.Enum)]
		public int? Role { get; set; }

		[NetFieldExport("Instigator", RepLayoutCmdType.Property)]
		public ActorGUID Instigator { get; set; }

		[NetFieldExport("ReplicatedMovement", RepLayoutCmdType.RepMovement)]
		public FRepMovement ReplicatedMovement{ get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch (property)
			{
				case "RemoteRole":
					RemoteRole = (int)value;
					break;
				case "Role":
					Role = (int)value;
					break;
				case "Instigator":
					Instigator = (ActorGUID)value;
					break;
				case "ReplicatedMovement":
					ReplicatedMovement = (FRepMovement)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}
	}
}
