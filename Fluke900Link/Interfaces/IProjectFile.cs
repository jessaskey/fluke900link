using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fluke900Link.Containers;

namespace Fluke900Link
{
    public interface IProjectFile
    {
        string PathFileName { get; set; }
        string FileNameOnly { get; }
    }
}
