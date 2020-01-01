using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    [NetFieldExportGroup("/Game/Athena/Athena_GameState.Athena_GameState_C")]
    public class GameStateC : INetFieldExportGroup
    {
        #region Ignored

        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public object RemoteRole { get; set; } //Type:  Bits: 2

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public object Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("GameModeClass", RepLayoutCmdType.Ignore)]
        public uint? GameModeClass { get; set; } //Type: TSubclassOf<AGameModeBase> Bits: 16

        [NetFieldExport("SpectatorClass", RepLayoutCmdType.Ignore)]
        public uint? SpectatorClass { get; set; } //Type: TSubclassOf<ASpectatorPawn> Bits: 16

        [NetFieldExport("FortTimeOfDayManager", RepLayoutCmdType.Ignore)]
        public uint? FortTimeOfDayManager { get; set; } //Type: AFortTimeOfDayManager* Bits: 16

        [NetFieldExport("PoiManager", RepLayoutCmdType.Ignore)]
        public uint? PoiManager { get; set; } //Type: AFortPoiManager* Bits: 8

        [NetFieldExport("FeedbackManager", RepLayoutCmdType.Ignore)]
        public uint? FeedbackManager { get; set; } //Type: AFortFeedbackManager* Bits: 8

        [NetFieldExport("MissionManager", RepLayoutCmdType.Ignore)]
        public uint? MissionManager { get; set; } //Type: AFortMissionManager* Bits: 8

        [NetFieldExport("AnnouncementManager", RepLayoutCmdType.Ignore)]
        public uint? AnnouncementManager { get; set; } //Type: AFortClientAnnouncementManager* Bits: 8

        [NetFieldExport("WorldManager", RepLayoutCmdType.Ignore)]
        public uint? WorldManager { get; set; } //Type: AFortWorldManager* Bits: 16

        [NetFieldExport("MusicManagerSubclass", RepLayoutCmdType.Ignore)]
        public uint? MusicManagerSubclass { get; set; } //Type: TSubclassOf<AFortMusicManager> Bits: 16

        [NetFieldExport("MusicManagerBank", RepLayoutCmdType.Ignore)]
        public uint? MusicManagerBank { get; set; } //Type: UFortMusicManagerBank* Bits: 16

        [NetFieldExport("PawnForReplayRelevancy", RepLayoutCmdType.Ignore)]
        public uint? PawnForReplayRelevancy { get; set; } //Type: AFortPawn* Bits: 16

        [NetFieldExport("RecorderPlayerState", RepLayoutCmdType.Ignore)]
        public uint? RecorderPlayerState { get; set; } //Type: AFortPlayerState* Bits: 8

        [NetFieldExport("GlobalEnvironmentAbilityActor", RepLayoutCmdType.Ignore)]
        public uint? GlobalEnvironmentAbilityActor { get; set; } //Type: AFortGlobalEnvironmentAbilityActor* Bits: 8

        [NetFieldExport("UIMapManager", RepLayoutCmdType.Ignore)]
        public uint? UIMapManager { get; set; } //Type: AFortInGameMapManager* Bits: 16

        [NetFieldExport("CreativePlotManager", RepLayoutCmdType.Ignore)]
        public uint? CreativePlotManager { get; set; } //Type: AFortCreativePlotManager* Bits: 8

        [NetFieldExport("PlayspaceManager", RepLayoutCmdType.Ignore)]
        public uint? PlayspaceManager { get; set; } //Type: AFortPlayspaceManager* Bits: 8

        [NetFieldExport("ItemCollector", RepLayoutCmdType.Ignore)]
        public uint? ItemCollector { get; set; } //Type: ABuildingItemCollectorActor* Bits: 16

        [NetFieldExport("SpecialActorData", RepLayoutCmdType.Ignore)]
        public uint? SpecialActorData { get; set; } //Type: AFortSpecialActorReplicationInfo* Bits: 8

        [NetFieldExport("SupplyDropWaveStartedSoundCue", RepLayoutCmdType.Ignore)]
        public uint? SupplyDropWaveStartedSoundCue { get; set; } //Type: USoundCue* Bits: 16

        [NetFieldExport("TeamXPlayersLeft", RepLayoutCmdType.Ignore)]
        public DebuggingObject TeamXPlayersLeft { get; set; } //Type:  Bits: 160

        [NetFieldExport("SafeZoneIndicator", RepLayoutCmdType.Ignore)]
        public uint? SafeZoneIndicator { get; set; } //Type: AFortSafeZoneIndicator* Bits: 16

        [NetFieldExport("MapInfo", RepLayoutCmdType.Ignore)]
        public uint? MapInfo { get; set; } //Type: AFortAthenaMapInfo* Bits: 16

        [NetFieldExport("GoldenPoiLocationTags", RepLayoutCmdType.Ignore)]
        public DebuggingObject GoldenPoiLocationTags { get; set; } //Type:  Bits: 24

        [NetFieldExport("DefaultBattleBus", RepLayoutCmdType.Ignore)]
        public uint? DefaultBattleBus { get; set; } //Type: UAthenaBattleBusItemDefinition* Bits: 16

        #endregion

        [NetFieldExport("bReplicatedHasBegunPlay", RepLayoutCmdType.Ignore)]
        public bool? bReplicatedHasBegunPlay { get; set; } //Type: bool Bits: 1

        [NetFieldExport("ReplicatedWorldTimeSeconds", RepLayoutCmdType.PropertyFloat)]
        public float? ReplicatedWorldTimeSeconds { get; set; } //Type: float Bits: 32

        [NetFieldExport("MatchState", RepLayoutCmdType.Ignore)]
        public FName MatchState { get; set; } //Type: FName Bits: 153

        [NetFieldExport("ElapsedTime", RepLayoutCmdType.PropertyInt)]
        public int? ElapsedTime { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("WorldLevel", RepLayoutCmdType.Ignore)]
        public int? WorldLevel { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("CraftingBonus", RepLayoutCmdType.Ignore)]
        public int? CraftingBonus { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("TeamCount", RepLayoutCmdType.PropertyInt)]
        public int? TeamCount { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("TeamSize", RepLayoutCmdType.PropertyInt)]
        public int? TeamSize { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("GameFlagData", RepLayoutCmdType.Ignore)]
        public int? GameFlagData { get; set; } //Type:  Bits: 32

        [NetFieldExport("AdditionalPlaylistLevelsStreamed", RepLayoutCmdType.Ignore)]
        public object[] AdditionalPlaylistLevelsStreamed { get; set; } //Type: TArray Bits: 393

        [NetFieldExport("WorldDaysElapsed", RepLayoutCmdType.Ignore)]
        public int? WorldDaysElapsed { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("GameplayState", RepLayoutCmdType.Ignore)]
        public int? GameplayState { get; set; } //Type: TEnumAsByte<EFortGameplayState::Type> Bits: 3

        [NetFieldExport("GameSessionId", RepLayoutCmdType.PropertyString)]
        public string GameSessionId { get; set; } //Type: FString Bits: 296

        [NetFieldExport("SpawnPointsCap", RepLayoutCmdType.Ignore)]
        public int? SpawnPointsCap { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("SpawnPointsAllocated", RepLayoutCmdType.Ignore)]
        public int? SpawnPointsAllocated { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("PlayerSharedMaxTrapAttributes", RepLayoutCmdType.Ignore)]
        public object[] PlayerSharedMaxTrapAttributes { get; set; } //Type: TArray Bits: 464

        [NetFieldExport("TotalPlayerStructures", RepLayoutCmdType.PropertyInt)]
        public int? TotalPlayerStructures { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("ServerGameplayTagIndexHash", RepLayoutCmdType.Ignore)]
        public uint? ServerGameplayTagIndexHash { get; set; } //Type:  Bits: 32

        [NetFieldExport("GameDifficulty", RepLayoutCmdType.Ignore)]
        public float? GameDifficulty { get; set; } //Type: float Bits: 32

        [NetFieldExport("bAllowLayoutRequirementsFeature", RepLayoutCmdType.Ignore)]
        public bool? bAllowLayoutRequirementsFeature { get; set; } //Type:  Bits: 1

        [NetFieldExport("ServerStability", RepLayoutCmdType.Ignore)]
        public int? ServerStability { get; set; } //Type:  Bits: 3

        [NetFieldExport("RoundTimeAccumulated", RepLayoutCmdType.Ignore)]
        public int? RoundTimeAccumulated { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("RoundTimeCriticalThreshold", RepLayoutCmdType.Ignore)]
        public int? RoundTimeCriticalThreshold { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("ServerChangelistNumber", RepLayoutCmdType.Ignore)]
        public int? ServerChangelistNumber { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("CreativeRealEstatePlotManager", RepLayoutCmdType.Ignore)]
        public uint? CreativeRealEstatePlotManager { get; set; } //Type:  Bits: 8

        [NetFieldExport("WarmupCountdownStartTime", RepLayoutCmdType.PropertyFloat)]
        public float? WarmupCountdownStartTime { get; set; } //Type: float Bits: 32

        [NetFieldExport("WarmupCountdownEndTime", RepLayoutCmdType.PropertyFloat)]
        public float? WarmupCountdownEndTime { get; set; } //Type: float Bits: 32

        [NetFieldExport("bSafeZonePaused", RepLayoutCmdType.Ignore)]
        public bool? bSafeZonePaused { get; set; } //Type:  Bits: 1

        [NetFieldExport("AircraftStartTime", RepLayoutCmdType.PropertyFloat)]
        public float? AircraftStartTime { get; set; } //Type: float Bits: 32

        [NetFieldExport("bSkyTubesShuttingDown", RepLayoutCmdType.Ignore)]
        public bool? bSkyTubesShuttingDown { get; set; } //Type:  Bits: 1

        [NetFieldExport("SafeZonesStartTime", RepLayoutCmdType.PropertyFloat)]
        public float? SafeZonesStartTime { get; set; } //Type: float Bits: 32

        [NetFieldExport("bSkyTubesDisabled", RepLayoutCmdType.Ignore)]
        public bool? bSkyTubesDisabled { get; set; } //Type:  Bits: 1

        [NetFieldExport("PlayersLeft", RepLayoutCmdType.PropertyInt)]
        public int? PlayersLeft { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("ReplOverrideData", RepLayoutCmdType.Ignore)]
        public uint? ReplOverrideData { get; set; } //Type:  Bits: 16

        [NetFieldExport("EndGameStartTime", RepLayoutCmdType.PropertyFloat)]
        public float? EndGameStartTime { get; set; } //Type:  Bits: 32

        [NetFieldExport("TeamsLeft", RepLayoutCmdType.PropertyInt)]
        public int? TeamsLeft { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("EndGameKickPlayerTime", RepLayoutCmdType.PropertyFloat)]
        public float? EndGameKickPlayerTime { get; set; } //Type:  Bits: 32

        [NetFieldExport("ServerToClientPreloadList", RepLayoutCmdType.Ignore)]
        public object[] ServerToClientPreloadList { get; set; } //Type: TArray Bits: 408

        [NetFieldExport("ClientVehicleClassesToLoad", RepLayoutCmdType.Ignore)]
        public int[] ClientVehicleClassesToLoad { get; set; } //Type:  Bits: 72

        [NetFieldExport("bAllowUserPickedCosmeticBattleBus", RepLayoutCmdType.Ignore)]
        public bool? bAllowUserPickedCosmeticBattleBus { get; set; } //Type: bool Bits: 1

        [NetFieldExport("TeamFlightPaths", RepLayoutCmdType.DynamicArray)]
        public GameStateC[] TeamFlightPaths { get; set; } //Type: TArray Bits: 403

        [NetFieldExport("StormCapState", RepLayoutCmdType.Enum)]
        public int? StormCapState { get; set; } //Type:  Bits: 3

        [NetFieldExport("WinningPlayerList", RepLayoutCmdType.DynamicArray)]
        public int[] WinningPlayerList { get; set; } //Type:  Bits: 160

        [NetFieldExport("FlightStartLocation", RepLayoutCmdType.PropertyVector100)]
        public FVector FlightStartLocation { get; set; } //Type: FVector_NetQuantize100 Bits: 80

        [NetFieldExport("FlightStartRotation", RepLayoutCmdType.PropertyRotator)]
        public FRotator FlightStartRotation { get; set; } //Type: FRotator Bits: 19

        [NetFieldExport("FlightSpeed", RepLayoutCmdType.PropertyFloat)]
        public float? FlightSpeed { get; set; } //Type: float Bits: 32

        [NetFieldExport("TimeTillFlightEnd", RepLayoutCmdType.PropertyFloat)]
        public float? TimeTillFlightEnd { get; set; } //Type: float Bits: 32

        [NetFieldExport("TimeTillDropStart", RepLayoutCmdType.PropertyFloat)]
        public float? TimeTillDropStart { get; set; } //Type: float Bits: 32

        [NetFieldExport("TimeTillDropEnd", RepLayoutCmdType.PropertyFloat)]
        public float? TimeTillDropEnd { get; set; } //Type: float Bits: 32

        [NetFieldExport("UtcTimeStartedMatch", RepLayoutCmdType.Property)]
        public FDateTime UtcTimeStartedMatch { get; set; } //Type: FDateTime Bits: 64

        [NetFieldExport("SafeZonePhase", RepLayoutCmdType.PropertyByte)]
        public byte? SafeZonePhase { get; set; } //Type: uint8 Bits: 8

        [NetFieldExport("GamePhase", RepLayoutCmdType.Enum)]
        public int? GamePhase { get; set; } //Type: EAthenaGamePhase Bits: 3

        [NetFieldExport("Aircrafts", RepLayoutCmdType.Ignore)]
        public DebuggingObject Aircrafts { get; set; } //Type: TArray Bits: 64

        [NetFieldExport("bAircraftIsLocked", RepLayoutCmdType.PropertyBool)]
        public bool? bAircraftIsLocked { get; set; } //Type: uint8 Bits: 1

        [NetFieldExport("LobbyAction", RepLayoutCmdType.Ignore)]
        public int? LobbyAction { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("WinningPlayerState", RepLayoutCmdType.Ignore)]
        public uint? WinningPlayerState { get; set; } //Type:  Bits: 16

        [NetFieldExport("WinningTeam", RepLayoutCmdType.PropertyUInt32)]
        public uint? WinningTeam { get; set; } //Type:  Bits: 32

        [NetFieldExport("CurrentHighScore", RepLayoutCmdType.Ignore)]
        public uint? CurrentHighScore { get; set; } //Type:  Bits: 32

        [NetFieldExport("CurrentHighScoreTeam", RepLayoutCmdType.Ignore)]
        public uint? CurrentHighScoreTeam { get; set; } //Type:  Bits: 32

        [NetFieldExport("bStormReachedFinalPosition", RepLayoutCmdType.Ignore)]
        public bool? bStormReachedFinalPosition { get; set; } //Type:  Bits: 1

        [NetFieldExport("SpectateAPartyMemberAvailable", RepLayoutCmdType.Ignore)]
        public bool? SpectateAPartyMemberAvailable { get; set; } //Type:  Bits: 1

        [NetFieldExport("HopRockDuration", RepLayoutCmdType.Ignore)]
        public float? HopRockDuration { get; set; } //Type: float Bits: 32

        [NetFieldExport("bIsLargeTeamGame", RepLayoutCmdType.PropertyBool)]
        public bool? bIsLargeTeamGame { get; set; } //Type:  Bits: 1

        [NetFieldExport("ActiveTeamNums", RepLayoutCmdType.DynamicArray)]
        public byte[] ActiveTeamNums { get; set; } //Type:  Bits: 2416

        [NetFieldExport("AirCraftBehavior", RepLayoutCmdType.Ignore)]
        public int? AirCraftBehavior { get; set; } //Type:  Bits: 2

        [NetFieldExport("DefaultGliderRedeployCanRedeploy", RepLayoutCmdType.Ignore)]
        public float? DefaultGliderRedeployCanRedeploy { get; set; } //Type: float Bits: 32

        [NetFieldExport("DefaultRedeployGliderLateralVelocityMult", RepLayoutCmdType.Ignore)]
        public float? DefaultRedeployGliderLateralVelocityMult { get; set; } //Type: float Bits: 32

        [NetFieldExport("DefaultRedeployGliderHeightLimit", RepLayoutCmdType.Ignore)]
        public float? DefaultRedeployGliderHeightLimit { get; set; } //Type: float Bits: 32

        [NetFieldExport("EventTournamentRound", RepLayoutCmdType.Enum)]
        public int? EventTournamentRound { get; set; } //Type:  Bits: 3

        [NetFieldExport("PlayerBotsLeft", RepLayoutCmdType.PropertyInt)]
        public int? PlayerBotsLeft { get; set; } //Type:  Bits: 32

        [NetFieldExport("DefaultParachuteDeployTraceForGroundDistance", RepLayoutCmdType.Ignore)]
        public float? DefaultParachuteDeployTraceForGroundDistance { get; set; } //Type:  Bits: 32

        [NetFieldExport("DefaultRebootMachineHotfix", RepLayoutCmdType.Ignore)]
        public float? DefaultRebootMachineHotfix { get; set; } //Type:  Bits: 32

        [NetFieldExport("SignalInStormRegenSpeed", RepLayoutCmdType.Ignore)]
        public float? SignalInStormRegenSpeed { get; set; } //Type:  Bits: 32

        [NetFieldExport("MutatorGenericInt", RepLayoutCmdType.Ignore)]
        public uint? MutatorGenericInt { get; set; } //Type:  Bits: 32

        [NetFieldExport("SignalInStormLostSpeed", RepLayoutCmdType.Ignore)]
        public float? SignalInStormLostSpeed { get; set; } //Type:  Bits: 32

        [NetFieldExport("StormCNDamageVulnerabilityLevel0", RepLayoutCmdType.Ignore)]
        public float? StormCNDamageVulnerabilityLevel0 { get; set; } //Type:  Bits: 32

        [NetFieldExport("StormCNDamageVulnerabilityLevel1", RepLayoutCmdType.Ignore)]
        public float? StormCNDamageVulnerabilityLevel1 { get; set; } //Type:  Bits: 32

        [NetFieldExport("StormCNDamageVulnerabilityLevel2", RepLayoutCmdType.Ignore)]
        public float? StormCNDamageVulnerabilityLevel2 { get; set; } //Type:  Bits: 32

        [NetFieldExport("StormCNDamageVulnerabilityLevel3", RepLayoutCmdType.Ignore)]
        public float? StormCNDamageVulnerabilityLevel3 { get; set; } //Type:  Bits: 32

        [NetFieldExport("bEnabled", RepLayoutCmdType.Ignore)]
        public bool? bEnabled { get; set; } //Type:  Bits: 1

        [NetFieldExport("bConnectedToRoot", RepLayoutCmdType.Ignore)]
        public bool? bConnectedToRoot { get; set; } //Type:  Bits: 1

        [NetFieldExport("GameServerNodeType", RepLayoutCmdType.Ignore)]
        public int? GameServerNodeType { get; set; } //Type:  Bits: 3

        [NetFieldExport("VolumeManager", RepLayoutCmdType.Ignore)]
        public uint? VolumeManager { get; set; } //Type:  Bits: 16
    }
}
