using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Models
{
    [NetFieldExportGroup("/Script/FortniteGame.FortPoiManager")]
    public class FortPoiManager : INetFieldExportGroup
    {
        [NetFieldExport("WorldGridStart", RepLayoutCmdType.Property, 15, "WorldGridStart", "FVector2D", 64)]
        public FVector2D WorldGridStart { get; set; } //Type: FVector2D Bits: 64

        [NetFieldExport("WorldGridEnd", RepLayoutCmdType.Property, 16, "WorldGridEnd", "FVector2D", 64)]
        public FVector2D WorldGridEnd { get; set; } //Type: FVector2D Bits: 64

        [NetFieldExport("WorldGridSpacing", RepLayoutCmdType.Property, 17, "WorldGridSpacing", "FVector2D", 64)]
        public FVector2D WorldGridSpacing { get; set; } //Type: FVector2D Bits: 64

        [NetFieldExport("GridCountX", RepLayoutCmdType.PropertyInt, 18, "GridCountX", "int32", 32)]
        public int? GridCountX { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("GridCountY", RepLayoutCmdType.PropertyInt, 19, "GridCountY", "int32", 32)]
        public int? GridCountY { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("WorldGridTotalSize", RepLayoutCmdType.Property, 20, "WorldGridTotalSize", "FVector2D", 64)]
        public FVector2D WorldGridTotalSize { get; set; } //Type: FVector2D Bits: 64

        [NetFieldExport("PoiTagContainerTable", RepLayoutCmdType.DynamicArray, 21, "PoiTagContainerTable", "TArray", 2384)]
        public FGameplayTagContainer[] PoiTagContainerTable { get; set; } //Type: TArray Bits: 2384

        [NetFieldExport("PoiTagContainerTableSize", RepLayoutCmdType.PropertyInt, 24, "PoiTagContainerTableSize", "int32", 32)]
        public int? PoiTagContainerTableSize { get; set; } //Type: int32 Bits: 32

    }
}
