using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class TestSequenceLocation
    {
        private int _locationCounter = 0;
        private string _sequenceName = "";

        private ProjectLocation _location = null;

        public TestSequenceLocation () { }
        public TestSequenceLocation(string sequenceName, ProjectLocation location)
        {
            _location = location;
            _sequenceName = sequenceName;
        }

        public bool AddByName(string locationName)
        {
            bool success = false;


            return success;
        }

        public string LocationName
        {
            get
            {
                if (_location != null)
                {
                    return _location.Name;
                }
                return "";
            }
        }

        public string LocationDeviceName
        {
            get
            {
                if (_location != null)
                {
                    return _location.DeviceName;
                }
                return "";
            }
        }


    }
}
