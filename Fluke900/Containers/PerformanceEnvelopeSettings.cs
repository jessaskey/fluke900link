using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class PerformanceEnvelopeSettings
    {
        public int FaultMask { get; set; }
        public int FaultMaskStep { get; set; }
        public int Threshold { get; set; }
        public int ThresholdStep { get; set; }
        public int FaultMaskTestCount { get; set; }
        public int ThresholdTestCount { get; set; }
    }
}
