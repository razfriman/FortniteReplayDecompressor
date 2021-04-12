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
		public DebuggingObject UIExtensionTags { get; set; }

		[NetFieldExport("TimeLimit", RepLayoutCmdType.PropertyFloat)]
		public float? TimeLimit { get; set; }

		[NetFieldExport("CurrentRound", RepLayoutCmdType.PropertyUInt32)]
		public uint? CurrentRound { get; set; }

		[NetFieldExport("TotalRounds", RepLayoutCmdType.PropertyUInt32)]
		public uint? TotalRounds { get; set; }

		[NetFieldExport("WarmupDuration", RepLayoutCmdType.Property)]
		public DebuggingObject WarmupDuration { get; set; }

		[NetFieldExport("PostGameResetDelay", RepLayoutCmdType.Property)]
		public DebuggingObject PostGameResetDelay { get; set; }

		[NetFieldExport("GameWinnerDisplayTime", RepLayoutCmdType.Property)]
		public DebuggingObject GameWinnerDisplayTime { get; set; }

		[NetFieldExport("GameScoreDisplayTime", RepLayoutCmdType.Property)]
		public DebuggingObject GameScoreDisplayTime { get; set; }

		[NetFieldExport("RoundWinnerDisplayTime", RepLayoutCmdType.Property)]
		public DebuggingObject RoundWinnerDisplayTime { get; set; }

		[NetFieldExport("RoundScoreDisplayTime", RepLayoutCmdType.Property)]
		public DebuggingObject RoundScoreDisplayTime { get; set; }

		[NetFieldExport("TeamArray", RepLayoutCmdType.Property)]
		public DebuggingObject TeamArray { get; set; }

		[NetFieldExport("TeamIndex", RepLayoutCmdType.Property)]
		public DebuggingObject TeamIndex { get; set; }

		[NetFieldExport("TeamName", RepLayoutCmdType.Property)]
		public DebuggingObject TeamName { get; set; }

		[NetFieldExport("TeamColorIndex", RepLayoutCmdType.Property)]
		public DebuggingObject TeamColorIndex { get; set; }

		[NetFieldExport("MaxInitTeamSize", RepLayoutCmdType.Property)]
		public DebuggingObject MaxInitTeamSize { get; set; }

		[NetFieldExport("InitTeamSizeWeight", RepLayoutCmdType.Property)]
		public DebuggingObject InitTeamSizeWeight { get; set; }

		[NetFieldExport("bHasBucketAvailable", RepLayoutCmdType.Property)]
		public DebuggingObject bHasBucketAvailable { get; set; }

		[NetFieldExport("EliminatedCount", RepLayoutCmdType.Property)]
		public DebuggingObject EliminatedCount { get; set; }

		[NetFieldExport("TeamSize", RepLayoutCmdType.Property)]
		public DebuggingObject TeamSize { get; set; }

		[NetFieldExport("TrackedStats", RepLayoutCmdType.Property)]
		public DebuggingObject TrackedStats { get; set; }

		[NetFieldExport("ScoreboardStats", RepLayoutCmdType.Property)]
		public DebuggingObject ScoreboardStats { get; set; }

		[NetFieldExport("Stats", RepLayoutCmdType.Property)]
		public DebuggingObject Stats { get; set; }

		[NetFieldExport("Filter", RepLayoutCmdType.Property)]
		public DebuggingObject Filter { get; set; }

		[NetFieldExport("Count", RepLayoutCmdType.Property)]
		public DebuggingObject Count { get; set; }

		[NetFieldExport("PlayerStats", RepLayoutCmdType.Property)]
		public DebuggingObject PlayerStats { get; set; }

		[NetFieldExport("Player", RepLayoutCmdType.Property)]
		public DebuggingObject Player { get; set; }

		[NetFieldExport("PlayerBucketStats", RepLayoutCmdType.Property)]
		public DebuggingObject PlayerBucketStats { get; set; }

		[NetFieldExport("BucketIndex", RepLayoutCmdType.Property)]
		public DebuggingObject BucketIndex { get; set; }

		[NetFieldExport("CurrentState", RepLayoutCmdType.Property)]
		public DebuggingObject CurrentState { get; set; }

		[NetFieldExport("SetupTime", RepLayoutCmdType.Property)]
		public DebuggingObject SetupTime { get; set; }

		[NetFieldExport("WarmupTime", RepLayoutCmdType.Property)]
		public DebuggingObject WarmupTime { get; set; }

		[NetFieldExport("StartTime", RepLayoutCmdType.Property)]
		public DebuggingObject StartTime { get; set; }

		[NetFieldExport("EndTime", RepLayoutCmdType.Property)]
		public DebuggingObject EndTime { get; set; }

		[NetFieldExport("ResetTime", RepLayoutCmdType.Property)]
		public DebuggingObject ResetTime { get; set; }

		[NetFieldExport("Volume", RepLayoutCmdType.Property)]
		public DebuggingObject Volume { get; set; }

		[NetFieldExport("bTeamsAreStable", RepLayoutCmdType.Property)]
		public DebuggingObject bTeamsAreStable { get; set; }

		[NetFieldExport("bAllowJoinInProgress", RepLayoutCmdType.Property)]
		public DebuggingObject bAllowJoinInProgress { get; set; }

		[NetFieldExport("MinigameStarter", RepLayoutCmdType.Property)]
		public DebuggingObject MinigameStarter { get; set; }

		[NetFieldExport("bStableTeamCosmetics", RepLayoutCmdType.Property)]
		public DebuggingObject bStableTeamCosmetics { get; set; }

		[NetFieldExport("MinigameMapWidget", RepLayoutCmdType.Property)]
		public DebuggingObject MinigameMapWidget { get; set; }

		[NetFieldExport("WinCondition", RepLayoutCmdType.Property)]
		public DebuggingObject WinCondition { get; set; }

		[NetFieldExport("GameEndCallout", RepLayoutCmdType.Property)]
		public DebuggingObject GameEndCallout { get; set; }

		[NetFieldExport("bShowCumulativeScoreboard", RepLayoutCmdType.Property)]
		public DebuggingObject bShowCumulativeScoreboard { get; set; }

		[NetFieldExport("NumMinigameComponentsServer", RepLayoutCmdType.Property)]
		public DebuggingObject NumMinigameComponentsServer { get; set; }

		[NetFieldExport("bVolumeNavigationHasBuilt", RepLayoutCmdType.Property)]
		public DebuggingObject bVolumeNavigationHasBuilt { get; set; }

		[NetFieldExport("RoundWinHistory", RepLayoutCmdType.Property)]
		public DebuggingObject RoundWinHistory { get; set; }

		[NetFieldExport("PlayerBuckets", RepLayoutCmdType.Property)]
		public DebuggingObject PlayerBuckets { get; set; }

		[NetFieldExport("TeamIdAtGameStart", RepLayoutCmdType.Property)]
		public DebuggingObject TeamIdAtGameStart { get; set; }

		[NetFieldExport("TeamIdAtRoundStart", RepLayoutCmdType.Property)]
		public DebuggingObject TeamIdAtRoundStart { get; set; }

		[NetFieldExport("DesiredTeamSizePercent", RepLayoutCmdType.Property)]
		public DebuggingObject DesiredTeamSizePercent { get; set; }

		[NetFieldExport("PlayerIds", RepLayoutCmdType.Property)]
		public DebuggingObject PlayerIds { get; set; }

		[NetFieldExport("NumberOfHidersGameStart", RepLayoutCmdType.Property)]
		public DebuggingObject NumberOfHidersGameStart { get; set; }

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
					UIExtensionTags = (DebuggingObject)value;
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
					WarmupDuration = (DebuggingObject)value;
					break;
				case "PostGameResetDelay":
					PostGameResetDelay = (DebuggingObject)value;
					break;
				case "GameWinnerDisplayTime":
					GameWinnerDisplayTime = (DebuggingObject)value;
					break;
				case "GameScoreDisplayTime":
					GameScoreDisplayTime = (DebuggingObject)value;
					break;
				case "RoundWinnerDisplayTime":
					RoundWinnerDisplayTime = (DebuggingObject)value;
					break;
				case "RoundScoreDisplayTime":
					RoundScoreDisplayTime = (DebuggingObject)value;
					break;
				case "TeamArray":
					TeamArray = (DebuggingObject)value;
					break;
				case "TeamIndex":
					TeamIndex = (DebuggingObject)value;
					break;
				case "TeamName":
					TeamName = (DebuggingObject)value;
					break;
				case "TeamColorIndex":
					TeamColorIndex = (DebuggingObject)value;
					break;
				case "MaxInitTeamSize":
					MaxInitTeamSize = (DebuggingObject)value;
					break;
				case "InitTeamSizeWeight":
					InitTeamSizeWeight = (DebuggingObject)value;
					break;
				case "bHasBucketAvailable":
					bHasBucketAvailable = (DebuggingObject)value;
					break;
				case "EliminatedCount":
					EliminatedCount = (DebuggingObject)value;
					break;
				case "TeamSize":
					TeamSize = (DebuggingObject)value;
					break;
				case "TrackedStats":
					TrackedStats = (DebuggingObject)value;
					break;
				case "ScoreboardStats":
					ScoreboardStats = (DebuggingObject)value;
					break;
				case "Stats":
					Stats = (DebuggingObject)value;
					break;
				case "Filter":
					Filter = (DebuggingObject)value;
					break;
				case "Count":
					Count = (DebuggingObject)value;
					break;
				case "PlayerStats":
					PlayerStats = (DebuggingObject)value;
					break;
				case "Player":
					Player = (DebuggingObject)value;
					break;
				case "PlayerBucketStats":
					PlayerBucketStats = (DebuggingObject)value;
					break;
				case "BucketIndex":
					BucketIndex = (DebuggingObject)value;
					break;
				case "CurrentState":
					CurrentState = (DebuggingObject)value;
					break;
				case "SetupTime":
					SetupTime = (DebuggingObject)value;
					break;
				case "WarmupTime":
					WarmupTime = (DebuggingObject)value;
					break;
				case "StartTime":
					StartTime = (DebuggingObject)value;
					break;
				case "EndTime":
					EndTime = (DebuggingObject)value;
					break;
				case "ResetTime":
					ResetTime = (DebuggingObject)value;
					break;
				case "Volume":
					Volume = (DebuggingObject)value;
					break;
				case "bTeamsAreStable":
					bTeamsAreStable = (DebuggingObject)value;
					break;
				case "bAllowJoinInProgress":
					bAllowJoinInProgress = (DebuggingObject)value;
					break;
				case "MinigameStarter":
					MinigameStarter = (DebuggingObject)value;
					break;
				case "bStableTeamCosmetics":
					bStableTeamCosmetics = (DebuggingObject)value;
					break;
				case "MinigameMapWidget":
					MinigameMapWidget = (DebuggingObject)value;
					break;
				case "WinCondition":
					WinCondition = (DebuggingObject)value;
					break;
				case "GameEndCallout":
					GameEndCallout = (DebuggingObject)value;
					break;
				case "bShowCumulativeScoreboard":
					bShowCumulativeScoreboard = (DebuggingObject)value;
					break;
				case "NumMinigameComponentsServer":
					NumMinigameComponentsServer = (DebuggingObject)value;
					break;
				case "bVolumeNavigationHasBuilt":
					bVolumeNavigationHasBuilt = (DebuggingObject)value;
					break;
				case "RoundWinHistory":
					RoundWinHistory = (DebuggingObject)value;
					break;
				case "PlayerBuckets":
					PlayerBuckets = (DebuggingObject)value;
					break;
				case "TeamIdAtGameStart":
					TeamIdAtGameStart = (DebuggingObject)value;
					break;
				case "TeamIdAtRoundStart":
					TeamIdAtRoundStart = (DebuggingObject)value;
					break;
				case "DesiredTeamSizePercent":
					DesiredTeamSizePercent = (DebuggingObject)value;
					break;
				case "PlayerIds":
					PlayerIds = (DebuggingObject)value;
					break;
				case "NumberOfHidersGameStart":
					NumberOfHidersGameStart = (DebuggingObject)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}
	}
}
