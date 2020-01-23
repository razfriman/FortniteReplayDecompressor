using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches
{
    [NetFieldExportRPC("FortTeamPrivateInfo_ClassNetCache", ParseType.Minimal)]
    public class FortTeamPrivateInfoCache
    {
        [NetFieldExportRPCProperty("LatentTeamRepData", "/Script/FortniteGame.FortTeamPrivateInfo")]
        public object LatentTeamRepData { get; set; }

        [NetFieldExportRPCProperty("RepData", "/Script/FortniteGame.FortTeamPrivateInfo")]
        public object RepData { get; set; }
    }
}
