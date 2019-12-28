using System;
using System.Collections.Generic;
using System.Text;

namespace Unreal.Core.Attributes
{
    public class ClassNetCachePropertyAttribute : Attribute
    {
        public string Name { get; set; }
        public string TypePathName { get; private set; }

        public ClassNetCachePropertyAttribute(string name, string typePathname)
        {
            Name = name;
            TypePathName = typePathname;
        }
    }
}
