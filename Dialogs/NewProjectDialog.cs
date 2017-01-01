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

using Fluke900Link.Containers;
using Fluke900Link.Helpers;

namespace Fluke900Link.Dialogs
{
    public partial class NewProjectDialog : Form
    {
        public Project NewProject { get; set; }

        public NewProjectDialog()
        {
            InitializeComponent();
            textBoxCreateDirectory.Text = Properties.Settings.Default.DefaultFilesDirectory;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void buttonBrowseProjectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.SelectedPath = Utilities.GetBrowseDirectory();
            DialogResult dr = fd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                textBoxCreateDirectory.Text = fd.SelectedPath;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string rootDirectory = textBoxCreateDirectory.Text;

            if (checkBoxCreateDirectory.Checked)
            {
                string targetDirectory = Path.Combine(textBoxCreateDirectory.Text, textBoxProjectName.Text);
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }
                rootDirectory = targetDirectory;
            }

            if (Directory.Exists(rootDirectory))
            {
                string projectPathFile = Path.Combine(rootDirectory, textBoxProjectName.Text + ".f9p");
                if (File.Exists(projectPathFile))
                {
                    DialogResult dr = MessageBox.Show("Project file already exists, do you want to overwrite it?", "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }

                Project project = new Project();
                project.ProjectPathFile = projectPathFile;
                project.IsModified = true;
                project.FileLocationCopyBehavior = (FileLocationCopyBehavior)Properties.Settings.Default.DefaultFileCopyBehavior;

                string[] existingFiles = Directory.GetFiles(rootDirectory);
                if (existingFiles.Length > 0)
                {
                    DialogResult dr = MessageBox.Show("There are already files in this directory, would you like to add them to the project now?", "Add Existing Files?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        foreach (string file in existingFiles)
                        {
                            project.AddFile(file);
                        }
                    }
                }

                NewProject = project;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Create Directory does not exist.", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
