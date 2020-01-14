using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Attributes;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Weapons
{
    public class Sniper : BaseWeapon
    { 
    }


    [NetFieldExportGroup("/Game/Weapons/FORT_Sniper/Blueprints/B_Prj_Bullet_Sniper.B_Prj_Bullet_Sniper_C", ParseType.Normal)]
    public class SniperProjectile : BaseProjectile
    {
        [NetFieldExport("FireStartLoc", RepLayoutCmdType.PropertyVector10)]
        public FVector FireStartLoc { get; set; }
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/B_Rifle_Sniper_Athena.B_Rifle_Sniper_Athena_C", ParseType.Normal)]
    public class BoltSniper : Sniper
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/B_Rifle_Sniper_Athena_HighTier.B_Rifle_Sniper_Athena_HighTier_C", ParseType.Normal)]
    public class BoltSniperHighTier : BoltSniper
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/B_Rifle_Sniper_Heavy_Athena.B_Rifle_Sniper_Heavy_Athena_C", ParseType.Normal)]
    public class HeavySniper : BoltSniper
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/B_Rifle_Sniper_Suppressed_Athena.B_Rifle_Sniper_Suppressed_Athena_C", ParseType.Normal)]
    public class SuppressedSniper : Sniper
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/B_Rifle_Sniper_Auto_Athena_Child.B_Rifle_Sniper_Auto_Athena_Child_C", ParseType.Normal)]
    public class SemiAutoSniper : Sniper
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/B_Rifle_Sniper_Auto_Suppressed_Athena.B_Rifle_Sniper_Auto_Suppressed_Athena_C", ParseType.Normal)]
    public class SuppressedAutoSniper : Sniper
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/B_Rifle_Sniper_Weather_Athena.B_Rifle_Sniper_Weather_Athena_C", ParseType.Normal)]
    public class StormRifle : Sniper
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Rifles/Blueprints/B_Rifle_NoScope_Athena.B_Rifle_NoScope_Athena_C", ParseType.Normal)]
    public class HuntingRifle : Sniper
    {
    }
}
