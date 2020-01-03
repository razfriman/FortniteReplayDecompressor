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
        public uint Channel { get; set; }
        public ItemInfo ItemInfo => new ItemInfo(ItemIdName);

        public string ItemIdName { get; internal set; }
        public uint ItemDefinition { get; internal set; }
        public int Count { get; internal set; }
        public int Level { get; internal set; }
        public int Ammo { get; internal set; }
        //public bool IsDirty { get; set; }

        public FVector InitialPosition { get; internal set; }

        public InventoryItem CombineTarget { get; internal set; }

        public Player CurrentOwner { get; internal set; }
        public Player LastDroppedBy { get; internal set; }


        public override string ToString()
        {
            return ItemIdName;
        }
    }

    public class ItemInfo
    {
        public ItemRarity Rarity { get; internal set; } = ItemRarity.Common;
        public string WeaponName { get; internal set; }

        public ItemInfo(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return;
            }

            string[] parts = name.Split('_');

            if (parts[0] != "WID")
            {
                return;
            }

            switch (parts[1])
            {
                case "Athena":
                    if (parts[2] == "FloppingRabbit")
                    {
                        WeaponName = "Fishing Pole";
                    }
                    break;
                case "Pistol":
                    if (parts[2] == "SemiAuto")
                    {
                        WeaponName = "Pistol";
                    }
                    else if (parts[2] == "AutoHeavyPDW")
                    {
                        WeaponName = "SMG";
                    }
                    break;
                case "Shotgun":
                    if (parts[2] == "Standard")
                    {
                        WeaponName = "Pump Shotgun";
                    }
                    else if (parts[2] == "SemiAuto")
                    {
                        WeaponName = "Tactical Shotgun";
                    }
                    break;
                case "Assault":
                    if (parts[2] == "SemiAuto" || parts[2] == "SemiAutoHigh")
                    {
                        WeaponName = "Burst Rifle";
                    }
                    else if (parts[2] == "Auto" || parts[2] == "AutoHigh")
                    {
                        WeaponName = "Assault Rifle";
                    }
                    break;
                case "Sniper":
                    if (parts[2] == "BoltAction")
                    {
                        WeaponName = "Bolt Action Sniper";
                    }
                    break;
                case "Hook":
                    if (parts[2] == "Gun")
                    {
                        WeaponName = "Grappler";
                    }
                    break;
            }

            string rarity = "C";

            for(int i = 3; i < parts.Length; i++)
            {
                if(parts[i] == "Athena")
                {
                    rarity = parts[i + 1];

                    break;
                }
                else if(parts[i] == "Ore")
                {
                    rarity = parts[i - 1];
                }
            }

            switch (rarity)
            {
                case "C":
                    Rarity = ItemRarity.Common;
                    break;
                case "UC":
                    Rarity = ItemRarity.Uncommon;
                    break;
                case "R":
                    Rarity = ItemRarity.Rare;
                    break;
                case "VR":
                    Rarity = ItemRarity.Epic;
                    break;
                case "SR":
                    Rarity = ItemRarity.Legendary;
                    break;
            }
        }

        public override string ToString()
        {
            return $"{Rarity} {WeaponName}";
        }
    }
}
