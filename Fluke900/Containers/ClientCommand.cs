using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;

namespace Fluke900.Containers
{
    public class ClientCommand : ClientCommandBase
    {

        public ClientCommand(ClientCommands commandCode) 
            : base(commandCode)
        {


        }

        public ClientCommand(string commandText)
        : base(commandText)
            {


            }


        public List<string> Parameters { get { return _parameters; } set { _parameters = value; } }
    }
}
