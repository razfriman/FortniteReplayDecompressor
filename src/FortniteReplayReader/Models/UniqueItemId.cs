using System;
using System.Collections.Generic;
using System.Text;

namespace FortniteReplayReader.Models
{
    internal class UniqueItemId
    {
        public uint A { get; set; }
        public uint B { get; set; }
        public uint C { get; set; }
        public uint D { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is UniqueItemId id)
            {
                return A == id.A && B == id.B && C == id.C && D == id.D;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return unchecked(A.GetHashCode() * 13 + B.GetHashCode() * 17 + C.GetHashCode() * 19 + D.GetHashCode() * 23);
        }
    }
}
