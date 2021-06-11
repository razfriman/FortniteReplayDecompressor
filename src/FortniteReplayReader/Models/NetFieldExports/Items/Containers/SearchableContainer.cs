using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Containers
{
    public class SearchableContainer : BaseContainer
    {
        [NetFieldExport("ForceMetadataRelevant", RepLayoutCmdType.Enum)]
        public int? ForceMetadataRelevant { get; set; }

        [NetFieldExport("StaticMesh", RepLayoutCmdType.PropertyObject)]
        public uint? StaticMesh { get; set; }

        [NetFieldExport("AltMeshIdx", RepLayoutCmdType.Property)]
        public ItemDefinitionGUID WeaponData { get; set; }

        [NetFieldExport("bDestroyOnPlayerBuildingPlacement", RepLayoutCmdType.PropertyBool)]
        public bool? bDestroyOnPlayerBuildingPlacement { get; set; }

        [NetFieldExport("bInstantDeath", RepLayoutCmdType.PropertyBool)]
        public bool? bInstantDeath { get; set; }

        [NetFieldExport("SearchedMesh", RepLayoutCmdType.Ignore)]
        public uint? SearchedMesh { get; set; }

        [NetFieldExport("AltMeshIdx", RepLayoutCmdType.PropertyInt)]
        public int? AltMeshIdx { get; set; }

        [NetFieldExport("ProxyGameplayCueDamagePhysicalMagnitude", RepLayoutCmdType.PropertyFloat)]
        public float? ProxyGameplayCueDamagePhysicalMagnitude { get; set; }

        [NetFieldExport("EffectContext", RepLayoutCmdType.Property)]
        public FGameplayEffectContextHandle EffectContext { get; set; }

        [NetFieldExport("ChosenRandomUpgrade", RepLayoutCmdType.PropertyInt)]
        public int? ChosenRandomUpgrade { get; set; }

        [NetFieldExport("bMirrored", RepLayoutCmdType.PropertyBool)]
        public bool? bMirrored { get; set; }

        [NetFieldExport("ReplicatedDrawScale3D", RepLayoutCmdType.PropertyVector100)]
        public FVector? ReplicatedDrawScale3D { get; set; }

        [NetFieldExport("bIsInitiallyBuilding", RepLayoutCmdType.PropertyBool)]
        public bool? bIsInitiallyBuilding { get; set; }

        [NetFieldExport("bForceReplayRollback", RepLayoutCmdType.PropertyBool)]
        public bool? bForceReplayRollback { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "ForceMetadataRelevant":
					ForceMetadataRelevant = (int)value;
					break;
				case "StaticMesh":
					StaticMesh = (uint)value;
					break;
				case "bDestroyOnPlayerBuildingPlacement":
					bDestroyOnPlayerBuildingPlacement = (bool)value;
					break;
				case "bInstantDeath":
					bInstantDeath = (bool)value;
					break;
				case "SearchedMesh":
					SearchedMesh = (uint)value;
					break;
				case "AltMeshIdx":
					AltMeshIdx = (int)value;
					break;
				case "ProxyGameplayCueDamagePhysicalMagnitude":
					ProxyGameplayCueDamagePhysicalMagnitude = (float)value;
					break;
				case "EffectContext":
					EffectContext = (FGameplayEffectContextHandle)value;
					break;
				case "ChosenRandomUpgrade":
					ChosenRandomUpgrade = (int)value;
					break;
				case "bMirrored":
					bMirrored = (bool)value;
					break;
				case "ReplicatedDrawScale3D":
					ReplicatedDrawScale3D = (FVector)value;
					break;
				case "bIsInitiallyBuilding":
					bIsInitiallyBuilding = (bool)value;
					break;
				case "bForceReplayRollback":
					bForceReplayRollback = (bool)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }
}
