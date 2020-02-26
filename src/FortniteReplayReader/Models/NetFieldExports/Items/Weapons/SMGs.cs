using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Weapons
{
    public class SMG : BaseWeapon
    {

    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_Light_PDW_Athena.B_Pistol_Light_PDW_Athena_C", ParseType.Normal)]
    public class StandardSMG : SMG
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_PDW_Athena_HighTier.B_Pistol_PDW_Athena_HighTier_C", ParseType.Normal)]
    public class StandardSMGHighTier : SMG
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_PDW_Athena.B_Pistol_PDW_Athena_C", ParseType.Normal)]
    public class CompactSMG : SMG
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_PostApocalyptic_Athena.B_Pistol_PostApocalyptic_Athena_C", ParseType.Normal)]
    public class TacticalSMG : SMG
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_AutoDrum_Athena_Child.B_Assault_AutoDrum_Athena_Child_C", ParseType.Normal)]
    public class DrumGun : SMG
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_MidasDrum_Athena.B_Assault_MidasDrum_Athena_C", ParseType.Normal)]
    public class MidasDrumGun : DrumGun
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_BurstFireSMG_Athena.B_Pistol_BurstFireSMG_Athena_C", ParseType.Normal)]
    public class BurstSMG : SMG
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_RapidFireSMG_Athena.B_Pistol_RapidFireSMG_Athena_C", ParseType.Normal)]
    public class RapidFireSMG : SMG
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Pistols/Blueprints/B_Pistol_AutoHeavy_Athena_Supp_Child.B_Pistol_AutoHeavy_Athena_Supp_Child_C", ParseType.Normal)]
    public class SuppressedSMG : SMG
    {
    }
}
