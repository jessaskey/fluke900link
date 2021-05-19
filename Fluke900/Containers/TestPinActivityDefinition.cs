using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
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
