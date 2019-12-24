using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class SearchableItem
    {
        public FVector Location { get; internal set; }
        public bool Opened { get; internal set; }
        public bool Destroyed { get; internal set; }
        public bool SpawnedItems { get; internal set; }

        public override string ToString()
        {
            return $"Location: {Location}. Opened: {Opened}.";
        }
    }
}
