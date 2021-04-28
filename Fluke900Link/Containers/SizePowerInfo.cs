using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class SizePowerInfo
    {
        private List<int> _vccPins = new List<int>();
        private List<int> _gndPins = new List<int>();

        public List<int> VccPins { get { return _vccPins; } }
        public List<int> GndPins { get { return _gndPins; } }
        public int Size { get; set; }
    }
}
