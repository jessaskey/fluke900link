using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class CTreeSchema
    {
        public List<CTreeColumn> Columns { get; set; } = new List<CTreeColumn>();
    }
}
