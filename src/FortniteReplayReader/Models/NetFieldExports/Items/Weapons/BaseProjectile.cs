using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Weapons
{
    public class BaseProjectile : INetFieldExportGroup
    {
        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public object RemoteRole { get; set; }

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public object Role { get; set; }

        [NetFieldExport("bHidden", RepLayoutCmdType.PropertyBool)]
        public bool bHidden { get; set; }

        [NetFieldExport("ReplicatedMovement", RepLayoutCmdType.RepMovementWholeNumber)]
        public FRepMovement ReplicatedMovement { get; set; }

        [NetFieldExport("Owner", RepLayoutCmdType.Property)]
        public NetworkGUID Owner { get; set; }

        [NetFieldExport("Instigator", RepLayoutCmdType.Property)]
        public NetworkGUID Instigator { get; set; }

        [NetFieldExport("Team", RepLayoutCmdType.Enum)]
        public int? Team { get; set; }

        [NetFieldExport("ReplicatedMaxSpeed", RepLayoutCmdType.PropertyFloat)]
        public float? ReplicatedMaxSpeed { get; set; }

        [NetFieldExport("GravityScale", RepLayoutCmdType.PropertyFloat)]
        public float? GravityScale { get; set; }

        [NetFieldExport("PawnHitResult", RepLayoutCmdType.Property)]
        public FHitResult PawnHitResult { get; set; }

        [NetFieldExport("SyncId", RepLayoutCmdType.PropertyUInt16)]
        public ushort? SyncId { get; set; }
    }

    public class BaseExplosiveProjectile : BaseProjectile
    {
        [NetFieldExport("bHasExploded", RepLayoutCmdType.PropertyBool)]
        public bool bHasExploded { get; set; }

    }

    public class BaseLauncherProjectile : BaseExplosiveProjectile
    {
        [NetFieldExport("bIsBeingKilled", RepLayoutCmdType.PropertyBool)]
        public bool bIsBeingKilled { get; set; }

        [NetFieldExport("StopLocation", RepLayoutCmdType.PropertyVector)]
        public FVector? StopLocation { get; set; }

        [NetFieldExport("DecalLocation", RepLayoutCmdType.PropertyVector)]
        public FVector? DecalLocation { get; set; }
    }
}
