using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Functions
{
    [NetFieldExportGroup("/Script/FortniteGame.FortPawn:NetMulticast_InvokeGameplayCueAdded_WithParams", ParseType.Normal)]
    public class GameplayCue : INetFieldExportGroup
    {
        [NetFieldExport("GameplayCueTag", RepLayoutCmdType.Property)]
        public FGameplayTag GameplayCueTag { get; set; }

        [NetFieldExport("PredictionKey", RepLayoutCmdType.Property)]
        public FPredictionKey PredictionKey { get; set; }

        [NetFieldExport("Parameters", RepLayoutCmdType.Ignore)]
        public FGameplayCueParameters Parameters { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "GameplayCueTag":
					GameplayCueTag = (FGameplayTag)value;
					break;
				case "PredictionKey":
					PredictionKey = (FPredictionKey)value;
					break;
				case "Parameters":
					Parameters = (FGameplayCueParameters)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }


    [NetFieldExportGroup("/Script/FortniteGame.FortPawn:NetMulticast_InvokeGameplayCueExecuted_FromSpec", ParseType.Normal)]
    public class GameplayCueExecuted : GameplayCue
    {
        [NetFieldExport("Def", RepLayoutCmdType.Property)]
        public NetworkGUID Def { get; set; }

        [NetFieldExport("ModifiedAttributes", RepLayoutCmdType.Ignore)]
        public DebuggingObject[] ModifiedAttributes { get; set; }

        [NetFieldExport("AttributeName", RepLayoutCmdType.PropertyName)]
        public string AttributeName { get; set; }

        [NetFieldExport("Attribute", RepLayoutCmdType.Property)]
        public NetworkGUID Attribute { get; set; }

        [NetFieldExport("AttributeOwner", RepLayoutCmdType.Property)]
        public NetworkGUID AttributeOwner { get; set; }

        [NetFieldExport("TotalMagnitude", RepLayoutCmdType.PropertyFloat)]
        public float? TotalMagnitude { get; set; }

        [NetFieldExport("EffectContext", RepLayoutCmdType.Property)]
        public FGameplayEffectContextHandle EffectContext { get; set; }

        [NetFieldExport("AggregatedSourceTags", RepLayoutCmdType.Property)]
        public FGameplayTagContainer AggregatedSourceTags { get; set; }

        [NetFieldExport("AggregatedTargetTags", RepLayoutCmdType.Property)]
        public FGameplayTagContainer AggregatedTargetTags { get; set; }

        [NetFieldExport("Level", RepLayoutCmdType.PropertyFloat)]
        public float? Level { get; set; }
        
        [NetFieldExport("AbilityLevel", RepLayoutCmdType.PropertyFloat)]
        public float? AbilityLevel { get; set; }

        [NetFieldExport("PredictionKey", RepLayoutCmdType.Property)]
        public FPredictionKey PredictionKey { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "Def":
					Def = (NetworkGUID)value;
					break;
				case "ModifiedAttributes":
					ModifiedAttributes = (DebuggingObject[])value;
					break;
				case "AttributeName":
					AttributeName = (string)value;
					break;
				case "Attribute":
					Attribute = (NetworkGUID)value;
					break;
				case "AttributeOwner":
					AttributeOwner = (NetworkGUID)value;
					break;
				case "TotalMagnitude":
					TotalMagnitude = (float)value;
					break;
				case "EffectContext":
					EffectContext = (FGameplayEffectContextHandle)value;
					break;
				case "AggregatedSourceTags":
					AggregatedSourceTags = (FGameplayTagContainer)value;
					break;
				case "AggregatedTargetTags":
					AggregatedTargetTags = (FGameplayTagContainer)value;
					break;
				case "Level":
					Level = (float)value;
					break;
				case "AbilityLevel":
					AbilityLevel = (float)value;
					break;
				case "PredictionKey":
					PredictionKey = (FPredictionKey)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }

    [NetFieldExportGroup("/Script/FortniteGame.FortPawn:NetMulticast_InvokeGameplayCueExecuted_WithParams", ParseType.Normal)]
    public class GameplayCueExecutedWithParams : GameplayCue
    {
        [NetFieldExport("GameplayCueParameters", RepLayoutCmdType.Ignore)]
        public FGameplayCueParameters GameplayCueParameters { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "GameplayCueParameters":
					GameplayCueParameters = (FGameplayCueParameters)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }
}
