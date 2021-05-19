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

        public TestSequenceLocation () { }
        public TestSequenceLocation(ProjectLocation location)
        {
            _location = location;

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

        private ProjectLocation _location = null;
    }
}
