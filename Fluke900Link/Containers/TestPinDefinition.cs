using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class TestPinDefinition
    {
        public int PinNumber { get; set; }
        public string Definition { get; set; }
        public bool IgnoreCompare { get; set; }
    }
}
