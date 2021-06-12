using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Extensions;

namespace Unreal.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class NetFieldExportRPCPropertyAttribute : Attribute
    {
        public string Name { get; private set; }
        public string TypePathName { get; private set; }
        public bool ReadChecksumBit { get; private set; }
        public bool IsFunction { get; private set; }
        public bool CustomStructure { get; private set; }

        public NetFieldExportRPCPropertyAttribute(string name, string typePathname, bool readChecksumBit = true, bool customStructure = false)
        {
            Name = name;
            TypePathName = Utilities.RemoveAllPathPrefixes(typePathname);
            ReadChecksumBit = readChecksumBit;
            CustomStructure = customStructure;

            if (typePathname.Length > name.Length)
            {
                IsFunction = typePathname[typePathname.Length - (name.Length + 1)] == ':';
            }
        }
    }
}
