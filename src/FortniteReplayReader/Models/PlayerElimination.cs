using System;

namespace FortniteReplayReader.Models
{
    public class PlayerElimination : BaseEvent, IEquatable<PlayerElimination>
    {
        public string Eliminated { get; internal set; }
        public string Eliminator { get; internal set; }
        public byte GunType { get; internal set; }
        public string Time { get; internal set; }
        public bool Knocked { get; internal set; }

        public bool Equals(PlayerElimination other)
        {
            if (other.Equals(null))
            {
                return false;
            }

            if (this.Eliminated == other.Eliminated && this.Eliminator == other.Eliminator && this.GunType == other.GunType && this.Time == other.Time && this.Knocked == other.Knocked)
            {
                return true;
            }

            return false;
        }
    }
}