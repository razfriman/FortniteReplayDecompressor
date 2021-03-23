using FortniteReplayReader.Extensions;
using System;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class PlayerElimination : BaseEvent, IEquatable<PlayerElimination>
    {
        public PlayerEliminationInfo EliminatedInfo { get; internal set; }
        public PlayerEliminationInfo EliminatorInfo { get; internal set; }

        public string Eliminated => EliminatedInfo?.Id;
        public string Eliminator => EliminatorInfo?.Id;

        public byte GunType { get; internal set; }
        public string Time => Timestamp.MillisecondsToTimeStamp();
        public uint Timestamp { get; internal set; }

        public bool Knocked { get; internal set; }
        public bool ValidDistance => EliminatorInfo.Location.Size() != 0;
        public bool SelfElimination => Eliminated == Eliminator;

        public double Distance
        {
            get
            {
                if(!ValidDistance)
                {
                    return -1;
                }

                return EliminatorInfo.Location.DistanceTo(EliminatedInfo.Location);
            }
        }

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