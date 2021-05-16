using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class TestParameterDefinition
    {
        public string DeviceName { get; set; }
        public int Pins { get; set; }
        public List<int> VccPins { get; set; } = new List<int>();
        public List<int> GndPins { get; set; } = new List<int>();

        public int ReferenceDeviceDrive { get; set; }
        public TestPinActivityDefinition PinActivity { get; set; }
        public int Simulation { get; set; }
        public int ReferenceDeviceTest { get; set; }
        public int Checksum { get; set; }
        public int ClipCheck { get; set; }
        public FloatCheckDefinition FloatCheck { get; set; }


    }
}
