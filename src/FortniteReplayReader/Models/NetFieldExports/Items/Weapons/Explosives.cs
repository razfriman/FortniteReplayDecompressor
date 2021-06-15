using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Attributes;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Weapons
{
    public class Explosives : BaseWeapon
    {
    }

    public class Launcher : Explosives
    {

    }

    #region RocketLauncher
    [NetFieldExportGroup("/Game/Weapons/FORT_RocketLaunchers/Blueprints/B_RocketLauncher_Generic_Athena.B_RocketLauncher_Generic_Athena_C", ParseType.Normal)]
    public class RocketLauncher : Launcher
    {
    }


    [NetFieldExportGroup("/Game/Weapons/FORT_RocketLaunchers/Blueprints/B_RocketLauncher_Generic_Athena_HighTier.B_RocketLauncher_Generic_Athena_HighTier_C", ParseType.Normal)]
    public class RocketLauncherHighTier : RocketLauncher
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_RocketLaunchers/Blueprints/B_Prj_Ranged_Rocket_Athena_LowTier.B_Prj_Ranged_Rocket_Athena_LowTier_C", ParseType.Normal)]
    public class RocketLauncherProjectile : BaseLauncherProjectile
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_RocketLaunchers/Blueprints/B_Prj_Ranged_Rocket_Athena.B_Prj_Ranged_Rocket_Athena_C", ParseType.Normal)]
    public class RocketLauncherProjectileHighTier : RocketLauncherProjectile
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_RocketLaunchers/Blueprints/B_Prj_Pumpkin_RPG_Athena_LowTier.B_Prj_Pumpkin_RPG_Athena_LowTier_C", ParseType.Normal)]
    public class PumpkinLauncher : Launcher
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_RocketLaunchers/Blueprints/B_Launcher_Pumpkin_RPG_Athena.B_Launcher_Pumpkin_RPG_Athena_C", ParseType.Normal)]
    public class PumpkinLauncherHighTier : PumpkinLauncher
    {
    }
    #endregion

    #region QuadLauncher

    [NetFieldExportGroup("/Game/Weapons/FORT_RocketLaunchers/Blueprints/B_RocketLauncher_Military_Athena.B_RocketLauncher_Military_Athena_C", ParseType.Normal)]
    public class QuadLauncher : Launcher
    {
    }

    #endregion

    #region GrenadeLauncher

    [NetFieldExportGroup("/Game/Weapons/FORT_GrenadeLaunchers/Blueprints/B_GrenadeLauncher_Generic_Athena.B_GrenadeLauncher_Generic_Athena_C", ParseType.Normal)]
    public class GrenadeLauncher : Launcher
    {
    }

    #endregion

    #region ProximityLauncher

    [NetFieldExportGroup("/Game/Weapons/FORT_GrenadeLaunchers/Blueprints/B_GrenadeLauncher_Prox_Athena.B_GrenadeLauncher_Prox_Athena_C", ParseType.Normal)]
    public class ProximityLauncher : Launcher
    {
    }

    #endregion

    #region Frag Grenades

    [NetFieldExportGroup("/Game/Abilities/Player/Generic/UtilityItems/B_Grenade_Frag_Athena.B_Grenade_Frag_Athena_C", ParseType.Normal)]
    public class FragGrenade : Explosives
    {
    }


    [NetFieldExportGroup("/Game/Athena/Items/Consumables/Grenade/B_Prj_Athena_FragGrenade.B_Prj_Athena_FragGrenade_C", ParseType.Normal)]
    public class FragGrenadeProjectile : BaseExplosiveProjectile
    {
    }

    #endregion

    #region MotorBoat

    [NetFieldExportGroup("/Game/Athena/Items/Weapons/Vehicles/MeatballWeapon/B_Meatball_Launcher_Athena.B_Meatball_Launcher_Athena_C", ParseType.Normal)]
    public class MotorBoatWeapon : Launcher
    {
        [NetFieldExport("HostVehicleCachedActor", RepLayoutCmdType.Property)]
        public ActorGUID HostVehicleCachedActor { get; set; }

        [NetFieldExport("HostVehicleSeatIndexCached", RepLayoutCmdType.PropertyUInt32)]
        public uint? HostVehicleSeatIndexCached { get; set; }
    }

    [NetFieldExportGroup("/Game/Athena/Items/Weapons/Vehicles/MeatballWeapon/B_Prj_Meatball_Missile.B_Prj_Meatball_Missile_C", ParseType.Normal)]
    public class MotorBoatProjectile : BaseLauncherProjectile
    {
        //Possibly part of base?
        [NetFieldExport("SurfaceType", RepLayoutCmdType.Enum)]
        public int? SurfaceType { get; set; }
    }

    #endregion

    #region C4

    [NetFieldExportGroup("/Game/Abilities/Player/Generic/UtilityItems/B_UtilityItem_Detonator_Athena.B_UtilityItem_Detonator_Athena_C", ParseType.Normal)]
    public class C4 : Explosives
    {
    }

    #endregion
}
