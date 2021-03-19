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
        public float? HealthBaseValue => HealthFortSet?.BaseValue;
        public float? HealthCurrentValue => HealthFortSet?.CurrentValue;
        public float? HealthMaxValue => HealthFortSet?.Maximum;
        public float? HealthUnclampedBaseValue => HealthFortSet?.UnclampedBaseValue;
        public float? HealthUnclampedCurrentValue => HealthFortSet?.UnclampedCurrentValue;

        public float? ShieldBaseValue => ShieldFortSet?.BaseValue;
        public float? ShieldCurrentValue => ShieldFortSet?.CurrentValue;
        public float? ShieldMaxValue => ShieldFortSet?.Maximum;
        public float? ShieldUnclampedBaseValue => ShieldFortSet?.UnclampedBaseValue;
        public float? ShieldUnclampedCurrentValue => ShieldFortSet?.UnclampedCurrentValue;

        public FortSet HealthFortSet { get; set; }
        public FortSet ShieldFortSet { get; set; }

        public bool HealthChange => HealthFortSet != null;
        public bool ShieldChange => ShieldFortSet != null;
    }
}
