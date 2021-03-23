using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Contracts;

namespace Unreal.Core.Models.Playback
{
    public class ExportGroupPlaybackEvent : ReplayPlaybackEvent
    {
        public INetFieldExportGroup ExportGroup { get; set; }
        public uint Channel { get; set; }
    }
}
