using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Containers
{
    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Containers/Tiered_Chest_Athena.Tiered_Chest_Athena_C", ParseType.Full)]
    public class Chest : BaseContainer
    {
        [NetFieldExport("ForceMetadataRelevant", RepLayoutCmdType.Enum)]
        public int? ForceMetadataRelevant { get; set; }

        [NetFieldExport("StaticMesh", RepLayoutCmdType.PropertyObject)]
        public uint? StaticMesh { get; set; }

        [NetFieldExport("AltMeshIdx", RepLayoutCmdType.Property)]
        public ItemDefinitionGUID WeaponData { get; set; }

        [NetFieldExport("bDestroyOnPlayerBuildingPlacement", RepLayoutCmdType.PropertyBool)]
        public bool? bDestroyOnPlayerBuildingPlacement { get; set; }

        [NetFieldExport("SearchedMesh", RepLayoutCmdType.Ignore)]
        public uint? SearchedMesh { get; set; }

        [NetFieldExport("AltMeshIdx", RepLayoutCmdType.PropertyInt)]
        public int? AltMeshIdx { get; set; }

        [NetFieldExport("ProxyGameplayCueDamagePhysicalMagnitude", RepLayoutCmdType.PropertyFloat)]
        public float? ProxyGameplayCueDamagePhysicalMagnitude { get; set; }

        [NetFieldExport("EffectContext", RepLayoutCmdType.Property)]
        public FGameplayEffectContextHandle EffectContext { get; set; }
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Containers/Creative_Tiered_Chest.Creative_Tiered_Chest_C", ParseType.Full)]
    public class CreativeChest : Chest
    {
        [NetFieldExport("SpawnItems", RepLayoutCmdType.Property)]
        public DebuggingObject SpawnItems { get; set; }

        [NetFieldExport("PrimaryAssetName", RepLayoutCmdType.Property)]
        public DebuggingObject PrimaryAssetName { get; set; }

        [NetFieldExport("Quantity", RepLayoutCmdType.Property)]
        public DebuggingObject Quantity { get; set; }
    }
}
