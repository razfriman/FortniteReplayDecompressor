using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.Weapons
{
    public class Shotgun : Weapon
    {
    }

    //Need to fix
    [NetFieldExportGroup("/Game/Weapons/FORT_Shotguns/Blueprints/B_Shotgun_Standard_Athena.B_Shotgun_Standard_Athena_C", ParseType.Normal)]
    public class PumpShotgun : Shotgun
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Shotguns/Blueprints/B_Shotgun_Standard_TopTier_Athena.B_Shotgun_Standard_TopTier_Athena_C", ParseType.Normal)]
    public class PumpShotgunHighTier : PumpShotgun
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Shotguns/Blueprints/B_Shotgun_Break_Athena.B_Shotgun_Break_Athena_C", ParseType.Normal)]
    public class DoubleBarrelShotgun : Shotgun
    {
    }

    /* Need to figure out heavy + tactical shotguns */
    [NetFieldExportGroup("/Game/Weapons/FORT_Shotguns/Blueprints/B_Shotgun_SemiAuto_Athena.B_Shotgun_SemiAuto_Athena_C", ParseType.Normal)]
    public class HeavyShotgun : Shotgun
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Shotguns/Blueprints/B_Shotgun_HighSemiAuto_Athena.B_Shotgun_HighSemiAuto_Athena_C", ParseType.Normal)]
    public class TacticalShotgunHighTier : Shotgun
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Shotguns/Blueprints/B_Shotgun_Heavy_Athena.B_Shotgun_Heavy_Athena_C", ParseType.Normal)]
    public class TacticalShotgun : Shotgun
    {
    }
    /* End Check area */

    [NetFieldExportGroup("/Game/Weapons/FORT_Shotguns/Blueprints/B_Shotgun_Combat_Athena.B_Shotgun_Combat_Athena_C", ParseType.Normal)]
    public class CombatShotgun : Shotgun
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Shotguns/Blueprints/B_Shotgun_AutoDrum_Athena.B_Shotgun_AutoDrum_Athena_C", ParseType.Normal)]
    public class DrumShotgun : Shotgun
    {
    }

    ///Game/Weapons/FORT_Shotguns/Blueprints/B_Shotgun_SemiAuto_Athena.B_Shotgun_SemiAuto_Athena_C Heavy?
}
