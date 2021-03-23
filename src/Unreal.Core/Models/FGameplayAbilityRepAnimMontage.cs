using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Contracts;

namespace Unreal.Core.Models
{
    public class FGameplayAbilityRepAnimMontage : IProperty
    {
        /** AnimMontage ref */
        public UObjectGUID AnimMontage { get; private set; }

        /** Play Rate */
        public float PlayRate { get; private set; }

        /** Montage position */
        public float Position { get; private set; }

        /** Montage current blend time */
        public float BlendTime { get; private set; }

        /** NextSectionID */
        public byte NextSectionID { get; private set; }

        /** flag indicating we should serialize the position or the current section id */
        public bool bRepPosition { get; private set; } = true;

        /** Bit set when montage has been stopped. */
        public bool IsStopped { get; private set; } = true;

        /** Bit flipped every time a new Montage is played. To trigger replication when the same montage is played again. */
        public bool ForcePlayBit { get; private set; }

        /** Stops montage position from replicating at all to save bandwidth */
        public bool SkipPositionCorrection { get; private set; }

        /** Stops PlayRate from replicating to save bandwidth. PlayRate will be assumed to be 1.f. */
        public bool bSkipPlayRate { get; private set; }

        public FPredictionKey PredictionKey { get; private set; }

        /** The current section Id used by the montage. Will only be valid if bRepPosition is false */
        public int SectionIdToPlay { get; private set; }

        public void Serialize(NetBitReader reader)
        {
            bool repPosition = reader.ReadBoolean();

            if(repPosition)
            {
                bRepPosition = true;
                SectionIdToPlay = 0;
                SkipPositionCorrection = false;

                uint packedPosition = reader.ReadIntPacked();

                Position = packedPosition / 100;
            }
            else
            {
                bRepPosition = false;

                SkipPositionCorrection = true;
                Position = 0;
                SectionIdToPlay = reader.ReadBitsToInt(7);
            }

            IsStopped = reader.ReadBit();
            ForcePlayBit = reader.ReadBit();
            SkipPositionCorrection = reader.ReadBit();
            bSkipPlayRate = reader.ReadBit();

            AnimMontage = new UObjectGUID { Value = reader.ReadIntPacked() };
            PlayRate = reader.SerializePropertyFloat();
            BlendTime = reader.SerializePropertyFloat();
            NextSectionID = reader.ReadByte();

            PredictionKey = new FPredictionKey();
            PredictionKey.Serialize(reader);
        }
    }
}
