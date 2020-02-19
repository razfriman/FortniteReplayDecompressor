﻿using System.Collections.Generic;

namespace FortniteReplayReader.Models
{
    public class KillFeedEntry
    {
        public int? PlayerId { get; set; }
        public string PlayerName { get; set; }
        public bool PlayerIsBot { get; set; }

        public int? FinisherOrDowner { get; set; }
        public string FinisherOrDownerName { get; set; }
        public bool FinisherOrDownerIsBot { get; set; }

        public float? ReplicatedWorldTimeSeconds { get; set; }
        public float? Distance { get; set; }
        public int? DeathCause { get; set; }
        public int? DeathCircumstance { get; set; }
        public IEnumerable<string> DeathTags { get; set; }

        public bool IsDowned { get; set; }
        public bool IsRevived { get; set; }
    }
}
