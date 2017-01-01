using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class CommunicationStatus
    {
        public CommunicationStatus(CommunicationDirection direction, bool active)
        {
            Direction = direction;
            Active = active;
        }

        public CommunicationDirection Direction { get; set; }
        public bool Active { get; set; }

    }
}
