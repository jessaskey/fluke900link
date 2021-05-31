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
        //public TestPinActivityDefinition PinActivity { get; set; }
        public bool ReferenceDeviceTest { get; set; }
        public int Checksum { get; set; }
        public SimulationShadowDefinition Simulation { get; set; }
        public SimulationShadowDefinition RAMShadow { get; set; }
        public TriggerExt1Definition TriggerExt1 { get; set; }
        public TriggerExt2Definition TriggerExt2 { get; set; }
        public GateExtDefinition GateExt { get; set; }
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

        public List<int> GetPinValues(Type enumType)
        {
            switch (enumType.Name.ToString())
            {
                case "PinActivityDefinition":
                    return PinDefinitions.Select(d => (int)d.PinActivity).ToList();
                case "FloatCheckDefinition":
                    return PinDefinitions.Select(d => (int)d.FloatCheck).ToList();
                case "TriggerWord1Definition":
                    return PinDefinitions.Select(d => (int)d.TriggerWord1).ToList();
                case "TriggerWord2Definition":
                    return PinDefinitions.Select(d => (int)d.TriggerWord2).ToList();
                case "GatePinDefinition":
                    return PinDefinitions.Select(d => (int)d.GatePinDefinition).ToList();
                //case "GateIgnoreCompareDefinition":
                //    return PinDefinitions.Select(d => (int)d.GateIgnoreCompareDefinition).ToList();
            }
            return null;
        }

        public bool SetPinValue(Type enumType, int pin, int enumValue)
        {
            switch (enumType.Name.ToString())
            {
                case "PinActivityDefinition":
                    PinActivityDefinition currentPad = PinDefinitions[pin].PinActivity;
                    if (currentPad != (PinActivityDefinition)enumValue)
                    {
                        PinDefinitions[pin].PinActivity = (PinActivityDefinition)enumValue;
                        return true;
                    }
                    break;
                case "FloatCheckDefinition":
                    FloatCheckDefinition currentFcd = PinDefinitions[pin].FloatCheck;
                    if (currentFcd != (FloatCheckDefinition)enumValue)
                    {
                        PinDefinitions[pin].FloatCheck = (FloatCheckDefinition)enumValue;
                        return true;
                    }
                    break;
                case "TriggerWord1Definition":
                    TriggerWord1Definition currentTwd1 = PinDefinitions[pin].TriggerWord1;
                    if (currentTwd1 != (TriggerWord1Definition)enumValue)
                    {
                        PinDefinitions[pin].TriggerWord1 = (TriggerWord1Definition)enumValue;
                        return true;
                    }
                    break;
                case "TriggerWord2Definition":
                    TriggerWord2Definition currentTwd2 = PinDefinitions[pin].TriggerWord2;
                    if (currentTwd2 != (TriggerWord2Definition)enumValue)
                    {
                        PinDefinitions[pin].TriggerWord2 = (TriggerWord2Definition)enumValue;
                        return true;
                    }
                    break;
                case "GatePinDefinition":
                    GatePinDefinition currentGpd = PinDefinitions[pin].GatePinDefinition;
                    if (currentGpd != (GatePinDefinition)enumValue)
                    {
                        PinDefinitions[pin].GatePinDefinition = (GatePinDefinition)enumValue;
                        return true;
                    }
                    break;
            }
            return false;
        }

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
                        pinDef.GatePinDefinition = GatePinDefinition.True;
                        break;
                    case "0":
                        pinDef.GatePinDefinition = GatePinDefinition.False;
                        break;
                    case "X":
                        pinDef.GatePinDefinition = GatePinDefinition.DontCare;
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
                        pinDef.TriggerWord1 = TriggerWord1Definition.DontCare;
                        break;
                    case "1":
                        pinDef.TriggerWord1 = TriggerWord1Definition.True;
                        break;
                    default:
                        pinDef.TriggerWord1 = TriggerWord1Definition.False;
                        break;
                }
                // SW2 Trigger Word 2
                string triggerWord2Value = Encoding.ASCII.GetString(pinDefinitionBytes[5]);
                switch (triggerWord2Value)
                {
                    case "X":
                        pinDef.TriggerWord2 = TriggerWord2Definition.DontCare;
                        break;
                    case "1":
                        pinDef.TriggerWord2 = TriggerWord2Definition.True;
                        break;
                    default:
                        pinDef.TriggerWord2 = TriggerWord2Definition.False;
                        break;
                }
                PinDefinitions.Add(pinDef);
            }
        }
    }
}
