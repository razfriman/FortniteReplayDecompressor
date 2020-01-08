using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FortniteReplayReader.Models.Enums;

namespace FortniteReplayReader.Models
{
    public class ItemName
    {
        private enum NextWeaponIdValue { Unknown, Type, WeaponName, Rarity };

        public string ItemId { get; private set; }
        public ItemRarity Rarity { get; private set; }
        public string WeaponName { get; private set; }

        private static Dictionary<string, string> _weaponIds = new Dictionary<string, string>();

        static ItemName()
        {
            _weaponIds.Add("WID_ExplosiveBow", "Explosive Bow");
            _weaponIds.Add("WID_Pistol_Scoped", "Scoped Revolver");
            _weaponIds.Add("WID_Pistol_HandCannon", "Hand Cannon");
            _weaponIds.Add("WID_Pistol_Standard", "Pistol");
            _weaponIds.Add("WID_Pistol_SixShooter", "Six Shooter");
            _weaponIds.Add("WID_Shotgun_Combat", "Combat Shotgun");
            _weaponIds.Add("WID_Shotgun_HighSemiAuto", "Tactical Shotgun");
            _weaponIds.Add("WID_Shotgun_BreakBarrel", "Double Barrel Shotgun");
            _weaponIds.Add("WID_Shotgun_SlugFire", "Heavy Shotgun");
            _weaponIds.Add("WID_Shotgun_Standard", "Pump Shotgun");
            _weaponIds.Add("WID_Pistol_AutoHeavyPDW", "SMG");
            _weaponIds.Add("WID_SMG", "Compact SMG");
            _weaponIds.Add("WID_Pistol_Scavenger", "Tactical SMG");
            _weaponIds.Add("WID_Assault_PistolCaliber_AR", "Tactical AR");
            _weaponIds.Add("WID_Assault_AutoHigh", "Assault Rifle");
            _weaponIds.Add("WID_Assault_Auto", "Assault Rifle");
            _weaponIds.Add("WID_Assault_SemiAuto", "Burst Rifle");
            _weaponIds.Add("WID_Assault_Suppressed", "Suppressed Assault Rifle");
            _weaponIds.Add("WID_Assault_Surgical_Thermal", "Thermal Rifle");
            _weaponIds.Add("WID_Assault_Infantry", "Infantry Rifle");
            _weaponIds.Add("WID_Sniper_Auto_Suppressed_Scope", "Auto Sniper");
            _weaponIds.Add("WID_Sniper_Weather", "Storm Sniper");
            _weaponIds.Add("WID_Sniper_BoltAction_Scope", "Bolt Sniper");
            _weaponIds.Add("WID_Sniper_Heavy", "Heavy Sniper");
            _weaponIds.Add("WID_Sniper_Suppressed_Scope", "Suppressed Sniper");
            _weaponIds.Add("WID_Assault_LMG", "Minigun");
            _weaponIds.Add("WID_Launcher_Grenade", "Grenade Launcher");
            _weaponIds.Add("WID_Launcher_Pumpkin", "Pumpkin Launcher");
            _weaponIds.Add("WID_GrenadeLauncher_Prox", "Proximity Launcher");
            _weaponIds.Add("WID_Launcher_Rocket", "Rocket Launcher");
            _weaponIds.Add("WID_Launcher_Military", "Quad Launcher");
            _weaponIds.Add("WID_Special_FiendHunter", "Field Hunter Bow");
            _weaponIds.Add("WID_Sniper_Valentine", "Cupid's Bow");
            _weaponIds.Add("WID_Pistol_Flashlight", "Flashlight Pistol");
            _weaponIds.Add("WID_Pistol_SemiAuto", "Pistol");
            _weaponIds.Add("WID_Pistol_Revolver_SingleAction", "Revolver");
            _weaponIds.Add("WID_DualPistol_SemiAuto", "Dual Pistol");
            _weaponIds.Add("WID_Shotgun_AutoDrum", "Drum Shotgun");
            _weaponIds.Add("WID_Shotgun_SemiAuto", "Tactical Shotgun");
            _weaponIds.Add("WID_Assault_AutoDrum", "Drum Gun");
            _weaponIds.Add("WID_Pistol_BurstFireSMG", "Burst SMG");
            _weaponIds.Add("WID_Pistol_AutoHeavySuppressed", "Suppressed SMG");
            _weaponIds.Add("WID_Assault_Heavy", "Heavy AR");
            _weaponIds.Add("WID_Assault_Surgical", "Scoped AR");
            _weaponIds.Add("WID_Sniper_NoScope", "Hunting Rifle");
            _weaponIds.Add("WID_Sniper_Standard_Scope", "Semi-Auto Sniper");
            _weaponIds.Add("WID_Assault_LMGSAW", "LMG");
        }

        public ItemName(string itemId)
        {
            ItemId = itemId;

            UpdateItem();
        }

        private void UpdateWeaponRarity(string rarity)
        {
            switch (rarity)
            {
                case "UC":
                    Rarity = ItemRarity.Uncommon;
                    break;
                case "C":
                    Rarity = ItemRarity.Common;
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

        private void UpdateItem()
        {
            if (ItemId == null)
            {
                return;
            }

            string[] parts = ItemId.Split('_');

            int athenaIndex = Array.IndexOf<string>(parts, "Athena");

            string weaponId = ItemId;
            if (athenaIndex > -1)
            {
                //For some weird reason the Compact SMG is odd WID_Athena_SMG_SR
                if (athenaIndex == 1)
                {
                    if(parts[athenaIndex + 1] == "SMG")
                    {
                        parts[athenaIndex] = "SMG";
                        parts[athenaIndex + 1] = "Athena";
                        athenaIndex++;
                    }
                }

                weaponId = String.Join("_", parts.Take(athenaIndex));

                if(athenaIndex + 1 < parts.Length)
                {
                    string rarity = parts[athenaIndex + 1];
                    UpdateWeaponRarity(rarity);
                }
            }

            if(_weaponIds.TryGetValue(weaponId, out string name))
            {
                WeaponName = name;
            }
        }

        public override string ToString()
        {
            if(!String.IsNullOrEmpty(WeaponName))
            {
                return $"{Rarity} {WeaponName}";
            }

            return ItemId;
        }
    }
}
