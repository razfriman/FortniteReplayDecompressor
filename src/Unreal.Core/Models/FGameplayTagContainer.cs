using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Contracts;

namespace Unreal.Core.Models
{
    public class FGameplayTagContainer : IProperty
    {
        public string[] TagNames { get; private set; }
        private uint?[] Tags { get; set; }

        public void Serialize(NetBitReader reader)
        {
            bool isEmpty = reader.ReadBit();

            if (isEmpty)
            {
                return;
            }

            int numTags = reader.ReadBitsToInt(7);

            Tags = new uint?[numTags];
            TagNames = new string[numTags];

            for (int i = 0; i < numTags; i++)
            {
                Tags[i] = reader.ReadIntPacked();
            }
        }

        public void UpdateTags(NetFieldExportGroup networkGameplayTagNode)
        {
            for (int i = 0; i < Tags.Length; i++)
            {
                TagNames[i] = networkGameplayTagNode.NetFieldExports[(int)Tags[i]]?.Name;
            }
        }
    }
}
