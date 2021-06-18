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
using Fluke900.Containers;
using Fluke900Link.Containers;
using Fluke900Link.Controllers;
using Fluke900Link.Helpers;
using Fluke900Link.Dialogs;

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
                comboBoxICSize.SelectedIndex = comboBoxICSize.FindStringExact(location.Pins.ToString());
                comboBoxRDDrive.SelectedIndex = location.ReferenceDeviceDrive ? 0 : 1;
                textBoxCheckSum.Enabled = location.Checksum >= 0;
                textBoxCheckSum.Text = "";
                if (location.Checksum > -1)
                {
                    textBoxCheckSum.Text = location.Checksum.ToString();
                }
                checkBoxSimulation.Checked = location.Simulation == SimulationShadowDefinition.Enabled;
                checkBoxSimulation.Enabled = location.Simulation != SimulationShadowDefinition.NotInstalled;
                checkBoxClipCheck.Checked = location.ClipCheck;
                checkBoxSynchronization.Checked = location.SyncTime.HasValue;
                numericUpDownSynchronizationTime.Visible = checkBoxSynchronization.Checked;
                labelSynchronizationUnit.Visible = checkBoxSynchronization.Checked;
                if (location.SyncTime.HasValue)
                {
                    numericUpDownSynchronizationTime.Value = location.SyncTime.Value;
                }
                checkBoxResetOffset.Checked = location.Reset.NegativeOffset >= 0;
                numericUpDownResetOffset.Value = location.Reset.NegativeOffset;
                numericUpDownResetOffset.Visible = checkBoxResetOffset.Enabled;
                labelResetOffsetUnit.Visible = checkBoxResetOffset.Enabled;
                checkBoxTrigger.Checked = location.TriggerEnabled;
                switch (location.RAMShadow)
                {
                    case SimulationShadowDefinition.Enabled:
                        labelRAMShadow.Text = "On";
                        break;
                    case SimulationShadowDefinition.Disabled:
                        labelRAMShadow.Text = "Off";
                        break;
                    case SimulationShadowDefinition.NotInstalled:
                        labelRAMShadow.Text = "N/A";
                        break;
                }
                numericUpDownFaultMask.Value = location.FaultMask;
                numericUpDownThreshold.Value = location.Threshold;
                checkBoxGate.Checked = location.Gate.Duration != null;
                checkBoxTestTimeContinuous.Checked = location.TestTime < 0;
                numericUpDownTestTime.Visible = !checkBoxTestTimeContinuous.Checked;
                labelTestTimeUnit.Visible = !checkBoxTestTimeContinuous.Checked;
                if (location.TestTime >= 0)
                {
                    numericUpDownTestTime.Value = location.TestTime;
                }
                radioButtonUUTReset.Checked = location.Reset == null;
                radioButtonF900Reset.Checked = location.Reset != null;



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

        private void checkBoxSynchronization_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownSynchronizationTime.Visible = checkBoxSynchronization.Checked;
            labelSynchronizationUnit.Visible = checkBoxSynchronization.Checked;
        }

        private void checkBoxResetOffset_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownResetOffset.Visible = checkBoxResetOffset.Enabled;
            labelResetOffsetUnit.Visible = checkBoxResetOffset.Enabled;
        }

        private void checkBoxTestTimeContinuous_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownTestTime.Visible = !checkBoxTestTimeContinuous.Checked;
            labelTestTimeUnit.Visible = !checkBoxTestTimeContinuous.Checked;
        }

        private void buttonLearn_Click(object sender, EventArgs e)
        {
            if (FlukeController.IsConnected)
            {
                PerformanceEnvelopeSettings pes = null;
                Task.Run(async () => { pes = await FlukeController.GetPerformanceEnvelopeSettings(); }).Wait();
            
                LearnPEDialog ped = new LearnPEDialog();
                ped.Learn(pes);
                DialogResult dr = ped.ShowDialog();
                if (dr == DialogResult.OK)
                {

                }
            }
            else
            {
                MessageBox.Show("Fluke is not connected. Please connect first in order to learn the DUT.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
