using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public enum IgnoreDefinition
    {
        Ignore,
        Compare
    }

    public class TestParameterPerformanceEnvelope
    {
        public int FaultMask { get; set; }
        public int Threshold { get; set; }
        public int TestTime { get; set; }
        public List<IgnoreDefinition> PinsIgnored { get; set; } = new List<IgnoreDefinition>();
        public List<TrueFalseX> Gate { get; set; } = new List<TrueFalseX>();
        public int Delay { get; set; }
        public int Duration { get; set; }
        public int Polarity { get; set; }
    }
}
