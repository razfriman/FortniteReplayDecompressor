using System;
using System.Collections.Generic;
using System.Text;
using Unreal.Core.Models.Enums;

namespace Unreal.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public sealed class PartialNetFieldExportGroup : Attribute
    {
        public string PartialPath { get; private set; }

        public PartialNetFieldExportGroup(string partialPath)
        {
            PartialPath = partialPath;
        }
    }
}
