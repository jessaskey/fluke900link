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
        public FloatCheckDefinition FloatCheck { get; set; }
        public IgnoreDefinition IgnoreCompare { get; set; }

        public PinActivityDefinition PinActivity { get; set; }

        public TriggerWord1Definition TriggerWord1 { get; set; }
        public TriggerWord2Definition TriggerWord2 { get; set; }

        public GatePinDefinition GatePinDefinition { get; set; }
        //public GateIgnoreCompareDefinition GateIgnoreCompareDefinition { get; set; }




    }
}
