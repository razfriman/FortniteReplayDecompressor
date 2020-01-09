using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class NetFieldExportHandleAttribute : Attribute
    {
        public uint Handle { get; private set; }
        public RepLayoutCmdType Type { get; private set; }
        public UnknownFieldInfo Info { get; private set; }

        public NetFieldExportHandleAttribute(uint handle, RepLayoutCmdType type)
        {
            Handle = handle;
            Type = type;
        }
    }
}
