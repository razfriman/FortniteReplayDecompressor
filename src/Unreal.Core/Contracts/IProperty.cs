using System;
using System.Collections.Generic;
using System.Text;

namespace Unreal.Core.Contracts
{
    public interface IProperty
    {
        void Serialize(NetBitReader reader);
    }
}
