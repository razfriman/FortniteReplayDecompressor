using FortniteReplayReader.Models.NetFieldExports.Enums;
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

        [NetFieldExport("WorldLevel", RepLayoutCmdType.PropertyInt)]
        public int? WorldLevel { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("CraftingBonus", RepLayoutCmdType.PropertyInt)]
        public int? CraftingBonus { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("TeamCount", RepLayoutCmdType.PropertyInt)]
        public int? TeamCount { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("TeamSize", RepLayoutCmdType.PropertyInt)]
        public int? TeamSize { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("GameFlagData", RepLayoutCmdType.PropertyInt)]
        public int? GameFlagData { get; set; } //Type:  Bits: 32

        [NetFieldExport("AdditionalPlaylistLevelsStreamed", RepLayoutCmdType.DynamicArray)]
        public FName[] AdditionalPlaylistLevelsStreamed { get; set; } //Type: TArray Bits: 393

        [NetFieldExport("WorldDaysElapsed", RepLayoutCmdType.Ignore)]
        public int? WorldDaysElapsed { get; set; } //Type: int32 Bits: 32

		[NetFieldExport("GameplayState", RepLayoutCmdType.Enum)]
		public EFortGameplayState GameplayState { get; set; } = EFortGameplayState.EFortGameplayState_MAX; //Type: TEnumAsByte<EFortGameplayState::Type> Bits: 3

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

		[NetFieldExport("ServerStability", RepLayoutCmdType.Enum)]
		public EServerStability ServerStability { get; set; } = EServerStability.EServerStability_MAX; //Type:  Bits: 3

        [NetFieldExport("RoundTimeAccumulated", RepLayoutCmdType.PropertyInt)]
        public int? RoundTimeAccumulated { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("RoundTimeCriticalThreshold", RepLayoutCmdType.PropertyInt)]
        public int? RoundTimeCriticalThreshold { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("ServerChangelistNumber", RepLayoutCmdType.PropertyInt)]
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

        [NetFieldExport("bSkyTubesShuttingDown", RepLayoutCmdType.PropertyBool)]
        public bool? bSkyTubesShuttingDown { get; set; } //Type:  Bits: 1

        [NetFieldExport("SafeZonesStartTime", RepLayoutCmdType.PropertyFloat)]
        public float? SafeZonesStartTime { get; set; } //Type: float Bits: 32

        [NetFieldExport("bSkyTubesDisabled", RepLayoutCmdType.PropertyBool)]
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
		public EAthenaStormCapState StormCapState { get; set; } = EAthenaStormCapState.EAthenaStormCapState_MAX; //Type:  Bits: 3

        [NetFieldExport("WinningPlayerList", RepLayoutCmdType.DynamicArray)]
        public int[] WinningPlayerList { get; set; } //Type:  Bits: 160

        [NetFieldExport("FlightStartLocation", RepLayoutCmdType.PropertyVector100)]
        public FVector? FlightStartLocation { get; set; } //Type: FVector_NetQuantize100 Bits: 80

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
		public EAthenaGamePhase GamePhase { get; set; } = EAthenaGamePhase.EAthenaGamePhase_MAX; //Type: EAthenaGamePhase Bits: 3

        [NetFieldExport("Aircrafts", RepLayoutCmdType.Ignore)]
        public DebuggingObject Aircrafts { get; set; } //Type: TArray Bits: 64

        [NetFieldExport("bAircraftIsLocked", RepLayoutCmdType.PropertyBool)]
        public bool? bAircraftIsLocked { get; set; } //Type: uint8 Bits: 1

        [NetFieldExport("LobbyAction", RepLayoutCmdType.Ignore)]
        public int? LobbyAction { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("WinningPlayerState", RepLayoutCmdType.Property)]
        public ActorGUID WinningPlayerState { get; set; } //Type:  Bits: 16

        [NetFieldExport("WinningTeam", RepLayoutCmdType.PropertyUInt32)]
        public uint? WinningTeam { get; set; } //Type:  Bits: 32

        [NetFieldExport("CurrentHighScore", RepLayoutCmdType.PropertyInt)]
        public int? CurrentHighScore { get; set; } //Type:  Bits: 32

        [NetFieldExport("CurrentHighScoreTeam", RepLayoutCmdType.PropertyInt)]
        public int? CurrentHighScoreTeam { get; set; } //Type:  Bits: 32

        [NetFieldExport("bStormReachedFinalPosition", RepLayoutCmdType.Ignore)]
        public bool? bStormReachedFinalPosition { get; set; } //Type:  Bits: 1

        [NetFieldExport("SpectateAPartyMemberAvailable", RepLayoutCmdType.Ignore)]
        public bool? SpectateAPartyMemberAvailable { get; set; } //Type:  Bits: 1

        [NetFieldExport("HopRockDuration", RepLayoutCmdType.PropertyFloat)]
        public float? HopRockDuration { get; set; } //Type: float Bits: 32

        [NetFieldExport("bIsLargeTeamGame", RepLayoutCmdType.PropertyBool)]
        public bool? bIsLargeTeamGame { get; set; } //Type:  Bits: 1

        [NetFieldExport("ActiveTeamNums", RepLayoutCmdType.DynamicArray)]
        public byte[] ActiveTeamNums { get; set; } //Type:  Bits: 2416

		[NetFieldExport("AirCraftBehavior", RepLayoutCmdType.Enum)]
		public EAirCraftBehavior AirCraftBehavior { get; set; } = EAirCraftBehavior.EAirCraftBehavior_MAX; //Type:  Bits: 2

        [NetFieldExport("DefaultGliderRedeployCanRedeploy", RepLayoutCmdType.PropertyFloat)]
        public float? DefaultGliderRedeployCanRedeploy { get; set; } //Type: float Bits: 32

        [NetFieldExport("DefaultRedeployGliderLateralVelocityMult", RepLayoutCmdType.PropertyFloat)]
        public float? DefaultRedeployGliderLateralVelocityMult { get; set; } //Type: float Bits: 32

        [NetFieldExport("DefaultRedeployGliderHeightLimit", RepLayoutCmdType.PropertyFloat)]
        public float? DefaultRedeployGliderHeightLimit { get; set; } //Type: float Bits: 32

		[NetFieldExport("EventTournamentRound", RepLayoutCmdType.Enum)]
		public EEventTournamentRound EventTournamentRound { get; set; } = EEventTournamentRound.EEventTournamentRound_MAX;//Type:  Bits: 3

        [NetFieldExport("PlayerBotsLeft", RepLayoutCmdType.PropertyInt)]
        public int? PlayerBotsLeft { get; set; } //Type:  Bits: 32

        [NetFieldExport("DefaultParachuteDeployTraceForGroundDistance", RepLayoutCmdType.PropertyFloat)]
        public float? DefaultParachuteDeployTraceForGroundDistance { get; set; } //Type:  Bits: 32

        [NetFieldExport("DefaultRebootMachineHotfix", RepLayoutCmdType.PropertyFloat)]
        public float? DefaultRebootMachineHotfix { get; set; } //Type:  Bits: 32

        [NetFieldExport("SignalInStormRegenSpeed", RepLayoutCmdType.PropertyFloat)]
        public float? SignalInStormRegenSpeed { get; set; } //Type:  Bits: 32

        [NetFieldExport("MutatorGenericInt", RepLayoutCmdType.Ignore)]
        public uint? MutatorGenericInt { get; set; } //Type:  Bits: 32

        [NetFieldExport("SignalInStormLostSpeed", RepLayoutCmdType.PropertyFloat)]
        public float? SignalInStormLostSpeed { get; set; } //Type:  Bits: 32

        [NetFieldExport("StormCNDamageVulnerabilityLevel0", RepLayoutCmdType.PropertyFloat)]
        public float? StormCNDamageVulnerabilityLevel0 { get; set; } //Type:  Bits: 32

        [NetFieldExport("StormCNDamageVulnerabilityLevel1", RepLayoutCmdType.PropertyFloat)]
        public float? StormCNDamageVulnerabilityLevel1 { get; set; } //Type:  Bits: 32

        [NetFieldExport("StormCNDamageVulnerabilityLevel2", RepLayoutCmdType.PropertyFloat)]
        public float? StormCNDamageVulnerabilityLevel2 { get; set; } //Type:  Bits: 32

        [NetFieldExport("StormCNDamageVulnerabilityLevel3", RepLayoutCmdType.PropertyFloat)]
        public float? StormCNDamageVulnerabilityLevel3 { get; set; } //Type:  Bits: 32

        [NetFieldExport("bEnabled", RepLayoutCmdType.PropertyBool)]
        public bool? bEnabled { get; set; } //Type:  Bits: 1

        [NetFieldExport("bConnectedToRoot", RepLayoutCmdType.PropertyBool)]
        public bool? bConnectedToRoot { get; set; } //Type:  Bits: 1

		[NetFieldExport("GameServerNodeType", RepLayoutCmdType.Enum)]
		public EMeshNetworkNodeType GameServerNodeType { get; set; } = EMeshNetworkNodeType.EMeshNetworkNodeType_MAX; //Type:  Bits: 3

        [NetFieldExport("VolumeManager", RepLayoutCmdType.Ignore)]
        public uint? VolumeManager { get; set; } //Type:  Bits: 16

        [NetFieldExport("TrackedCosmetics", RepLayoutCmdType.DynamicArray)]
        public ItemDefinitionGUID[] TrackedCosmetics { get; set; }

        [NetFieldExport("VariantUsageByCosmetic", RepLayoutCmdType.DynamicArray)]
        public int[] VariantUsageByCosmetic { get; set; }

        [NetFieldExport("PrioritizedCosmeticIndices", RepLayoutCmdType.DynamicArray)]
        public int[] PrioritizedCosmeticIndices { get; set; }

        [NetFieldExport("EventId", RepLayoutCmdType.PropertyUInt32)]
        public uint? EventId { get; set; }

        [NetFieldExport("WinningScore", RepLayoutCmdType.PropertyUInt32)]
        public uint? WinningScore { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "RemoteRole":
					RemoteRole = value;
					break;
				case "Role":
					Role = value;
					break;
				case "GameModeClass":
					GameModeClass = (uint)value;
					break;
				case "SpectatorClass":
					SpectatorClass = (uint)value;
					break;
				case "FortTimeOfDayManager":
					FortTimeOfDayManager = (uint)value;
					break;
				case "PoiManager":
					PoiManager = (uint)value;
					break;
				case "FeedbackManager":
					FeedbackManager = (uint)value;
					break;
				case "MissionManager":
					MissionManager = (uint)value;
					break;
				case "AnnouncementManager":
					AnnouncementManager = (uint)value;
					break;
				case "WorldManager":
					WorldManager = (uint)value;
					break;
				case "MusicManagerSubclass":
					MusicManagerSubclass = (uint)value;
					break;
				case "MusicManagerBank":
					MusicManagerBank = (uint)value;
					break;
				case "PawnForReplayRelevancy":
					PawnForReplayRelevancy = (uint)value;
					break;
				case "RecorderPlayerState":
					RecorderPlayerState = (uint)value;
					break;
				case "GlobalEnvironmentAbilityActor":
					GlobalEnvironmentAbilityActor = (uint)value;
					break;
				case "UIMapManager":
					UIMapManager = (uint)value;
					break;
				case "CreativePlotManager":
					CreativePlotManager = (uint)value;
					break;
				case "PlayspaceManager":
					PlayspaceManager = (uint)value;
					break;
				case "ItemCollector":
					ItemCollector = (uint)value;
					break;
				case "SpecialActorData":
					SpecialActorData = (uint)value;
					break;
				case "SupplyDropWaveStartedSoundCue":
					SupplyDropWaveStartedSoundCue = (uint)value;
					break;
				case "TeamXPlayersLeft":
					TeamXPlayersLeft = (DebuggingObject)value;
					break;
				case "SafeZoneIndicator":
					SafeZoneIndicator = (uint)value;
					break;
				case "MapInfo":
					MapInfo = (uint)value;
					break;
				case "GoldenPoiLocationTags":
					GoldenPoiLocationTags = (DebuggingObject)value;
					break;
				case "DefaultBattleBus":
					DefaultBattleBus = (uint)value;
					break;
				case "bReplicatedHasBegunPlay":
					bReplicatedHasBegunPlay = (bool)value;
					break;
				case "ReplicatedWorldTimeSeconds":
					ReplicatedWorldTimeSeconds = (float)value;
					break;
				case "MatchState":
					MatchState = (FName)value;
					break;
				case "ElapsedTime":
					ElapsedTime = (int)value;
					break;
				case "WorldLevel":
					WorldLevel = (int)value;
					break;
				case "CraftingBonus":
					CraftingBonus = (int)value;
					break;
				case "TeamCount":
					TeamCount = (int)value;
					break;
				case "TeamSize":
					TeamSize = (int)value;
					break;
				case "GameFlagData":
					GameFlagData = (int)value;
					break;
				case "AdditionalPlaylistLevelsStreamed":
					AdditionalPlaylistLevelsStreamed = (FName[])value;
					break;
				case "WorldDaysElapsed":
					WorldDaysElapsed = (int)value;
					break;
				case "GameplayState":
					GameplayState = (EFortGameplayState)value;
					break;
				case "GameSessionId":
					GameSessionId = (string)value;
					break;
				case "SpawnPointsCap":
					SpawnPointsCap = (int)value;
					break;
				case "SpawnPointsAllocated":
					SpawnPointsAllocated = (int)value;
					break;
				case "PlayerSharedMaxTrapAttributes":
					PlayerSharedMaxTrapAttributes = (object[])value;
					break;
				case "TotalPlayerStructures":
					TotalPlayerStructures = (int)value;
					break;
				case "ServerGameplayTagIndexHash":
					ServerGameplayTagIndexHash = (uint)value;
					break;
				case "GameDifficulty":
					GameDifficulty = (float)value;
					break;
				case "bAllowLayoutRequirementsFeature":
					bAllowLayoutRequirementsFeature = (bool)value;
					break;
				case "ServerStability":
					ServerStability = (EServerStability)value;
					break;
				case "RoundTimeAccumulated":
					RoundTimeAccumulated = (int)value;
					break;
				case "RoundTimeCriticalThreshold":
					RoundTimeCriticalThreshold = (int)value;
					break;
				case "ServerChangelistNumber":
					ServerChangelistNumber = (int)value;
					break;
				case "CreativeRealEstatePlotManager":
					CreativeRealEstatePlotManager = (uint)value;
					break;
				case "WarmupCountdownStartTime":
					WarmupCountdownStartTime = (float)value;
					break;
				case "WarmupCountdownEndTime":
					WarmupCountdownEndTime = (float)value;
					break;
				case "bSafeZonePaused":
					bSafeZonePaused = (bool)value;
					break;
				case "AircraftStartTime":
					AircraftStartTime = (float)value;
					break;
				case "bSkyTubesShuttingDown":
					bSkyTubesShuttingDown = (bool)value;
					break;
				case "SafeZonesStartTime":
					SafeZonesStartTime = (float)value;
					break;
				case "bSkyTubesDisabled":
					bSkyTubesDisabled = (bool)value;
					break;
				case "PlayersLeft":
					PlayersLeft = (int)value;
					break;
				case "ReplOverrideData":
					ReplOverrideData = (uint)value;
					break;
				case "EndGameStartTime":
					EndGameStartTime = (float)value;
					break;
				case "TeamsLeft":
					TeamsLeft = (int)value;
					break;
				case "EndGameKickPlayerTime":
					EndGameKickPlayerTime = (float)value;
					break;
				case "ServerToClientPreloadList":
					ServerToClientPreloadList = (object[])value;
					break;
				case "ClientVehicleClassesToLoad":
					ClientVehicleClassesToLoad = (int[])value;
					break;
				case "bAllowUserPickedCosmeticBattleBus":
					bAllowUserPickedCosmeticBattleBus = (bool)value;
					break;
				case "TeamFlightPaths":
					TeamFlightPaths = (GameStateC[])value;
					break;
				case "StormCapState":
					StormCapState = (EAthenaStormCapState)value;
					break;
				case "WinningPlayerList":
					WinningPlayerList = (int[])value;
					break;
				case "FlightStartLocation":
					FlightStartLocation = (FVector)value;
					break;
				case "FlightStartRotation":
					FlightStartRotation = (FRotator)value;
					break;
				case "FlightSpeed":
					FlightSpeed = (float)value;
					break;
				case "TimeTillFlightEnd":
					TimeTillFlightEnd = (float)value;
					break;
				case "TimeTillDropStart":
					TimeTillDropStart = (float)value;
					break;
				case "TimeTillDropEnd":
					TimeTillDropEnd = (float)value;
					break;
				case "UtcTimeStartedMatch":
					UtcTimeStartedMatch = (FDateTime)value;
					break;
				case "SafeZonePhase":
					SafeZonePhase = (byte)value;
					break;
				case "GamePhase":
					GamePhase = (EAthenaGamePhase)value;
					break;
				case "Aircrafts":
					Aircrafts = (DebuggingObject)value;
					break;
				case "bAircraftIsLocked":
					bAircraftIsLocked = (bool)value;
					break;
				case "LobbyAction":
					LobbyAction = (int)value;
					break;
				case "WinningPlayerState":
					WinningPlayerState = (ActorGUID)value;
					break;
				case "WinningTeam":
					WinningTeam = (uint)value;
					break;
				case "CurrentHighScore":
					CurrentHighScore = (int)value;
					break;
				case "CurrentHighScoreTeam":
					CurrentHighScoreTeam = (int)value;
					break;
				case "bStormReachedFinalPosition":
					bStormReachedFinalPosition = (bool)value;
					break;
				case "SpectateAPartyMemberAvailable":
					SpectateAPartyMemberAvailable = (bool)value;
					break;
				case "HopRockDuration":
					HopRockDuration = (float)value;
					break;
				case "bIsLargeTeamGame":
					bIsLargeTeamGame = (bool)value;
					break;
				case "ActiveTeamNums":
					ActiveTeamNums = (byte[])value;
					break;
				case "AirCraftBehavior":
					AirCraftBehavior = (EAirCraftBehavior)value;
					break;
				case "DefaultGliderRedeployCanRedeploy":
					DefaultGliderRedeployCanRedeploy = (float)value;
					break;
				case "DefaultRedeployGliderLateralVelocityMult":
					DefaultRedeployGliderLateralVelocityMult = (float)value;
					break;
				case "DefaultRedeployGliderHeightLimit":
					DefaultRedeployGliderHeightLimit = (float)value;
					break;
				case "EventTournamentRound":
					EventTournamentRound = (EEventTournamentRound)value;
					break;
				case "PlayerBotsLeft":
					PlayerBotsLeft = (int)value;
					break;
				case "DefaultParachuteDeployTraceForGroundDistance":
					DefaultParachuteDeployTraceForGroundDistance = (float)value;
					break;
				case "DefaultRebootMachineHotfix":
					DefaultRebootMachineHotfix = (float)value;
					break;
				case "SignalInStormRegenSpeed":
					SignalInStormRegenSpeed = (float)value;
					break;
				case "MutatorGenericInt":
					MutatorGenericInt = (uint)value;
					break;
				case "SignalInStormLostSpeed":
					SignalInStormLostSpeed = (float)value;
					break;
				case "StormCNDamageVulnerabilityLevel0":
					StormCNDamageVulnerabilityLevel0 = (float)value;
					break;
				case "StormCNDamageVulnerabilityLevel1":
					StormCNDamageVulnerabilityLevel1 = (float)value;
					break;
				case "StormCNDamageVulnerabilityLevel2":
					StormCNDamageVulnerabilityLevel2 = (float)value;
					break;
				case "StormCNDamageVulnerabilityLevel3":
					StormCNDamageVulnerabilityLevel3 = (float)value;
					break;
				case "bEnabled":
					bEnabled = (bool)value;
					break;
				case "bConnectedToRoot":
					bConnectedToRoot = (bool)value;
					break;
				case "GameServerNodeType":
					GameServerNodeType = (EMeshNetworkNodeType)value;
					break;
				case "VolumeManager":
					VolumeManager = (uint)value;
					break;
				case "TrackedCosmetics":
					TrackedCosmetics = (ItemDefinitionGUID[])value;
					break;
				case "VariantUsageByCosmetic":
					VariantUsageByCosmetic = (int[])value;
					break;
				case "PrioritizedCosmeticIndices":
					PrioritizedCosmeticIndices = (int[])value;
					break;
				case "EventId":
					EventId = (uint)value;
					break;
				case "WinningScore":
					WinningScore = (uint)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }
}
