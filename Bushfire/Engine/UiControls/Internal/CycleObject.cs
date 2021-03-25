using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine.UIControls.Internal
{
    class CycleObject
    {
        public object value;
        public string displayName;

        public CycleObject(string displayName, object value)
        {
            this.displayName = displayName;
            this.value = value;
        }
    }
}
