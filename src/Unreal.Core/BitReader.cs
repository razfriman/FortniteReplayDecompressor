using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Unreal.Core.Extensions;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace Unreal.Core
{
    /// <summary>
    /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Public/Serialization/BitArchive.h
    /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Private/Serialization/BitArchive.cpp
    /// </summary>
    public class BitReader : FBitArchive
    {
        protected FBitArray Bits { get; set; }

        /// <summary>
        /// Position in current BitArray. Set with <see cref="Seek(int, SeekOrigin)"/>
        /// </summary>
        public override int Position { get => _position; protected set => _position = value; }

        protected int _position;

        /// <summary>
        /// Last used bit Position in current BitArray. Used to avoid reading trailing zeros to fill last byte.
        /// </summary>
        public int LastBit { get; private set; }

        private int[] _tempLastBit = new int[10];
        private int[] _tempPosition = new int[10];

        /// <summary>
        /// For pushing and popping FBitReaderMark positions.
        /// </summary>
        public int MarkPosition { get; private set; }

        /// <summary>
        /// Initializes a new instance of the BitReader class based on the specified bytes.
        /// </summary>
        /// <param name="input">The input bytes.</param>
        /// <exception cref="System.ArgumentException">The stream does not support reading, is null, or is already closed.</exception>
        public BitReader(byte[] input)
        {
            Bits = new FBitArray(input);

            LastBit = Bits.Length;
        }

        public BitReader(byte[] input, int bitCount)
        {
            Bits = new FBitArray(input);
            LastBit = bitCount;
        }

        /// <summary>
        /// Initializes a new instance of the BitReader class based on the specified bool[].
        /// </summary>
        /// <param name="input">The input bool[].</param>

        public BitReader(FBitArray input)
        {
            Bits = input;
            LastBit = Bits.Length;
        }

        /// <summary>
        /// Returns whether <see cref="Position"/> in current <see cref="Bits"/> is greater than the lenght of the current <see cref="Bits"/>.
        /// </summary>
        /// <returns>true, if <see cref="Position"/> is greater than lenght, false otherwise</returns>
        public override bool AtEnd()
        {
            return _position >= LastBit;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool CanRead(int count)
        {
            return _position + count <= LastBit;
        }

        /// <summary>
        /// Returns the bit at <see cref="Position"/> and does not advance the <see cref="Position"/> by one bit.
        /// </summary>
        /// <returns>The value of the bit at position index.</returns>
        /// <seealso cref="ReadBit"/>
        public override bool PeekBit()
        {
            return Bits[_position];
        }

        /// <summary>
        /// Returns the bit at <see cref="Position"/> and advances the <see cref="Position"/> by one bit.
        /// </summary>
        /// <returns>The value of the bit at position index.</returns>
        /// <seealso cref="PeekBit"/>
        public override bool ReadBit()
        {
            if (_position >= LastBit)
            {
                IsError = true;
                return false;
            }

            return Bits[_position++];
        }

        /// <summary>
        /// Retuns int and advances the <see cref="Position"/> by <paramref name="bits"/> bits.
        /// </summary>
        /// <param name="bits">The number of bits to read.</param>
        /// <returns>int</returns>
        public int ReadBitsToInt(int bitCount)
        {
            if (!CanRead(bitCount))
            {
                IsError = true;

                return 0;
            }

            var result = 0;

            for (var i = 0; i < bitCount; i++)
            {
                result |= (byte)(Bits.GetAsByte(_position + i) << i);
            }

            _position += bitCount;

            return result;
        }

        /// <summary>
        /// Retuns bool[] and advances the <see cref="Position"/> by <paramref name="bits"/> bits.
        /// </summary>
        /// <param name="bits">The number of bits to read.</param>
        /// <returns>bool[]</returns>

        public override ReadOnlyMemory<bool> ReadBits(int bitCount)
        {
            if (!CanRead(bitCount) || bitCount < 0)
            {
                IsError = true;
                return ReadOnlyMemory<bool>.Empty;
            }


            var result = Bits.Items.Slice(_position, bitCount);

            _position += bitCount;

            return result;
        }

        /*
        public override void Read(bool[] buffer, int count)
        {
            if (!CanRead(count) || count < 0)
            {
                IsError = true;

                return;
            }

            Buffer.BlockCopy(Bits.Items.ToArray(), _position, buffer, 0, count);

            _position += count;
        }*/

        /// <summary>
        /// Retuns bool[] and advances the <see cref="Position"/> by <paramref name="bits"/> bits.
        /// </summary>
        /// <param name="bits">The number of bits to read.</param>
        /// <returns>bool[]</returns>
        public override ReadOnlyMemory<bool> ReadBits(uint bitCount)
        {
            return ReadBits((int)bitCount);
        }

        /// <summary>
        /// Returns the bit at <see cref="Position"/> and advances the <see cref="Position"/> by one bit.
        /// </summary>
        /// <returns>The value of the bit at position index.</returns>
        /// <seealso cref="ReadBit"/>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public override bool ReadBoolean()
        {
            return ReadBit();
        }

        /// <summary>
        /// Returns the byte at <see cref="Position"/>
        /// </summary>
        /// <returns>The value of the byte at <see cref="Position"/> index.</returns>
        public override byte PeekByte()
        {
            var result = ReadByte();
            _position -= 8;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe byte ReadByteNoCheck()
        {
            var result = new byte();

            var pos = _position;

            if (Bits.ByteArrayUsed != null && _position % 8 == 0)
            {
                result = Bits.ByteArrayUsed[_position / 8];
            }
            else
            {
                result |= (Bits.GetAsByte(pos + 0));
                result |= (byte)(Bits.GetAsByte(pos + 1) << 1);
                result |= (byte)(Bits.GetAsByte(pos + 2) << 2);
                result |= (byte)(Bits.GetAsByte(pos + 3) << 3);
                result |= (byte)(Bits.GetAsByte(pos + 4) << 4);
                result |= (byte)(Bits.GetAsByte(pos + 5) << 5);
                result |= (byte)(Bits.GetAsByte(pos + 6) << 6);
                result |= (byte)(Bits.GetAsByte(pos + 7) << 7);
            }

            _position += 8;

            return result;
        }

        /// <summary>
        /// Returns the byte at <see cref="Position"/> and advances the <see cref="Position"/> by 8 bits.
        /// </summary>
        /// <returns>The value of the byte at <see cref="Position"/> index.</returns>
        public override byte ReadByte()
        {
            if (!CanRead(8))
            {
                IsError = true;

                return 0;
            }

            return ReadByteNoCheck();
        }

        public override T ReadByteAsEnum<T>()
        {
            return (T)Enum.ToObject(typeof(T), ReadByte());
        }

        public void ReadBytes(Span<byte> data)
        {
            if (!CanRead(data.Length * 8))
            {
                IsError = true;
                return;
            }

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = ReadByteNoCheck();
            }
        }

        public override byte[] ReadBytes(int byteCount)
        {
            if (byteCount < 0)
            {
                IsError = true;
                return new byte[0];
            }

            if (!CanRead(byteCount))
            {
                IsError = true;
                return Array.Empty<byte>();
            }

            var result = new byte[byteCount];

            if (Bits.ByteArrayUsed != null && _position % 8 == 0)
            {
                //Pull straight from byte array
                Buffer.BlockCopy(Bits.ByteArrayUsed, _position / 8, result, 0, byteCount);

                _position += byteCount * 8;
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = ReadByteNoCheck();
                }
            }

            return result;
        }

        public override byte[] ReadBytes(uint byteCount)
        {
            return ReadBytes((int)byteCount);
        }

        public override T[] ReadArray<T>(Func<T> func1)
        {
            throw new NotImplementedException();
        }

        public override string ReadBytesToString(int count)
        {
            //Never hits this for newer replay

            // https://github.com/dotnet/corefx/issues/10013
            return BitConverter.ToString(ReadBytes(count)).Replace("-", "");
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Private/Containers/String.cpp#L1390
        /// </summary>
        /// <returns>string</returns>
        public override string ReadFString()
        {
            var length = ReadInt32();

            if (length == 0)
            {
                return String.Empty;
            }

            var isUnicode = length < 0;
            length = isUnicode ? -2 * length : length;

            if (length > 256 || length < 0)
            {
                IsError = true;

                return String.Empty;
            }

            Span<byte> bytes = stackalloc byte[length];
            ReadBytes(bytes);


            return isUnicode ? Encoding.Unicode.GetString(bytes.Slice(0, bytes.Length - 2)) : Encoding.Default.GetString(bytes.Slice(0, bytes.Length - 1));
        }

        public override string ReadGUID()
        {
            return ReadBytesToString(16);
        }

        public override string ReadGUID(int size)
        {
            return ReadBytesToString(size);
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Public/Serialization/BitReader.h#L69
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns>uint</returns>
        /// <exception cref="OverflowException"></exception>
        public unsafe override uint ReadSerializedInt(int maxValue)
        {
            int value = 0;
            int count = 0;

            for (uint mask = 1; (value + mask) < maxValue; mask *= 2)
            {
                if (_position >= LastBit)
                {
                    IsError = true;
                    return 0;
                }

                bool isSet = Bits[_position++];

                value |= *(byte*)&isSet << count++;
            }
            
            return (uint)value;
        }

        public UInt32 ReadUInt32Max(Int32 maxValue)
        {
            var maxBits = Math.Floor(Math.Log10(maxValue) / Math.Log10(2)) + 1;

            UInt32 value = 0;
            for (int i = 0; i < maxBits && (value + (1 << i)) < maxValue; ++i)
            {
                value += (ReadBit() ? 1U : 0U) << i;
            }

            if (value > maxValue)
            {
                throw new Exception("ReadUInt32Max overflowed!");
            }

            return value;

        }

        public override short ReadInt16()
        {
            Span<byte> value = stackalloc byte[2];

            ReadBytes(value);

            return BinaryPrimitives.ReadInt16LittleEndian(value);
        }

        public override int ReadInt32()
        {
            Span<byte> value = stackalloc byte[4];

            ReadBytes(value);

            return BinaryPrimitives.ReadInt32LittleEndian(value);
        }

        public override bool ReadInt32AsBoolean()
        {
            return ReadInt32() == 1;
        }

        public override long ReadInt64()
        {
            Span<byte> value = stackalloc byte[8];

            ReadBytes(value);

            return BinaryPrimitives.ReadInt64LittleEndian(value);
        }

        /// <summary>
        /// Retuns uint
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Private/Serialization/BitReader.cpp#L254
        /// </summary>
        /// <returns>uint</returns>
        public override uint ReadIntPacked()
        {
            uint value = 0;
            byte count = 0;
            var remaining = true;

            while (remaining)
            {
                if (_position + 8 > LastBit)
                {
                    IsError = true;
                    return 0;
                }

                var nextByte = ReadByteNoCheck();
                remaining = (nextByte & 1) == 1;            // Check 1 bit to see if theres more after this
                nextByte >>= 1;                             // Shift to get actual 7 bit value
                value += (uint)nextByte << (7 * count++);   // Add to total value
            }

            return value;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Classes/Engine/NetSerialization.h#L1210
        /// </summary>
        /// <returns>Vector</returns>
        public override FVector ReadPackedVector(int scaleFactor, int maxBits)
        {
            var bits = ReadSerializedInt(maxBits);

            var bias = 1 << ((int)bits + 1);
            var max = 1 << ((int)bits + 2);

            var dx = ReadSerializedInt(max);
            var dy = ReadSerializedInt(max);
            var dz = ReadSerializedInt(max);

            if (IsError)
            {
                return new FVector(0, 0, 0);
            }

            var x = (float)(dx - bias) / scaleFactor;
            var y = (float)(dy - bias) / scaleFactor;
            var z = (float)(dz - bias) / scaleFactor;

            FVector vector = new FVector(x, y, z);

            return vector;
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Private/Math/UnrealMath.cpp#L79
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Public/Math/Rotator.h#L654
        /// </summary>
        /// <returns></returns>
        public override FRotator ReadRotation()
        {
            float pitch = 0;
            float yaw = 0;
            float roll = 0;

            if (Bits[_position++]) // Pitch
            {
                pitch = ReadByte() * 360f / 256f;
            }

            if (Bits[_position++])
            {
                yaw = ReadByte() * 360f / 256f;
            }

            if (Bits[_position++])
            {
                roll = ReadByte() * 360f / 256f;
            }

            if (IsError)
            {
                return new FRotator(0, 0, 0);
            }

            return new FRotator(pitch, yaw, roll);
        }

        /// <summary>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Private/Math/UnrealMath.cpp#L79
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Public/Math/Rotator.h#L654
        /// </summary>
        /// <returns></returns>
        public override FRotator ReadRotationShort()
        {
            float pitch = 0;
            float yaw = 0;
            float roll = 0;

            if (Bits[_position++]) // Pitch
            {
                pitch = ReadUInt16() * 360 / 65536f;
            }

            if (Bits[_position++])
            {
                yaw = ReadUInt16() * 360 / 65536f;
            }

            if (Bits[_position++])
            {
                roll = ReadUInt16() * 360 / 65536f;
            }

            if (IsError)
            {
                return new FRotator(0, 0, 0);
            }

            return new FRotator(pitch, yaw, roll);
        }

        public override sbyte ReadSByte()
        {
            throw new NotImplementedException();
        }

        public override float ReadSingle()
        {
            Span<byte> value = stackalloc byte[4];

            ReadBytes(value);

            return BinaryPrimitives.ReadSingleLittleEndian(value);
        }

        public override (T, U)[] ReadTupleArray<T, U>(Func<T> func1, Func<U> func2)
        {
            throw new NotImplementedException();
        }

        public override ushort ReadUInt16()
        {
            Span<byte> value = stackalloc byte[2];

            ReadBytes(value);

            return BinaryPrimitives.ReadUInt16LittleEndian(value);
        }

        public override uint ReadUInt32()
        {
            Span<byte> value = stackalloc byte[4];

            ReadBytes(value);

            return BinaryPrimitives.ReadUInt32LittleEndian(value);
        }

        public override bool ReadUInt32AsBoolean()
        {
            throw new NotImplementedException();
        }

        public override T ReadUInt32AsEnum<T>()
        {
            throw new NotImplementedException();
        }

        public override ulong ReadUInt64()
        {
            Span<byte> value = stackalloc byte[8];

            ReadBytes(value);

            return BinaryPrimitives.ReadUInt64LittleEndian(value);
        }

        /// <summary>
        /// Sets <see cref="Position"/> within current BitArray.
        /// </summary>
        /// <param name="offset">The offset relative to the <paramref name="seekOrigin"/>.</param>
        /// <param name="seekOrigin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override void Seek(int offset, SeekOrigin seekOrigin = SeekOrigin.Begin)
        {
            if (offset < 0 || offset > LastBit || (seekOrigin == SeekOrigin.Current && offset + _position > LastBit))
            {
                throw new ArgumentOutOfRangeException("Specified offset doesnt fit within the BitArray buffer");
            }

            _ = (seekOrigin switch
            {
                SeekOrigin.Begin => _position = offset,
                SeekOrigin.End => _position = LastBit - offset,
                SeekOrigin.Current => _position += offset,
                _ => _position = offset,
            });
        }

        public override void SkipBytes(uint byteCount)
        {
            SkipBytes((int)byteCount);
        }

        public override void SkipBytes(int byteCount)
        {
            Seek(byteCount * 8, SeekOrigin.Current);
        }

        public override void SkipBits(int numbits)
        {
            _position += numbits;

            if (numbits < 0 || _position > LastBit)
            {
                IsError = true;

                _position = LastBit;
            }
        }

        /// <summary>
        /// Save Position to <see cref="MarkPosition"/> so we can reset back to this point.
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Public/Serialization/BitReader.h#L228
        /// </summary>
        public override void Mark()
        {
            MarkPosition = _position;
        }

        /// <summary>
        /// Set Position back to <see cref="MarkPosition"/>
        /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Core/Public/Serialization/BitReader.h#L228
        /// </summary>
        public override void Pop()
        {
            // TODO: pop makes it sound like a list...
            _position = MarkPosition;
        }

        /// <summary>
        /// Get number of bits left, including any bits after <see cref="LastBit"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetBitsLeft()
        {
            return LastBit - _position;
        }

        /// <summary>
        /// Append bool array to this archive.
        /// </summary>
        /// <param name="data"></param>
        public override void AppendDataFromChecked(ReadOnlyMemory<bool> data)
        {
            LastBit += data.Length;
            Bits.Append(data);
        }

        public override void Dispose()
        {
            Bits.Dispose();
        }

        public void SetTempEnd(int totalBits, int index = 0)
        {
            _tempLastBit[index] = LastBit;
            _tempPosition[index] = _position + totalBits;
            LastBit = _position + totalBits;
        }

        public void RestoreTemp(int index = 0)
        {
            LastBit = _tempLastBit[index];
            _position = _tempPosition[index];

            /*
            _tempLastBit = 0;
            _tempPosition = 0;
            */

            IsError = false;
        }
    }
}
