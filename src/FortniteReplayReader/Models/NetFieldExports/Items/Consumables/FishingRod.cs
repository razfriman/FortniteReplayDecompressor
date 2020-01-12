using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Consumables
{
    [NetFieldExportGroup("/Game/Athena/Items/Consumables/FloppingRabbit/B_FloppingRabbit_Weap_Athena.B_FloppingRabbit_Weap_Athena_C", ParseType.Normal)]
    public class FishingRod : Consumable
    {
        [NetFieldExport("Projectile", RepLayoutCmdType.Property)]
        public NetworkGUID Projectile { get; set; }

        [NetFieldExport("Wire", RepLayoutCmdType.Property)]
        public NetworkGUID Wire { get; set; }

        [NetFieldExport("HideBobber", RepLayoutCmdType.PropertyBool)]
        public bool? HideBobber { get; set; }

        [NetFieldExport("OneHandGrip", RepLayoutCmdType.PropertyBool)]
        public bool? OneHandGrip { get; set; }
    }

    [NetFieldExportGroup("/Game/Athena/Items/Consumables/FloppingRabbit/B_Athena_FloppingRabbit_Wire.B_Athena_FloppingRabbit_Wire_C", ParseType.Normal)]
    public class FishingRodWire : Consumable
    {
        [NetFieldExport("ReplicatedMovement", RepLayoutCmdType.RepMovement)]
        public FRepMovement ReplicatedMovement { get; set; }

        [NetFieldExport("ProjectileActor", RepLayoutCmdType.Property)]
        public NetworkGUID ProjectileActor { get; set; }

        [NetFieldExport("PlayerPawn", RepLayoutCmdType.Property)]
        public NetworkGUID PlayerPawn { get; set; }
    }

    [NetFieldExportGroup("/Game/Athena/Items/Consumables/HappyGhost/B_HappyGhost_Athena.B_HappyGhost_Athena_C", ParseType.Normal)]
    public class Harpoon : Weapon
    {
    }
}
