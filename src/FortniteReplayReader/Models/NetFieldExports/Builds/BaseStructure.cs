using FortniteReplayReader.Models.NetFieldExports.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Builds
{
    public abstract class BaseStructure : INetFieldExportGroup
    {
        [NetFieldExport("bHidden", RepLayoutCmdType.Ignore)]
        public bool? bHidden { get; set; }

        [NetFieldExport("RemoteRole", RepLayoutCmdType.Ignore)]
        public int? RemoteRole { get; set; }

        [NetFieldExport("Role", RepLayoutCmdType.Ignore)]
        public int? Role { get; set; }

        [NetFieldExport("OwnerPersistentID", RepLayoutCmdType.PropertyInt16)]
        public short? OwnerPersistentID { get; set; }

        [NetFieldExport("bDestroyed", RepLayoutCmdType.PropertyBool)]
        public bool? bDestroyed { get; set; }

        [NetFieldExport("bPlayerPlaced", RepLayoutCmdType.PropertyBool)]
        public bool? bPlayerPlaced { get; set; }

        [NetFieldExport("bInstantDeath", RepLayoutCmdType.PropertyBool)]
        public bool? bInstantDeath { get; set; }

        [NetFieldExport("bCollisionBlockedByPawns", RepLayoutCmdType.PropertyBool)]
        public bool? bCollisionBlockedByPawns { get; set; }

        [NetFieldExport("bIsInitiallyBuilding", RepLayoutCmdType.PropertyBool)]
        public bool? bIsInitiallyBuilding { get; set; }

        [NetFieldExport("TeamIndex", RepLayoutCmdType.Enum)]
        public int? TeamIndex { get; set; }

        [NetFieldExport("BuildingAnimation", RepLayoutCmdType.Enum)]
        public EBuildingAnim BuildingAnimation { get; set; }

        [NetFieldExport("BuildTime", RepLayoutCmdType.Ignore)]
        public FQuantizedBuildingAttribute BuildTime { get; set; }

        [NetFieldExport("RepairTime", RepLayoutCmdType.Ignore)]
        public FQuantizedBuildingAttribute RepairTime { get; set; }

        [NetFieldExport("Health", RepLayoutCmdType.PropertyInt16)]
        public short? Health { get; set; }

        [NetFieldExport("MaxHealth", RepLayoutCmdType.PropertyInt16)]
        public short? MaxHealth { get; set; }

        [NetFieldExport("EditingPlayer", RepLayoutCmdType.Property)]
        public ActorGUID EditingPlayer { get; set; }

        [NetFieldExport("ProxyGameplayCueDamagePhysicalMagnitude", RepLayoutCmdType.Ignore)]
        public DebuggingObject ProxyGameplayCueDamagePhysicalMagnitude { get; set; }

        [NetFieldExport("EffectContext", RepLayoutCmdType.Ignore)]
        public DebuggingObject EffectContext { get; set; }

        [NetFieldExport("bAttachmentPlacementBlockedFront", RepLayoutCmdType.PropertyBool)]
        public bool? bAttachmentPlacementBlockedFront { get; set; }

        [NetFieldExport("bAttachmentPlacementBlockedBack", RepLayoutCmdType.PropertyBool)]
        public bool? bAttachmentPlacementBlockedBack { get; set; }

        [NetFieldExport("bUnderConstruction", RepLayoutCmdType.PropertyBool)]
        public bool? bUnderConstruction { get; set; }

        [NetFieldExport("StaticMesh", RepLayoutCmdType.Ignore)]
        public uint? StaticMesh { get; set; }

        [NetFieldExport("Gnomed", RepLayoutCmdType.PropertyBool)]
        public bool? Gnomed { get; set; }

        [NetFieldExport("InitialOverlappingVehicles", RepLayoutCmdType.Ignore)]
        public DebuggingObject InitialOverlappingVehicles { get; set; }

        [NetFieldExport("ReplicatedDrawScale3D", RepLayoutCmdType.PropertyVector100)]
        public FVector ReplicatedDrawScale3D { get; set; }

        [NetFieldExport("bUnderRepair", RepLayoutCmdType.PropertyBool)]
        public bool? bUnderRepair { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "bHidden":
					bHidden = (bool)value;
					break;
				case "RemoteRole":
					RemoteRole = (int)value;
					break;
				case "Role":
					Role = (int)value;
					break;
				case "OwnerPersistentID":
					OwnerPersistentID = (short)value;
					break;
				case "bDestroyed":
					bDestroyed = (bool)value;
					break;
				case "bPlayerPlaced":
					bPlayerPlaced = (bool)value;
					break;
				case "bInstantDeath":
					bInstantDeath = (bool)value;
					break;
				case "bCollisionBlockedByPawns":
					bCollisionBlockedByPawns = (bool)value;
					break;
				case "bIsInitiallyBuilding":
					bIsInitiallyBuilding = (bool)value;
					break;
				case "TeamIndex":
					TeamIndex = (int)value;
					break;
				case "BuildingAnimation":
					BuildingAnimation = (EBuildingAnim)value;
					break;
				case "BuildTime":
					BuildTime = (FQuantizedBuildingAttribute)value;
					break;
				case "RepairTime":
					RepairTime = (FQuantizedBuildingAttribute)value;
					break;
				case "Health":
					Health = (short)value;
					break;
				case "MaxHealth":
					MaxHealth = (short)value;
					break;
				case "EditingPlayer":
					EditingPlayer = (ActorGUID)value;
					break;
				case "ProxyGameplayCueDamagePhysicalMagnitude":
					ProxyGameplayCueDamagePhysicalMagnitude = (DebuggingObject)value;
					break;
				case "EffectContext":
					EffectContext = (DebuggingObject)value;
					break;
				case "bAttachmentPlacementBlockedFront":
					bAttachmentPlacementBlockedFront = (bool)value;
					break;
				case "bAttachmentPlacementBlockedBack":
					bAttachmentPlacementBlockedBack = (bool)value;
					break;
				case "bUnderConstruction":
					bUnderConstruction = (bool)value;
					break;
				case "StaticMesh":
					StaticMesh = (uint)value;
					break;
				case "Gnomed":
					Gnomed = (bool)value;
					break;
				case "InitialOverlappingVehicles":
					InitialOverlappingVehicles = (DebuggingObject)value;
					break;
				case "ReplicatedDrawScale3D":
					ReplicatedDrawScale3D = (FVector)value;
					break;
				case "bUnderRepair":
					bUnderRepair = (bool)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }

    public class BaseFloorStructure : BaseStructure
    { 
    }

    public class BaseWallStructure : BaseStructure
    {
    }

    public class BaseStairsStructure : BaseStructure
    {
        [NetFieldExport("bMirrored", RepLayoutCmdType.PropertyBool)]
        public bool? bMirrored { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "bMirrored":
					bMirrored = (bool)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }

    public class BaseRoofStructure : BaseStructure
    {
    }

    public interface IWoodStructure
    {
    }

    public interface IBrickStructure
    {
    }

    public interface IMetalStructure
    {

    }
	//Maybe add base edits?
}
