using System;
using System.Collections.Generic;
using System.Text;

namespace Unreal.Core.Attributes
{
    public class ClassNetCacheAttribute : Attribute
    {
        public string PathName { get; private set; }

        public ClassNetCacheAttribute(string typePathname)
        {
            PathName = typePathname;
        }
    }
}
