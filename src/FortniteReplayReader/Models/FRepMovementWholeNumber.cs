using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models
{
    public class FRepMovementWholeNumber : FRepMovement, IProperty
    {
        public new void Serialize(NetBitReader reader)
        {
            SerializeRepMovement(reader, VectorQuantization.RoundWholeNumber, RotatorQuantization.ByteComponents, VectorQuantization.RoundWholeNumber);
        }
    }
}
