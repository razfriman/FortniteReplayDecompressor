using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class PartialNetFieldExportGroup : NetFieldExportGroupAttribute
    {
        public string PartialPath { get; private set; }

        public PartialNetFieldExportGroup(string partialPath, string redirectPath, ParseType minParseType = ParseType.Minimal) : base(redirectPath, minParseType)
        {
            PartialPath = partialPath;
        }
    }
}
