using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches
{
    [NetFieldExportRPC("FortInventory_ClassNetCache", ParseType.Normal)]
    public class FortInventoryCache
    {
        [NetFieldExportRPCProperty("Inventory", "/Script/FortniteGame.FortInventory")]
        public object Inventory { get; set; }
    }
}
