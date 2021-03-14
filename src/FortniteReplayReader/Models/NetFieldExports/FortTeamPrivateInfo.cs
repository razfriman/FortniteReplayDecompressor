using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    [NetFieldExportGroup("/Script/FortniteGame.FortTeamPrivateInfo", ParseType.Full)]
    public class FortTeamPrivateInfo : INetFieldExportGroup
    {
        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public int? RemoteRole { get; set; } //Type:  Bits: 2

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public int? Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("Owner", RepLayoutCmdType.PropertyObject)]
        public ActorGUID Owner { get; set; } //Type: AActor* Bits: 16

        [NetFieldExport("Value", RepLayoutCmdType.PropertyFloat)]
        public float? Value { get; set; }

        [NetFieldExport("PlayerID", RepLayoutCmdType.PropertyNetId)]
        public string PlayerID { get; set; }

        [NetFieldExport("PlayerState", RepLayoutCmdType.Property)]
        public ActorGUID PlayerState { get; set; }

        [NetFieldExport("LastRepLocation", RepLayoutCmdType.PropertyVector100)]
        public FVector LastRepLocation { get; set; }

        [NetFieldExport("LastRepYaw", RepLayoutCmdType.PropertyFloat)]
        public float? LastRepYaw { get; set; }

        [NetFieldExport("PawnStateMask", RepLayoutCmdType.Enum)]
        public int? PawnStateMask { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "RemoteRole":
					RemoteRole = (int)value;
					break;
				case "Role":
					Role = (int)value;
					break;
				case "Owner":
					Owner = (ActorGUID)value;
					break;
				case "Value":
					Value = (float)value;
					break;
				case "PlayerID":
					PlayerID = (string)value;
					break;
				case "PlayerState":
					PlayerState = (ActorGUID)value;
					break;
				case "LastRepLocation":
					LastRepLocation = (FVector)value;
					break;
				case "LastRepYaw":
					LastRepYaw = (float)value;
					break;
				case "PawnStateMask":
					PawnStateMask = (int)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }
}
