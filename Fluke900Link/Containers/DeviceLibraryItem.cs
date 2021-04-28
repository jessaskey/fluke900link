using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class DeviceLibraryItem
    {
        public DeviceLibraryConfigurationItem TypeDefinition { get; set; }

        //these are only for Simulation data
        //public List<BinaryObject> BinaryObjects = new List<BinaryObject>();
        public List<List<byte[]>> RDTestData = new List<List<byte[]>>();
        public List<byte> SimulationData = new List<byte>();
        public List<byte> ShadowData = new List<byte>();
        //public List<byte[]> VectorData = new List<byte[]>();

        public byte VectorType { get; set; }

        private string _dataString = null;
        private byte[] _dataBytes = null;

        public string Data
        {
            get
            {
                return _dataString;
            }
            set
            {
                _dataString = value;
                _dataBytes = Encoding.ASCII.GetBytes(_dataString);
            }
        }

        public byte[] DataBytes
        {
            get
            {
                return _dataBytes;
            }
            set
            {
                _dataBytes = value;
                _dataString = Encoding.ASCII.GetString(_dataBytes);
            }
        }

        public override string ToString()
        {
            if (Enum.IsDefined(typeof(DeviceLibraryConfigurationItem), TypeDefinition))
            {
                return Enum.GetName(typeof(DeviceLibraryConfigurationItem), TypeDefinition);
            }
            return "Unknown_" + TypeDefinition.ToString("2X");
        }

    }
}
