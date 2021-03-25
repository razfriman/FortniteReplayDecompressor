using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Vehicles
{
    [NetFieldExportGroup("/Valet/BigRig/Valet_BigRig_Vehicle.Valet_BigRig_Vehicle_C", ParseType.Normal)]
    public class BigRig : ValetVehicle
    {

    }

    [NetFieldExportGroup("/Valet/BigRig/Valet_BigRig_Vehicle_Upgrade.Valet_BigRig_Vehicle_Upgrade_C", ParseType.Normal)]
    public class BigRigUpgrade : ValetVehicle
    {

    }
}
