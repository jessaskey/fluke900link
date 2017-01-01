using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class BinaryObject
    {
        private byte[] _bytes = null;
        private int _type = 0;
        private int _identifier = 0;

        public int DataType { get { return _type; } }
        public byte[] Bytes { get { return _bytes; } }
        public int Indentifier { get { return _identifier; } }

        public BinaryObject(byte[] bytes)
        {
            //bytes must be defined and min of length 3
            if (bytes == null || bytes.Length < 0)
            {
                throw new Exception("Binary array is either null or too short.");
            }

            //first byte should equal the length, otherwise we have a problem
            if (bytes[0] != bytes.Length)
            {
                throw new Exception("Binary array is incorrect length.");
            }

            if (bytes[1] != 0x01)
            {
                throw new Exception ("Type is not Binary.");
            }

            _type = bytes[1];
            _identifier = bytes[2];
            _bytes = bytes.Skip(3).Take(bytes.Length - 3).ToArray();
        }

        public bool Append(BinaryObject nextObj)
        {
            if (_type != nextObj.DataType)
            {
                throw new Exception("Error trying to append two binary objects with different type specifications.");
            }
            _bytes = _bytes.Concat(nextObj.Bytes).ToArray();
            return true;
        }
    }
}
