using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    [NetFieldExportGroup("/Labrador/Pawn/BP_AIPawn_Labrador.BP_AIPawn_Labrador_C")]
    public class LabradorLlamaC : INetFieldExportGroup
    {
        [NetFieldExport("PawnUniqueID", RepLayoutCmdType.PropertyInt)]
        public int? PawnUniqueId { get; set; } //Type: int32 Bits: 32
    }
}