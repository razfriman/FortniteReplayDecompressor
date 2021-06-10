using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unreal.Core.Contracts
{
    public interface ISingleInstance
    {
        public void ClearInstance();
        public object Clone();
    }
}
