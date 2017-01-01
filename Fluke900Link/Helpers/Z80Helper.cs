using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Helpers
{
    public static class Z80Helper
    {
        /// <summary>
        /// Converts a series of LSB-First bytes into a single address. Array may be of any length;
        /// </summary>
        /// <param name="bytes">Byte Array of LSB-First bytes</param>
        /// <returns>The converted address</returns>
        public static int GetBigEndianNumber(byte[] bytes)
        {
            int address = 0;
            for (int i = bytes.Length - 1; i >= 0; i-- )
            {
                address += bytes[i] << (8 * i);
            }
            return address;

        }
    }
}
