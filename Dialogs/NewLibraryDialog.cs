using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Containers;

namespace Fluke900Link.Dialogs
{
    public partial class NewLibraryDialog : Form
    {

        private string _createdLibraryFile = "";

        public LibraryFileType LibraryType
        {
            get
            {
                return (LibraryFileType)comboBoxSourceType.SelectedIndex;
            }
        }

        public string CreatedLibraryFile
        {
            get { return _createdLibraryFile; }
        }

        public string CreateFolder
        {
            get
            {
                return textBoxCreateFolder.Text;
            }
            set
            {
                textBoxCreateFolder.Text = value;
            }
        }
        public NewLibraryDialog()
        {
            InitializeComponent();

            comboBoxSourceType.SelectedIndex = 0;

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //create the .LIB file now...
            if (!String.IsNullOrWhiteSpace(textBoxLibraryName.Text))
            {
                Regex r = new Regex(@"^[A-Z][A-Z0-9_]*$");
                string libraryName = textBoxLibraryName.Text;
                if (r.IsMatch(libraryName))
                {
                    if (libraryName.Length > 15)
                    {
                        MessageBox.Show("Library Name cannot be longer than 15 characters.", "Library Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string folder = textBoxCreateFolder.Text;
                        if (Directory.Exists(folder))
                        {
                            Project currentProject = ProjectFactory.CurrentProject;
                            string fullPathFilename = Path.Combine(folder, libraryName + ".LIB").ToUpper();
                            if (File.Exists(fullPathFilename))
                            {
                                DialogResult dr = MessageBox.Show("Library file '" + Path.GetFileName(fullPathFilename) + "' alredy exists in this location. Would you like to overwrite the file?", "Confirm File Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                if (dr == System.Windows.Forms.DialogResult.No)
                                {
                                    return;
                                }
                                else
                                {
                                    //delete the file
                                    File.Delete(fullPathFilename);
                                    if (currentProject != null)
                                    {
                                        currentProject.RemoveFileFromCurrentProject(Path.GetFileName(fullPathFilename));
                                    }
                                }
                            }  
                            File.Create(fullPathFilename);
                            _createdLibraryFile = fullPathFilename;
                            DialogResult = System.Windows.Forms.DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Folder does not exists - '" + folder + "'", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    //bad characters
                    MessageBox.Show("Library name can only contain letters, numbers and hypens.", "Name Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Library name cannot be blank.", "Name Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonBrowseCreateFolder_Click(object sender, EventArgs e)
        {

        }

        private void buttonBrowseCloneFile_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBoxSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxSourceType.SelectedIndex)
            {
                //Reference Libraries
                case 0:
                    textBoxCopyFrom.Enabled = false;
                    buttonBrowseCloneFile.Enabled = false;
                    break;
                case 1:
                    textBoxCopyFrom.Enabled = true;
                    buttonBrowseCloneFile.Enabled = true;
                    break;
                case 2:
                    textBoxCopyFrom.Enabled = false;
                    buttonBrowseCloneFile.Enabled = false;
                    break;
            }
        }
    }
}
