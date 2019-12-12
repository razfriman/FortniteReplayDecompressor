using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core;
using Unreal.Core.Contracts;

namespace Unreal.Core.Models
{
    public class FGameplayTag : IProperty
    {
        public string TagName { get; private set; }

        public void Serialize(NetBitReader reader)
        {
            TagName = reader.ReadFString();
        }
    }
}
