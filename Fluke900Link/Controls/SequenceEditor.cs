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

using Fluke900Link.Containers;
using Fluke900Link.Dialogs;
using Fluke900Link.Helpers;
using Telerik.WinControls.UI.Docking;
using Fluke900.Containers;

namespace Fluke900Link.Controls
{
    public partial class SequenceEditor : DockContentEx
    {
        private TreeNode _sequenceNode = null;
        private ProjectSequence _sequence = null;
        private DeviceLibrary _matchedLibrary = null;
        private SequenceDevice _currentDevice = null;

        public SequenceEditor()
        {
            InitializeComponent();
        }

        public bool OpenSequence(ProjectSequence sequence)
        {
            bool success = false;

            treeViewLocations.Nodes.Clear();
            _sequence = sequence;

            //set up our tree node
            TreeNode _sequenceNode = new TreeNode(_sequence.Title);
            _sequenceNode.ImageIndex = 0;
            _sequenceNode.SelectedImageIndex = 0;

            TreeNode newDeviceNode = new TreeNode("<new location>");
            newDeviceNode.ImageIndex = 1;
            newDeviceNode.SelectedImageIndex = 1;
            newDeviceNode.Tag = new SequenceDevice();
            _sequenceNode.Nodes.Add(newDeviceNode);

            treeViewLocations.Nodes.Add(_sequenceNode);
            treeViewLocations.SelectedNode = newDeviceNode;

            success = true;
            return success;
        }

        private void treeViewLocations_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeViewLocations_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeViewLocations_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode NewNode = null;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                NewNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if (DestinationNode.TreeView != NewNode.TreeView)
                {
                    DestinationNode.Nodes.Add((TreeNode)NewNode.Clone());
                    DestinationNode.Expand();
                    //Remove Original Node
                    NewNode.Remove();
                }
            }
        }

        private void textBoxICName_Leave(object sender, EventArgs e)
        {
            DeviceLibrary lib = LibraryHelper.GetDeviceLibrary(textBoxICName.Text);
            if (lib != null)
            {
                _matchedLibrary = lib;
                CmdSize size = lib.Size;
                if (size != null)
                {
                    int index = comboBoxICSize.FindStringExact(size.Value.ToString());
                    if (index > -1)
                    {
                        comboBoxICSize.SelectedIndex = index;
                    }
                    textBoxVccPins.Text = String.Join(",", size.Vcc.Select(v => v.ToString()).ToArray());
                    textBoxGndPins.Text = String.Join(",", size.Gnd.Select(v => v.ToString()).ToArray());
                }
            }
            else
            {
                MessageBox.Show("Device not found in library.", "Device not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxLocationName_Leave(object sender, EventArgs e)
        {
            //on leave, check if this is valid and then change it in the tree...

            TreeNode currentNode = treeViewLocations.SelectedNode;
            if (currentNode != null)
            {
                currentNode.Text = textBoxLocationName.Text;
            }
        }

        private void buttonPinActivity_Click(object sender, EventArgs e)
        {
            if (_currentDevice != null)
            {
                if (_matchedLibrary != null)
                {
                    DevicePinDefinitionDialog pdd = new DevicePinDefinitionDialog();
                    pdd.Initialize(_matchedLibrary.Size.Value, _matchedLibrary.ToString(), new List<string>() { "H", "L", "A", "F", "X" });
                    DialogResult dr = pdd.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        
                    }
                }
            }
        }

        private void treeViewLocations_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeViewLocations.SelectedNode != null)
            {
                SequenceDevice sd = treeViewLocations.SelectedNode.Tag as SequenceDevice;
                if (sd != null)
                {
                    LoadNode(sd);
                }
            }
        }
        
        private void LoadNode(SequenceDevice sd)
        {
            _currentDevice = sd;
            DeviceLibrary lib = LibraryHelper.GetDeviceLibrary(sd.DeviceName);
            if (lib != null)
            {
                _matchedLibrary = lib;
            }
        }
    }
}
