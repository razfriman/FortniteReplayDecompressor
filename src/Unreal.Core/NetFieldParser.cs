using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using FastMember;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace Unreal.Core
{
    public class NetFieldParser
    {
        private static Dictionary<string, NetFieldGroupInfo> _netFieldGroups = new Dictionary<string, NetFieldGroupInfo>();
        private static Dictionary<Type, RepLayoutCmdType> _primitiveTypeLayout = new Dictionary<Type, RepLayoutCmdType>();
        private static Dictionary<string, NetRPCFieldGroupInfo> _netRPCStructureTypes = new Dictionary<string, NetRPCFieldGroupInfo>(); //Mapping from ClassNetCache -> Type path name
        private static HashSet<string> _playerControllers = new HashSet<string>(); //Player controllers require 1 extra byte to be read when creating actor
        private static CompiledLinqCache _linqCache = new CompiledLinqCache();

#if DEBUG
        public static Dictionary<string, HashSet<UnknownFieldInfo>> UnknownNetFields { get; private set; } = new Dictionary<string, HashSet<UnknownFieldInfo>>();

        public static bool HasNewNetFields => UnknownNetFields.Count > 0;
#endif

        static NetFieldParser()
        {
            Assembly[] currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            IEnumerable<Type> netFields = currentAssemblies.SelectMany(x => x.GetTypes()).Where(x => x.GetCustomAttribute<NetFieldExportGroupAttribute>() != null);

            foreach (Type type in netFields)
            {
                NetFieldExportGroupAttribute attribute = type.GetCustomAttribute<NetFieldExportGroupAttribute>();
                PlayerControllerAttribute playerController = type.GetCustomAttribute<PlayerControllerAttribute>();

                if(playerController != null)
                {
                    _playerControllers.Add(playerController.Name);
                }

                NetFieldGroupInfo info = new NetFieldGroupInfo();

                info.Type = type;
                info.Attribute = attribute;

                _netFieldGroups[attribute.Path] = info;

                foreach (PropertyInfo property in type.GetProperties())
                {
                    NetFieldExportAttribute netFieldExportAttribute = property.GetCustomAttribute<NetFieldExportAttribute>(); //Uses name to determine property
                    NetFieldExportHandleAttribute netFieldExportHandleAttribute = property.GetCustomAttribute<NetFieldExportHandleAttribute>(); //Uses handle id

                    if (netFieldExportAttribute == null && netFieldExportHandleAttribute == null)
                    {
                        continue;
                    }

                    if (netFieldExportAttribute != null)
                    {
                        info.Properties[netFieldExportAttribute.Name] = new NetFieldInfo
                        {
                            Attribute = netFieldExportAttribute,
                            PropertyInfo = property
                        };
                    }
                    else
                    {
                        info.UsesHandles = true;

                        info.HandleProperties[netFieldExportHandleAttribute.Handle] = new NetFieldInfo
                        {
                            Attribute = netFieldExportHandleAttribute,
                            PropertyInfo = property
                        };
                    }
                }
            }

            //Class net cache
            IEnumerable<Type> netCache = currentAssemblies.SelectMany(x => x.GetTypes()).Where(x => x.GetCustomAttribute<NetFieldExportRPCAttribute>() != null);

            foreach (Type type in netCache)
            {
                NetFieldExportRPCAttribute attribute = type.GetCustomAttribute<NetFieldExportRPCAttribute>();
                NetRPCFieldGroupInfo info = new NetRPCFieldGroupInfo();
                info.ParseType = attribute.MinimumParseType;

                _netRPCStructureTypes[attribute.PathName] = info;

                foreach (PropertyInfo property in type.GetProperties())
                {
                    NetFieldExportRPCPropertyAttribute propertyAttribute = property.GetCustomAttribute<NetFieldExportRPCPropertyAttribute>();

                    if (propertyAttribute != null)
                    {
                        info.PathNames.TryAdd(propertyAttribute.Name, new NetRPCFieldInfo
                        {
                            PropertyInfo = property,
                            Attribute = propertyAttribute,
                            IsCustomStructure = propertyAttribute.CustomStructure
                        });
                    }
                }
            }

            //Type layout for dynamic arrays
            _primitiveTypeLayout.Add(typeof(bool), RepLayoutCmdType.PropertyBool);
            _primitiveTypeLayout.Add(typeof(byte), RepLayoutCmdType.PropertyByte);
            _primitiveTypeLayout.Add(typeof(ushort), RepLayoutCmdType.PropertyUInt16);
            _primitiveTypeLayout.Add(typeof(int), RepLayoutCmdType.PropertyInt);
            _primitiveTypeLayout.Add(typeof(uint), RepLayoutCmdType.PropertyUInt32);
            _primitiveTypeLayout.Add(typeof(ulong), RepLayoutCmdType.PropertyUInt64);
            _primitiveTypeLayout.Add(typeof(float), RepLayoutCmdType.PropertyFloat);
            _primitiveTypeLayout.Add(typeof(string), RepLayoutCmdType.PropertyString);
            _primitiveTypeLayout.Add(typeof(object), RepLayoutCmdType.Ignore);

            IEnumerable<Type> iPropertyTypes = currentAssemblies.SelectMany(x => x.GetTypes())
                .Where(x => typeof(IProperty).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            //Allows deserializing IProperty type arrays
            foreach (var iPropertyType in iPropertyTypes)
            {
                _primitiveTypeLayout.Add(iPropertyType, RepLayoutCmdType.Property);
            }
        }

        public static bool IsPlayerController(string name)
        {
            return _playerControllers.Contains(name);
        }

        public static string GetClassNetPropertyPathname(string netCache, string property, out bool readChecksumBit)
        {
            readChecksumBit = false;

            if (_netRPCStructureTypes.TryGetValue(netCache, out NetRPCFieldGroupInfo netCacheFieldGroupInfo))
            {
                if(netCacheFieldGroupInfo.PathNames.TryGetValue(property, out NetRPCFieldInfo rpcAttribute))
                {
                    readChecksumBit = rpcAttribute.Attribute.ReadChecksumBit;

                    return rpcAttribute.Attribute.TypePathName;
                }
                else
                {
                    //Debugging
                }
            }
            else
            {
                //Debugging
            }

            return null;
        }

        public static bool TryGetNetFieldGroupRPC(string classNetPathName, string property, ParseType parseType, out NetRPCFieldInfo netFieldInfo, out bool willParse)
        {
            willParse = false;
            netFieldInfo = null;

            if (_netRPCStructureTypes.TryGetValue(classNetPathName, out NetRPCFieldGroupInfo groups))
            {
                willParse = parseType >= groups.ParseType;

                if(!willParse)
                {
                    return true;
                }

                if(groups.PathNames.TryGetValue(property, out NetRPCFieldInfo netFieldExportRPCPropertyAttribute))
                {
                    netFieldInfo = netFieldExportRPCPropertyAttribute;

                    return true;
                }
            }

            return false;
        }

        public static bool WillReadType(string group, ParseType parseType, out bool ignoreChannel)
        {
            ignoreChannel = false;

            if (_netFieldGroups.ContainsKey(group))
            {
                if (parseType >= _netFieldGroups[group].Attribute.MinimumParseType)
                {
                    return true;
                }

                //Ignore channels where we know the type and outside the parse mode
                ignoreChannel = true;

                return false;
            }

            return false;
        }

        public static void ReadField(object obj, NetFieldExport export, NetFieldExportGroup exportGroup, uint handle, NetBitReader netBitReader)
        {
            string group = exportGroup.PathName;

            string fixedExportName = FixInvalidNames(export.Name);

            bool isDebug = obj is DebuggingExportGroup;

            if (isDebug)
            {
                group = "DebuggingExportGroup";
                fixedExportName = "Handles";
            }

            if (!_netFieldGroups.TryGetValue(group, out NetFieldGroupInfo netGroupInfo))
            {
                return;
            }

            NetFieldInfo netFieldInfo = null;

            if ((!netGroupInfo.UsesHandles && !netGroupInfo.Properties.TryGetValue(fixedExportName, out netFieldInfo)) ||
                (netGroupInfo.UsesHandles && !netGroupInfo.HandleProperties.TryGetValue(handle, out netFieldInfo)))
            {
                //Clean this up
                if (obj is IHandleNetFieldExportGroup handleGroup)
                {
                    DebuggingObject data = (DebuggingObject)ReadDataType(RepLayoutCmdType.Property, netBitReader, typeof(DebuggingObject));
                    handleGroup.UnknownHandles.Add(handle, data);
                }

                return;
            }

            SetType(obj, netFieldInfo, netGroupInfo, exportGroup, handle, netBitReader);
        }

        private static void SetType(object obj, NetFieldInfo netFieldInfo, NetFieldGroupInfo groupInfo, NetFieldExportGroup exportGroup, uint handle, NetBitReader netBitReader)
        {
            object data;

            switch (netFieldInfo.Attribute.Type)
            {
                case RepLayoutCmdType.DynamicArray:
                    data = ReadArrayField(exportGroup, netFieldInfo, groupInfo, netBitReader);
                    break;
                default:
                    data = ReadDataType(netFieldInfo.Attribute.Type, netBitReader, netFieldInfo.PropertyInfo.PropertyType);
                    break;
            }


            if (obj is DebuggingExportGroup debugGroup)
            {
                debugGroup.Handles.Add(handle, data as DebuggingObject);

                return;
            }

            if (data != null)
            {
                TypeAccessor typeAccessor = TypeAccessor.Create(obj.GetType());
                typeAccessor[obj, netFieldInfo.PropertyInfo.Name] = data;
            }
        }

        private static object ReadDataType(RepLayoutCmdType replayout, NetBitReader netBitReader, Type objectType = null)
        {
            object data = null;

            switch (replayout)
            {
                case RepLayoutCmdType.Property:
                case RepLayoutCmdType.RepMovement:
                    data = _linqCache.CreateObject(objectType);
                    (data as IProperty).Serialize(netBitReader);
                    break;
                case RepLayoutCmdType.PropertyBool:
                    data = netBitReader.SerializePropertyBool();
                    break;
                case RepLayoutCmdType.PropertyName:
                    data = netBitReader.SerializePropertyName();
                    break;
                case RepLayoutCmdType.PropertyFloat:
                    data = netBitReader.SerializePropertyFloat();
                    break;
                case RepLayoutCmdType.PropertyNativeBool:
                    data = netBitReader.SerializePropertyNativeBool();
                    break;
                case RepLayoutCmdType.PropertyNetId:
                    data = netBitReader.SerializePropertyNetId();
                    break;
                case RepLayoutCmdType.PropertyObject:
                    data = netBitReader.SerializePropertyObject();
                    break;
                case RepLayoutCmdType.PropertyPlane:
                    throw new NotImplementedException("Plane RepLayoutCmdType not implemented");
                case RepLayoutCmdType.PropertyRotator:
                    data = netBitReader.SerializePropertyRotator();
                    break;
                case RepLayoutCmdType.PropertyString:
                    data = netBitReader.SerializePropertyString();
                    break;
                case RepLayoutCmdType.PropertyVector10:
                    data = netBitReader.SerializePropertyVector10();
                    break;
                case RepLayoutCmdType.PropertyVector100:
                    data = netBitReader.SerializePropertyVector100();
                    break;
                case RepLayoutCmdType.PropertyVectorNormal:
                    data = netBitReader.SerializePropertyVectorNormal();
                    break;
                case RepLayoutCmdType.PropertyVectorQ:
                    data = netBitReader.SerializePropertyQuantizeVector();
                    break;
                case RepLayoutCmdType.Enum:
                    data = netBitReader.SerializeEnum();
                    break;
                case RepLayoutCmdType.PropertyByte:
                    data = (byte)netBitReader.ReadBitsToInt(netBitReader.GetBitsLeft());
                    break;
                case RepLayoutCmdType.PropertyInt:
                    data = netBitReader.ReadInt32();
                    break;
                case RepLayoutCmdType.PropertyUInt64:
                    data = netBitReader.ReadUInt64();
                    break;
                case RepLayoutCmdType.PropertyInt16:
                    data = netBitReader.ReadInt16();
                    break;
                case RepLayoutCmdType.PropertyUInt16:
                    data = netBitReader.ReadUInt16();
                    break;
                case RepLayoutCmdType.PropertyUInt32:
                    data = netBitReader.ReadUInt32();
                    break;
                case RepLayoutCmdType.PropertyVector:
                    data = netBitReader.SerializePropertyVector();
                    break;
                case RepLayoutCmdType.Ignore:
                    netBitReader.Seek(netBitReader.GetBitsLeft(), SeekOrigin.Current);
                    break;
                case RepLayoutCmdType.Debug:
                    data = _linqCache.CreateObject(typeof(DebuggingObject));
                    (data as IProperty).Serialize(netBitReader);
                    break;
            }

            return data;
        }

        private static Array ReadArrayField(NetFieldExportGroup netfieldExportGroup, NetFieldInfo fieldInfo, NetFieldGroupInfo groupInfo, NetBitReader netBitReader)
        {
            uint arrayIndexes = netBitReader.ReadIntPacked();

            Type elementType = fieldInfo.PropertyInfo.PropertyType.GetElementType();
            RepLayoutCmdType replayout = RepLayoutCmdType.Ignore;
            bool isGroupType = elementType == groupInfo.Type || elementType == groupInfo.Type.BaseType;

            if (!isGroupType)
            {
                groupInfo = null;

                if (!_primitiveTypeLayout.TryGetValue(elementType, out replayout))
                {
                    replayout = RepLayoutCmdType.Ignore;
                }
            }

            Array arr = Array.CreateInstance(elementType, arrayIndexes);

            while (true)
            {
                uint index = netBitReader.ReadIntPacked();

                if (index == 0)
                {
                    if (netBitReader.GetBitsLeft() == 8)
                    {
                        uint terminator = netBitReader.ReadIntPacked();

                        if (terminator != 0x00)
                        {
                            //Log error

                            return arr;
                        }
                    }

                    return arr;
                }

                --index;

                if (index >= arrayIndexes)
                {
                    //Log error

                    return arr;
                }

                object data = null;

                if (isGroupType)
                {
                    data = _linqCache.CreateObject(elementType);
                }

                while (true)
                {
                    uint handle = netBitReader.ReadIntPacked();

                    if (handle == 0)
                    {
                        break;
                    }

                    handle--;

                    if (netfieldExportGroup.NetFieldExports.Length < handle)
                    {
                        return arr;
                    }

                    NetFieldExport export = netfieldExportGroup.NetFieldExports[handle];
                    uint numBits = netBitReader.ReadIntPacked();

                    if (numBits == 0)
                    {
                        continue;
                    }

                    if (export == null)
                    {
                        netBitReader.SkipBits((int)numBits);

                        continue;
                    }

                    NetBitReader cmdReader = new NetBitReader(netBitReader.ReadBits(numBits))
                    {
                        EngineNetworkVersion = netBitReader.EngineNetworkVersion,
                        NetworkVersion = netBitReader.NetworkVersion
                    };

                    //Uses the same type for the array
                    if (groupInfo != null)
                    {
                        ReadField(data, export, netfieldExportGroup, handle, cmdReader);
                    }
                    else //Probably primitive values
                    {
                        data = ReadDataType(replayout, cmdReader, elementType);
                    }
                }

                arr.SetValue(data, index);
            }
        }

        public static INetFieldExportGroup CreateType(string group)
        {
            if (!_netFieldGroups.ContainsKey(group))
            {
                return null;
            }

            return (INetFieldExportGroup)_linqCache.CreateObject(_netFieldGroups[group].Type);
        }

        /// <summary>
        /// Create the object associated with the property that should be read.
        /// Used as a workaround for RPC structs.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool TryCreateRPCPropertyType(string group, string propertyName, out IProperty property)
        {
            property = null;

            if (_netRPCStructureTypes.TryGetValue(group, out NetRPCFieldGroupInfo groupInfo))
            {
                if(groupInfo.PathNames.TryGetValue(propertyName, out NetRPCFieldInfo fieldInfo))
                {
                    property = (IProperty)_linqCache.CreateObject(fieldInfo.PropertyInfo.PropertyType);

                    return true;
                }
            }

            return false;
        }

        private static unsafe string FixInvalidNames(string str)
        {
            int len = str.Length;
            char* newChars = stackalloc char[len];
            char* currentChar = newChars;

            for (int i = 0; i < len; ++i)
            {
                char c = str[i];

                bool isDigit = (c ^ '0') <= 9;

                byte val = (byte)((c & 0xDF) - 0x40);
                bool isChar = val > 0 && val <= 26;

                if (isDigit || isChar)
                {
                    *currentChar++ = c;
                }
            }

            return new string(newChars, 0, (int)(currentChar - newChars));
        }

#if DEBUG
        private static void AddUnknownField(string group, UnknownFieldInfo fieldInfo)
        {
            HashSet<UnknownFieldInfo> fields = new HashSet<UnknownFieldInfo>();

            if (!UnknownNetFields.TryAdd(group, fields))
            {
                UnknownNetFields.TryGetValue(group, out fields);
            }

            fields.Add(fieldInfo);
        }

        private static void AddUnknownField(string exportName, string exportType, string group, uint handle, NetBitReader netBitReader)
        {
            HashSet<UnknownFieldInfo> fields = new HashSet<UnknownFieldInfo>();

            if (!UnknownNetFields.TryAdd(group, fields))
            {
                UnknownNetFields.TryGetValue(group, out fields);
            }

            fields.Add(new UnknownFieldInfo(exportName, exportType, netBitReader.GetBitsLeft(), handle));

        }
#endif

        private class NetFieldGroupInfo
        {
            public NetFieldExportGroupAttribute Attribute { get; set; }
            public Type Type { get; set; }
            public bool UsesHandles { get; set; }
            public Dictionary<string, NetFieldInfo> Properties { get; set; } = new Dictionary<string, NetFieldInfo>();
            public Dictionary<uint, NetFieldInfo> HandleProperties { get; set; } = new Dictionary<uint, NetFieldInfo>();
        }

        private class NetFieldInfo
        {
            public RepLayoutAttribute Attribute { get; set; }

            public PropertyInfo PropertyInfo { get; set; }
        }
    }

#if DEBUG
    public class NetRPCFieldInfo
    {
        public NetFieldExportRPCPropertyAttribute Attribute { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public bool IsCustomStructure { get; set; }
    }

    public class NetRPCFieldGroupInfo
    {
        public ParseType ParseType { get; set; }
        public Dictionary<string, NetRPCFieldInfo> PathNames { get; set; } = new Dictionary<string, NetRPCFieldInfo>();
    }

    public class UnknownFieldInfo
    {
        public string PropertyName { get; set; }
        public string Type { get; set; }
        public int BitCount { get; set; }
        public uint Handle { get; set; }

        public override string ToString()
        {
            return PropertyName;
        }

        public UnknownFieldInfo(string propertyname, string type, int bitCount, uint handle)
        {
            PropertyName = propertyname;
            Type = type;
            BitCount = bitCount;
            Handle = handle;
        }

        public override bool Equals(object obj)
        {
            UnknownFieldInfo fieldInfo = obj as UnknownFieldInfo;

            if (fieldInfo == null)
            {
                return base.Equals(obj);
            }

            return fieldInfo.PropertyName == PropertyName;
        }

        public override int GetHashCode()
        {
            return PropertyName.GetHashCode();
        }
    }
#endif
}
