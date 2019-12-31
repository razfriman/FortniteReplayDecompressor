using System;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class NetFieldExportGroupAttribute : Attribute
    {
        public string Path { get; private set; }
        public bool PartialGroup { get; private set; }
        public ParseType MinimumParseType { get; set; }

        public NetFieldExportGroupAttribute(string path, ParseType minParseType = ParseType.Minimal, bool partialGroup = false)
        {
            Path = path;
            PartialGroup = partialGroup;
            MinimumParseType = minParseType;
        }
    }
}
