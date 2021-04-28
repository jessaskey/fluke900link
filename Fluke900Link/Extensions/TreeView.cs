using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fluke900Link.Extensions
{
    public static class TreeViewExtensions
    {
        public static List<TreeNode> GetAllTreeNodes(this TreeView treeView)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            foreach (TreeNode rootNode in treeView.Nodes)
            {
                nodes.Add(rootNode);
                GetAllDescendents(rootNode, nodes);
            }
            return nodes;
        }

        private static void GetAllDescendents(TreeNode parentNode, List<TreeNode> nodes)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                nodes.Add(childNode);
                GetAllDescendents(childNode, nodes);
            }
        }
    }
}
