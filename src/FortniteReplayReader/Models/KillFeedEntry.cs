using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.Enums;

namespace FortniteReplayReader.Models
{
    public class KillFeedEntry
    {
        public Player FinisherOrDowner { get; internal set; }
        public Player Player { get; internal set; }
        public PlayerState CurrentPlayerState { get; internal set; }
        public ItemRarity ItemRarity { get; internal set; }
        public ItemType ItemType { get; internal set; }
        public int ItemId { get; internal set; }
        public float DeltaGameTimeSeconds { get; internal set; }
        public float Distance { get; internal set; }
        public string[] DeathTags
        {
            get
            {
                return _deathTags;
            }
            internal set
            {
                _deathTags = value;
                UpdateDeathTagInfo();
            }
        }


        private string[] _deathTags;

        public bool KilledSelf => FinisherOrDowner == Player;
        public bool HasError { get; internal set; }

        private void UpdateDeathTagInfo()
        {
            if(_deathTags == null)
            {
                return;
            }

            foreach (string deathTag in DeathTags)
            {
                switch (deathTag)
                {
                    case "Weapon.Melee.Impact.Pickaxe":
                        ItemType = ItemType.PickAxe;
                        break;
                    case "weapon.ranged.assault.burst":
                        ItemType = ItemType.Burst;
                        break;
                    case "weapon.ranged.assault.standard":
                        ItemType = ItemType.Assault;
                        break;
                    case "weapon.ranged.heavy.rocket_launcher":
                        ItemType = ItemType.Launcher;
                        break;
                    case "Weapon.Ranged.Pistol.Standard":
                        ItemType = ItemType.Pistol;
                        break;
                    case "Weapon.Ranged.Shotgun.Pump":
                        ItemType = ItemType.PumpShotgun;
                        break;
                    case "Weapon.Ranged.Shotgun.Tactical":
                        ItemType = ItemType.TacticalShotgun;
                        break;
                    case "Weapon.Ranged.SMG":
                        ItemType = ItemType.SMG;
                        break;
                    case "weapon.ranged.sniper.bolt":
                        ItemType = ItemType.BoltSniper;
                        break;
                    case "Gameplay.Damage.Environment":
                    case "Gameplay.Damage.Environment.Falling":
                        ItemType = ItemType.Environment;
                        break;
                    case "Rarity.Common":
                        ItemRarity = ItemRarity.Common;
                        break;
                    case "Rarity.Uncommon":
                        ItemRarity = ItemRarity.Uncommon;
                        break;
                    case "Rarity.Rare":
                        ItemRarity = ItemRarity.Rare;
                        break;
                    case "Rarity.SuperRare":
                        ItemRarity = ItemRarity.Legendary;
                        break;
                    case "Rarity.VeryRare":
                        ItemRarity = ItemRarity.Epic;
                        break;
                }
            }
        }
    }

    public enum PlayerState { Unknown, Alive, Knocked, BleedOut, Killed, Revived, FinallyEliminated }
}
