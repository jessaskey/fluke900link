using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Containers;

namespace Fluke900Link.Controls
{
    public partial class DeveloperConsole : UserControl
    {
        private List<ProjectIssue> _issues = new List<ProjectIssue>();

        public DeveloperConsole()
        {
            InitializeComponent();
            objectListViewIssues.Objects = _issues;
        }


        public void ClearIssues()
        {
            _issues.Clear();
            objectListViewIssues.Objects = _issues;
            textBoxOutput.Text = "";
        }

        public void AddOutputLine(string output)
        {
            textBoxOutput.Text += (output + "\r\n");
        }

        public void AddIssue(ProjectIssue pi)
        {
            _issues.Add(pi);
            objectListViewIssues.Objects = _issues;
        }

        private void objectListViewIssues_ItemActivate(object sender, EventArgs e)
        {
            ProjectIssue pi = objectListViewIssues.GetItem(objectListViewIssues.SelectedIndex).RowObject as ProjectIssue;

            if (pi != null)
            {

            }
        }


    }
}
