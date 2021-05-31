using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fluke900;
using Fluke900Link.Containers;

namespace Fluke900Link
{
    public delegate void LogMessageHandler(string msg);
    public delegate void LogIssueHandler(ProjectIssue pi);


    public delegate void SerialDataStatusChanged(bool sending, bool receiving);
    public delegate void ConnectionStatusChanged(EventArgs e, ConnectionStatus previousStatus, ConnectionStatus currentStatus);

}
