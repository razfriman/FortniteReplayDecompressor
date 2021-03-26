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

		public override bool ManualRead(string property, object value)
		{
			switch (property)
			{
				case "DamageableParts":
					DamageableParts = (ValetVehicle[])value;
					break;
				case "ConfigIndex":
					ConfigIndex = (int)value;
					break;
				case "BoneIndex":
					BoneIndex = (int)value;
					break;
				case "ShapeIndex":
					ShapeIndex = (int)value;
					break;
				case "Health":
					Health = (float)value;
					break;
				case "MaxFuel":
					MaxFuel = (float)value;
					break;
				case "FuelPerSecondDriving":
					FuelPerSecondDriving = (float)value;
					break;
				case "FuelPerSecondBoosting":
					FuelPerSecondBoosting = (float)value;
					break;
				case "SpringStiffMultiplier":
					SpringStiffMultiplier = (float)value;
					break;
				case "SpringDampMultiplier":
					SpringDampMultiplier = (float)value;
					break;
				case "SpringLengthMultiplier":
					SpringLengthMultiplier = (float)value;
					break;
				case "RearSpringLengthMultiplier":
					RearSpringLengthMultiplier = (float)value;
					break;
				case "GravityMultiplier":
					GravityMultiplier = (float)value;
					break;
				case "GearInfos":
					GearInfos = (ValetVehicle[])value;
					break;
				case "GearIndex":
					GearIndex = (int)value;
					break;
				case "TopSpeed":
					TopSpeed = (float)value;
					break;
				case "MinSpeed":
					MinSpeed = (float)value;
					break;
				case "PushForce":
					PushForce = (float)value;
					break;
				case "TireStates":
					TireStates = (ValetVehicle[])value;
					break;
				case "bIsInoperable":
					bIsInoperable = (bool)value;
					break;
				case "RandomSeed":
					RandomSeed = (int)value;
					break;
				case "RandomMatInt":
					RandomMatInt = (int)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}
	}
}
