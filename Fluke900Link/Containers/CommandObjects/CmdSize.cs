using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    /// <summary>
    /// The Size Command defines both the total number of pins and optionally the VCC and GND pins of the device.
    /// 
    /// SIZE{# of pins} [Vcc pin list GND pin list] 
    /// 
    /// Examples:
    ///     SIZE 8 VCC 8 GND 7,6
    ///     SIZE 14 VCC 1,14 GND 2,7
    /// 
    /// Binary Format Notes:
    /// 
    ///     Command Enum: 0x10
    ///     
    ///     The VCC and GND pins are encoded by single bytes for each entry. VCC data is masked with 0x80 and GND is not masked.
    ///     The above examples would be encoded as
    ///     
    ///         SIZE 8 VCC 8 GND 7,6        10 08 88 07 06
    ///         SIZE 14 VCC 1,14 GND 2,7    10 0E 81 8E 02 07
    /// 
    ///     If the VCC and GND pins are not specified, then by documentation, the VCC is set to the MAX Pin# and GND is set to Max Pin#/2
    /// </summary>
    public class CmdSize
    {
        public int Value { get; set; }
        public List<int> Vcc = new List<int>();
        public List<int> Gnd = new List<int>();

        public CmdSize(byte[] dataBytes)
        {
            Value = dataBytes[0];
            for (int i = 1; i < dataBytes.Length; i++)
            {
                if ((dataBytes[i] & 0x80) > 0)
                {
                    //VCC pin
                    Vcc.Add(dataBytes[i] & 0x1f);
                }
                else
                {
                    Gnd.Add(dataBytes[i] & 0x1f);
                }
            }

            if (Vcc.Count == 0 && Gnd.Count == 0)
            {
                Vcc.Add(Value);
                Gnd.Add(Value / 2);
            }
        }
    }
}
