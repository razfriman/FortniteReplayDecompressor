using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class Aircraft
    {
        public FVector? FlightStartLocation { get; internal set; }
        public FRotator FlightRotation { get; internal set; }
    }
}
