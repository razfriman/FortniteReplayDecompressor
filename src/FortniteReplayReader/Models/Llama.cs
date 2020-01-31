﻿using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class Llama
    {
        public Llama()
        {

        }

        public Llama(uint channelIndex, SupplyDropLlama drop)
        {
            Id = channelIndex;
            Looted = drop.Looted;
            FinalDestination = drop.FinalDestination;
            Location = drop.ReplicatedMovement?.Location;
            HasSpawnedPickups = drop.bHasSpawnedPickups;
        }
       
        public uint Id { get; set; }
        public FVector Location { get; set; }
        public bool HasSpawnedPickups { get; set; }
        public bool Looted { get; set; }
        public FVector FinalDestination { get; set; }
    }
}