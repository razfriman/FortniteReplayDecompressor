using System;
using System.Collections.Generic;
using System.Text;

namespace Unreal.Core.Models.Playback
{
    public class ChannelClosedPlaybackEvent : ReplayPlaybackEvent
    {
        public uint Channel { get; set; }
    }
}
