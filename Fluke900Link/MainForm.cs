using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Containers;
using Fluke900Link.Controllers;
using Fluke900Link.Controls;
using Fluke900Link.Dialogs;
using Fluke900Link.Factories;
using Fluke900Link.Helpers;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace Fluke900Link
{
    public partial class MainForm : Form
    {
        public string[] OpenArgs = null;
        Splash _splash = null;

        public MainForm()
        {
            InitializeComponent();

            ControlFactory.Initialize(radDockMain);

            //set up the FlukeController to notify the ControlFactory 
            FlukeController.SetConnectionStatusProgress(ControlFactory.ConnectionStatusProgress);
            FlukeController.SetDataStatusProgress(ControlFactory.DataStatusProgress);
            FlukeController.SetDataSendProgress(ControlFactory.DataSendProgress);
            FlukeController.SetDataReceiveProgress(ControlFactory.DataReceiveProgress);

            //FlukeController. += new ConnectionStatusChanged(ConnectionStatusChanged);
            //FlukeController.OnDataStatusChanged += new SerialDataStatusChanged(DataStatusChanged);

            //Global UI Elements
            ProgressManager.SetUIComponents(radLabelElementStatus, radWaitingBarElement1);

            ControlFactory.MainForm = this;
            ControlFactory.ImageList16x16 = imageList16x16;

            LoadRecentFiles();
        }

        private void MainForm_Load(object sender, EventArgs e)
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

            RemoteCommandFactory.Initialize();

            //copy into user templates if they do not exist
            if (Directory.Exists(Properties.Settings.Default.DefaultFilesDirectory))
            {
                if (Properties.Settings.Default.AutoCopyTemplates)
                {
                    string userTemplateFolder = Path.Combine(Properties.Settings.Default.DefaultFilesDirectory, Globals.TEMPLATES_FOLDER);
                    AutoCopyDirectory(Path.Combine(Utilities.GetExecutablePath(), "Templates"), userTemplateFolder, false);
                }
                if (Properties.Settings.Default.AutoCopyDocuments)
                {
                    string userDocumentsFolder = Path.Combine(Properties.Settings.Default.DefaultFilesDirectory, Globals.DOCUMENTS_FOLDER);
                    AutoCopyDirectory(Path.Combine(Utilities.GetExecutablePath(), "Documents"), userDocumentsFolder, true);
                }
                if (Properties.Settings.Default.AutoCopyExamples)
                {
                    string userExamplesFolder = Path.Combine(Properties.Settings.Default.DefaultFilesDirectory, Globals.EXAMPLES_FOLDER);
                    AutoCopyDirectory(Path.Combine(Utilities.GetExecutablePath(), "Examples"), userExamplesFolder, true);
                }
            }

            //dont reload the window layout if the user has the shift key held down, this is 
            //convienient for corrupted layout files.
            if (!(Control.ModifierKeys == Keys.Shift))
            {
                ControlFactory.LoadSavedDockConfiguration(Path.Combine(Utilities.GetExecutablePath(), Globals.DOCK_CONFIGURATION_FILE));
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

        public void DataStatusChanged(bool sending, bool receiving)
        {
            toolStripButtonSendingData.Enabled = sending;
            toolStripButtonReceivingData.Enabled = receiving;
        }

        private void AutoCopyDirectory (string sourceDirectory, string destinationDirectory, bool overwriteAlways)
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
                                    Globals.Exceptions.Add(new AppException(ex));
                                }
                            }
                        }
                        string[] templateFiles = Directory.GetFiles(sourceDirectory);
                        foreach (string templateFile in templateFiles)
                        {
                            try{
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
                                Globals.Exceptions.Add(new AppException(ex));
                            }
                        }
                        //subfolders
                        foreach(string directory in Directory.GetDirectories(sourceDirectory))
                        {
                            AutoCopyDirectory(directory, destinationDirectory + directory.Replace(sourceDirectory, ""), overwriteAlways);                  
                        }
                    }
                }
                catch (Exception ex)
                {
                    Globals.Exceptions.Add(new AppException(ex));
                }
            }
        }

        private void ConnectionStatusChanged(EventArgs e, ConnectionStatus previousStatus, ConnectionStatus currentStatus)
        {
            if (previousStatus == ConnectionStatus.Unknown && currentStatus == ConnectionStatus.Connected)
            {
               
            }
        }

        private void toolStripButtonConnect_Click(object sender, EventArgs e)
        {
            ConnectToFluke();
        }

        private void ConnectToFluke()
        {

            //load configuration settings
            FlukeController.SetConnectionProperties( Properties.Settings.Default.COM_Port
                                                    ,Properties.Settings.Default.COM_Baud
                                                    ,(RJCP.IO.Ports.Parity)Enum.Parse(typeof(RJCP.IO.Ports.Parity), Properties.Settings.Default.COM_Parity)
                                                    ,Convert.ToInt16(Properties.Settings.Default.COM_DataBits)
                                                    ,(RJCP.IO.Ports.StopBits)Enum.Parse(typeof(RJCP.IO.Ports.StopBits), Properties.Settings.Default.COM_StopBits));

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
                radMenuItemFlukeDisconnect.Enabled = true;

                toolStripButtonConnect.Enabled = false;
                radMenuItemFlukeConnect.Enabled = false;

                toolStripButtonResetSoft.Enabled = true;
                radMenuItemSoftReset.Enabled = true;

                toolStripButtonResetFull.Enabled = true;
                radMenuItemHardReset.Enabled = true;

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

        //private async Task ConnectAndInit()
        //{
        //    {
                //if (FlukeController.Connect())
                //{
                //    RunWorkerCompletedEventHandler myAction = (RunWorkerCompletedEventHandler)((sender, e) =>
                //    {
                //    });
                //    RunWorkerCompletedEventHandler rwc = delegate(object sender, RunWorkerCompletedEventArgs e)
                //    {
                //        BackgroundWorker worker = sender as BackgroundWorker;
                //        RemoteCommandResponse cr = e.Result as RemoteCommandResponse;
                //        if (cr != null && cr.Status == CommandResponseStatus.Success)
                //        {
                //            worker.ReportProgress(0, "Checking Fluke Date + Time...");

                //            RunWorkerCompletedEventHandler rwc2 = delegate(object sender2, RunWorkerCompletedEventArgs e2)
                //            {
                //                BackgroundWorker worker2 = sender2 as BackgroundWorker;
                //                RemoteCommandResponse cr2 = e2.Result as RemoteCommandResponse;
                //                if (cr2 != null && cr2.Status == CommandResponseStatus.Success)
                //                {
                //                    if (Properties.Settings.Default.AutoSyncDateTime)
                //                    {
                //                        if (cr2 != null)
                //                        {
                //                            DateTime currentDateTime = DateTime.Now;
                //                            string rawResultString = Encoding.ASCII.GetString(cr.RawBytes, 1, (cr.RawBytes.Length - 2));
                //                            string[] resultParts = rawResultString.Split('\r');
                //                            string flukeDateTimeString = resultParts[1] + " " + resultParts[0];
                //                            //write the current date time if it is different from the current computer time
                //                            IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

                //                            DateTime flukeDateTime = DateTime.Parse(flukeDateTimeString, culture);
                //                            if (flukeDateTime.Date != currentDateTime.Date || flukeDateTime.Hour != currentDateTime.Hour || flukeDateTime.Minute != currentDateTime.Minute)
                //                            {
                //                                worker2.ReportProgress(0, "Updating Fluke DATETIME...");
                //                                //send over the correct DATE
                //                                FlukeController.SyncDateToUnit();
                //                                FlukeController.SyncTimeToUnit();
                //                                ProgressManager.Stop();
                //                            }
                //                        }
                //                        else
                //                        {
                //                            MessageBox.Show("Date/Time could not be loaded from Fluke, will try to AutoSync next time.", "Sync Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //                        }
                //                    }
                //                }
                //            };
                //            WorkerFactory.Fluke900DoWork(ProgressManager.ProgressChanged, rwc2, new RemoteCommand(RemoteCommandCodes.GetDateTime, null));
                //        }
                //    };
                //    WorkerFactory.Fluke900DoWork(ProgressManager.ProgressChanged, rwc, new RemoteCommand(RemoteCommandCodes.Identify, null));
                //}
          //  }
        //}


        private void Disconnect(bool sendDisconnectCommand)
        {
            ProgressManager.Start("Disconnecting...");
            if (sendDisconnectCommand)
            {
                Task.Run(async () => { await FlukeController.Disconnect(); }).Wait();
            }

            ProgressManager.Stop("Disconnected");
            toolStripButtonDisconnect.Enabled = false;
            radMenuItemFlukeDisconnect.Enabled = false;

            toolStripButtonConnect.Enabled = true;
            radMenuItemFlukeConnect.Enabled = true;

            toolStripButtonResetSoft.Enabled = false;
            radMenuItemSoftReset.Enabled = false;

            toolStripButtonResetFull.Enabled = false;
            radMenuItemHardReset.Enabled = false;
        }

        private void toolStripButtonDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect(true);
        }

        private void toolStripButtonResetSoft_Click(object sender, EventArgs e)
        {
            SoftReset();
        }

        private void radMenuItemSoftReset_Click(object sender, EventArgs e)
        {
            SoftReset();
        }

        private void radMenuItemHardReset_Click(object sender, EventArgs e)
        {
            HardReset();
        }

        private void toolStripButtonResetFull_Click(object sender, EventArgs e)
        {
            HardReset();
        }

        private void SoftReset()
        {
            if (!FlukeController.IsConnected) return;

            Task.Run(async () => { await FlukeController.SoftReset(); }).Wait();
            Disconnect(false);
        }

        private void HardReset()
        {
            if (!FlukeController.IsConnected) return;

            Task.Run(async () => { await FlukeController.HardReset(); }).Wait();
            Disconnect(false);
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Focus();

            if (radDockMain != null && radDockMain.DocumentManager != null && radDockMain.DocumentManager.DocumentArray != null)
            {
                foreach (DockWindow control in radDockMain.DocumentManager.DocumentArray)
                {
                    DocumentEditor editor = control as DocumentEditor;
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
                                radDockMain.RemoveWindow(control as DockWindow, DockWindowCloseAction.CloseAndDispose);
                                control.Close();
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

            radDockMain.Refresh();
            radDockMain.Update();
            ControlFactory.SaveDockConfiguration();
            SaveRecentFiles();
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            ConfigurationDialog cd = new ConfigurationDialog();
            DialogResult dr = cd.ShowDialog();

        }

        private void radMenuItem_DirectoryLocal_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.DirectoryLocalPC);
        }

        private void radMenuItem_DirectoryCartridge_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.DirectoryFlukeCartridge);
        }

        private void radMenuItem_DirectorySystem_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.DirectoryFlukeSystem);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void radMenuItem_DirectoryOpenAll_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.DirectoryLocalPC);
            ControlFactory.ShowDockWindow(DockWindowControls.DirectoryFlukeSystem);
            ControlFactory.ShowDockWindow(DockWindowControls.DirectoryFlukeCartridge);
        }

        private void radMenuItem_DirectoryCloseAll_Click(object sender, EventArgs e)
        {
            ControlFactory.RemoveDockWindow(DockWindowControls.DirectoryLocalPC);
            ControlFactory.RemoveDockWindow(DockWindowControls.DirectoryFlukeSystem);
            ControlFactory.RemoveDockWindow(DockWindowControls.DirectoryFlukeCartridge);
        }

        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {
            OpenDocumentation();
        }

        private void radMenuItemExceptions_Click(object sender, EventArgs e)
        {
            ShowExceptionDialog sd = new ShowExceptionDialog();
            sd.ShowDialog();
        }

        private void radMenuItemAbout_Click(object sender, EventArgs e)
        {
            Splash splash = new Splash();
            splash.HideButtons = false;
            splash.ShowDialog();
        }

        // this was an old test that probed the serial port to see which commands were valid or not
        //private void RunExtractTest()
        //{
        //    string outputDirectory = @"C:\Fluke900Test";
        //    for(byte i = 0x41; i < 0x5b; i++)
        //    {
        //        for (byte j = 0x41; j < 0x5b; j++)
        //        {
        //            string c1 = Encoding.ASCII.GetString(new byte[] { i });
        //            string c2 = Encoding.ASCII.GetString(new byte[] { j });
        //            string command = c1 + c2;
        //            if (RemoteCommandFactory.GetCommandByStringCommand(command, null) == null)
        //            {
        //                System.Diagnostics.Debug.WriteLine("Querying " + command);
        //                string outputFile = System.IO.Path.Combine(outputDirectory, command);
        //                //no existing command found... lets run it...
        //                RemoteCommand genericCommand = new RemoteCommand(RemoteCommandCodes.Identify, command);
        //                RemoteCommandResponse cr = new RemoteCommandResponse(genericCommand);

        //                if (FlukeController.SendCommand(genericCommand))
        //                {
        //                    FlukeController.GetResponse(cr);

        //                    if (cr.Status == CommandResponseStatus.Success)
        //                    {
        //                        System.IO.File.WriteAllBytes(outputFile + ".suc", cr.RawBytes);
        //                    }
        //                    else
        //                    {
        //                        if (cr.RawBytes != null)
        //                        {
        //                            if (cr.RawBytes.Length > 1 && cr.RawBytes[1] == 0x46)
        //                            {
        //                                System.IO.File.WriteAllBytes(outputFile + ".nac", cr.RawBytes);
        //                            }
        //                            else
        //                            {
        //                                System.IO.File.WriteAllBytes(outputFile + ".err", cr.RawBytes);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                System.Diagnostics.Debug.WriteLine("Skipping " + command);
        //            }
        //        }
        //    }
        //}

        private void radMenuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButtonTestDump_Click(object sender, EventArgs e)
        {
            //RunExtractTest();
            SelfTestDialog st = new SelfTestDialog();
            st.ShowDialog();
        }

        private void toolStripButtonCreateLibrary_Click(object sender, EventArgs e)
        {
            CreateLibrary();
        }

        private void radMenuItemLibrary_Click(object sender, EventArgs e)
        {
            CreateLibrary();
        }

        private void CreateLibrary()
        {
            MessageBox.Show("This is where you would defined a build a Library of components for a sequence.", "No", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void toolStripButtonNewSequence_Click(object sender, EventArgs e)
        {
            CreateSequence();
        }

        private void radMenuItemCreateSequence_Click(object sender, EventArgs e)
        {
            CreateSequence();
        }

        private void CreateSequence()
        {
            MessageBox.Show("This is where you would defined a build a sequence of tests.", "No", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void toolStripButtonQuickTest_Click(object sender, EventArgs e)
        {
            CreateQuickTest();
        }

        private void radMenuItemQuickTestSequence_Click(object sender, EventArgs e)
        {
            CreateQuickTest();
        }

        private void CreateQuickTest()
        {
            MessageBox.Show("This is where you can quickly test single IC's from the PC interface and attempting to use simulation without a reference device.", "No", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void radMenuItemTerminalRaw_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.TerminalRaw);
           
        }

        private void radMenuItemTerminalFormatted_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.TerminalFormatted);
        }

        private void radMenuItemTerminalSend_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.TerminalSend);

            //if (ControlFactory.UIElements.TerminalSendWindow == null)
            //{
            //    TerminalSend tsWindow = new TerminalSend();
            //    tsWindow.Dock = DockStyle.Fill;
            //    string caption = "Terminal Send:";

            //    ControlFactory.DockToolStrip(tsWindow, caption, DockPosition.Right, DockPosition.Fill, DockWindowControl.TerminalSend);

            //    ControlFactory.UIElements.TerminalSendWindow = tsWindow;               
            //}
            //else
            //{
            //    ControlFactory.UIElements.TerminalSendWindow.Show();
            //}
        }

        private void radMenuItemDocumentation_Click(object sender, EventArgs e)
        {
            OpenDocumentation();
        }

        private void OpenDocumentation()
        {
            System.Diagnostics.Process.Start(Globals.WIKIPAGE_URL);         
        }

        private void radMenuItemOperatorManual_Click(object sender, EventArgs e)
        {
            string exePath = Utilities.GetExecutablePath();
            string helpDoc = Path.Combine(exePath, "Documents", "Fluke_900_OperatorManual_RV5_0.pdf");
            if (File.Exists(helpDoc))
            {
                Utilities.OpenFileInDefaultViewer(helpDoc);
            }
            else
            {
                MessageBox.Show("Could not find the document at: " + helpDoc, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radMenuItemServiceManual_Click(object sender, EventArgs e)
        {
            string exePath = Utilities.GetExecutablePath();
            string helpDoc = Path.Combine(exePath, "Documents", "Fluke_900_ServiceManual.pdf");
            if (File.Exists(helpDoc))
            {
                Utilities.OpenFileInDefaultViewer(helpDoc);
            }
            else
            {
                MessageBox.Show("Could not find the document at: " + helpDoc, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonFileNew_Click(object sender, EventArgs e)
        {
            //TODO: this
            ControlFactory.OpenExistingDocumentInEditor(null);
        }

        private void toolStripButtonFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "Fluke Files (*.lib,*.loc,*.seq)|*.lib;*.loc;*.seq|All Files (*.*)|*";
            od.Multiselect = true;
            od.CheckFileExists = true;
            od.InitialDirectory = Utilities.GetBrowseDirectory();
            DialogResult dr = od.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in od.FileNames)
                {
                    ControlFactory.OpenExistingDocumentInEditor(file);
                    Globals.LastDirectoryBrowse = Path.GetDirectoryName(file);
                }
            }
        }

        private void toolStripButtonFileSave_Click(object sender, EventArgs e)
        {
            DocumentEditor de = radDockMain.DocumentManager.ActiveDocument as DocumentEditor;
            if (de != null)
            {
                de.SaveDocument();
            }
        }

        private void toolStripButtonFileNewLib_Click(object sender, EventArgs e)
        {
            ControlFactory.OpenNewDocumentInEditor(".lib");
        }

        private void toolStripButtonFileNewLOC_Click(object sender, EventArgs e)
        {
            ControlFactory.OpenNewDocumentInEditor(".loc");
        }

        private void toolStripButtonFileNewSEQ_Click(object sender, EventArgs e)
        {
            ControlFactory.OpenNewDocumentInEditor(".seq");
        }

        private void toolStripButtonFileSaveAll_Click(object sender, EventArgs e)
        {
            ControlFactory.SaveAllOpenFiles();
        }

        private void toolStripButtonProjectCreate_Click(object sender, EventArgs e)
        {
            CreateNewProject();
        }

        private void toolStripButtonProjectOpen_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private void radMenuItemOpenProject_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private void radMenuItemNewProject_Click(object sender, EventArgs e)
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

        private void toolStripButtonRunSeq_Click(object sender, EventArgs e)
        {
            if (!FlukeController.IsConnected)
            {
                MessageBox.Show("You must be connected to perform this action.", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            };

            DocumentEditor activeDocument = radDockMain.DocumentManager.ActiveDocument as DocumentEditor;
            if (activeDocument != null)
            {
                string lowerCaseExtension = Path.GetExtension(activeDocument.PathFilename).ToLower();
                if (lowerCaseExtension == ".seq" || lowerCaseExtension == ".loc")
                {
                    CompilationResult result = null;
                    ProgressManager.Start("Uploading and Compiling File...");
                    try
                    {
                        //copy file to Fluke
                        string flukeFilename = FileHelper.AdjustForTransfer(FileHelper.GetFilenameOnly(activeDocument.PathFilename));

                        FileLocations destinationLocation = FileLocations.FlukeCartridge;
                        bool? isCartridgeWriteable = false;
                        Task.Run(async () => { isCartridgeWriteable = await FlukeController.IsCartridgeWritable(); }).Wait();

                        bool IsCartridgeAvailable = false;
                        Task.Run(async () => { IsCartridgeAvailable = await FlukeController.IsCartridgeAvailable(); }).Wait();

                        if (!IsCartridgeAvailable || (isCartridgeWriteable.HasValue && !isCartridgeWriteable.Value))
                        {
                            destinationLocation = FileLocations.FlukeSystem;
                        }

                        //append the location suffix
                        flukeFilename = FileHelper.AppendLocation(flukeFilename, destinationLocation);

                        bool fileExists = false;
                        Task.Run(async () => { fileExists = await FlukeController.FileExists(flukeFilename); }).Wait();

                        if (fileExists)
                        {
                            DialogResult dr = MessageBox.Show("File already exists on Fluke, overwrite and continue?", "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == System.Windows.Forms.DialogResult.No)
                            {
                                return;
                            }
                            Task.Run(async () => { await FlukeController.DeleteFile(flukeFilename); }).Wait();
                        }

                        int filesCopied = 0;
                        Task.Run(async () => { filesCopied += await FlukeController.TransferFile(FileHelper.AppendLocation(activeDocument.PathFilename, FileLocations.LocalComputer), flukeFilename); }).Wait();
                        Task.Run(async () => { result = await FlukeController.CompileFile(flukeFilename); });
                    }
                    catch (Exception ex)
                    {
                        Globals.Exceptions.Add(new AppException(ex));
                        
                    }

                    ProgressManager.Stop();

                    if (result != null)
                    {
                        if (result.Success)
                        {
                            //getting farther, time to RUN
                            MessageBox.Show("Compilation Sucessful, run sequence from Fluke 900", "Ready to Run", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //CompilationError
                            MessageBox.Show(result.GetSummary(), "Compilation Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Please select a .SEQ file.", "Run Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void radMenuItemDirectory_Click(object sender, EventArgs e)
        {

        }

        private void AddRecentfile(string fileName)
        {
            //get the recent files already here...take them out
            RemoveRecentItem(fileName);
            //put it back, on top tho
            Telerik.WinControls.UI.RadMenuItem newItem = new Telerik.WinControls.UI.RadMenuItem();
            newItem.Text = fileName;
            radMenuItemRecent.Items.Insert(0, newItem);


        }


        private void LoadRecentFiles()
        {
            radMenuItemRecent.Items.Clear();

            string[] files = Properties.Settings.Default.RecentFileList.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string file in files)
            {
                Telerik.WinControls.UI.RadMenuItem item = new Telerik.WinControls.UI.RadMenuItem(file);
                item.Click += recentItem_Click;
                radMenuItemRecent.Items.Add(item);
            }
        }

        /// <summary>
        /// Saves the current recent files Menu Items into the application settings
        /// </summary>
        private void SaveRecentFiles()
        {
            Properties.Settings.Default.RecentFileList = String.Join(";", radMenuItemRecent.Items.Select(i => i.Text).ToArray());
            Properties.Settings.Default.Save();
        }

        private void RemoveRecentItem(string fileName)
        {
            for (int i = radMenuItemRecent.Items.Count - 1; i >= 0; i--)
            {
                Telerik.WinControls.UI.RadMenuItem item = radMenuItemRecent.Items[i] as Telerik.WinControls.UI.RadMenuItem;
                if (item != null)
                {
                    if (fileName.ToLower() == item.Text.ToLower())
                    {
                        radMenuItemRecent.Items.Remove(item);
                    }
                }
            }
        }

        private void recentItem_Click(object sender, EventArgs e)
        {
            //a recent item was checked here... open it 
            RadMenuItem item = sender as RadMenuItem;
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


        private void radMenuItemProjectDeveloper_Click(object sender, EventArgs e)
        {
            ControlFactory.ShowDockWindow(DockWindowControls.DeveloperOutput);
        }

        private void toolStripButtonLibraryTools_Click(object sender, EventArgs e)
        {
            //opens the dialog for parsing the binary library files
            LibraryParserDialog lp = new LibraryParserDialog();
            lp.ShowDialog();
        }

    }
}
