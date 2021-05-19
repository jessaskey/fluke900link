using Fluke900.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class CommandFile
    {
        public ProjectFile ProjectFile = null;
        public List<CommandFileGroup> FileGroups = new List<CommandFileGroup>();
        public bool HasErrors = false;
    }
}
