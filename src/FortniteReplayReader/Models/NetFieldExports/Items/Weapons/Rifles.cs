using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Weapons
{
    public class Rifles : BaseWeapon
    {
    }

    //Game/Weapons/FORT_Rifles/Blueprints/B_Rifle_NoScope_Athena.B_Rifle_NoScope_Athena_C ?
    
    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_Auto_Athena.B_Assault_Auto_Athena_C", ParseType.Normal)]
    public class AssaultRifle : Rifles
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_Auto_Zoom_SR_Child_Athena.B_Assault_Auto_Zoom_SR_Child_Athena_C", ParseType.Normal)]
    public class AssaultRifleHighTier : AssaultRifle
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_Suppressed_Athena.B_Assault_Suppressed_Athena_C", ParseType.Normal)]
    public class SuppressedAssaultRifle : Rifles
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_BurstBullpup_Athena.B_Assault_BurstBullpup_Athena_C", ParseType.Normal)]
    public class BurstRifle : Rifles
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_BurstBullpup_Athena_HighTier.B_Assault_BurstBullpup_Athena_HighTier_C", ParseType.Normal)]
    public class BurstRifleHighTier : BurstRifle
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_PistolCaliber_AR_Athena.B_Assault_PistolCaliber_AR_Athena_C", ParseType.Normal)]
    public class TacticalAssaultRifle : Rifles
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_Surgical_Thermal_Athena.B_Assault_Surgical_Thermal_Athena_C", ParseType.Normal)]
    public class ThermalRifle : Rifles
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_InfantryRifle_Athena.B_Assault_InfantryRifle_Athena_C", ParseType.Normal)]
    public class InfantryRifle : Rifles
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_InfantryRifle_SR_Athena.B_Assault_InfantryRifle_SR_Athena_C", ParseType.Normal)]
    public class InfantryRifleHighTier : InfantryRifle
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_Heavy_Athena.B_Assault_Heavy_Athena_C", ParseType.Normal)]
    public class HeavyAssaultRifle : Rifles
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_Heavy_SR_Athena.B_Assault_Heavy_SR_Athena_C", ParseType.Normal)]
    public class HeavyAssaultRifleHighTier : HeavyAssaultRifle
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/Assault/B_Assault_Surgical_Athena.B_Assault_Surgical_Athena_C", ParseType.Normal)]
    public class ScopedAssaultRifle : Rifles
    {
    }
}
