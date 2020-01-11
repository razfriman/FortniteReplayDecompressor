using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Custom
{
    [NetFieldExportGroup("CurrentPlaylistInfo", ParseType.Minimal)]
    public class CurrentPlaylistInfo : INetFieldExportGroup, IProperty
    {
        public NetworkGUID Id { get; private set; }

        public void Serialize(NetBitReader reader)
        {
            reader.ReadBits(2); //Unknown

            Id = new NetworkGUID
            {
                Value = reader.ReadIntPacked()
            };

            reader.ReadBits(31); //Unknown
        }
    }
}
