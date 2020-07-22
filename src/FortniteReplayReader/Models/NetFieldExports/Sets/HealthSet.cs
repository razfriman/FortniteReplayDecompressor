using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Sets
{
    [NetFieldExportGroup("/Script/FortniteGame.FortRegenHealthSet", ParseType.Normal)]
    [RedirectPath("HealthSet")]
    public class HealthSet : IHandleNetFieldExportGroup
    {
        [NetFieldExportHandle(0, RepLayoutCmdType.PropertyFloat)]
        public float? HealthBaseValue { get; set; }

        [NetFieldExportHandle(1, RepLayoutCmdType.PropertyFloat)]
        public float? HealthCurrentValue { get; set; }

        [NetFieldExportHandle(3, RepLayoutCmdType.PropertyFloat)]
        public float? HealthMaxValue { get; set; }

        [NetFieldExportHandle(7, RepLayoutCmdType.PropertyFloat)]
        public float? HealthUnclampedBaseValue { get; set; }

        [NetFieldExportHandle(8, RepLayoutCmdType.PropertyFloat)]
        public float? HealthUnclampedCurrentValue { get; set; }

        [NetFieldExportHandle(18, RepLayoutCmdType.PropertyFloat)]
        public float? ShieldBaseValue { get; set; }

        [NetFieldExportHandle(19, RepLayoutCmdType.PropertyFloat)]
        public float? ShieldCurrentValue { get; set; }

        [NetFieldExportHandle(21, RepLayoutCmdType.PropertyFloat)]
        public float? ShieldMaxValue { get; set; }

        public override bool ManualRead(string property, object value)
        {
            switch (property)
            {
                case "HealthBaseValue":
                    HealthBaseValue = (float)value;
                    break;
                case "HealthCurrentValue":
                    HealthCurrentValue = (float)value;
                    break;
                case "HealthMaxValue":
                    HealthMaxValue = (float)value;
                    break;
                case "HealthUnclampedBaseValue":
                    HealthUnclampedBaseValue = (float)value;
                    break;
                case "HealthUnclampedCurrentValue":
                    HealthUnclampedCurrentValue = (float)value;
                    break;
                case "ShieldBaseValue":
                    ShieldBaseValue = (float)value;
                    break;
                case "ShieldCurrentValue":
                    ShieldCurrentValue = (float)value;
                    break;
                case "ShieldMaxValue":
                    ShieldMaxValue = (float)value;
                    break;
                default:
                    base.ManualRead(property, value);
                    break;
            }

            return true;
        }
    }
}
