using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class NetFieldExportRPCAttribute : Attribute
    {
        public string PathName { get; private set; }
        public ParseType MinimumParseType { get; private set; }

        public NetFieldExportRPCAttribute(string typePathname, ParseType minimumParseType = ParseType.Full)
        {
            PathName = typePathname;
            MinimumParseType = minimumParseType;
        }
    }
}
