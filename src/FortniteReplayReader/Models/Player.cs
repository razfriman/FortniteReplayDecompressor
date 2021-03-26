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
    }

    public class PlayerLocation
    {
        public virtual FVector Location { get; set; }
        public virtual float Yaw { get; set; }

        public float WorldTime { get; set; }
        public float DeltaGameTimeSeconds { get; set; }
        public bool InVehicle { get; set; }

        internal uint VehicleChannel { get; set; }
    }

    public class PlayerLocationRepMovement : PlayerLocation
    {
        public override FVector Location => RepLocation?.Location;
        public override float Yaw => RepLocation.Rotation.Yaw;

        public FRepMovement RepLocation { get; set; }
        public float? LastUpdateTime { get; set; }
    }

    public class WeaponShot
    {
        public PlayerPawn ShotByPlayerPawn { get; set; }
        public PlayerPawn HitPlayerPawn { get; set; }
        public Weapon Weapon { get; set;}
        public float DeltaGameTimeSeconds { get; set; }
        public FVector Location { get; set; } 
        public FVector Normal { get; set; }
        public float Damage { get; set; }
        public bool WeaponActivate { get; set; }
        public bool IsFatal { get; set; }
        public bool IsCritical { get; set; }
        public bool IsShield { get; set; }
        public bool IsShieldDestroyed { get; set; }
        public bool IsBallistic { get; set; } 
        public bool FatalHitNonPlayer { get; set; }
        public bool CriticalHitNonPlayer { get; set; }
        public bool HitPlayer => HitPlayerPawn != null;
    }
}
