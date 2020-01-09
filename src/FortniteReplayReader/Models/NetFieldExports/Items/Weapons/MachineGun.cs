using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.Items.Weapons
{
    public class MachineGun : BaseWeapon
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Minigun_Athena.B_Minigun_Athena_C", ParseType.Normal)]
    public class Minigun : MachineGun
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_LMG_SAW_Athena.B_Assault_LMG_SAW_Athena_C", ParseType.Normal)]
    public class LightMachineGun : MachineGun
    {
    }
}
