using System;

namespace Unreal.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class NetFieldExportGroupAttribute : Attribute
    {
        public string Path { get; private set; }
        public bool PartialGroup { get; private set; }

        public NetFieldExportGroupAttribute(string path, bool partialGroup = false)
        {
            Path = path;
            PartialGroup = partialGroup;
        }
    }
}
