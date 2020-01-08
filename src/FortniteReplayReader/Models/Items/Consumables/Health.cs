using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.Items.Consumables
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
}
