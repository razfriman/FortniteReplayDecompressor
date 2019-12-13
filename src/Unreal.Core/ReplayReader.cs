using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public abstract class ReplayReader<T> where T : Replay
    {
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

        private int replayDataIndex = 0;
        private int checkpointIndex = 0;
        private int packetIndex = 0;
        private int externalDataIndex = 0;
        private int bunchIndex = 0;

        private int InPacketId;
        private DataBunch PartialBunch;
        // const int32 UNetConnection::DEFAULT_MAX_CHANNEL_SIZE = 32767; netconnection.cpp 84
        private Dictionary<uint, int> InReliable = new Dictionary<uint, int>(); // TODO: array in unreal
        public Dictionary<uint, UChannel> Channels = new Dictionary<uint, UChannel>();
        private Dictionary<uint, uint> ChannelNetGuids = new Dictionary<uint, uint>();
        private Dictionary<uint, bool> ChannelActors = new Dictionary<uint, bool>();

        public NetGuidCache GuidCache = new NetGuidCache();
        public int NullHandles { get; private set; }
        public int TotalErrors { get; private set; }
        public int TotalGroupsRead { get; private set; }
        public int TotalFailedBunches { get; private set; }
        public int TotalFailedReplicatorReceives { get; private set; }
        public int PropertyError { get; private set; }
        public int TotalMappedGUIDs { get; private set; }
        public int FailedToRead { get; private set; }

        public Dictionary<uint, List<INetFieldExportGroup>> ExportGroups { get; private set; } = new Dictionary<uint, List<INetFieldExportGroup>>();

        //private List<string> UnknownFields = new List<string>();

        /// <summary>
        /// Tracks channels that we should ignore when handling special demo data.
        /// </summary>
        private Dictionary<uint, uint> IgnoringChannels = new Dictionary<uint, uint>(); // channel index, actorguid
        private HashSet<uint> RejectedChans = new HashSet<uint>();

        private Dictionary<string, StreamWriter> _streamWriters = new Dictionary<string, StreamWriter>();

        public virtual T ReadReplay(FArchive archive)
        {
            Directory.CreateDirectory("debugFiles");

            ReadReplayInfo(archive);
            ReadReplayChunks(archive);

            Cleanup();

            return Replay;
        }

        private void Cleanup()
        {
            foreach (StreamWriter writer in _streamWriters.Values)
            {
                using (writer)
                {
                    writer.Flush();
                }
            }

            _streamWriters.Clear();

            InReliable.Clear();
            Channels.Clear();
            ChannelActors.Clear();
            GuidCache.NetFieldExportGroupIndexToGroup.Clear();
            GuidCache.NetFieldExportGroupMap.Clear();
            GuidCache.NetFieldExportGroupPathToIndex.Clear();
            GuidCache.NetGuidToPathName.Clear();
            GuidCache.ObjectLookup.Clear();
            GuidCache.NetFieldExportGroupMapPathFixed.Clear();

        }

        protected virtual void Debug(string filename, string directory, byte[] data)
        {
            return;

            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllBytes($"{directory}/{filename}.dump", data);
        }

        private void Debug(string filename, string line)
        {
            return;

            string name = $"debugFiles/{filename}.txt";

            if (!_streamWriters.TryGetValue(name, out StreamWriter writer))
            {
                writer = new StreamWriter(name, true);

                _streamWriters.TryAdd(name, writer);
            }

            writer.WriteLine(line);
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

            using var binaryArchive = Decompress(archive, info.SizeInBytes);

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

                GuidCache.ObjectLookup[guid] = cacheObject;

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
                GuidCache.NetFieldExportGroupPathToIndex.Clear();
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

            // SerializeDemoFrameFromQueuedDemoPackets
            // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L1978
            var playbackPackets = ReadDemoFrameIntoPlaybackPackets(binaryArchive);
            foreach (var packet in playbackPackets)
            {
                if (packet.State == PacketState.Success)
                {
                    Debug($"checkpoint-{checkpointIndex}-packet-{packetIndex}", "checkpoint-packets", packet.Data);
                    packetIndex++;

                    //Not accurate currently
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
                }

                else if (chunkType == ReplayChunkType.Event)
                {
                    ReadEvent(archive);
                }

                else if (chunkType == ReplayChunkType.ReplayData)
                {
                    ReadReplayData(archive);
                }

                else if (chunkType == ReplayChunkType.Header)
                {
                    ReadReplayHeader(archive);
                }

                if (archive.Position != offset + chunkSize)
                {
                    _logger?.LogWarning($"Chunk ({chunkType}) at offset {offset} not incorrectly read...");
                    archive.Seek(offset + chunkSize, SeekOrigin.Begin);
                }
            }
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

            using var binaryArchive = Decompress(archive, (int)info.Length);

            while (!binaryArchive.AtEnd())
            {
                var playbackPackets = ReadDemoFrameIntoPlaybackPackets(binaryArchive);

                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L3338

                foreach (var packet in playbackPackets.Where(x => x.State == PacketState.Success))
                {
                    Debug($"replaydata-{replayDataIndex}-packet-{packetIndex}", "replay-packets", packet.Data);
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
                _logger.LogError($"Header.Version < MIN_NETWORK_DEMO_VERSION. Header.Version: {header.NetworkVersion}, MIN_NETWORK_DEMO_VERSION: {NetworkVersionHistory.HISTORY_EXTRA_VERSION}");
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
                _logger.LogError("UDemoNetDriver::ReadPacket: OutBufferSize > 2048");

                packet.State = PacketState.Error;

                return packet;
            }
            else if (bufferSize < 0)
            {
                //UE_LOG(LogDemo, Error, TEXT("UDemoNetDriver::ReadPacket: OutBufferSize > MAX_DEMO_READ_WRITE_BUFFER"));
                _logger.LogError("UDemoNetDriver::ReadPacket: OutBufferSize < 0");

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
                var externalData = archive.ReadBytes(externalDataNumBytes);

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

                Debug($"externaldata-{externalDataIndex}-{netGuid}", "externaldata", externalData);
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
                    _logger.LogInformation("ReceiveNetFieldExports: Unable to find NetFieldExportGroup for export.");
                }
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L2848
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<PlaybackPacket> ReadDemoFrameIntoPlaybackPackets(FArchive archive)
        {
            if (archive.NetworkVersion >= NetworkVersionHistory.HISTORY_MULTIPLE_LEVELS)
            {
                var currentLevelIndex = archive.ReadInt32();
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
                _logger.LogWarning("InternalLoadObject: Hit recursion limit.");

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
                _logger.LogError($"UPackageMapClient::ReceiveNetGUIDBunch: NumGUIDsInBunch > MAX_GUID_COUNT({numGUIDsInBunch})");
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
                                    _logger?.LogWarning("Reliable initial partial trying to destroy reliable initial partial");
                                    return;
                                }
                                _logger?.LogWarning("Unreliable initial partial trying to destroy unreliable initial partial");
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
                            _logger?.LogWarning($"Corrupt partial bunch. Initial partial bunches are expected to be byte-aligned. BitsLeft = {bitsLeft % 8}.");
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
                            _logger?.LogWarning("Corrupt partial bunch. Non-final partial bunches are expected to be byte-aligned.");
                            return;
                        }

                        // Advance the sequence of the current partial bunch so we know what to expect next
                        PartialBunch.ChSequence = bunch.ChSequence;

                        if (bunch.bPartialFinal)
                        {
                            _logger?.LogDebug("Completed Partial Bunch.");

                            if (bunch.bHasPackageMapExports)
                            {
                                _logger?.LogWarning("Corrupt partial bunch. Final partial bunch has package map exports.");
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
                ChannelActors[bunch.ChIndex] = false;
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
                ++TotalMappedGUIDs;

                var numMustBeMappedGUIDs = bunch.Archive.ReadUInt16();
                for (var i = 0; i < numMustBeMappedGUIDs; i++)
                {
                    var guid = bunch.Archive.ReadIntPacked();

                }
            }

            /*
            // if actor == null
            var actor = ChannelActors.ContainsKey(bunch.ChIndex) ? ChannelActors[bunch.ChIndex] : false;
            if (!actor && bunch.bOpen)
            {

            }*/

            //_logger?.LogError($"Processing new bunch. MustBeMappedGUIDs {bunch.bHasMustBeMappedGUIDs}");

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

            if (channel.Broken)
            {
                //_logger?.LogInformation($"Channel {bunch.ChIndex} broken. Ignoring bunch");

                return;
            }

            var actor = ChannelActors.ContainsKey(bunch.ChIndex) ? ChannelActors[bunch.ChIndex] : false;
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
                        bool bShouldQuantize = false;

                        if (bWasSerialized)
                        {
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

                Channels[bunch.ChIndex].Actor = inActor;
                ChannelActors[bunch.ChIndex] = true;
                //ChannelNetGuids[bunch.ChIndex] = inActor.ActorNetGUID.Value;
            }

            // RepFlags.bNetOwner = true; // ActorConnection == Connection is always true??

            //RepFlags.bIgnoreRPCs = Bunch.bIgnoreRPCs;
            //RepFlags.bSkipRoleSwap = bSkipRoleSwap;

            //  Read chunks of actor content
            while (!bunch.Archive.AtEnd())
            {
                //FNetBitReader Reader(Bunch.PackageMap, 0 );
                var reader = ReadContentBlockPayload(bunch, out var bHasRepLayout);

                if (bunch.Archive.IsError)
                {
                    //channel.Broken = true;
                    ++TotalFailedBunches;

                    _logger?.LogError($"UActorChannel::ReceivedBunch: ReadContentBlockPayload FAILED. Bunch Info: {bunch}");
                    break;
                }
                else
                {
                    //_logger?.LogError($"No error. Bunch Info: {bunch}");
                }

                if (reader.AtEnd())
                {
                    // Nothing else in this block, continue on (should have been a delete or create block)
                    continue;
                }

                // if ( !Replicator->ReceivedBunch( Reader, RepFlags, bHasRepLayout, bHasUnmapped ) )
                if (!ReceivedReplicatorBunch(bunch, reader, bHasRepLayout))
                {
                    ++TotalFailedReplicatorReceives;
                    // Don't consider this catastrophic in replays
                    _logger?.LogWarning("UActorChannel::ProcessBunch: Replicator.ReceivedBunch returned false");
                    continue;
                }
            }
            // PostReceivedBunch, not interesting?
        }

        private List<NetFieldExportGroup> GeneratePossibleClasses(FBitArchive archive, uint channelIndex)
        {
            var testArchive = new NetBitReader(archive.ReadBits(archive.GetBitsLeft()));

            List<NetFieldExportGroup> possibleClasses = new List<NetFieldExportGroup>();

            foreach (var netFieldExport in GuidCache.NetFieldExportGroupMap.Values)
            {
                bool didFail = false;
                testArchive.Reset();
                testArchive.Mark();

                var doChecksum = testArchive.ReadBit();

                while (true)
                {
                    var handle = testArchive.ReadIntPacked();

                    if (handle == 0)
                    {
                        break;
                    }

                    handle--;

                    if(handle >= netFieldExport.NetFieldExportsLength)
                    {
                        break;
                    }

                    var numBits = testArchive.ReadIntPacked();
                        testArchive.ReadBits(numBits);

                    if (netFieldExport.NetFieldExports[handle] == null)
                    {
                        didFail = true;
                    }
                }

                if(testArchive.AtEnd())
                {
                    if(!didFail)
                    {
                        possibleClasses.Add(netFieldExport);
                    }
                }

                testArchive.Pop();
            }

            return possibleClasses;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Private/DataReplication.cpp#L896
        /// </summary>
        /// <param name="archive"></param>
        protected virtual bool ReceivedReplicatorBunch(DataBunch bunch, FBitArchive archive, bool bHasRepLayout)
        {
            // outer is used to get path name
            // coreredirects.cpp ...
            NetFieldExportGroup netFieldExportGroup = GuidCache.GetNetFieldExportGroup(Channels[bunch.ChIndex].Actor, out string testPath);

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

                if(!ReceiveProperties(archive, netFieldExportGroup, bunch.ChIndex))
                {
                    return false;
                }
            }

            //Disabled
            /*
            FBitArchive reader;
            while (ReadFieldHeaderAndPayload(archive, netFieldExportGroup, out reader))
            {
                _logger?.LogDebug($"RPCs to read for group {netFieldExportGroup.PathName} and numbits: {reader.GetBitsLeft()}");

                //if (FieldCache == nullptr)
                //{
                //    UE_LOG(LogNet, Warning, TEXT("ReceivedBunch: FieldCache == nullptr: %s"), *Object->GetFullName());
                //    continue;
                //}

                //if (FieldCache->bIncompatible)
                //{
                //    // We've already warned about this property once, so no need to continue to do so
                //    UE_LOG(LogNet, Verbose, TEXT("ReceivedBunch: FieldCache->bIncompatible == true. Object: %s, Field: %s"), *Object->GetFullName(), *FieldCache->Field->GetFName().ToString());
                //    continue;
                //}


                // Handle property
                // if (UProperty * ReplicatedProp = Cast<UProperty>(FieldCache->Field))
                // {
                // We should only be receiving custom delta properties (since RepLayout handles the rest)
                //if (!Retirement[ReplicatedProp->RepIndex].CustomDelta)

                //// Call PreNetReceive if we haven't yet
                //if (!bHasReplicatedProperties)
                //{
                //    bHasReplicatedProperties = true;        // Persistent, not reset until PostNetReceive is called
                //    PreNetReceive();
                //}

                // // Receive array index (static sized array, i.e. MemberVariable[4])
                // bunch.Archive.ReadIntPacked();

                // Call the custom delta serialize function to handle it
                //CppStructOps->NetDeltaSerialize(Parms, Data);

                // Successfully received it.
                // }
                //else
                //{
                // Handle function call
                //Cast<UFunction>(FieldCache->Field)
                //}
            }*/


            return true;
        }

        /// <summary>
        ///  https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Private/RepLayout.cpp#L2895
        ///  https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Private/RepLayout.cpp#L2971
        ///  https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Private/RepLayout.cpp#L3022
        /// </summary>
        /// <param name="archive"></param>
        protected virtual bool ReceiveProperties(FBitArchive archive, NetFieldExportGroup group, uint channelIndex)
        {
            ++TotalGroupsRead;

            var doChecksum = archive.ReadBit();
            Debug("types", $"\n{group.PathName}");

            if(NetFieldParser.IncludeOnlyMode && !NetFieldParser.WillReadType(group.PathName))
            {
                return true;
            }

            INetFieldExportGroup exportGroup = NetFieldParser.CreateType(group.PathName);

            List<INetFieldExportGroup> groups = new List<INetFieldExportGroup>();

            if (exportGroup != null)
            {
                if (!ExportGroups.TryAdd(channelIndex, groups))
                {
                    ExportGroups.TryGetValue(channelIndex, out groups);
                }
            }

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
                    _logger.LogError($"NetFieldExport length ({group.NetFieldExports.Length}) < handle ({handle})");

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
                    var cmdReader = new NetBitReader(archive.ReadBits(numBits))
                    {
                        EngineNetworkVersion = Replay.Header.EngineNetworkVersion,
                        NetworkVersion = Replay.Header.NetworkVersion
                    };

                    NetFieldParser.ReadField(exportGroup, export, group, handle, cmdReader);

                    if (cmdReader.IsError)
                    {
                        ++PropertyError;

                        _logger?.LogError($"Property {export.Name} caused error when reading (bits: {numBits}, group: {group.PathName})");

#if DEBUG
                        cmdReader.Reset();

                        NetFieldParser.ReadField(exportGroup, export, group, handle, cmdReader);
#endif
                        continue;
                    }

                    if (archive.IsError)
                    {
                        _logger?.LogError($"Property {export.Name} caused error when reading (bits: {numBits}, group: {group.PathName})");
                        continue;
                    }

                    if (!cmdReader.AtEnd())
                    {
                        ++FailedToRead;

                        _logger?.LogWarning($"Property {export.Name} {group.PathName} didnt read proper number of bits: {cmdReader.GetBitsLeft()} out of {numBits}");

                        continue;
                    }
                }
                catch(Exception ex)
                {
                    _logger?.LogError($"NetFieldParser exception. Ex: {ex.Message}");
                }

            }

            if (hasData)
            {
                groups.Add(exportGroup);
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
            if (bunch.AtEnd())
            {
                reader = null;
                outField = null;
                return false;
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

            // const FNetFieldExport& NetFieldExport = NetFieldExportGroup->NetFieldExports[NetFieldExportHandle];
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
        protected virtual FBitArchive ReadContentBlockPayload(DataBunch bunch, out bool bOutHasRepLayout)
        {
            bOutHasRepLayout = ReadContentBlockHeader(bunch);

            var numPayloadBits = bunch.Archive.ReadIntPacked();

            return new BitReader(bunch.Archive.ReadBits(numPayloadBits));
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DataChannel.cpp#L3175
        /// </summary>
        protected virtual bool ReadContentBlockHeader(DataBunch bunch)
        {
            //  bool& bObjectDeleted, bool& bOutHasRepLayout 
            //var bObjectDeleted = false;
            var bOutHasRepLayout = bunch.Archive.ReadBit();
            var bIsActor = bunch.Archive.ReadBit();
            if (bIsActor)
            {
                // If this is for the actor on the channel, we don't need to read anything else
                return bOutHasRepLayout;
            }

            // We need to handle a sub-object
            // Manually serialize the object so that we can get the NetGUID (in order to assign it if we spawn the object here)

            var netGuid = InternalLoadObject(bunch.Archive, false);

            var bStablyNamed = bunch.Archive.ReadBit();
            if (bStablyNamed)
            {
                // If this is a stably named sub-object, we shouldn't need to create it. Don't raise a bunch error though because this may happen while a level is streaming out.
                return bOutHasRepLayout;
            }

            // Serialize the class in case we have to spawn it.

            var classNetGUID = InternalLoadObject(bunch.Archive, false);

            //Object deleteed
            if (!classNetGUID.IsValid())
            {
                // bObjectDeleted = true;
            }

            return bOutHasRepLayout;
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

                    if (!InReliable.ContainsKey(bunch.ChIndex))
                    {
                        InReliable.Add(bunch.ChIndex, 0);
                    }
                    bunch.ChSequence = InReliable[bunch.ChIndex] + 1;
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
                            _logger.LogError("Channel name serialization failed.");

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

                var channel = Channels.ContainsKey(bunch.ChIndex);

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
                var ignoreAlreadyOpenedChannels = true;

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
                        IgnoringChannels.TryAdd(bunch.ChIndex, actorGuid);
                    }

                    if (IgnoringChannels.ContainsKey(bunch.ChIndex))
                    {
                        if (bunch.bClose && (!bunch.bPartial || bunch.bPartialFinal))
                        {
                            //FNetworkGUID ActorGUID = IgnoringChannels.FindAndRemoveChecked(Bunch.ChIndex);
                            IgnoringChannels.Remove(bunch.ChIndex, out var actorguid);
                        }

                        continue;
                    }
                }

                // Ignore if reliable packet has already been processed.
                if (bunch.bReliable && InReliable.TryGetValue(bunch.ChIndex, out int reliableChIndex) && bunch.ChSequence <= reliableChIndex)
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

                    Channels.Add(bunch.ChIndex, newChannel);
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

            var decompressedSize = archive.ReadInt32();
            var compressedSize = archive.ReadInt32();
            var compressedBuffer = archive.ReadBytes(compressedSize);
            var output = Oodle.DecompressReplayData(compressedBuffer, compressedSize, decompressedSize);
            var decompressed = new Core.BinaryReader(new MemoryStream(output))
            {
                EngineNetworkVersion = Replay.Header.EngineNetworkVersion,
                NetworkVersion = Replay.Header.NetworkVersion,
                ReplayHeaderFlags = Replay.Header.Flags,
                ReplayVersion = Replay.Info.FileVersion
            };

            //_logger?.LogInformation($"Decompressed archive from {compressedSize} to {decompressedSize}.");
            return decompressed;
        }
    }
}
