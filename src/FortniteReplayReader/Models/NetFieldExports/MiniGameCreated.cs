using FortniteReplayReader.Models.NetFieldExports.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
	[NetFieldExportGroup("/Game/Athena/Playset/Minigames/Minigame_PlayerCreated.Minigame_PlayerCreated_C", ParseType.Normal)]
	public class MiniGameCreated : INetFieldExportGroup
	{
		[NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
		public DebuggingObject RemoteRole { get; set; }

		[NetFieldExport("Role", RepLayoutCmdType.Ignore)]
		public DebuggingObject Role { get; set; }

		[NetFieldExport("UIExtensionTags", RepLayoutCmdType.Property)]
		public FGameplayTagContainer UIExtensionTags { get; set; }

		[NetFieldExport("TimeLimit", RepLayoutCmdType.PropertyFloat)]
		public float? TimeLimit { get; set; }

		[NetFieldExport("CurrentRound", RepLayoutCmdType.PropertyUInt32)]
		public uint? CurrentRound { get; set; }

		[NetFieldExport("TotalRounds", RepLayoutCmdType.PropertyUInt32)]
		public uint? TotalRounds { get; set; }

		[NetFieldExport("WarmupDuration", RepLayoutCmdType.PropertyFloat)]
		public float? WarmupDuration { get; set; }

		[NetFieldExport("PostGameResetDelay", RepLayoutCmdType.PropertyFloat)]
		public float? PostGameResetDelay { get; set; }

		[NetFieldExport("GameWinnerDisplayTime", RepLayoutCmdType.PropertyFloat)]
		public float? GameWinnerDisplayTime { get; set; }

		[NetFieldExport("GameScoreDisplayTime", RepLayoutCmdType.PropertyFloat)]
		public float? GameScoreDisplayTime { get; set; }

		[NetFieldExport("RoundWinnerDisplayTime", RepLayoutCmdType.PropertyFloat)]
		public float? RoundWinnerDisplayTime { get; set; }

		[NetFieldExport("RoundScoreDisplayTime", RepLayoutCmdType.PropertyFloat)]
		public float? RoundScoreDisplayTime { get; set; }

		[NetFieldExport("TeamArray", RepLayoutCmdType.DynamicArray)]
		public MiniGameCreated[] TeamArray { get; set; }

		[NetFieldExport("TeamIndex", RepLayoutCmdType.PropertyByte)]
		public byte? TeamIndex { get; set; }

		[NetFieldExport("TeamName", RepLayoutCmdType.PropertyString)]
		public string TeamName { get; set; }

		[NetFieldExport("TeamColorIndex", RepLayoutCmdType.PropertyUInt32)]
		public uint? TeamColorIndex { get; set; }

		[NetFieldExport("MaxInitTeamSize", RepLayoutCmdType.PropertyInt)]
		public int? MaxInitTeamSize { get; set; }

		[NetFieldExport("InitTeamSizeWeight", RepLayoutCmdType.PropertyInt)]
		public int? InitTeamSizeWeight { get; set; }

		[NetFieldExport("bHasBucketAvailable", RepLayoutCmdType.PropertyBool)]
		public bool? bHasBucketAvailable { get; set; }

		[NetFieldExport("EliminatedCount", RepLayoutCmdType.PropertyByte)]
		public byte? EliminatedCount { get; set; }

		[NetFieldExport("TeamSize", RepLayoutCmdType.PropertyByte)]
		public byte? TeamSize { get; set; }

		[NetFieldExport("TrackedStats", RepLayoutCmdType.Ignore)]
		public DebuggingObject TrackedStats { get; set; }

		[NetFieldExport("ScoreboardStats", RepLayoutCmdType.Ignore)]
		public DebuggingObject ScoreboardStats { get; set; }

		[NetFieldExport("Stats", RepLayoutCmdType.DynamicArray)]
		public MiniGameCreated[] Stats { get; set; }

		[NetFieldExport("Filter", RepLayoutCmdType.Property)]
		public NetworkGUID Filter { get; set; }

		[NetFieldExport("Count", RepLayoutCmdType.PropertyInt)]
		public int? Count { get; set; }

		[NetFieldExport("PlayerStats", RepLayoutCmdType.DynamicArray)]
		public MiniGameCreated[] PlayerStats { get; set; }

		[NetFieldExport("Player", RepLayoutCmdType.PropertyNetId)]
		public string Player { get; set; }

		[NetFieldExport("PlayerBucketStats", RepLayoutCmdType.DynamicArray)]
		public MiniGameCreated[] PlayerBucketStats { get; set; }

		[NetFieldExport("BucketIndex", RepLayoutCmdType.PropertyInt)]
		public int? BucketIndex { get; set; }

		[NetFieldExport("CurrentState", RepLayoutCmdType.Enum)]
		public EFortMinigameState CurrentState { get; set; } = EFortMinigameState.EFortMinigameState_MAX;

		[NetFieldExport("SetupTime", RepLayoutCmdType.PropertyFloat)]
		public float? SetupTime { get; set; }

		[NetFieldExport("WarmupTime", RepLayoutCmdType.PropertyFloat)]
		public float? WarmupTime { get; set; }

		[NetFieldExport("StartTime", RepLayoutCmdType.PropertyFloat)]
		public float? StartTime { get; set; }

		[NetFieldExport("EndTime", RepLayoutCmdType.PropertyFloat)]
		public float? EndTime { get; set; }

		[NetFieldExport("ResetTime", RepLayoutCmdType.PropertyFloat)]
		public float? ResetTime { get; set; }

		[NetFieldExport("Volume", RepLayoutCmdType.Ignore)]
		public DebuggingObject Volume { get; set; }

		[NetFieldExport("bTeamsAreStable", RepLayoutCmdType.PropertyBool)]
		public bool? bTeamsAreStable { get; set; }

		[NetFieldExport("bAllowJoinInProgress", RepLayoutCmdType.PropertyBool)]
		public bool? bAllowJoinInProgress { get; set; }

		[NetFieldExport("MinigameStarter", RepLayoutCmdType.Property)]
		public NetworkGUID MinigameStarter { get; set; }

		[NetFieldExport("bStableTeamCosmetics", RepLayoutCmdType.PropertyBool)]
		public bool? bStableTeamCosmetics { get; set; }

		[NetFieldExport("MinigameMapWidget", RepLayoutCmdType.Enum)]
		public int? MinigameMapWidget { get; set; }

		[NetFieldExport("WinCondition", RepLayoutCmdType.Enum)]
		public EMinigameWinCondition WinCondition { get; set; } = EMinigameWinCondition.EMinigameWinCondition_MAX;

		[NetFieldExport("GameEndCallout", RepLayoutCmdType.Enum)]
		public EMinigameGameEndCallout GameEndCallout { get; set; } = EMinigameGameEndCallout.EMinigameGameEndCallout_MAX;

		[NetFieldExport("bShowCumulativeScoreboard", RepLayoutCmdType.PropertyBool)]
		public bool? bShowCumulativeScoreboard { get; set; }

		[NetFieldExport("NumMinigameComponentsServer", RepLayoutCmdType.PropertyInt)]
		public int? NumMinigameComponentsServer { get; set; }

		[NetFieldExport("bVolumeNavigationHasBuilt", RepLayoutCmdType.PropertyBool)]
		public bool? bVolumeNavigationHasBuilt { get; set; }

		[NetFieldExport("RoundWinHistory", RepLayoutCmdType.Ignore)]
		public DebuggingObject RoundWinHistory { get; set; }

		[NetFieldExport("PlayerBuckets", RepLayoutCmdType.DynamicArray)]
		public MiniGameCreated[] PlayerBuckets { get; set; }

		[NetFieldExport("TeamIdAtGameStart", RepLayoutCmdType.PropertyByte)]
		public byte? TeamIdAtGameStart { get; set; }

		[NetFieldExport("TeamIdAtRoundStart", RepLayoutCmdType.PropertyByte)]
		public byte? TeamIdAtRoundStart { get; set; }

		[NetFieldExport("DesiredTeamSizePercent", RepLayoutCmdType.PropertyFloat)]
		public float? DesiredTeamSizePercent { get; set; }

		[NetFieldExport("PlayerIds", RepLayoutCmdType.Property)]
		public MinigamePlayerId PlayerIds { get; set; }

		[NetFieldExport("NumberOfHidersGameStart", RepLayoutCmdType.PropertyInt)]
		public int? NumberOfHidersGameStart { get; set; }
	}
}
