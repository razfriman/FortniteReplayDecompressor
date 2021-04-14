using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Weapons
{

    public class Throwable : BaseWeapon
    {
    }

    [NetFieldExportGroup("/Game/Abilities/Player/Generic/UtilityItems/B_Grenade_Shockwave_Athena.B_Grenade_Shockwave_Athena_C", ParseType.Normal)]
    public class ShockwaveGrenade : Throwable
    {

    }
}
