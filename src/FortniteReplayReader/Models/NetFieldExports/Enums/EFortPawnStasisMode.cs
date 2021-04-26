using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteReplayReader.Models.NetFieldExports.Enums
{
	public enum EFortPawnStasisMode
	{
		None,
		NoMovement,
		NoMovementOrTurning,
		NoMovementOrFalling,
		NoMovement_EmotesEnabled,
		NoMovementOrTurning_EmotesEnabled,
		NoMovementOrFalling_EmotesEnabled,
		EFortPawnStasisMode_MAX,
	};
}
