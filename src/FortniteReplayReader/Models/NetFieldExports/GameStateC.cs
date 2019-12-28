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

        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore, 4, "RemoteRole", "", 2)]
        public object RemoteRole { get; set; } //Type:  Bits: 2

        [NetFieldExport("Role", RepLayoutCmdType.Ignore, 13, "Role", "", 2)]
        public object Role { get; set; } //Type:  Bits: 2

        [NetFieldExport("GameModeClass", RepLayoutCmdType.Ignore, 15, "GameModeClass", "TSubclassOf<AGameModeBase>", 16)]
        public uint? GameModeClass { get; set; } //Type: TSubclassOf<AGameModeBase> Bits: 16

        [NetFieldExport("SpectatorClass", RepLayoutCmdType.Ignore, 16, "SpectatorClass", "TSubclassOf<ASpectatorPawn>", 16)]
        public uint? SpectatorClass { get; set; } //Type: TSubclassOf<ASpectatorPawn> Bits: 16

        [NetFieldExport("FortTimeOfDayManager", RepLayoutCmdType.Ignore, 21, "FortTimeOfDayManager", "AFortTimeOfDayManager*", 16)]
        public uint? FortTimeOfDayManager { get; set; } //Type: AFortTimeOfDayManager* Bits: 16

        [NetFieldExport("PoiManager", RepLayoutCmdType.Ignore, 30, "PoiManager", "AFortPoiManager*", 8)]
        public uint? PoiManager { get; set; } //Type: AFortPoiManager* Bits: 8

        [NetFieldExport("FeedbackManager", RepLayoutCmdType.Ignore, 36, "FeedbackManager", "AFortFeedbackManager*", 8)]
        public uint? FeedbackManager { get; set; } //Type: AFortFeedbackManager* Bits: 8

        [NetFieldExport("MissionManager", RepLayoutCmdType.Ignore, 37, "MissionManager", "AFortMissionManager*", 8)]
        public uint? MissionManager { get; set; } //Type: AFortMissionManager* Bits: 8

        [NetFieldExport("AnnouncementManager", RepLayoutCmdType.Ignore, 38, "AnnouncementManager", "AFortClientAnnouncementManager*", 8)]
        public uint? AnnouncementManager { get; set; } //Type: AFortClientAnnouncementManager* Bits: 8

        [NetFieldExport("WorldManager", RepLayoutCmdType.Ignore, 39, "WorldManager", "AFortWorldManager*", 16)]
        public uint? WorldManager { get; set; } //Type: AFortWorldManager* Bits: 16

        [NetFieldExport("MusicManagerSubclass", RepLayoutCmdType.Ignore, 41, "MusicManagerSubclass", "TSubclassOf<AFortMusicManager>", 16)]
        public uint? MusicManagerSubclass { get; set; } //Type: TSubclassOf<AFortMusicManager> Bits: 16

        [NetFieldExport("MusicManagerBank", RepLayoutCmdType.Ignore, 42, "MusicManagerBank", "UFortMusicManagerBank*", 16)]
        public uint? MusicManagerBank { get; set; } //Type: UFortMusicManagerBank* Bits: 16

        [NetFieldExport("PawnForReplayRelevancy", RepLayoutCmdType.Ignore, 44, "PawnForReplayRelevancy", "AFortPawn*", 16)]
        public uint? PawnForReplayRelevancy { get; set; } //Type: AFortPawn* Bits: 16

        [NetFieldExport("RecorderPlayerState", RepLayoutCmdType.Ignore, 45, "RecorderPlayerState", "AFortPlayerState*", 8)]
        public uint? RecorderPlayerState { get; set; } //Type: AFortPlayerState* Bits: 8

        [NetFieldExport("GlobalEnvironmentAbilityActor", RepLayoutCmdType.Ignore, 69, "GlobalEnvironmentAbilityActor", "AFortGlobalEnvironmentAbilityActor*", 8)]
        public uint? GlobalEnvironmentAbilityActor { get; set; } //Type: AFortGlobalEnvironmentAbilityActor* Bits: 8

        [NetFieldExport("UIMapManager", RepLayoutCmdType.Ignore, 106, "UIMapManager", "AFortInGameMapManager*", 16)]
        public uint? UIMapManager { get; set; } //Type: AFortInGameMapManager* Bits: 16

        [NetFieldExport("CreativePlotManager", RepLayoutCmdType.Ignore, 133, "CreativePlotManager", "AFortCreativePlotManager*", 8)]
        public uint? CreativePlotManager { get; set; } //Type: AFortCreativePlotManager* Bits: 8

        [NetFieldExport("PlayspaceManager", RepLayoutCmdType.Ignore, 134, "PlayspaceManager", "AFortPlayspaceManager*", 8)]
        public uint? PlayspaceManager { get; set; } //Type: AFortPlayspaceManager* Bits: 8

        [NetFieldExport("ItemCollector", RepLayoutCmdType.Ignore, 141, "ItemCollector", "ABuildingItemCollectorActor*", 16)]
        public uint? ItemCollector { get; set; } //Type: ABuildingItemCollectorActor* Bits: 16

        [NetFieldExport("SpecialActorData", RepLayoutCmdType.Ignore, 153, "SpecialActorData", "AFortSpecialActorReplicationInfo*", 8)]
        public uint? SpecialActorData { get; set; } //Type: AFortSpecialActorReplicationInfo* Bits: 8

        [NetFieldExport("SupplyDropWaveStartedSoundCue", RepLayoutCmdType.Ignore, 192, "SupplyDropWaveStartedSoundCue", "USoundCue*", 16)]
        public uint? SupplyDropWaveStartedSoundCue { get; set; } //Type: USoundCue* Bits: 16

        [NetFieldExport("TeamXPlayersLeft", RepLayoutCmdType.Ignore, 193, "TeamXPlayersLeft", "", 160)]
        public DebuggingObject TeamXPlayersLeft { get; set; } //Type:  Bits: 160

        [NetFieldExport("SafeZoneIndicator", RepLayoutCmdType.Ignore, 197, "SafeZoneIndicator", "AFortSafeZoneIndicator*", 16)]
        public uint? SafeZoneIndicator { get; set; } //Type: AFortSafeZoneIndicator* Bits: 16

        [NetFieldExport("MapInfo", RepLayoutCmdType.Ignore, 203, "MapInfo", "AFortAthenaMapInfo*", 16)]
        public uint? MapInfo { get; set; } //Type: AFortAthenaMapInfo* Bits: 16

        [NetFieldExport("GoldenPoiLocationTags", RepLayoutCmdType.Ignore, 286, "GoldenPoiLocationTags", "", 24)]
        public DebuggingObject GoldenPoiLocationTags { get; set; } //Type:  Bits: 24

        [NetFieldExport("DefaultBattleBus", RepLayoutCmdType.Ignore, 174, "DefaultBattleBus", "UAthenaBattleBusItemDefinition*", 16)]
        public uint? DefaultBattleBus { get; set; } //Type: UAthenaBattleBusItemDefinition* Bits: 16

        #endregion

        [NetFieldExport("bReplicatedHasBegunPlay", RepLayoutCmdType.Ignore, 17, "bReplicatedHasBegunPlay", "bool", 1)]
        public bool? bReplicatedHasBegunPlay { get; set; } //Type: bool Bits: 1

        [NetFieldExport("ReplicatedWorldTimeSeconds", RepLayoutCmdType.PropertyFloat, 18, "ReplicatedWorldTimeSeconds", "float", 32)]
        public float? ReplicatedWorldTimeSeconds { get; set; } //Type: float Bits: 32

        [NetFieldExport("MatchState", RepLayoutCmdType.Ignore, 19, "MatchState", "FName", 153)]
        public FName MatchState { get; set; } //Type: FName Bits: 153

        [NetFieldExport("ElapsedTime", RepLayoutCmdType.PropertyInt, 20, "ElapsedTime", "int32", 32)]
        public int? ElapsedTime { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("WorldLevel", RepLayoutCmdType.Ignore, 25, "WorldLevel", "int32", 32)]
        public int? WorldLevel { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("CraftingBonus", RepLayoutCmdType.Ignore, 26, "CraftingBonus", "int32", 32)]
        public int? CraftingBonus { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("TeamCount", RepLayoutCmdType.PropertyInt, 28, "TeamCount", "int32", 32)]
        public int? TeamCount { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("TeamSize", RepLayoutCmdType.PropertyInt, 29, "TeamSize", "int32", 32)]
        public int? TeamSize { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("GameFlagData", RepLayoutCmdType.Ignore, 29, "GameFlagData", "", 32)]
        public int? GameFlagData { get; set; } //Type:  Bits: 32

        [NetFieldExport("AdditionalPlaylistLevelsStreamed", RepLayoutCmdType.Ignore, 32, "AdditionalPlaylistLevelsStreamed", "TArray", 393)]
        public object[] AdditionalPlaylistLevelsStreamed { get; set; } //Type: TArray Bits: 393

        [NetFieldExport("WorldDaysElapsed", RepLayoutCmdType.Ignore, 34, "WorldDaysElapsed", "int32", 32)]
        public int? WorldDaysElapsed { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("GameplayState", RepLayoutCmdType.Ignore, 40, "GameplayState", "TEnumAsByte<EFortGameplayState::Type>", 3)]
        public int? GameplayState { get; set; } //Type: TEnumAsByte<EFortGameplayState::Type> Bits: 3

        [NetFieldExport("GameSessionId", RepLayoutCmdType.PropertyString, 43, "GameSessionId", "FString", 296)]
        public string GameSessionId { get; set; } //Type: FString Bits: 296

        [NetFieldExport("SpawnPointsCap", RepLayoutCmdType.Ignore, 57, "SpawnPointsCap", "int32", 32)]
        public int? SpawnPointsCap { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("SpawnPointsAllocated", RepLayoutCmdType.Ignore, 58, "SpawnPointsAllocated", "int32", 32)]
        public int? SpawnPointsAllocated { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("PlayerSharedMaxTrapAttributes", RepLayoutCmdType.Ignore, 67, "PlayerSharedMaxTrapAttributes", "TArray", 464)]
        public object[] PlayerSharedMaxTrapAttributes { get; set; } //Type: TArray Bits: 464

        [NetFieldExport("TotalPlayerStructures", RepLayoutCmdType.PropertyInt, 72, "TotalPlayerStructures", "int32", 32)]
        public int? TotalPlayerStructures { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("ServerGameplayTagIndexHash", RepLayoutCmdType.Ignore, 86, "ServerGameplayTagIndexHash", "int", 32)]
        public uint? ServerGameplayTagIndexHash { get; set; } //Type:  Bits: 32

        [NetFieldExport("GameDifficulty", RepLayoutCmdType.Ignore, 103, "GameDifficulty", "float", 32)]
        public float? GameDifficulty { get; set; } //Type: float Bits: 32

        [NetFieldExport("bAllowLayoutRequirementsFeature", RepLayoutCmdType.Ignore, 134, "bAllowLayoutRequirementsFeature", "", 1)]
        public bool? bAllowLayoutRequirementsFeature { get; set; } //Type:  Bits: 1

        [NetFieldExport("ServerStability", RepLayoutCmdType.Ignore, 136, "ServerStability", "", 3)]
        public int? ServerStability { get; set; } //Type:  Bits: 3

        [NetFieldExport("RoundTimeAccumulated", RepLayoutCmdType.Ignore, 136, "RoundTimeAccumulated", "int32", 32)]
        public int? RoundTimeAccumulated { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("RoundTimeCriticalThreshold", RepLayoutCmdType.Ignore, 137, "RoundTimeCriticalThreshold", "int32", 32)]
        public int? RoundTimeCriticalThreshold { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("ServerChangelistNumber", RepLayoutCmdType.Ignore, 152, "ServerChangelistNumber", "int32", 32)]
        public int? ServerChangelistNumber { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("CreativeRealEstatePlotManager", RepLayoutCmdType.Ignore, 154, "CreativeRealEstatePlotManager", "", 8)]
        public uint? CreativeRealEstatePlotManager { get; set; } //Type:  Bits: 8

        [NetFieldExport("WarmupCountdownStartTime", RepLayoutCmdType.PropertyFloat, 156, "WarmupCountdownStartTime", "float", 32)]
        public float? WarmupCountdownStartTime { get; set; } //Type: float Bits: 32

        [NetFieldExport("WarmupCountdownEndTime", RepLayoutCmdType.PropertyFloat, 157, "WarmupCountdownEndTime", "float", 32)]
        public float? WarmupCountdownEndTime { get; set; } //Type: float Bits: 32

        [NetFieldExport("bSafeZonePaused", RepLayoutCmdType.Ignore, 158, "bSafeZonePaused", "", 1)]
        public bool? bSafeZonePaused { get; set; } //Type:  Bits: 1

        [NetFieldExport("AircraftStartTime", RepLayoutCmdType.PropertyFloat, 158, "AircraftStartTime", "float", 32)]
        public float? AircraftStartTime { get; set; } //Type: float Bits: 32

        [NetFieldExport("bSkyTubesShuttingDown", RepLayoutCmdType.Ignore, 159, "bSkyTubesShuttingDown", "", 1)]
        public bool? bSkyTubesShuttingDown { get; set; } //Type:  Bits: 1

        [NetFieldExport("SafeZonesStartTime", RepLayoutCmdType.PropertyFloat, 159, "SafeZonesStartTime", "float", 32)]
        public float? SafeZonesStartTime { get; set; } //Type: float Bits: 32

        [NetFieldExport("bSkyTubesDisabled", RepLayoutCmdType.Ignore, 160, "bSkyTubesDisabled", "", 1)]
        public bool? bSkyTubesDisabled { get; set; } //Type:  Bits: 1

        [NetFieldExport("PlayersLeft", RepLayoutCmdType.PropertyInt, 163, "PlayersLeft", "int32", 32)]
        public int? PlayersLeft { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("ReplOverrideData", RepLayoutCmdType.Ignore, 163, "ReplOverrideData", "", 16)]
        public uint? ReplOverrideData { get; set; } //Type:  Bits: 16

        [NetFieldExport("EndGameStartTime", RepLayoutCmdType.PropertyFloat, 170, "EndGameStartTime", "", 32)]
        public float? EndGameStartTime { get; set; } //Type:  Bits: 32

        [NetFieldExport("TeamsLeft", RepLayoutCmdType.PropertyInt, 170, "TeamsLeft", "int32", 32)]
        public int? TeamsLeft { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("EndGameKickPlayerTime", RepLayoutCmdType.PropertyFloat, 171, "EndGameKickPlayerTime", "", 32)]
        public float? EndGameKickPlayerTime { get; set; } //Type:  Bits: 32

        [NetFieldExport("ServerToClientPreloadList", RepLayoutCmdType.Ignore, 171, "ServerToClientPreloadList", "TArray", 408)]
        public object[] ServerToClientPreloadList { get; set; } //Type: TArray Bits: 408

        [NetFieldExport("ClientVehicleClassesToLoad", RepLayoutCmdType.Ignore, 174, "ClientVehicleClassesToLoad", "", 72)]
        public int[] ClientVehicleClassesToLoad { get; set; } //Type:  Bits: 72

        [NetFieldExport("bAllowUserPickedCosmeticBattleBus", RepLayoutCmdType.Ignore, 175, "bAllowUserPickedCosmeticBattleBus", "bool", 1)]
        public bool? bAllowUserPickedCosmeticBattleBus { get; set; } //Type: bool Bits: 1

        [NetFieldExport("TeamFlightPaths", RepLayoutCmdType.DynamicArray, 176, "TeamFlightPaths", "TArray", 403)]
        public GameStateC[] TeamFlightPaths { get; set; } //Type: TArray Bits: 403

        [NetFieldExport("StormCapState", RepLayoutCmdType.Enum, 178, "StormCapState", "", 3)]
        public int? StormCapState { get; set; } //Type:  Bits: 3

        [NetFieldExport("WinningPlayerList", RepLayoutCmdType.DynamicArray, 184, "WinningPlayerList", "", 160)]
        public int[] WinningPlayerList { get; set; } //Type:  Bits: 160

        [NetFieldExport("FlightStartLocation", RepLayoutCmdType.PropertyVector100, 184, "FlightStartLocation", "FVector_NetQuantize100", 80)]
        public FVector FlightStartLocation { get; set; } //Type: FVector_NetQuantize100 Bits: 80

        [NetFieldExport("FlightStartRotation", RepLayoutCmdType.PropertyRotator, 185, "FlightStartRotation", "FRotator", 19)]
        public FRotator FlightStartRotation { get; set; } //Type: FRotator Bits: 19

        [NetFieldExport("FlightSpeed", RepLayoutCmdType.PropertyFloat, 186, "FlightSpeed", "float", 32)]
        public float? FlightSpeed { get; set; } //Type: float Bits: 32

        [NetFieldExport("TimeTillFlightEnd", RepLayoutCmdType.PropertyFloat, 187, "TimeTillFlightEnd", "float", 32)]
        public float? TimeTillFlightEnd { get; set; } //Type: float Bits: 32

        [NetFieldExport("TimeTillDropStart", RepLayoutCmdType.PropertyFloat, 188, "TimeTillDropStart", "float", 32)]
        public float? TimeTillDropStart { get; set; } //Type: float Bits: 32

        [NetFieldExport("TimeTillDropEnd", RepLayoutCmdType.PropertyFloat, 189, "TimeTillDropEnd", "float", 32)]
        public float? TimeTillDropEnd { get; set; } //Type: float Bits: 32

        [NetFieldExport("UtcTimeStartedMatch", RepLayoutCmdType.Property, 190, "UtcTimeStartedMatch", "FDateTime", 64)]
        public FDateTime UtcTimeStartedMatch { get; set; } //Type: FDateTime Bits: 64

        [NetFieldExport("SafeZonePhase", RepLayoutCmdType.PropertyByte, 201, "SafeZonePhase", "uint8", 8)]
        public byte? SafeZonePhase { get; set; } //Type: uint8 Bits: 8

        [NetFieldExport("GamePhase", RepLayoutCmdType.Enum, 204, "GamePhase", "EAthenaGamePhase", 3)]
        public int? GamePhase { get; set; } //Type: EAthenaGamePhase Bits: 3

        [NetFieldExport("Aircrafts", RepLayoutCmdType.Ignore, 207, "Aircrafts", "TArray", 64)]
        public DebuggingObject Aircrafts { get; set; } //Type: TArray Bits: 64

        [NetFieldExport("bAircraftIsLocked", RepLayoutCmdType.PropertyBool, 210, "bAircraftIsLocked", "uint8", 1)]
        public bool? bAircraftIsLocked { get; set; } //Type: uint8 Bits: 1

        [NetFieldExport("LobbyAction", RepLayoutCmdType.Ignore, 211, "LobbyAction", "int32", 32)]
        public int? LobbyAction { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("WinningPlayerState", RepLayoutCmdType.Ignore, 212, "WinningPlayerState", "", 16)]
        public uint? WinningPlayerState { get; set; } //Type:  Bits: 16

        [NetFieldExport("WinningTeam", RepLayoutCmdType.PropertyUInt32, 213, "WinningTeam", "", 32)]
        public uint? WinningTeam { get; set; } //Type:  Bits: 32

        [NetFieldExport("CurrentHighScore", RepLayoutCmdType.Ignore, 215, "CurrentHighScore", "", 32)]
        public uint? CurrentHighScore { get; set; } //Type:  Bits: 32

        [NetFieldExport("CurrentHighScoreTeam", RepLayoutCmdType.Ignore, 216, "CurrentHighScoreTeam", "", 32)]
        public uint? CurrentHighScoreTeam { get; set; } //Type:  Bits: 32

        [NetFieldExport("bStormReachedFinalPosition", RepLayoutCmdType.Ignore, 219, "bStormReachedFinalPosition", "", 1)]
        public bool? bStormReachedFinalPosition { get; set; } //Type:  Bits: 1

        [NetFieldExport("SpectateAPartyMemberAvailable", RepLayoutCmdType.Ignore, 220, "SpectateAPartyMemberAvailable", "", 1)]
        public bool? SpectateAPartyMemberAvailable { get; set; } //Type:  Bits: 1

        [NetFieldExport("HopRockDuration", RepLayoutCmdType.Ignore, 220, "HopRockDuration", "float", 32)]
        public float? HopRockDuration { get; set; } //Type: float Bits: 32

        [NetFieldExport("bIsLargeTeamGame", RepLayoutCmdType.PropertyBool, 220, "bIsLargeTeamGame", "", 1)]
        public bool? bIsLargeTeamGame { get; set; } //Type:  Bits: 1

        [NetFieldExport("ActiveTeamNums", RepLayoutCmdType.DynamicArray, 227, "ActiveTeamNums", "", 2416)]
        public byte[] ActiveTeamNums { get; set; } //Type:  Bits: 2416

        [NetFieldExport("AirCraftBehavior", RepLayoutCmdType.Ignore, 227, "AirCraftBehavior", "", 2)]
        public int? AirCraftBehavior { get; set; } //Type:  Bits: 2

        [NetFieldExport("DefaultGliderRedeployCanRedeploy", RepLayoutCmdType.Ignore, 229, "DefaultGliderRedeployCanRedeploy", "float", 32)]
        public float? DefaultGliderRedeployCanRedeploy { get; set; } //Type: float Bits: 32

        [NetFieldExport("DefaultRedeployGliderLateralVelocityMult", RepLayoutCmdType.Ignore, 230, "DefaultRedeployGliderLateralVelocityMult", "float", 32)]
        public float? DefaultRedeployGliderLateralVelocityMult { get; set; } //Type: float Bits: 32

        [NetFieldExport("DefaultRedeployGliderHeightLimit", RepLayoutCmdType.Ignore, 231, "DefaultRedeployGliderHeightLimit", "float", 32)]
        public float? DefaultRedeployGliderHeightLimit { get; set; } //Type: float Bits: 32

        [NetFieldExport("EventTournamentRound", RepLayoutCmdType.Enum, 235, "EventTournamentRound", "", 3)]
        public int? EventTournamentRound { get; set; } //Type:  Bits: 3

        [NetFieldExport("PlayerBotsLeft", RepLayoutCmdType.PropertyInt, 244, "PlayerBotsLeft", "", 32)]
        public int? PlayerBotsLeft { get; set; } //Type:  Bits: 32

        [NetFieldExport("DefaultParachuteDeployTraceForGroundDistance", RepLayoutCmdType.Ignore, 268, "DefaultParachuteDeployTraceForGroundDistance", "", 32)]
        public float? DefaultParachuteDeployTraceForGroundDistance { get; set; } //Type:  Bits: 32

        [NetFieldExport("DefaultRebootMachineHotfix", RepLayoutCmdType.Ignore, 270, "DefaultRebootMachineHotfix", "", 32)]
        public float? DefaultRebootMachineHotfix { get; set; } //Type:  Bits: 32

        [NetFieldExport("SignalInStormRegenSpeed", RepLayoutCmdType.Ignore, 271, "SignalInStormRegenSpeed", "", 32)]
        public float? SignalInStormRegenSpeed { get; set; } //Type:  Bits: 32

        [NetFieldExport("MutatorGenericInt", RepLayoutCmdType.Ignore, 271, "MutatorGenericInt", "", 32)]
        public uint? MutatorGenericInt { get; set; } //Type:  Bits: 32

        [NetFieldExport("SignalInStormLostSpeed", RepLayoutCmdType.Ignore, 272, "SignalInStormLostSpeed", "", 32)]
        public float? SignalInStormLostSpeed { get; set; } //Type:  Bits: 32

        [NetFieldExport("StormCNDamageVulnerabilityLevel0", RepLayoutCmdType.Ignore, 273, "StormCNDamageVulnerabilityLevel0", "", 32)]
        public float? StormCNDamageVulnerabilityLevel0 { get; set; } //Type:  Bits: 32

        [NetFieldExport("StormCNDamageVulnerabilityLevel1", RepLayoutCmdType.Ignore, 274, "StormCNDamageVulnerabilityLevel1", "", 32)]
        public float? StormCNDamageVulnerabilityLevel1 { get; set; } //Type:  Bits: 32

        [NetFieldExport("StormCNDamageVulnerabilityLevel2", RepLayoutCmdType.Ignore, 275, "StormCNDamageVulnerabilityLevel2", "", 32)]
        public float? StormCNDamageVulnerabilityLevel2 { get; set; } //Type:  Bits: 32

        [NetFieldExport("StormCNDamageVulnerabilityLevel3", RepLayoutCmdType.Ignore, 276, "StormCNDamageVulnerabilityLevel3", "", 32)]
        public float? StormCNDamageVulnerabilityLevel3 { get; set; } //Type:  Bits: 32

        [NetFieldExport("bEnabled", RepLayoutCmdType.Ignore, 277, "bEnabled", "", 1)]
        public bool? bEnabled { get; set; } //Type:  Bits: 1

        [NetFieldExport("bConnectedToRoot", RepLayoutCmdType.Ignore, 278, "bConnectedToRoot", "", 1)]
        public bool? bConnectedToRoot { get; set; } //Type:  Bits: 1

        [NetFieldExport("GameServerNodeType", RepLayoutCmdType.Ignore, 279, "GameServerNodeType", "", 3)]
        public int? GameServerNodeType { get; set; } //Type:  Bits: 3

        [NetFieldExport("VolumeManager", RepLayoutCmdType.Ignore, 280, "VolumeManager", "", 16)]
        public uint? VolumeManager { get; set; } //Type:  Bits: 16


    }


    [NetFieldExportGroup("/Script/FortniteGame.FortGameplayEffectDeliveryActor:BroadcastExplosion")]
    public class Explosion : INetFieldExportGroup
    {
        [NetFieldExport("HitActors", RepLayoutCmdType.DynamicArray, 0, "HitActors", "", 1)]
        public ActorId[] HitActors { get; set; } //Type: bool Bits: 1

        [NetFieldExport("HitResults", RepLayoutCmdType.DynamicArray, 0, "HitResults", "", 1)]
        public FHitResult[] HitResults { get; set; } //Type: bool Bits: 1
    }
}
