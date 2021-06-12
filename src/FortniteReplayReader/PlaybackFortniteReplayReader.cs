using FortniteReplayReader.Exceptions;
using FortniteReplayReader.Models;
using FortniteReplayReader.Models.Enums;
using FortniteReplayReader.Models.NetFieldExports;
using FortniteReplayReader.Models.NetFieldExports.Builds;
using FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Custom;
using FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Functions;
using FortniteReplayReader.Models.NetFieldExports.Items.Containers;
using FortniteReplayReader.Models.NetFieldExports.Items.Weapons;
using FortniteReplayReader.Models.NetFieldExports.Sets;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using Unreal.Core;
using Unreal.Core.Contracts;
using Unreal.Core.Exceptions;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;
using Unreal.Core.Models.Playback;

namespace FortniteReplayReader
{
    public class PlaybackFortniteReplayReader : PlaybackReplayReader<FortniteReplay>
    {
        private string _branch;
        public int Major { get; set; }
        public int Minor { get; set; }
        public string Branch
        {
            get { return _branch; }
            set
            {
                var regex = new Regex(@"\+\+Fortnite\+Release\-(?<major>\d+)\.(?<minor>\d*)");
                var result = regex.Match(value);
                if (result.Success)
                {
                    Major = int.Parse(result.Groups["major"]?.Value ?? "0");
                    Minor = int.Parse(result.Groups["minor"]?.Value ?? "0");
                }
                _branch = value;
            }
        }

        public PlaybackFortniteReplayReader(ILogger logger) : base(logger)
        {
        }

        protected override void Update(ReplayPlaybackEvent playbackEvent)
        {
            switch (playbackEvent)
            {
                case ExportGroupPlaybackEvent exportGroupEvent:
                    ExportGroupRead(exportGroupEvent.Channel, exportGroupEvent.ExportGroup);
                    break;
                case ChannelClosedPlaybackEvent channelClosedEvent:
                    Replay.GameInformation.ChannelClosed(channelClosedEvent.Channel);
                    break;
                case NetDeltaPlaybackEvent deltaEvent:
                    Replay.GameInformation.HandleDeltaNetRead(deltaEvent.DeltaUpdate);
                    break;
                case ActorReadPlaybackEvent actorReadEvent:
                    Replay.GameInformation.AddActor(actorReadEvent.Channel, actorReadEvent.Actor);
                    break;
            }
        }

        private void ExportGroupRead(uint channel, INetFieldExportGroup exportGroup)
        { 
            //Used for weapon data and possibly other things
            if (Replay.GameInformation.NetGUIDToPathName == null)
            {
                Replay.GameInformation.NetGUIDToPathName = GuidCache.NetGuidToPathName;
            }

            switch (exportGroup)
            {
                case BaseStructure baseBuild:
                    Replay.GameInformation.UpdateBuild(channel, baseBuild);
                    break;
                case FortTeamPrivateInfo privateTeamInfo:
                    Replay.GameInformation.UpdatePrivateTeamInfo(channel, privateTeamInfo);
                    break;
                case SearchableContainer searchableContainer:
                    Replay.GameInformation.UpdateSearchableContainer(channel, searchableContainer);
                    break;
                case GameplayCueExecuted cueExecuted:
                    //Fall damage should be in here
                    break;
                case CurrentPlaylistInfo playlistInfo:
                    Replay.GameInformation.UpdatePlaylistInfo(channel, playlistInfo);
                    break;
                case SupplyDropLlamaC llama:
                    Replay.GameInformation.UpdateLlama(channel, llama);
                    break;
                case SupplyDropC supplyDrop:
                    Replay.GameInformation.UpdateSupplyDrop(channel, supplyDrop);
                    break;
                case LabradorLlamaC labradorLlama:
                    Replay.GameInformation.UpdateLabradorLlama(labradorLlama);
                    break;
                case FreshCheeseMinigameC freshCheeseMinigame:
                    Replay.GameInformation.UpdateFreshCheeseMinigame(channel, freshCheeseMinigame);
                    break;
                case GameStateC gameState:
                    Replay.GameInformation.UpdateGameState(gameState);
                    break;
                case FortPlayerState playerState:
                    Replay.GameInformation.UpdatePlayerState(channel, playerState, GuidCache.NetworkGameplayTagNodeIndex);
                    break;
                case PlayerPawnC playerPawn:
                    if (ParseType >= ParseType.Normal)
                    {
                        Replay.GameInformation.UpdatePlayerPawn(channel, playerPawn, false);
                    }
                    break;
                case FortPickup fortPickup:
                    if (ParseType >= ParseType.Normal)
                    {
                        Replay.GameInformation.UpdateFortPickup(channel, fortPickup);
                    }
                    break;
                case SafeZoneIndicatorC safeZoneIndicator:
                    Replay.GameInformation.UpdateSafeZone(safeZoneIndicator);
                    break;
                case BatchedDamage batchedDamage:
                    Replay.GameInformation.UpdateBatchedDamage(channel, batchedDamage);
                    break;
                case GameplayCue gameplayCue:
                    gameplayCue.GameplayCueTag?.UpdateTagName(GuidCache.NetworkGameplayTagNodeIndex);
                    Replay.GameInformation.HandleGameplayCue(gameplayCue);
                    break;
                case BaseWeapon weapon:
                    Replay.GameInformation.HandleWeapon(channel, weapon);
                    break;
                case FortPoiManager poiManager:
                    Replay.GameInformation.UpdatePoiManager(poiManager, GuidCache.NetworkGameplayTagNodeIndex);
                    break;
                case FortInventory inventory:
                    Replay.GameInformation.UpdateFortInventory(channel, inventory);
                    break;
                case BaseProp prop:
                    Replay.GameInformation.UpdateContainer(channel, prop);
                    break;
                case HealthSet healthSet:
                    Replay.GameInformation.UpdateHealth(channel, healthSet, GuidCache);
                    break;
            }
        }

        #region Somehow use a general fortnite reader?

        protected override void ReadReplayHeader(FArchive archive)
        {
            base.ReadReplayHeader(archive);
            Branch = Replay.Header.Branch;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/NetworkReplayStreaming/LocalFileNetworkReplayStreaming/Private/LocalFileNetworkReplayStreaming.cpp#L363
        /// </summary>
        /// <param name="archive"></param>
        /// <returns></returns>
        protected override void ReadEvent(FArchive archive)
        {
            var info = new EventInfo
            {
                Id = archive.ReadFString(),
                Group = archive.ReadFString(),
                Metadata = archive.ReadFString(),
                StartTime = archive.ReadUInt32(),
                EndTime = archive.ReadUInt32(),
                SizeInBytes = archive.ReadInt32()
            };

            _logger?.LogDebug($"Encountered event {info.Group} ({info.Metadata}) at {info.StartTime} of size {info.SizeInBytes}");

            // Every event seems to start with some unknown int
            if (info.Group == ReplayEventTypes.PLAYER_ELIMINATION)
            {
                var elimination = ParseElimination(archive, info);
                Replay.Eliminations.Add(elimination);
                return;
            }
            else if (info.Metadata == ReplayEventTypes.MATCH_STATS)
            {
                Replay.Stats = ParseMatchStats(archive, info);
                return;
            }
            else if (info.Metadata == ReplayEventTypes.TEAM_STATS)
            {
                Replay.TeamStats = ParseTeamStats(archive, info);
                return;
            }
            else if (info.Metadata == ReplayEventTypes.ENCRYPTION_KEY)
            {
                Replay.GameInformation.PlayerStateEncryptionKey = ParseEncryptionKeyEvent(archive, info);
                return;
            }
            else if (info.Metadata == ReplayEventTypes.CHARACTER_SAMPLE)
            {
                ParseCharacterSample(archive, info);
                return;
            }
            else if (info.Group == ReplayEventTypes.ZONE_UPDATE)
            {
                ParseZoneUpdateEvent(archive, info);
                return;
            }
            else if (info.Group == ReplayEventTypes.BATTLE_BUS)
            {
                ParseBattleBusFlightEvent(archive, info);
                return;
            }
            else if (info.Group == "fortBenchEvent")
            {
                return;
            }

            _logger?.LogWarning($"Unknown event {info.Group} ({info.Metadata}) of size {info.SizeInBytes}");
            // optionally throw?
            throw new UnknownEventException($"Unknown event {info.Group} ({info.Metadata}) of size {info.SizeInBytes}");
        }

        protected virtual CharacterSample ParseCharacterSample(FArchive archive, EventInfo info)
        {
            return new CharacterSample()
            {
                Info = info,
            };
        }

        protected virtual EncryptionKey ParseEncryptionKeyEvent(FArchive archive, EventInfo info)
        {
            return new EncryptionKey()
            {
                Info = info,
                Key = archive.ReadBytesToString(32)
            };
        }

        protected virtual ZoneUpdate ParseZoneUpdateEvent(FArchive archive, EventInfo info)
        {
            // 21 bytes in 9, 20 in 9.10...
            return new ZoneUpdate()
            {
                Info = info,
            };
        }

        protected virtual BattleBusFlight ParseBattleBusFlightEvent(FArchive archive, EventInfo info)
        {
            // Added in 9 and removed again in 9.10?
            return new BattleBusFlight()
            {
                Info = info,
            };
        }

        protected virtual TeamStats ParseTeamStats(FArchive archive, EventInfo info)
        {
            return new TeamStats()
            {
                Info = info,
                Unknown = archive.ReadUInt32(),
                Position = archive.ReadUInt32(),
                TotalPlayers = archive.ReadUInt32()
            };
        }

        protected virtual Stats ParseMatchStats(FArchive archive, EventInfo info)
        {
            return new Stats()
            {
                Info = info,
                Unknown = archive.ReadUInt32(),
                Accuracy = archive.ReadSingle(),
                Assists = archive.ReadUInt32(),
                Eliminations = archive.ReadUInt32(),
                WeaponDamage = archive.ReadUInt32(),
                OtherDamage = archive.ReadUInt32(),
                Revives = archive.ReadUInt32(),
                DamageTaken = archive.ReadUInt32(),
                DamageToStructures = archive.ReadUInt32(),
                MaterialsGathered = archive.ReadUInt32(),
                MaterialsUsed = archive.ReadUInt32(),
                TotalTraveled = archive.ReadUInt32()
            };
        }

        protected virtual PlayerElimination ParseElimination(FArchive archive, EventInfo info)
        {
            try
            {
                var elim = new PlayerElimination
                {
                    Info = info,
                };

                if (archive.EngineNetworkVersion >= EngineNetworkVersionHistory.HISTORY_FAST_ARRAY_DELTA_STRUCT && Major >= 9)
                {
                    archive.SkipBytes(9);

                    elim.EliminatedInfo = new PlayerEliminationInfo
                    {
                        Unknown1 = new FVector(archive.ReadSingle(), archive.ReadSingle(), archive.ReadSingle()),
                        Location = new FVector(archive.ReadSingle(), archive.ReadSingle(), archive.ReadSingle()),
                        Unknown2 = new FVector(archive.ReadSingle(), archive.ReadSingle(), archive.ReadSingle()),
                    };

                    archive.ReadSingle(); //?

                    elim.EliminatorInfo = new PlayerEliminationInfo
                    {
                        Unknown1 = new FVector(archive.ReadSingle(), archive.ReadSingle(), archive.ReadSingle()),
                        Location = new FVector(archive.ReadSingle(), archive.ReadSingle(), archive.ReadSingle()),
                        Unknown2 = new FVector(archive.ReadSingle(), archive.ReadSingle(), archive.ReadSingle()),
                    };

                    ParsePlayer(archive, elim.EliminatedInfo);
                    ParsePlayer(archive, elim.EliminatorInfo);
                }
                else
                {
                    if (Major <= 4 && Minor < 2)
                    {
                        archive.SkipBytes(12);
                    }
                    else if (Major == 4 && Minor <= 2)
                    {
                        archive.SkipBytes(40);
                    }
                    else
                    {
                        archive.SkipBytes(45);
                    }

                    elim.EliminatedInfo = new PlayerEliminationInfo
                    {
                        Id = archive.ReadFString()
                    };

                    elim.EliminatorInfo = new PlayerEliminationInfo
                    {
                        Id = archive.ReadFString()
                    };
                }

                elim.GunType = archive.ReadByte();
                elim.Knocked = archive.ReadUInt32AsBoolean();
                elim.Timestamp = info.StartTime;

                return elim;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error while parsing PlayerElimination at timestamp {info.StartTime}");
                throw new PlayerEliminationException($"Error while parsing PlayerElimination at timestamp {info.StartTime}", ex);
            }
        }

        protected virtual void ParsePlayer(FArchive archive, PlayerEliminationInfo info)
        {
            info.PlayerType = archive.ReadByteAsEnum<PlayerTypes>();

            switch (info.PlayerType)
            {
                case PlayerTypes.Bot:

                    break;
                case PlayerTypes.NamedBot:
                    info.Id = archive.ReadFString();
                    break;
                case PlayerTypes.Player:
                    info.Id = archive.ReadGUID(archive.ReadByte());
                    break;
            }
        }

        protected override BinaryReader Decrypt(BinaryReader archive, int size)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
