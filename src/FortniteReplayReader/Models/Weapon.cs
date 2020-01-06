using System;
using System.Collections.Generic;
using System.Text;

namespace FortniteReplayReader.Models
{
    public class Weapon
    {
        public Player Owner { get; internal set; }
        public uint Ammo { get; internal set; }
        public uint WeaponLevel { get; internal set; }
        public float LastFireTime { get; internal set; }
        public bool IsEquipping { get; internal set; }
        public bool IsReloading { get; internal set; }
        public uint ItemId { get; internal set; }
        public ItemName Item { get; internal set; }
        public InventoryItem InventoryItem { get; internal set; }

        internal UniqueItemId UniqueItemId { get; set; }

        public override string ToString()
        {
            return Item?.ToString();
        }
    }
}
