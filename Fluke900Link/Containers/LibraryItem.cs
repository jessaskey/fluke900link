using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class LibraryItem
    {
        public string DeviceName { get; set; }
        public string LibraryFilename { get; set; }
        public int Checksum { get; set; }
        public int Fmask { get; set; }
        public int Threshold { get; set; }
        public string TestTime { get; set; }
        public string SyncTime { get; set; }
        public List<PinDefinition> PinDefinition { get; set;}

        public SizePowerInfo SizePowerInfo { get; set; }


    }
}
