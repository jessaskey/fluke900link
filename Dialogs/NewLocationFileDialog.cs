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
    public partial class NewLocationFileDialog : Form
    {
        public string LibraryName { get; set; }
        public string ProjectPath { get; set; }
        public string CreatedLocationFile { get; set; }

        public NewLocationFileDialog(string projectPath)
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
                    CreatedLocationFile = Path.Combine(ProjectPath, textBoxLocationFileName.Text + ".loc");
                    string templateContent = Helpers.FileHelper.GetTemplate(".loc");
                    File.WriteAllText(CreatedLocationFile, templateContent);
                    Close();
                }
            }
            else
            {
                MessageBox.Show("You must specifiy a location file name.", "Location Filename Error", MessageBoxButtons.OK);
            }
        }
    }
}
