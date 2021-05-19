using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;

namespace Fluke900.Containers
{
    public class RemoteCommand : RemoteCommandBase
    {

        public RemoteCommand(RemoteCommandCodes commandCode) 
            : base(commandCode)
        {


        }

        public RemoteCommand(string commandText)
        : base(commandText)
            {


            }

        public List<string> Parameters { get { return _parameters; } set { _parameters = value; } }
    }
}
