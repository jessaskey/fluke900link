using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class CommandBinaryObject
    {
        public byte[] Bytes { get; set; }
        public DeviceLibraryConfigurationItem Item { get; set; }
        public int Offset { get; set; }

        public CommandBinaryObject(DeviceLibraryConfigurationItem item, int offset, byte[] bytes)
        {
            Item = item;
            Offset = offset;
            Bytes = bytes;
        }
    }
}
