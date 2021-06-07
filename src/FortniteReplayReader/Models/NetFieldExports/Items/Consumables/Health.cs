using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Consumables
{
    public class HealthConsumable : Consumable
    { }

    [NetFieldExportGroup("/Game/Abilities/Player/Generic/UtilityItems/B_ConsumableSmall_Bandages_Athena.B_ConsumableSmall_Bandages_Athena_C", ParseType.Normal)]
    public class Bandages : HealthConsumable
    {
    }

    [NetFieldExportGroup("/Game/Abilities/Player/Generic/UtilityItems/B_ConsumableSmall_Medkit_Athena.B_ConsumableSmall_Medkit_Athena_C", ParseType.Normal)]
    public class Medkit : HealthConsumable
    {
    }

    [NetFieldExportGroup("/Game/Athena/Items/Consumables/ForagedItemVersions/Cabbage/B_Cabbage_Weap_Athena.B_Cabbage_Weap_Athena_C", ParseType.Normal)]
    public class Cabbage : HealthConsumable
    {
    }

    [NetFieldExportGroup("/Game/Athena/Items/Consumables/ForagedItemVersions/Banana/B_Banana_Weap_Athena.B_Banana_Weap_Athena_C", ParseType.Normal)]
    public class Banana : HealthConsumable
    {
    }

    [NetFieldExportGroup("/PrimalGameplay/Items/Consumables/Meat/B_Meat_Weap_Athena.B_Meat_Weap_Athena_C", ParseType.Normal)]
    public class Meat : HealthConsumable
    {
    }

    [NetFieldExportGroup("/Game/Athena/Items/Consumables/ForagedItemVersions/Coconut/B_Coconut_Weap_Athena.B_Coconut_Weap_Athena_C", ParseType.Normal)]
    public class Coconut : HealthConsumable
    {
    }
}
