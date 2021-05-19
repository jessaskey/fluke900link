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
        public int Pins { get; set; }

        public List<TestPinDefinition> PinDefinitions { get; set; } = new List<TestPinDefinition>();


        public void LoadPinDefinitions(List<byte[]> pinDefinitionBytes)
        {
            for (int j = 0; j < Pins; j++)
            {
                TestPinDefinition pinDef = new TestPinDefinition();
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
                PinDefinitions.Add(pinDef);
            }
        }
    }
}
