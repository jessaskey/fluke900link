﻿using System;
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

        public ProjectLocation Location { get; set; }

        public TestSequenceLocation () { }
        public TestSequenceLocation(string sequenceName, ProjectLocation location)
        {
            Location = location;
            _sequenceName = sequenceName;
        }

        public bool AddByName(string locationName)
        {
            bool success = false;


            return success;
        }


    }
}
