using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class CommandFileError
    {
        public CommandFileError(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
        public CommandFileGroupItem ErrorItem { get; set; }

        public string FileName { get; set; }
    }
}
