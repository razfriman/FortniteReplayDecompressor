using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Structures
{
    [NetFieldExportGroup("/Script/FortniteGame.FortPropertyOverrideReplShared", ParseType.Debug)]
    public class FortPropertyOverrideReplShared : INetFieldExportGroup
    {
        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public object RemoteRole { get; set; } //Type:  Bits: 2

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public object Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("Owner", RepLayoutCmdType.Property)]
        public NetworkGUID Owner { get; set; }

        [NetFieldExport("PropertyScopedName", RepLayoutCmdType.PropertyString)]
        public string PropertyScopedName { get; set; }

        [NetFieldExport("PropertyData", RepLayoutCmdType.Property)]
        public DebuggingObject PropertyData { get; set; }

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
				case "Owner":
					Owner = (NetworkGUID)value;
					break;
				case "PropertyScopedName":
					PropertyScopedName = (string)value;
					break;
				case "PropertyData":
					PropertyData = (DebuggingObject)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }
}
