using FortniteReplayReader.Exceptions;
using FortniteReplayReader.Extensions;
using FortniteReplayReader.Models;
using FortniteReplayReader.Models.NetFieldExports;
using FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Functions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Unreal.Core;
using Unreal.Core.Contracts;
using Unreal.Core.Exceptions;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader
{
    public class ReplayReader : ReplayReader<FortniteReplay>
    {
        public int TotalPropertiesRead { get; private set; }

        public ReplayReader(ILogger logger = null)
        {
            Replay = new FortniteReplay();
            _logger = logger;
        }

        public FortniteReplay ReadReplay(string fileName, ParseType parseType = ParseType.Minimal)
        {
            using var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var archive = new Unreal.Core.BinaryReader(stream);
            return ReadReplay(stream, parseType);
        }

        public FortniteReplay ReadReplay(Stream stream, ParseType parseType = ParseType.Minimal)
        {
            using var archive = new Unreal.Core.BinaryReader(stream);
            var replay = ReadReplay(archive, parseType);

            return replay;
        }

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

        protected override void OnChannelActorRead(uint channel, Actor actor)
        {
            Replay.GameInformation.AddActor(channel, actor);
        }

        protected override void OnNetDeltaRead(NetDeltaUpdate deltaUpdate)
        {

        }

        protected override void OnExportRead(uint channel, INetFieldExportGroup exportGroup)
        {
            ++TotalPropertiesRead;
#if DEBUG
            if(Replay.GameInformation.Channels == null)
            {
                Replay.GameInformation.Channels = Channels;
            }
#endif
            //Used for weapon data and possibly other things
            if (Replay.GameInformation.NetGUIDToPathName == null)
            {
                Replay.GameInformation.NetGUIDToPathName = GuidCache.NetGuidToPathName;
            }

            Actor actor = Channels[channel].Actor;

            switch (exportGroup)
            {
                case SupplyDropLlamaC llama:
                    Replay.GameInformation.UpdateLlama(channel, llama);
                    break;
                case SupplyDropC supplyDrop:
                    Replay.GameInformation.UpdateSupplyDrop(channel, supplyDrop);
                    break;
                case GameStateC gameState:
                    Replay.GameInformation.UpdateGameState(gameState);
                    break;
                case FortPlayerState playerState:
                    Replay.GameInformation.UpdatePlayerState(channel, playerState, actor, GuidCache.NetworkGameplayTagNodeIndex);
                    break;
                case PlayerPawnC playerPawn:
                    if (ParseType >= ParseType.Normal)
                    {
                        Replay.GameInformation.UpdatePlayerPawn(channel, playerPawn);
                    }
                    break;
                case FortPickup fortPickup:
                    if(ParseType >= ParseType.Normal)
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
                    gameplayCue.GameplayCueTag.UpdateTagName(GuidCache.NetworkGameplayTagNodeIndex);
                    break;
                case Explosion explosion:
                    break;
                case FortPoiManager poiManager:
                    Replay.GameInformation.UpdatePoiManager(poiManager, GuidCache.NetworkGameplayTagNodeIndex);
                    break;
                case FortInventory inventory:
                    Replay.GameInformation.UpdateFortInventory(channel, inventory);
                    break;
                case DebuggingExportGroup debuggingObject: //Only occurs in debug mode
                    break;
            }
        }

        protected override bool ContinueParsingChannel(INetFieldExportGroup exportGroup)
        {
            switch (exportGroup)
            {
                //Always fully parse these
                case SupplyDropLlamaC _:
                case SupplyDropC _:
                case SafeZoneIndicatorC _:
                case FortPoiManager _:
                case GameStateC _:
                    return true;
            }

            switch (ParseType)
            {
                case ParseType.Minimal:
                    return false;
                default:
                    return true;
            }
        }

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
                    archive.SkipBytes(85);
                    elim.Eliminated = ParsePlayer(archive);
                    elim.Eliminator = ParsePlayer(archive);
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
                    elim.Eliminated = archive.ReadFString();
                    elim.Eliminator = archive.ReadFString();
                }

                elim.GunType = archive.ReadByte();
                elim.Knocked = archive.ReadUInt32AsBoolean();
                elim.Time = info?.StartTime.MillisecondsToTimeStamp();
                return elim;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error while parsing PlayerElimination at timestamp {info.StartTime}");
                throw new PlayerEliminationException($"Error while parsing PlayerElimination at timestamp {info.StartTime}", ex);
            }
        }

        protected virtual string ParsePlayer(FArchive archive)
        {
            // TODO player type enum
            var botIndicator = archive.ReadByte();
            if (botIndicator == 0x03)
            {
                return "Bot";
            } else if (botIndicator == 0x10)
            {
                return archive.ReadFString();
            }

            // 0x11
            var size = archive.ReadByte();
            return archive.ReadGUID(size);
        }
    }
}
