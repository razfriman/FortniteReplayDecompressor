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
        /// Parses events and initial game state.
        /// </summary>
        Minimal,
        /// <summary>
        /// Parses events and full game state.
        /// </summary>
        Normal,
        /// <summary>
        /// Parses everything currently handled.
        /// </summary>
        Full,
        /// <summary>
        /// Parses everything + contains debugging information
        /// </summary>
        Debug
    }
}
