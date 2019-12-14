using System;
using System.Collections.Generic;
using System.Text;

namespace Unreal.Core.Models.Enums
{
    public enum ParseType
    {
        /// <summary>
        /// Parses only events.
        /// </summary>
        EventsOnly,
        /// <summary>
        /// Parses events and partial useful data. 
        /// </summary>
        Minimal,
        /// <summary>
        /// Parses events and all useful data.
        /// </summary>
        Normal
    }
}
