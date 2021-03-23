using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Contracts;

namespace Unreal.Core.Models
{
    public class FGameplayTagContainer : IProperty
    {
        public FGameplayTag[] Tags { get; private set; } = new FGameplayTag[0];

        public void Serialize(NetBitReader reader)
        {
            bool isEmpty = reader.ReadBit();

            if (isEmpty)
            {
                return;
            }

            int numTags = reader.ReadBitsToInt(7);

            Tags = new FGameplayTag[numTags];

            for (int i = 0; i < numTags; i++)
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
                Tags[i].UpdateTagName(networkGameplayTagNode);
            }
        }
    }
}
