﻿using FortniteReplayReader;
using FortniteReplayReader.Core.Models;
using FortniteReplayReader.Core.Models.Enums;
using FortniteReplayReaderDecompressor.Core.Models;
using FortniteReplayReaderDecompressor.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace FortniteReplayReaderDecompressor
{
    public class FortniteBinaryDecompressor : FortniteBinaryReader
    {
        public FortniteBinaryDecompressor(Stream input) : base(input)
        {
        }

        public FortniteBinaryDecompressor(Stream input, int offset) : base(input)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/CoreUObject/Private/UObject/CoreNet.cpp#L277
        /// </summary>
        public void StaticParseName()
        {
            var isHardcoded = ReadBoolean();
            if (isHardcoded)
            {
                uint nameIndex;
                if (Replay.Header.EngineNetworkVersionHistory < EngineNetworkVersionHistory.HISTORY_CHANNEL_NAMES)
                {
                    nameIndex = ReadUInt32();
                }
                else
                {
                    nameIndex = ReadIntPacked();
                }

                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Public/UObject/UnrealNames.h#L31
                // hard coded names in "UnrealNames.inl"
            }
            else
            {
                var inString = ReadFString();
                var inNumber = ReadInt32();
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Classes/Engine/PackageMapClient.h#L64
        /// </summary>
        public virtual void ReadNetFieldExport()
        {
            var isExported = ReadBoolean();
            if (isExported)
            {
                var handle = ReadIntPacked();
                var checksum = ReadUInt32();


                // we dont know the header on checkpoint0 ...?
                // guess we can skip isLoading parts
                if (EngineNetworkVersionHistory.HISTORY_NEW_ACTOR_OVERRIDE_LEVEL < EngineNetworkVersionHistory.HISTORY_NETEXPORT_SERIALIZATION)
                {
                    var name = ReadFString();
                    var type = ReadFString();
                }
                else if (EngineNetworkVersionHistory.HISTORY_NEW_ACTOR_OVERRIDE_LEVEL < EngineNetworkVersionHistory.HISTORY_NETEXPORT_SERIALIZE_FIX)
                {
                    // fname
                    var exportname = ReadFString();
                }
                else
                {
                    StaticParseName();
                }
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Classes/Engine/PackageMapClient.h#L133
        /// </summary>
        public virtual void ReadNetFieldExportGroupMap()
        {
            var pathName = ReadFString();
            var pathNameIndex = ReadIntPacked();
            var numNetFieldExports = ReadUInt32();
            for (var i = 0; i < numNetFieldExports; i++)
            {
                ReadNetFieldExport();
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L1667
        /// </summary>
        public override void ParseCheckPoint()
        {
            var id = ReadFString();
            var group = ReadFString();
            var metadata = ReadFString();
            var time1 = ReadUInt32();
            var time2 = ReadUInt32();
            var eventSizeInBytes = ReadInt32();

            var offset = BaseStream.Position;
            using (var reader = Decompress())
            {
                // SerializeDeletedStartupActors
                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L1916
                var packetOffset = reader.ReadInt64();
                var levelForCheckpoint = reader.ReadInt32();
                var deletedNetStartupActors = reader.ReadArray(reader.ReadFString);

                // SerializeGuidCache
                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L1591
                var count = reader.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    var guid = reader.ReadIntPacked();
                    var outerGuid = reader.ReadIntPacked();
                    var path = reader.ReadFString();
                    var checksum = reader.ReadUInt32();
                    var flags = reader.ReadByte();
                }

                // SerializeNetFieldExportGroupMap 
                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L1289
                var numNetFieldExportGroups = reader.ReadUInt32();
                for (var i = 0; i < numNetFieldExportGroups; i++)
                {
                    //reader.ReadNetFieldExportGroupMap();
                }

                var remainingBytes = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
                var output = reader.ReadBytes(remainingBytes);
                File.WriteAllBytes($"{id}.dump", output);

                // SerializeDemoFrameFromQueuedDemoPackets
                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L1978
            }
        }

        // fname
        // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Public/Serialization/NameAsStringProxyArchive.h#L11

        // ReceiveNetFieldExports
        // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L1497

        // ReadDemoFrameIntoPlaybackPackets
        // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L2848

        // SaveExternalData
        // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L2071

        // TickDemoRecord
        // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L2255

        // TickDemoRecordFrame
        // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L2324

        // WritePacket
        // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L3450

        // Serialize - might be useful...
        // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L5775

        // SerializeHitResult - might be useful one day
        // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Developer/CollisionAnalyzer/Private/CollisionAnalyzer.cpp#L19

        //  UnrealMath - FVector etc
        // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Private/Math/UnrealMath.cpp#L57

        // PackedVector
        // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Classes/Engine/NetSerialization.h#L1210


        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L2106
        /// </summary>
        public virtual void ReadExternalData()
        {
            while (true)
            {
                var externalDataNumBits = ReadIntPacked();
                if (externalDataNumBits == 0)
                {
                    return;
                }

                var netGuid = ReadIntPacked();
                var externalDataNumBytes = (int)(externalDataNumBits + 7) >> 3;
                var unknown = ReadBytes(externalDataNumBytes);
            }
        }


        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L1348
        /// </summary>
        public virtual void ReadExportData()
        {
            ReadNetFieldExports();
            ReadNetExportGuids();
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L1564
        /// </summary>
        public virtual void ReadNetExportGuids()
        {
            var numGuids = ReadIntPacked();
            for (var i = 0; i < numGuids; i++)
            {
                var size = ReadInt32();
                var guid = ReadBytes(size);
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L1356
        /// </summary>
        public virtual void ReadNetFieldExports()
        {
            var netFieldCount = ReadIntPacked();
            for (var i = 0; i < netFieldCount; i++)
            {
                var pathNameIndex = ReadIntPacked();
                var needsExport = ReadIntPacked() == 1;

                if (needsExport)
                {
                    var pathName = ReadFString();
                    var numExports = ReadIntPacked();
                }

                ReadNetFieldExport();
            }

        }


        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L3220
        /// </summary>
        public virtual PlaybackPacket ReadPacket()
        {
            var packet = new PlaybackPacket();

            var bufferSize = ReadInt32();
            if (bufferSize == 0)
            {
                packet.State = PacketState.End;
                return packet;
            }

            packet.Data = ReadBytes(bufferSize);
            packet.State = PacketState.Success;
            return packet;
        }

        public virtual void ProcessPacket(PlaybackPacket packet)
        {
            var bitReader = new BitReader(packet.Data);

            // For demo backwards compatibility, old replays still have this bit
            //if (InternalAck && EngineNetworkProtocolVersion < EEngineNetworkVersionHistory::HISTORY_ACKS_INCLUDED_IN_HEADER)
            //{
            //    const bool IsAckDummy = Reader.ReadBit() == 1u;
            //}

            var startPos = bitReader.Position;
            var bControl = bitReader.ReadBit();
            var bOpen = bControl ? bitReader.ReadBit() : false;
            var bClose = bControl ? bitReader.ReadBit() : false;

            //if (Bunch.EngineNetVer() < HISTORY_CHANNEL_CLOSE_REASON)
            //{
            //    Bunch.bDormant = Bunch.bClose ? Reader.ReadBit() : 0;
            //    Bunch.CloseReason = Bunch.bDormant ? EChannelCloseReason::Dormancy : EChannelCloseReason::Destroyed;
            //}
            //else
            //{
            //    Bunch.CloseReason = Bunch.bClose ? (EChannelCloseReason)Reader.ReadInt((uint32)EChannelCloseReason::MAX) : EChannelCloseReason::Destroyed;
            //    Bunch.bDormant = (Bunch.CloseReason == EChannelCloseReason::Dormancy);
            //}

            var bIsReplicationPaused = bitReader.ReadBit();
            var bReliable = bitReader.ReadBit();

            //if (Bunch.EngineNetVer() < HISTORY_MAX_ACTOR_CHANNELS_CUSTOMIZATION)
            //{
            //    static const int OLD_MAX_ACTOR_CHANNELS = 10240;
            //    Bunch.ChIndex = Reader.ReadInt(OLD_MAX_ACTOR_CHANNELS);
            //}
            //else
            //{
            //    uint32 ChIndex;
            //    Reader.SerializeIntPacked(ChIndex);

            //    if (ChIndex >= (uint32)MaxChannelSize)
            //    {
            //        CLOSE_CONNECTION_DUE_TO_SECURITY_VIOLATION(this, ESecurityEvent::Malformed_Packet, TEXT("Bunch channel index exceeds channel limit"));
            //        return;
            //    }

            //    Bunch.ChIndex = ChIndex;
            //}

            var bHasPackageMapExports = bitReader.ReadBit();
            var bHasMustBeMappedGUIDs = bitReader.ReadBit();
            var bPartial = bitReader.ReadBit();

            var ChSequence = 0; // todo

            var bPartialInitial = bPartial ? bitReader.ReadBit() : false;
            var bPartialFinal = bPartial ? bitReader.ReadBit() : false;

            //int32 BunchDataBits = Reader.ReadInt(UNetConnection::MaxPacket * 8);
            var headerPos = bitReader.Position;
            if (bHasPackageMapExports)
            {
                // ReceiveNetGUIDBunch
            }

            // var bNewlyOpenedActorChannel = Bunch.bOpen && (Bunch.ChName == NAME_Actor) && (!Bunch.bPartial || Bunch.bPartialInitial);
            if (bHasMustBeMappedGUIDs)
            {
                // var numMustBeMappedGUIDs = bitReader.readuint16();
                // for (var i = 0; i < numMustBeMappedGUIDs; i++) {
                //  FNetworkGUID
                // }
            }

            //FNetworkGUID ActorGUID;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/NetworkReplayStreaming/LocalFileNetworkReplayStreaming/Private/LocalFileNetworkReplayStreaming.cpp#L318
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L2848
        /// </summary>
        public override void ParseReplayData()
        {
            if (Replay.Metadata.FileVersion >= ReplayVersionHistory.StreamChunkTimes)
            {
                var start = ReadUInt32();
                var end = ReadUInt32();
                var length = ReadUInt32();
            }

            using (var reader = Decompress())
            {
                var remainingBytes = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
                var output = reader.ReadBytes(remainingBytes);
                reader.BaseStream.Position -= remainingBytes;
                File.WriteAllBytes($"replaydata-start-{reader.BaseStream.Position}.dump", output);

                // if HISTORY_MULTIPLE_LEVELS
                var currentLevelIndex = reader.ReadInt32();
                var timeSeconds = reader.ReadSingle();

                // if HISTORY_LEVEL_STREAMING_FIXES
                reader.ReadExportData();

                // if HasLevelStreamingFixes()
                if (true)
                {
                    var numStreamingLevels = reader.ReadIntPacked();
                    for (var i = 0; i < numStreamingLevels; i++)
                    {
                        var levelName = reader.ReadFString();
                    }
                }
                else
                {
                    var numStreamingLevels = reader.ReadIntPacked();
                    for (var i = 0; i < numStreamingLevels; i++)
                    {
                        var packageName = reader.ReadFString();
                        var packageNameToLoad = reader.ReadFString();
                        // FTransform
                        //var levelTransform = reader.ReadFString();
                        // filter duplicates
                    }
                }

                var externalOffset = reader.ReadUInt64();

                // if (!bForLevelFastForward)
                reader.ReadExternalData();
                // else skip externalOffset

                remainingBytes = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
                output = reader.ReadBytes(remainingBytes);
                reader.BaseStream.Position -= remainingBytes;
                File.WriteAllBytes($"replaydata-prepackets-{reader.BaseStream.Position}.dump", output);

                var playbackPackets = new List<PlaybackPacket>();
                var @continue = true;
                while (@continue)
                {
                    var seenLevelIndex = reader.ReadIntPacked();
                    var packet = reader.ReadPacket();
                    playbackPackets.Add(packet);

                    @continue = packet.State switch
                    {
                        PacketState.End => false,
                        PacketState.Error => false,
                        PacketState.Success => true,
                        _ => false
                    };
                }

                // https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/DemoNetDriver.cpp#L3338
                foreach (var packet in playbackPackets)
                {
                    //ProcessPacket(packet);
                }

                remainingBytes = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
                output = reader.ReadBytes(remainingBytes);
                File.WriteAllBytes($"replaydata-postpackets-{reader.BaseStream.Position}.dump", output);
            }
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Private/Serialization/CompressedChunkInfo.cpp#L9
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Plugins/Runtime/PacketHandlers/CompressionComponents/Oodle/Source/OodleHandlerComponent/Private/OodleArchives.cpp#L21
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private FortniteBinaryDecompressor Decompress()
        {
            var decompressedSize = ReadInt32();
            var compressedSize = ReadInt32();
            var compressedBuffer = ReadBytes(compressedSize);
            var output = Oodle.DecompressReplayData(compressedBuffer, compressedBuffer.Length, decompressedSize);
            var reader = new FortniteBinaryDecompressor(new MemoryStream(output));
            reader.BaseStream.Position = 0;
            return reader;
        }
    }
}
