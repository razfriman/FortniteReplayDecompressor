using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.Weapons
{
    [NetFieldExportGroup("/Game/Weapons/FORT_BuildingTools/Blueprints/DefaultBuildingTool.DefaultBuildingTool_C", ParseType.Normal)]
    public class BuildingTools : Weapon
    {
    }

    [NetFieldExportGroup("/Game/Weapons/FORT_BuildingTools/Blueprints/DefaultEditingTool.DefaultEditingTool_C", ParseType.Normal)]
    public class EditingTools : Weapon
    {
    }
}
