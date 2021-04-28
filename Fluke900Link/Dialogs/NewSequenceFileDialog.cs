using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluke900Link.Dialogs
{
    public partial class NewSequenceFileDialog : Form
    {
        public string LibraryName { get; set; }
        public string ProjectPath { get; set; }
        public string CreatedSequenceFile { get; set; }

        public NewSequenceFileDialog(string projectPath)
        {
            InitializeComponent();
            ProjectPath = projectPath;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxLocationFileName.Text))
            {
                if (Helpers.FileHelper.ValidateFilename(textBoxLocationFileName.Text))
                {
                    CreatedSequenceFile = Path.Combine(ProjectPath, textBoxLocationFileName.Text + ".seq");
                    string templateContent = Helpers.FileHelper.GetTemplate(".seq");
                    File.WriteAllText(CreatedSequenceFile, templateContent);
                    Close();
                }
            }
            else
            {
                MessageBox.Show("You must specifiy a sequence file name.", "Sequence Filename Error", MessageBoxButtons.OK);
            }
        }
    }
}
