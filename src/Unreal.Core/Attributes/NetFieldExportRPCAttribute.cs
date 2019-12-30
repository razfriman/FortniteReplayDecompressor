using System;
using System.Collections.Generic;
using System.Text;

namespace Unreal.Core.Attributes
{
    public class NetFieldExportRPCAttribute : Attribute
    {
        public string PathName { get; private set; }

        public NetFieldExportRPCAttribute(string typePathname)
        {
            PathName = typePathname;
        }
    }
}
