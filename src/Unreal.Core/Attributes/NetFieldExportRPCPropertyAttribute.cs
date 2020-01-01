using System;
using System.Collections.Generic;
using System.Text;

namespace Unreal.Core.Attributes
{
    public class NetFieldExportRPCPropertyAttribute : Attribute
    {
        public string Name { get; private set; }
        public string TypePathName { get; private set; }
        public bool NetDeltaSerialization { get; private set; }
        public bool IsFunction { get; private set; }

        public NetFieldExportRPCPropertyAttribute(string name, string typePathname, bool netDeltaSerialize = true)
        {
            Name = name;
            TypePathName = typePathname;
            NetDeltaSerialization = netDeltaSerialize;

            IsFunction = typePathname[^(name.Length + 1)] == ':';
        }
    }
}
