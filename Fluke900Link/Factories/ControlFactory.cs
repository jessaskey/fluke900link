using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Fluke900Link.Containers;
using Fluke900Link.Controls;
using Fluke900Link.Controllers;
using Fluke900Link.Dialogs;
using Fluke900Link.Helpers;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace Fluke900Link.Factories
{
    public enum DockWindowControls
    {
        TerminalRaw,
        TerminalFormatted,
        TerminalSend,
        DirectoryLocalPC,
        DirectoryFlukeSystem,
        DirectoryFlukeCartridge,
        SolutionExplorer,
        DeveloperOutput
    }

    /// <summary>
    /// Class that will instantiate all child controls for the DockControls and deal with the interrelations between them.
    /// </summary>
    static class ControlFactory
    {
        private static RadDock _radDock = null;

        //References to our Instantiated Controls (cheaters)
        public static ImageList ImageList16x16 = null;
        public static Splash Splash = null;
        public static MainForm MainForm = null;

        private static Dictionary<DockWindowControls, UserControl> _controlDictionary = new Dictionary<DockWindowControls, UserControl>();

        public static class UIElements
        {
            //toolstip areas for docking
            public static ToolTabStrip LeftSideStrip = null;
            public static ToolTabStrip RightSideStrip = null;
            public static ToolTabStrip BottomSideStrip = null;
            public static ToolTabStrip FillStrip = null;

            
            ////toolboxes and documents
            //public static DirectoryEditorControl DirectoryEditorLocal = null;
            //public static DirectoryEditorControl DirectoryEditorCartridge = null;
            //public static DirectoryEditorControl DirectoryEditorSystem = null;
            //public static TerminalOutputControl TerminalFormattedWindow = null;
            //public static TerminalOutputControl TerminalRawWindow = null;
            //public static TerminalSend TerminalSendWindow = null;
            //public static LibraryBrowser LibraryBrowser = null;
            //public static SolutionExplorer SolutionExplorer = null;
            //public static DeveloperOutput DeveloperOutput = null;
        }

        private static Dictionary<DockWindowControls, DockInformation> _dockWindowDefaultPositions = new Dictionary<DockWindowControls, DockInformation>() 
        {
                {DockWindowControls.TerminalRaw, new DockInformation(DockPosition.Right,DockPosition.Fill)},
                {DockWindowControls.TerminalFormatted, new DockInformation(DockPosition.Right,DockPosition.Fill)},
                {DockWindowControls.TerminalSend, new DockInformation(DockPosition.Right,DockPosition.Fill)},
                {DockWindowControls.DirectoryLocalPC, new DockInformation(DockPosition.Bottom,DockPosition.Right)},
                {DockWindowControls.DirectoryFlukeSystem, new DockInformation(DockPosition.Bottom,DockPosition.Right)},
                {DockWindowControls.DirectoryFlukeCartridge, new DockInformation(DockPosition.Bottom,DockPosition.Right)},
                {DockWindowControls.DeveloperOutput, new DockInformation(DockPosition.Bottom,DockPosition.Right)},
                {DockWindowControls.SolutionExplorer, new DockInformation(DockPosition.Left,DockPosition.Left)}
        };

        //we will manage cross-controll communication through the ControFactory
        //especially because many updates will come from async threads, so we will
        //implement IProgress objects that are set into the Asyc objects... they will 
        //trigger back to these IProgress methods, and UI controls can then subscribe
        //to the matching events so that a single Async update, can notify as many 
        //controls that want to know about the change. NOTE: These are registered in
        //the constructor of the MainForm object.
        public static Progress<ConnectionStatus> ConnectionStatusProgress = new Progress<ConnectionStatus>();
        public static Progress<CommunicationDirection> DataStatusProgress = new Progress<CommunicationDirection>();
        public static Progress<RemoteCommand> DataSendProgress = new Progress<RemoteCommand>();
        public static Progress<RemoteCommandResponse> DataReceiveProgress = new Progress<RemoteCommandResponse>(); 

        public static void Initialize(RadDock radDock)
        {
            _radDock = radDock;
        }

        public static void LoadSavedDockConfiguration(string dockLayoutPath)
        {
            if (File.Exists(dockLayoutPath))
            {
                try
                {
                    _radDock.LoadFromXml(dockLayoutPath);

                    if (Properties.Settings.Default.SaveToolboxWindows)
                    {
                        //fill controls
                        for (int i = 0; i < _radDock.DockWindows.Count; i++)
                        {
                            HostWindow hw = _radDock.DockWindows[i] as HostWindow;

                            if (hw != null)
                            {
                                //get our control enum
                                DockWindowControls? controlEnum = (DockWindowControls?)Enum.Parse(typeof(DockWindowControls), hw.Name.Replace("control_", ""));

                                if (controlEnum != null)
                                {
                                    //UserControl control = _controlDictionary[controlEnum.Value];
                                    if (!_controlDictionary.ContainsKey(controlEnum.Value))
                                    {
                                        UserControl control = CreateControl(controlEnum.Value);
                                        _controlDictionary.Add(controlEnum.Value, control);
                                        if (control != null)
                                        {
                                            //put the control into our HostWindow
                                            hw.LoadContent(control);
                                        }
                                    }
                                    else
                                    {
                                        //if we are here, then the control has already been automagically loaded? 

                                    }
                                }
                                else
                                {
                                    //close anything not configured here actually
                                    hw.Close();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //this happens if something is wrong with the file, clean up and do nothing then.
                    _radDock.RemoveAllWindows(DockWindowCloseAction.CloseAndDispose);
                    _radDock.RemoveAllDocumentWindows(DockWindowCloseAction.CloseAndDispose);
                    _radDock.CleanUp();
                    Globals.Exceptions.Add(new AppException(ex));
                }

                if (Properties.Settings.Default.SaveEditorWindows)
                {
                    for (int i = 0; i < _radDock.DocumentManager.DocumentArray.Length; i++)
                    {
                        DocumentEditor de = _radDock.DocumentManager.DocumentArray[i] as DocumentEditor;
                        if (de != null)
                        {
                            if (!File.Exists(de.ToolTipText))
                            {
                                de.Close();
                            }
                            else
                            {
                                de.Text = _radDock.DocumentManager.DocumentArray[i].Text.Replace("*", "");
                                de.OpenDocumentForEditing(de.ToolTipText);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Create control method... this is the only method that should instantiate new UserControls for docking
        /// </summary>
        /// <param name="controlEnum">The enum value of the window to instatiate.</param>
        /// <returns>The instantiated control.</returns>
        private static UserControl CreateControl(DockWindowControls controlEnum)
        {
            UserControl control = null;
            switch (controlEnum)
            {
                case DockWindowControls.TerminalRaw:
                    TerminalOutputControl rawTerminal = new TerminalOutputControl();
                    rawTerminal.Tag = "Terminal-Raw";
                    control = rawTerminal;
                    break;
                case DockWindowControls.TerminalFormatted:
                    TerminalOutputControl formattedTerminal = new TerminalOutputControl();
                    formattedTerminal.Tag = "Terminal-Formatted";
                    control = formattedTerminal;
                    break;
                case DockWindowControls.DirectoryLocalPC:
                    DirectoryEditorControl del = new DirectoryEditorControl();
                    del.Tag = "Directory: Local PC";
                    del.FileLocation = FileLocations.LocalComputer;
                    del.LoadFiles();
                    ConnectionStatusProgress.ProgressChanged += del.ConnectionStatusChanged;
                    control = del;
                    break;
                case DockWindowControls.DirectoryFlukeSystem:
                    DirectoryEditorControl des = new DirectoryEditorControl();
                    des.Tag = "Directory: FlukeSystem";
                    des.FileLocation = FileLocations.FlukeSystem;
                    des.LoadFiles();
                    ConnectionStatusProgress.ProgressChanged += des.ConnectionStatusChanged;
                    control = des;
                    break;
                case DockWindowControls.DirectoryFlukeCartridge:
                    DirectoryEditorControl dec = new DirectoryEditorControl();
                    dec.Tag = "Directory: Fluke Cartridge";
                    dec.FileLocation = FileLocations.FlukeCartridge;
                    dec.LoadFiles();
                    ConnectionStatusProgress.ProgressChanged += dec.ConnectionStatusChanged;
                    control = dec;
                    break;
                case DockWindowControls.SolutionExplorer:
                    SolutionExplorer se = new SolutionExplorer();
                    se.Tag = "Project Explorer";
                    control = se;
                    break;
                case DockWindowControls.DeveloperOutput:
                    DeveloperOutput dc = new DeveloperOutput();
                    dc.Tag = "Developer Output";
                    control = dc;
                    break;
            }
            return control;
        }

        public static void OpenPCSequence(string sequencePathFile)
        {
            //see if the current document is already there, if so show it and exit
            if (GetCurrentEditorWindow(sequencePathFile))
            {
                return;
            }

            SequenceEditor editor = new SequenceEditor();

            if (editor.OpenSequence(sequencePathFile))
            {
                HostWindow hw = new HostWindow(editor, DockType.Document);
                _radDock.DockWindow(hw, DockPosition.Fill); 
                //_radDock.AddDocument(editor);
            }
            else
            {
                MessageBox.Show("Couldn't open file - '" + sequencePathFile + "'", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static HostWindow GetExistingHostWindow(DockWindowControls controlEnum)
        {      
            if (_controlDictionary.ContainsKey(controlEnum))
            {
                UserControl userControl = _controlDictionary[controlEnum];
                return _radDock.GetHostWindow(userControl);
            }
            return null;   
        }

        public static void SaveDockConfiguration()
        {
            string dockLayoutPath = Path.Combine(Utilities.GetExecutablePath(), Globals.DOCK_CONFIGURATION_FILE);
            _radDock.SaveToXml(dockLayoutPath);

        }

        //public static void OpenLibraryInEditor(ProjectLibraryFile projectLibrary)
        //{
        //    //see if the current document is already there, if so show it and exit
        //    if (GetCurrentEditorWindow(projectLibrary.PathFileName))
        //    {
        //        return;
        //    }

        //    LibraryEditor le = new LibraryEditor();

        //    if (le.LoadLibrary(projectLibrary))
        //    {
        //        _radDock.AddDocument(le);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Couldn't open file - '" + projectLibrary.PathFileName + "'", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //public static void OpenLocationInEditor(ProjectLocationFile projectLocations)
        //{
        //    //see if the current document is already there, if so show it and exit
        //    if (GetCurrentEditorWindow(projectLocations.PathFileName))
        //    {
        //        return;
        //    }

        //    LocationsEditor le = new LocationsEditor();

        //    if (le.LoadLocations(projectLocations))
        //    {
        //        _radDock.AddDocument(le);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Couldn't open file - '" + projectLocations.PathFileName + "'", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //public static void OpenSequenceInEditor(ProjectSequenceFile projectSequence)
        //{
        //    //see if the current document is already there, if so show it and exit
        //    if (GetCurrentEditorWindow(projectSequence.PathFileName))
        //    {
        //        return;
        //    }

        //    SequenceEditor se = new SequenceEditor();

        //    if (se.LoadSequence(projectSequence))
        //    {
        //        _radDock.AddDocument(se);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Couldn't open file - '" + projectSequence.PathFileName + "'", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        public static void OpenProjectFileInEditor(ProjectFile projectFile)
        {
            //see if the current document is already there, if so show it and exit
            if (GetCurrentEditorWindow(projectFile.PathFileName))
            {
                return;
            }

            DocumentEditor editor = new DocumentEditor();

            if (editor.OpenDocumentForEditing(projectFile.PathFileName))
            {
                _radDock.AddDocument(editor);
            }
            else
            {
                MessageBox.Show("Couldn't open file - '" + projectFile.PathFileName + "'", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void OpenExistingDocumentInEditor(string pathFileName)
        {
            //see if the current document is already there, if so show it and exit
            if (GetCurrentEditorWindow(pathFileName))
            {
                return;
            }

            DocumentEditor de = new DocumentEditor();
            if (!String.IsNullOrEmpty(pathFileName))
            {
                de.Text = pathFileName;
                de.ToolTipText = pathFileName;
                de.Name = pathFileName;

                if (de.OpenDocumentForEditing(pathFileName))
                {
                    _radDock.AddDocument(de);
                }
                else
                {
                    MessageBox.Show("Couldn't open file - '" + pathFileName + "'", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                string newDocument = "new" + Globals.NEW_DOCUMENT_COUNTER.ToString();
                Globals.NEW_DOCUMENT_COUNTER++;
                de.Text = newDocument;
                de.ToolTipText = "";
                de.Name = newDocument;
                _radDock.AddDocument(de);
            }
        }

        public static void ThisIsACrappyMethodToTellADocumentWindowToReloadItsFiles(DockWindowControls documentEnum)
        {
            DirectoryEditorControl de = _controlDictionary[documentEnum] as DirectoryEditorControl;
            if (de != null)
            {
                de.LoadFiles();
            }
        }

        public static bool GetCurrentEditorWindow(string pathFileName)
        {
            if (!String.IsNullOrEmpty(pathFileName))
            {
                foreach (DockWindow dw in _radDock.DocumentManager.DocumentArray)
                {
                    if (dw.ToolTipText.ToLower() == pathFileName.ToLower())
                    {
                        _radDock.ActiveWindow = dw;
                        return true;
                    }
                }
            }
            return false;
        }

        public static void OpenNewDocumentInEditor(string extension)
        {
            string templateContent = Helpers.FileHelper.GetTemplate(extension);
            DocumentEditor de = new DocumentEditor();
            string newDocument = "new" + Globals.NEW_DOCUMENT_COUNTER.ToString() + extension.ToUpper();
            Globals.NEW_DOCUMENT_COUNTER++;
            de.CreateNewDocument(newDocument, templateContent);
            de.Name = Guid.NewGuid().ToString();
            _radDock.AddDocument(de);

        }

        /// <summary>
        /// Creates the DirectoryWindow for the specified location. The directory windows will always
        /// be placed across the bottom of the dock if it does not have a location already defined.
        /// </summary>
        /// <param name="fileLocation"></param>
        public static UserControl ShowDockWindow(DockWindowControls controlEnum)
        {
            //does this control already exist somewhere?
            HostWindow hw = GetExistingHostWindow(controlEnum);
            if (hw != null)
            {
                hw.Show();
                hw.BringToFront();
                hw.Focus();
                return _controlDictionary[controlEnum];
            }

            //if we are here, the control, doesn't exist anywhere so we do a few things..
            //instantiate the control
            UserControl control = CreateControl(controlEnum);

            //dock it into the default position
            DockInformation defaultDock = _dockWindowDefaultPositions[controlEnum];
            DockToolStrip(control, controlEnum, defaultDock.MainDockPosition, defaultDock.SubDockPosition);

            //log it so the factory knows it is here now
            _controlDictionary.Add(controlEnum, control);

            return control;
        }

        public static void RemoveDockWindow(DockWindowControls controlEnum)
        {
            HostWindow hw = GetExistingHostWindow(controlEnum);
            _controlDictionary.Remove(controlEnum);
            hw.Close();
        }

        public static void CreateDocumentEditor(string pathFileName)
        {

        }

        //public static void CreateTerminalWindow(TerminalWindowTypes terminalType)
        //{
        //    TerminalOutputControl terminalControl = null;
        //    DockWindowControl? controlEnum = null;
        //    string name = "";
        //    switch (terminalType)
        //    {
        //        case TerminalWindowTypes.Raw:
        //            terminalControl = ControlFactory.UIElements.TerminalRawWindow;
        //            controlEnum = DockWindowControl.TerminalRaw;
        //            name = "Raw Terminal Out";
        //            break;
        //        case TerminalWindowTypes.Formatted:
        //            terminalControl = ControlFactory.UIElements.TerminalFormattedWindow;
        //            controlEnum = DockWindowControl.TerminalFormatted;
        //            name = "Formatted Terminal Out";
        //            break;
        //    }

        //    if (terminalControl == null)
        //    {
        //        terminalControl = new TerminalOutputControl();
        //        terminalControl.Name = name;
        //        terminalControl.Dock = DockStyle.Fill;

        //        DockToolStrip(terminalControl, name, DockPosition.Right, DockPosition.Fill, controlEnum.Value);

        //        switch (terminalType)
        //        {
        //            case TerminalWindowTypes.Raw:
        //                ControlFactory.UIElements.TerminalRawWindow = terminalControl;
        //                break;
        //            case TerminalWindowTypes.Formatted:
        //                ControlFactory.UIElements.TerminalFormattedWindow = terminalControl;
        //                break;
        //        }

        //    }
        //}

        private static void DockToolStrip(UserControl control, DockWindowControls controlEnum, DockPosition mainDockPosition, DockPosition subDockPosition)
        {
            HostWindow hw = null;
            string hostWindowName = "control_" + controlEnum.ToString();

            switch (mainDockPosition)
            {
                case DockPosition.Right:

                    if (ControlFactory.UIElements.RightSideStrip == null)
                    {
                        hw = _radDock.DockControl(control, DockPosition.Right);
                        ControlFactory.UIElements.RightSideStrip = (ToolTabStrip)hw.Parent;
                        ((ToolTabStrip)hw.Parent).SizeInfo.SizeMode = SplitPanelSizeMode.Absolute;
                        ((ToolTabStrip)hw.Parent).SizeInfo.AbsoluteSize = new Size(350, 0);
                    }
                    else
                    {
                        //make sure there isnt already one here..
                        if (!ControlFactory.UIElements.RightSideStrip.Contains(control))
                        {
                            hw = _radDock.DockControl(control, ControlFactory.UIElements.RightSideStrip, subDockPosition);
                        }
                        //ControlFactory.UIElements.RightSideStrip = (ToolTabStrip)hw.Parent;
                    }
                    break;
                case DockPosition.Bottom:

                    if (ControlFactory.UIElements.BottomSideStrip == null)
                    {
                        hw = _radDock.DockControl(control, DockPosition.Bottom);
                        ControlFactory.UIElements.BottomSideStrip = (ToolTabStrip)hw.Parent;
                        ((ToolTabStrip)hw.Parent).SizeInfo.SizeMode = SplitPanelSizeMode.Absolute;
                        ((ToolTabStrip)hw.Parent).SizeInfo.AbsoluteSize = new Size(0, 250);
                    }
                    else
                    {
                        //make sure there isnt already one here..
                        if (!ControlFactory.UIElements.BottomSideStrip.Contains(control))
                        {
                            hw = _radDock.DockControl(control, ControlFactory.UIElements.BottomSideStrip, subDockPosition);
                        }
                        //ControlFactory.UIElements.BottomSideStrip = (ToolTabStrip)hw.Parent;
                    }
                    break;
                case DockPosition.Left:

                    if (ControlFactory.UIElements.LeftSideStrip == null)
                    {
                        hw = _radDock.DockControl(control, DockPosition.Left);
                        ControlFactory.UIElements.LeftSideStrip = (ToolTabStrip)hw.Parent;
                        ((ToolTabStrip)hw.Parent).SizeInfo.SizeMode = SplitPanelSizeMode.Absolute;
                        ((ToolTabStrip)hw.Parent).SizeInfo.AbsoluteSize = new Size(350, 0);
                    }
                    else
                    {
                        //make sure there isnt already one here..
                        if (!ControlFactory.UIElements.LeftSideStrip.Contains(control))
                        {
                            hw = _radDock.DockControl(control, ControlFactory.UIElements.LeftSideStrip, subDockPosition);
                        }
                        //ControlFactory.UIElements.LeftSideStrip = (ToolTabStrip)hw.Parent;
                    }
                    break;
                case DockPosition.Fill:

                    if (ControlFactory.UIElements.FillStrip == null)
                    {
                        hw = _radDock.DockControl(control, DockPosition.Fill);
                        ControlFactory.UIElements.FillStrip = (ToolTabStrip)hw.Parent;
                        ControlFactory.UIElements.FillStrip.SizeInfo.SizeMode = SplitPanelSizeMode.Absolute;
                        ControlFactory.UIElements.FillStrip.SizeInfo.AbsoluteSize = new Size(350, 0);
                    }
                    else
                    {
                        //make sure there isnt already one here..
                        if (!ControlFactory.UIElements.FillStrip.Contains(control))
                        {
                            hw = _radDock.DockControl(control, ControlFactory.UIElements.FillStrip, subDockPosition);
                        }
                        //ControlFactory.UIElements.FillStrip = (ToolTabStrip)hw.Parent;
                    }
                    break;
            }

            if (hw != null)
            {
                hw.Text = control.Tag.ToString();
                hw.Name = hostWindowName;

                if (control.MinimumSize.Width > 0 || control.MinimumSize.Height > 0)
                {
                    DockTabStrip dockTabStrip = (DockTabStrip)hw.TabStrip;
                    dockTabStrip.SizeInfo.MinimumSize = new System.Drawing.Size(control.MinimumSize.Width, control.MinimumSize.Height);
                }
            }

        }

        //public static void ShowDeveloperConsole()
        //{

        //    string caption = "Developer Console";

        //    if (ControlFactory.UIElements.DeveloperOutput == null)
        //    {
        //        ControlFactory.UIElements.DeveloperOutput = new DeveloperOutput();
        //        ControlFactory.DockToolStrip(ControlFactory.UIElements.DeveloperOutput, caption, DockPosition.Bottom, DockPosition.Right, DockWindowControl.DeveloperOutput);
        //        ControlFactory.UIElements.DeveloperOutput.Dock = DockStyle.Fill;
        //    }

        //    if (ControlFactory.UIElements.DeveloperOutput != null)
        //    {
        //        HostWindow hw = _radDock.GetHostWindow(ControlFactory.UIElements.DeveloperOutput);
        //        hw.Show();
        //        hw.Focus();
        //    }
        //}

        //public static void LoadProjectToTree(Project project, bool show)
        //{
        //    if (ControlFactory.UIElements.SolutionExplorer == null)
        //    {
        //        SolutionExplorer se = new SolutionExplorer();
        //        ControlFactory.DockToolStrip(se, "Project Browser", DockPosition.Left, DockPosition.Left, DockWindowControl.SolutionExplorer);
        //        ControlFactory.UIElements.SolutionExplorer = se;
        //    }
        //    ControlFactory.UIElements.SolutionExplorer.LoadProject(project);

        //    if (show)
        //    {
        //        HostWindow hw = _radDock.GetHostWindow(ControlFactory.UIElements.SolutionExplorer);
        //        if (hw != null)
        //        {
        //            hw.Show();
        //            hw.Focus();
        //        }
        //    }
        //}

        public static void SaveAllOpenFiles()
        {
            foreach (DockWindow dw in _radDock.DocumentManager.DocumentArray)
            {
                DocumentEditor de = dw as DocumentEditor;
                if (de != null)
                {
                    if (de.IsModified)
                    {
                        de.SaveDocument();
                    }
                }
            }
            ProjectFactory.SaveProject();
        }

    }
}
