using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluke900Link.Controls
{
    public partial class LocationQuickAdd : UserControl
    {

        public delegate void CreateLocationDevice(string locationName, string deviceName);

        public CreateLocationDevice OnLocationDeviceAdded { get; set; }

        public LocationQuickAdd()
        {
            InitializeComponent();
        }

        private void LocationQuickAdd_Resize(object sender, EventArgs e)
        {
            int padding = 4;
            int startLocation = labelLocation.Left + labelLocation.Width + padding;
            int adjustmentWidth = Width - (labelLocation.Width + labelDevice.Width + buttonAdd.Width + (5*padding));
            textBoxLocation.Width = adjustmentWidth / 2;
            textBoxDevice.Width = adjustmentWidth / 2;
            labelDevice.Left = textBoxLocation.Left + textBoxLocation.Width + padding;
            textBoxDevice.Left = labelDevice.Left + labelDevice.Width + padding;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            QuickAdd();
        }

        private void textBoxLocation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                textBoxDevice.Focus();
            }
        }

        private void textBoxDevice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                //add it now
                QuickAdd();
            }
        }

        private void QuickAdd()
        {
            if (String.IsNullOrWhiteSpace(textBoxLocation.Text))
            {
                MessageBox.Show("Location name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (String.IsNullOrWhiteSpace(textBoxDevice.Text))
                {
                    MessageBox.Show("Device name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (OnLocationDeviceAdded != null)
                    {
                        OnLocationDeviceAdded(textBoxLocation.Text, textBoxDevice.Text);
                    }
                }
            }
        }
    }
}
