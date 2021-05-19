using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class TestPinDefinition
    {
        public int PinNumber { get; set; }
        public decimal Frequency { get; set; }
        public FrequencyUnitDefinition FrequencyUnit { get; set; }
        public decimal FrequencyTolerance { get; set; } 
        public GateDefinition Gate { get; set; }
        public FloatCheckDefinition FloatCheck { get; set; }
        public IgnoreDefinition IgnoreCompare { get; set; }

        public PinActivityDefinition PinActivity { get; set; }

    }
}
