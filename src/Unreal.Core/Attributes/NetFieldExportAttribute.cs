using System;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class RepLayoutAttribute : Attribute
    {
        public RepLayoutCmdType Type { get; set; }

        public RepLayoutAttribute(RepLayoutCmdType type)
        {
            Type = type;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class NetFieldExportAttribute : RepLayoutAttribute
    {
        public string Name { get; set; }

        public NetFieldExportAttribute(string name, RepLayoutCmdType type) : base(type)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class NetFieldExportHandleAttribute : RepLayoutAttribute
    {
        public uint Handle { get; private set; }

        public NetFieldExportHandleAttribute(uint handle, RepLayoutCmdType type) : base(type)
        {
            Handle = handle;
            Type = type;
        }
    }
}