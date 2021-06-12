using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class GameplayCueEvent
    {
        public string TagName { get; set; }
        public uint? TagId { get; set; }
        public float DeltaGameTimeSeconds { get; set; }
        public FVector Location { get; set; }
    }
}