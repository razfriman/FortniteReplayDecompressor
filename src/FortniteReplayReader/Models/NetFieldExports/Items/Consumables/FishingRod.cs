using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Consumables
{
    [NetFieldExportGroup("/Game/Athena/Items/Consumables/FloppingRabbit/B_FloppingRabbit_Weap_Athena.B_FloppingRabbit_Weap_Athena_C", ParseType.Normal)]
    public class FishingRod : Consumable
    {
    }

    [NetFieldExportGroup("/Game/Athena/Items/Consumables/FloppingRabbit/B_Athena_FloppingRabbit_Wire.B_Athena_FloppingRabbit_Wire_C", ParseType.Normal)]
    public class FishingRodWire : Consumable
    {
    }

    [NetFieldExportGroup("/Game/Athena/Items/Consumables/HappyGhost/B_HappyGhost_Athena.B_HappyGhost_Athena_C", ParseType.Normal)]
    public class Harpoon : Weapon
    {
    }
}
