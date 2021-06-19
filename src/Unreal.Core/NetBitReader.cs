﻿using System;
using System.IO;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace Unreal.Core
{
    /// <summary>
    /// A <see cref="BitReader"/> used for reading everything related to RepLayout. 
    /// see https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/CoreUObject/Public/UObject/CoreNet.h#L303
    /// </summary>
    public unsafe sealed class NetBitReader : BitReader
    {
        public NetBitReader() : base() { }

        //public NetBitReader(byte[] input) : base(input) { }
        //public NetBitReader(byte[] input, int bitCount) : base(input, bitCount) { }
        public NetBitReader(byte* buffer, int byteCount, int bitCount) : base(buffer, byteCount, bitCount) { }
        private NetBitReader(bool* boolPtr, int bitCount) : base(boolPtr, bitCount) { }

        //public NetBitReader(FBitArray input) : base(input) { }

        public NetBitReader GetNetBitReader(int bitCount)
        {
            NetBitReader reader = new NetBitReader(Bits + _position, bitCount);

            reader._items = _items.Slice(_position, bitCount);

            _position += bitCount;

            return reader;
        }

        public int SerializePropertyInt()
        {
            return ReadInt32();
        }

        public uint SerializePropertyUInt32()
        {
            return ReadUInt32();
        }

        public uint SerializePropertyUInt16()
        {
            return ReadUInt16();
        }

        public ulong SerializePropertyUInt64()
        {
            return ReadUInt64();
        }

        public float SerializePropertyFloat()
        {
            return ReadSingle();
        }

        public string SerializePropertyName()
        {
            var isHardcoded = ReadBoolean();
            if (isHardcoded)
            {
                uint nameIndex;
                if (EngineNetworkVersion < EngineNetworkVersionHistory.HISTORY_CHANNEL_NAMES)
                {
                    nameIndex = ReadUInt32();
                }
                else
                {
                    nameIndex = ReadIntPacked();
                }

                return ((UnrealNames)nameIndex).ToString();
            }

            var inString = ReadFString();
            var inNumber = ReadInt32();

            return inString;
        }

        public string SerializePropertyString()
        {
            if(GetBitsLeft() == 32)
            {
                Seek(32, SeekOrigin.Current);

                return String.Empty;
            }

            return ReadFString();
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Classes/Engine/EngineTypes.h#L3074
        /// </summary>
        public FRepMovement SerializeRepMovement(
            VectorQuantization locationQuantizationLevel = VectorQuantization.RoundTwoDecimals,
            RotatorQuantization rotationQuantizationLevel = RotatorQuantization.ByteComponents,
            VectorQuantization velocityQuantizationLevel = VectorQuantization.RoundWholeNumber)
        {
            var repMovement = new FRepMovement();

            if(!CanRead(2))
            {
                IsError = true;

                return repMovement;
            }

            repMovement.bSimulatedPhysicSleep = Bits[_position++];
            repMovement.bRepPhysics = Bits[_position++];

            repMovement.Location = SerializeQuantizedVector(locationQuantizationLevel);

            switch(rotationQuantizationLevel)
            {
                case RotatorQuantization.ByteComponents:
                    repMovement.Rotation = ReadRotation();
                    break;
                case RotatorQuantization.ShortComponents:
                    repMovement.Rotation = ReadRotationShort();
                    break;
            }

            repMovement.LinearVelocity = SerializeQuantizedVector(velocityQuantizationLevel);

            if (repMovement.bRepPhysics)
            {
                repMovement.AngularVelocity = SerializeQuantizedVector(velocityQuantizationLevel);
            }

            return repMovement;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/5677c544747daa1efc3b5ede31642176644518a6/Engine/Source/Runtime/Core/Private/Math/UnrealMath.cpp#L65
        /// </summary>
        public FVector2D SerializeVector2D()
        {
            var x = ReadSingle();
            var y = ReadSingle();
            return new FVector2D(x, y);
        }

        public FVector SerializePropertyVector()
        {
            return new FVector(ReadSingle(), ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// NetSerialization.cpp 1858
        /// </summary>
        public FVector SerializePropertyVectorNormal()
        {
            return new FVector(ReadFixedCompressedFloat(1, 16), ReadFixedCompressedFloat(1, 16), ReadFixedCompressedFloat(1, 16));
        }

        /// <summary>
        /// NetSerialization.h 1768
        /// </summary>
        public FVector SerializePropertyVector10()
        {
            return ReadPackedVector(10, 24);
        }

        /// <summary>
        /// NetSerialization.h 1768
        /// </summary>
        public FVector SerializePropertyVector100()
        {
            return ReadPackedVector(100, 30);
        }

        public FVector SerializePropertyQuantizeVector()
        {
            return ReadPackedVector(1, 20);
        }

        public void SerializPropertyPlane()
        {

        }

        /// <summary>
        /// NetSerialization.h 1821
        /// </summary>
        public float ReadFixedCompressedFloat(int maxValue, int numBits)
        {
            int maxBitValue = (1 << (numBits - 1)) - 1;
            int bias = (1 << (numBits - 1));
            int serIntMax = (1 << (numBits - 0));
            //int maxDelta = (1 << (numBits - 0)) -1;

            uint delta = ReadSerializedInt(serIntMax);
            float unscaledValue = unchecked((int)delta) - bias;

            if(maxValue > maxBitValue)
            {
                float invScale = maxValue / (float)maxBitValue;
                
                return unscaledValue * invScale;
            }
            else
            {
                float scale = maxBitValue / (float)maxValue;
                float invScale = 1f / scale;

                return unscaledValue * invScale;
            }
        }

        /// <summary>
        /// Unrealmath.cpp 65
        /// </summary>
        public FRotator SerializePropertyRotator()
        {
            return ReadRotationShort();
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/5677c544747daa1efc3b5ede31642176644518a6/Engine/Source/Runtime/CoreUObject/Private/UObject/PropertyByte.cpp#L83
        /// </summary>
        public int SerializePropertyByte(int enumMaxValue)
        {
            //Ar.SerializeBits( Data, Enum ? FMath::CeilLogTwo(Enum->GetMaxEnumValue()) : 8 );

#if NETSTANDARD
            var log2 = Math.Log(enumMaxValue, 2);
#else
            var log2 = Math.Log2(enumMaxValue);
#endif
            return ReadBitsToInt(enumMaxValue > 0 ? (int)Math.Ceiling(log2) : 8);
        }

        public byte SerializePropertyByte()
        {
            return (byte)SerializePropertyByte(-1);
        }

        public int SerializeEnum(int bits)
        {
            return ReadBitsToInt(bits);
        }

        public int SerializeEnum()
        {
            return ReadBitsToInt(GetBitsLeft());
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/5677c544747daa1efc3b5ede31642176644518a6/Engine/Source/Runtime/CoreUObject/Private/UObject/PropertyBool.cpp#L331
        /// </summary>
        public bool SerializePropertyBool()
        {
            return ReadBit();
            //uint8* ByteValue = (uint8*)Data + ByteOffset;
            //uint8 Value = ((*ByteValue & FieldMask) != 0);
            //Ar.SerializeBits(&Value, 1);
            //*ByteValue = ((*ByteValue) & ~FieldMask) | (Value ? ByteMask : 0);
            //return true;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/5677c544747daa1efc3b5ede31642176644518a6/Engine/Source/Runtime/CoreUObject/Private/UObject/PropertyBool.cpp#L331
        /// </summary>
        public bool SerializePropertyNativeBool()
        {
            return ReadBit();
            //uint8* ByteValue = (uint8*)Data + ByteOffset;
            //uint8 Value = ((*ByteValue & FieldMask) != 0);
            //Ar.SerializeBits(&Value, 1);
            //*ByteValue = ((*ByteValue) & ~FieldMask) | (Value ? ByteMask : 0);
            //return true;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/5677c544747daa1efc3b5ede31642176644518a6/Engine/Source/Runtime/CoreUObject/Private/UObject/EnumProperty.cpp#L142
        /// </summary>
        public int SerializePropertyEnum(int enumMaxValue)
        {
            return ReadBitsToInt((int)CeilLogTwo64((ulong)enumMaxValue));

            // Ar.SerializeBits(Data, FMath::CeilLogTwo64(Enum->GetMaxEnumValue()));

            /*
             * if (Ar.EngineNetVer() < HISTORY_FIX_ENUM_SERIALIZATION)
            {
            Ar.SerializeBits(Data, FMath::CeilLogTwo64(Enum->GetMaxEnumValue()));
            }
            else
            {
            Ar.SerializeBits(Data, FMath::CeilLogTwo64(Enum->GetMaxEnumValue() + 1));
            }
            */
        }

        /// <summary>
        /// PropertyBaseObject.cpp 84
        /// </summary>
        public uint SerializePropertyObject()
        {
            //InternalLoadObject(); // TODO make available in archive

            var netGuid = new NetworkGUID()
            {
                Value = ReadIntPacked()
            };

            return netGuid.Value;

            //if (!netGuid.IsValid())
            //{
            //    return;
            //}

            //if (netGuid.IsDefault() || exportGUIDs)
            //{
            //    var flags = archive.ReadByteAsEnum<ExportFlags>();

            //    // outerguid
            //    if (flags == ExportFlags.bHasPath || flags == ExportFlags.bHasPathAndNetWorkChecksum || flags == ExportFlags.All)
            //    {
            //        var outerGuid = InternalLoadObject(archive, true); // TODO: archetype?

            //        var pathName = archive.ReadFString();

            //        if (!NetGuidCache.ContainsKey(netGuid.Value))
            //        {
            //            NetGuidCache.Add(netGuid.Value, pathName);
            //        }

            //        if (flags >= ExportFlags.bHasNetworkChecksum)
            //        {
            //            var networkChecksum = archive.ReadUInt32();
            //        }

            //        return netGuid;
            //    }
            //}

            //return netGuid;

            //UObject* Object = GetObjectPropertyValue(Data);
            //bool Result = Map->SerializeObject(Ar, PropertyClass, Object);
            //SetObjectPropertyValue(Data, Object);
            //return Result;
        }


        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/bf95c2cbc703123e08ab54e3ceccdd47e48d224a/Engine/Source/Runtime/Engine/Classes/Engine/EngineTypes.h#L3047
        /// </summary>
        public FVector SerializeQuantizedVector(VectorQuantization quantizationLevel)
        {
            return quantizationLevel switch
            {
                VectorQuantization.RoundTwoDecimals => ReadPackedVector(100, 30),
                VectorQuantization.RoundOneDecimal => ReadPackedVector(10, 27),
                _ => ReadPackedVector(1, 24),
            };
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/5677c544747daa1efc3b5ede31642176644518a6/Engine/Source/Runtime/Engine/Private/OnlineReplStructs.cpp#L209
        /// </summary>
        public string SerializePropertyNetId()
        {
            if(GetBitsLeft() == 32)
            {
                ReadBits(GetBitsLeft());

                return String.Empty;
            }

            // Use highest value for type for other (out of engine) oss type 
            const byte typeHashOther = 31;

            var encodingFlags = ReadByteAsEnum<UniqueIdEncodingFlags>();
            var encoded = false;
            if ((encodingFlags & UniqueIdEncodingFlags.IsEncoded) == UniqueIdEncodingFlags.IsEncoded)
            {
                encoded = true;
                if ((encodingFlags & UniqueIdEncodingFlags.IsEmpty) == UniqueIdEncodingFlags.IsEmpty)
                {
                    // empty cleared out unique id
                    return "";
                }
            }

            // Non empty and hex encoded
            var typeHash = ((int)(encodingFlags & UniqueIdEncodingFlags.TypeMask)) >> 3;
            if (typeHash == 0)
            {
                // If no type was encoded, assume default
                //TypeHash = UOnlineEngineInterface::Get()->GetReplicationHashForSubsystem(UOnlineEngineInterface::Get()->GetDefaultOnlineSubsystemName());
                return "NULL";
            }

            bool bValidTypeHash = typeHash != 0;
            string typeString;
            if (typeHash == typeHashOther)
            {
                typeString = ReadFString();
                if (typeString == UnrealNameConstants.Names[(int)UnrealNames.None])
                {
                    bValidTypeHash = false;
                }
            }

            if (bValidTypeHash)
            {
                if (encoded)
                {
                    var encodedSize = ReadByte();

                    Span<byte> bytes = stackalloc byte[encodedSize];
                    ReadBytes(bytes);
                    return Convert.ToHexString(bytes);
                }
                else
                {
                    return ReadFString();
                }
            }

            return String.Empty;
        }

        /// <summary>
        /// Computes the base 2 logarithm for a 64-bit value that is greater than 0.
        /// The result is rounded down to the nearest integer.
        /// see https://github.com/EpicGames/UnrealEngine/blob/5677c544747daa1efc3b5ede31642176644518a6/Engine/Source/Runtime/Core/Public/GenericPlatform/GenericPlatformMath.h#L332
        /// </summary>
        /// <param name="Value">The value to compute the log of</param>
        /// <returns>Log2 of Value. 0 if Value is 0.</returns>
        public static ulong FloorLog2_64(ulong Value)
        {
            ulong pos = 0;
            if (Value >= 1ul << 32) { Value >>= 32; pos += 32; }
            if (Value >= 1ul << 16) { Value >>= 16; pos += 16; }
            if (Value >= 1ul << 8) { Value >>= 8; pos += 8; }
            if (Value >= 1ul << 4) { Value >>= 4; pos += 4; }
            if (Value >= 1ul << 2) { Value >>= 2; pos += 2; }
            if (Value >= 1ul << 1) { pos += 1; }
            return (Value == 0) ? 0 : pos;
        }

        /// <summary>
        /// Counts the number of leading zeros in the bit representation of the 64-bit value.
        /// see https://github.com/EpicGames/UnrealEngine/blob/5677c544747daa1efc3b5ede31642176644518a6/Engine/Source/Runtime/Core/Public/GenericPlatform/GenericPlatformMath.h#L364
        /// </summary>
        /// <param name="Value">the value to determine the number of leading zeros for</param>
        /// <returns>the number of zeros before the first "on" bit</returns>
        public static ulong CountLeadingZeros64(ulong Value)
        {
            if (Value == 0) return 64;
            return 63 - FloorLog2_64(Value);
        }

        public static ulong CeilLogTwo64(ulong Arg)
        {
            ulong Bitmask = ((ulong)(CountLeadingZeros64(Arg) << 57)) >> 63;
            return (64 - CountLeadingZeros64(Arg - 1)) & (~Bitmask);
        }
    }
}
