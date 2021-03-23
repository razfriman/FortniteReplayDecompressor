using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Contracts;

namespace Unreal.Core.Models
{
    /// <summary>
    /// https://github.com/EpicGames/UnrealEngine/blob/6c20d9831a968ad3cb156442bebb41a883e62152/Engine/Plugins/Runtime/GameplayAbilities/Source/GameplayAbilities/Private/GameplayEffectTypes.cpp#L311
    /// </summary>
    public class FGameplayEffectContextHandle : IProperty
    {
        public void Serialize(NetBitReader reader)
        {
            bool validData = reader.ReadBit();

            if(!validData)
            {
                return;
            }

            //???
            reader.Seek(reader.GetBitsLeft(), System.IO.SeekOrigin.Current);
        }
    }
}
