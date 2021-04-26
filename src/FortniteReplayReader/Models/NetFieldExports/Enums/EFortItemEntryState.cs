using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteReplayReader.Models.NetFieldExports.Enums
{
	public enum EFortItemEntryState
	{
		NoneState,
		NewItemCount,
		ShouldShowItemToast,
		DurabilityInitialized,
		DoNotShowSpawnParticles,
		FromRecoveredBackpack,
		FromGift,
		PendingUpgradeCriteriaProgress,
		OwnerBuildingHandle,
		FromDroppedPickup,
		JustCrafted,
		CraftAndSlotTarget,
		GenericAttributeValueSet,
		PickupInstigatorHandle,
		RechargingWeaponServerTime,
		DisallowSwapOnNextPickUpAttempt,
		DroppedByAI,
		EFortItemEntryState_MAX,
	};
}
