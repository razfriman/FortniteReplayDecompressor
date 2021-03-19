using System;
using System.Collections.Generic;
using System.Text;

namespace FortniteReplayReader.Models.NetFieldExports.Sets
{
    public class FortSet
    {
        public float? BaseValue { get; set; }
        public float? CurrentValue { get; set; }
        public float? Maximum { get; set; }
        public float? UnclampedBaseValue { get; set; }
        public float? UnclampedCurrentValue { get; set; }
    }
}
