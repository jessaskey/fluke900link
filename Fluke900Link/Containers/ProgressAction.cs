using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class ProgressAction
    {
        public string Title { get; set; }
        public string TopMessage { get; set; }
        public string BottomMessage { get; set; }
        public bool StartProgress { get; set; }
        public string CompletionText { get; set; }
    }
}
