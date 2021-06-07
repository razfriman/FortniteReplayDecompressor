using FortniteReplayReader.Models.NetFieldExports.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Weapons
{
    public class Healing : BaseWeapon
    {
    }

    [NetFieldExportGroup("/Game/Athena/Items/Gameplay/Lotus/Mustache/B_Ranged_Lotus_Mustache.B_Ranged_Lotus_Mustache_C", ParseType.Normal)]
    public class BandageBazooka : Healing
    {
        [NetFieldExport("OverheatState", RepLayoutCmdType.Enum)]
        public EFortWeaponOverheatState OverheatState { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "OverheatState":
					OverheatState = (EFortWeaponOverheatState)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }
}
