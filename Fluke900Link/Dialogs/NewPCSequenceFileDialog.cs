using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluke900Link.Dialogs
{
    public partial class NewPCSequenceFileDialog : Form
    {
        public string SequenceName { get; set; }

        public NewPCSequenceFileDialog()
        {
            InitializeComponent();
            textBoxSequenceName.Focus();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxSequenceName.Text))
            {
                MessageBox.Show("You must specify a name for the PC sequence.", "Name Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SequenceName = textBoxSequenceName.Text;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
