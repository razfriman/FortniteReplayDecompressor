using System;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NetFieldExportAttribute : Attribute
    {
        public string Name { get; private set; }
        public RepLayoutCmdType Type { get; private set; }
        public UnknownFieldInfo Info { get; private set; }

        public NetFieldExportAttribute(string name, RepLayoutCmdType type)
        {
            Name = name;
            Type = type;
        }
    }
}