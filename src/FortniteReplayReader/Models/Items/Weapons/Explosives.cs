using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.Items.Weapons
{
    public class Explosives : BaseWeapon
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_RocketLaunchers/Blueprints/B_RocketLauncher_Generic_Athena.B_RocketLauncher_Generic_Athena_C", ParseType.Normal)]
    public class RocketLauncher : Explosives
    {

    }

    [NetFieldExportGroup("/Game/Weapons/FORT_RocketLaunchers/Blueprints/B_RocketLauncher_Generic_Athena_HighTier.B_RocketLauncher_Generic_Athena_HighTier_C", ParseType.Normal)]
    public class RocketLauncherHighTier : RocketLauncher
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_RocketLaunchers/Blueprints/B_RocketLauncher_Military_Athena.B_RocketLauncher_Military_Athena_C", ParseType.Normal)]
    public class QuadLauncher : Explosives
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_GrenadeLaunchers/Blueprints/B_GrenadeLauncher_Generic_Athena.B_GrenadeLauncher_Generic_Athena_C", ParseType.Normal)]
    public class GrenadeLauncher : Explosives
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_RocketLaunchers/Blueprints/B_Prj_Pumpkin_RPG_Athena_LowTier.B_Prj_Pumpkin_RPG_Athena_LowTier_C", ParseType.Normal)]
    public class PumpkinLauncher : Explosives
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_RocketLaunchers/Blueprints/B_Launcher_Pumpkin_RPG_Athena.B_Launcher_Pumpkin_RPG_Athena_C", ParseType.Normal)]
    public class PumpkinLauncherHighTier : PumpkinLauncher
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_GrenadeLaunchers/Blueprints/B_GrenadeLauncher_Prox_Athena.B_GrenadeLauncher_Prox_Athena_C", ParseType.Normal)]
    public class ProximityLauncher : Explosives
    {
    }
}
