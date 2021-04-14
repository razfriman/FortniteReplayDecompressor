using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteReplayReader.Models
{
    public enum MovementType { 
        /// <summary>
        /// Value is set whenever sprinting has stopped
        /// </summary>
        NotSprinting = 0,
        ADS = 1, //Always when ADS
        Sprinting = 3 
    };

    public class PlayerMovementInformation
    {
        public bool InAircraft { get; internal set; }
        public bool Crouched { get; internal set; }
        public bool IsSlopeSliding { get; internal set; }
        public bool GliderOpen { get; internal set; }
        public bool Skydiving { get; internal set; }
        public bool IsInteracting { get; internal set; }
        public bool IsEmoting { get; internal set; }
        public bool IsTargeting { get; internal set; }
        public bool JumpedForceApplied { get; internal set; }
        public bool ADS { get; internal set; }
        public bool Sprinting { get; internal set; }

        public PlayerMovementInformation Copy()
        {
            return new PlayerMovementInformation
            {
                InAircraft = InAircraft,
                Crouched = Crouched,
                GliderOpen = GliderOpen,
                IsEmoting = IsEmoting,
                IsInteracting = IsInteracting,
                IsSlopeSliding = IsSlopeSliding,
                IsTargeting = IsTargeting,
                ADS = ADS,
                Sprinting = Sprinting,
                Skydiving = Skydiving,
                JumpedForceApplied = JumpedForceApplied
            };
        }
    }

}
