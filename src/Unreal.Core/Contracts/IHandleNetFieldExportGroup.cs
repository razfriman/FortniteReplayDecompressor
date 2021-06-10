using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Contracts
{
    //Throws unknown handles here instead of throwing warnings 
    public abstract class IHandleNetFieldExportGroup : INetFieldExportGroup
    {
        public abstract RepLayoutCmdType Type { get; protected set; }
        public Dictionary<uint, object> UnknownHandles = new Dictionary<uint, object>();
    }
}
