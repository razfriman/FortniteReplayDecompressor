using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Weapons
{
    public class Pistol : BaseWeapon
    {

    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_Vigilante_Athena.B_Pistol_Vigilante_Athena_C", ParseType.Normal)]
    public class StandardPistol : Pistol
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_Vigilante_Athena_HighTier.B_Pistol_Vigilante_Athena_HighTier_C", ParseType.Normal)]
    public class StandardPistolHighTier : StandardPistol
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_Vigilante_Supp_Athena.B_Pistol_Vigilante_Supp_Athena_C", ParseType.Normal)]
    public class SuppressedPistol : Pistol
    { 
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_Handcannon_Athena.B_Pistol_Handcannon_Athena_C", ParseType.Normal)]
    public class HandCannon : Pistol
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_DualPistol_Athena.B_DualPistol_Athena_C", ParseType.Normal)]
    public class DualPistols : Pistol
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_Scoped_Athena.B_Pistol_Scoped_Athena_C", ParseType.Normal)]
    public class ScopedRevolver : Pistol
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_SingleActionRevolver_Athena.B_Pistol_SingleActionRevolver_Athena_C", ParseType.Normal)]
    public class Revolver : Pistol
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_Revolver_Futuristic_Athena.B_Pistol_Revolver_Futuristic_Athena_C", ParseType.Normal)]
    public class RevolverHighTier : Revolver
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_SixShooter_Athena.B_Pistol_SixShooter_Athena_C", ParseType.Normal)]
    public class SixShooter : Pistol
    {
    }

    [NetFieldExportGroup("/Game/Athena/Items/Weapons/Creative/Ranged/B_Pistol_Flashlight.B_Pistol_Flashlight_C", ParseType.Normal)]
    public class FlashlightPistol : Pistol
    {
    }
}
