using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class Player : PlayerPawn
    {
        public string EpicId { get; set; }
        public string PartyOwnerEpicId { get; set; }
        public string Platform { get; set; }
        public int Teamindex { get; set; }
        public string BotId { get; set; }
        public bool IsBot { get; set; }
        public int Level { get; set; }
        public bool IsGameSessionOwner { get; set; }
        public bool FinishedLoading { get; set; }
        public bool StartedPlaying { get; set; }
        public bool IsPlayersReplay { get; set; }
        public bool StreamerMode { get; set; }
        public bool ThankedBusDriver { get; set; }
        public int Placement { get; set; }
        public string ColorId { get; set; }
        public string BannerId { get; set; }

        //Extended information
        public List<PlayerLocation> Locations { get; private set; } = new List<PlayerLocation>();
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
