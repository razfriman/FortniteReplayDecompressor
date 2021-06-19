﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unreal.Core.Contracts;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Models
{
    //Purely for debugging. It's not optimized
    public class DebuggingObject : IProperty
    {
        private enum VectorType { Normal, Vector10, Vector100, Quantize, Single };
        private enum RotatorType { Byte, Short };

        public byte[] Bytes => AsByteArray();
        public int TotalBits { get; private set; }
        public bool? BoolValue => TotalBits == 1 ? _reader.PeekBit() : new bool?();
        public int? ShortValue => Bytes.Length == 2 ? BitConverter.ToInt16(Bytes, 0) : new short?();
        public float? FloatValue => Bytes.Length == 4 ? BitConverter.ToSingle(Bytes, 0) : new float?();
        public int? IntValue => Bytes.Length == 4 ? BitConverter.ToInt32(Bytes, 0) : new int?();
        public long? LongValue => Bytes.Length == 8 ? BitConverter.ToInt64(Bytes, 0) : new long?();
        public int? ByteValue => Bytes.Length == 1 ? Bytes[0] : new byte?();
        public string NetId => AsNetId();
        public uint? PropertyObject => AsPropertyObject();
        public int? Enum => AsEnum();

        public List<IProperty> PotentialProperties => AsPotentialPropeties();
        public List<DebuggingHandle> PossibleExport => AsExportHandle();

        public FVector? QuantizeVector => AsVector(VectorType.Quantize);
        public FVector? VectorNormal => AsVector(VectorType.Normal);
        public FVector? VectorSingle => AsVector(VectorType.Single);
        public FVector? Vector10 => AsVector(VectorType.Vector10);
        public FVector? Vector100 => AsVector(VectorType.Vector100);
        public FRotator RotatorByte => AsRotator(RotatorType.Byte);
        public FRotator RotatorShort => AsRotator(RotatorType.Short);
        public FRepMovement FRepMovementWholeNumber => AsFRepMovement(VectorQuantization.RoundWholeNumber);
        public FRepMovement FRepMovementTwoDecimal => AsFRepMovement(VectorQuantization.RoundTwoDecimals);

        //public FRepMovement RepMovement => AsRepMovement();
        public string AsciiString => Encoding.ASCII.GetString(Bytes);
        public string UnicodeString => Encoding.ASCII.GetString(Bytes);

        public string FString => AsFString();
        public string StaticName => AsStaticName();
        public object[] Array => AsArray();
        public bool DidError => _reader.IsError;

        private static IEnumerable<Type> _propertyTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(IProperty).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

        private NetBitReader _reader;

        public void Serialize(NetBitReader reader)
        {
            _reader = reader.GetNetBitReader(reader.GetBitsLeft());

            TotalBits = _reader.GetBitsLeft();
        }

        private byte[] AsByteArray()
        {
            _reader.Reset();

            return _reader.ReadBytes((int)Math.Floor(_reader.GetBitsLeft() / 8.0));
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

        private FRotator AsRotator(RotatorType type)
        {
            _reader.Reset();

            FRotator rotator = null;

            switch (type)
            {
                case RotatorType.Byte:
                    rotator = _reader.ReadRotation();
                    break;
                case RotatorType.Short:
                    rotator = _reader.ReadRotationShort();
                    break;
            }


            if (_reader.IsError || !_reader.AtEnd())
            {
                return null;
            }

            return rotator;
        }

        private FVector? AsVector(VectorType type)
        {
            _reader.Reset();

            FVector? tVector = null;

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
                case VectorType.Single:
                    tVector = _reader.SerializePropertyVector();
                    break;
            }

            if (_reader.IsError || !_reader.AtEnd())
            {
                return null;
            }

            return tVector;
        }

        /*
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
        */

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

                return UnrealNameConstants.Names[nameIndex];
            }

            var inString = _reader.ReadFString();
            var inNumber = _reader.ReadInt32();

            if(_reader.IsError || !_reader.AtEnd())
            {
                return null;
            }

            return inString;
        }

        private object[] AsArray()
        {
            _reader.Reset();

            uint totalElements = _reader.ReadIntPacked();

            object[] data = new object[totalElements];


            while (true)
            {
                uint index = _reader.ReadIntPacked();

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

                    if (_reader.IsError || !_reader.AtEnd())
                    {
                        return null;
                    }

                    return data;
                }

                --index;

                if(index >= totalElements)
                {
                    return null;
                }

                List<DebuggingHandle> handles = new List<DebuggingHandle>();
                bool isExportHandles = false;

                while(true)
                {
                    DebuggingHandle debuggingHandle = new DebuggingHandle();

                    uint handle = _reader.ReadIntPacked();

                    debuggingHandle.Handle = handle;

                    if(handle == 0)
                    {
                        break;
                    }

                    --handle;

                    uint numBits = _reader.ReadIntPacked();

                    debuggingHandle.NumBits = numBits;

                    DebuggingObject obj = new DebuggingObject();

                    using NetBitReader tempReader = _reader.GetNetBitReader((int)numBits);
                    tempReader.EngineNetworkVersion = _reader.EngineNetworkVersion;

                    obj.Serialize(tempReader);

                    data[index] = obj;

                    handles.Add(debuggingHandle);
                }

                //Assume it's an export handle
                if(handles.Count > 0)
                {
                    data[index] = handles;
                }
            }
        }

        private List<DebuggingHandle> AsExportHandle()
        {
            List<DebuggingHandle> handles = new List<DebuggingHandle>();

            _reader.Reset();

            while (true)
            {
                var handle = _reader.ReadIntPacked();

                if (handle == 0)
                {
                    if(_reader.IsError || !_reader.AtEnd())
                    {
                        return null;
                    }

                    break;
                }

                handle--;

                var numBits = _reader.ReadIntPacked();

                DebuggingHandle debuggingHandle = new DebuggingHandle
                {
                    Handle = handle,
                    NumBits = numBits
                };

                handles.Add(debuggingHandle);

                if (numBits == 0)
                {
                    continue;
                }

                _reader.SkipBits((int)numBits);

                if(_reader.IsError)
                {
                    return null;
                }
            }

            return handles;
        }

        private List<IProperty> AsPotentialPropeties()
        {
            List<IProperty> possibleProperties = new List<IProperty>();

            foreach(Type type in _propertyTypes)
            {
                if(type == typeof(DebuggingObject))
                {
                    continue;
                }

                _reader.Reset();

                IProperty iProperty = (IProperty)Activator.CreateInstance(type);

                try
                {
                    iProperty.Serialize(_reader);

                    if (_reader.IsError || !_reader.AtEnd())
                    {
                        continue;
                    }

                    possibleProperties.Add(iProperty);
                }
                catch
                {
                    //Ignore
                }
            }

            return possibleProperties;
        }

        private uint? AsPropertyObject()
        {
            _reader.Reset();

            uint obj = _reader.SerializePropertyObject();

            if (_reader.IsError || !_reader.AtEnd())
            {
                return null;
            }

            return obj;
        }

        private int? AsEnum()
        {
            _reader.Reset();

            return _reader.SerializePropertyEnum(_reader.GetBitsLeft());
        }

        private FRepMovement AsFRepMovement(VectorQuantization vectorQuantization)
        {
            _reader.Reset();

            FRepMovement movement  = _reader.SerializeRepMovement(vectorQuantization);

            if(_reader.IsError || !_reader.AtEnd())
            {
                return null;
            }

            return movement;
        }
    }

    public class DebuggingHandle
    { 
        public uint NumBits { get; set; }
        public uint Handle { get; set; }

        public override string ToString()
        {
            return $"Handle: {Handle} NumBits: {NumBits}";
        }
    }
}
