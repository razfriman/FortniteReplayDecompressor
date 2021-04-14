using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Weapons
{
    [NetFieldExportGroup("/PrimalGameplay/Items/Guns/Bone/BoneAssaultRifle/B_Assault_Bone_Athena_Weap.B_Assault_Bone_Athena_Weap_C", ParseType.Normal)]
    public class PrimalAssaultRifle : AssaultRifle
    {
    }


    [NetFieldExportGroup("/PrimalGameplay/Items/Guns/Bone/BoneSMG/B_SMG_Bone_Athena_Weap.B_SMG_Bone_Athena_Weap_C", ParseType.Normal)]
    public class PrimalSMG : SMG
    {
    }
    
    [NetFieldExportGroup("/PrimalGameplay/Items/Guns/Bone/BoneShotgun/B_Shotgun_Bone_Athena_Weap.B_Shotgun_Bone_Athena_Weap_C", ParseType.Normal)]
    public class PrimalShotgun : Shotgun
    {
    }
    
    [NetFieldExportGroup("/PrimalGameplay/Items/Guns/Bone/BoneRevolver/B_Revolver_Bone_Athena_Weap.B_Revolver_Bone_Athena_Weap_C", ParseType.Normal)]
    public class PrimalRevolver : Revolver
    {
    }
    
    [NetFieldExportGroup("/PrimalGameplay/Items/JunkGun/B_JunkGun.B_JunkGun_C", ParseType.Normal)]
    public class Recycler : BaseWeapon
    {
    }


    [NetFieldExportGroup("/PrimalGameplay/Items/Bows/Flame/B_Weap_Bow_Flame_Athena.B_Weap_Bow_Flame_Athena_C", ParseType.Normal)]
    public class PrimalFlameBow : CrossBows
    {
    }
    
    [NetFieldExportGroup("/PrimalGameplay/Items/Bows/ClusterBomb/B_Weap_Bow_ClusterBomb_Athena.B_Weap_Bow_ClusterBomb_Athena_C", ParseType.Normal)]
    public class MechanicalExplosiveBow : ExplosiveBow
    {
    }
    
    [NetFieldExportGroup("/PrimalGameplay/Items/Bows/Metal/B_Weap_Bow_Metal_Athena.B_Weap_Bow_Metal_Athena_C", ParseType.Normal)]
    public class MechanicalBow : ExplosiveBow
    {
    }





    [NetFieldExportGroup("/PrimalGameplay/Items/Guns/Scrap/ScrapAssaultRifle/B_Assault_Scrap_Athena.B_Assault_Scrap_Athena_C", ParseType.Normal)]
    public class MakeshiftAssaultRifle : AssaultRifle
    {
    }
    
    [NetFieldExportGroup("/PrimalGameplay/Items/Guns/Scrap/ScrapSMG/B_SMG_Scrap_Athena.B_SMG_Scrap_Athena_C", ParseType.Normal)]
    public class MakeshiftSMG : SMG
    {
    }

    [NetFieldExportGroup("/PrimalGameplay/Items/Guns/Scrap/ScrapShotgun/B_Shotgun_Scrap_Athena.B_Shotgun_Scrap_Athena_C", ParseType.Normal)]
    public class MakeshiftShotgun : Shotgun
    {
    }
    
    [NetFieldExportGroup("/PrimalGameplay/Items/Bows/Scrap/B_Weap_Bow_Athena_Scrap.B_Weap_Bow_Athena_Scrap_C", ParseType.Normal)]
    public class MakeshiftBow : CrossBows
    {
    }

    [NetFieldExportGroup("/PrimalGameplay/Items/Guns/Scrap/ScrapRevolver/B_Revolver_Scrap_Athena.B_Revolver_Scrap_Athena_C", ParseType.Normal)]
    public class MakeshiftRevolver : Revolver
    {
    }
}
