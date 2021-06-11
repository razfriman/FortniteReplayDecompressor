using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports.Sets;
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
        public uint TotalKills { get; internal set; }
        public uint TeamKills { get; internal set; }
        public bool IsGameSessionOwner { get; internal set; }
        public bool FinishedLoading { get; internal set; }
        public bool StartedPlaying { get; internal set; }
        public bool IsPlayersReplay { get; internal set; }
        public bool StreamerMode { get; internal set; }
        public bool ThankedBusDriver { get; internal set; }
        public int Placement { get; internal set; }
        public Team Team { get; internal set; }
        public float LastDeathOrKnockTime { get; internal set; }
        public List<WeaponShot> Shots { get; internal set; } = new List<WeaponShot>();
        public List<WeaponShot> DamageTaken { get; internal set; } = new List<WeaponShot>();
        public FGameplayTag[] DeathTags { get; internal set; }
        public bool Disconnected { get; internal set; }
        public PlayerLocationRepMovement LastKnownLocation { get; internal set; }
        public int WorldPlayerId { get; internal set; }
        public Cosmetics Cosmetics { get; internal set; } = new Cosmetics();
        public bool AnonMode { get; internal set; }

        //Extended information
        public List<PlayerLocationRepMovement> Locations { get; private set; } = new List<PlayerLocationRepMovement>();
        public List<PlayerLocation> PrivateTeamLocations { get; private set; } = new List<PlayerLocation>(); //Locations pulled from FortTeamPrivateInfo
        public NetDeltaArray<InventoryItem> CurrentInventory { get; private set; } = new NetDeltaArray<InventoryItem>();
        public List<InventoryItem> InventoryOnDeath { get; private set; } = new List<InventoryItem>();
        public NetDeltaArray<PrivateTeamInfo> PrivateTeamInfo { get; internal set; }

        public Weapon CurrentWeapon { get; internal set; }
        public List<WeaponSwitch> WeaponSwitches { get; private set; } = new List<WeaponSwitch>();

        public List<KillFeedEntry> StatusChanges { get; private set; } = new List<KillFeedEntry>();
        public List<HealthUpdate> HealthChanges { get; private set; } = new List<HealthUpdate>();

        /// <summary>
        /// Last known location when player landed from bus
        /// </summary>
        public PlayerLocation LandingLocation { get; set; }

        //Internal 
        internal KillFeedEntry LastKnockedEntry { get; set; }
        internal List<InventoryItem> InventoryBeforeDeletes { get; set; } = new List<InventoryItem>();
        internal int InventoryBaseReplicationKey { get; set; }
        internal float LastTransformUpdate { get; set; }
        internal ActorGUID PrivateTeamActorId { get; set; } //Used to set the team data later
        internal uint ReplayPawnId { get; set; }
        internal PlayerMovementInformation MovementInformation { get; set; } = new PlayerMovementInformation();
        internal float InitialMovementTimestamp { get; set; }
    }

    public class PlayerLocation
    {
        public virtual FVector? Location { get; internal set; }
        public virtual float Yaw { get; internal set; }

        public float WorldTime { get; internal set; }
        public float DeltaGameTimeSeconds { get; internal set; }

    }

    public class PlayerLocationRepMovement : PlayerLocation
    {
        public override FVector? Location => RepLocation?.Location;
        public override float Yaw => RepLocation.Rotation.Yaw;

        public bool InVehicle { get; internal set; }
        public PlayerState CurrentPlayerState { get; internal set; } = PlayerState.Alive;
        public PlayerMovementInformation MovementInformation { get; internal set; }
        public FRepMovement RepLocation { get; internal set; }
        public float? LastUpdateTime { get; internal set; }

        internal uint VehicleChannel { get; set; }
    }
}
