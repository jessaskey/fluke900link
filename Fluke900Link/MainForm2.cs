using Fluke900.Containers;
using Fluke900Link.Containers;
using Fluke900Link.Controllers;
using Fluke900Link.Controls;
using Fluke900Link.Dialogs;
using Fluke900Link.Factories;
using Fluke900Link.Helpers;
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
using WeifenLuo.WinFormsUI.Docking;

namespace Fluke900Link
{
    public partial class MainForm2 : Form
    {
        public string[] OpenArgs = null;
        Splash _splash = null;

        public MainForm2()
        {
            InitializeComponent();
            ControlFactory.Initialize(dockPanelMain);

            //set up the FlukeController to notify the ControlFactory 
            FlukeController.SetConnectionStatusProgress(ControlFactory.ConnectionStatusProgress);
            FlukeController.SetDataStatusProgress(ControlFactory.DataStatusProgress);
            FlukeController.SetDataSendProgress(ControlFactory.DataSendProgress);
            FlukeController.SetDataReceiveProgress(ControlFactory.DataReceiveProgress);

            //FlukeController. += new ConnectionStatusChanged(ConnectionStatusChanged);
            //FlukeController.OnDataStatusChanged += new SerialDataStatusChanged(DataStatusChanged);

            //Global UI Elements
            ProgressManager.SetUIComponents2(toolStripStatusLabel, toolStripProgressBar);

            ControlFactory.MainForm2 = this;
            ControlFactory.ImageList16x16 = imageList16x16;

            LoadRecentFiles();
        }

        private void MainForm2_Load(object sender, EventArgs e)
        {
            _splash = new Splash();
            _splash.HideButtons = true;
            timerSplash.Enabled = true;
            _splash.Show();
        }

        private void timerSplash_Tick(object sender, EventArgs e)
        {
            timerSplash.Enabled = false;
            if (_splash != null)
            {
                _splash.Close();
                _splash = null;
            }

#if !DEBUG
            if (String.IsNullOrEmpty(Properties.Settings.Default.DefaultFilesDirectory))
            {

                //the default file director is not defined... this has to be done right away as
                //many things will depend on it.
                DialogResult dr = MessageBox.Show("Fluke900Link needs to copy several files relating to documentation, examples and templates into your user specific workspace. By default, this will be put in your MyDocuments folder and called 'Fluke900Files'. Would you like to create this folder now?", "Default Files Location", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    //create default
                    string defaultDirectoryPath = Utilities.GetDefaultDirectoryPath();
                    Directory.CreateDirectory(defaultDirectoryPath);
                    Properties.Settings.Default.DefaultFilesDirectory = defaultDirectoryPath;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    MessageBox.Show("No files will be copied into the working directory. In order to set up the Working directory, please go into the Configuration Dialog, select the default file location and hit OK. The template files will automatically be copied to that folder on the next launch of Fluke900Link", "File Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
#endif

            ClientCommandFactory.Initialize();

            //copy into user templates if they do not exist
            if (Directory.Exists(Properties.Settings.Default.DefaultFilesDirectory))
            {
                if (Properties.Settings.Default.AutoCopyTemplates)
                {
                    string userTemplateFolder = Path.Combine(Properties.Settings.Default.DefaultFilesDirectory, ApplicationGlobals.TEMPLATES_FOLDER);
                    AutoCopyDirectory(Path.Combine(Utilities.GetExecutablePath(), "Templates"), userTemplateFolder, false);
                }
                if (Properties.Settings.Default.AutoCopyDocuments)
                {
                    string userDocumentsFolder = Path.Combine(Properties.Settings.Default.DefaultFilesDirectory, ApplicationGlobals.DOCUMENTS_FOLDER);
                    AutoCopyDirectory(Path.Combine(Utilities.GetExecutablePath(), "Documents"), userDocumentsFolder, true);
                }
                if (Properties.Settings.Default.AutoCopyExamples)
                {
                    string userExamplesFolder = Path.Combine(Properties.Settings.Default.DefaultFilesDirectory, ApplicationGlobals.EXAMPLES_FOLDER);
                    AutoCopyDirectory(Path.Combine(Utilities.GetExecutablePath(), "Examples"), userExamplesFolder, true);
                }
            }

            //dont reload the window layout if the user has the shift key held down, this is 
            //convienient for corrupted layout files.
            if (!(Control.ModifierKeys == Keys.Shift))
            {
                ControlFactory.LoadSavedDockConfiguration(Path.Combine(Utilities.GetExecutablePath(), ApplicationGlobals.DOCK_CONFIGURATION_FILE));
            }

            //check for passed args
            if (OpenArgs != null)
            {
                foreach (string arg in OpenArgs)
                {
                    //MessageBox.Show(arg, "Open");
                    if (File.Exists(arg))
                    {
                        //MessageBox.Show(arg, "Exists");
                        if (arg.ToLower().EndsWith(".f9p"))
                        {
                            //MessageBox.Show(arg, "Project");
                            OpenProjectFile(arg);
                        }
                        else
                        {
                            ControlFactory.OpenExistingDocumentInEditor(arg);
                        }
                    }
                }
            }

            //Do we always do it?
            if (Properties.Settings.Default.AutoConnect)
            {
                ConnectToFluke();
            }

            ProgressManager.Start("Loading Device Libraries...");
            Task.Run(() =>
            {
                LibraryHelper.LoadReferenceLibrary();
            }).Wait();
            ProgressManager.Stop();
        }

        private void AddRecentfile(string fileName)
        {
            //get the recent files already here...take them out
            RemoveRecentItem(fileName);
            //put it back, on top tho
            ToolStripItem newItem = new ToolStripMenuItem(fileName);
            recentFilesToolStripMenuItem.DropDownItems.Insert(0, newItem);
        }

        private void RemoveRecentItem(string fileName)
        {
            for (int i = recentFilesToolStripMenuItem.DropDownItems.Count - 1; i >= 0; i--)
            {
                ToolStripMenuItem item = recentFilesToolStripMenuItem.DropDownItems[i] as ToolStripMenuItem;
                if (item != null)
                {
                    if (fileName.ToLower() == item.Text.ToLower())
                    {
                        recentFilesToolStripMenuItem.DropDownItems.Remove(item);
                    }
                }
            }
        }

        private void OpenProject()
        {
            OpenFileDialog od = new OpenFileDialog();
            od.InitialDirectory = Utilities.GetBrowseDirectory();
            od.Filter = "Fluke Project File (*.f9p)|*.f9p";
            od.CheckFileExists = true;
            od.CheckFileExists = true;
            od.Multiselect = false;
            DialogResult dr = od.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                string projectFile = od.FileName;
                if (File.Exists(projectFile))
                {
                    OpenProjectFile(projectFile);
                }
                else
                {
                    MessageBox.Show("Project file was not found or could not be opened.", "Project Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void OpenProjectFile(string projectFile)
        {
            Project project = ProjectFactory.LoadProject(projectFile);
            if (project != null)
            {
                ProjectFactory.CurrentProject = project;
                ProjectFactory.CurrentProject.IsModified = false;
                SolutionExplorer se = ControlFactory.ShowDockWindow(DockWindowControls.SolutionExplorer) as SolutionExplorer;
                se.LoadProject(project);
                AddRecentfile(project.ProjectPathFile);
            }
            else
            {
                MessageBox.Show("Project could not be loaded. Check exception log for details.", "Project Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ConnectToFluke()
        {

            //load configuration settings
            FlukeController.SetConnectionProperties(Properties.Settings.Default.COM_Port
                                                    , Properties.Settings.Default.COM_Baud
                                                    , (RJCP.IO.Ports.Parity)Enum.Parse(typeof(RJCP.IO.Ports.Parity), Properties.Settings.Default.COM_Parity)
                                                    , Convert.ToInt16(Properties.Settings.Default.COM_DataBits)
                                                    , (RJCP.IO.Ports.StopBits)Enum.Parse(typeof(RJCP.IO.Ports.StopBits), Properties.Settings.Default.COM_StopBits));

            //always disconnect first in case something went wrong before
            //AsyncHelper.RunSync(FlukeController.Disconnect);
            Task.Run(async () => { await FlukeController.Disconnect(); }).Wait();


            bool connectSuccess = false;
            //AsyncHelper.RunSync(FlukeController.Connect);
            Task.Run(async () => { connectSuccess = await FlukeController.Connect(); }).Wait();

            ProgressManager.Start("Connecting to Fluke900...");
            if (connectSuccess)
            {
                if (Properties.Settings.Default.AutoSyncDateTime)
                {
                    ProgressManager.Start("Checking Fluke Date + Time...");
                    DateTime? flukeDateTime = null;
                    Task.Run(async () => { flukeDateTime = await FlukeController.GetDateTime(); }).Wait();

                    if (flukeDateTime.HasValue)
                    {
                        DateTime currentDateTime = DateTime.Now;

                        if (flukeDateTime.Value.Date != currentDateTime.Date || flukeDateTime.Value.Hour != currentDateTime.Hour || flukeDateTime.Value.Minute != currentDateTime.Minute)
                        {
                            ProgressManager.Start("Updating Fluke DATETIME...");
                            //send over the correct DATE
                            Task.Run(async () => { connectSuccess = await FlukeController.SetDate(currentDateTime); }).Wait();
                            Task.Run(async () => { connectSuccess = await FlukeController.SetTime(currentDateTime); }).Wait();
                            //await FlukeController.SetTime(currentDateTime);
                            ProgressManager.Stop();
                        }
                    }
                }
                //textBox1.Text = cr.ResultAsString;
                toolStripButtonDisconnect.Enabled = true;
                disconnectToolStripMenuItem.Enabled = true;

                toolStripButtonConnect.Enabled = false;
                connectToolStripMenuItem.Enabled = false;

                toolStripButtonResetSoft.Enabled = true;
                softResetToolStripMenuItem.Enabled = true;

                toolStripButtonResetFull.Enabled = true;
                hardResetToolStripMenuItem.Enabled = true;

                ControlFactory.ShowDockWindow(DockWindowControls.TerminalRaw);
                ControlFactory.ShowDockWindow(DockWindowControls.TerminalFormatted);

                ProgressManager.Start("Loading directory windows...");
                //for now, open up a set of 'Common' windows
                // 1. Raw Terminal - Right
                // 2. Formatted Terminal - Main
                ProgressManager.Start("Loading Local Files...");
                ControlFactory.ShowDockWindow(DockWindowControls.DirectoryLocalPC);
                ProgressManager.Start("Loading Fluke900 Cartridge Files...");
                ControlFactory.ShowDockWindow(DockWindowControls.DirectoryFlukeCartridge);
                ProgressManager.Start("Loading Fluke900 System Files...");
                ControlFactory.ShowDockWindow(DockWindowControls.DirectoryFlukeSystem);

                //these are loaded above during the CreateDirectoryWindowCall
                //if (ControlFactory.UIElements.DirectoryEditorCartridge != null)
                //{
                //    ProgressManager.Start("Loading Fluke900 Cartridge Files...");
                //    ControlFactory.UIElements.DirectoryEditorCartridge.LoadFiles();
                //}

                //if (ControlFactory.UIElements.DirectoryEditorSystem != null)
                //{
                //    ProgressManager.Start("Loading Fluke900 System Files...");
                //    ControlFactory.UIElements.DirectoryEditorSystem.LoadFiles();
                //}

                ProgressManager.Stop("Connected!");
            }
            else
            {
                ProgressManager.Stop("There were problems...");
                MessageBox.Show("There was a problem connecting or communicating to the Fluke, check your connections and port settings, verify you have a good NULL MODEM cable.", "COM Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void LoadRecentFiles()
        {
            recentFilesToolStripMenuItem.DropDownItems.Clear();

            string[] files = Properties.Settings.Default.RecentFileList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string file in files)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(file);
                item.Click += recentItem_Click;
                recentFilesToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private void recentItem_Click(object sender, EventArgs e)
        {
            //a recent item was checked here... open it 
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item != null)
            {
                if (File.Exists(item.Text))
                {
                    switch (Path.GetExtension(item.Text).ToLower())
                    {
                        case ".f9p":
                            Project project = ProjectFactory.LoadProject(item.Text);
                            if (project != null)
                            {
                                ProjectFactory.CurrentProject = project;
                                ProjectFactory.CurrentProject.IsModified = true;
                                SolutionExplorer se = ControlFactory.ShowDockWindow(DockWindowControls.SolutionExplorer) as SolutionExplorer;
                                se.LoadProject(project);
                                AddRecentfile(project.ProjectPathFile);
                            }
                            break;
                    }
                }
                else
                {
                    DialogResult dr = MessageBox.Show("File no longer exists in the location or cannot be opened. Would you like to remove this from the Recent File list?", "Missing File", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        RemoveRecentItem(item.Text);
                    }
                }
            }
        }

        private void AutoCopyDirectory(string sourceDirectory, string destinationDirectory, bool overwriteAlways)
        {
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            if (Directory.Exists(destinationDirectory))
            {
                try
                {
                    if (Directory.Exists(sourceDirectory))
                    {
                        //delete any other files in the destination directory just to clean up old files
                        foreach (string existingFile in Directory.GetFiles(destinationDirectory))
                        {
                            if (overwriteAlways)
                            {
                                try
                                {
                                    File.Delete(existingFile);
                                }
                                catch (Exception ex)
                                {
                                    ApplicationGlobals.Exceptions.Add(new AppException(ex));
                                }
                            }
                        }
                        string[] templateFiles = Directory.GetFiles(sourceDirectory);
                        foreach (string templateFile in templateFiles)
                        {
                            try
                            {
                                string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(templateFile));
                                if (overwriteAlways)
                                {
                                    if (File.Exists(destinationFile))
                                    {
                                        File.Delete(destinationFile);
                                    }
                                }
                                if (!File.Exists(destinationFile))
                                {
                                    File.Copy(templateFile, destinationFile);
                                }
                            }
                            catch (Exception ex)
                            {
                                ApplicationGlobals.Exceptions.Add(new AppException(ex));
                            }
                        }
                        //subfolders
                        foreach (string directory in Directory.GetDirectories(sourceDirectory))
                        {
                            AutoCopyDirectory(directory, destinationDirectory + directory.Replace(sourceDirectory, ""), overwriteAlways);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ApplicationGlobals.Exceptions.Add(new AppException(ex));
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MainForm2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MainForm2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Focus();

            if (dockPanelMain != null && dockPanelMain.Documents != null)
            {
                foreach (IDockContent content in dockPanelMain.Documents)
                {
                    DocumentEditor editor = content as DocumentEditor;
                    if (editor != null)
                    {
                        if (editor.IsModified)
                        {
                            DialogResult dr = MessageBox.Show("The file '" + editor.Filename + "' has modifications. Do you want to save this document before closing?", "Document Modified", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (dr == DialogResult.Cancel)
                            {
                                e.Cancel = true;
                                return;
                            }
                            if (dr == System.Windows.Forms.DialogResult.No)
                            {
                                //TODO: Fix this?
                                //dockPanelMain.DockHandler.RemoveWindow(content as DockWindow, DockWindowCloseAction.CloseAndDispose);
                                content.DockHandler.Close();
                                continue;
                            }
                            if (dr == System.Windows.Forms.DialogResult.Yes)
                            {
                                editor.SaveDocument();
                            }
                        }
                    }
                }
            }

            if (ProjectFactory.CurrentProject != null && ProjectFactory.CurrentProject.IsModified)
            {
                DialogResult dr = MessageBox.Show("The current project is not saved. Would you like to save changes before closing?", "Save Project", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    ProjectFactory.SaveProject();
                }
            }

            //Try you will not
            if (FlukeController.IsConnected)
            {
                Disconnect(true);
            }

            dockPanelMain.Refresh();
            dockPanelMain.Update();
            ControlFactory.SaveDockConfiguration();
            SaveRecentFiles();
        }

        /// <summary>
        /// Saves the current recent files Menu Items into the application settings
        /// </summary>
        private void SaveRecentFiles()
        {
            Properties.Settings.Default.RecentFileList = String.Join(";", recentFilesToolStripMenuItem.DropDownItems.Cast<ToolStripMenuItem>().Select(i => i.Text).ToArray());
            Properties.Settings.Default.Save();
        }

        private void Disconnect(bool sendDisconnectCommand)
        {
            ProgressManager.Start("Disconnecting...");
            if (sendDisconnectCommand)
            {
                Task.Run(async () => { await FlukeController.Disconnect(); }).Wait();
            }

            ProgressManager.Stop("Disconnected");
            toolStripButtonDisconnect.Enabled = false;
            disconnectToolStripMenuItem.Enabled = false;

            toolStripButtonConnect.Enabled = true;
            connectToolStripMenuItem.Enabled = true;

            toolStripButtonResetSoft.Enabled = false;
            softResetToolStripMenuItem.Enabled = false;

            toolStripButtonResetFull.Enabled = false;
            hardResetToolStripMenuItem.Enabled = false;
        }

        private void toolStripButtonProjectCreate_Click(object sender, EventArgs e)
        {
            CreateNewProject();
        }

        private void CreateNewProject()
        {
            NewProjectDialog pd = new NewProjectDialog();
            DialogResult dr = pd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                Project p = pd.NewProject;
                p.IsModified = true;
                ProjectFactory.CurrentProject = p;

                SolutionExplorer se = ControlFactory.ShowDockWindow(DockWindowControls.SolutionExplorer) as SolutionExplorer;
                se.LoadProject(p);
                AddRecentfile(p.ProjectPathFile);
            }
        }

        private void localComputerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.DirectoryLocalPC);
        }

        private void flukeCartridgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.DirectoryFlukeCartridge);
        }

        private void flukeSystemDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.DirectoryFlukeSystem);
        }



        private void terminalLogRawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.TerminalRaw);
        }

        private void terminalLogFormattedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.TerminalFormatted);
        }

        private void terminalSendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.TerminalSend);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private void toolStripButtonProjectOpen_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        { 

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonConnect_Click(object sender, EventArgs e)
        {
            ConnectToFluke();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectToFluke();
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Disconnect(true);
        }

        private void toolStripButtonDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect(true);
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            ConfigurationDialog cd = new ConfigurationDialog();
            DialogResult dr = cd.ShowDialog();
        }

        private void toolStripButtonFileNewLib_Click(object sender, EventArgs e)
        {
            ControlFactory.OpenNewDocumentInEditor(".lib");
        }


        private void toolStripMenuItemImport_Click(object sender, EventArgs e)
        {
            ImportZSQDialog zd = new ImportZSQDialog();
            zd.ShowDialog();
        }
    }
}
