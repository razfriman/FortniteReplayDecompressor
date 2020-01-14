using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Attributes;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Weapons
{
    public class CrossBows : BaseWeapon
    {

    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Crossbows/Blueprints/B_Hookgun_Athena.B_Hookgun_Athena_C", ParseType.Normal)]
    public class Grappler : CrossBows
    {

    }


    [NetFieldExportGroup("/Game/Weapons/FORT_Crossbows/Blueprints/B_ExplosiveBow_Athena.B_ExplosiveBow_Athena_C", ParseType.Normal)]
    public class ExplosiveBow : CrossBows
    {
        [NetFieldExport("bIsChargingWeapon", RepLayoutCmdType.PropertyBool)]
        public bool? bIsChargingWeapon { get; set; }

        [NetFieldExport("ChargeStatusPack", RepLayoutCmdType.Property)]
        public DebuggingObject ChargeStatusPack { get; set; }
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Crossbows/Blueprints/B_DemonHunter_Crossbow_Athena.B_DemonHunter_Crossbow_Athena_C", ParseType.Normal)]
    public class FiendHunterBow : CrossBows
    {

    }

    [NetFieldExportGroup("/Game/Weapons/FORT_Crossbows/Blueprints/B_Valentine_Crossbow_Athena.B_Valentine_Crossbow_Athena_C", ParseType.Normal)]
    public class CupidsCrossBow : CrossBows
    {

    }
}
