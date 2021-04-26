using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteReplayReader.Models.NetFieldExports.Enums
{
	public enum EFortMinigameState
	{
		PreGame,
		Setup,
		Transitioning,
		WaitingForCameras,
		Warmup,
		InProgress,
		PostGameTimeDilation,
		PostRoundEnd,
		PostGameEnd,
		PostGameAbandon,
		PostGameReset,
		EFortMinigameState_MAX,
	};
}
