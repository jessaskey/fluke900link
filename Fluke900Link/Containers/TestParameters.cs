using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class TestParameters
    {
        public string Message = String.Empty;
        public TestParameterDefinition Definition = null;
        public TestParameterInitialization Initialization = null;
        public TestParameterPerformanceEnvelope PerformanceEnvelope = null;
        public TestParameterStimulation Stimulation = null;
    }
}
