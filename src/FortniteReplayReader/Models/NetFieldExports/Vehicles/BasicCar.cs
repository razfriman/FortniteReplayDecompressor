using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Vehicles
{
    [NetFieldExportGroup("/Valet/BasicCar/Valet_BasicCar_Vehicle.Valet_BasicCar_Vehicle_C", ParseType.Normal)]
    public class BasicCar : ValetVehicle
    {
    }
    
    [NetFieldExportGroup("/Valet/BasicCar/Valet_BasicCar_Vehicle_Upgrade.Valet_BasicCar_Vehicle_Upgrade_C", ParseType.Normal)]
    public class BasicCarUpgrade : ValetVehicle
    {
    }
}
