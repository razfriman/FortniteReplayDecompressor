using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class Player : PlayerPawn
    {
        public string EpicId { get; internal set; }
        public string PartyOwnerEpicId { get; internal set; }
        public string Platform { get; internal set; }
        public int Teamindex { get; internal set; }
        public string BotId { get; internal set; }
        public bool IsBot { get; internal set; }
        public int Level { get; internal set; }
        public bool IsGameSessionOwner { get; internal set; }
        public bool FinishedLoading { get; internal set; }
        public bool StartedPlaying { get; internal set; }
        public bool IsPlayersReplay { get; internal set; }
        public bool StreamerMode { get; internal set; }
        public bool ThankedBusDriver { get; internal set; }
        public int Placement { get; internal set; }
        public Team Team { get; internal set; }
        public float LastDeathTime { get; internal set; }

        //Extended information
        public List<PlayerLocation> Locations { get; private set; } = new List<PlayerLocation>();
        public List<InventoryItemChange> InventoryChanges { get; private set; } = new List<InventoryItemChange>();

        public HashSet<InventoryItem> CurrentInventory
        {
            get
            {
                HashSet<InventoryItem> items = new HashSet<InventoryItem>();

                foreach (InventoryItemChange itemChange in InventoryChanges)
                {
                    //Keeps the last inventory before death
                    if(itemChange.WorldTime >= LastDeathTime)
                    {
                        continue;
                    }

                    //Only care about weapon ids
                    if(!itemChange.Item.ItemIdName.StartsWith("WID"))
                    {
                        continue;
                    }

                    if (itemChange.State == ItemChangeState.PickedUp)
                    {
                        items.Add(itemChange.Item);
                    }
                    else
                    {
                        //Best that I can figure out
                        InventoryItem item = items.FirstOrDefault(x => x.ItemDefinition == itemChange.Item.ItemDefinition);

                        items.Remove(item);
                    }
                }

                return items;
            }
        }

        /// <summary>
        /// Last known location when player landed from bus
        /// </summary>
        public PlayerLocation LandingLocation { get; set; }

        //Internal 
        internal int WorldPlayerId { get; set; }
    }

    public class PlayerLocation
    {
        public FVector Location => RepLocation?.Location;
        public FRepMovement RepLocation { get; set; }

        public float? WorldTime { get; set; }
        public float? LastUpdateTime { get; set; }
    }
}
