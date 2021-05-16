using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public enum FloatCheckState
    {
        HighZ,
        DontCare
    }
    public class FloatCheckDefinition
    {
        public List<FloatCheckState> FloatChecks { get; set; } = new List<FloatCheckState>();
    }
}
