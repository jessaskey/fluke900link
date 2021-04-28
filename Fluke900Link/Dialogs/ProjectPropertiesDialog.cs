using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Containers;

namespace Fluke900Link.Dialogs
{
    public partial class ProjectPropertiesDialog : Form
    {
        public ProjectPropertiesDialog()
        {
            InitializeComponent();

            Project p = ProjectFactory.CurrentProject;

            if (ProjectFactory.CurrentProject != null)
            {
                textBoxProjectPathFile.Text = p.ProjectPathFile;
                checkBoxAutoIncludeDevices.Checked = p.AutoBuildDeviceLibraries;
                checkBoxUseSimulationData.Checked = p.IncludeSimulationData;
            }
            else
            {
                MessageBox.Show("There is no project context.", "Project Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }

            projectFilePreferencesControl1.ProjectFileCopyBehavior = p.FileLocationCopyBehavior;

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;

            if (checkBoxAutoIncludeDevices.Checked != ProjectFactory.CurrentProject.AutoBuildDeviceLibraries)
            {
                ProjectFactory.CurrentProject.AutoBuildDeviceLibraries = checkBoxAutoIncludeDevices.Checked;
                ProjectFactory.CurrentProject.IsModified = true;
            }
            if (checkBoxUseSimulationData.Checked != ProjectFactory.CurrentProject.IncludeSimulationData)
            {
                ProjectFactory.CurrentProject.IncludeSimulationData = checkBoxUseSimulationData.Checked;
                ProjectFactory.CurrentProject.IsModified = true;
            }

            ProjectFactory.CurrentProject.FileLocationCopyBehavior = projectFilePreferencesControl1.ProjectFileCopyBehavior;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
