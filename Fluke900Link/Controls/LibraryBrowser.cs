using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Dialogs;
using Fluke900Link.Factories;

namespace Fluke900Link.Controls
{
    public partial class LibraryBrowser : UserControl
    {
        private const string LIBFILE_EXTENSION = ".lib";
        public LibraryBrowser()
        {
            InitializeComponent();
            treeViewLibraries.ImageList = ControlFactory.ImageList16x16;
        }

        public void LoadFiles()
        {
            Utilities.LoadTreeFromPath(treeViewLibraries, Properties.Settings.Default.DefaultFilesDirectory, new List<string>() { ".lib" }, true, false, InitialTreeStatus.AllNodesExpanded);
        }

        //private TreeNode LoadFolder(string folder)
        //{
        //    TreeNode node = GetFolderTreeNode(folder); 

        //    foreach (string file in Directory.GetFiles(folder))
        //    {
        //        if (Path.GetExtension(file.ToLower()) == ".lib")
        //        {
        //            node.Nodes.Add(GetLibraryTreeNode(file));
        //        }
        //    }

        //    foreach (string subfolder in Directory.GetDirectories(folder))
        //    {
        //        TreeNode subNode = LoadFolder(subfolder);
          
        //        if (subNode != null)
        //        {
        //            node.Nodes.Insert(0, subNode);
        //        }
        //    }

        //    if (node.Nodes.Count > 0 || toolStripButtonShowAllFolders.Checked)
        //    {
        //        return node;
        //    }
        //    return null;
        //}

        //private TreeNode GetFolderTreeNode(string folder)
        //{
        //    string[] folderParts = folder.Split('\\');
        //    TreeNode node = new TreeNode(folderParts[folderParts.Length - 1]);
        //    node.SelectedImageIndex = 0;
        //    return node;
        //}

        //private TreeNode GetLibraryTreeNode(string pathName)
        //{
        //    TreeNode node = new TreeNode(Path.GetFileName(pathName));
        //    node.ImageIndex = 2;
        //    node.SelectedImageIndex = 2;
        //    node.Tag = pathName;
        //    return node;
        //}

        private void toolStripButtonShowAllFolders_CheckStateChanged(object sender, EventArgs e)
        {
            LoadFiles();
        }

        private void treeViewLibraries_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.ImageIndex == 0)
            {
                e.Node.ImageIndex = 1;
                e.Node.SelectedImageIndex = 1;
            }
        }

        private void treeViewLibraries_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.ImageIndex == 1)
            {
                e.Node.ImageIndex = 0;
                e.Node.SelectedImageIndex = 0;
            }
        }

        private void toolStripButtonOpenExplorer_Click(object sender, EventArgs e)
        {
            if (treeViewLibraries.SelectedNode != null)
            {
                Utilities.OpenDirectoryInWindowsExplorer(Path.GetDirectoryName(treeViewLibraries.SelectedNode.Tag.ToString()));
            }
        }

        private void toolStripButtonLibraryNew_Click(object sender, EventArgs e)
        {
            NewLibraryDialog nl = new NewLibraryDialog();
            if (treeViewLibraries.SelectedNode != null)
            {
                nl.CreateFolder = Path.GetDirectoryName(treeViewLibraries.SelectedNode.Tag.ToString());
            }
            DialogResult dr = nl.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //open the file

            }

        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            LoadFiles();
        }

        private void treeViewLibraries_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //open the .lib file in the main editor
            TreeNode node = treeViewLibraries.SelectedNode;
            if (node != null && node.Tag != null)
            {
                ControlFactory.OpenExistingDocumentInEditor(node.Tag.ToString());
            }
        }
    }
}
