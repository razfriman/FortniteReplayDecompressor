using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unreal.Core.Contracts;
using Unreal.Core.Exceptions;
using Unreal.Core.Extensions;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;
using Unreal.Encryption;

namespace Unreal.Core
{
    public abstract class ReplayReader<T> where T : Replay, new ()
    {
        private const int DefaultMaxChannelSize = 32767;

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/NetworkReplayStreaming/LocalFileNetworkReplayStreaming/Private/LocalFileNetworkReplayStreaming.cpp#L59
        /// </summary>
        public const uint FileMagic = 0x1CA2E27F;

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/811c1ce579564fa92ecc22d9b70cbe9c8a8e4b9a/Engine/Source/Runtime/Engine/Classes/Engine/DemoNetDriver.h#L107
        /// </summary>
        public const uint NetworkMagic = 0x2CF5A13D;

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/811c1ce579564fa92ecc22d9b70cbe9c8a8e4b9a/Engine/Source/Runtime/Engine/Classes/Engine/DemoNetDriver.h#L111
        /// </summary>
        public const uint MetadataMagic = 0x3D06B24E;

        protected ILogger _logger;
        protected T Replay { get; set; }
        protected ParseType ParseType;
        protected NetGuidCache GuidCache = new NetGuidCache();

        private int replayDataIndex = 0;
        private int checkpointIndex = 0;
        private int externalDataIndex = 0;
        private int packetIndex = 0;
        private int bunchIndex = 0;

        private int InPacketId;
        private DataBunch PartialBunch;
        private int?[] InReliable = new int?[DefaultMaxChannelSize];
        public UChannel[] Channels = new UChannel[DefaultMaxChannelSize];
        //private bool?[] ChannelActors = new bool?[DefaultMaxChannelSize];
        private uint?[] IgnoringChannels = new uint?[DefaultMaxChannelSize]; // channel index, actorguid
        private bool isReading = false;
        private NetFieldParser _netFieldParser;

        public int NullHandles { get; private set; }
        public int TotalErrors { get; private set; }
        public int TotalGroupsRead { get; private set; }
        public int TotalFailedBunches { get; private set; }
        public int TotalFailedReplicatorReceives { get; private set; }
        public int PropertyError { get; private set; }
        public int TotalMappedGUIDs { get; private set; }
        public int FailedToRead { get; private set; }
        private bool[] _tempBuffer = new bool[0x10000];

        protected ReplayReader(ILogger logger)
        {
            _logger = logger;
            _netFieldParser = new NetFieldParser(GetType().Assembly);
        }

        public virtual T ReadReplay(FArchive archive, ParseType parseType)
        {
            if (isReading)
            {
                throw new InvalidOperationException("Multithreaded reading currently isn't supported");
            }

            Replay = new T();

            ParseType = parseType;
            isReading = true;

            ReadReplayInfo(archive);
            ReadReplayChunks(archive);

            Cleanup();

            isReading = false;

            return Replay;
        }

        protected virtual void Cleanup()
        {
#if DEBUG
            StringBuilder builder = new StringBuilder();

            foreach(var exportGroupMap in GuidCache.NetFieldExportGroupMap)
            {
                builder.AppendLine($"Path: {exportGroupMap.Key}");

                foreach(var exportGroup in exportGroupMap.Value.NetFieldExports)
                {
                    if(exportGroup == null)
                    {
                        continue;
                    }

                    builder.AppendLine($"\t{exportGroup.Name} - {exportGroup.Type} - {exportGroup.Handle}");
                }
            }

            var s = builder.ToString();

            var n = String.Join("\n", GuidCache.NetGuidToPathName.Select(x => $"{x.Key} - {x.Value}"));

#endif

            InReliable = new int?[DefaultMaxChannelSize];
            Channels = new UChannel[DefaultMaxChannelSize];
            //ChannelActors = new bool?[DefaultMaxChannelSize];
            IgnoringChannels = new uint?[DefaultMaxChannelSize];

            GuidCache.ClearCache();
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/NetworkReplayStreaming/LocalFileNetworkReplayStreaming/Private/LocalFileNetworkReplayStreaming.cpp#L243
        /// </summary>
        /// <param name="archive"></param>
        protected virtual void ReadReplayChunks(FArchive archive)
        {
            while (!archive.AtEnd())
            {
                var chunkType = archive.ReadUInt32AsEnum<ReplayChunkType>();
                var chunkSize = archive.ReadInt32();
                var offset = archive.Position;

                //Console.WriteLine($"Chunk {chunkType}. Size: {chunkSize}. Offset: {offset}");

                if (chunkType == ReplayChunkType.Checkpoint)
                {
                    //Failing to read checkpoints properly
                    //ReadCheckpoint(archive);

                    archive.Seek(chunkSize, SeekOrigin.Current);
                }

                else if (chunkType == ReplayChunkType.Event)
                {
                    ReadEvent(archive);
                }
                else if (chunkType == ReplayChunkType.ReplayData)
                {
                    if (ParseType >= ParseType.Minimal)
                    {
                        ReadReplayData(archive);
                    }
                    else
                    {
                        archive.Seek(offset + chunkSize, SeekOrigin.Begin);
                    }
                }
                else if (chunkType == ReplayChunkType.Header)
                {
                    ReadReplayHeader(archive);
                }

                if (archive.Position != offset + chunkSize)
                {
                    _logger?.LogError($"Chunk ({chunkType}) at offset {offset} not correctly read...");
                    archive.Seek(offset + chunkSize, SeekOrigin.Begin);
                }
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L4892
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/NetworkReplayStreaming/LocalFileNetworkReplayStreaming/Private/LocalFileNetworkReplayStreaming.cpp#L282
        /// </summary>
        /// <param name="archive"></param>
        /// <returns></returns>
        protected virtual void ReadCheckpoint(FArchive archive)
        {
            var info = new CheckpointInfo
            {
                Id = archive.ReadFString(),
                Group = archive.ReadFString(),
                Metadata = archive.ReadFString(),
                StartTime = archive.ReadUInt32(),
                EndTime = archive.ReadUInt32(),
                SizeInBytes = archive.ReadInt32()
            };

            if (!archive.CanRead(info.SizeInBytes))
            {
                _logger?.LogError($"Can't read checkpoint data {info.Id}");

                return;
            }

            using var decrypted = Decrypt(archive, info.SizeInBytes);
            using var binaryArchive = Decompress(decrypted, (int)decrypted.BaseStream.Length);

            // SerializeDeletedStartupActors
            // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L1916

            if (binaryArchive.HasDeltaCheckpoints())
            {
                var checkPointSize = binaryArchive.ReadUInt32();
            }

            if (binaryArchive.HasLevelStreamingFixes())
            {
                var packetOffset = binaryArchive.ReadInt64();
            }

            if (binaryArchive.NetworkVersion >= NetworkVersionHistory.HISTORY_MULTIPLE_LEVELS)
            {
                var levelForCheckpoint = binaryArchive.ReadInt32();
            }

            if (binaryArchive.NetworkVersion >= NetworkVersionHistory.HISTORY_DELETED_STARTUP_ACTORS)
            {
                if (binaryArchive.HasDeltaCheckpoints())
                {
                    throw new NotImplementedException("Delta checkpoints not supported currently");
                }

                var deletedNetStartupActors = binaryArchive.ReadArray(binaryArchive.ReadFString);
            }

            // SerializeGuidCache
            // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L1591
            var count = binaryArchive.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var guid = binaryArchive.ReadIntPacked();

                NetGuidCacheObject cacheObject = new NetGuidCacheObject
                {
                    OuterGuid = new NetworkGUID
                    {
                        Value = binaryArchive.ReadIntPacked()
                    },
                    PathName = binaryArchive.ReadFString(),
                    NetworkChecksum = binaryArchive.ReadUInt32(),
                    Flags = binaryArchive.ReadByte()
                };

                // TODO DemoNetDriver 5319
                // GuidCache->ObjectLookup.Add(Guid, CacheObject);
            }

            if (binaryArchive.HasDeltaCheckpoints())
            {
                throw new NotImplementedException("Delta checkpoints not implemented");
            }
            else
            {
                // Clear all of our mappings, since we're starting over
                GuidCache.NetFieldExportGroupMap.Clear();
                GuidCache.NetFieldExportGroupIndexToGroup.Clear();

                // SerializeNetFieldExportGroupMap 
                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L1289

                var numNetFieldExportGroups = binaryArchive.ReadUInt32();

                for (var i = 0; i < numNetFieldExportGroups; i++)
                {
                    var group = ReadNetFieldExportGroupMap(binaryArchive);

                    // Add the export group to the map
                    //GuidCache.NetFieldExportGroupPathToIndex[group.PathName] = group.PathNameIndex;
                    GuidCache.NetFieldExportGroupIndexToGroup[group.PathNameIndex] = group;
                    GuidCache.AddToExportGroupMap(group.PathName, group);
                }
            }

            //Remove all actors
            foreach(var channel in Channels)
            {
                if(channel == null)
                {
                    continue;
                }

                channel.Actor = null;
            }

            // SerializeDemoFrameFromQueuedDemoPackets
            // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L1978
            var playbackPackets = ReadDemoFrameIntoPlaybackPackets(binaryArchive);
            foreach (var packet in playbackPackets)
            {
                if (packet.State == PacketState.Success)
                {
                    packetIndex++;

                    ReceivedRawPacket(packet);
                }
            }
            checkpointIndex++;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/NetworkReplayStreaming/LocalFileNetworkReplayStreaming/Private/LocalFileNetworkReplayStreaming.cpp#L363
        /// </summary>
        /// <param name="archive"></param>
        protected virtual void ReadEvent(FArchive archive)
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

            throw new UnknownEventException();
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/NetworkReplayStreaming/LocalFileNetworkReplayStreaming/Private/LocalFileNetworkReplayStreaming.cpp#L318
        /// </summary> 
        /// <param name="archive"></param>
        protected virtual void ReadReplayData(FArchive archive)
        {
            var info = new ReplayDataInfo();
            if (archive.ReplayVersion >= ReplayVersionHistory.StreamChunkTimes)
            {
                info.Start = archive.ReadUInt32();
                info.End = archive.ReadUInt32();
                info.Length = archive.ReadUInt32();
            }
            else
            {
                info.Length = archive.ReadUInt32();
            }

            int memorySizeInBytes = (int)info.Length;

            if(archive.ReplayVersion >= ReplayVersionHistory.Encryption)
            {
                memorySizeInBytes = archive.ReadInt32();
            }

            using var decryptedReader = Decrypt(archive, (int)info.Length);
            using var binaryArchive = Decompress(decryptedReader, memorySizeInBytes);

            while (!binaryArchive.AtEnd())
            {
                var playbackPackets = ReadDemoFrameIntoPlaybackPackets(binaryArchive);

                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L3338

                foreach (var packet in playbackPackets.Where(x => x.State == PacketState.Success))
                {
                    packetIndex++;
                    ReceivedRawPacket(packet);
                }
            }

            replayDataIndex++;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/811c1ce579564fa92ecc22d9b70cbe9c8a8e4b9a/Engine/Source/Runtime/Engine/Classes/Engine/DemoNetDriver.h#L191
        /// </summary>
        /// <param name="archive"></param>
        /// <returns>ReplayHeader</returns>
        protected virtual void ReadReplayHeader(FArchive archive)
        {
            var magic = archive.ReadUInt32();

            if (magic != NetworkMagic)
            {
                _logger?.LogError($"Header.Magic != NETWORK_DEMO_MAGIC. Header.Magic: {magic}, NETWORK_DEMO_MAGIC: {NetworkMagic}");
                throw new InvalidReplayException($"Header.Magic != NETWORK_DEMO_MAGIC. Header.Magic: {magic}, NETWORK_DEMO_MAGIC: {NetworkMagic}");
            }

            var header = new ReplayHeader
            {
                NetworkVersion = archive.ReadUInt32AsEnum<NetworkVersionHistory>()
            };

            if (header.NetworkVersion <= NetworkVersionHistory.HISTORY_EXTRA_VERSION)
            {
                _logger?.LogError($"Header.Version < MIN_NETWORK_DEMO_VERSION. Header.Version: {header.NetworkVersion}, MIN_NETWORK_DEMO_VERSION: {NetworkVersionHistory.HISTORY_EXTRA_VERSION}");
                throw new InvalidReplayException($"Header.Version < MIN_NETWORK_DEMO_VERSION. Header.Version: {header.NetworkVersion}, MIN_NETWORK_DEMO_VERSION: {NetworkVersionHistory.HISTORY_EXTRA_VERSION}");
            }

            header.NetworkChecksum = archive.ReadUInt32();
            header.EngineNetworkVersion = archive.ReadUInt32AsEnum<EngineNetworkVersionHistory>();
            header.GameNetworkProtocolVersion = archive.ReadUInt32();

            if (header.NetworkVersion >= NetworkVersionHistory.HISTORY_HEADER_GUID)
            {
                header.Guid = archive.ReadGUID();
            }

            if (header.NetworkVersion >= NetworkVersionHistory.HISTORY_SAVE_FULL_ENGINE_VERSION)
            {
                header.Major = archive.ReadUInt16();
                header.Minor = archive.ReadUInt16();
                header.Patch = archive.ReadUInt16();
                header.Changelist = archive.ReadUInt32();
                header.Branch = archive.ReadFString();

                archive.NetworkReplayVersion = new NetworkReplayVersion()
                {
                    Major = header.Major,
                    Minor = header.Minor,
                    Patch = header.Patch,
                    Changelist = header.Changelist,
                    Branch = header.Branch
                };
            }
            else
            {
                header.Changelist = archive.ReadUInt32();
            }

            if (header.NetworkVersion <= NetworkVersionHistory.HISTORY_MULTIPLE_LEVELS)
            {
                throw new NotImplementedException();
            }
            else
            {
                header.LevelNamesAndTimes = archive.ReadTupleArray(archive.ReadFString, archive.ReadUInt32);
            }

            if (header.NetworkVersion >= NetworkVersionHistory.HISTORY_HEADER_FLAGS)
            {
                header.Flags = archive.ReadUInt32AsEnum<ReplayHeaderFlags>();
                archive.ReplayHeaderFlags = header.Flags;
            }

            header.GameSpecificData = archive.ReadArray(archive.ReadFString);

            archive.EngineNetworkVersion = header.EngineNetworkVersion;
            archive.NetworkVersion = header.NetworkVersion;

            Replay.Header = header;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/NetworkReplayStreaming/LocalFileNetworkReplayStreaming/Private/LocalFileNetworkReplayStreaming.cpp#L183
        /// </summary>
        /// <param name="archive"></param>
        /// <returns>ReplayInfo</returns>
        protected virtual void ReadReplayInfo(FArchive archive)
        {
            var magicNumber = archive.ReadUInt32();

            if (magicNumber != FileMagic)
            {
                _logger?.LogError("Invalid replay file");
                throw new InvalidReplayException("Invalid replay file");
            }

            var fileVersion = archive.ReadUInt32AsEnum<ReplayVersionHistory>();
            archive.ReplayVersion = fileVersion;

            var info = new ReplayInfo()
            {
                FileVersion = fileVersion,
                LengthInMs = archive.ReadUInt32(),
                NetworkVersion = archive.ReadUInt32(),
                Changelist = archive.ReadUInt32(),
                FriendlyName = archive.ReadFString(),
                IsLive = archive.ReadUInt32AsBoolean()
            };

            if (fileVersion >= ReplayVersionHistory.RecordedTimestamp)
            {
                info.Timestamp = DateTime.FromBinary(archive.ReadInt64());
            }

            if (fileVersion >= ReplayVersionHistory.Compression)
            {
                info.IsCompressed = archive.ReadUInt32AsBoolean();
            }

            if(fileVersion >= ReplayVersionHistory.Encryption)
            {
                info.Encrypted = archive.ReadUInt32AsBoolean();

                info.EncryptionKey = archive.ReadArray(archive.ReadByte);
            }

            if (!info.IsLive && info.Encrypted && (info.EncryptionKey.Length == 0))
            {
                _logger?.LogError("ReadReplayInfo: Completed replay is marked encrypted but has no key!");
                throw new InvalidReplayException("Completed replay is marked encrypted but has no key!");
            }

            if (info.IsLive && info.Encrypted)
            {
                _logger?.LogError("ReadReplayInfo: Replay is marked encrypted and but not yet marked as completed!");
                throw new InvalidReplayException("Replay is marked encrypted and but not yet marked as completed!");
            }

            Replay.Info = info;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L3220
        /// </summary>
        protected virtual PlaybackPacket ReadPacket(FArchive archive)
        {
            var packet = new PlaybackPacket();

            var bufferSize = archive.ReadInt32();
            if (bufferSize == 0)
            {
                packet.State = PacketState.End;
                return packet;
            }
            else if (bufferSize > 2048)
            {
                //UE_LOG(LogDemo, Error, TEXT("UDemoNetDriver::ReadPacket: OutBufferSize > MAX_DEMO_READ_WRITE_BUFFER"));
                _logger?.LogError("UDemoNetDriver::ReadPacket: OutBufferSize > 2048");

                packet.State = PacketState.Error;

                return packet;
            }
            else if (bufferSize < 0)
            {
                //UE_LOG(LogDemo, Error, TEXT("UDemoNetDriver::ReadPacket: OutBufferSize > MAX_DEMO_READ_WRITE_BUFFER"));
                _logger?.LogError("UDemoNetDriver::ReadPacket: OutBufferSize < 0");

                packet.State = PacketState.Error;

                return packet;
            }

            packet.Data = archive.ReadBytes(bufferSize);
            packet.State = PacketState.Success;
            return packet;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L2106
        /// </summary>
        protected virtual void ReadExternalData(FArchive archive)
        {
            while (true)
            {
                var externalDataNumBits = archive.ReadIntPacked();
                if (externalDataNumBits == 0)
                {
                    return;
                }

                // Read net guid this payload belongs to
                var netGuid = archive.ReadIntPacked();

                var externalDataNumBytes = (int)(externalDataNumBits + 7) >> 3;
                //var externalData = archive.ReadBytes(externalDataNumBytes);
                archive.SkipBytes(externalDataNumBytes);

                // replayout setexternaldata
                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Public/Net/RepLayout.h#L122
                // FMemory::Memcpy(ExternalData.GetData(), Src, NumBytes);

                // this is a bitreader...
                //var bitReader = new BitReader(externalData);
                //bitReader.ReadBytes(3); // always 19 FB 01 ?
                //var size = bitReader.ReadUInt32();

                // FCharacterSample
                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/Components/CharacterMovementComponent.cpp#L7074
                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Classes/GameFramework/CharacterMovementComponent.h#L2656
                //var location = bitReader.ReadPackedVector(10, 24);
                //var velocity = bitReader.ReadPackedVector(10, 24);
                //var acceleration = bitReader.ReadPackedVector(10, 24);
                //var rotation = bitReader.ReadSerializeCompressed();
                //var remoteViewPitch = bitReader.ReadByte();
                //if (!bitReader.AtEnd())
                //{
                //    var time = bitReader.ReadSingle();
                //}

                externalDataIndex++;
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/CoreUObject/Private/UObject/CoreNet.cpp#L277
        /// </summary>
        protected virtual string StaticParseName(FArchive archive)
        {
            var isHardcoded = archive.ReadBoolean();
            if (isHardcoded)
            {
                uint nameIndex;
                if (Replay.Header.EngineNetworkVersion < EngineNetworkVersionHistory.HISTORY_CHANNEL_NAMES)
                {
                    nameIndex = archive.ReadUInt32();
                }
                else
                {
                    nameIndex = archive.ReadIntPacked();
                }
                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Public/UObject/UnrealNames.h#L31
                // hard coded names in "UnrealNames.inl"
                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Public/UObject/UnrealNames.inl

                // https://github.com/EpicGames/UnrealEngine/blob/375ba9730e72bf85b383c07a5e4a7ba98774bcb9/Engine/Source/Runtime/Core/Public/UObject/NameTypes.h#L599
                // https://github.com/EpicGames/UnrealEngine/blob/375ba9730e72bf85b383c07a5e4a7ba98774bcb9/Engine/Source/Runtime/Core/Private/UObject/UnrealNames.cpp#L283
                // TODO: Combine with Fortnite SDK dump
                return ((UnrealNames)nameIndex).ToString();
            }

            // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Public/UObject/UnrealNames.h#L17
            // MAX_NETWORKED_HARDCODED_NAME = 410

            // https://github.com/EpicGames/UnrealEngine/blob/375ba9730e72bf85b383c07a5e4a7ba98774bcb9/Engine/Source/Runtime/Core/Public/UObject/NameTypes.h#L34
            // NAME_SIZE = 1024

            // InName.GetComparisonIndex() <= MAX_NETWORKED_HARDCODED_NAME;
            // InName.GetPlainNameString();
            // InName.GetNumber();

            var inString = archive.ReadFString();
            var inNumber = archive.ReadInt32();
            return inString;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Classes/Engine/PackageMapClient.h#L64
        /// </summary>
        protected virtual NetFieldExport ReadNetFieldExport(FArchive archive)
        {
            var isExported = archive.ReadBoolean();

            if (isExported)
            {
                var fieldExport = new NetFieldExport()
                {
                    Handle = archive.ReadIntPacked(),
                    CompatibleChecksum = archive.ReadUInt32()
                };

                if (Replay.Header.EngineNetworkVersion < EngineNetworkVersionHistory.HISTORY_NETEXPORT_SERIALIZATION)
                {
                    fieldExport.Name = archive.ReadFString();
                    fieldExport.Type = archive.ReadFString();
                }
                else if (Replay.Header.EngineNetworkVersion < EngineNetworkVersionHistory.HISTORY_NETEXPORT_SERIALIZE_FIX)
                {
                    // FName
                    fieldExport.Name = archive.ReadFString();
                }
                else
                {
                    fieldExport.Name = StaticParseName(archive);
                }

                return fieldExport;
            }

            return null;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Classes/Engine/PackageMapClient.h#L133
        /// </summary>
        protected virtual NetFieldExportGroup ReadNetFieldExportGroupMap(FArchive archive)
        {
            var group = new NetFieldExportGroup()
            {
                PathName = archive.ReadFString(),
                PathNameIndex = archive.ReadIntPacked(),
                NetFieldExportsLength = archive.ReadIntPacked()
            };

            group.NetFieldExports = new NetFieldExport[group.NetFieldExportsLength];

            for (var i = 0; i < group.NetFieldExportsLength; i++)
            {
                var netFieldExport = ReadNetFieldExport(archive);

                if (netFieldExport != null)
                {
                    // TODO fix null fields
                    group.NetFieldExports[netFieldExport.Handle] = netFieldExport;
                }
            }

            return group;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L1348
        /// </summary>
        protected virtual void ReadExportData(FArchive archive)
        {
            ReadNetFieldExports(archive);
            ReadNetExportGuids(archive);
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L1579
        /// </summary>
        protected virtual void ReadNetExportGuids(FArchive archive)
        {
            var numGuids = archive.ReadIntPacked();
            // TODO bIgnoreReceivedExportGUIDs ?

            for (var i = 0; i < numGuids; i++)
            {
                // TODO seperate reader?
                var size = archive.ReadInt32();

                NetBitReader reader = new NetBitReader(archive.ReadBytes(size));

                InternalLoadObject(reader, true);
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L1571
        /// </summary>
        protected virtual void ReadNetFieldExports(FArchive archive)
        {
            var numLayoutCmdExports = archive.ReadIntPacked();
            for (var i = 0; i < numLayoutCmdExports; i++)
            {
                var pathNameIndex = archive.ReadIntPacked();
                var isExported = archive.ReadIntPacked() == 1;
                NetFieldExportGroup group;

                if (isExported)
                {
                    var pathName = archive.ReadFString();
                    var numExports = archive.ReadIntPacked();

                    if (!GuidCache.NetFieldExportGroupMap.TryGetValue(pathName, out group))
                    {
                        group = new NetFieldExportGroup
                        {
                            PathName = pathName,
                            PathNameIndex = pathNameIndex,
                            NetFieldExportsLength = numExports
                        };

                        // TODO: 0 is reserved !?
                        group.NetFieldExports = new NetFieldExport[numExports];

                        GuidCache.AddToExportGroupMap(pathName, group);
                    }

                    //GuidCache.NetFieldExportGroupPathToIndex[pathName] = pathNameIndex;
                    GuidCache.NetFieldExportGroupIndexToGroup[pathNameIndex] = group;
                }
                else
                {
                    GuidCache.NetFieldExportGroupIndexToGroup.TryGetValue(pathNameIndex, out group);
                }

                var netField = ReadNetFieldExport(archive);

                if (group != null)
                {
                    group.NetFieldExports[netField.Handle] = netField;
                }
                else
                {
                    _logger?.LogInformation("ReceiveNetFieldExports: Unable to find NetFieldExportGroup for export.");
                }
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L2848
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<PlaybackPacket> ReadDemoFrameIntoPlaybackPackets(FArchive archive)
        {
            var currentLevelIndex = 0;

            if (archive.NetworkVersion >= NetworkVersionHistory.HISTORY_MULTIPLE_LEVELS)
            {
                currentLevelIndex = archive.ReadInt32();
            }

            var timeSeconds = archive.ReadSingle();
            //_logger?.LogInformation($"ReadDemoFrameIntoPlaybackPackets at {timeSeconds}");

            if (archive.NetworkVersion >= NetworkVersionHistory.HISTORY_LEVEL_STREAMING_FIXES)
            {
                ReadExportData(archive);
            }

            if (archive.HasLevelStreamingFixes())
            {
                var numStreamingLevels = archive.ReadIntPacked();
                for (var i = 0; i < numStreamingLevels; i++)
                {
                    var levelName = archive.ReadFString();
                }
            }
            else
            {
                var numStreamingLevels = archive.ReadIntPacked();
                for (var i = 0; i < numStreamingLevels; i++)
                {
                    var packageName = archive.ReadFString();
                    var packageNameToLoad = archive.ReadFString();
                    // FTransform
                    //var levelTransform = reader.ReadFString();
                    // filter duplicates

                    throw new NotImplementedException("FTransform deserialize not implemented");
                }
            }

            if (archive.HasLevelStreamingFixes())
            {
                var externalOffset = archive.ReadUInt64();
            }

            ReadExternalData(archive);

            if (archive.HasGameSpecificFrameData())
            {
                var skipExternalOffset = archive.ReadUInt64();

                if (skipExternalOffset > 0)
                {
                    // ignore it for now
                    archive.SkipBytes((int)skipExternalOffset);
                }
            }

            var playbackPackets = new List<PlaybackPacket>();
            var toContinue = true;
            while (toContinue)
            {
                uint seenLevelIndex = 0;

                if (archive.HasLevelStreamingFixes())
                {
                    seenLevelIndex = archive.ReadIntPacked();
                }

                var packet = ReadPacket(archive);
                packet.TimeSeconds = timeSeconds;
                packet.LevelIndex = currentLevelIndex;
                packet.SeenLevelIndex = seenLevelIndex;

                playbackPackets.Add(packet);

                toContinue = packet.State switch
                {
                    PacketState.End => false,
                    PacketState.Error => false,
                    PacketState.Success => true,
                    _ => false
                };
            }

            return playbackPackets;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L1409
        /// </summary>
        protected virtual void ReceiveNetFieldExportsCompat(FBitArchive bitArchive)
        {
            var numLayoutCmdExports = bitArchive.ReadUInt32();
            for (var i = 0; i < numLayoutCmdExports; i++)
            {
                var pathNameIndex = bitArchive.ReadIntPacked();
                NetFieldExportGroup group;

                if (bitArchive.ReadBit())
                {
                    var pathName = bitArchive.ReadFString();
                    var numExports = bitArchive.ReadUInt32();

                    if (!GuidCache.NetFieldExportGroupMap.TryGetValue(pathName, out group))
                    {
                        group = new NetFieldExportGroup
                        {
                            PathName = pathName,
                            PathNameIndex = pathNameIndex,
                            NetFieldExportsLength = numExports
                        };

                        group.NetFieldExports = new NetFieldExport[numExports];

                        GuidCache.AddToExportGroupMap(pathName, group);
                    }

                    //GuidCache.NetFieldExportGroupPathToIndex.Add(pathName, pathNameIndex);
                    GuidCache.NetFieldExportGroupIndexToGroup.Add(pathNameIndex, group);
                }
                else
                {
                    group = GuidCache.NetFieldExportGroupIndexToGroup[pathNameIndex];
                }

                var netField = ReadNetFieldExport(bitArchive);

                if (group.IsValidIndex(netField.Handle))
                {
                    //netField.Incompatible = group.NetFieldExports[(int)netField.Handle].Incompatible;
                    group.NetFieldExports[(int)netField.Handle] = netField;
                }
                else
                {
                    // ReceiveNetFieldExports: Invalid NetFieldExport Handle
                    // InBunch.SetError();
                }
            }
        }

        /// <summary>
        /// Loads a UObject from an FArchive stream. Reads object path if there, and tries to load object if its not already loaded
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L804
        /// </summary>
        protected virtual NetworkGUID InternalLoadObject(FArchive archive, bool isExportingNetGUIDBunch, int internalLoadObjectRecursionCount = 0)
        {
            if (internalLoadObjectRecursionCount > 16)
            {
                _logger?.LogWarning("InternalLoadObject: Hit recursion limit.");

                return new NetworkGUID();
            }

            var netGuid = new NetworkGUID()
            {
                Value = archive.ReadIntPacked()
            };

            if (!netGuid.IsValid())
            {
                return netGuid;
            }

            ExportFlags flags = ExportFlags.None;

            if (netGuid.IsDefault() || isExportingNetGUIDBunch)
            {
                flags = archive.ReadByteAsEnum<ExportFlags>();
            }

            // outerguid
            if (flags.HasFlag(ExportFlags.bHasPath))
            {
                var outerGuid = InternalLoadObject(archive, true, internalLoadObjectRecursionCount + 1);

                var pathName = archive.ReadFString();

                if (flags.HasFlag(ExportFlags.bHasNetworkChecksum))
                {
                    var networkChecksum = archive.ReadUInt32();
                }

                if (isExportingNetGUIDBunch)
                {
                    GuidCache.NetGuidToPathName[netGuid.Value] = GuidCache.RemoveAllPathPrefixes(pathName);
                }

                return netGuid;
            }

            return netGuid;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L1203
        /// </summary>
        protected virtual void ReceiveNetGUIDBunch(FBitArchive bitArchive)
        {
            var bHasRepLayoutExport = bitArchive.ReadBit();

            if (bHasRepLayoutExport)
            {
                // We need to keep this around to ensure we don't break backwards compatability.
                ReceiveNetFieldExportsCompat(bitArchive);
                return;
            }

            var numGUIDsInBunch = bitArchive.ReadInt32();
            // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L1027
            const int MAX_GUID_COUNT = 2048;
            if (numGUIDsInBunch > MAX_GUID_COUNT)
            {
                _logger?.LogError($"UPackageMapClient::ReceiveNetGUIDBunch: NumGUIDsInBunch > MAX_GUID_COUNT({numGUIDsInBunch})");
                return;
            }

            var numGUIDsRead = 0;
            while (numGUIDsRead < numGUIDsInBunch)
            {
                InternalLoadObject(bitArchive, true);
                numGUIDsRead++;
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DataChannel.cpp#L384
        /// </summary>
        /// <param name="bitReader"></param>
        /// <param name="bunch"></param>
        protected virtual void ReceivedRawBunch(DataBunch bunch)
        {
            // bDeleted =

            ReceivedNextBunch(bunch);

            // if (bDeleted) return;
            // else { We shouldn't hit this path on 100% reliable connections }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DataChannel.cpp#L517
        /// </summary>
        /// <param name="bitReader"></param>
        /// <param name="bunch"></param>
        protected virtual void ReceivedNextBunch(DataBunch bunch)
        {
            // We received the next bunch. Basically at this point:
            // -We know this is in order if reliable
            // -We dont know if this is partial or not
            // If its not a partial bunch, of it completes a partial bunch, we can call ReceivedSequencedBunch to actually handle it

            // Note this bunch's retirement.
            if (bunch.bReliable)
            {
                // Reliables should be ordered properly at this point
                //check(Bunch.ChSequence == Connection->InReliable[Bunch.ChIndex] + 1);
                InReliable[bunch.ChIndex] = bunch.ChSequence;
            }

            // merge
            if (bunch.bPartial)
            {
                if (bunch.bPartialInitial)
                {
                    if (PartialBunch != null)
                    {
                        if (!PartialBunch.bPartialFinal)
                        {
                            if (PartialBunch.bReliable)
                            {
                                if (bunch.bReliable)
                                {
                                    _logger?.LogError("Reliable initial partial trying to destroy reliable initial partial");
                                    return;
                                }
                                _logger?.LogError("Unreliable initial partial trying to destroy unreliable initial partial");
                                return;

                            }
                            // Incomplete partial bunch. 
                        }
                        PartialBunch = null;
                    }

                    // InPartialBunch = new FInBunch(Bunch, false);
                    PartialBunch = new DataBunch(bunch);
                    var bitsLeft = bunch.Archive.GetBitsLeft();
                    if (!bunch.bHasPackageMapExports && bitsLeft > 0)
                    {
                        if (bitsLeft % 8 != 0)
                        {
                            _logger?.LogError($"Corrupt partial bunch. Initial partial bunches are expected to be byte-aligned. BitsLeft = {bitsLeft % 8}.");
                            return;
                        }

                        PartialBunch.Archive.AppendDataFromChecked(bunch.Archive.ReadBits(bitsLeft));
                    }
                    else
                    {
                        //_logger?.LogInformation("Received New partial bunch. It only contained NetGUIDs.");
                    }

                    return;
                }
                else
                {
                    // Merge in next partial bunch to InPartialBunch if:
                    // -We have a valid InPartialBunch
                    // -The current InPartialBunch wasn't already complete
                    // -ChSequence is next in partial sequence
                    // -Reliability flag matches
                    var bSequenceMatches = false;

                    if (PartialBunch != null)
                    {
                        var bReliableSequencesMatches = bunch.ChSequence == PartialBunch.ChSequence + 1;
                        var bUnreliableSequenceMatches = bReliableSequencesMatches || (bunch.ChSequence == PartialBunch.ChSequence);

                        // Unreliable partial bunches use the packet sequence, and since we can merge multiple bunches into a single packet,
                        // it's perfectly legal for the ChSequence to match in this case.
                        // Reliable partial bunches must be in consecutive order though
                        bSequenceMatches = PartialBunch.bReliable ? bReliableSequencesMatches : bUnreliableSequenceMatches;
                    }

                    // if (InPartialBunch && !InPartialBunch->bPartialFinal && bSequenceMatches && InPartialBunch->bReliable == Bunch.bReliable)
                    if (PartialBunch != null && !PartialBunch.bPartialFinal && bSequenceMatches && PartialBunch.bReliable == bunch.bReliable)
                    {
                        var bitsLeft = bunch.Archive.GetBitsLeft();
                        _logger?.LogDebug($"Merging Partial Bunch: {bitsLeft} Bytes");
                        if (!bunch.bHasPackageMapExports && bitsLeft > 0)
                        {
                            PartialBunch.Archive.AppendDataFromChecked(bunch.Archive.ReadBits(bitsLeft));
                            // InPartialBunch->AppendDataFromChecked( Bunch.GetDataPosChecked(), Bunch.GetBitsLeft() );
                        }

                        // Only the final partial bunch should ever be non byte aligned. This is enforced during partial bunch creation
                        // This is to ensure fast copies/appending of partial bunches. The final partial bunch may be non byte aligned.
                        if (!bunch.bHasPackageMapExports && !bunch.bPartialFinal && (bitsLeft % 8 != 0))
                        {
                            _logger?.LogError("Corrupt partial bunch. Non-final partial bunches are expected to be byte-aligned.");
                            return;
                        }

                        // Advance the sequence of the current partial bunch so we know what to expect next
                        PartialBunch.ChSequence = bunch.ChSequence;

                        if (bunch.bPartialFinal)
                        {
                            _logger?.LogDebug("Completed Partial Bunch.");

                            if (bunch.bHasPackageMapExports)
                            {
                                _logger?.LogError("Corrupt partial bunch. Final partial bunch has package map exports.");
                                return;
                            }

                            // HandleBunch = InPartialBunch;
                            PartialBunch.bPartialFinal = true;
                            PartialBunch.bClose = bunch.bClose;
                            PartialBunch.bDormant = bunch.bDormant;
                            PartialBunch.CloseReason = bunch.CloseReason;
                            PartialBunch.bIsReplicationPaused = bunch.bIsReplicationPaused;
                            PartialBunch.bHasMustBeMappedGUIDs = bunch.bHasMustBeMappedGUIDs;

                            ReceivedSequencedBunch(PartialBunch);
                            return;
                        }
                        return;
                    }
                    else
                    {
                        // Merge problem - delete InPartialBunch. This is mainly so that in the unlikely chance that ChSequence wraps around, we wont merge two completely separate partial bunches.
                        // We shouldn't hit this path on 100% reliable connections
                        _logger?.LogError("Merge problem:  We shouldn't hit this path on 100% reliable connections");
                        return;
                    }
                }
                // bunch size check...
            }

            // something with opening channels...

            // Receive it in sequence.
            ReceivedSequencedBunch(bunch);
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DataChannel.cpp#L348
        /// </summary>
        /// <param name="bitReader"></param>
        /// <param name="bunch"></param>
        protected virtual bool ReceivedSequencedBunch(DataBunch bunch)
        {
            // if ( !Closing ) {
            switch (bunch.ChName)
            {
                case "Control":
                    ReceivedControlBunch(bunch);
                    break;
                default:
                    ReceivedActorBunch(bunch);
                    break;
            };
            // }

            if (bunch.bClose)
            {
                // We have fully received the bunch, so process it.
                //ChannelActors[bunch.ChIndex] = false;
                Channels[bunch.ChIndex] = null;
                OnChannelClosed(bunch.ChIndex);

                return true;
            }

            return false;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DataChannel.cpp#L1346
        /// </summary>
        /// <param name="bitReader"></param>
        /// <param name="bunch"></param>
        protected virtual void ReceivedControlBunch(DataBunch bunch)
        {
            // control channel
            while (!bunch.Archive.AtEnd())
            {
                var messageType = bunch.Archive.ReadByte();
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DataChannel.cpp#L2298
        /// </summary>
        /// <param name="bitReader"></param>
        /// <param name="bunch"></param>
        protected virtual void ReceivedActorBunch(DataBunch bunch)
        {
            if (bunch.bHasMustBeMappedGUIDs)
            {
                var numMustBeMappedGUIDs = bunch.Archive.ReadUInt16();
                for (var i = 0; i < numMustBeMappedGUIDs; i++)
                {
                    var guid = bunch.Archive.ReadIntPacked();
                }
            }

            ProcessBunch(bunch);
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DataChannel.cpp#L2411
        /// </summary>
        /// <param name="bitReader"></param>
        /// <param name="bunch"></param>
        protected virtual void ProcessBunch(DataBunch bunch)
        {
            UChannel channel = Channels[bunch.ChIndex];

            if (channel?.IgnoreChannel == true)
            {
                return;
            }

            var actor = Channels[bunch.ChIndex].Actor != null;

            if (!actor)
            {
                if (!bunch.bOpen)
                {
                    _logger?.LogError("New actor channel received non-open packet.");
                    return;
                }

                #region SerializeNewActor https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L257

                var inActor = new Actor
                {
                    // Initialize client if first time through.

                    // SerializeNewActor
                    // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L257
                    ActorNetGUID = InternalLoadObject(bunch.Archive, false)
                };

                if (bunch.Archive.AtEnd() && inActor.ActorNetGUID.IsDynamic())
                {
                    return;
                }

                if (inActor.ActorNetGUID.IsDynamic())
                {
                    inActor.Archetype = InternalLoadObject(bunch.Archive, false);

                    // if (Ar.IsSaving() || (Connection && (Connection->EngineNetworkProtocolVersion >= EEngineNetworkVersionHistory::HISTORY_NEW_ACTOR_OVERRIDE_LEVEL)))
                    if (bunch.Archive.EngineNetworkVersion >= EngineNetworkVersionHistory.HISTORY_NEW_ACTOR_OVERRIDE_LEVEL)
                    {
                        inActor.Level = InternalLoadObject(bunch.Archive, false);
                    }

                    FVector ConditionallySerializeQuantizedVector(FVector defaultValue)
                    {
                        bool bWasSerialized = bunch.Archive.ReadBit();
                        if (bWasSerialized)
                        {
                            bool bShouldQuantize;
                            if (bunch.Archive.EngineNetworkVersion < EngineNetworkVersionHistory.HISTORY_OPTIONALLY_QUANTIZE_SPAWN_INFO)
                            {
                                bShouldQuantize = true;
                            }
                            else
                            {
                                bShouldQuantize = bunch.Archive.ReadBit();
                            }

                            if (bShouldQuantize)
                            {
                                return bunch.Archive.ReadPackedVector(10, 24);
                            }
                            else
                            {
                                return new FVector(bunch.Archive.ReadSingle(), bunch.Archive.ReadSingle(), bunch.Archive.ReadSingle());
                            }
                        }
                        else
                        {
                            return defaultValue;
                        }
                    }


                    inActor.Location = ConditionallySerializeQuantizedVector(new FVector(0, 0, 0));

                    // bSerializeRotation
                    if (bunch.Archive.ReadBit())
                    {
                        inActor.Rotation = bunch.Archive.ReadRotationShort();
                    }
                    else
                    {
                        inActor.Rotation = new FRotator(0, 0, 0);
                    }

                    inActor.Scale = ConditionallySerializeQuantizedVector(new FVector(1, 1, 1));
                    inActor.Velocity = ConditionallySerializeQuantizedVector(new FVector(0, 0, 0));
                }

                #endregion

                channel.Actor = inActor;

                OnChannelActorRead(channel.ChannelIndex, inActor);


                if(Channels[bunch.ChIndex].Actor.Archetype != null && 
                    GuidCache.NetGuidToPathName.TryGetValue(Channels[bunch.ChIndex].Actor.Archetype.Value, out string pathName))
                {
                    if (_netFieldParser.IsPlayerController(pathName))
                    {
                        byte netPlayerIndex = bunch.Archive.ReadByte();
                    }
                }

                //ChannelNetGuids[bunch.ChIndex] = inActor.ActorNetGUID.Value;
            }

            // RepFlags.bNetOwner = true; // ActorConnection == Connection is always true??

            //RepFlags.bIgnoreRPCs = Bunch.bIgnoreRPCs;
            //RepFlags.bSkipRoleSwap = bSkipRoleSwap;

            //  Read chunks of actor content
            while (!bunch.Archive.AtEnd())
            {
                var repObject = ReadContentBlockPayload(bunch, out var bObjectDeleted, out var bHasRepLayout, out var reader);

                if(bObjectDeleted)
                {
                    continue;
                }

                if (bunch.Archive.IsError)
                {
                    ++TotalFailedBunches;

                    _logger?.LogError($"UActorChannel::ReceivedBunch: ReadContentBlockPayload FAILED. Bunch Info: {bunch}");

                    break;
                }

                if (repObject == 0 || reader == null || reader.AtEnd())
                {
                    // Nothing else in this block, continue on (should have been a delete or create block)
                    continue;
                }

                //Channel's being ignored
                if(Channels[bunch.ChIndex].IgnoreChannel == true)
                {
                    continue;
                }

                // if ( !Replicator->ReceivedBunch( Reader, RepFlags, bHasRepLayout, bHasUnmapped ) )
                if (!ReceivedReplicatorBunch(bunch, reader, repObject, bHasRepLayout))
                {
                    continue;
                }
            }
            // PostReceivedBunch, not interesting?
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Private/DataReplication.cpp#L896
        /// </summary>
        /// <param name="archive"></param>
        protected virtual bool ReceivedReplicatorBunch(DataBunch bunch, FBitArchive archive, uint repObject, bool bHasRepLayout)
        {
            // outer is used to get path name
            // coreredirects.cpp ...
            NetFieldExportGroup netFieldExportGroup = GuidCache.GetNetFieldExportGroup(repObject);

            //Mainly props. If needed, add them in
            if (netFieldExportGroup == null)
            {
                //_logger?.LogWarning($"Failed group. {bunch.ChIndex}");

                return true;
            }

            // Handle replayout properties
            if (bHasRepLayout)
            {
                // if ENABLE_PROPERTY_CHECKSUMS
                //var doChecksum = archive.ReadBit();

                if (!ReceiveProperties(archive, netFieldExportGroup, bunch.ChIndex, out INetFieldExportGroup export))
                {
                    //Either failed to read properties or ignoring the channel
                    return false;
                }
            }

            if (archive.AtEnd())
            {
                return true;
            }

            NetFieldExportGroup classNetCache = GuidCache.GetNetFieldExportGroupForClassNetCache(netFieldExportGroup.PathName, bunch.Archive.EngineNetworkVersion >= EngineNetworkVersionHistory.HISTORY_CLASSNETCACHE_FULLNAME);

            if (classNetCache == null)
            {
                if (ParseType == ParseType.Debug)
                {
                    _logger?.LogDebug($"Couldn't find ClassNetCache for {netFieldExportGroup?.PathName}");
                }

                return false;
            }

            while (ReadFieldHeaderAndPayload(archive, classNetCache, out NetFieldExport fieldCache, out FBitArchive reader))
            {
                if (fieldCache == null)
                {
                    _logger?.LogInformation($"ReceivedBunch: FieldCache == nullptr: {classNetCache.PathName}");
                    continue;
                }

                if (fieldCache.Incompatible)
                {
                    // We've already warned about this property once, so no need to continue to do so
                    _logger?.LogInformation($"ReceivedBunch: FieldCache->bIncompatible == true: {fieldCache.Name}");
                    continue;
                }

                if (reader == null || reader.IsError)
                {
                    _logger?.LogInformation($"ReceivedBunch: reader == nullptr or IsError: {classNetCache.PathName}");
                    continue;
                }

                if (reader.AtEnd())
                {
                    continue;
                }

                //Find export group
                bool rpcGroupFound = _netFieldParser.TryGetNetFieldGroupRPC(classNetCache.PathName, fieldCache.Name, ParseType, out NetRPCFieldInfo netFieldInfo, out bool willParse);

                if (rpcGroupFound)
                {
                    if(!willParse)
                    {
                        return true;
                    }

                    bool isFunction = netFieldInfo.Attribute.IsFunction;
                    string pathName = netFieldInfo.Attribute.TypePathName;
                    bool customSerialization = netFieldInfo.IsCustomStructure;

                    NetFieldExportGroup exportGroup = GuidCache.GetNetFieldExportGroup(pathName);

                    if (isFunction)
                    {
                        if (exportGroup == null)
                        {
                            _logger?.LogError($"Failed to find export group for function property {fieldCache.Name} {classNetCache.PathName}. BunchIndex: {bunchIndex}, packetId: {bunch.PacketId}");

                            return false;
                        }

                        if (!ReceivedRPC(reader, exportGroup, bunch.ChIndex))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (customSerialization)
                        {
                            if (!ReceiveCustomProperty(reader, classNetCache, fieldCache, bunch.ChIndex))
                            {
                                _logger?.LogError($"Failed to parse custom property {classNetCache.PathName} {fieldCache.Name}");
                            }
                        }
                        else if (exportGroup != null)
                        {
                            if (!ReceiveCustomDeltaProperty(reader, classNetCache, fieldCache.Handle, bunch.ChIndex))
                            {
                                _logger?.LogError($"Failed to find custom delta property {fieldCache.Name}. BunchIndex: {bunchIndex}, packetId: {bunch.PacketId}");

                                return false;
                            }
                        }
                    }
                }
                else
                {
                    return true;
                }
            }

            return true;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Private/DataReplication.cpp#L1158
        /// see https://github.com/EpicGames/UnrealEngine/blob/8776a8e357afff792806b997fbbd8e715111a271/Engine/Source/Runtime/Engine/Private/RepLayout.cpp#L5801
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual bool ReceivedRPC(FBitArchive reader, NetFieldExportGroup netFieldExportGroup, uint channelIndex)
        {
            ReceiveProperties(reader, netFieldExportGroup, channelIndex, out INetFieldExportGroup export);

            if (reader.IsError)
            {
                _logger?.LogError("ReceivedRPC: ReceivePropertiesForRPC - Reader.IsError() == true");
                return false;
            }

            if (!reader.AtEnd())
            {
                _logger?.LogError("ReceivedRPC: ReceivePropertiesForRPC - Mismatch read.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/8776a8e357afff792806b997fbbd8e715111a271/Engine/Source/Runtime/Engine/Private/RepLayout.cpp#L3744
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual bool ReceiveCustomDeltaProperty(FBitArchive reader, NetFieldExportGroup netFieldExportGroup, uint handle, uint channelIndex)
        {
            bool bSupportsFastArrayDeltaStructSerialization = false;

            if (Replay.Header.EngineNetworkVersion >= EngineNetworkVersionHistory.HISTORY_FAST_ARRAY_DELTA_STRUCT)
            {
                // bSupportsFastArrayDeltaStructSerialization
                bSupportsFastArrayDeltaStructSerialization = reader.ReadBit();
            }

            //Need to figure out which properties require this
            //var staticArrayIndex = reader.ReadIntPacked();

            if (NetDeltaSerialize(reader, bSupportsFastArrayDeltaStructSerialization, netFieldExportGroup, handle, channelIndex))
            {
                // Successfully received it.
                return true;
            }

            return false;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/8776a8e357afff792806b997fbbd8e715111a271/Engine/Source/Runtime/Engine/Classes/Engine/NetSerialization.h#L1064
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual bool NetDeltaSerialize(FBitArchive reader, bool bSupportsFastArrayDeltaStructSerialization, NetFieldExportGroup netFieldExportGroup, uint handle, uint channelIndex)
        {
            if(!bSupportsFastArrayDeltaStructSerialization)
            {
                //No support for now
                return false;
            }

            string pathName = _netFieldParser.GetClassNetPropertyPathname(netFieldExportGroup.PathName, netFieldExportGroup.NetFieldExports[handle].Name, out bool readChecksumBit);

            NetFieldExportGroup propertyExportGroup = GuidCache.GetNetFieldExportGroup(pathName);

            bool readProperties = propertyExportGroup != null ? (_netFieldParser.WillReadType(propertyExportGroup.PathName, ParseType, out bool _) || ParseType == ParseType.Debug) : false;

            if (!readProperties)
            {
                //Return true to prevent any warnings about failed readings
                return true;
            }

            FFastArraySerializerHeader header = ReadDeltaHeader(reader);

            if (reader.IsError)
            {
                _logger?.LogError($"Failed to read DeltaSerialize header {netFieldExportGroup.PathName} {netFieldExportGroup.NetFieldExports[handle].Name}");

                return false;
            }

            for (int i = 0; i < header.NumDeleted; i++)
            {
                int elementIndex = reader.ReadInt32();

                if (propertyExportGroup != null)
                {
                    OnNetDeltaRead(new NetDeltaUpdate
                    {
                        Deleted = true,
                        ChannelIndex = channelIndex,
                        ElementIndex = elementIndex,
                        ExportGroup = netFieldExportGroup,
                        PropertyExport = propertyExportGroup,
                        Handle = handle,
                        Header = header
                    });
                }
            }

            for (int i = 0; i < header.NumChanged; i++)
            {
                int elementIndex = reader.ReadInt32();
                
                if (ReceiveProperties(reader, propertyExportGroup, channelIndex, out INetFieldExportGroup export, !readChecksumBit, true))
                {
                    OnNetDeltaRead(new NetDeltaUpdate
                    {
                        ChannelIndex = channelIndex,
                        ElementIndex = elementIndex,
                        Export = export,
                        ExportGroup = netFieldExportGroup,
                        PropertyExport = propertyExportGroup,
                        Handle = handle,
                        Header = header
                    });
                }
            }

            if (reader.IsError || !reader.AtEnd())
            {
                _logger?.LogError($"Failed to read DeltaSerialize {netFieldExportGroup.PathName} {netFieldExportGroup.NetFieldExports[handle].Name}");

                return false;
            }

            return true;
        }

        /// <summary>
        /// https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Classes/Engine/NetSerialization.h#L895
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private FFastArraySerializerHeader ReadDeltaHeader(FBitArchive reader)
        {
            FFastArraySerializerHeader header = new FFastArraySerializerHeader();

            header.ArrayReplicationKey = reader.ReadInt32();
            header.BaseReplicationKey = reader.ReadInt32();
            header.NumDeleted = reader.ReadInt32();
            header.NumChanged = reader.ReadInt32();

            return header;
        }

        private bool ReceiveCustomProperty(FBitArchive reader, NetFieldExportGroup classNetCache, NetFieldExport fieldCache, uint channelIndex)
        {
            if (_netFieldParser.TryCreateRPCPropertyType(classNetCache.PathName, fieldCache.Name, out IProperty customProperty))
            {
                NetBitReader netreader = new NetBitReader(reader.ReadBits(reader.GetBitsLeft()))
                {
                    EngineNetworkVersion = reader.EngineNetworkVersion,
                    NetworkVersion = reader.NetworkVersion
                };

                customProperty.Serialize(netreader);

                OnExportRead(channelIndex, customProperty as INetFieldExportGroup);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///  https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Private/RepLayout.cpp#L2895
        ///  https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Private/RepLayout.cpp#L2971
        ///  https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Private/RepLayout.cpp#L3022
        /// </summary>
        /// <param name="archive"></param>
        protected virtual bool ReceiveProperties(FBitArchive archive, NetFieldExportGroup group, uint channelIndex, out INetFieldExportGroup outExport, bool readChecksumBit = true, bool isDeltaRead = false)
        {
            outExport = null;

            ++TotalGroupsRead;
            
#if DEBUG
            Channels[channelIndex].Group.Add(group.PathName);
#endif

            if (!isDeltaRead) //Makes sure delta reads don't cause the channel to be ignored
            {
                if (ParseType != ParseType.Debug && !_netFieldParser.WillReadType(group.PathName, ParseType, out bool ignoreChannel))
                {
                    if (ignoreChannel)
                    {
                        Channels[channelIndex].IgnoreChannel = ignoreChannel;
                    }

                    return false;
                }
            }

            if (readChecksumBit)
            {
                var doChecksum = archive.ReadBit();
            }

            //Debug("types", $"\n{group.PathName}");

            INetFieldExportGroup exportGroup = _netFieldParser.CreateType(group.PathName);

            if(exportGroup == null || exportGroup is DebuggingExportGroup)
            {
                exportGroup = new DebuggingExportGroup
                {
                    ExportGroup = group
                };
            }

            outExport = exportGroup;
            outExport.ChannelActor = Channels[channelIndex].Actor;

            bool hasData = false;

            while (true)
            {
                var handle = archive.ReadIntPacked();

                if (handle == 0)
                {
                    break;
                }

                handle--;

                if (group.NetFieldExports.Length <= handle)
                {
                    //Need to figure these out
                    //_logger.LogError($"NetFieldExport length ({group.NetFieldExports.Length}) < handle ({handle}) {group.PathName}");

                    return false;
                }

                var export = group.NetFieldExports[handle];
                var numBits = archive.ReadIntPacked();

                if (numBits == 0)
                {
                    continue;
                }

                if (export == null)
                {
                    NullHandles++;

                    archive.SkipBits((int)numBits);

                    continue;
                }

                if (export.Incompatible)
                {
                    archive.SkipBits((int)numBits);

                    continue;
                }

                hasData = true;

                try
                {
                    bool[] buffer = _tempBuffer;

                    if (numBits > _tempBuffer.Length)
                    {
                        buffer = new bool[numBits];
                    }

                    archive.Read(buffer, (int)numBits);

                    var cmdReader = new NetBitReader(_tempBuffer, (int)numBits)
                    {
                        EngineNetworkVersion = Replay.Header.EngineNetworkVersion,
                        NetworkVersion = Replay.Header.NetworkVersion
                    };

                    _netFieldParser.ReadField(exportGroup, export, group, handle, cmdReader);

                    if (cmdReader.IsError)
                    {
                        ++PropertyError;

                        _logger?.LogWarning($"Property {export.Name} caused error when reading (bits: {numBits}, group: {group.PathName})");

#if DEBUG
                        cmdReader.Reset();

                        _netFieldParser.ReadField(exportGroup, export, group, handle, cmdReader);
#endif
                        continue;
                    }

                    if (archive.IsError)
                    {
                        _logger?.LogWarning($"Property {export.Name} caused error when reading (bits: {numBits}, group: {group.PathName})");
                        continue;
                    }

                    if (!cmdReader.AtEnd())
                    {
                        ++FailedToRead;

                        _logger?.LogInformation($"Property {export.Name} ({export.Handle}) in {group.PathName} didn't read proper number of bits: {cmdReader.GetBitsLeft()} out of {numBits}");

                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError($"NetFieldParser exception. Ex: {ex.Message}");
                }

            }

            //Delta structures are handled differently
            if (hasData && !isDeltaRead)
            {
                OnExportRead(channelIndex, exportGroup);
            }

            if (Channels[channelIndex].IgnoreChannel == null && ParseType != ParseType.Debug)
            {
                Channels[channelIndex].IgnoreChannel = !ContinueParsingChannel(exportGroup);
            }

            return true;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Private/DataChannel.cpp#L3579
        /// </summary>
        /// <param name="archive"></param>
        /// <returns></returns>
        protected virtual bool ReadFieldHeaderAndPayload(FBitArchive bunch, NetFieldExportGroup group, out NetFieldExport outField, out FBitArchive reader)
        {
            if(bunch.AtEnd())
            {
                reader = null;
                outField = null;
                return false; //We're done
            }

            // const int32 NetFieldExportHandle = Bunch.ReadInt(FMath::Max(NetFieldExportGroup->NetFieldExports.Num(), 2));
            var netFieldExportHandle = bunch.ReadSerializedInt(Math.Max((int)group.NetFieldExportsLength, 2));


            if (bunch.IsError)
            {
                reader = null;
                outField = null;
                _logger?.LogError("ReadFieldHeaderAndPayload: Error reading NetFieldExportHandle.");
                return false;
            }

            if(netFieldExportHandle >= group.NetFieldExportsLength)
            {
                reader = null;
                outField = null;

                _logger?.LogError("ReadFieldHeaderAndPayload: netFieldExportHandle > NetFieldExportsLength.");

                return false;
            }

            outField = group.NetFieldExports[(int)netFieldExportHandle];

            var numPayloadBits = bunch.ReadIntPacked();
            if (bunch.IsError)
            {
                reader = null;
                outField = null;
                _logger?.LogError("ReadFieldHeaderAndPayload: Error reading numbits.");
                return false;
            }

            reader = new BitReader(bunch.ReadBits(numPayloadBits));

            if (bunch.IsError)
            {
                _logger?.LogError($"ReadFieldHeaderAndPayload: Error reading payload. Bunch: {bunchIndex}, OutField: {netFieldExportHandle}");
                return false;
            }

            // More to read
            return true;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DataChannel.cpp#L3391
        /// </summary>
        protected virtual uint ReadContentBlockPayload(DataBunch bunch, out bool bObjectDeleted, out bool bOutHasRepLayout, out FBitArchive reader)
        {
            reader = null;
            var repObject = ReadContentBlockHeader(bunch, out bObjectDeleted, out bOutHasRepLayout);

            if(bObjectDeleted)
            {
                return repObject;
            }

            var numPayloadBits = bunch.Archive.ReadIntPacked();
            reader = new BitReader(bunch.Archive.ReadBits(numPayloadBits));

            return repObject;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DataChannel.cpp#L3175
        /// </summary>
        protected virtual uint ReadContentBlockHeader(DataBunch bunch, out bool bObjectDeleted, out bool bOutHasRepLayout)
        {
            //  bool& bObjectDeleted, bool& bOutHasRepLayout 
            //var bObjectDeleted = false;
            bObjectDeleted = false;
            bOutHasRepLayout = bunch.Archive.ReadBit();
            var bIsActor = bunch.Archive.ReadBit();
            if (bIsActor)
            {
                // If this is for the actor on the channel, we don't need to read anything else
                return Channels[bunch.ChIndex].Actor.Archetype?.Value ?? Channels[bunch.ChIndex].Actor.ActorNetGUID.Value;
            }

            // We need to handle a sub-object
            // Manually serialize the object so that we can get the NetGUID (in order to assign it if we spawn the object here)

            var netGuid = InternalLoadObject(bunch.Archive, false);

            var bStablyNamed = bunch.Archive.ReadBit();
            if (bStablyNamed)
            {
                // If this is a stably named sub-object, we shouldn't need to create it. Don't raise a bunch error though because this may happen while a level is streaming out.
                return netGuid.Value;
            }

            // Serialize the class in case we have to spawn it.
            var classNetGUID = InternalLoadObject(bunch.Archive, false);

            //Object deleted
            if (!classNetGUID.IsValid())
            {
                bObjectDeleted = true;
            }

            return classNetGUID.Value;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/NetConnection.cpp#L1007
        /// </summary>
        /// <param name="packet"></param>
        protected virtual void ReceivedRawPacket(PlaybackPacket packet)
        {
            if (Replay.Header.HasLevelStreamingFixes() && packet.SeenLevelIndex == 0)
            {
                return;
            }

            if (packet.Data.Length == 0)
            {
                _logger?.LogError($"Received zero-size packet");

                return;
            }

            var lastByte = packet.Data[^1];

            if (lastByte != 0)
            {
                var bitSize = (packet.Data.Length * 8) - 1;

                // Bit streaming, starts at the Least Significant Bit, and ends at the MSB.
                //while (!((lastByte & 0x80) >= 1))
                while (!((lastByte & 0x80) > 0))
                {
                    lastByte *= 2;
                    bitSize--;
                }

                var bitArchive = new BitReader(packet.Data, bitSize)
                {
                    EngineNetworkVersion = Replay.Header.EngineNetworkVersion,
                    NetworkVersion = Replay.Header.NetworkVersion,
                    ReplayHeaderFlags = Replay.Header.Flags
                };

                try
                {
                    if (bitArchive.GetBitsLeft() > 0)
                    {
                        ReceivedPacket(bitArchive);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, $"failed ReceivedPacket, index: {packetIndex}");
                }
            }
            else
            {
                _logger?.LogError("Malformed packet: Received packet with 0's in last byte of packet");
                throw new MalformedPacketException("Malformed packet: Received packet with 0's in last byte of packet");
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L3352
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/NetConnection.cpp#L1525
        /// </summary>
        /// <param name="bitReader"><see cref="Core.BitReader"/></param>
        /// <param name="packet"><see cref="PlaybackPacket"/></param>
        protected virtual void ReceivedPacket(FBitArchive bitReader)
        {
            // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L5101
            // InternalAck always true!

            // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/NetConnection.cpp#L1669
            const int OLD_MAX_ACTOR_CHANNELS = 10240;

            // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/NetConnection.cpp#1549
            InPacketId++;

            //var rejectedChannels = new Dictionary<uint, uint>();
            while (!bitReader.AtEnd())
            {
                // For demo backwards compatibility, old replays still have this bit
                if (bitReader.EngineNetworkVersion < EngineNetworkVersionHistory.HISTORY_ACKS_INCLUDED_IN_HEADER)
                {
                    var isAckDummy = bitReader.ReadBit();
                }

                // FInBunch
                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DataBunch.cpp#L18
                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Public/Net/DataBunch.h#L168
                var bunch = new DataBunch();

                var bControl = bitReader.ReadBit();
                bunch.PacketId = InPacketId;
                bunch.bOpen = bControl ? bitReader.ReadBit() : false;
                bunch.bClose = bControl ? bitReader.ReadBit() : false;

                if (bitReader.EngineNetworkVersion < EngineNetworkVersionHistory.HISTORY_CHANNEL_CLOSE_REASON)
                {
                    bunch.bDormant = bunch.bClose ? bitReader.ReadBit() : false;
                    bunch.CloseReason = bunch.bDormant ? ChannelCloseReason.Dormancy : ChannelCloseReason.Destroyed;
                }
                else
                {
                    bunch.CloseReason = bunch.bClose ? (ChannelCloseReason)bitReader.ReadSerializedInt((int)ChannelCloseReason.MAX) : ChannelCloseReason.Destroyed;
                    bunch.bDormant = bunch.CloseReason == ChannelCloseReason.Dormancy;
                }

                bunch.bIsReplicationPaused = bitReader.ReadBit();
                bunch.bReliable = bitReader.ReadBit();

                if (bitReader.EngineNetworkVersion < EngineNetworkVersionHistory.HISTORY_MAX_ACTOR_CHANNELS_CUSTOMIZATION)
                {
                    bunch.ChIndex = bitReader.ReadSerializedInt(OLD_MAX_ACTOR_CHANNELS);
                }
                else
                {
                    bunch.ChIndex = bitReader.ReadIntPacked();
                }

                bunch.bHasPackageMapExports = bitReader.ReadBit();
                bunch.bHasMustBeMappedGUIDs = bitReader.ReadBit();
                bunch.bPartial = bitReader.ReadBit();

                if (bunch.bReliable)
                {
                    // We can derive the sequence for 100% reliable connections
                    //Bunch.ChSequence = InReliable[Bunch.ChIndex] + 1;

                    if (InReliable[bunch.ChIndex] == null)
                    {
                        InReliable[bunch.ChIndex] = 0;
                    }

                    bunch.ChSequence = (InReliable[bunch.ChIndex] + 1).Value;
                }
                else if (bunch.bPartial)
                {
                    // If this is an unreliable partial bunch, we simply use packet sequence since we already have it
                    bunch.ChSequence = InPacketId;
                }
                else
                {
                    bunch.ChSequence = 0;
                }

                bunch.bPartialInitial = bunch.bPartial ? bitReader.ReadBit() : false;
                bunch.bPartialFinal = bunch.bPartial ? bitReader.ReadBit() : false;

                var chType = ChannelType.None;
                var chName = "";

                if (bitReader.EngineNetworkVersion < EngineNetworkVersionHistory.HISTORY_CHANNEL_NAMES)
                {
                    chType = (bunch.bReliable || bunch.bOpen) ? (ChannelType)bitReader.ReadSerializedInt((int)ChannelType.MAX) : ChannelType.None;

                    if (chType == ChannelType.Control)
                    {
                        chName = ChannelName.Control.ToString();
                    }
                    else if (chType == ChannelType.Voice)
                    {
                        chName = ChannelName.Voice.ToString();
                    }
                    else if (chType == ChannelType.Actor)
                    {
                        chName = ChannelName.Actor.ToString();
                    }
                }
                else
                {
                    if (bunch.bReliable || bunch.bOpen)
                    {
                        chName = StaticParseName(bitReader);

                        if (bitReader.IsError)
                        {
                            _logger?.LogError("Channel name serialization failed.");

                            return;
                        }

                        if (chName.Equals(ChannelName.Control.ToString()))
                        {
                            chType = ChannelType.Control;
                        }
                        else if (chName.Equals(ChannelName.Voice.ToString()))
                        {
                            chType = ChannelType.Voice;
                        }
                        else if (chName.Equals(ChannelName.Actor.ToString()))
                        {
                            chType = ChannelType.Actor;
                        }
                    }
                }

                bunch.ChType = chType;
                bunch.ChName = chName;

                var channel = Channels[bunch.ChIndex] != null;

                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L83
                var maxPacket = 1024 * 2;
                var bunchDataBits = bitReader.ReadSerializedInt(maxPacket * 8);

                bunch.Archive = new BitReader(bitReader.ReadBits(bunchDataBits))
                {
                    EngineNetworkVersion = bitReader.EngineNetworkVersion,
                    NetworkVersion = bitReader.NetworkVersion,
                    ReplayHeaderFlags = bitReader.ReplayHeaderFlags
                };

                bunchIndex++;

                if (bunch.bHasPackageMapExports)
                {
                    ReceiveNetGUIDBunch(bunch.Archive);
                }

                // We're on a 100% reliable connection and we are rolling back some data.
                // In that case, we can generally ignore these bunches.
                // if (InternalAck && Channel && bIgnoreAlreadyOpenedChannels)
                // bIgnoreAlreadyOpenedChannels always true?  https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L4393

                /*
                if (channel && (bunch.ChIndex != 0 || bunch.ChType != ChannelType.Control))
                {
                    if (!Channels.TryGetValue(0, out var controlChannel))
                    {
                        _logger?.LogWarning($"UNetConnection::ReceivedPacket: Received non-control bunch before control channel was created. ChIndex: {bunch.ChIndex}, ChName: {bunch.ChName}");

                        Console.WriteLine($"UNetConnection::ReceivedPacket: Received non-control bunch before control channel was created. ChIndex: {bunch.ChIndex}, ChName: {bunch.ChName}");

                        return;
                    }
                }
                */

                if (channel && false)
                {
                    var bNewlyOpenedActorChannel = bunch.bOpen && (bunch.ChName == ChannelName.Actor.ToString()) && (!bunch.bPartial || bunch.bPartialInitial);

                    if (bNewlyOpenedActorChannel)
                    {
                        if (bunch.bHasMustBeMappedGUIDs)
                        {
                            var numMustBeMappedGUIDs = bunch.Archive.ReadUInt16();
                            for (var i = 0; i < numMustBeMappedGUIDs; i++)
                            {
                                // FNetworkGUID NetGUID
                                var guid = bunch.Archive.ReadIntPacked();
                            }
                        }

                        //FNetworkGUID ActorGUID;
                        var actorGuid = bunch.Archive.ReadIntPacked();
                        IgnoringChannels[bunch.ChIndex] = actorGuid;
                    }

                    if (IgnoringChannels[bunch.ChIndex] != null)
                    {
                        if (bunch.bClose && (!bunch.bPartial || bunch.bPartialFinal))
                        {
                            //FNetworkGUID ActorGUID = IgnoringChannels.FindAndRemoveChecked(Bunch.ChIndex);
                            IgnoringChannels[bunch.ChIndex] = null;
                        }

                        continue;
                    }
                }

                // Ignore if reliable packet has already been processed.
                if (bunch.bReliable && bunch.ChSequence <= InReliable[bunch.ChIndex])
                {
                    continue;
                }

                // If opening the channel with an unreliable packet, check that it is "bNetTemporary", otherwise discard it
                if (!channel && !bunch.bReliable)
                {
                    if (!(bunch.bOpen && (bunch.bClose || bunch.bPartial)))
                    {
                        continue;
                    }
                }

                // Create channel if necessary
                if (!channel)
                {
                    var newChannel = new UChannel()
                    {
                        ChannelName = bunch.ChName,
                        ChannelType = bunch.ChType,
                        ChannelIndex = bunch.ChIndex,
                    };

                    Channels[bunch.ChIndex] = newChannel;
                }

                try
                {
                    ReceivedRawBunch(bunch);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, $"failed ReceivedRawBunch, index: {bunchIndex}");
                }
            }

            if (!bitReader.AtEnd())
            {
                _logger?.LogWarning("Packet not fully read...");
            }

            // termination bit?
            // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/NetConnection.cpp#L1170
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Private/Serialization/CompressedChunkInfo.cpp#L9
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Plugins/Runtime/PacketHandlers/CompressionComponents/Oodle/Source/OodleHandlerComponent/Private/OodleArchives.cpp#L21
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private Core.BinaryReader Decompress(FArchive archive, int size)
        {
            if (!Replay.Info.IsCompressed)
            {
                var uncompressed = new Core.BinaryReader(new MemoryStream(archive.ReadBytes(size)))
                {
                    EngineNetworkVersion = Replay.Header.EngineNetworkVersion,
                    NetworkVersion = Replay.Header.NetworkVersion,
                    ReplayHeaderFlags = Replay.Header.Flags,
                    ReplayVersion = Replay.Info.FileVersion
                };

                return uncompressed;
            }

            //File.WriteAllBytes("test.dat", archive.ReadBytes(size));

            var decompressedSize = archive.ReadInt32();
            var compressedSize = archive.ReadInt32();
            var compressedBuffer = archive.ReadBytes(compressedSize);
            var output = Oodle.DecompressReplayData(compressedBuffer, decompressedSize);
            var decompressed = new BinaryReader(new MemoryStream(output))
            {
                EngineNetworkVersion = Replay.Header.EngineNetworkVersion,
                NetworkVersion = Replay.Header.NetworkVersion,
                ReplayHeaderFlags = Replay.Header.Flags,
                ReplayVersion = Replay.Info.FileVersion
            };

            //_logger?.LogInformation($"Decompressed archive from {compressedSize} to {decompressedSize}.");
            return decompressed;
        }

        protected abstract void OnExportRead(uint channel, INetFieldExportGroup exportGroup);
        protected abstract void OnNetDeltaRead(NetDeltaUpdate deltaUpdate);
        protected abstract bool ContinueParsingChannel(INetFieldExportGroup exportGroup);
        protected abstract void OnChannelActorRead(uint channel, Actor actor);
        protected abstract void OnChannelClosed(uint channel); //Allows reuse of channel
        protected abstract BinaryReader Decrypt(FArchive archive, int size);
    }
}
