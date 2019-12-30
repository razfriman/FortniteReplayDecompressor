using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unreal.Core.Attributes;
using Unreal.Core.Contracts;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Models
{
    [NetFieldExportGroup("DebuggingExportGroup")]
    public class DebuggingExportGroup : INetFieldExportGroup
    {
        public NetFieldExportGroup ExportGroup { get; set; }
        public Dictionary<uint, string> HandleNames => ExportGroup?.NetFieldExports.Where(x => x != null).ToDictionary(x => x.Handle, x=> x.Name);


        [NetFieldExport("Handles", RepLayoutCmdType.Debug)]
        public Dictionary<uint, DebuggingObject> Handles { get; set; } = new Dictionary<uint, DebuggingObject>();

        public override string ToString()
        {
            return ExportGroup?.PathName;
        }
    }
}
