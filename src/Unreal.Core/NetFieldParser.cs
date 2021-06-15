using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;
using Unreal.Core.Extensions;
using System.Reflection.Emit;

namespace Unreal.Core
{
    public sealed class NetFieldParser
    {
        private static Dictionary<string, NetFieldParserInfo> _parserInfoDict = new Dictionary<string, NetFieldParserInfo>();

        private NetFieldParserInfo _parserInfo;

        internal NetFieldParser(Assembly callingAssembly)
        {
            lock (_parserInfoDict)
            {
                if (_parserInfoDict.TryGetValue(callingAssembly.FullName, out NetFieldParserInfo info))
                {
                    //Already intialized data
                    _parserInfo = info;
                    return;
                }

                Dictionary<string, Assembly> allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(x => x.FullName, x => x);

                HashSet<Assembly> referencedAssemblies = GetAllReferencedAssemblies(callingAssembly, allAssemblies);

                referencedAssemblies.Add(callingAssembly);

                IEnumerable<Type> allTypes = referencedAssemblies.SelectMany(x => x.GetTypes());

                //UpdateFiles(allTypes.Where(x => typeof(INetFieldExportGroup).IsAssignableFrom(x)));

                List<Type> netFields = new List<Type>();
                List<Type> classNetCaches = new List<Type>();
                List<Type> propertyTypes = new List<Type>();

                //Loads all types from game reader assembly. Currently no support for referenced assemblies (TODO)
                netFields.AddRange(allTypes.Where(x => x.GetCustomAttribute<NetFieldExportGroupAttribute>(false) != null));
                classNetCaches.AddRange(allTypes.Where(x => x.GetCustomAttribute<NetFieldExportRPCAttribute>(false) != null));
                propertyTypes.AddRange(allTypes.Where(x => typeof(IProperty).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract));

                _parserInfo = new NetFieldParserInfo();
                _parserInfoDict.Add(callingAssembly.FullName, _parserInfo);

                LoadNetFields(netFields);
                LoadClassNetCaches(classNetCaches);
                LoadPropertyTypes(propertyTypes);
            }
        }

        internal bool SetMinimalParseType(string path, ParseType type)
        {
            if (!_parserInfo.NetFieldGroups.TryGetValue(path, out NetFieldGroupInfo netFieldGroup))
            {
                return false;
            }

            netFieldGroup.Attribute.MinimumParseType = type;

            return true;
        }

        //TODO: FIX
        internal Dictionary<string, ParseType> GetNetFieldExportTypes()
        {
            return _parserInfo.NetFieldGroups.ToDictionary(x => x, x => x.Attribute.MinimumParseType);
        }

        private static Action<object, object> CreateSetter(PropertyInfo propertyInfo)
        {
            FieldInfo field = GetBackingField(propertyInfo);

            string methodName = field.ReflectedType.FullName + ".set_" + field.Name;
            DynamicMethod setterMethod = new DynamicMethod(methodName, null, new Type[2] { typeof(object), typeof(object) }, true);
            ILGenerator gen = setterMethod.GetILGenerator();
            if (field.IsStatic)
            {
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Stsfld, field);
            }
            else
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Unbox_Any, field.FieldType);
                gen.Emit(OpCodes.Stfld, field);
            }
            gen.Emit(OpCodes.Ret);

            return (Action<object, object>)setterMethod.CreateDelegate(typeof(Action<object, object>));

            FieldInfo GetBackingField(PropertyInfo property)
            {
                return property.DeclaringType.GetField($"<{property.Name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            }
        }

#if DEBUG

        public static string CreateFileData(DebuggingExportGroup debuggingGroup)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"\t[NetFieldExportGroup(\"{debuggingGroup.ExportGroup.PathName}\", ParseType.Normal)]");
            builder.AppendLine($"\tpublic class {debuggingGroup.ExportGroup.PathName.Split('/').Last()} : INetFieldExportGroup");
            builder.AppendLine("\t{");

            HashSet<string> alreadyAdded = new HashSet<string>();

            foreach (KeyValuePair<uint, string> kvp in debuggingGroup.HandleNames)
            {
                if (alreadyAdded.Contains(kvp.Value))
                {
                    continue;
                }

                builder.AppendLine($"\t\t[NetFieldExport(\"{kvp.Value}\", RepLayoutCmdType.Property)]");
                builder.AppendLine($"\t\tpublic DebuggingObject {kvp.Value} {{ get; set; }}");
                builder.AppendLine($"");

                alreadyAdded.Add(kvp.Value);
            }

            builder.AppendLine("\t\tpublic override bool ManualRead(string property, object value)");
            builder.AppendLine("\t\t{");
            builder.AppendLine("\t\t\tswitch(property)");
            builder.AppendLine("\t\t\t{");

            foreach (string properties in alreadyAdded)
            {
                builder.AppendLine($"\t\t\t\tcase \"{properties}\":");
                builder.AppendLine($"\t\t\t\t\t{properties} = (DebuggingObject)value;");
                builder.AppendLine($"\t\t\t\t\tbreak;");
            }

            builder.AppendLine("\t\t\t\tdefault:");
            builder.AppendLine("\t\t\t\t\treturn base.ManualRead(property, value);");
            builder.AppendLine("\t\t\t}");
            builder.AppendLine();
            builder.AppendLine("\t\t\treturn true;");
            builder.AppendLine("\t\t}");
            builder.AppendLine("\t}");

            return builder.ToString();
        }

        private void UpdateFiles(IEnumerable<Type> types)
        {
            Dictionary<string, Type> keyValues = types.ToDictionary(x => x.Name, x => x);

            string[] allFiles = Directory.GetFiles("NetFieldExports", "*.bak", SearchOption.AllDirectories);

            foreach (string file in allFiles)
            {
                if (file.Contains("BaseStructure"))
                {

                }

                List<string> lines = File.ReadAllLines(file).ToList();

                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i];

                    Match match = Regex.Match(line, "public( abstract)? class (.*?) ");

                    if (match.Success)
                    {
                        if (!keyValues.TryGetValue(match.Groups[2].Value, out Type t))
                        {
                            continue;
                        }

                        PropertyInfo[] props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                        if (props.Length == 0)
                        {
                            continue;
                        }

                        int insertLine = FindInsertLine(lines, i);

                        if (insertLine == -1)
                        {
                            continue;
                        }

                        string text = InsertManualRead(props, out bool add);

                        if (add)
                        {
                            lines.InsertRange(insertLine, text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
                        }

                        i = insertLine; //Skip ahead
                    }
                }

                File.WriteAllLines(file.Replace(".bak", ""), lines);
            }
        }

        private int FindInsertLine(List<string> lines, int startLine)
        {
            int braceCount = 0;

            for (int i = startLine; i < lines.Count; i++)
            {
                string line = lines[i];

                switch (line.Trim())
                {
                    case "public override bool ManualRead(string property, object value)":
                        return -1;
                    case "{":
                        ++braceCount;
                        break;
                    case "}":
                        --braceCount;

                        if (braceCount == 0)
                        {
                            return i;
                        }

                        break;
                }
            }

            return -1;
        }

        private string InsertManualRead(PropertyInfo[] props, out bool addToFile)
        {
            static string GetTypeName(Type type)
            {
                if (type.Name == "Nullable`1")
                {
                    return GetTypeName(type.GenericTypeArguments[0]);
                }

                if (type == typeof(int))
                {
                    return "int";
                }
                else if (type == typeof(double))
                {
                    return "double";
                }
                else if (type == typeof(float))
                {
                    return "float";
                }
                else if (type == typeof(long))
                {
                    return "long";
                }
                else if (type == typeof(ulong))
                {
                    return "ulong";
                }
                else if (type == typeof(byte))
                {
                    return "byte";
                }
                else if (type == typeof(bool))
                {
                    return "bool";
                }
                else if (type == typeof(uint))
                {
                    return "uint";
                }
                else if (type == typeof(short))
                {
                    return "short";
                }
                else if (type == typeof(ushort))
                {
                    return "ushort";
                }
                else if (type == typeof(double))
                {
                    return "double";
                }
                else if (type == typeof(object))
                {
                    return "object";
                }
                else if (type.IsPrimitive)
                {
                    return null;
                }
                else if (type == typeof(string))
                {
                    return "string";
                }
                else if (type.IsArray)
                {
                    return $"{GetTypeName(type.GetElementType())}[]";
                }
                else
                {
                    return type.Name;
                }
            }

            addToFile = false;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine("\t\tpublic override bool ManualRead(string property, object value)");
            builder.AppendLine("\t\t{");
            builder.AppendLine("\t\t\tswitch(property)");
            builder.AppendLine("\t\t\t{");

            foreach (PropertyInfo info in props)
            {
                NetFieldExportAttribute export = info.GetCustomAttribute<NetFieldExportAttribute>();

                if (export == null)
                {
                    continue;
                }

                addToFile = true;

                builder.AppendLine($"\t\t\t\tcase \"{export.Name}\":");
                builder.AppendLine($"\t\t\t\t\t{info.Name} = {(info.PropertyType != typeof(object) ? $"({GetTypeName(info.PropertyType)})" : String.Empty)}value;");
                builder.AppendLine($"\t\t\t\t\tbreak;");
            }

            builder.AppendLine("\t\t\t\tdefault:");
            builder.AppendLine("\t\t\t\t\treturn base.ManualRead(property, value);");
            builder.AppendLine("\t\t\t}");
            builder.AppendLine();
            builder.AppendLine("\t\t\treturn true;");
            builder.AppendLine("\t\t}");

            return builder.ToString();
        }
#endif

        internal void UpdateExportGroup(NetFieldExportGroup group)
        {
            if (_parserInfo.NetFieldGroups.TryGetIndex(group.PathName, out int val))
            {
                group.GroupId = val;
            }
        }

        private HashSet<Assembly> GetAllReferencedAssemblies(Assembly assembly, Dictionary<string, Assembly> allAssemblies)
        {
            HashSet<Assembly> allAssemblyNames = new HashSet<Assembly>();

            foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
            {
                Assembly referencedAssembly = null;

                if (!allAssemblies.TryGetValue(assemblyName.FullName, out referencedAssembly))
                {
                    continue;
                }

                allAssemblyNames.Add(allAssemblies[assemblyName.FullName]);

                foreach (Assembly newAssembly in GetAllReferencedAssemblies(referencedAssembly, allAssemblies))
                {
                    allAssemblyNames.Add(newAssembly);
                }
            }

            return allAssemblyNames;
        }

        private void LoadNetFields(List<Type> netFields)
        {
            Dictionary<Type, int> _typeIds = new Dictionary<Type, int>();
            int typeId = 0;

            foreach (Type type in netFields)
            {
                _typeIds.TryAdd(type, typeId++);

                NetFieldExportGroupAttribute attribute = type.GetCustomAttribute<NetFieldExportGroupAttribute>();
                PlayerControllerAttribute playerController = type.GetCustomAttribute<PlayerControllerAttribute>();

                if (playerController != null)
                {
                    _parserInfo.PlayerControllers.Add(playerController.Name);
                }

                NetFieldGroupInfo info = new NetFieldGroupInfo();

                info.Type = type;
                info.Attribute = attribute;
                info.UsesHandles = typeof(IHandleNetFieldExportGroup).IsAssignableFrom(type);
                info.SingleInstance = typeof(ISingleInstance).IsAssignableFrom(type);

                if(info.SingleInstance)
                {
                    info.Instance = (ISingleInstance)Activator.CreateInstance(type);
                }

                _parserInfo.NetFieldGroups.Add(attribute.Path, info);

                info.TypeId = _parserInfo.LinqCache.AddExportType(info.Type);

                foreach (PropertyInfo property in type.GetProperties())
                {
                    NetFieldExportAttribute netFieldExportAttribute = property.GetCustomAttribute<NetFieldExportAttribute>(); //Uses name to determine property
                    NetFieldExportHandleAttribute netFieldExportHandleAttribute = property.GetCustomAttribute<NetFieldExportHandleAttribute>(); //Uses handle id

                    if (netFieldExportAttribute == null && netFieldExportHandleAttribute == null)
                    {
                        continue;
                    }

                    if(netFieldExportHandleAttribute != null)
                    {
                        info.HandleProperties[netFieldExportHandleAttribute.Handle] = new NetFieldInfo
                        {
                            Attribute = netFieldExportHandleAttribute,
                            PropertyInfo = property
                        };
                    }
                    else if (netFieldExportAttribute != null)
                    {
                        NetFieldInfo fieldInfo = new NetFieldInfo
                        {
                            Attribute = netFieldExportAttribute,
                            PropertyInfo = property
                        };

                        info.Properties.Add(netFieldExportAttribute.Name, fieldInfo);

                        //No reason to add ignored types
                        if (netFieldExportAttribute.Type != RepLayoutCmdType.Ignore)
                        {
                            if (property.PropertyType.IsArray)
                            {
                                Type elementType = property.PropertyType.GetElementType();

                                if (typeof(INetFieldExportGroup).IsAssignableFrom(elementType))
                                {
                                    fieldInfo.ElementTypeId = _parserInfo.LinqCache.AddExportType(elementType);
                                }
                            }

                            fieldInfo.SetMethod = CreateSetter(fieldInfo.PropertyInfo);
                        }
                    }
                }
            }
        }

        private void LoadClassNetCaches(List<Type> classNetCaches)
        {
            foreach (Type type in classNetCaches)
            {
                NetFieldExportRPCAttribute attribute = type.GetCustomAttribute<NetFieldExportRPCAttribute>();
                NetRPCFieldGroupInfo info = new NetRPCFieldGroupInfo();
                info.ParseType = attribute.MinimumParseType;

                _parserInfo.NetRPCStructureTypes[attribute.PathName] = info;

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
        }

        private void LoadPropertyTypes(List<Type> propertyTypes)
        {
            //Type layout for dynamic arrays
            _parserInfo.PrimitiveTypeLayout.Add(typeof(bool), RepLayoutCmdType.PropertyBool);
            _parserInfo.PrimitiveTypeLayout.Add(typeof(byte), RepLayoutCmdType.PropertyByte);
            _parserInfo.PrimitiveTypeLayout.Add(typeof(ushort), RepLayoutCmdType.PropertyUInt16);
            _parserInfo.PrimitiveTypeLayout.Add(typeof(int), RepLayoutCmdType.PropertyInt);
            _parserInfo.PrimitiveTypeLayout.Add(typeof(uint), RepLayoutCmdType.PropertyUInt32);
            _parserInfo.PrimitiveTypeLayout.Add(typeof(ulong), RepLayoutCmdType.PropertyUInt64);
            _parserInfo.PrimitiveTypeLayout.Add(typeof(float), RepLayoutCmdType.PropertyFloat);
            _parserInfo.PrimitiveTypeLayout.Add(typeof(string), RepLayoutCmdType.PropertyString);
            _parserInfo.PrimitiveTypeLayout.Add(typeof(object), RepLayoutCmdType.Ignore);

            //Allows deserializing IProperty type arrays
            foreach (var iPropertyType in propertyTypes)
            {
                _parserInfo.PrimitiveTypeLayout.Add(iPropertyType, RepLayoutCmdType.Property);
            }
        }

        internal bool IsPlayerController(string name)
        {
            return _parserInfo.PlayerControllers.Contains(name);
        }

        internal string GetClassNetPropertyPathname(string netCache, string property, out bool readChecksumBit)
        {
            readChecksumBit = false;

            if (_parserInfo.NetRPCStructureTypes.TryGetValue(netCache, out NetRPCFieldGroupInfo netCacheFieldGroupInfo))
            {
                if (netCacheFieldGroupInfo.PathNames.TryGetValue(property, out NetRPCFieldInfo rpcAttribute))
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

        internal bool TryGetNetFieldGroupRPC(string classNetPathName, string property, ParseType parseType, out NetRPCFieldInfo netFieldInfo, out bool willParse)
        {
            willParse = false;
            netFieldInfo = null;

            if (_parserInfo.NetRPCStructureTypes.TryGetValue(classNetPathName, out NetRPCFieldGroupInfo groups))
            {
                willParse = parseType >= groups.ParseType;

                if (!willParse)
                {
                    return true;
                }

                if (groups.PathNames.TryGetValue(property, out NetRPCFieldInfo netFieldExportRPCPropertyAttribute))
                {
                    netFieldInfo = netFieldExportRPCPropertyAttribute;

                    return true;
                }
            }

            return false;
        }

        internal bool WillReadType(int groupId, ParseType parseType, out bool ignoreChannel)
        {
            ignoreChannel = false;

            if (groupId == -1)
            {
                return false;
            }

            NetFieldGroupInfo groupInfo = _parserInfo.NetFieldGroups[groupId];

            if (parseType >= groupInfo.Attribute.MinimumParseType)
            {
                return true;
            }

            //Ignore channels where we know the type and outside the parse mode
            ignoreChannel = true;

            return false;
        }

        internal void ReadField(INetFieldExportGroup obj, NetFieldExport export, NetFieldExportGroup exportGroup, uint handle, NetBitReader netBitReader)
        {
            if(export.PropertyId == -2)
            {
                return;
            }

            string group = exportGroup.PathName;

            string fixedExportName = export.Name;

            /*
            if (fixedExportName == null)
            {
                export.CleanedName = FixInvalidNames(export.Name);
                fixedExportName = export.CleanedName;
            }*/


            bool isDebug = obj is DebuggingExportGroup;
            int groupId = exportGroup.GroupId;

            if (isDebug)
            {
                _parserInfo.NetFieldGroups.TryGetIndex("DebuggingExportGroup", out groupId);
                fixedExportName = "Handles";
            }

            if (exportGroup.GroupId == -1)
            {
                return;
            }

            NetFieldGroupInfo netGroupInfo = _parserInfo.NetFieldGroups[groupId];
            NetFieldInfo netFieldInfo = null;

            //Update
            if (!netGroupInfo.UsesHandles)
            {
                int propertyIndex = export.PropertyId;

                if(export.PropertyId == -1)
                {
                    if (!netGroupInfo.Properties.TryGetIndex(fixedExportName, out propertyIndex))
                    {
                        export.PropertyId = -2;
                        return;
                    }

                    export.PropertyId = propertyIndex;
                }

                netFieldInfo = netGroupInfo.Properties[propertyIndex];
            }
            
            if (netGroupInfo.UsesHandles && !netGroupInfo.HandleProperties.TryGetValue(handle, out netFieldInfo))
            {
                //Clean this up
                if (obj is IHandleNetFieldExportGroup handleGroup)
                {
                    object data = ReadDataType(handleGroup.Type, netBitReader);
                    handleGroup.UnknownHandles.Add(handle, data);
                }

                return;
            }

            SetType(obj, netFieldInfo, netGroupInfo, exportGroup, handle, netBitReader);
        }

        private void SetType(INetFieldExportGroup obj, NetFieldInfo netFieldInfo, NetFieldGroupInfo groupInfo, NetFieldExportGroup exportGroup, uint handle, NetBitReader netBitReader)
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

#if DEBUG
            if (obj is DebuggingExportGroup debugGroup)
            {
                debugGroup.Handles.Add(handle, data as DebuggingObject);

                return;
            }
#endif
            if (data != null)
            {
                netFieldInfo.SetMethod(obj, data);
            }
        }

        private object ReadDataType(RepLayoutCmdType replayout, NetBitReader netBitReader, Type type = null)
        {
            object data = null;

            switch (replayout)
            {
                case RepLayoutCmdType.Property:
                    data = _parserInfo.LinqCache.CreatePropertyObject(type);
                    ((IProperty)data).Serialize(netBitReader);
                    break;
                case RepLayoutCmdType.RepMovement:
                    data = netBitReader.SerializeRepMovement();
                    break;
                case RepLayoutCmdType.RepMovementWholeNumber:
                    data = netBitReader.SerializeRepMovement(VectorQuantization.RoundWholeNumber, RotatorQuantization.ByteComponents, VectorQuantization.RoundWholeNumber);
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
                    //Fix later
                    /*
                    data = _parserInfo.LinqCache.CreateObject(typeof(DebuggingObject));
                    (data as IProperty).Serialize(netBitReader);
                    */
                    break;
            }

            return data;
        }

        private Array ReadArrayField(NetFieldExportGroup netfieldExportGroup, NetFieldInfo fieldInfo, NetFieldGroupInfo groupInfo, NetBitReader netBitReader)
        {
            uint arrayIndexes = netBitReader.ReadIntPacked();
            
            if(arrayIndexes == 0)
            {
                netBitReader.Seek(netBitReader.GetBitsLeft(), SeekOrigin.Current);

                return null;
            }

            Type elementType = fieldInfo.PropertyInfo.PropertyType.GetElementType();
            RepLayoutCmdType replayout = RepLayoutCmdType.Ignore;
            bool isGroupType = elementType == groupInfo.Type || elementType == groupInfo.Type.BaseType;

            if (!isGroupType)
            {
                groupInfo = null;

                if (!_parserInfo.PrimitiveTypeLayout.TryGetValue(elementType, out replayout))
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
                    data = _parserInfo.LinqCache.CreateObject(fieldInfo.ElementTypeId);
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

                    try
                    {
                        netBitReader.SetTempEnd((int)numBits, 4);

                        //Uses the same type for the array
                        if (groupInfo != null)
                        {
                            ReadField((INetFieldExportGroup)data, export, netfieldExportGroup, handle, netBitReader);
                        }
                        else //Probably primitive values
                        {
                            data = ReadDataType(replayout, netBitReader, elementType);
                        }
                    }
                    finally
                    {
                        netBitReader.RestoreTemp(4);
                    }
                }

                arr.SetValue(data, index);
            }
        }

        internal INetFieldExportGroup CreateType(int groupId)
        {
            if (groupId == -1)
            {
                return null;
            }

            NetFieldGroupInfo exportGroup = _parserInfo.NetFieldGroups[groupId];

            if(exportGroup.SingleInstance)
            {
                exportGroup.Instance.ClearInstance();

                return (INetFieldExportGroup)exportGroup.Instance;
            }


            return _parserInfo.LinqCache.CreateObject(exportGroup.TypeId);
        }

        /// <summary>
        /// Create the object associated with the property that should be read.
        /// Used as a workaround for RPC structs.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal bool TryCreateRPCPropertyType(string group, string propertyName, out IProperty property)
        {
            property = null;

            if (_parserInfo.NetRPCStructureTypes.TryGetValue(group, out NetRPCFieldGroupInfo groupInfo))
            {
                if (groupInfo.PathNames.TryGetValue(propertyName, out NetRPCFieldInfo fieldInfo))
                {
                    property = _parserInfo.LinqCache.CreatePropertyObject(fieldInfo.PropertyInfo.PropertyType);

                    return true;
                }
            }

            return false;
        }

        private unsafe string FixInvalidNames(string str)
        {
            int len = str.Length;
            char* newChars = stackalloc char[len];
            char* currentChar = newChars;

            bool found = false;

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
                else
                {
                    found = true;
                }
            }

            if (found)
            {
                Console.WriteLine(str);
            }
            return new string(newChars, 0, (int)(currentChar - newChars));
        }

        private class NetFieldGroupInfo
        {
            public NetFieldExportGroupAttribute Attribute { get; set; }
            public Type Type { get; set; }
            public int TypeId { get; set; }

            public bool UsesHandles { get; set; }
            public bool SingleInstance { get; set; }
            public ISingleInstance Instance { get; set; }

            public KeyList<string, NetFieldInfo> Properties { get; set; } = new KeyList<string, NetFieldInfo>();
            public Dictionary<uint, NetFieldInfo> HandleProperties { get; set; } = new Dictionary<uint, NetFieldInfo>();
        }

        private class NetFieldInfo
        {
            public RepLayoutAttribute Attribute { get; set; }
            public PropertyInfo PropertyInfo { get; set; }
            public int ElementTypeId { get; set; }
            public Action<object, object> SetMethod { get; set; }
        }

        /// <summary>
        /// Holds type info for assembly
        /// </summary>
        private class NetFieldParserInfo
        {
            public KeyList<string, NetFieldGroupInfo> NetFieldGroups { get; private set; } = new KeyList<string, NetFieldGroupInfo>();

            public Dictionary<Type, RepLayoutCmdType> PrimitiveTypeLayout { get; private set; } =
            new Dictionary<Type, RepLayoutCmdType>();

            public Dictionary<string, NetRPCFieldGroupInfo> NetRPCStructureTypes { get; private set; } = new Dictionary<string, NetRPCFieldGroupInfo>(); //Mapping from ClassNetCache -> Type path name
            public HashSet<string> PlayerControllers { get; private set; } = new HashSet<string>(); //Player controllers require 1 extra byte to be read when creating actor
            public CompiledLinqCache LinqCache { get; private set; } = new CompiledLinqCache();
        }
    }

    internal class NetRPCFieldInfo
    {
        public NetFieldExportRPCPropertyAttribute Attribute { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public bool IsCustomStructure { get; set; }
    }

    internal class NetRPCFieldGroupInfo
    {
        public ParseType ParseType { get; set; }
        public Dictionary<string, NetRPCFieldInfo> PathNames { get; set; } = new Dictionary<string, NetRPCFieldInfo>();
    }
}
