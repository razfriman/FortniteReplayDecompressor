using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Builds
{
    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_Solid.PBWA_W1_Solid_C", ParseType.Full)]
    public class WoodWall : BaseWallStructure, IWoodStructure
    {
    }

    #region Wall Edits

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_Brace.PBWA_W1_Brace_C", ParseType.Full)]
    public class WoodBrace : WoodWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_WindowSide.PBWA_W1_WindowSide_C", ParseType.Full)]
    public class WoodWindowSide : WoodWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_WindowC.PBWA_W1_WindowC_C", ParseType.Full)]
    public class WoodWindowCenter : WoodWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_Archway.PBWA_W1_Archway_C", ParseType.Full)]
    public class WoodCenterArch : WoodWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_ArchwayLarge.PBWA_W1_ArchwayLarge_C", ParseType.Full)]
    public class WoodSideArch : WoodWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_HalfWallS.PBWA_W1_HalfWallS_C", ParseType.Full)]
    public class WoodHalfWall : WoodWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_QuarterWallS.PBWA_W1_QuarterWallS_C", ParseType.Full)]
    public class WoodQuarterWall : WoodWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_ArchwayLargeSupport.PBWA_W1_ArchwayLargeSupport_C", ParseType.Full)]
    public class WoodSideWall : WoodWall
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_QuarterWallHalf.PBWA_W1_QuarterWallHalf_C", ParseType.Full)]
    public class WoodQuarterHalfWall : WoodWall
    {
    }

    //Missing door and door + window edits

    #endregion


    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_StairF.PBWA_W1_StairF_C", ParseType.Full)]
    public class WoodStairs : BaseStairsStructure, IWoodStructure
    {
    }

    #region Stair Edits

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_StairW.PBWA_W1_StairW_C", ParseType.Full)]
    public class WoodHalfStairs : WoodStairs
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_StairR.PBWA_W1_StairR_C", ParseType.Full)]
    public class WoodWalkWay : WoodStairs
    {
    }

    #endregion

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_Floor.PBWA_W1_Floor_C", ParseType.Full)]
    public class WoodFloor : BaseFloorStructure, IWoodStructure
    {
    }


    #region Floor Edits

    //Name these better

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_BalconyI.PBWA_W1_BalconyI_C", ParseType.Full)]
    public class WoodFloorSingleEdit : WoodFloor
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_BalconyS.PBWA_W1_BalconyS_C", ParseType.Full)]
    public class WoodFloorDoubleSideEdit : WoodFloor
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_BalconyO.PBWA_W1_BalconyO_C", ParseType.Full)]
    public class WoodFloorTripleEdit : WoodFloor
    {
    }

    //Missing diagonal edit
    public class WoodFloorDiagonalEdit : WoodFloor
    {
    }


    #endregion


    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_RoofC.PBWA_W1_RoofC_C", ParseType.Full)]
    public class WoodRoof : BaseRoofStructure, IWoodStructure
    {
    }

    #region Roof Edits

    //Name these better

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_RoofO.PBWA_W1_RoofO_C", ParseType.Full)]
    public class WoodRoofSingleEdit : WoodRoof
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_RoofS.PBWA_W1_RoofS_C", ParseType.Full)]
    public class WoodRoofDoubleEdit : WoodRoof
    {
    }

    [NetFieldExportGroup("/Game/Building/ActorBlueprints/Player/Wood/L1/PBWA_W1_RoofI.PBWA_W1_RoofI_C", ParseType.Full)]
    public class WoodRoofTripleEdit : WoodRoof
    {
    }

    //Missing "dorito" (diagonal) edit
    public class WoodRoofDiagonalEdit : WoodRoof
    {
    }

    #endregion
}
