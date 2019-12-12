using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Contracts;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Models
{
    //Purely for debugging. It's not optimized
    public class DebuggingObject : IProperty
    {
        private enum VectorType { Normal, Vector10, Vector100, Quantize };

        public byte[] Bytes => AsByteArray();
        public int TotalBits { get; private set; }
        public bool? BoolValue => TotalBits == 1 ? _reader.PeekBit() : new bool?();
        public int? ShortValue => Bytes.Length == 2 ? BitConverter.ToInt16(Bytes, 0) : new short?();
        public float? FloatValue => Bytes.Length == 4 ? BitConverter.ToSingle(Bytes, 0) : new float?();
        public int? IntValue => Bytes.Length == 4 ? BitConverter.ToInt32(Bytes, 0) : new int?();
        public long? LongValue => Bytes.Length == 8 ? BitConverter.ToInt64(Bytes, 0) : new long?();
        public int? ByteValue => Bytes.Length == 1 ? Bytes[0] : new byte?();
        public string NetId => AsNetId();
        public FVector QuantizeVector => AsVector(VectorType.Quantize);
        public FVector VectorNormal => AsVector(VectorType.Normal);
        public FVector Vector10 => AsVector(VectorType.Vector10);
        public FVector Vector100 => AsVector(VectorType.Vector100);
        public FRepMovement RepMovement => AsRepMovement();
        public string AsciiString => Encoding.ASCII.GetString(Bytes);
        public string UnicodeString => Encoding.ASCII.GetString(Bytes);

        public string FString => AsFString();
        public string StaticName => AsStaticName();
        public DebuggingObject[] Array => AsArray();
        public bool DidError => _reader.IsError;

        private NetBitReader _reader;

        public void Serialize(NetBitReader reader)
        {
            _reader = new NetBitReader(reader.ReadBits(reader.GetBitsLeft()));
            _reader.EngineNetworkVersion = reader.EngineNetworkVersion;

            TotalBits = _reader.GetBitsLeft();
        }

        private byte[] AsByteArray()
        {
            _reader.Reset();

            return _reader.ReadBytes((int)Math.Ceiling(_reader.GetBitsLeft() / 8.0));
        }

        private string AsFString()
        {
            _reader.Reset();

            return _reader.ReadFString();
        }

        private string AsNetId()
        {
            _reader.Reset();

            string netId = _reader.SerializePropertyNetId();

            if(_reader.IsError || !_reader.AtEnd())
            {
                return null;
            }

            return netId;
        }

        private FVector AsVector(VectorType type)
        {
            _reader.Reset();

            FVector tVector = null;

            switch (type)
            {
                case VectorType.Normal:
                    tVector = _reader.SerializePropertyVectorNormal();
                    break;
                case VectorType.Vector10:
                    tVector = _reader.SerializePropertyVector10();
                    break;
                case VectorType.Vector100:
                    tVector = _reader.SerializePropertyVector100();
                    break;
                case VectorType.Quantize:
                    tVector = _reader.SerializePropertyQuantizeVector();
                    break;
            }

            if (_reader.IsError || !_reader.AtEnd())
            {
                return null;
            }

            return tVector;
        }

        private FRepMovement AsRepMovement()
        {
            _reader.Reset();

            FRepMovement repMovement = _reader.SerializeRepMovement();

            if(_reader.IsError || !_reader.AtEnd())
            {
                return null;
            }

            return repMovement;
        }

        private string AsStaticName()
        {
            _reader.Reset();

            var isHardcoded = _reader.ReadBoolean();

            if (isHardcoded)
            {
                uint nameIndex;
                if (_reader.EngineNetworkVersion < EngineNetworkVersionHistory.HISTORY_CHANNEL_NAMES)
                {
                    nameIndex = _reader.ReadUInt32();
                }
                else
                {
                    nameIndex = _reader.ReadIntPacked();
                }

                if(_reader.IsError || !_reader.AtEnd())
                {
                    return null;
                }

                return ((UnrealNames)nameIndex).ToString();
            }

            var inString = _reader.ReadFString();
            var inNumber = _reader.ReadInt32();

            if(_reader.IsError || !_reader.AtEnd())
            {
                return null;
            }

            return inString;
        }

        private DebuggingObject[] AsArray()
        {
            _reader.Reset();

            uint totalElements = _reader.ReadIntPacked();

            DebuggingObject[] data = new DebuggingObject[totalElements];

            while (true)
            {
                uint index = _reader.ReadIntPacked();

                if (index == 0)
                {
                    if (index == 0)
                    {
                        if (_reader.GetBitsLeft() == 8)
                        {
                            uint terminator = _reader.ReadIntPacked();

                            if (terminator != 0x00)
                            {
                                //Log error
                                return null;
                            }
                        }

                        if(_reader.IsError || !_reader.AtEnd())
                        {
                            return null;
                        }

                        return data;
                    }
                }

                --index;

                if(index >= totalElements)
                {
                    return null;
                }

                while(true)
                {
                    uint handle = _reader.ReadIntPacked();

                    if(handle == 0)
                    {
                        break;
                    }

                    --handle;
                    uint numBits = _reader.ReadIntPacked();

                    DebuggingObject obj = new DebuggingObject();


                    NetBitReader tempReader = new NetBitReader(_reader.ReadBits(numBits));
                    tempReader.EngineNetworkVersion = _reader.EngineNetworkVersion;

                    obj.Serialize(tempReader);

                    data[index] = obj;
                }
            }
        }
    }
}
