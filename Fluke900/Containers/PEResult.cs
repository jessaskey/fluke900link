using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class PEResult
    {
        public int FaultMask { get; set; }
        public int Threshold { get; set; }
        public bool PassFail { get; set; }
    }
}
