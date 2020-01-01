using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches
{
    [NetFieldExportRPC("FortInventory_ClassNetCache")]
    public class FortInventoryCache
    {
        [NetFieldExportRPCProperty("Inventory", "/Script/FortniteGame.FortInventory")]
        public object Inventory { get; set; }
    }
}
