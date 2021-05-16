using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;

namespace Fluke900Link
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

        public string[] Parameters { get { return _parameters; } set { _parameters = value; } }
    }
}
