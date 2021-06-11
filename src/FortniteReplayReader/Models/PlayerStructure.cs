using FortniteReplayReader.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class PlayerStructure
    {
        public FVector? Location { get; internal set; }
        public FRotator Rotation { get; internal set; }
        public Team Team { get; internal set; }
        public MaterialType MaterialType { get; internal set; }
        public BaseStructureType BaseStructureType { get; internal set; }
        public short CurrentHealth { get; internal set; }
        public short MaxHealth { get; internal set; }

        //Edit type not added yet
    }
}
