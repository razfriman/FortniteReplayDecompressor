using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    [NetFieldExportGroup("/Game/Creative/Items/Gameplay/FortCreativePickup/Fort_Pickup_Creative.Fort_Pickup_Creative_C", ParseType.Full)]
    public class FortPickupCreative : FortPickup
    {
    }
}
