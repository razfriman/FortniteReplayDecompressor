using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Contracts;

namespace Unreal.Core.Models
{
    public class FAthenaPawnReplayData : IProperty
    {
        public byte[] EncryptedReplayData { get; private set; }

        public void Serialize(NetBitReader reader)
        {
            EncryptedReplayData = reader.ReadBytes(reader.GetBitsLeft() / 8);
        }
    }
}
