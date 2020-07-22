using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Builds
{
    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_Solid.PBWA_M1_Solid_C", ParseType.Full)]
    public class MetalWall : BaseWallStructure, IMetalStructure
    {
    }

    #region Wall Edits

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_Brace.PBWA_M1_Brace_C", ParseType.Full)]
    public class MetalBrace : MetalWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_WindowSide.PBWA_M1_WindowSide_C", ParseType.Full)]
    public class MetalWindowSide : MetalWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_WindowC.PBWA_M1_WindowC_C", ParseType.Full)]
    public class MetalWindowCenter : MetalWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_Archway.PBWA_M1_Archway_C", ParseType.Full)]
    public class MetalCenterArch : MetalWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_ArchwayLarge.PBWA_M1_ArchwayLarge_C", ParseType.Full)]
    public class MetalSideArch : MetalWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_HalfWallS.PBWA_M1_HalfWallS_C", ParseType.Full)]
    public class MetalHalfWall : MetalWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_QuarterWallS.PBWA_M1_QuarterWallS_C", ParseType.Full)]
    public class MetalQuarterWall : MetalWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_ArchwayLargeSupport.PBWA_M1_ArchwayLargeSupport_C", ParseType.Full)]
    public class MetalSideWall : MetalWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_QuarterWallHalf.PBWA_M1_QuarterWallHalf_C", ParseType.Full)]
    public class MetalQuarterHalfWall : MetalWall
    {
    }

    //Missing door and door + window edits

    #endregion


    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_StairF.PBWA_M1_StairF_C", ParseType.Full)]
    public class MetalStairs : BaseStairsStructure, IMetalStructure
    {
    }

    #region Stair Edits

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_StairW.PBWA_M1_StairW_C", ParseType.Full)]
    public class MetalHalfStairs : MetalStairs
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_StairR.PBWA_M1_StairR_C", ParseType.Full)]
    public class MetalWalkWay : MetalStairs
    {
    }

    #endregion

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_Floor.PBWA_M1_Floor_C", ParseType.Full)]
    public class MetalFloor : BaseFloorStructure, IMetalStructure
    {
    }


    #region Floor Edits

    //Name these better

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_BalconyI.PBWA_M1_BalconyI_C", ParseType.Full)]
    public class MetalFloorSingleEdit : MetalFloor
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_BalconyS.PBWA_M1_BalconyS_C", ParseType.Full)]
    public class MetalFloorDoubleSideEdit : MetalFloor
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_BalconyO.PBWA_M1_BalconyO_C", ParseType.Full)]
    public class MetalFloorTripleEdit : MetalFloor
    {
    }

    //Missing diagonal edit
    public class MetalFloorDiagonalEdit : MetalFloor
    {
    }


    #endregion


    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_RoofC.PBWA_M1_RoofC_C", ParseType.Full)]
    public class MetalRoof : BaseRoofStructure, IMetalStructure
    {
    }

    #region Roof Edits

    //Name these better

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_RoofO.PBWA_M1_RoofO_C", ParseType.Full)]
    public class MetalRoofSingleEdit : MetalRoof
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_RoofS.PBWA_M1_RoofS_C", ParseType.Full)]
    public class MetalRoofDoubleEdit : MetalRoof
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Metal/L1/PBWA_M1_RoofI.PBWA_M1_RoofI_C", ParseType.Full)]
    public class MetalRoofTripleEdit : MetalRoof
    {
    }

    //Missing "dorito" (diagonal) edit
    public class MetalRoofDiagonalEdit : MetalRoof
    {
    }

    #endregion
}
