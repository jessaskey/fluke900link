using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fluke900Link.Helpers;

namespace Fluke900Link.Containers
{
    public class LibraryListItem
    {
        public string Name { get; set;  }
        public int StartOffset { get; set; }
        //public int Length { get; set; }

        public LibraryListItem(byte[] inputBytes)
        {
            Name = ASCIIEncoding.ASCII.GetString(inputBytes.Take(0x0e).ToArray()).Trim();
            StartOffset = Z80Helper.GetBigEndianNumber(inputBytes.Skip(0x0f).Take(3).ToArray());
            //Length = Z80Helper.GetBigEndianNumber(inputBytes.Skip(0x17).Take(2).ToArray());
        }

    }
}
