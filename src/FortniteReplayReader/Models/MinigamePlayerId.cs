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
        public bool RemoveAll { get; private set; }
        public uint? RemoveId { get; private set; }
        internal List<string> debugStrings = new List<string>();

        public void Serialize(NetBitReader reader)
        {
            uint total = reader.ReadIntPacked();

            if(total == 0)
            {
                RemoveAll = true;
            }
            else
            {
                uint id = reader.ReadIntPacked();

                if (id == 0)
                {
                    RemoveId = (total);
                }
                else
                {
                    Ids = new string[total];

                    Ids[id - 1] = ParseId();

                    while (id < total)
                    {
                        id = reader.ReadIntPacked();

                        Ids[id - 1] = ParseId();
                    }
                }
            }

            reader.SkipBits(8);

            string ParseId()
            {
                reader.SkipBits(32); //Always the same

                string playerId = reader.SerializePropertyNetId();

                reader.SkipBits(8); //Always 0

                return playerId;
            }
        }
    }
}
