using System;

namespace Unreal.Core.Models.Enums
{
    /// <summary>
    /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Private/PackageMapClient.cpp#L573
    /// </summary>
    
    [Flags]
    public enum ExportFlags
    {
        None = 0,
        bHasPath = 1,
        bNoLoad = 2,
        bHasNetworkChecksum = 4,
    }
}
