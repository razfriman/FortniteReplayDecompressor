using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;

namespace FortniteReplayReader.Models.ClassNetCaches
{
    [ClassNetCache("FortInventory_ClassNetCache")]
    public class FortInventoryCache
    {
        [ClassNetCacheProperty("Inventory", "/Script/FortniteGame.FortInventory")]
        public object Inventory { get; set; }
    }
}
