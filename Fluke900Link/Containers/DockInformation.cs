using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeifenLuo.WinFormsUI.Docking;

namespace Fluke900Link.Containers
{
    public class DockInformation
    {
        public DockState MainDockPosition { get; set; }
        public DockAlignment DockAlignment { get; set; }

        public DockInformation(DockState dockPosition, DockAlignment dockAlignment)
        {
            MainDockPosition = dockPosition;
            DockAlignment = dockAlignment;
        }
    }
}
