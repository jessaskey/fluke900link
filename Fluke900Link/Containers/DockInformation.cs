using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telerik.WinControls.UI.Docking;

namespace Fluke900Link.Containers
{
    public class DockInformation
    {
        public DockPosition MainDockPosition { get; set; }
        public DockPosition SubDockPosition { get; set; }

        public DockInformation(DockPosition mainDockPosition, DockPosition subDockPosition)
        {
            MainDockPosition = mainDockPosition;
            SubDockPosition = subDockPosition;
        }
    }
}
