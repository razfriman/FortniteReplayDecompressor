using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Functions
{
    [NetFieldExportGroup("/Script/FortniteGame.FortPlayerPawnAthena:FastSharedReplication")]
    public class FastSharedReplication : INetFieldExportGroup
    {
        [NetFieldExport("SharedRepMovement", RepLayoutCmdType.RepMovement)]
        public FRepMovement SharedRepMovement { get; set; } //Type:  Bits: 1

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "SharedRepMovement":
					SharedRepMovement = (FRepMovement)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }
}
