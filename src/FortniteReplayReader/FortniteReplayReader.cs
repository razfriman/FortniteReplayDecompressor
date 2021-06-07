using FortniteReplayReader.Exceptions;
using FortniteReplayReader.Extensions;
using FortniteReplayReader.Models;
using FortniteReplayReader.Models.Enums;
using FortniteReplayReader.Models.NetFieldExports;
using FortniteReplayReader.Models.NetFieldExports.Builds;
using FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Custom;
using FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Functions;
using FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Structures;
using FortniteReplayReader.Models.NetFieldExports.Items.Containers;
using FortniteReplayReader.Models.NetFieldExports.Items.Weapons;
using FortniteReplayReader.Models.NetFieldExports.Sets;
using FortniteReplayReader.Models.NetFieldExports.Vehicles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Unreal.Core;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Exceptions;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader
{
    public class ReplayReader : ReplayReader<FortniteReplay>
    {
        public int TotalPropertiesRead { get; private set; }

        private FortniteReplaySettings _fortniteSettings = new FortniteReplaySettings();

        public ReplayReader(ILogger logger = null, FortniteReplaySettings settings = null) : base(logger)
        {
            _fortniteSettings = settings ?? new FortniteReplaySettings();
        }

        public FortniteReplay ReadReplay(string fileName, ParseType parseType = ParseType.Minimal)
        {
            using var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); 
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
            Replay.GameInformation.HandleDeltaNetRead(deltaUpdate);
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

            if (Replay.GameInformation.Settings == null)
            {
                Replay.GameInformation.Settings = _fortniteSettings;
            }

            switch (exportGroup)
            {
                case AircraftC Test:
                    break;
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
                case GameStateC gameState:
                    Replay.GameInformation.UpdateGameState(gameState);
                    break;
                case FortPlayerState playerState:
                    Replay.GameInformation.UpdatePlayerState(channel, playerState, GuidCache.NetworkGameplayTagNodeIndex);
                    break;
                case PlayerPawnC playerPawn:
                    if (ParseType >= ParseType.Normal)
                    {
                        Replay.GameInformation.UpdatePlayerPawn(channel, playerPawn);
                    }
                    break;
                case FortPickup fortPickup:
                    if (ParseType >= ParseType.Normal)
                    {
                        if (!_fortniteSettings.IgnoreFloorLoot)
                        {
                            Replay.GameInformation.UpdateFortPickup(channel, fortPickup);
                        }
                    }
                    break;
                case SafeZoneIndicatorC safeZoneIndicator:
                    Replay.GameInformation.UpdateSafeZone(safeZoneIndicator);
                    break;
                case BatchedDamage batchedDamage:
                    if (!_fortniteSettings.IgnoreShots)
                    {
                        Replay.GameInformation.UpdateBatchedDamage(channel, batchedDamage);
                    }

                    break;
                case GameplayCue gameplayCue:
                    gameplayCue.GameplayCueTag.UpdateTagName(GuidCache.NetworkGameplayTagNodeIndex);

                    Replay.GameInformation.HandleGameplayCue(channel, gameplayCue);
                    break;
                case BaseWeapon weapon:
                    Replay.GameInformation.HandleWeapon(channel, weapon);
                    break;
                case FortPoiManager poiManager:
                    Replay.GameInformation.UpdatePoiManager(poiManager, GuidCache.NetworkGameplayTagNodeIndex);
                    break;
                case FortInventory inventory:
                    if (!_fortniteSettings.IgnoreInventory)
                    {
                        Replay.GameInformation.UpdateFortInventory(channel, inventory);
                    }
                    break;
                case BaseProp prop:

                    if (!_fortniteSettings.IgnoreContainers)
                    {
                        Replay.GameInformation.UpdateContainer(channel, prop);
                    }
                    break;
                case HealthSet healthSet:
                    if (!_fortniteSettings.IgnoreHealth)
                    {
                        Replay.GameInformation.UpdateHealth(channel, healthSet, GuidCache);
                    }
                    break;
                case BaseVehicle vehicle:
                    Replay.GameInformation.UpdateVehicle(channel, vehicle);
                    break;
                case MiniGameCreated minigame: //Multiple rounds
                    Replay.GameInformation.MiniGameUpdate(channel, minigame, GuidCache.NetworkGameplayTagNodeIndex);
                    break;
                case DebuggingExportGroup debuggingObject:
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

        protected override void OnChannelClosed(uint channel)
        {
            Replay.GameInformation.ChannelClosed(channel);
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

            using var decryptedReader = Decrypt(archive, info.SizeInBytes);

            // Every event seems to start with some unknown int
            if (info.Group == ReplayEventTypes.PLAYER_ELIMINATION)
            {
                var elimination = ParseElimination(decryptedReader, info);
                Replay.Eliminations.Add(elimination);
                return;
            }
            else if (info.Metadata == ReplayEventTypes.MATCH_STATS)
            {
                Replay.Stats = ParseMatchStats(decryptedReader, info);
                return;
            }
            else if (info.Metadata == ReplayEventTypes.TEAM_STATS)
            {
                Replay.TeamStats = ParseTeamStats(decryptedReader, info);
                return;
            }
            else if (info.Metadata == ReplayEventTypes.ENCRYPTION_KEY)
            {
                Replay.GameInformation.PlayerStateEncryptionKey = ParseEncryptionKeyEvent(decryptedReader, info);
                return;
            }
            else if (info.Metadata == ReplayEventTypes.CHARACTER_SAMPLE)
            {
                ParseCharacterSample(decryptedReader, info);
                return;
            }
            else if (info.Group == ReplayEventTypes.ZONE_UPDATE)
            {
                ParseZoneUpdateEvent(decryptedReader, info);
                return;
            }
            else if (info.Group == ReplayEventTypes.BATTLE_BUS)
            {
                ParseBattleBusFlightEvent(decryptedReader, info);
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

        protected override Unreal.Core.BinaryReader Decrypt(FArchive archive, int size)
        {
            if(!this.Replay.Info.Encrypted)
            {
                var decryptedReader = new Unreal.Core.BinaryReader(new MemoryStream(archive.ReadBytes(size)))
                {
                    EngineNetworkVersion = Replay.Header.EngineNetworkVersion,
                    NetworkVersion = Replay.Header.NetworkVersion,
                    ReplayHeaderFlags = Replay.Header.Flags,
                    ReplayVersion = Replay.Info.FileVersion
                };

                return decryptedReader;
            }

            var encryptedBytes = archive.ReadBytes(size);
            var key = this.Replay.Info.EncryptionKey;

            using RijndaelManaged rDel = new RijndaelManaged
            {
                KeySize = (key.Length * 8),
                Key = key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            using ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] decryptedArray = cTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            var decrypted = new Unreal.Core.BinaryReader(new MemoryStream(decryptedArray))
            {
                EngineNetworkVersion = Replay.Header.EngineNetworkVersion,
                NetworkVersion = Replay.Header.NetworkVersion,
                ReplayHeaderFlags = Replay.Header.Flags,
                ReplayVersion = Replay.Info.FileVersion
            };

            return decrypted;
        }

        public void SetParseType(ParsingGroup group, ParseType type)
        {
            switch (group)
            {
                case ParsingGroup.PlayerPawn:
                    SetParseType(typeof(PlayerPawnC), type); //Normal player locations
                    SetParseType(GetInheritedClasses(typeof(PlayerPawnC)), type); //Bot locations
                    SetParseType(GetInheritedClasses(typeof(BaseVehicle)), type); //Vehicle locations (required for players in them)
                    break;
            }

            List<Type> GetInheritedClasses(Type type)
            {
                List<Type> inheritedTypes = new List<Type>();

                IEnumerable<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());

                foreach(Type checkType in allTypes)
                {
                    if (checkType.IsSubclassOf(type))
                    {
                        NetFieldExportGroupAttribute netFieldExport = (NetFieldExportGroupAttribute)checkType.GetCustomAttributes(typeof(NetFieldExportGroupAttribute), true).FirstOrDefault();

                        if (netFieldExport != null)
                        {
                            inheritedTypes.Add(checkType);
                        }
                    }
                }

                return inheritedTypes;
            }
        }
    }
}
