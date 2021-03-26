using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteReplayReader.Models
{
    public enum ParsingGroup 
    { 
        /// <summary>
        /// Changes all netfieldexports that deal with locations.
        /// Also changes: Vehicles, Inventory, Shots, Weapon switches
        /// </summary>
        PlayerPawn 
    }
}
