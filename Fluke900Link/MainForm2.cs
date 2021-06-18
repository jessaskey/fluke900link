using Fluke900.Containers;
using Fluke900.Controllers;
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
        }

        public async Task InitializeAsync()
        {
            ControlFactory.Initialize(this, dockPanelMain,imageList16x16);
            //set up the FlukeController to notify the ControlFactory 
            FlukeController.Initialize(ControlFactory.ConnectionStatusProgress,
                                        ControlFactory.DataStatusProgress,
                                        ControlFactory.DataSendProgress,
                                        ControlFactory.DataReceiveProgress,
                                        ControlFactory.CommandSendProgress,
                                        ControlFactory.CommandResponseProgress);
            //Global UI Elements
            ProgressManager.SetUIComponents2(toolStripStatusLabel, toolStripProgressBar);
            LoadRecentFiles();

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
                await ConnectToFluke();
            }

        }


        private void AddRecentfile(string fileName)
        {
            //get the recent files already here...take them out
            RemoveRecentItem(fileName);
            //put it back, on top tho
            ToolStripItem newItem = new ToolStripMenuItem(fileName);
            recentProjectsToolStripMenuItem.DropDownItems.Insert(0, newItem);
        }

        private void RemoveRecentItem(string fileName)
        {
            for (int i = recentProjectsToolStripMenuItem.DropDownItems.Count - 1; i >= 0; i--)
            {
                ToolStripMenuItem item = recentProjectsToolStripMenuItem.DropDownItems[i] as ToolStripMenuItem;
                if (item != null)
                {
                    if (fileName.ToLower() == item.Text.ToLower())
                    {
                        recentProjectsToolStripMenuItem.DropDownItems.Remove(item);
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
            od.CheckPathExists = true;
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

        private async Task ConnectToFluke()
        {
            ControlFactory.ShowDockWindow(DockWindowControls.TerminalRaw);
            ControlFactory.ShowDockWindow(DockWindowControls.TerminalFormatted);

            //load configuration settings
            FlukeController.SetConnectionProperties(Properties.Settings.Default.COM_Port
                                                    , Properties.Settings.Default.COM_Baud
                                                    , (RJCP.IO.Ports.Parity)Enum.Parse(typeof(RJCP.IO.Ports.Parity), Properties.Settings.Default.COM_Parity)
                                                    , Convert.ToInt16(Properties.Settings.Default.COM_DataBits)
                                                    , (RJCP.IO.Ports.StopBits)Enum.Parse(typeof(RJCP.IO.Ports.StopBits), Properties.Settings.Default.COM_StopBits));

            //always disconnect first in case something went wrong before
            //AsyncHelper.RunSync(FlukeController.Disconnect);
            //Task.Run(async () => { await FlukeController.Disconnect(); }).Wait();
            await FlukeController.Disconnect();
            bool connectSuccess = false;

            toolStripProgressBar.Minimum = 0;
            toolStripProgressBar.Maximum = 5;
            toolStripProgressBar.Value = 0;
            toolStripStatusLabel.Text = "Connecting to Fluke 900...";
            toolStripProgressBar.Increment(1);

            connectSuccess = await FlukeController.Connect();
            //ProgressManager.Start("Connecting to Fluke900...");
            if (connectSuccess)
            {
                if (Properties.Settings.Default.AutoSyncDateTime)
                {
                    //ProgressManager.Start("Checking Fluke Date + Time...");
                    toolStripStatusLabel.Text = "Checking Fluke Date + Time...";
                    toolStripProgressBar.Increment(1);

                    DateTime? flukeDateTime = null;
                    await FlukeController.GetDateTime();

                    if (flukeDateTime.HasValue)
                    {
                        DateTime currentDateTime = DateTime.Now;

                        if (flukeDateTime.Value.Date != currentDateTime.Date || flukeDateTime.Value.Hour != currentDateTime.Hour || flukeDateTime.Value.Minute != currentDateTime.Minute)
                        {
                            //ProgressManager.Start("Updating Fluke DATETIME...");
                            toolStripStatusLabel.Text = "Updating Fluke DATETIME...";
                            toolStripProgressBar.Increment(1);

                            //send over the correct DATE
                            await FlukeController.SetDate(currentDateTime);
                            connectSuccess = await FlukeController.SetTime(currentDateTime);
                            //await FlukeController.SetTime(currentDateTime);
                            //ProgressManager.Stop();
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



                //ProgressManager.Start("Loading directory windows...");
                toolStripStatusLabel.Text = "Loading directory windows...";
                toolStripProgressBar.Increment(1);

                //for now, open up a set of 'Common' windows
                // 1. Raw Terminal - Right
                // 2. Formatted Terminal - Main
                //ProgressManager.Start("Loading Local Files...");
                toolStripStatusLabel.Text = "Loading Local Files...";
                toolStripProgressBar.Increment(1);

                ControlFactory.ShowDockWindow(DockWindowControls.DirectoryLocalPC);
                //ProgressManager.Start("Loading Fluke900 Cartridge Files...");
                toolStripStatusLabel.Text = "Loading Fluke900 Cartridge Files...";
                toolStripProgressBar.Increment(1);

                ControlFactory.ShowDockWindow(DockWindowControls.DirectoryFlukeCartridge);
                //ProgressManager.Start("Loading Fluke900 System Files...");
                toolStripStatusLabel.Text = "Loading Fluke900 System Files...";
                toolStripProgressBar.Increment(1);

                ControlFactory.ShowDockWindow(DockWindowControls.DirectoryFlukeSystem);

                //these are loaded above during the CreateDirectoryWindowCall
                DirectoryEditorControl directoryFlukeCartridge = ControlFactory.GetControl(DockWindowControls.DirectoryFlukeCartridge) as DirectoryEditorControl;
                if (directoryFlukeCartridge != null)
                {
                    //ProgressManager.Start("Loading Fluke900 Cartridge Files...");
                    toolStripStatusLabel.Text = "Loading Fluke900 Cartridge Files...";
                    toolStripProgressBar.Increment(1);
                    directoryFlukeCartridge.LoadFiles();
                }

                DirectoryEditorControl directoryFlukeSystem = ControlFactory.GetControl(DockWindowControls.DirectoryFlukeSystem) as DirectoryEditorControl;
                if (directoryFlukeSystem != null)
                {
                    //ProgressManager.Start("Loading Fluke900 System Files...");
                    toolStripStatusLabel.Text = "Loading Fluke900 System Files...";
                    toolStripProgressBar.Increment(1);

                    directoryFlukeSystem.LoadFiles();
                }

                //ProgressManager.Stop("Connected!");
                toolStripStatusLabel.Text = "Connected: Ready";
                toolStripProgressBar.Value = 0;
            }
            else
            {
                //ProgressManager.Stop("There were problems...");
                toolStripStatusLabel.Text = "Not Connected";
                toolStripProgressBar.Value = 0;

                MessageBox.Show("There was a problem connecting or communicating to the Fluke, check your connections and port settings, verify you have a good NULL MODEM cable.", "COM Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void LoadRecentFiles()
        {
            recentProjectsToolStripMenuItem.DropDownItems.Clear();

            string[] files = Properties.Settings.Default.RecentFileList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string file in files)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(file);
                item.Click += recentItem_Click;
                recentProjectsToolStripMenuItem.DropDownItems.Add(item);
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

        private async void MainForm2_FormClosing(object sender, FormClosingEventArgs e)
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
                await Disconnect(true);
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
            Properties.Settings.Default.RecentFileList = String.Join(";", recentProjectsToolStripMenuItem.DropDownItems.Cast<ToolStripMenuItem>().Select(i => i.Text).ToArray());
            Properties.Settings.Default.Save();
        }

        private async Task<bool> Disconnect(bool sendDisconnectCommand)
        {
            this.Invoke((MethodInvoker)delegate
            {
                toolStripStatusLabel.Text = "Disconnecting...";
                toolStripProgressBar.Increment(1);
            });
            if (sendDisconnectCommand)
            {
                await FlukeController.Disconnect();
            }

            //ProgressManager.Stop("Disconnected");
            this.Invoke((MethodInvoker)delegate
            {
                toolStripStatusLabel.Text = "Disconnected";
                toolStripProgressBar.Increment(0);
            });
            toolStripButtonDisconnect.Enabled = false;
            disconnectToolStripMenuItem.Enabled = false;

            toolStripButtonConnect.Enabled = true;
            connectToolStripMenuItem.Enabled = true;

            toolStripButtonResetSoft.Enabled = false;
            softResetToolStripMenuItem.Enabled = false;

            toolStripButtonResetFull.Enabled = false;
            hardResetToolStripMenuItem.Enabled = false;
            return true;
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

        private void toolStripButtonProjectOpen_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private async void toolStripButtonConnect_Click(object sender, EventArgs e)
        {
            await ConnectToFluke();
        }

        private async void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ConnectToFluke();
        }

        private async void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Disconnect(true);
        }

        private async void toolStripButtonDisconnect_Click(object sender, EventArgs e)
        {
            await Disconnect(true);
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            ConfigurationDialog cd = new ConfigurationDialog();
            DialogResult dr = cd.ShowDialog();
        }

        //private void toolStripButtonFileNewLib_Click(object sender, EventArgs e)
        //{
        //    ControlFactory.OpenNewDocumentInEditor(".lib");
        //}

        private void createNewProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewProject();
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectFactory.SaveProject();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.InitialDirectory = Utilities.GetBrowseDirectory();
            sd.Filter = "Fluke Project File (*.f9p)|*.f9p";
            sd.CheckPathExists = true;
            DialogResult dr = sd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                ProjectFactory.SaveProjectAs(sd.FileName);
            }
        }

        private async void toolStripButtonLibraryTools_Click(object sender, EventArgs e)
        {
            //opens the dialog for parsing the binary library files
            LibraryParserDialog lp = new LibraryParserDialog();
            await lp.Initialize();
            lp.ShowDialog();
        }
    }
}
