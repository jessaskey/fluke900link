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
        public ProjectLocationControl()
        {
            InitializeComponent();

            //set up diagram controls
            PinDiagramControl pinActivityDiagram = new PinDiagramControl();
            pinActivityDiagram.PinCount = 20;
            pinActivityDiagram.ValueType = typeof(PinActivityDefinition);
            pinActivityDiagram.Device = "7432";
            pinActivityDiagram.Values = new List<string>() { "F", "X", "H", "L", "A" };
            groupBoxPinActivity.Controls.Add(pinActivityDiagram);
            pinActivityDiagram.Dock = DockStyle.Fill;

            PinDiagramControl floatCheckDiagram = new PinDiagramControl();
            floatCheckDiagram.PinCount = 20;
            floatCheckDiagram.ValueType = typeof(FloatCheckDefinition);
            floatCheckDiagram.Device = "7432";
            floatCheckDiagram.Values = new List<string>() { "X", "Z" };
            groupBoxPinActivity.Controls.Add(floatCheckDiagram);
            floatCheckDiagram.Dock = DockStyle.Fill;
        }

        public bool OpenLocation(ProjectLocation location)
        {
            bool result = false;
            try
            {
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
    }
}
