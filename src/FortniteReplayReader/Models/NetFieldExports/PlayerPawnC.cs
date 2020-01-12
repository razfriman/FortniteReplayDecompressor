using System.Collections.Generic;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports
{
    /*
    [NetFieldExportGroup("/Script/FortniteGame.FortPlayerPawnAthena:FastSharedReplication")]
    public class FortPlayerPawnFastSharedReplication : INetFieldExportGroup
    {
        [NetFieldExport("SharedRepMovement", RepLayoutCmdType.RepMovement)]
        public FRepMovement SharedRepMovement { get; set; }
    }*/

    [NetFieldExportGroup("/Game/Athena/PlayerPawn_Athena.PlayerPawn_Athena_C", ParseType.Normal)]
	public class PlayerPawnC : INetFieldExportGroup
	{
		[NetFieldExport("bHidden", RepLayoutCmdType.PropertyBool)]
		public bool? bHidden { get; set; } //Type:  Bits: 1

		[NetFieldExport("bReplicateMovement", RepLayoutCmdType.PropertyBool)]
		public bool? bReplicateMovement { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bTearOff", RepLayoutCmdType.PropertyBool)]
		public bool? bTearOff { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bCanBeDamaged", RepLayoutCmdType.PropertyBool)]
		public bool? bCanBeDamaged { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("RemoteRole", RepLayoutCmdType.Enum)]
		public int? RemoteRole { get; set; } //Type: TEnumAsByte<ENetRole> Bits: 2

		[NetFieldExport("ReplicatedMovement", RepLayoutCmdType.RepMovement)]
		public FRepMovement ReplicatedMovement { get; set; } //Type: FRepMovement Bits: 127

		[NetFieldExport("AttachParent", RepLayoutCmdType.Property)]
		public ActorGUID AttachParent { get; set; } //Type: AActor* Bits: 16

		[NetFieldExport("LocationOffset", RepLayoutCmdType.PropertyVector100)]
		public FVector LocationOffset { get; set; } //Type: FVector_NetQuantize100 Bits: 20

		[NetFieldExport("RelativeScale3D", RepLayoutCmdType.PropertyVector100)]
		public FVector RelativeScale3D { get; set; } //Type: FVector_NetQuantize100 Bits: 29

		[NetFieldExport("RotationOffset", RepLayoutCmdType.PropertyRotator)]
		public FRotator RotationOffset { get; set; } //Type: FRotator Bits: 3

		[NetFieldExport("AttachSocket", RepLayoutCmdType.Property)]
		public FName AttachSocket { get; set; } //Type: FName Bits: 121

		[NetFieldExport("AttachComponent", RepLayoutCmdType.Property)]
		public UObjectGUID AttachComponent { get; set; } //Type: USceneComponent* Bits: 16

		[NetFieldExport("Role", RepLayoutCmdType.Enum)]
		public int? Role { get; set; } //Type: Enum Bits: 2

		[NetFieldExport("Instigator", RepLayoutCmdType.Property)]
		public ActorGUID Instigator { get; set; } //Type: APawn* Bits: 8

		[NetFieldExport("RemoteViewPitch", RepLayoutCmdType.PropertyByte)]
		public byte? RemoteViewPitch { get; set; } //Type: uint8 Bits: 8

		[NetFieldExport("PlayerState", RepLayoutCmdType.Property)]
		public ActorGUID PlayerState { get; set; } //Type: APlayerState* Bits: 8

		[NetFieldExport("Controller", RepLayoutCmdType.Property)]
		public ActorGUID Controller { get; set; } //Type: AController* Bits: 8

		[NetFieldExport("MovementBase", RepLayoutCmdType.Property)]
		public UObjectGUID MovementBase { get; set; } //Type: UPrimitiveComponent* Bits: 16

		[NetFieldExport("BoneName", RepLayoutCmdType.Property)]
		public FName BoneName { get; set; } //Type: FName Bits: 113

		[NetFieldExport("Location", RepLayoutCmdType.PropertyVector100)]
		public FVector Location { get; set; } //Type: FVector Bits: 47

		[NetFieldExport("Rotation", RepLayoutCmdType.PropertyRotator)]
		public FRotator Rotation { get; set; } //Type: FRotator Bits: 19

		[NetFieldExport("bServerHasBaseComponent", RepLayoutCmdType.PropertyBool)]
		public bool? bServerHasBaseComponent { get; set; } //Type: bool Bits: 1

		[NetFieldExport("bRelativeRotation", RepLayoutCmdType.PropertyBool)]
		public bool? bRelativeRotation { get; set; } //Type: bool Bits: 1

		[NetFieldExport("bServerHasVelocity", RepLayoutCmdType.PropertyBool)]
		public bool? bServerHasVelocity { get; set; } //Type: bool Bits: 1

		[NetFieldExport("ReplayLastTransformUpdateTimeStamp", RepLayoutCmdType.PropertyFloat)]
		public float? ReplayLastTransformUpdateTimeStamp { get; set; } //Type: float Bits: 32

		[NetFieldExport("ReplicatedMovementMode", RepLayoutCmdType.PropertyByte)]
		public byte? ReplicatedMovementMode { get; set; } //Type: uint8 Bits: 8

		[NetFieldExport("bIsCrouched", RepLayoutCmdType.PropertyBool)]
		public bool? bIsCrouched { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bProxyIsJumpForceApplied", RepLayoutCmdType.PropertyBool)]
		public bool? bProxyIsJumpForceApplied { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bIsActive", RepLayoutCmdType.PropertyBool)]
		public bool? bIsActive { get; set; } //Type: bool Bits: 1

		[NetFieldExport("Position", RepLayoutCmdType.PropertyFloat)]
		public float? Position { get; set; } //Type: float Bits: 32

		[NetFieldExport("Acceleration", RepLayoutCmdType.PropertyVector10)]
		public FVector Acceleration { get; set; } //Type:  Bits: 46

		[NetFieldExport("LinearVelocity", RepLayoutCmdType.PropertyVector10)]
		public FVector LinearVelocity { get; set; } //Type: FVector_NetQuantize10 Bits: 26

		[NetFieldExport("CurrentMovementStyle", RepLayoutCmdType.Enum)]
		public int? CurrentMovementStyle { get; set; } //Type: TEnumAsByte<EFortMovementStyle::Type> Bits: 3

		[NetFieldExport("bIgnoreNextFallingDamage", RepLayoutCmdType.PropertyBool)]
		public bool? bIgnoreNextFallingDamage { get; set; } //Type:  Bits: 1

		[NetFieldExport("TeleportCounter", RepLayoutCmdType.PropertyByte)]
		public byte? TeleportCounter { get; set; } //Type: uint8 Bits: 8

		[NetFieldExport("PawnUniqueID", RepLayoutCmdType.PropertyInt)]
		public int? PawnUniqueID { get; set; } //Type: int32 Bits: 32

		[NetFieldExport("bIsDying", RepLayoutCmdType.PropertyBool)]
		public bool? bIsDying { get; set; } //Type: bool Bits: 1

		[NetFieldExport("CurrentWeapon", RepLayoutCmdType.Property)]
		public ActorGUID CurrentWeapon { get; set; } //Type: AFortWeapon* Bits: 16

		[NetFieldExport("bIsInvulnerable", RepLayoutCmdType.PropertyBool)]
		public bool? bIsInvulnerable { get; set; } //Type:  Bits: 1

		[NetFieldExport("bMovingEmote", RepLayoutCmdType.PropertyBool)]
		public bool? bMovingEmote { get; set; } //Type:  Bits: 1

		[NetFieldExport("bWeaponActivated", RepLayoutCmdType.PropertyBool)]
		public bool? bWeaponActivated { get; set; } //Type:  Bits: 1

		[NetFieldExport("bIsDBNO", RepLayoutCmdType.PropertyBool)]
		public bool? bIsDBNO { get; set; } //Type:  Bits: 1

		[NetFieldExport("bWasDBNOOnDeath", RepLayoutCmdType.PropertyBool)]
		public bool? bWasDBNOOnDeath { get; set; } //Type:  Bits: 1

		[NetFieldExport("JumpFlashCount", RepLayoutCmdType.PropertyByte)]
		public byte? JumpFlashCount { get; set; } //Type: uint8 Bits: 8

		[NetFieldExport("bWeaponHolstered", RepLayoutCmdType.PropertyBool)]
		public bool? bWeaponHolstered { get; set; } //Type: bool Bits: 1

		[NetFieldExport("FeedbackAudioComponent", RepLayoutCmdType.Property)]
		public UObjectGUID FeedbackAudioComponent { get; set; } //Type: UAudioComponent* Bits: 0

		[NetFieldExport("VocalChords", RepLayoutCmdType.DynamicArray)]
		public PlayerPawnC[] VocalChords { get; set; } //Type: TArray Bits: 115

		[NetFieldExport("SpawnImmunityTime", RepLayoutCmdType.PropertyFloat)]
		public float? SpawnImmunityTime { get; set; } //Type: float Bits: 32

		[NetFieldExport("JumpFlashCountPacked", RepLayoutCmdType.Property)]
		public DebuggingObject JumpFlashCountPacked { get; set; } //Type:  Bits: 8

		[NetFieldExport("LandingFlashCountPacked", RepLayoutCmdType.Property)]
		public DebuggingObject LandingFlashCountPacked { get; set; } //Type:  Bits: 8

		[NetFieldExport("bInterruptCurrentLine", RepLayoutCmdType.PropertyBool)]
		public bool? bInterruptCurrentLine { get; set; } //Type: bool Bits: 33

		[NetFieldExport("LastReplicatedEmoteExecuted", RepLayoutCmdType.PropertyObject)]
		public uint? LastReplicatedEmoteExecuted { get; set; } //Type: * Bits: 16

		[NetFieldExport("bCanBeInterrupted", RepLayoutCmdType.PropertyBool)]
		public bool? bCanBeInterrupted { get; set; } //Type: bool Bits: 95

		[NetFieldExport("bCanQue", RepLayoutCmdType.PropertyBool)]
		public bool? bCanQue { get; set; } //Type: bool Bits: 0

		[NetFieldExport("ForwardAlpha", RepLayoutCmdType.PropertyFloat)]
		public float? ForwardAlpha { get; set; } //Type: float Bits: 32

		[NetFieldExport("RightAlpha", RepLayoutCmdType.PropertyFloat)]
		public float? RightAlpha { get; set; } //Type: float Bits: 32

		[NetFieldExport("TurnDelta", RepLayoutCmdType.PropertyFloat)]
		public float? TurnDelta { get; set; } //Type: float Bits: 32

		[NetFieldExport("SteerAlpha", RepLayoutCmdType.PropertyFloat)]
		public float? SteerAlpha { get; set; } //Type: float Bits: 32

		[NetFieldExport("GravityScale", RepLayoutCmdType.PropertyFloat)]
		public float? GravityScale { get; set; } //Type: float Bits: 32

		[NetFieldExport("WorldLookDir", RepLayoutCmdType.PropertyVectorQ)]
		public FVector WorldLookDir { get; set; } //Type: FVector_NetQuantize Bits: 11

		[NetFieldExport("bIgnoreForwardInAir", RepLayoutCmdType.PropertyBool)]
		public bool? bIgnoreForwardInAir { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bIsHonking", RepLayoutCmdType.PropertyBool)]
		public bool? bIsHonking { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bIsJumping", RepLayoutCmdType.PropertyBool)]
		public bool? bIsJumping { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("bIsSprinting", RepLayoutCmdType.PropertyBool)]
		public bool? bIsSprinting { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("Vehicle", RepLayoutCmdType.Property)]
		public ActorGUID Vehicle { get; set; } //Type: AFortAthenaVehicle* Bits: 16

		[NetFieldExport("VehicleApexZ", RepLayoutCmdType.PropertyFloat)]
		public float? VehicleApexZ { get; set; } //Type: float Bits: 32

		[NetFieldExport("SeatIndex", RepLayoutCmdType.PropertyByte)]
		public byte? SeatIndex { get; set; } //Type: uint8 Bits: 8

		[NetFieldExport("bIsWaterJump", RepLayoutCmdType.PropertyBool)]
		public bool? bIsWaterJump { get; set; } //Type:  Bits: 1

		[NetFieldExport("bIsWaterSprintBoost", RepLayoutCmdType.PropertyBool)]
		public bool? bIsWaterSprintBoost { get; set; } //Type:  Bits: 1

		[NetFieldExport("bIsWaterSprintBoostPending", RepLayoutCmdType.PropertyBool)]
		public bool? bIsWaterSprintBoostPending { get; set; } //Type:  Bits: 1

		[NetFieldExport("StasisMode", RepLayoutCmdType.Ignore)]
		public object StasisMode { get; set; } //Type: Enum Bits: 3

		[NetFieldExport("BuildingState", RepLayoutCmdType.Enum)]
		public int? BuildingState { get; set; } //Type: TEnumAsByte<EFortBuildingState::Type> Bits: 2

		[NetFieldExport("bIsTargeting", RepLayoutCmdType.PropertyBool)]
		public bool? bIsTargeting { get; set; } //Type: bool Bits: 1

		[NetFieldExport("PawnMontage", RepLayoutCmdType.Property)]
		public UObjectGUID PawnMontage { get; set; } //Type: UAnimMontage* Bits: 16

		[NetFieldExport("bPlayBit", RepLayoutCmdType.PropertyBool)]
		public bool? bPlayBit { get; set; } //Type: bool Bits: 1

		[NetFieldExport("bIsPlayingEmote", RepLayoutCmdType.PropertyBool)]
		public bool? bIsPlayingEmote { get; set; } //Type:  Bits: 1

		[NetFieldExport("FootstepBankOverride", RepLayoutCmdType.Property)]
		public UObjectGUID FootstepBankOverride { get; set; } //Type: UFortFootstepAudioBank* Bits: 16

		[NetFieldExport("PackedReplicatedSlopeAngles", RepLayoutCmdType.PropertyUInt16)]
		public ushort? PackedReplicatedSlopeAngles { get; set; } //Type: uint16 Bits: 16

		[NetFieldExport("bStartedInteractSearch", RepLayoutCmdType.PropertyBool)]
		public bool? bStartedInteractSearch { get; set; } //Type:  Bits: 1

		[NetFieldExport("AccelerationPack", RepLayoutCmdType.PropertyUInt16)]
		public ushort? AccelerationPack { get; set; } //Type: uint16 Bits: 16

		[NetFieldExport("AccelerationZPack", RepLayoutCmdType.PropertyByte)]
		public byte? AccelerationZPack { get; set; } //Type: int8 Bits: 8

		[NetFieldExport("bIsWaitingForEmoteInteraction", RepLayoutCmdType.PropertyBool)]
		public bool? bIsWaitingForEmoteInteraction { get; set; } //Type:  Bits: 1

		[NetFieldExport("GroupEmoteLookTarget", RepLayoutCmdType.Property)]
		public NetworkGUID GroupEmoteLookTarget { get; set; } //Type:  Bits: 16

		[NetFieldExport("bIsSkydiving", RepLayoutCmdType.PropertyBool)]
		public bool? bIsSkydiving { get; set; } //Type: bool Bits: 1

		[NetFieldExport("bIsParachuteOpen", RepLayoutCmdType.PropertyBool)]
		public bool? bIsParachuteOpen { get; set; } //Type: bool Bits: 1

		[NetFieldExport("bIsParachuteForcedOpen", RepLayoutCmdType.PropertyBool)]
		public bool? bIsParachuteForcedOpen { get; set; } //Type: bool Bits: 1

		[NetFieldExport("bIsSkydivingFromBus", RepLayoutCmdType.PropertyBool)]
		public bool? bIsSkydivingFromBus { get; set; } //Type: bool Bits: 1

		[NetFieldExport("bReplicatedIsInSlipperyMovement", RepLayoutCmdType.PropertyBool)]
		public bool? bReplicatedIsInSlipperyMovement { get; set; } //Type: bool Bits: 1

		[NetFieldExport("MovementDir", RepLayoutCmdType.Ignore)]
		public object MovementDir { get; set; } //Type:  Bits: 26

		[NetFieldExport("bIsInAnyStorm", RepLayoutCmdType.PropertyBool)]
		public bool? bIsInAnyStorm { get; set; } //Type:  Bits: 1

		[NetFieldExport("bIsSlopeSliding", RepLayoutCmdType.PropertyBool)]
		public bool? bIsSlopeSliding { get; set; } //Type: bool Bits: 1

		[NetFieldExport("bIsProxySimulationTimedOut", RepLayoutCmdType.PropertyBool)]
		public bool? bIsProxySimulationTimedOut { get; set; } //Type: bool Bits: 1

		[NetFieldExport("bIsInsideSafeZone", RepLayoutCmdType.PropertyBool)]
		public bool? bIsInsideSafeZone { get; set; } //Type:  Bits: 1

		[NetFieldExport("Zipline", RepLayoutCmdType.Property)]
		public NetworkGUID Zipline { get; set; } //Type:  Bits: 16

		[NetFieldExport("PetState", RepLayoutCmdType.Property)]
		public ActorGUID PetState { get; set; } //Type: AFortPlayerPetRepState* Bits: 8

		[NetFieldExport("bIsZiplining", RepLayoutCmdType.PropertyBool)]
		public bool? bIsZiplining { get; set; } //Type:  Bits: 1

		[NetFieldExport("bJumped", RepLayoutCmdType.PropertyBool)]
		public bool? bJumped { get; set; } //Type:  Bits: 1

		[NetFieldExport("ParachuteAttachment", RepLayoutCmdType.Property)]
		public ActorGUID ParachuteAttachment { get; set; } //Type: AFortPlayerParachute* Bits: 16

		[NetFieldExport("AuthoritativeValue", RepLayoutCmdType.PropertyUInt32)]
		public uint? AuthoritativeValue { get; set; } //Type:  Bits: 32

		[NetFieldExport("SocketOffset", RepLayoutCmdType.Property)]
		public DebuggingObject SocketOffset { get; set; } //Type:  Bits: 96

		[NetFieldExport("RemoteViewData32", RepLayoutCmdType.PropertyUInt32)]
		public uint? RemoteViewData32 { get; set; } //Type: uint32 Bits: 32

		[NetFieldExport("bNetMovementPrioritized", RepLayoutCmdType.PropertyBool)]
		public bool? bNetMovementPrioritized { get; set; } //Type: bool Bits: 1

		[NetFieldExport("EntryTime", RepLayoutCmdType.PropertyUInt32)]
		public uint? EntryTime { get; set; } //Type:  Bits: 32

		[NetFieldExport("CapsuleRadiusAthena", RepLayoutCmdType.PropertyFloat)]
		public float? CapsuleRadiusAthena { get; set; } //Type: float Bits: 32

		[NetFieldExport("CapsuleHalfHeightAthena", RepLayoutCmdType.PropertyFloat)]
		public float? CapsuleHalfHeightAthena { get; set; } //Type: float Bits: 32

		[NetFieldExport("WalkSpeed", RepLayoutCmdType.PropertyFloat)]
		public float? WalkSpeed { get; set; } //Type: float Bits: 32

		[NetFieldExport("RunSpeed", RepLayoutCmdType.PropertyFloat)]
		public float? RunSpeed { get; set; } //Type: float Bits: 32

		[NetFieldExport("SprintSpeed", RepLayoutCmdType.PropertyFloat)]
		public float? SprintSpeed { get; set; } //Type: float Bits: 32

		[NetFieldExport("CrouchedRunSpeed", RepLayoutCmdType.PropertyFloat)]
		public float? CrouchedRunSpeed { get; set; } //Type: float Bits: 32

		[NetFieldExport("CrouchedSprintSpeed", RepLayoutCmdType.PropertyFloat)]
		public float? CrouchedSprintSpeed { get; set; } //Type: float Bits: 32

		[NetFieldExport("AnimMontage", RepLayoutCmdType.Property)]
		public UObjectGUID AnimMontage { get; set; } //Type: UAnimMontage* Bits: 16

		[NetFieldExport("PlayRate", RepLayoutCmdType.PropertyFloat)]
		public float? PlayRate { get; set; } //Type: float Bits: 32

		[NetFieldExport("BlendTime", RepLayoutCmdType.PropertyFloat)]
		public float? BlendTime { get; set; } //Type: float Bits: 32

		[NetFieldExport("ForcePlayBit", RepLayoutCmdType.PropertyBool)]
		public bool? ForcePlayBit { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("IsStopped", RepLayoutCmdType.PropertyBool)]
		public bool? IsStopped { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("SkipPositionCorrection", RepLayoutCmdType.PropertyBool)]
		public bool? SkipPositionCorrection { get; set; } //Type: uint8 Bits: 1

		[NetFieldExport("RepAnimMontageStartSection", RepLayoutCmdType.PropertyInt)]
		public int? RepAnimMontageStartSection { get; set; } //Type: int32 Bits: 32

		[NetFieldExport("SimulatedProxyGameplayCues", RepLayoutCmdType.Property)]
		public FMinimalGameplayCueReplicationProxy SimulatedProxyGameplayCues { get; set; } //Type: FMinimalGameplayCueReplicationProxy Bits: 5

		[NetFieldExport("ItemWraps", RepLayoutCmdType.DynamicArray)]
		public NetworkGUID[] ItemWraps { get; set; } //Type:  Bits: 408

		[NetFieldExport("WeaponActivated", RepLayoutCmdType.PropertyBool)]
		public bool? WeaponActivated { get; set; } //Type: bool Bits: 1

		[NetFieldExport("bIsInWaterVolume", RepLayoutCmdType.PropertyBool)]
		public bool? bIsInWaterVolume { get; set; } //Type: bool Bits: 1

		[NetFieldExport("BannerIconId", RepLayoutCmdType.PropertyString)]
		public string BannerIconId { get; set; } //Type: FString Bits: 144

		[NetFieldExport("BannerColorId", RepLayoutCmdType.PropertyString)]
		public string BannerColorId { get; set; } //Type: FString Bits: 152

		[NetFieldExport("SkyDiveContrail", RepLayoutCmdType.Property)]
		public ItemDefinitionGUID SkyDiveContrail { get; set; } //Type: UAthenaSkyDiveContrailItemDefinition* Bits: 16

		[NetFieldExport("Glider", RepLayoutCmdType.Property)]
		public ItemDefinitionGUID Glider { get; set; } //Type: UAthenaGliderItemDefinition* Bits: 16

		[NetFieldExport("Pickaxe", RepLayoutCmdType.Property)]
		public ItemDefinitionGUID Pickaxe { get; set; } //Type: UAthenaPickaxeItemDefinition* Bits: 16

		[NetFieldExport("bIsDefaultCharacter", RepLayoutCmdType.PropertyBool)]
		public bool? bIsDefaultCharacter { get; set; } //Type: bool Bits: 1

		[NetFieldExport("Character", RepLayoutCmdType.Property)]
		public ItemDefinitionGUID Character { get; set; } //Type: UAthenaCharacterItemDefinition* Bits: 16

		[NetFieldExport("CharacterVariantChannels", RepLayoutCmdType.DynamicArray)]
		public ItemDefinitionGUID[] CharacterVariantChannels { get; set; } //Type: TArray Bits: 280

		[NetFieldExport("DBNOHoister", RepLayoutCmdType.Property)]
		public ActorGUID DBNOHoister { get; set; } //Type:  Bits: 16

		[NetFieldExport("DBNOCarryEvent", RepLayoutCmdType.Enum)]
		public int? DBNOCarryEvent { get; set; } //Type:  Bits: 2

		[NetFieldExport("Backpack", RepLayoutCmdType.Property)]
		public ItemDefinitionGUID Backpack { get; set; } //Type: UAthenaBackpackItemDefinition* Bits: 16

		[NetFieldExport("LoadingScreen", RepLayoutCmdType.Property)]
		public ItemDefinitionGUID LoadingScreen { get; set; } //Type: UAthenaLoadingScreenItemDefinition* Bits: 16

		[NetFieldExport("Dances", RepLayoutCmdType.DynamicArray)]
		public ItemDefinitionGUID[] Dances { get; set; } //Type: TArray Bits: 352

		[NetFieldExport("MusicPack", RepLayoutCmdType.Property)]
		public ItemDefinitionGUID MusicPack { get; set; } //Type: UAthenaMusicPackItemDefinition* Bits: 16

		[NetFieldExport("PetSkin", RepLayoutCmdType.Property)]
		public ItemDefinitionGUID PetSkin { get; set; } //Type: UAthenaPetItemDefinition* Bits: 16

		[NetFieldExport("EncryptedPawnReplayData", RepLayoutCmdType.Property)]
		public FAthenaPawnReplayData EncryptedPawnReplayData { get; set; } //Type: FAthenaPawnReplayData Bits: 160

		[NetFieldExport("GravityFloorAltitude", RepLayoutCmdType.PropertyUInt32)]
		public uint? GravityFloorAltitude { get; set; } //Type:  Bits: 32

		[NetFieldExport("GravityFloorWidth", RepLayoutCmdType.PropertyUInt32)]
		public uint? GravityFloorWidth { get; set; } //Type:  Bits: 32

		[NetFieldExport("GravityFloorGravityScalar", RepLayoutCmdType.PropertyUInt32)]
		public uint? GravityFloorGravityScalar { get; set; } //Type:  Bits: 32

		[NetFieldExport("ReplicatedWaterBody", RepLayoutCmdType.Property)]
		public DebuggingObject ReplicatedWaterBody { get; set; } //Type:  Bits: 16

		[NetFieldExport("DBNORevivalStacking", RepLayoutCmdType.PropertyByte)]
		public byte? DBNORevivalStacking { get; set; } //Type:  Bits: 8

		[NetFieldExport("ServerWorldTimeRevivalTime", RepLayoutCmdType.PropertyFloat)]
		public float? ServerWorldTimeRevivalTime { get; set; } //Type:  Bits: 32

		[NetFieldExport("ItemSpecialActorID", RepLayoutCmdType.Property)]
		public DebuggingObject ItemSpecialActorID { get; set; } //Type:  Bits: 225

		[NetFieldExport("FlySpeed", RepLayoutCmdType.PropertyFloat)]
		public float? FlySpeed { get; set; } //Type: float Bits: 32

		[NetFieldExport("NextSectionID", RepLayoutCmdType.Property)]
		public DebuggingObject NextSectionID { get; set; } //Type:  Bits: 8

		[NetFieldExport("FastReplicationMinimalReplicationTags", RepLayoutCmdType.Property)]
		public FMinimalGameplayCueReplicationProxy FastReplicationMinimalReplicationTags { get; set; } //Type:  Bits: 37

		[NetFieldExport("bIsCreativeGhostModeActivated", RepLayoutCmdType.PropertyBool)]
		public bool? bIsCreativeGhostModeActivated { get; set; } //Type:  Bits: 1

		[NetFieldExport("PlayRespawnFXOnSpawn", RepLayoutCmdType.PropertyBool)]
		public bool? PlayRespawnFXOnSpawn { get; set; } //Type:  Bits: 1

        [NetFieldExport("AuthoritativeRootMotion", RepLayoutCmdType.Ignore)]
        public FRootMotionSourceGroup AuthoritativeRootMotion { get; set; }

        [NetFieldExport("AnimRootMotionTranslationScale", RepLayoutCmdType.Property)]
        public DebuggingObject AnimRootMotionTranslationScale { get; set; }

        [NetFieldExport("bReplicatedIsInVortex", RepLayoutCmdType.PropertyBool)]
        public bool? bReplicatedIsInVortex { get; set; }

        [NetFieldExport("PitchAlpha", RepLayoutCmdType.PropertyFloat)]
        public float? PitchAlpha { get; set; }

        [NetFieldExport("StreamerCharacter", RepLayoutCmdType.Property)]
        public NetworkGUID StreamerCharacter { get; set; }
    }
}
