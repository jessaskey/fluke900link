using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fluke900;
using Fluke900Link.Containers;
using Fluke900.Containers;

namespace Fluke900Link.Controls
{
    public partial class ProjectLocationControl : DockContentEx
    {
        private ProjectLocation _projectLocation = null;

        public ProjectLocationControl()
        {
            InitializeComponent();
        }

        public bool OpenLocation(ProjectLocation location)
        {
            bool result = false;
            try
            {
                _projectLocation = location;
                pinDiagramControl.ProjectLocation = _projectLocation;
                this.Text = location.Name;
                this.ToolTipText = location.Name;
                textBoxICName.Text = location.DeviceName;
                comboBoxICSize.SelectedIndex = comboBoxICSize.FindStringExact(location.Name);
                comboBoxRDDrive.SelectedIndex = location.ReferenceDeviceDrive ? 0 : 1;

                ////set up diagram controls
                //PinDiagramControl pinActivityDiagram = new PinDiagramControl();
                //pinActivityDiagram.Values = location.GetPinValues(typeof(PinActivityDefinition));
                //pinActivityDiagram.ValueType = typeof(PinActivityDefinition);
                //pinActivityDiagram.Device = location.DeviceName;
                //pinActivityDiagram.EnumValues = new List<string>() { "F", "X", "H", "L", "A" };
                //groupBoxPinActivity.Controls.Add(pinActivityDiagram);
                //pinActivityDiagram.Dock = DockStyle.Fill;



                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private void checkBoxTrigger_CheckedChanged(object sender, EventArgs e)
        {
            _projectLocation.TriggerEnabled = checkBoxTrigger.Checked;
            pinDiagramControl.Invalidate();
        }

        private void checkBoxGate_CheckStateChanged(object sender, EventArgs e)
        {
            _projectLocation.GateEnabled = checkBoxGate.Checked;
            pinDiagramControl.Invalidate();
        }
    }
}
