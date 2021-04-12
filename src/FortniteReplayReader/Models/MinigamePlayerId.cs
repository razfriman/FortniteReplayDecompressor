using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.Core;
using Unreal.Core.Contracts;

namespace FortniteReplayReader.Models
{
    public class MinigamePlayerId : IProperty
    {
        public string[] Ids { get; private set; }
        public bool RemoveAll => Ids == null;

        public void Serialize(NetBitReader reader)
        {
            uint total = reader.ReadIntPacked();

            if(reader.LastBit == 16)
            {
                reader.ReadBits(16);

                return;
            }

            Ids = new string[total];

            for(int i = 0; i < total; i++)
            {
                reader.SkipBits(40);

                Ids[i] = reader.SerializePropertyNetId();

                reader.SkipBits(8); //Unknown

            }

            reader.SkipBits(8);
        }
    }
}
