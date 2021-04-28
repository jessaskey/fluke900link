using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluke900Link
{
    public static class Utilities
    {
        public static string GetExecutablePath()
        {
            return Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath); 
        }

        public static void OpenFileInDefaultViewer(string filepath)
        {
            System.Diagnostics.Process.Start(filepath);
        }

        public static string GetDefaultDirectoryPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Fluke900Files");
        }

        public static string GetBrowseDirectory()
        {
            if (String.IsNullOrEmpty(Globals.LastDirectoryBrowse))
            {
                return Properties.Settings.Default.DefaultFilesDirectory;
            }
            return Globals.LastDirectoryBrowse;
        }

        public static void OpenDirectoryInWindowsExplorer(string filePath)
        {
            if (!String.IsNullOrEmpty(filePath))
            {
                if (Directory.Exists(filePath))
                {
                    Process.Start(filePath);
                }
                else
                {
                    MessageBox.Show("The directory '" + filePath + "' could not be found to open.", "Open Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public static TreeNode LoadFolder(string folder, List<string> visibleExtensions, bool showDirectoryOnly, bool showAllFolders)
        {
            TreeNode node = GetFolderTreeNode(folder);
            node.ImageIndex = 0;
            node.SelectedImageIndex = 0;
            node.Tag = folder;

            if (!showDirectoryOnly)
            {
                foreach (string file in Directory.GetFiles(folder))
                {
                    if (visibleExtensions == null || visibleExtensions.Where(s => s.ToLower() == Path.GetExtension(file.ToLower())).FirstOrDefault() != null)
                    {
                        node.Nodes.Add(GetFileTreeNode(file));
                    }
                }
            }

            foreach (string subfolder in Directory.GetDirectories(folder))
            {
                TreeNode subNode = LoadFolder(subfolder, visibleExtensions, showDirectoryOnly, showAllFolders);

                if (subNode != null)
                {
                    node.Nodes.Insert(0, subNode);
                }
            }

            if (node.Nodes.Count > 0 || showAllFolders)
            {
                return node;
            }
            return null;
        }

        public static TreeNode GetFolderTreeNode(string folder)
        {
            string[] folderParts = folder.Split('\\');
            TreeNode node = new TreeNode(folderParts[folderParts.Length - 1]);
            node.SelectedImageIndex = 0;
            return node;
        }

        public static TreeNode GetFileTreeNode(string pathName)
        {
            TreeNode node = new TreeNode(Path.GetFileName(pathName));
            switch (Path.GetExtension(pathName).ToLower())
            {
                case ".lib":
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                    break;
                default:
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                    break;
            }
            
            node.Tag = pathName;
            return node;
        }

        public static void LoadTreeFromPath(TreeView treeView, string startPath, List<string> visibleExtensions, bool hideEmptyFolders, bool showDirectoryOnly, InitialTreeStatus expandStatus)
        {
            treeView.Nodes.Clear();

            if (Directory.Exists(startPath))
            {
                //always add the root node if it exists
                TreeNode rootNode = new TreeNode(startPath);
                rootNode.ImageIndex = 0;
                rootNode.SelectedImageIndex = 0;
                rootNode.Tag = startPath;
                treeView.Nodes.Add(rootNode);

                if (!showDirectoryOnly)
                {
                    foreach (string file in Directory.GetFiles(startPath))
                    {
                        if (visibleExtensions == null || visibleExtensions.Where(s => s.ToLower() == Path.GetExtension(file.ToLower())).FirstOrDefault() != null)
                        {
                            rootNode.Nodes.Add(Utilities.GetFileTreeNode(file));
                        }
                    }
                }

                foreach (string folder in Directory.GetDirectories(startPath))
                {
                    TreeNode node = Utilities.LoadFolder(folder, visibleExtensions, showDirectoryOnly, hideEmptyFolders);
                    if (node != null || !hideEmptyFolders)
                    {
                        rootNode.Nodes.Insert(0, node);
                    }
                }
            }

            switch (expandStatus)
            {
                case InitialTreeStatus.FirstNodeExpanded:
                    if (treeView.Nodes.Count > 0)
                    {
                        treeView.Nodes[0].Expand();
                    }
                    break;
                case InitialTreeStatus.AllNodesExpanded:
                    treeView.ExpandAll();
                    break;
            }


        }

    }
}
