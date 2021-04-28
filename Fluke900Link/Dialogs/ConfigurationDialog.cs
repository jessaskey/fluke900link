using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluke900Link
{
    public partial class ConfigurationDialog : Form
    {

        private string _comPort = "";
        private int _comBaudRate = (int)BaudRates.Rate9600;
        private Parity _comParity = Parity.Even;
        private int _comDataBits = 7;
        private StopBits _comStopBits = StopBits.One;

        public ConfigurationDialog()
        {
            InitializeComponent();

            _comPort = Properties.Settings.Default.COM_Port;
            _comBaudRate = Properties.Settings.Default.COM_Baud;
            _comParity =  (Parity)Enum.Parse(typeof(Parity), Properties.Settings.Default.COM_Parity);
            _comDataBits = Properties.Settings.Default.COM_DataBits;
            _comStopBits = (StopBits)Enum.Parse(typeof(StopBits), Properties.Settings.Default.COM_StopBits);

            checkBoxAutoConnect.Checked = Properties.Settings.Default.AutoConnect;
            checkBoxSyncDateTime.Checked = Properties.Settings.Default.AutoSyncDateTime;
            textBoxFilesDirectory.Text = Properties.Settings.Default.DefaultFilesDirectory;
            textBoxLibloadDirectory.Text = Properties.Settings.Default.DefaultLibloadDirectory;
            checkBoxRememberDocuments.Checked = Properties.Settings.Default.SaveEditorWindows;
            checkBoxRememberToolWindows.Checked = Properties.Settings.Default.SaveToolboxWindows;

            if (String.IsNullOrEmpty(textBoxFilesDirectory.Text))
            {
                //no value... make a default
                textBoxFilesDirectory.Text = Utilities.GetDefaultDirectoryPath();
            }

            //populate dropdowns
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0)
            {
                MessageBox.Show("There were no communication ports found on this computer. Check device manager and drivers.", "No Ports Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            comboBoxPorts.Items.AddRange(ports);
            comboBoxBaudRate.Items.AddRange(Enum.GetValues(typeof(BaudRates)).Cast<int>().Select(v=>v.ToString()).ToArray());
            comboBoxParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
            comboBoxDataBits.Items.AddRange(Enum.GetValues(typeof(DataBits)).Cast<int>().Select(v => v.ToString()).ToArray());
            comboBoxStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));

            //assign combo boxes
            if (Properties.Settings.Default.COM_CableType == (int)SerialCableType.StraightThrough)
            {
                comboBoxSerialCableType.SelectedIndex = 0;
            }
            else
            {
                comboBoxSerialCableType.SelectedIndex = 1;
            }


            if (!String.IsNullOrEmpty(_comPort))
            {
                int comPortIndex = comboBoxPorts.FindString(_comPort);
                if (comPortIndex > -1)
                {
                    comboBoxPorts.SelectedIndex = comPortIndex;
                }
                else
                {
                    MessageBox.Show("Configured COM Port '" + _comPort + "' was not found in the valid ports. Please assign the correct port and re-save the configuration.", "COM Port Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            int baudRateIndex = comboBoxBaudRate.FindString(((int)_comBaudRate).ToString());
            if (baudRateIndex > -1)
            {
                comboBoxBaudRate.SelectedIndex = baudRateIndex;
            }

            int parityIndex = comboBoxParity.FindString((Enum.GetName(typeof(Parity),_comParity)));
            if (parityIndex > -1)
            {
                comboBoxParity.SelectedIndex = parityIndex;
            }

            int dataBitsIndex = comboBoxDataBits.FindString(((int)_comDataBits).ToString());
            if (dataBitsIndex > -1)
            {
                comboBoxDataBits.SelectedIndex = dataBitsIndex;
            }

            int stopBitsIndex = comboBoxStopBits.FindString((Enum.GetName(typeof(StopBits), _comStopBits)));
            if (stopBitsIndex > -1)
            {
                comboBoxStopBits.SelectedIndex = stopBitsIndex;
            }

            //Compile/Run 
            projectFilePreferencesControl.ProjectFileCopyBehavior = (FileLocationCopyBehavior)Properties.Settings.Default.DefaultFileCopyBehavior;

            //AutoCopy
            checkBoxAutoCopyDocuments.Checked = Properties.Settings.Default.AutoCopyDocuments;
            checkBoxAutoCopyExamples.Checked = Properties.Settings.Default.AutoCopyExamples;
            checkBoxAutoCopyTemplates.Checked = Properties.Settings.Default.AutoCopyTemplates;

            //Editor Options
            if (Properties.Settings.Default.ConvertTabsToSpaces >= numericUpDownTabsToSpaces.Minimum && Properties.Settings.Default.ConvertTabsToSpaces <= numericUpDownTabsToSpaces.Maximum)
            {
                numericUpDownTabsToSpaces.Value = Properties.Settings.Default.ConvertTabsToSpaces;
            }
            else
            {
                numericUpDownTabsToSpaces.Value = 5;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //data checks first...
            if (!Directory.Exists(textBoxFilesDirectory.Text))
            {
                DialogResult dr = MessageBox.Show("Files directory, does not exists. Would you like to create it?", "Create Folder", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                else if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    if (!Directory.Exists(textBoxFilesDirectory.Text))
                    {
                        Directory.CreateDirectory(textBoxFilesDirectory.Text);
                    }
                }
            }

            Properties.Settings.Default.COM_CableType = comboBoxSerialCableType.SelectedIndex;

            //save to settings here...
            Properties.Settings.Default.COM_Port = _comPort;
            Properties.Settings.Default.COM_Baud = _comBaudRate;
            Properties.Settings.Default.COM_Parity = Enum.GetName(typeof(Parity), _comParity);
            Properties.Settings.Default.COM_DataBits = _comDataBits;
            Properties.Settings.Default.COM_StopBits = Enum.GetName(typeof(StopBits), _comStopBits);

            Properties.Settings.Default.AutoConnect = checkBoxAutoConnect.Checked;
            Properties.Settings.Default.AutoSyncDateTime = checkBoxSyncDateTime.Checked;
            Properties.Settings.Default.DefaultFilesDirectory = textBoxFilesDirectory.Text;
            Properties.Settings.Default.DefaultLibloadDirectory = textBoxLibloadDirectory.Text;
            Properties.Settings.Default.SaveEditorWindows = checkBoxRememberDocuments.Checked;
            Properties.Settings.Default.SaveToolboxWindows = checkBoxRememberToolWindows.Checked;


            Properties.Settings.Default.DefaultFileCopyBehavior = (int)projectFilePreferencesControl.ProjectFileCopyBehavior;

            //AutoCopy
            Properties.Settings.Default.AutoCopyDocuments = checkBoxAutoCopyDocuments.Checked;
            Properties.Settings.Default.AutoCopyExamples = checkBoxAutoCopyExamples.Checked;
            Properties.Settings.Default.AutoCopyTemplates = checkBoxAutoCopyTemplates.Checked;


            //Editor Options
            Properties.Settings.Default.ConvertTabsToSpaces = (int)numericUpDownTabsToSpaces.Value;

            Properties.Settings.Default.Save();

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void buttonBrowseFilesDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.ShowNewFolderButton = true;
            if (!String.IsNullOrEmpty(textBoxFilesDirectory.Text))
            {
                fb.SelectedPath = textBoxFilesDirectory.Text;
            }
            DialogResult dr = fb.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                textBoxFilesDirectory.Text = fb.SelectedPath;
            }
        }

        private void buttonBroseLibloadDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.ShowNewFolderButton = true;
            if (!String.IsNullOrEmpty(textBoxLibloadDirectory.Text))
            {
                fb.SelectedPath = textBoxLibloadDirectory.Text;
            }
            DialogResult dr = fb.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                textBoxLibloadDirectory.Text = fb.SelectedPath;
            }
        }

        private void comboBoxPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            _comPort = comboBoxPorts.Text;
        }

        private void comboBoxBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _comBaudRate = Convert.ToInt16(comboBoxBaudRate.Text);
            }
            catch
            {
                MessageBox.Show("Invalid value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxParity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _comParity = (Parity)Enum.Parse(typeof(Parity), comboBoxParity.Text);
            }
            catch
            {
                MessageBox.Show("Invalid value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxDataBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _comDataBits = Convert.ToInt16(comboBoxDataBits.Text);
            }
            catch
            {
                MessageBox.Show("Invalid value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxStopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _comStopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBoxStopBits.Text);
            }
            catch
            {
                MessageBox.Show("Invalid value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxSerialCableType_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBoxCableStraightThrough.Visible = comboBoxSerialCableType.SelectedIndex == 0;
            labelInstrutionsStraightThrough.Visible = comboBoxSerialCableType.SelectedIndex == 0;
            pictureBoxCableNullModem.Visible = comboBoxSerialCableType.SelectedIndex != 0;
            labelInstructionsNullModem.Visible = comboBoxSerialCableType.SelectedIndex != 0;
        }
    }
}
