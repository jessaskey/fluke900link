using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class TestParameters
    {




        public string Message = String.Empty;

        #region Definition

        public string DeviceName { get; set; }
        public int Pins { get; set; }
        public List<int> VccPins { get; set; } = new List<int>();
        public List<int> GndPins { get; set; } = new List<int>();
        public List<TestPinDefinition> PinDefinitions { get; set; } = new List<TestPinDefinition>();

        public bool ReferenceDeviceDrive { get; set; }
        public TestPinActivityDefinition PinActivity { get; set; }
        public int Simulation { get; set; }
        public int ReferenceDeviceTest { get; set; }
        public int Checksum { get; set; }
        public int ClipCheck { get; set; }
        public FloatCheckDefinition FloatCheck { get; set; }


        #endregion

        #region Initialization

        public int Synchronization { get; set; }
        public TriggerDefinition Trigger { get; set; }
        public int ResetOffset { get; set; }
        public int RamShadow { get; set; }

        #endregion

        #region Performance Envelope

        public int FaultMask { get; set; }
        public int Threshold { get; set; }
        public int TestTime { get; set; }
        public List<TrueFalseX> Gate { get; set; } = new List<TrueFalseX>();
        public int Delay { get; set; }
        public int Duration { get; set; }
        public int Polarity { get; set; }

        #endregion

        #region Stimulation
        public int Reset { get; set; }
        public int ResetPolarity { get; set; }
        public int ResetVcc { get; set; }
        public int ResetDuration { get; set; }

        #endregion
    }
}
