using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Containers;
using Telerik.WinControls.UI.Docking;

namespace Fluke900Link.Controls
{
    public partial class LocationsEditor : DocumentEditor
    {
        ProjectLocationFile _locations = null;

        public LocationsEditor()
        {
            InitializeComponent();
        }

        public bool LoadLocations(ProjectLocationFile locations)
        {
            _locations = locations;
            this.Text = _locations.FileNameOnly;
            this.ToolTipText = _locations.PathFileName;
            this.Name = _locations.PathFileName;

            this.OpenDocumentForEditing(_locations.PathFileName);

            return true;
        }
    }
}
