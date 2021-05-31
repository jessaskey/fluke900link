using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class GateDefinition
    {
        public bool Polarity { get; set; }
        public UnitTime Delay { get; set; }
        public UnitTime? Duration { get; set; }
    }
}
