using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.Enums;

namespace FortniteReplayReader.Models
{
    public class KillFeedEntry
    {
        public Player ReceivingPlayer { get; internal set; }
        public Player AttackingPlayer { get; internal set; }
        public PlayerState CurrentPlayerState { get; internal set; }
        public ItemRarity ItemRarity { get; internal set; }
        public ItemType ItemType { get; internal set; }

        public bool KilledSelf => ReceivingPlayer == AttackingPlayer;
    }

    public enum PlayerState { Alive, Knocked, BleedOut, Killed, Revived }
}
