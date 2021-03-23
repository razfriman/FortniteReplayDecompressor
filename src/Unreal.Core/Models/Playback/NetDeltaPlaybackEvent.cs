using System;
using System.Collections.Generic;
using System.Text;

namespace Unreal.Core.Models.Playback
{
    public class NetDeltaPlaybackEvent : ReplayPlaybackEvent
    {
        public NetDeltaUpdate DeltaUpdate { get; set; }
    }
}
