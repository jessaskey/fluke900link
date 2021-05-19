using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public enum TrueFalseX
    {
        DontCare = -1,
        False,
        True
    }

    public class TriggerDefinition
    {
        public List<Tuple<TrueFalseX>> Pins { get; set; } = new List<Tuple<TrueFalseX>>();
        public Tuple<TrueFalseX> Ext { get; set; }
    }
}
