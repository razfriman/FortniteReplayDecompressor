using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Sets
{
    [NetFieldExportGroup("/Script/FortniteGame.FortMovementSet", ParseType.Normal)]
    [RedirectPath("MovementSet")]
    public class MovementSet : IHandleNetFieldExportGroup
    {
        public override RepLayoutCmdType Type { get; protected set; } = RepLayoutCmdType.PropertyFloat;

        /*
        [NetFieldExportHandle(0, RepLayoutCmdType.PropertyFloat)]
        public float? HealthBaseValue { get; set; }

        [NetFieldExportHandle(1, RepLayoutCmdType.PropertyFloat)]
        public float? HealthCurrentValue { get; set; }

        [NetFieldExportHandle(3, RepLayoutCmdType.PropertyFloat)]
        public float? HealthMaxValue { get; set; }

        [NetFieldExportHandle(18, RepLayoutCmdType.PropertyFloat)]
        public float? ShieldBaseValue { get; set; }

        [NetFieldExportHandle(19, RepLayoutCmdType.PropertyFloat)]
        public float? ShieldCurrentValue { get; set; }

        [NetFieldExportHandle(21, RepLayoutCmdType.PropertyFloat)]
        public float? ShieldMaxValue { get; set; }*/
    }
}
