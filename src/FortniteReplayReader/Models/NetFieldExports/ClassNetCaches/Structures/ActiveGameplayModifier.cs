using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Structures
{
    [NetFieldExportGroup("/Script/FortniteGame.ActiveGameplayModifier", ParseType.Debug)]
    public class ActiveGameplayModifier : INetFieldExportGroup
    {
        [NetFieldExport("ModifierDef", RepLayoutCmdType.Property)]
        public DebuggingObject ModifierDef { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "ModifierDef":
					ModifierDef = (DebuggingObject)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }
}
