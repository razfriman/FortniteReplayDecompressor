using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class Llama
    {
        public FVector Location { get; set; }
        public bool Looted { get; set; }
        public bool Destroyed { get; set; }
        public bool SpawnedItems { get; set; }

        public override string ToString()
        {
            return $"Location: {Location}. Looted: {Looted}. Spawned Items: {SpawnedItems}. Destroyed: {Destroyed}";
        }
    }
}
