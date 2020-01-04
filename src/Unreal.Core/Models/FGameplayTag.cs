using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core;
using Unreal.Core.Contracts;

namespace Unreal.Core.Models
{
    public class FGameplayTag : IProperty
    {
        public string TagName { get; set; }
        public uint? TagIndex { get; private set; }

        public void Serialize(NetBitReader reader)
        {
            TagIndex = reader.ReadIntPacked();
        }

        public override string ToString()
        {
            return TagName ?? TagIndex.ToString();
        }

        public void UpdateTagName(NetFieldExportGroup networkGameplayTagNode)
        {
            if(TagIndex > networkGameplayTagNode.NetFieldExportsLength)
            {
                return;
            }

            TagName = networkGameplayTagNode.NetFieldExports[(int)TagIndex]?.Name;
        }
    }
}
