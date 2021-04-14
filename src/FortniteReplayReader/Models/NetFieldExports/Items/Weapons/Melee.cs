using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Weapons
{
    //Melee weapons have individual types for each skin
    public class Melee : BaseWeapon
    {

    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Melee/Blueprints/B_Athena_Pickaxe_Generic.B_Athena_Pickaxe_Generic_C", ParseType.Normal)]
    [PartialNetFieldExportGroup("/Game/Weapons/FORT_Melee/Blueprints")]
    public class PickAxe : Melee
    {

    }
}
