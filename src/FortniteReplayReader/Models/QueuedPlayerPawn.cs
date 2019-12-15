using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class QueuedPlayerPawn
    {
        public uint ChannelId { get; set; }
        public PlayerPawnC PlayerPawn { get; set; }
    }
}
