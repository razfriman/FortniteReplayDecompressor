using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Containers
{
    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Containers/Tiered_Chest_Athena.Tiered_Chest_Athena_C", ParseType.Full)]
    public class Chest : SearchableContainer
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Containers/Creative_Tiered_Chest.Creative_Tiered_Chest_C", ParseType.Full)]
    public class CreativeChest : Chest
    {
        [NetFieldExport("SpawnItems", RepLayoutCmdType.Ignore)]
        public DebuggingObject SpawnItems { get; set; }

        [NetFieldExport("PrimaryAssetName", RepLayoutCmdType.Ignore)]
        public DebuggingObject PrimaryAssetName { get; set; }

        [NetFieldExport("Quantity", RepLayoutCmdType.Ignore)]
        public DebuggingObject Quantity { get; set; }
    }
}
