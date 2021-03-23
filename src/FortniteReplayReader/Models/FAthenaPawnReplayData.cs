using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core;
using Unreal.Core.Contracts;

namespace FortniteReplayReader.Models
{
    public class FAthenaPawnReplayData : IProperty
    {
        public byte[] EncryptedReplayData { get; private set; }

        public override string ToString()
        {
            return BitConverter.ToString(EncryptedReplayData).Replace("-", "");
        }

        public void Serialize(NetBitReader reader)
        {
            int length = reader.ReadInt32();

            EncryptedReplayData = reader.ReadBytes(length);
        }
    }
}
