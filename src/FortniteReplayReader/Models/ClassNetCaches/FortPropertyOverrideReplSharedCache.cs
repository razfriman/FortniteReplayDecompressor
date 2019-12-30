using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;

namespace FortniteReplayReader.Models.ClassNetCaches
{
    [NetFieldExportRPC("FortPropertyOverrideReplShared_ClassNetCache")]
    public class FortPropertyOverrideReplSharedCache
    {
        [NetFieldExportRPCProperty("ReplOverrides", "/Script/FortniteGame.FortPropertyOverrideReplShared")]
        public object ReplOverrides { get; set; }
    }
}
