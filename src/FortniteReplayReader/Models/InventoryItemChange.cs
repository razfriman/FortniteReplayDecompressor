using System;
using System.Collections.Generic;
using System.Text;

namespace FortniteReplayReader.Models
{
    public class InventoryItemChange
    {
        public InventoryItem Item { get; internal set; }
        public ItemChangeState State { get; internal set; }
        public float WorldTime { get; internal set; }

        public override string ToString()
        {
            return Item.ToString();
        }
    }

    public enum ItemChangeState { PickedUp, Dropped };
}
