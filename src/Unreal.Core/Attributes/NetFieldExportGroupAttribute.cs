using System;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class NetFieldExportGroupAttribute : Attribute
    {
        public string Path { get; protected set; }
        public bool PartialGroup { get; protected set; }
        public ParseType MinimumParseType { get; protected set; }

        public NetFieldExportGroupAttribute(string path, ParseType minParseType = ParseType.Minimal, bool partialGroup = false)
        {
            Path = path;
            PartialGroup = partialGroup;
            MinimumParseType = minParseType;
        }
    }
}
