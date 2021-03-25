using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Vehicles
{
    [NetFieldExportGroup("/Valet/SportsCar/Valet_SportsCar_Vehicle.Valet_SportsCar_Vehicle_C", ParseType.Normal)]
    public class SportsCar : ValetVehicle
    {
    }

    [NetFieldExportGroup("/Valet/SportsCar/Valet_SportsCar_Vehicle_Upgrade.Valet_SportsCar_Vehicle_Upgrade_C", ParseType.Normal)]
    public class SportsCarUpgrade : ValetVehicle
    {
    }

    [NetFieldExportGroup("/Valet/SportsCar/Valet_SportsCar_Vehicle_Turbo.Valet_SportsCar_Vehicle_Turbo_C", ParseType.Normal)]
    public class SportsCarTrubo : ValetVehicle
    {
    }
}
