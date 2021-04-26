using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteReplayReader.Models.NetFieldExports.Enums
{
	public enum EFortKickReason
	{
		NotKicked,
		GenericKick,
		WasBanned,
		EncryptionRequired,
		CrossPlayRestriction,
		ClientIdRestriction,
		EFortKickReason_MAX,
	};
}
