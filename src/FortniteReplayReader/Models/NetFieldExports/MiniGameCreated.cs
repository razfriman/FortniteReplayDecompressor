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

		[NetFieldExport("TrackedStats", RepLayoutCmdType.Property)]
		public DebuggingObject TrackedStats { get; set; }

		[NetFieldExport("ScoreboardStats", RepLayoutCmdType.Property)]
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
		public int? CurrentState { get; set; }

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

		[NetFieldExport("Volume", RepLayoutCmdType.Property)]
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
		public int? WinCondition { get; set; }

		[NetFieldExport("GameEndCallout", RepLayoutCmdType.Enum)]
		public int? GameEndCallout { get; set; }

		[NetFieldExport("bShowCumulativeScoreboard", RepLayoutCmdType.PropertyBool)]
		public bool? bShowCumulativeScoreboard { get; set; }

		[NetFieldExport("NumMinigameComponentsServer", RepLayoutCmdType.PropertyInt)]
		public int? NumMinigameComponentsServer { get; set; }

		[NetFieldExport("bVolumeNavigationHasBuilt", RepLayoutCmdType.PropertyBool)]
		public bool? bVolumeNavigationHasBuilt { get; set; }

		[NetFieldExport("RoundWinHistory", RepLayoutCmdType.Property)]
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

		public override bool ManualRead(string property, object value)
		{
			switch (property)
			{
				case "RemoteRole":
					RemoteRole = (DebuggingObject)value;
					break;
				case "Role":
					Role = (DebuggingObject)value;
					break;
				case "UIExtensionTags":
					UIExtensionTags = (FGameplayTagContainer)value;
					break;
				case "TimeLimit":
					TimeLimit = (float)value;
					break;
				case "CurrentRound":
					CurrentRound = (uint)value;
					break;
				case "TotalRounds":
					TotalRounds = (uint)value;
					break;
				case "WarmupDuration":
					WarmupDuration = (float)value;
					break;
				case "PostGameResetDelay":
					PostGameResetDelay = (float)value;
					break;
				case "GameWinnerDisplayTime":
					GameWinnerDisplayTime = (float)value;
					break;
				case "GameScoreDisplayTime":
					GameScoreDisplayTime = (float)value;
					break;
				case "RoundWinnerDisplayTime":
					RoundWinnerDisplayTime = (float)value;
					break;
				case "RoundScoreDisplayTime":
					RoundScoreDisplayTime = (float)value;
					break;
				case "TeamArray":
					TeamArray = (MiniGameCreated[])value;
					break;
				case "TeamIndex":
					TeamIndex = (byte?)value;
					break;
				case "TeamName":
					TeamName = (string)value;
					break;
				case "TeamColorIndex":
					TeamColorIndex = (uint)value;
					break;
				case "MaxInitTeamSize":
					MaxInitTeamSize = (int)value;
					break;
				case "InitTeamSizeWeight":
					InitTeamSizeWeight = (int)value;
					break;
				case "bHasBucketAvailable":
					bHasBucketAvailable = (bool)value;
					break;
				case "EliminatedCount":
					EliminatedCount = (byte)value;
					break;
				case "TeamSize":
					TeamSize = (byte)value;
					break;
				case "TrackedStats":
					TrackedStats = (DebuggingObject)value; //Maybe an array of Stats?
					break;
				case "ScoreboardStats":
					ScoreboardStats = (DebuggingObject)value; //Maybe an array of Stats?
					break;
				case "Stats":
					Stats = (MiniGameCreated[])value;
					break;
				case "Filter":
					Filter = (NetworkGUID)value;
					break;
				case "Count":
					Count = (int)value;
					break;
				case "PlayerStats":
					PlayerStats = (MiniGameCreated[])value;
					break;
				case "Player":
					Player = (string)value;
					break;
				case "PlayerBucketStats":
					PlayerBucketStats = (MiniGameCreated[])value;
					break;
				case "BucketIndex":
					BucketIndex = (int)value;
					break;
				case "CurrentState":
					CurrentState = (int)value;
					break;
				case "SetupTime":
					SetupTime = (float)value;
					break;
				case "WarmupTime":
					WarmupTime = (float)value;
					break;
				case "StartTime":
					StartTime = (float)value;
					break;
				case "EndTime":
					EndTime = (float)value;
					break;
				case "ResetTime":
					ResetTime = (float)value;
					break;
				case "Volume":
					Volume = (DebuggingObject)value;
					break;
				case "bTeamsAreStable":
					bTeamsAreStable = (bool)value;
					break;
				case "bAllowJoinInProgress":
					bAllowJoinInProgress = (bool)value;
					break;
				case "MinigameStarter":
					MinigameStarter = (NetworkGUID)value;
					break;
				case "bStableTeamCosmetics":
					bStableTeamCosmetics = (bool)value;
					break;
				case "MinigameMapWidget":
					MinigameMapWidget = (int)value;
					break;
				case "WinCondition":
					WinCondition = (int)value;
					break;
				case "GameEndCallout":
					GameEndCallout = (int)value;
					break;
				case "bShowCumulativeScoreboard":
					bShowCumulativeScoreboard = (bool)value;
					break;
				case "NumMinigameComponentsServer":
					NumMinigameComponentsServer = (int)value;
					break;
				case "bVolumeNavigationHasBuilt":
					bVolumeNavigationHasBuilt = (bool)value;
					break;
				case "RoundWinHistory":
					RoundWinHistory = (DebuggingObject)value; //?
					break;
				case "PlayerBuckets":
					PlayerBuckets = (MiniGameCreated[])value;
					break;
				case "TeamIdAtGameStart":
					TeamIdAtGameStart = (byte)value;
					break;
				case "TeamIdAtRoundStart":
					TeamIdAtRoundStart = (byte)value;
					break;
				case "DesiredTeamSizePercent":
					DesiredTeamSizePercent = (float)value;
					break;
				case "PlayerIds":
					PlayerIds = (MinigamePlayerId)value;

					break;
				case "NumberOfHidersGameStart":
					NumberOfHidersGameStart = (int)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}
	}
}
