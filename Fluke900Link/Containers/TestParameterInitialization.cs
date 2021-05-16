using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class TestParameterInitialization
    {
        public int Synchronization { get; set; }
        public TriggerDefinition Trigger { get; set; }
        public int ResetOffset { get; set; }
        public int RamShadow { get; set; }

    }
}
