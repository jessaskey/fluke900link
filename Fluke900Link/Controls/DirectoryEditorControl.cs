using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Containers;
using Fluke900Link.Controllers;
using Fluke900Link.Extensions;
using Fluke900Link.Factories;
using Fluke900Link.Helpers;
using Telerik.WinControls.UI.Docking;
using WeifenLuo.WinFormsUI.Docking;
using Fluke900;
using Fluke900.Containers;
using Fluke900.Helpers;

namespace Fluke900Link.Controls
{
    public partial class DirectoryEditorControl : DockContentEx
    {
        private FileLocations? _fileLocation = null;
        private string _localBaseFilePath = "";
        private string _localCurrentFilePath = "";

        private bool _isDragging = false;
        private System.Drawing.Point _draggingStartPoint = new System.Drawing.Point();


        public FileLocations? FileLocation
        {
            get
            {
                return _fileLocation;
            }
            set
            {
                _fileLocation = value;
                toolStripButtonFormat.Visible = _fileLocation.Value == FileLocations.FlukeCartridge;
                toolStripButtonExplore.Visible = _fileLocation.Value == FileLocations.LocalComputer;
                toolStripLabelReadWriteLabel.Visible = _fileLocation.Value == FileLocations.FlukeCartridge;
                toolStripButtonCompile.Visible = _fileLocation.Value != FileLocations.LocalComputer;

                if (_fileLocation.HasValue)
                {
                    //enable the 'other' buttons
                    switch (_fileLocation.Value)
                    {
                        case FileLocations.LocalComputer:
                            toolStripButtonToPC.Visible = false;
                            toolStripButtonToSYST.Visible = true;
                            toolStripButtonToCART.Visible = true;
                            break;
                        case FileLocations.FlukeSystem:
                            toolStripButtonToPC.Visible = true;
                            toolStripButtonToSYST.Visible = false;
                            toolStripButtonToCART.Visible = true;
                            break;
                        case FileLocations.FlukeCartridge:
                            toolStripButtonToPC.Visible = true;
                            toolStripButtonToSYST.Visible = true;
                            toolStripButtonToCART.Visible = false;
                            break;
                    }
                }
                toolStripLabelReadWriteLabel.Text = "";
            }
        }

        public DirectoryEditorControl()
        {
            InitializeComponent();
            toolStripButtonFormat.Visible = false;
            treeViewMain.ImageList = ControlFactory.ImageList16x16;
        }

        public void ConnectionStatusChanged(object sender, ConnectionStatus currentStatus)
        {
            if (currentStatus == ConnectionStatus.Connected)
            {
                toolStripButtonToCART.Enabled = true;
                toolStripButtonToPC.Enabled = true;
                toolStripButtonToSYST.Enabled = true;

                if (_fileLocation.HasValue)
                {
                    switch (_fileLocation.Value)
                    {
                        case FileLocations.FlukeCartridge:
                        case FileLocations.FlukeSystem:
                            LoadFiles();
                            break;
                    }
                }
            }
            else if (currentStatus == ConnectionStatus.Disconnected || currentStatus == ConnectionStatus.Unknown)
            {
                toolStripButtonToCART.Enabled = false;
                toolStripButtonToPC.Enabled = false;
                toolStripButtonToSYST.Enabled = false;

                if (_fileLocation.HasValue)
                {
                    switch (_fileLocation.Value)
                    {
                        case FileLocations.FlukeCartridge:
                        case FileLocations.FlukeSystem:
                            ShowDisconnected();
                            break;
                    }
                }
            }
        }


        public bool LoadFiles()
        {
            bool result = false;
            DirectoryListingInfo dl = null;

            if (_fileLocation.HasValue)
            {
                switch (_fileLocation.Value)
                {
                    case FileLocations.LocalComputer:
                        //local computer will get file listing directly and not use DirectoryListing
                        ProgressManager.Start("Loading PC Working Directory Files...");

                        string lastPath = "";
                        List<string> expandedNodes = treeViewMain.GetAllTreeNodes().Where(n => n.IsExpanded && n.Tag != null).Select(n => n.Tag.ToString()).ToList();

                        //see if a current node is selected so we can go back to that spot later
                        if (treeViewMain.SelectedNode != null && treeViewMain.SelectedNode.Tag != null)
                        {
                            lastPath = treeViewMain.SelectedNode.Tag.ToString().ToLower();
                        }

                        _localBaseFilePath = Properties.Settings.Default.DefaultFilesDirectory;
                        _localCurrentFilePath = _localBaseFilePath;
                        splitContainerMain.Panel1Collapsed = false;
                        
                        Utilities.LoadTreeFromPath(treeViewMain, Properties.Settings.Default.DefaultFilesDirectory, null, true, true, InitialTreeStatus.FirstNodeExpanded);

                        if (!String.IsNullOrEmpty(lastPath))
                        {
                            TreeNode nodeToSelect = treeViewMain.GetAllTreeNodes().Where(n => n.Tag.ToString().ToLower() == lastPath).FirstOrDefault();
                            if (nodeToSelect != null)
                            {
                                treeViewMain.SelectedNode = nodeToSelect;
                            }
                        }
                        else
                        {
                            if (treeViewMain.Nodes.Count > 0)
                            {
                                treeViewMain.SelectedNode = treeViewMain.Nodes[0];
                            }
                        }
                        toolStripButtonExplore.Enabled = true;
                        toolStripButtonRefresh.Enabled = true;
                        toolStripButtonDeleteFile.Enabled = true;
                        break;
                    case FileLocations.FlukeSystem:
                        dl = new DirectoryListingInfo();
                        ProgressManager.Start("Loading Fluke System Directory Files...");
                        splitContainerMain.Panel1Collapsed = true;
                        if (FlukeController.IsConnected)
                        {
                            //dl = FlukeController.GetDirectoryListing(_fileLocation.Value);
                            Task.Run(async () => { dl = await FlukeController.GetDirectoryListing(_fileLocation.Value); }).Wait();
                            toolStripButtonRefresh.Enabled = true;
                            toolStripButtonDeleteFile.Enabled = true;
                        }
                        else
                        {
                            dl.ErrorMessage = "NOT CONNECTED";
                            dl.FontBold = true;
                            dl.TextColor = Color.Red;
                            toolStripButtonRefresh.Enabled = false;
                            toolStripButtonDeleteFile.Enabled = false;
                            toolStripButtonFormat.Enabled = false;
                        }
                        break;
                    case FileLocations.FlukeCartridge:
                        dl = new DirectoryListingInfo();
                        ProgressManager.Start("Loading Fluke Cartridge Directory Files...");
                        splitContainerMain.Panel1Collapsed = true;
                        if (FlukeController.IsConnected)
                        {
                            Task.Run(async () => { dl = await FlukeController.GetDirectoryListing(_fileLocation.Value); }).Wait();
                            //dl = FlukeController.GetDirectoryListing(_fileLocation.Value);
                            toolStripButtonRefresh.Enabled = true;
                            toolStripButtonDeleteFile.Enabled = true;
                        }
                        else
                        {
                            dl.ErrorMessage = "NOT CONNECTED";
                            dl.FontBold = true;
                            dl.TextColor = Color.Red;
                            toolStripButtonRefresh.Enabled = false;
                            toolStripButtonDeleteFile.Enabled = false;
                            toolStripButtonFormat.Enabled = false;
                        }
                        break;
                }
            }

            ProgressManager.Stop();

            if (dl != null)
            {
                //okay, now we can update the UI with the results passed back.
                SetDirectoryInformation(dl);
                toolStripLabelReadWriteLabel.Text = "";
                if (_fileLocation == FileLocations.FlukeCartridge)
                {
                    if (FlukeController.IsConnected)
                    {
                        //check to see if there is a leftover test file on the cart
                        //this can happen sometimes and will cause odd behavior
                        if (listViewFiles.Items.Cast<ListViewItem>().Where(i => i.Text == ApplicationGlobals.CARTRIDGE_TEST_FILENAME).FirstOrDefault() != null)
                        {
                            //it may be there, try deleting, just in case
                            Task.Run(async () => { await FlukeController.DeleteFile(ApplicationGlobals.CARTRIDGE_TEST_FILENAME); }).Wait();
                            //FlukeController.DeleteFile(Globals.CARTRIDGE_TEST_FILENAME);
                        }

                        bool? isCartridgeWritable = false;
                        Task.Run(async () => { isCartridgeWritable = await FlukeController.IsCartridgeWritable(); }).Wait();
                        //bool? isCartridgeWritable = FlukeController.IsCartridgeWritable();

                        if (isCartridgeWritable.HasValue)
                        {
                            if (isCartridgeWritable.Value)
                            {
                                toolStripButtonDeleteFile.Enabled = true;
                                toolStripButtonFormat.Enabled = true;
                            }
                            else
                            {
                                toolStripLabelReadWriteLabel.ForeColor = Color.Red;
                                toolStripLabelReadWriteLabel.Text = "WRITE PROTECTED";
                                toolStripButtonDeleteFile.Enabled = false;
                                toolStripButtonFormat.Enabled = false;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void ShowDisconnected()
        {
            DirectoryListingInfo dl = new DirectoryListingInfo();
            dl.ErrorMessage = "NOT CONNECTED";
            dl.FontBold = true;
            dl.TextColor = Color.Red;
            toolStripButtonRefresh.Enabled = false;
            toolStripButtonDeleteFile.Enabled = false;
            toolStripButtonFormat.Enabled = false;
            SetDirectoryInformation(dl);
        }

        private void SetDirectoryInformation(DirectoryListingInfo dl)
        {
            long totalBytesFree = 0;
            long totalBytesUsed = 0;
            if (dl != null)
            {
                listViewFiles.Items.Clear();
                foreach (Tuple<string, string> t in dl.Files)
                {
                    ListViewItem item = new ListViewItem(t.Item1);
                    int fileSize = 0;
                    if (int.TryParse(t.Item2, out fileSize))
                    {
                        item.SubItems.Add(fileSize.ToString("###,##0") + " Bytes");
                    }
                    else
                    {
                        item.SubItems.Add(t.Item2);
                    }
                    if (_fileLocation == FileLocations.LocalComputer)
                    {
                        item.Tag = Path.Combine(dl.Directory, t.Item1);
                    }
                    else
                    {
                        item.Tag = t.Item1;
                    }
                    listViewFiles.Items.Add(item);
                }
                totalBytesFree = dl.BytesFree;
                totalBytesUsed = dl.BytesUsed;
            }
            //summary info here...
            if (!String.IsNullOrEmpty(dl.ErrorMessage))
            {
                toolStripLabelSummary.Text = dl.ErrorMessage;
            }
            else
            {
                toolStripLabelSummary.Text = dl.BytesUsed.ToString("###,###,##0") + " Bytes (" + dl.BytesFree.ToString("###,###,##0") + " Bytes Free)";
            }
            if (dl.FontBold)
            {
                toolStripLabelSummary.Font = new Font(toolStripLabelSummary.Font, FontStyle.Bold);
            }
            toolStripLabelSummary.ForeColor = dl.TextColor;
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            LoadFiles();
        }

        private void toolStripButtonFormat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Formatting the cartridge will erase ALL files, are you sure you want to continue?", "Confirm Format", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dr == DialogResult.Yes)
            {
                if (FlukeController.IsConnected)
                {
                    ClientCommandResponse cr = null;
                    Task.Run(async () => { cr = await FlukeController.FormatCartridge(); }).Wait();
                    //ClientCommandResponse cr = FlukeController.SendCommand(ClientCommands.GetDirectorySystem, null);
                    if (cr.Status == CommandResponseStatus.Success)
                    {
                        LoadFiles();
                        MessageBox.Show("Cartridge Formatted Sucessfully", "Format", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Errors during format: " + cr.ErrorMessage, "Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void listViewFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(FileDragDropInfo)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void listViewFiles_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                FileDragDropInfo info = e.Data.GetData(typeof(FileDragDropInfo)) as FileDragDropInfo;
                if (info != null)
                {
                    CopyFiles(info);
                }
            }
            catch (Exception ex)
            {
                ApplicationGlobals.Exceptions.Add(new AppException(ex));
                MessageBox.Show("There was an error copying the file. See exceptions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void listViewFiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _isDragging = true;
                _draggingStartPoint = new System.Drawing.Point(e.X, e.Y);
            }
        }

        private void toolStripButtonExplore_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeViewMain.SelectedNode;
            if (selectedNode != null && selectedNode.Tag != null)
            {
                Utilities.OpenDirectoryInWindowsExplorer(selectedNode.Tag.ToString());
            }
            else
            {
                Utilities.OpenDirectoryInWindowsExplorer(_localBaseFilePath);
            }
        }

        private void CopyFiles(FileDragDropInfo info)
        {
            //must be connected
            if (!FlukeController.IsConnected)
            {
                MessageBox.Show("You must be connected to perform this action.", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //never ever drag and drop to yourself
            if (_fileLocation != info.Location)
            {

                //we've got files!!
                //lots of combinations
                // PC -> Fluke System
                // PC -> Fluke Cartridge
                // Fluke System -> Fluke Cartridge
                // Fluke System -> PC
                // Fluke Cartridge -> Fluke System
                // Fluke Cartridge -> PC

                int filesCopied = 0;

                ProgressManager.Start("Copying " + info.Files.Count.ToString() + " files...");

                try
                {

                    foreach (string sourceFile in info.Files)
                    {
                        FileLocations sourceLocation = info.Location;
                        FileLocations destLocation = _fileLocation.Value;

                        int result = 0;
                        string pcFile = "";

                        //CART TO PC
                        //SYST TO PC
                        if (destLocation == FileLocations.LocalComputer)
                        {
                            //we are dropping to PC
                            pcFile = Path.Combine(_localCurrentFilePath, FileHelper.GetFilenameOnly(sourceFile)) + ":PC";
                            Task.Run(async () => { result = await FlukeController.TransferFile(FileHelper.AppendLocation(sourceFile, sourceLocation), pcFile); }).Wait();
                            //result = FlukeController.TransferFile(FileHelper.AppendLocation(sourceFile, sourceLocation), pcFile);
                        }
                        //PC TO CART
                        //SYST TO CART
                        else if (destLocation == FileLocations.FlukeCartridge)
                        {
                            if (sourceLocation == FileLocations.LocalComputer)
                            {
                                //PC to CART
                                pcFile = sourceFile + ":PC";
                                Task.Run(async () => { result = await FlukeController.TransferFile(pcFile, FileHelper.GetFilenameOnly(sourceFile) + ":CART"); }).Wait();
                            }
                            else if (sourceLocation == FileLocations.FlukeSystem)
                            {
                                //Two Steps
                                //SYST to PC
                                string tempFile = Path.GetTempFileName() + ":PC";
                                Task.Run(async () => { result = await FlukeController.TransferFile(FileHelper.AppendLocation(sourceFile, sourceLocation), tempFile, false); }).Wait();
                                //PC to CART
                                Task.Run(async () => { result = await FlukeController.TransferFile(tempFile.ToUpper(), FileHelper.GetFilenameOnly(sourceFile) + ":CART"); }).Wait();
                            }
                        }
                        //PC TO SYST
                        //CART TO SYST
                        else if (destLocation == FileLocations.FlukeSystem)
                        {
                            if (sourceLocation == FileLocations.LocalComputer)
                            {
                                //PC to Fluke
                                pcFile = sourceFile + ":PC";
                                Task.Run(async () => { result = await FlukeController.TransferFile(pcFile, FileHelper.GetFilenameOnly(sourceFile) + ":SYST"); }).Wait();
                            }
                            else if (sourceLocation == FileLocations.FlukeCartridge)
                            {
                                //Two Steps
                                //CART to PC
                                string tempFile = Path.GetTempFileName() + ":PC";
                                Task.Run(async () => { result = await FlukeController.TransferFile(FileHelper.AppendLocation(sourceFile, sourceLocation), tempFile, false); });
                                //PC to SYST
                                Task.Run(async () => { result = await FlukeController.TransferFile(tempFile.ToUpper(), FileHelper.GetFilenameOnly(sourceFile) + ":SYST"); });
                            }
                        }

                        if (result == -1)
                        {
                            continue;
                        }
                        else if (result > 0)
                        {
                            filesCopied++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ProgressManager.Stop();
                    ApplicationGlobals.Exceptions.Add(new AppException(ex));
                    MessageBox.Show("There was an error copying the file. See exceptions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                if (filesCopied > 0)
                {
                    LoadFiles();
                    //MessageBox.Show(filesCopied.ToString() + " file(s) sucesfully copied.", "Copy Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ProgressManager.Stop(filesCopied.ToString() + " file(s) copied successfully.");
                }
            }
        }



        private void toolStripButtonDeleteFile_Click(object sender, EventArgs e)
        {
            int filesSelected = listViewFiles.SelectedItems.Count;
            int filesDeleted = 0;

            if (filesSelected > 0)
            {
                DialogResult dr = MessageBox.Show("You have selected " + filesSelected.ToString() + " files for deletion. This action cannot be undone and all data will be lost. Are you sure you want to continue?", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
                ProgressManager.Start("Deleting " + listViewFiles.SelectedItems.Count.ToString() + " file(s)...");
                //good to go... destroy centaur
                foreach (ListViewItem item in listViewFiles.SelectedItems)
                {
                    string fileToDelete = item.Text;
                    if (_fileLocation == FileLocations.LocalComputer)
                    {
                        string currentPath = _localBaseFilePath;
                        if (treeViewMain.SelectedNode != null && treeViewMain.SelectedNode.Tag != null)
                        {
                            currentPath = treeViewMain.SelectedNode.Tag.ToString();
                        }

                        string localPathFilename = Path.Combine(currentPath, fileToDelete);
                        if (File.Exists(localPathFilename))
                        {
                            File.Delete(localPathFilename);
                            filesDeleted++;
                        }
                    }
                    else
                    {
                        //send command
                        bool fileDeleted = false;
                        Task.Run(async () => { fileDeleted = await FlukeController.DeleteFile(FileHelper.AppendLocation(fileToDelete, _fileLocation.Value)); }).Wait();
                        if (fileDeleted)
                        {
                            filesDeleted++;
                        }
                    }
                }
                //ProgressManager.UpdateStatus(filesDeleted.ToString() + " file(s) deleted.");
                LoadFiles();
                ProgressManager.Stop(filesDeleted.ToString() + " file(s) deleted sucessfully.");
            }
            else
            {
                MessageBox.Show("There are no files selected for deletion. Please select one or more files to delete first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void decodeToBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count > 0)
            {
                string fileName = Path.Combine( _localCurrentFilePath, listViewFiles.SelectedItems[0].Text);
                if (File.Exists(fileName))
                {
                    if (!fileName.Contains("@") && !fileName.EndsWith(".NSQ"))
                    {
                        MessageBox.Show("This is not a binary file.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        List<byte> byteList = new List<byte>();
                        string fileString = File.ReadAllText(fileName, Encoding.ASCII);
                        string[] lines = fileString.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string line in lines)
                        {
                            string[] byteStrings = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach(string byteString in byteStrings)
                            {
                                if (byteString != "20" && !String.IsNullOrEmpty(byteString))
                                {
                                    try
                                    {
                                        byte b = (byte)Convert.ToInt32(byteString, 16);
                                        byteList.Add(b);
                                    }
                                    catch { }
                                }

                            }
                        }
                        string targetFilename = fileName + ".bin";
                        File.WriteAllBytes(targetFilename, byteList.ToArray());
                        MessageBox.Show("Binary File '" + targetFilename + "' was created.", "Conversion Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }

            }
        }

        private void listViewFiles_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listViewFiles.SelectedItems.Count == 1 && (listViewFiles.SelectedItems[0].Text.Contains("@") || listViewFiles.SelectedItems[0].Text.EndsWith(".NSQ")))
                {
                    if (listViewFiles.FocusedItem.Bounds.Contains(e.Location) == true)
                    {
                        contextMenuStripLocalComputer.Show(Cursor.Position);
                    }
                }
            }
            //update menu items of things
            //CompilationButton
            toolStripButtonCompile.Enabled = false;
            if (listViewFiles.SelectedItems.Count == 1 &&
                (listViewFiles.SelectedItems[0].Text.EndsWith(".LOC")
                 || listViewFiles.SelectedItems[0].Text.EndsWith(".LIB")
                 || listViewFiles.SelectedItems[0].Text.EndsWith(".SEQ")))
            {
                toolStripButtonCompile.Enabled = true;
            }
        }

        private void treeViewMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeViewMain.SelectedNode != null && treeViewMain.SelectedNode.Tag != null)
            {
                string path = treeViewMain.SelectedNode.Tag.ToString();
                FileAttributes attr = File.GetAttributes(path);
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    //this is a directory
                    _localCurrentFilePath = path;
                    LoadLocalFiles(path);
                }
            }
        }

        private void LoadLocalFiles(string path)
        {
            DirectoryListingInfo dl = new DirectoryListingInfo();
            if (Directory.Exists(path))
            {
                dl.Directory = path;
                dl.BytesFree = new DriveInfo(Path.GetPathRoot(path)).TotalFreeSpace;
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    dl.Files.Add(new Tuple<string, string>(Path.GetFileName(file).Trim(), fi.Length.ToString("###,###,##0")));
                    dl.BytesUsed += fi.Length;
                }
            }
            else
            {
                dl.ErrorMessage = "Directory Not Found";
                dl.TextColor = Color.Red;
                dl.FontBold = true;
            }
            SetDirectoryInformation(dl);
        }

        private void listViewFiles_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }

        private void listViewFiles_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                if (System.Math.Abs(e.X - _draggingStartPoint.X) > 4 ||
                    System.Math.Abs(e.Y - _draggingStartPoint.Y) > 4)
                {
                    StartDragging();
                }
            }
        }

        private void StartDragging()
        {

            FileDragDropInfo di = new FileDragDropInfo();
            di.Files = new List<string>();
            di.Location = _fileLocation.Value;
            foreach (ListViewItem item in listViewFiles.SelectedItems)
            {
                if (_fileLocation == FileLocations.LocalComputer)
                {
                    string currentPath = _localBaseFilePath;
                    if (treeViewMain.SelectedNode != null && treeViewMain.SelectedNode.Tag != null)
                    {
                        currentPath = treeViewMain.SelectedNode.Tag.ToString();
                    }

                    //append full path for local files
                    di.Files.Add(Path.Combine(currentPath, item.Text));
                }
                else
                {
                    di.Files.Add(item.Text);
                }
            }
            DragDropEffects dde = listViewFiles.DoDragDrop(di, DragDropEffects.Copy);
            
        }

        private void listViewFiles_DoubleClick(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count > 0 && _fileLocation == FileLocations.LocalComputer)
            {
                if (listViewFiles.SelectedItems[0].Tag != null)
                {
                    ControlFactory.OpenExistingDocumentInEditor(listViewFiles.SelectedItems[0].Tag.ToString());
                }
            }
        }


        

        private void toolStripButtonCompile_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listViewFiles.SelectedItems)
                {
                    string extension = Path.GetExtension(FileHelper.GetFilenameOnly(item.Text));
                    switch (extension)
                    {
                        case ".SEQ":
                        case ".LOC":
                        case ".LIB":

                            CompilationResult result = null;
                            Task.Run(async () => { result = await FlukeController.CompileFile(FileHelper.AppendLocation(item.Text, _fileLocation.Value)); }).Wait();
                            //CompilationResult result = FlukeController.CompileFile(FileHelper.AppendLocation(item.Text, _fileLocation.Value));

                            if (result.Success)
                            {
                                LoadFiles();
                                //nice

                            }
                            else
                            {
                                MessageBox.Show(result.GetSummary(), "Compilation Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }

                            break;
                        default:
                            MessageBox.Show("Filetype is not supported for compilation: " + item.Text, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("You must select a file to compile.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripButtonToPC_Click(object sender, EventArgs e)
        {
            SendFiles(FileLocations.LocalComputer);
        }

        private void toolStripButtonToSYST_Click(object sender, EventArgs e)
        {
            SendFiles(FileLocations.FlukeSystem);
        }

        private void toolStripButtonToCART_Click(object sender, EventArgs e)
        {
            SendFiles(FileLocations.FlukeCartridge);
        }

        private void SendFiles(FileLocations destinationLocation)
        {

            FileLocations currentLocation = this._fileLocation.Value;

            int filesCopied = 0;

            foreach(ListViewItem item in listViewFiles.SelectedItems)
            {
                string sourceFilename = FileHelper.AppendLocation(Path.Combine(_localCurrentFilePath, item.Text), currentLocation);
                Task.Run(async () => { filesCopied += await FlukeController.TransferFile(sourceFilename, FileHelper.AppendLocation(item.Text, destinationLocation)); }).Wait();
            }

            //Files have been copied now...
            switch (destinationLocation)
            {
                case FileLocations.FlukeCartridge:
                    ControlFactory.ThisIsACrappyMethodToTellADocumentWindowToReloadItsFiles(DockWindowControls.DirectoryFlukeCartridge);
                    break;
                case FileLocations.FlukeSystem:
                    ControlFactory.ThisIsACrappyMethodToTellADocumentWindowToReloadItsFiles(DockWindowControls.DirectoryFlukeSystem);
                    break;
                case FileLocations.LocalComputer:
                    ControlFactory.ThisIsACrappyMethodToTellADocumentWindowToReloadItsFiles(DockWindowControls.DirectoryLocalPC);
                    break;
            }

            MessageBox.Show(filesCopied.ToString() + " file(s) copied sucessfully.", "File Copy", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

       
    }
}
