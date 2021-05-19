using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class RemoteCommandSet
    {
        public RemoteCommand Command { get; set; }
        public RemoteCommandResponse Response { get; set; }

        public RemoteCommandSet()
        {

        }

        public RemoteCommandSet(RemoteCommand command)
        {
            Command = command;
        }
    }
}
