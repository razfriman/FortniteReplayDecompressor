using Unreal.Core.Attributes;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Contracts
{
    public abstract class INetFieldExportGroup
    {
        public Actor ChannelActor { get; internal protected set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
