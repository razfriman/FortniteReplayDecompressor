using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Models
{
    [NetFieldExportGroup("/Script/FortniteGame.FortPoiManager")]
    public class FortPoiManager : INetFieldExportGroup
    {
        [NetFieldExport("WorldGridStart", RepLayoutCmdType.Property)]
        public FVector2D WorldGridStart { get; set; } //Type: FVector2D Bits: 64

        [NetFieldExport("WorldGridEnd", RepLayoutCmdType.Property)]
        public FVector2D WorldGridEnd { get; set; } //Type: FVector2D Bits: 64

        [NetFieldExport("WorldGridSpacing", RepLayoutCmdType.Property)]
        public FVector2D WorldGridSpacing { get; set; } //Type: FVector2D Bits: 64

        [NetFieldExport("GridCountX", RepLayoutCmdType.PropertyInt)]
        public int? GridCountX { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("GridCountY", RepLayoutCmdType.PropertyInt)]
        public int? GridCountY { get; set; } //Type: int32 Bits: 32

        [NetFieldExport("WorldGridTotalSize", RepLayoutCmdType.Property)]
        public FVector2D WorldGridTotalSize { get; set; } //Type: FVector2D Bits: 64

        [NetFieldExport("PoiTagContainerTable", RepLayoutCmdType.DynamicArray)]
        public FGameplayTagContainer[] PoiTagContainerTable { get; set; } //Type: TArray Bits: 2384

        [NetFieldExport("PoiTagContainerTableSize", RepLayoutCmdType.PropertyInt)]
        public int? PoiTagContainerTableSize { get; set; } //Type: int32 Bits: 32

    }
}
