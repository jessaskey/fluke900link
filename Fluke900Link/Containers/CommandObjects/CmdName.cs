using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    /// <summary>
    /// The NAME Command defines the Names of the Device, multiple names are allowed
    /// 
    /// NAME {device name}
    /// 
    /// Examples:
    ///     NAME 7400
    ///     NAME 7404
    /// 
    /// Binary Format Notes:
    /// 
    ///     Command Enum: 0x0d
    ///     
    /// </summary>
    public class CmdName
    {
        public string Value { get; set; }

        public CmdName(byte[] dataBytes)
        {
            Value = Encoding.ASCII.GetString(dataBytes);
        }
    }
}
