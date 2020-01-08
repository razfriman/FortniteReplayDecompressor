using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FortniteReplayReader.Models.Enums;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class InventoryItem
    {
        public Weapon Weapon { get; internal set; }

        public ItemName Item { get; internal set; }
        public uint ItemDefinition { get; internal set; }
        public int Count { get; internal set; }
        public int LoadedAmmo { get; internal set; }

        internal UniqueItemId UniqueWeaponId { get; set; }

        //These can be updated later with the FortPickup groups
        public FVector InitialPosition { get; internal set; }
        public InventoryItem CombineTarget { get; internal set; }
        public Player LastDroppedBy { get; internal set; }

        public override string ToString()
        {
            return Item?.ToString();
        }
    }
}
