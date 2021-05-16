using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class TestParameterStimulation
    {
        public int Reset { get; set; }
        public int ResetPolarity { get; set; }
        public int ResetVcc { get; set; }
        public int ResetDuration { get; set; }
    }
}
