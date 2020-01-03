using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Contracts;

namespace Unreal.Core.Models
{
    public class FMinimalGameplayCueReplicationProxy : IProperty
    {
        public FGameplayTag[] Tags { get; private set; } = new FGameplayTag[0];

        public void Serialize(NetBitReader reader)
        {
            int numElements = reader.ReadBitsToInt(5);

            if(numElements == 0)
            {
                return;
            }

            Tags = new FGameplayTag[numElements];

            for (int i = 0; i < numElements; i++)
            {
                FGameplayTag tag = new FGameplayTag();
                tag.Serialize(reader);

                Tags[i] = tag;
            }
        }

        public void UpdateTags(NetFieldExportGroup networkGameplayTagNode)
        {
            for (int i = 0; i < Tags.Length; i++)
            {
                Tags[i].TagName = networkGameplayTagNode.NetFieldExports[(int)Tags[i].TagIndex]?.Name;
            }
        }
    }
}
