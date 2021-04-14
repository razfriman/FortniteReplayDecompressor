using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class WeaponShot
    {
        public PlayerPawn ShotByPlayerPawn { get; internal set; }
        public PlayerPawn HitPlayerPawn { get; internal set; }
        public Weapon Weapon { get; internal set; }
        public float DeltaGameTimeSeconds { get; internal set; }
        public FVector Location { get; internal set; }
        public FVector Normal { get; internal set; }
        public float Damage { get; internal set; }
        public bool WeaponActivate { get; internal set; }
        public bool IsFatal { get; internal set; }
        public bool IsCritical { get; internal set; }
        public bool IsShield { get; internal set; }
        public bool IsShieldDestroyed { get; internal set; }
        public bool IsBallistic { get; internal set; }
        public bool FatalHitNonPlayer { get; internal set; }
        public bool CriticalHitNonPlayer { get; internal set; }
        public bool HitPlayer => HitPlayerPawn != null;
    }
}
