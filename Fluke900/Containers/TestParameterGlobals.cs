using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class TestParameterGlobals
    {
        public int FaultMask { get; set; }
        public int Threshold { get; set; }
        public int TestTime { get; set; }

        public int Reset { get; set; }
        public int ResetPolarity { get; set; }
        public int ResetVcc { get; set; }
        public int ResetDuration { get; set; }
        public int ResetOffset { get; set; }
        public int ExtTrigger { get; set; }
        public int ExtGate { get; set; }
        public int ExtGateDelay { get; set; }
        public int ExtGateDuration { get; set; }
        public int ExtGatePolarity { get; set; }

    }
}
