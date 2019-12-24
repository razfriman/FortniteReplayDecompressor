namespace FortniteReplayReader.Models
{
    public class TeamStats : BaseEvent
    {
        public uint Unknown { get; internal set; }
        public uint Position { get; internal set; }
        public uint TotalPlayers { get; internal set; }
    }
}
