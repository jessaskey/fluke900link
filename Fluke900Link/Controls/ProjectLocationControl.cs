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
using Fluke900Link.Helpers;

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
                pinDiagramControl.SetProjectLocation(_projectLocation);
                this.Text = location.Name;
                this.ToolTipText = location.Name;
                textBoxICName.Text = location.DeviceName;
                comboBoxICSize.SelectedIndex = comboBoxICSize.FindStringExact(location.Name);
                comboBoxRDDrive.SelectedIndex = location.ReferenceDeviceDrive ? 0 : 1;

                


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
            checkBoxGate.Enabled = !_projectLocation.TriggerEnabled;
            pinDiagramControl.Refresh();
        }

        private void checkBoxGate_CheckStateChanged(object sender, EventArgs e)
        {
            _projectLocation.GateEnabled = checkBoxGate.Checked;
            checkBoxTrigger.Enabled = !_projectLocation.GateEnabled;
            pinDiagramControl.Refresh();
        }

        private void textBoxICName_Leave(object sender, EventArgs e)
        {
            if (LibraryHelper.HasDevice(textBoxICName.Text))
            {
                textBoxICName.ForeColor = Color.Blue;
            }
            else
            {
                textBoxICName.ForeColor = Color.Red;
            }
        }
    }
}
