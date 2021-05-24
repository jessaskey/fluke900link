using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fluke900.Containers;

namespace Fluke900.Containers
{
    public class ProjectLocation
    {
        public string Name { get; set; }

        public string DeviceName { get; set; }
        public int Pins { get; set; }
        public List<int> VccPins { get; set; } = new List<int>();
        public List<int> GndPins { get; set; } = new List<int>();

        public List<TestPinDefinition> PinDefinitions { get; set; } = new List<TestPinDefinition>();

        public bool ReferenceDeviceDrive { get; set; }
        public TestPinActivityDefinition PinActivity { get; set; }
        public bool ReferenceDeviceTest { get; set; }
        public int Checksum { get; set; }
        public SimulationShadowDefinition Simulation { get; set; }
        public SimulationShadowDefinition RAMShadow { get; set; }

        public TriggerGateDefinition TriggerExt1 { get; set; }
        public TriggerGateDefinition TriggerExt2 { get; set; }
        public TriggerGateDefinition GateExt { get; set; }
        public bool GateEnabled { get; set; }

        public GateDefinition Gate { get; set; }

        #region Initialization

        public bool ClipCheck { get; set; }
        public int? SyncTime { get; set; }
        public string TriggerConfiguration { get; set; }
        public bool TriggerEnabled { get; set; }

        #endregion

        #region Performance Envelope

        public int FaultMask { get; set; }
        public int Threshold { get; set; }
        public string TestTime { get; set; }
        public int Delay { get; set; }
        public int Duration { get; set; }
        public int Polarity { get; set; }

        #endregion

        #region Stimulation
        public ResetDefinition Reset { get; set; }

        #endregion


        public void LoadPinDefinitions(List<byte[]> pinDefinitionBytes)
        {
            for (int j = 0; j < Pins; j++)
            {
                TestPinDefinition pinDef = new TestPinDefinition();
                //activity nybble first
                pinDef.PinActivity = (PinActivityDefinition)(pinDefinitionBytes[0][0] & 0x0F);
                //frequency
                Match frequencyMatch = Regex.Match(Encoding.ASCII.GetString(pinDefinitionBytes[1]), @"(?<freq>[0-9]\d*(\.\d+)?)(?<unit>[MK]?Hz)(?<tolerance>[0-9]\d*(\.\d+)?)%");
                pinDef.Frequency = decimal.Parse(frequencyMatch.Groups["freq"].Value);
                switch (frequencyMatch.Groups["unit"].Value.ToLower())
                {
                    case "mhz":
                        pinDef.FrequencyUnit = FrequencyUnitDefinition.MHz;
                        break;
                    case "khz":
                        pinDef.FrequencyUnit = FrequencyUnitDefinition.kHz;
                        break;
                    default:
                        pinDef.FrequencyUnit = FrequencyUnitDefinition.Hz;
                        break;
                }
                pinDef.FrequencyTolerance = decimal.Parse(frequencyMatch.Groups["tolerance"].Value);
                // X   Float Check Defintion
                //pinDef.FloatCheck = Encoding.ASCII.GetString(pinDefinitionBytes[2]) == "Z" ? FloatCheckDefinition.Inactive : FloatCheckDefinition.NotChecked;
                // X   Gate Definition
                string gateValue = Encoding.ASCII.GetString(pinDefinitionBytes[2]);
                switch (gateValue)
                {
                    case "1":
                        pinDef.Gate = TriggerGateDefinition.True;
                        break;
                    case "0":
                        pinDef.Gate = TriggerGateDefinition.False;
                        break;
                    case "X":
                        pinDef.Gate = TriggerGateDefinition.DontCare;
                        break;
                }
                // P   Pin Ignore Flag (P?/I/C?)
                if (Encoding.ASCII.GetString(pinDefinitionBytes[3]) == "C")
                {
                    pinDef.IgnoreCompare = IgnoreDefinition.Compare;
                }
                else
                {
                    pinDef.IgnoreCompare = IgnoreDefinition.Ignore;
                }
                // SW1 Trigger Word 1
                string triggerWord1Value = Encoding.ASCII.GetString(pinDefinitionBytes[4]);
                switch (triggerWord1Value)
                {
                    case "X":
                        pinDef.TriggerWord1 = TriggerGateDefinition.DontCare;
                        break;
                    case "1":
                        pinDef.TriggerWord1 = TriggerGateDefinition.True;
                        break;
                    default:
                        pinDef.TriggerWord1 = TriggerGateDefinition.False;
                        break;
                }
                // SW2 Trigger Word 2
                string triggerWord2Value = Encoding.ASCII.GetString(pinDefinitionBytes[5]);
                switch (triggerWord2Value)
                {
                    case "X":
                        pinDef.TriggerWord2 = TriggerGateDefinition.DontCare;
                        break;
                    case "1":
                        pinDef.TriggerWord2 = TriggerGateDefinition.True;
                        break;
                    default:
                        pinDef.TriggerWord2 = TriggerGateDefinition.False;
                        break;
                }
                PinDefinitions.Add(pinDef);
            }
        }
    }
}
