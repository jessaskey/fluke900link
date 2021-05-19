using Fluke900.Helpers;
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
                List<string> errors = new List<string>();
                if (FileHelper.ValidateFilename(textBoxLocationFileName.Text, errors))
                {
                    CreatedLocationFile = Path.Combine(ProjectPath, textBoxLocationFileName.Text + ".loc");
                    string templateContent = FileHelper.GetTemplate(".loc", Properties.Settings.Default.DefaultFilesDirectory);
                    File.WriteAllText(CreatedLocationFile, templateContent);
                    Close();
                }
                else
                {
                    MessageBox.Show(String.Join("\r\n",errors.ToArray()), "Filename Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                MessageBox.Show("You must specifiy a location file name.", "Location Filename Error", MessageBoxButtons.OK);
            }
        }
    }
}
