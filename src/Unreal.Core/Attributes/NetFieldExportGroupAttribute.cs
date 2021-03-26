using System;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class NetFieldExportGroupAttribute : Attribute
    {
        public string Path { get; private set; }
        public ParseType MinimumParseType { get; internal set; }

        public NetFieldExportGroupAttribute(string path, ParseType minParseType = ParseType.Minimal)
        {
            Path = path;
            MinimumParseType = minParseType;
        }
    }
}
