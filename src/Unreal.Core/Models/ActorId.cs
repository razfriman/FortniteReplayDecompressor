using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Contracts;

namespace Unreal.Core.Models
{
    public class ActorId : IProperty
    {
        public uint? Id { get; set; }

        public void Serialize(NetBitReader reader)
        {
            Id = reader.SerializePropertyObject();
        }

        public override string ToString()
        {
            return Id?.ToString();
        }
    }
}
