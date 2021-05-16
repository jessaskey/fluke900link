using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public enum PinActivityState
    {
        High,
        Low,
        Active,
        Frequency,
        DontCare
    }
    public class TestPinActivityDefinition
    {
        public List<TestPinActivityDefinition> ActivityDefinitions { get; set; } = new List<TestPinActivityDefinition>();
    }
}
