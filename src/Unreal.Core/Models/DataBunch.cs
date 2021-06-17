﻿using System.Text;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Models
{
    /// <summary>
    /// see https://github.com/EpicGames/UnrealEngine/blob/70bc980c6361d9a7d23f6d23ffe322a2d6ef16fb/Engine/Source/Runtime/Engine/Public/Net/DataBunch.h#L112
    /// </summary>
    public class DataBunch
    {
        // TODO: make BunchArchive?
        public DataBunch()
        {

        }

        public DataBunch(DataBunch InBunch)
        {
            Archive = InBunch.Archive;
            //PacketId = InBunch.PacketId;
            ChIndex = InBunch.ChIndex;
            ChType = InBunch.ChType;
            ChName = InBunch.ChName;
            ChSequence = InBunch.ChSequence;
            bOpen = InBunch.bOpen;
            bClose = InBunch.bClose;
            bDormant = InBunch.bDormant;
            //bIsReplicationPaused = InBunch.bIsReplicationPaused;
            bReliable = InBunch.bReliable;
            bPartial = InBunch.bPartial;
            bPartialInitial = InBunch.bPartialInitial;
            bPartialFinal = InBunch.bPartialFinal;
            bHasPackageMapExports = InBunch.bHasPackageMapExports;
            bHasMustBeMappedGUIDs = InBunch.bHasMustBeMappedGUIDs;
            //bIgnoreRPCs = InBunch.bIgnoreRPCs;
            CloseReason = InBunch.CloseReason;
        }
        public NetBitReader Archive { get; set; }
        //public int PacketId { get; set; }
        //FInBunch* Next;
        //UNetConnection* Connection;
        public uint ChIndex { get; set; }
        // UE_DEPRECATED(4.22, "ChType deprecated in favor of ChName.")
        public ChannelType ChType { get; set; }
        // FName
        public string ChName { get; set; }
        public int ChSequence { get; set; }
        public bool bOpen { get; set; }
        public bool bClose { get; set; }
        // UE_DEPRECATED(4.22, "bDormant is deprecated in favor of CloseReason")
        public bool bDormant { get; set; }                 // Close, but go dormant
       // public bool bIsReplicationPaused { get; set; }     // Replication on this channel is being paused by the server
        public bool bReliable { get; set; }
        public bool bPartial { get; set; }                // Not a complete bunch
        public bool bPartialInitial { get; set; }           // The first bunch of a partial bunch
        public bool bPartialFinal { get; set; }      // The final bunch of a partial bunch
        public bool bHasPackageMapExports { get; set; }  // This bunch has networkGUID name/id pairs
        public bool bHasMustBeMappedGUIDs { get; set; }  // This bunch has guids that must be mapped before we can process this bunch
        //public bool bIgnoreRPCs { get; set; }
        public ChannelCloseReason CloseReason { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            //builder.AppendLine($"{nameof(PacketId)}: {PacketId}");
            builder.AppendLine($"{nameof(ChIndex)}: {ChIndex}");
            builder.AppendLine($"{nameof(bOpen)}: {bOpen}");
            builder.AppendLine($"{nameof(bClose)}: {bClose}");
            builder.AppendLine($"{nameof(bDormant)}: {bDormant}");
            //builder.AppendLine($"{nameof(bIsReplicationPaused)}: {bIsReplicationPaused}");
            builder.AppendLine($"{nameof(bReliable)}: {bReliable}");
            builder.AppendLine($"{nameof(bPartial)}: {bPartial}");
            builder.AppendLine($"{nameof(bPartialInitial)}: {bPartialInitial}");
            builder.AppendLine($"{nameof(bPartialFinal)}: {bPartialFinal}");
            builder.AppendLine($"{nameof(bHasPackageMapExports)}: {bHasPackageMapExports}");
            builder.AppendLine($"{nameof(bHasMustBeMappedGUIDs)}: {bHasMustBeMappedGUIDs}");
            //builder.AppendLine($"{nameof(bIgnoreRPCs)}: {bIgnoreRPCs}");
            builder.AppendLine($"{nameof(CloseReason)}: {CloseReason}");

            return builder.ToString();
        }
    }
}
