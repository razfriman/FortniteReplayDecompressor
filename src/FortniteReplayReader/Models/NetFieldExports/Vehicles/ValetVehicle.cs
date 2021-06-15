using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unreal.Core.Attributes;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Vehicles
{
	public class ValetVehicle : BaseVehicle
	{
		[NetFieldExport("DamageableParts", RepLayoutCmdType.DynamicArray)]
		public ValetVehicle[] DamageableParts { get; set; }

		[NetFieldExport("ConfigIndex", RepLayoutCmdType.PropertyInt)]
		public int? ConfigIndex { get; set; }

		[NetFieldExport("BoneIndex", RepLayoutCmdType.PropertyInt)]
		public int? BoneIndex { get; set; }

		[NetFieldExport("ShapeIndex", RepLayoutCmdType.PropertyInt)]
		public int? ShapeIndex { get; set; }

		[NetFieldExport("Health", RepLayoutCmdType.PropertyFloat)]
		public float? Health { get; set; }

		[NetFieldExport("MaxFuel", RepLayoutCmdType.PropertyFloat)]
		public float? MaxFuel { get; set; }

		[NetFieldExport("FuelPerSecondDriving", RepLayoutCmdType.PropertyFloat)]
		public float? FuelPerSecondDriving { get; set; }

		[NetFieldExport("FuelPerSecondBoosting", RepLayoutCmdType.PropertyFloat)]
		public float? FuelPerSecondBoosting { get; set; }

		[NetFieldExport("SpringStiffMultiplier", RepLayoutCmdType.PropertyFloat)]
		public float? SpringStiffMultiplier { get; set; }

		[NetFieldExport("SpringDampMultiplier", RepLayoutCmdType.PropertyFloat)]
		public float? SpringDampMultiplier { get; set; }

		[NetFieldExport("SpringLengthMultiplier", RepLayoutCmdType.PropertyFloat)]
		public float? SpringLengthMultiplier { get; set; }

		[NetFieldExport("RearSpringLengthMultiplier", RepLayoutCmdType.PropertyFloat)]
		public float? RearSpringLengthMultiplier { get; set; }

		[NetFieldExport("GravityMultiplier", RepLayoutCmdType.PropertyFloat)]
		public float? GravityMultiplier { get; set; }

		[NetFieldExport("GearInfos", RepLayoutCmdType.DynamicArray)]
		public ValetVehicle[] GearInfos { get; set; }

		[NetFieldExport("GearIndex", RepLayoutCmdType.PropertyInt)]
		public int? GearIndex { get; set; }

		[NetFieldExport("TopSpeed", RepLayoutCmdType.PropertyFloat)]
		public float? TopSpeed { get; set; }

		[NetFieldExport("MinSpeed", RepLayoutCmdType.PropertyFloat)]
		public float? MinSpeed { get; set; }

		[NetFieldExport("PushForce", RepLayoutCmdType.PropertyFloat)]
		public float? PushForce { get; set; }

		[NetFieldExport("TireStates", RepLayoutCmdType.DynamicArray)]
		public ValetVehicle[] TireStates { get; set; }

		[NetFieldExport("bIsInoperable", RepLayoutCmdType.PropertyBool)]
		public bool? bIsInoperable { get; set; }

		[NetFieldExport("RandomSeed", RepLayoutCmdType.PropertyInt)]
		public int? RandomSeed { get; set; }

		[NetFieldExport("RandomMatInt", RepLayoutCmdType.PropertyInt)]
		public int? RandomMatInt { get; set; }
	}
}
