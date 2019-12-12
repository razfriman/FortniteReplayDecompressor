using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class GameInformation
    {
        public ICollection<Llama> Llamas => _llamas.Values;

        private Dictionary<uint, Llama> _llamas = new Dictionary<uint, Llama>();

        public void UpdateLlama(uint channel, SupplyDropLlamaC supplyDropLlama)
        {
            Llama newLlama = new Llama();

            if(!_llamas.TryAdd(channel, newLlama))
            {
                _llamas.TryGetValue(channel, out newLlama);
            }

            newLlama.Location = supplyDropLlama.ReplicatedMovement?.Location ?? newLlama.Location;
            newLlama.Looted = supplyDropLlama.Looted ?? newLlama.Looted;
            newLlama.Destroyed = supplyDropLlama.bDestroyed ?? newLlama.Destroyed;
            newLlama.SpawnedItems = supplyDropLlama.bHasSpawnedPickups ?? newLlama.SpawnedItems;
        }
    }
}
