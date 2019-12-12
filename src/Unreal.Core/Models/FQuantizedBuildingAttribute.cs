using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Contracts;

namespace Unreal.Core.Models
{
    public class FQuantizedBuildingAttribute : IProperty
    {
        public byte[] DebugData { get; private set; }

        public void Serialize(NetBitReader reader)
        {
            DebugData = reader.ReadBytes(2);
        }
    }
}
