using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Consumables
{
    public class ShieldConsumable : Consumable
    {
    }

    [NetFieldExportGroup("/Game/Abilities/Player/Generic/UtilityItems/B_ConsumableSmall_MiniShield_Athena.B_ConsumableSmall_MiniShield_Athena_C", ParseType.Normal)]
    public class MiniShields : ShieldConsumable
    {
    }

    [NetFieldExportGroup("/Game/Abilities/Player/Generic/UtilityItems/B_ConsumableSmall_HalfShield_Athena.B_ConsumableSmall_HalfShield_Athena_C", ParseType.Normal)]
    public class HalfPot : ShieldConsumable
    {
    }

    [NetFieldExportGroup("/Game/Abilities/Player/Generic/UtilityItems/B_UtilityItem_Generic_Athena.B_UtilityItem_Generic_Athena_C", ParseType.Normal)]
    public class ChugJug : ShieldConsumable
    {
    }
    
    [NetFieldExportGroup("/Game/Athena/Items/Consumables/ForagedItemVersions/ShieldMushroom/B_ShieldMushroom_Weap_Athena.B_ShieldMushroom_Weap_Athena_C", ParseType.Normal)]
    public class ShieldMushroom : ShieldConsumable
    {
    }
}
