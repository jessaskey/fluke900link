using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Containers;
using Fluke900Link.Dialogs;
using Fluke900Link.Extensions;
using Fluke900Link.Factories;
using WeifenLuo.WinFormsUI.Docking;

namespace Fluke900Link.Controls
{
    public partial class SolutionExplorer : DockContentEx
    {


        public SolutionExplorer()
        {
            InitializeComponent();
            treeViewSolution.Nodes.Add(new TreeNode("<No Project Loaded>"));

            toolStripSolution.Enabled = false;
        }

        public bool LoadProject(Project project)
        {
            string selectedNodeText = null;
            if (treeViewSolution.SelectedNode != null)
            {
                selectedNodeText = treeViewSolution.SelectedNode.Text;
            }

            treeViewSolution.Nodes.Clear();

            if (project != null)
            {
                TreeNode projectNode = new TreeNode(Path.GetFileName(project.ProjectPathFile));
                projectNode.ToolTipText = project.ProjectPathFile;
                Font boldFont = new Font(treeViewSolution.Font, FontStyle.Bold);
                projectNode.NodeFont = boldFont;
                projectNode.Text = projectNode.Text.Replace(Path.GetExtension(projectNode.Text), "");
                projectNode.ImageIndex = (int)ProjectNodeType.Project;
                projectNode.SelectedImageIndex = (int)ProjectNodeType.Project;
                projectNode.Tag = project.ProjectPathFile;

                foreach (string file in project.Files.Select(f=>f.PathFileName))
                {
                    TreeNode fileNode = new TreeNode(Path.GetFileName(file));
                    fileNode.Tag = file;
                    projectNode.Nodes.Add(fileNode);
                    switch (Path.GetExtension(file).ToLower())
                    {
                        case ".lib":
                            fileNode.ImageIndex = (int)ProjectNodeType.Library;
                            fileNode.SelectedImageIndex = (int)ProjectNodeType.Library;
                            break;
                        case ".loc":
                            fileNode.ImageIndex = (int)ProjectNodeType.Location;
                            fileNode.SelectedImageIndex = (int)ProjectNodeType.Location;
                            break;
                        case ".lst":
                            fileNode.ImageIndex = (int)ProjectNodeType.List;
                            fileNode.SelectedImageIndex = (int)ProjectNodeType.List;
                            break;
                        case ".seq":
                            fileNode.ImageIndex = (int)ProjectNodeType.Sequence;
                            fileNode.SelectedImageIndex = (int)ProjectNodeType.Sequence;
                            break;
                    }
                }

                treeViewSolution.Nodes.Add(projectNode);
                treeViewSolution.ExpandAll();

                if (!String.IsNullOrEmpty(selectedNodeText))
                {
                    TreeNode sn = treeViewSolution.Nodes.OfType<TreeNode>().Where(n => n.Text.ToLower() == selectedNodeText.ToLower()).FirstOrDefault();
                    if (sn != null)
                    {
                        treeViewSolution.SelectedNode = sn;
                    }
                }

                toolStripSolution.Enabled = true;
                return true;
            }

            return false;
        }

        private void toolStripButtonAddSequence_Click(object sender, EventArgs e)
        {
            string projectFolder = Path.GetDirectoryName(treeViewSolution.Nodes[0].Tag.ToString());

            NewSequenceFileDialog sd = new NewSequenceFileDialog(projectFolder);
            DialogResult dr = sd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //get our template file
                if (File.Exists(sd.CreatedSequenceFile))
                {
                    ProjectSequenceFile sequence = new ProjectSequenceFile(sd.CreatedSequenceFile); ;
                    ProjectFactory.CurrentProject.Sequences.Add(sequence);
                    ProjectFactory.CurrentProject.IsModified = true;

                    //refresh tree
                    LoadProject(ProjectFactory.CurrentProject);

                    //open in editor
                    TreeNode libraryNode = treeViewSolution.GetAllTreeNodes().Where(n => n.Tag != null && n.Tag.ToString().ToUpper() == sd.CreatedSequenceFile).FirstOrDefault();
                    treeViewSolution.SelectedNode = libraryNode;

                    ControlFactory.OpenProjectFileInEditor(sequence);
                    //ControlFactory.UIElements.MainForm.OpenExistingDocumentInEditor(locations.PathFileName);
                }
            }
        }

        private void toolStripButtonAddLocation_Click(object sender, EventArgs e)
        {
            string projectFolder = Path.GetDirectoryName(treeViewSolution.Nodes[0].Tag.ToString());

            NewLocationFileDialog fd = new NewLocationFileDialog(projectFolder);
            DialogResult dr = fd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //get our template file
                if (File.Exists(fd.CreatedLocationFile))
                {
                    ProjectLocationFile locations = new ProjectLocationFile(fd.CreatedLocationFile); ;
                    ProjectFactory.CurrentProject.Locations.Add(locations);
                    ProjectFactory.CurrentProject.IsModified = true;

                    //refresh tree
                    LoadProject(ProjectFactory.CurrentProject);

                    //open in editor
                    TreeNode libraryNode = treeViewSolution.GetAllTreeNodes().Where(n => n.Tag != null && n.Tag.ToString().ToUpper() == fd.CreatedLocationFile).FirstOrDefault();
                    treeViewSolution.SelectedNode = libraryNode;

                    ControlFactory.OpenProjectFileInEditor(locations);
                    //ControlFactory.UIElements.MainForm.OpenExistingDocumentInEditor(locations.PathFileName);
                }
            }
        }

        private void toolStripButtonAddLibrary_Click(object sender, EventArgs e)
        {
            NewLibraryDialog nl = new NewLibraryDialog();
            if (treeViewSolution.Nodes.Count > 0 && treeViewSolution.Nodes[0].Tag != null)
            {
                string projectFolder = Path.GetDirectoryName(treeViewSolution.Nodes[0].Tag.ToString());
                nl.CreateFolder = projectFolder;
                DialogResult dr = nl.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    //add to project
                    if (File.Exists(nl.CreatedLibraryFile))
                    {
                        ProjectLibraryFile library = new ProjectLibraryFile(nl.CreatedLibraryFile, nl.LibraryType); ;
                        ProjectFactory.CurrentProject.Libraries.Add(library);
                        ProjectFactory.CurrentProject.IsModified = true;

                        //refresh tree
                        LoadProject(ProjectFactory.CurrentProject);

                        //open in editor
                        TreeNode libraryNode = treeViewSolution.GetAllTreeNodes().Where(n => n.Tag != null && n.Tag.ToString().ToUpper() == nl.CreatedLibraryFile).FirstOrDefault();
                        treeViewSolution.SelectedNode = libraryNode;

                        ControlFactory.OpenProjectFileInEditor(library);
                    }
                }
            }
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            LoadProject(ProjectFactory.CurrentProject);
        }

        private void toolStripButtonOpenExplorer_Click(object sender, EventArgs e)
        {
            if (ProjectFactory.CurrentProject != null && File.Exists(ProjectFactory.CurrentProject.ProjectPathFile))
            {
                Utilities.OpenDirectoryInWindowsExplorer(Path.GetDirectoryName(ProjectFactory.CurrentProject.ProjectPathFile));
            }
            else
            {
                Utilities.OpenDirectoryInWindowsExplorer(Properties.Settings.Default.DefaultFilesDirectory);
            }
        }

        private void excludeFromProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeViewSolution.SelectedNode != null)
            {
                ProjectFactory.CurrentProject.Files.RemoveAll(f => f.PathFileName.ToUpper().EndsWith(treeViewSolution.SelectedNode.Text.ToUpper()));
                ProjectFactory.CurrentProject.IsModified = true;
                LoadProject(ProjectFactory.CurrentProject);
            }
        }

        private void toolStripButtonAddExisting_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "Fluke Files (*.lib,*.loc,*.seq)|*.lib;*.loc;*.seq|All Files (*.*)|*";
            od.Multiselect = true;
            od.CheckFileExists = true;
            if (ProjectFactory.CurrentProject != null && Directory.Exists(Path.GetDirectoryName(ProjectFactory.CurrentProject.ProjectPathFile)))
            {
                od.InitialDirectory = Path.GetDirectoryName(ProjectFactory.CurrentProject.ProjectPathFile);
            }
            else
            {
                od.InitialDirectory = Utilities.GetBrowseDirectory();
            }
            DialogResult dr = od.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in od.FileNames)
                {
                    if (ProjectFactory.CurrentProject.Files.Where(f=>f.PathFileName.Contains(Path.GetFileName(file))).FirstOrDefault() != null)
                    {
                        MessageBox.Show("File is already in this project.", "Add Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ProjectFactory.CurrentProject.AddFile(file);
                        ProjectFactory.CurrentProject.IsModified = true;
                        Globals.LastDirectoryBrowse = Path.GetDirectoryName(file);
                        LoadProject(ProjectFactory.CurrentProject);
                    }
                }
            }
        }

        private void contextMenuStripTree_Opening(object sender, CancelEventArgs e)
        {
            if (treeViewSolution.SelectedNode != null && treeViewSolution.Nodes.Count > 0 && treeViewSolution.SelectedNode == treeViewSolution.Nodes[0])
            {
                //this could be the 'empty project' node... if so, cancel this
                if (treeViewSolution.SelectedNode.Tag == null)
                {
                    e.Cancel = true;
                }
                //main project node
                propertiesToolStripMenuItem.Visible = true;
                excludeFromProjectToolStripMenuItem.Visible = false;
            }
            else
            {
                //subnodes
                propertiesToolStripMenuItem.Visible = false;
                excludeFromProjectToolStripMenuItem.Visible = true;
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this node should only be the Project...
            TreeNode node = treeViewSolution.SelectedNode;
            if (node != null)
            {
                if (node.ImageIndex == (int)ProjectNodeType.Project)
                {
                    ProjectPropertiesDialog pd = new ProjectPropertiesDialog();
                    pd.ShowDialog();
                }
            }


        }

        private void treeViewSolution_DoubleClick(object sender, EventArgs e)
        {
            //open in editor
            if (ControlFactory.MainForm2 != null)
            {
                if (treeViewSolution.SelectedNode != null && treeViewSolution.SelectedNode.Tag != null)
                {
                    string projectPathFile = treeViewSolution.SelectedNode.Tag.ToString();
                    ControlFactory.OpenExistingDocumentInEditor(projectPathFile);
                }
                
            }
        }

        private void openFileLocationInWindowsExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeViewSolution.SelectedNode;
            if (node != null)
            {
                if (node.Tag != null)
                {
                    if (File.Exists(node.Tag.ToString()))
                    {
                        string directory = Path.GetDirectoryName(node.Tag.ToString());
                        Utilities.OpenDirectoryInWindowsExplorer(directory);
                    }
                }
            }
        }

        private void toolStripButtonExpandAll_Click(object sender, EventArgs e)
        {
            foreach(TreeNode node in treeViewSolution.Nodes)
            {
                node.Expand();
                node.ExpandAll();
            }
        }

        private void toolStripButtonCollapseAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeViewSolution.Nodes)
            {
                node.Collapse(false);
            }
        }

        private void toolStripButtonCheckForErrors_Click(object sender, EventArgs e)
        {
            CheckProjectForErrors();


        }

        private void toolStripButtonRunSequence_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //this button will do the following
            // 1. Check the project for errors
            if (CheckProjectForErrors())
            {
                DialogResult dr = MessageBox.Show("Project has issues. Please review the developer console to fix. Would you like to proceed anyway?", "Project Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.No)
                {
                    return;
                    Cursor.Current = Cursors.Default;
                }
            }
            
            // 2. If autogenerate libs is on, then make a library file with simulation 
            //and shadow data as defined in project, the filename is always the default
            //filename as defined in the settings of the application.
            if (ProjectFactory.GenerateLibraries())
            {
                // 3. If okay, then copy the whole project over to the fluke
                List<Tuple<string,bool>> filesCopied = ProjectFactory.CopyFilesToFluke(true);
                if (filesCopied.Count > 0)
                {
                    // 4. Compile all files
                    if (ProjectFactory.CompileProjectFiles(filesCopied.Where(f => f.Item2).Select(f => f.Item1).ToList()))
                    {
                        MessageBox.Show("Project files were copied and compiled sucessfully to the Fluke. You can now manually run the sequence by disconnecting Fluke900Link and manually running the sequence using the keypad.", "Project Sequences Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Project files were copied to Fluke but there were compilation errors reported from the Fluke.", "Compile Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("There were zero files copied. Check the exception log for more information.", "File Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private bool CheckProjectForErrors()
        {
            //save all project files
            ControlFactory.SaveAllOpenFiles();
            //quick check project for errors
            DeveloperOutput devo = ControlFactory.ShowDockWindow(DockWindowControls.DeveloperOutput) as DeveloperOutput;
            devo.ClearIssues();
            ProjectFactory.ParseProjectCommands((LogIssueHandler)devo.AddIssue, (LogMessageHandler)devo.AddOutputLine);

            //return value of and command file errors...
            return ProjectFactory.HasCommandErrors;
        }

        private void toolStripButtonAddPCSequence_Click(object sender, EventArgs e)
        {
            NewPCSequenceFileDialog sfd = new NewPCSequenceFileDialog();
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string sequencePathFile = Path.Combine(Path.GetDirectoryName(ProjectFactory.CurrentProject.ProjectPathFile), sfd.SequenceName + ".psq");
                File.WriteAllText(sequencePathFile, "");
                ControlFactory.OpenPCSequence(sequencePathFile);
            }
        }




    }
}
