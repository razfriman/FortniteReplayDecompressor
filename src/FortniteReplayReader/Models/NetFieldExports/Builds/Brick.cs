using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Builds
{
    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_Solid.PBWA_S1_Solid_C", ParseType.Full)]
    public class BrickWall : BaseWallStructure, IBrickStructure
    {
    }

    #region Wall Edits

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_Brace.PBWA_S1_Brace_C", ParseType.Full)]
    public class BrickBrace : BrickWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_WindowSide.PBWA_S1_WindowSide_C", ParseType.Full)]
    public class BrickWindowSide : BrickWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_WindowC.PBWA_S1_WindowC_C", ParseType.Full)]
    public class BrickWindowCenter : BrickWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_Archway.PBWA_S1_Archway_C", ParseType.Full)]
    public class BrickCenterArch : BrickWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_ArchwayLarge.PBWA_S1_ArchwayLarge_C", ParseType.Full)]
    public class BrickSideArch : BrickWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_HalfWallS.PBWA_S1_HalfWallS_C", ParseType.Full)]
    public class BrickHalfWall : BrickWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_QuarterWallS.PBWA_S1_QuarterWallS_C", ParseType.Full)]
    public class BrickQuarterWall : BrickWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_ArchwayLargeSupport.PBWA_S1_ArchwayLargeSupport_C", ParseType.Full)]
    public class BrickSideWall : BrickWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_QuarterWallHalf.PBWA_S1_QuarterWallHalf_C", ParseType.Full)]
    public class BrickQuarterHalfWall : BrickWall
    {
    }

    //Missing door and door + window edits

    #endregion


    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_StairF.PBWA_S1_StairF_C", ParseType.Full)]
    public class BrickStairs : BaseStairsStructure, IBrickStructure
    {
    }

    #region Stair Edits

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_StairW.PBWA_S1_StairW_C", ParseType.Full)]
    public class BrickHalfStairs : BrickStairs
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_StairR.PBWA_S1_StairR_C", ParseType.Full)]
    public class BrickWalkWay : BrickStairs
    {
    }

    #endregion

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_Floor.PBWA_S1_Floor_C", ParseType.Full)]
    public class BrickFloor : BaseFloorStructure, IBrickStructure
    {
    }


    #region Floor Edits

    //Name these better

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_BalconyI.PBWA_S1_BalconyI_C", ParseType.Full)]
    public class BrickFloorSingleEdit : BrickFloor
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_BalconyS.PBWA_S1_BalconyS_C", ParseType.Full)]
    public class BrickFloorDoubleSideEdit : BrickFloor
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_BalconyO.PBWA_S1_BalconyO_C", ParseType.Full)]
    public class BrickFloorTripleEdit : BrickFloor
    {
    }

    //Missing diagonal edit
    public class BrickFloorDiagonalEdit : BrickFloor
    {
    }


    #endregion


    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_RoofC.PBWA_S1_RoofC_C", ParseType.Full)]
    public class BrickRoof : BaseRoofStructure, IBrickStructure
    {
    }

    #region Roof Edits

    //Name these better

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_RoofO.PBWA_S1_RoofO_C", ParseType.Full)]
    public class BrickRoofSingleEdit : BrickRoof
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_RoofS.PBWA_S1_RoofS_C", ParseType.Full)]
    public class BrickRoofDoubleEdit : BrickRoof
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Brick/L1/PBWA_S1_RoofI.PBWA_S1_RoofI_C", ParseType.Full)]
    public class BrickRoofTripleEdit : BrickRoof
    {
    }

    //Missing "dorito" (diagonal) edit
    public class BrickRoofDiagonalEdit : BrickRoof
    {
    }

    #endregion
}
