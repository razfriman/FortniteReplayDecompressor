using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Contracts;

namespace Unreal.Core.Models
{
    public class FRootMotionSourceGroup : IProperty
    {
        public void Serialize(NetBitReader reader)
        {
            byte sourceNum = reader.ReadByte();

            bool bHasAdditiveSources = reader.ReadBit();
            bool bHasOverrideSources = reader.ReadBit();
            FVector lastPreAdditiveVelocity = reader.SerializePropertyVector10();
            bool bIsAdditiveVelocityApplied = reader.ReadBit();
            byte lastAccumulatedSettingsFlags = reader.ReadByte();

            //Off by about 130 bits
            for (int i = 0; i < sourceNum; i++)
            {
                uint guid = reader.SerializePropertyObject();

                ushort priority = reader.ReadUInt16();
                ushort localId = reader.ReadUInt16();
                byte accumulatedModeSerialize = reader.ReadByte();
                string instanceName = reader.SerializePropertyName();
                float currentTime = reader.SerializePropertyFloat();
                float duration = reader.SerializePropertyFloat();
                byte statusFlags = reader.ReadByte();
                bool bInLocalSpace = reader.ReadBit();
            }
        }
    }
}
