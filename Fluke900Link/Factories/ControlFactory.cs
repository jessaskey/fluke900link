using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Fluke900;
using Fluke900Link.Containers;
using Fluke900Link.Controls;
//using Telerik.WinControls.UI;
//using Telerik.WinControls.UI.Docking;
using WeifenLuo.WinFormsUI.Docking;
using Fluke900.Helpers;
using Fluke900.Containers;

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
        //private static RadDock _radDock = null;
        private static DockPanel _dockPanel = null;

        //References to our Instantiated Controls (cheaters)
        public static ImageList ImageList16x16 = null;
        public static Splash Splash = null;
        public static MainForm2 MainForm2 = null;

        private static Dictionary<DockWindowControls, DockContent> _controlDictionary = new Dictionary<DockWindowControls, DockContent>();

        //public static class UIElements
        //{
        //    //toolstip areas for docking
        //    public static ToolTabStrip LeftSideStrip = null;
        //    public static ToolTabStrip RightSideStrip = null;
        //    public static ToolTabStrip BottomSideStrip = null;
        //    public static ToolTabStrip FillStrip = null;

            
        //    ////toolboxes and documents
        //    //public static DirectoryEditorControl DirectoryEditorLocal = null;
        //    //public static DirectoryEditorControl DirectoryEditorCartridge = null;
        //    //public static DirectoryEditorControl DirectoryEditorSystem = null;
        //    //public static TerminalOutputControl TerminalFormattedWindow = null;
        //    //public static TerminalOutputControl TerminalRawWindow = null;
        //    //public static TerminalSend TerminalSendWindow = null;
        //    //public static LibraryBrowser LibraryBrowser = null;
        //    //public static SolutionExplorer SolutionExplorer = null;
        //    //public static DeveloperOutput DeveloperOutput = null;
        //}

        private static Dictionary<DockWindowControls, DockInformation> _dockWindowDefaultPositions = new Dictionary<DockWindowControls, DockInformation>() 
        {
                {DockWindowControls.TerminalRaw, new DockInformation(DockState.DockRight,DockAlignment.Top)},
                {DockWindowControls.TerminalFormatted, new DockInformation(DockState.DockRight,DockAlignment.Top)},
                {DockWindowControls.TerminalSend, new DockInformation(DockState.DockRight,DockAlignment.Top)},
                {DockWindowControls.DirectoryLocalPC, new DockInformation(DockState.DockBottom,DockAlignment.Left)},
                {DockWindowControls.DirectoryFlukeSystem, new DockInformation(DockState.DockBottom,DockAlignment.Right)},
                {DockWindowControls.DirectoryFlukeCartridge, new DockInformation(DockState.DockBottom,DockAlignment.Right)},
                {DockWindowControls.DeveloperOutput, new DockInformation(DockState.DockBottom,DockAlignment.Right)},
                {DockWindowControls.SolutionExplorer, new DockInformation(DockState.DockLeft,DockAlignment.Left)}
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
        public static Progress<byte[]> DataSendProgress = new Progress<byte[]>();
        public static Progress<byte[]> DataReceiveProgress = new Progress<byte[]>();
        public static Progress<ClientCommand> CommandSendProgress = new Progress<ClientCommand>();
        public static Progress<ClientCommandResponse> CommandResponseProgress = new Progress<ClientCommandResponse>();

        //[Obsolete]
        //public static void Initialize(RadDock radDock)
        //{
        //    _radDock = radDock;
        //}

        public static void Initialize(MainForm2 mainForm, DockPanel dockPanel, ImageList imageList)
        {
            _dockPanel = dockPanel;
            MainForm2 = mainForm;
            ImageList16x16 = imageList;
        }

        public static DockContent GetControl(DockWindowControls controlEnum)
        {
            if (!_controlDictionary.ContainsKey(controlEnum))
            {
                return _controlDictionary[controlEnum];
            }
            return null;
        }

        public static void LoadSavedDockConfiguration(string dockLayoutPath)
        {
            if (File.Exists(dockLayoutPath))
            {
                try
                {
                    if (_dockPanel != null)
                    {
                        // TODO: Fix this
                        ///_dockPanel.LoadFromXml(dockLayoutPath, DeserializeDockContent.R);

                        if (Properties.Settings.Default.SaveToolboxWindows)
                        {
                            //fill controls
                            for (int i = 0; i < _dockPanel.DockWindows.Count; i++)
                            {
                                WeifenLuo.WinFormsUI.Docking.DockWindow hw = _dockPanel.DockWindows[i];

                                if (hw != null)
                                {
                                    //get our control enum
                                    DockWindowControls? controlEnum = (DockWindowControls?)Enum.Parse(typeof(DockWindowControls), hw.Name.Replace("control_", ""));

                                    if (controlEnum != null)
                                    {
                                        //UserControl control = _controlDictionary[controlEnum.Value];
                                        if (!_controlDictionary.ContainsKey(controlEnum.Value))
                                        {
                                            DockContent content = CreateControl(controlEnum.Value);
                                            _controlDictionary.Add(controlEnum.Value, content);
                                            if (content != null)
                                            {
                                                //put the control into our HostWindow
                                                hw.Controls.Add(content);
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
                                        //TODO: Fix this?
                                        //hw..Close();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //TODO: fix this?
                    //if (_radDock != null)
                    //{
                    //    //this happens if something is wrong with the file, clean up and do nothing then.
                    //    _radDock.RemoveAllWindows(DockWindowCloseAction.CloseAndDispose);
                    //    _radDock.RemoveAllDocumentWindows(DockWindowCloseAction.CloseAndDispose);
                    //    _radDock.CleanUp();
                    //}
                    ApplicationGlobals.Exceptions.Add(new AppException(ex));
                }

                //if (_radDock != null)
                //{
                //    if (Properties.Settings.Default.SaveEditorWindows)
                //    {
                //        for (int i = 0; i < _radDock.DocumentManager.DocumentArray.Length; i++)
                //        {
                //            DocumentEditor de = _radDock.DocumentManager.DocumentArray[i] as DocumentEditor;
                //            if (de != null)
                //            {
                //                if (!File.Exists(de.ToolTipText))
                //                {
                //                    de.Close();
                //                }
                //                else
                //                {
                //                    de.Text = _radDock.DocumentManager.DocumentArray[i].Text.Replace("*", "");
                //                    de.OpenDocumentForEditing(de.ToolTipText);
                //                }
                //            }
                //        }
                //    }
                //}
            }
        }


        /// <summary>
        /// Create control method... this is the only method that should instantiate new UserControls for docking
        /// </summary>
        /// <param name="controlEnum">The enum value of the window to instatiate.</param>
        /// <returns>The instantiated control.</returns>
        private static DockContentEx CreateControl(DockWindowControls controlEnum)
        {
            DockContentEx content = null;
            switch (controlEnum)
            {
                case DockWindowControls.TerminalRaw:
                    TerminalOutputControl rawTerminal = new TerminalOutputControl();
                    rawTerminal.TabText = "Terminal-Raw";
                    DataSendProgress.ProgressChanged += (s, e) =>
                    {
                        rawTerminal.DataSendProgress(e);
                    };
                    DataReceiveProgress.ProgressChanged += (s, e) =>
                    {
                        rawTerminal.DataReceiveProgress(e);
                    };
                    content = rawTerminal;
                    break;
                case DockWindowControls.TerminalFormatted:
                    TerminalOutputFormattedControl formattedTerminal = new TerminalOutputFormattedControl();
                    CommandSendProgress.ProgressChanged += (s, e) =>
                    {
                        formattedTerminal.CommandSendProgress(e);
                    };
                    CommandResponseProgress.ProgressChanged += (s, e) =>
                    {
                        formattedTerminal.CommandResponseProgress(e);
                    };
                    formattedTerminal.TabText = "Terminal-Formatted";
                    content = formattedTerminal;
                    break;
                case DockWindowControls.TerminalSend:
                    TerminalSendControl ts = new TerminalSendControl();
                    ts.TabText = "Terminal Send";
                    //ConnectionStatusProgress.ProgressChanged += ts.ConnectionStatusChanged;
                    content = ts;
                    break;
                case DockWindowControls.DirectoryLocalPC:
                    DirectoryEditorControl del = new DirectoryEditorControl();
                    del.TabText = "Directory: Local PC";
                    del.FileLocation = FileLocations.LocalComputer;
                    del.LoadFiles();
                    ConnectionStatusProgress.ProgressChanged += del.ConnectionStatusChanged;
                    content = del;
                    break;
                case DockWindowControls.DirectoryFlukeSystem:
                    DirectoryEditorControl des = new DirectoryEditorControl();
                    des.TabText = "Directory: FlukeSystem";
                    des.FileLocation = FileLocations.FlukeSystem;
                    des.LoadFiles();
                    ConnectionStatusProgress.ProgressChanged += des.ConnectionStatusChanged;
                    content = des;
                    break;
                case DockWindowControls.DirectoryFlukeCartridge:
                    DirectoryEditorControl dec = new DirectoryEditorControl();
                    dec.TabText = "Directory: Fluke Cartridge";
                    dec.FileLocation = FileLocations.FlukeCartridge;
                    dec.LoadFiles();
                    ConnectionStatusProgress.ProgressChanged += dec.ConnectionStatusChanged;
                    content = dec;
                    break;
                case DockWindowControls.SolutionExplorer:
                    SolutionExplorer se = new SolutionExplorer();
                    se.TabText = "Project Explorer";
                    content = se;
                    break;
                case DockWindowControls.DeveloperOutput:
                    DeveloperOutput dc = new DeveloperOutput();
                    dc.TabText = "Developer Output";
                    content = dc;
                    break;
            }
            content.DockWindowControl = controlEnum;
            return content;
        }

        public static void OpenTestLocation(ProjectLocation location)
        {
            //see if the current document is already there, if so show it and exit
            if (GetCurrentLocationWindow(location.Name))
            {
                return;
            }

            ProjectLocationControl plc = new ProjectLocationControl();
            plc.ToolTipText = location.Name;

            if (plc.OpenLocation(location))
            {
                //_radDock.AddDocument(editor);
                plc.Show(_dockPanel, DockState.Document);
            }
            else
            {
                MessageBox.Show("Couldn't open location - '" + location.Name + "'", "Location Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                //HostWindow hw = new HostWindow(editor, DockType.Document);
                //_radDock.DockWindow(hw, DockPosition.Fill); 
                //_radDock.AddDocument(editor);
                editor.Show(_dockPanel, DockState.Document);
            }
            else
            {
                MessageBox.Show("Couldn't open file - '" + sequencePathFile + "'", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static DockContentEx GetExistingDockContent(DockWindowControls controlEnum)
        {
            return _dockPanel.Contents.OfType<DockContentEx>().Where(p => p.DockWindowControl == controlEnum).FirstOrDefault();
            //if (_controlDictionary.ContainsKey(controlEnum))
            //{
            //    return _controlDictionary[controlEnum];
            //    //return _dockPanel.Win[_dockPanel.Contents.IndexOf(content)];
            //}
            //return null;   
        }

        public static void SaveDockConfiguration()
        {
            string dockLayoutPath = Path.Combine(Utilities.GetExecutablePath(), ApplicationGlobals.DOCK_CONFIGURATION_FILE);
            if (_dockPanel != null)
            {
                _dockPanel.SaveAsXml(dockLayoutPath);
            }
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
                //_radDock.AddDocument(editor);
                editor.Show(_dockPanel, DockState.Document);
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
                    //_radDock.AddDocument(de);
                    de.Show(_dockPanel, DockState.Document);
                }
                else
                {
                    MessageBox.Show("Couldn't open file - '" + pathFileName + "'", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                string newDocument = "new" + ApplicationGlobals.NEW_DOCUMENT_COUNTER.ToString();
                ApplicationGlobals.NEW_DOCUMENT_COUNTER++;
                de.Text = newDocument;
                de.ToolTipText = "";
                de.Name = newDocument;
                //_radDock.AddDocument(de);
                de.Show(_dockPanel, DockState.Document);
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
                foreach (DockContentEx doc in _dockPanel.Documents)
                {
                    if (doc.ToolTipText != null)
                    {
                        if (doc.ToolTipText.ToLower() == pathFileName.ToLower())
                        {
                            //_radDock.ActiveWindow = doc;
                            doc.Activate();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool GetCurrentLocationWindow(string locationName)
        {
            if (!String.IsNullOrEmpty(locationName))
            {
                foreach (DockContentEx doc in _dockPanel.Documents)
                {
                    if (doc.ToolTipText.ToLower() == locationName.ToLower())
                    {
                        //_radDock.ActiveWindow = doc;
                        doc.Activate();
                        return true;
                    }
                }
            }
            return false;
        }

        public static void OpenNewDocumentInEditor(string extension)
        {
            string templateContent = FileHelper.GetTemplate(extension, Properties.Settings.Default.DefaultFilesDirectory);
            DocumentEditor de = new DocumentEditor();
            string newDocument = "new" + ApplicationGlobals.NEW_DOCUMENT_COUNTER.ToString() + extension.ToUpper();
            ApplicationGlobals.NEW_DOCUMENT_COUNTER++;
            de.CreateNewDocument(newDocument, templateContent);
            de.Name = Guid.NewGuid().ToString();
            //_radDock.AddDocument(de);
            de.Show(_dockPanel, DockState.Document);

        }

        /// <summary>
        /// Creates the DirectoryWindow for the specified location. The directory windows will always
        /// be placed across the bottom of the dock if it does not have a location already defined.
        /// </summary>
        /// <param name="fileLocation"></param>
        public static DockContent ShowDockWindow(DockWindowControls contentType)
        {
            _dockPanel.SuspendLayout();
            //does this control already exist somewhere?
            DockContent content = GetExistingDockContent(contentType);
            if (content != null)
            {
                content.Activate();
                content = _controlDictionary[contentType];
            }
            else
            {
                //dock it into the default position
                DockInformation dockInfo = _dockWindowDefaultPositions[contentType];
                //if we are here, the control, doesn't exist anywhere so we do a few things..
                //instantiate the control
                content = CreateControl(contentType);
                switch (dockInfo.MainDockPosition)
                {
                    //Left and Right default to tabs
                    case WeifenLuo.WinFormsUI.Docking.DockState.DockLeft:
                        content.Show(_dockPanel, dockInfo.MainDockPosition);
                        break;
                    case WeifenLuo.WinFormsUI.Docking.DockState.DockRight:                    
                        content.Show(_dockPanel, dockInfo.MainDockPosition);
                        break;
                    case WeifenLuo.WinFormsUI.Docking.DockState.DockBottom:
                        DockPane pane = _dockPanel.Panes.Where(p => p.DockState == DockState.DockBottom).FirstOrDefault();
                        if (pane == null)
                        {
                            content.Show(_dockPanel, dockInfo.MainDockPosition);
                        }
                        else
                        {
                            int count = _dockPanel.Panes.Where(p => p.DockState == DockState.DockBottom).Count();
                            double pr = 0.333333d;
                            switch (count)
                            {
                                case 2:
                                    pr = .5d;
                                    break;
                            }
                            content.Show(pane, dockInfo.DockAlignment, pr);
                        }
                        //bottom always forces content to equal parts
                        //double proportion = 1.0d / (double)(_dockPanel.Panes.Where(p => p.DockState == DockState.DockBottom).Count());
                        //foreach(var v in _dockPanel.Panes.Where(p => p.DockState == DockState.DockBottom))
                        //{
                        //    //v.Width = (int)(v.DockPanel.Width * proportion);
                        //    //v.SetNestedDockingProportion(proportion); // = new Size((int)(v.DockPanel.Width * proportion), v.DockPanel.Size.Height);
                        //}
                        break;
                    case WeifenLuo.WinFormsUI.Docking.DockState.Document:
                        content.Show(_dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        break;
                }

                //log it so the factory knows it is here now
                _controlDictionary.Add(contentType, content);
            }
            _dockPanel.ResumeLayout();
            return content;
        }

        public static void RemoveDockWindow(DockWindowControls controlEnum)
        {
            DockContent content = GetExistingDockContent(controlEnum);
            _controlDictionary.Remove(controlEnum);
            content.Close();
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


        //private static void DockToolStrip(DockContent content, DockWindowControls controlEnum, DockPosition mainDockPosition, DockPosition subDockPosition)
        //{
        //    HostWindow hw = null;
        //    string hostWindowName = "control_" + controlEnum.ToString();

        //    switch (mainDockPosition)
        //    {
        //        case DockPosition.Right:

        //            if (ControlFactory.UIElements.RightSideStrip == null)
        //            {
        //                content.Show(_dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
        //                //hw = _radDock.DockControl(control, DockPosition.Right);
        //                //ControlFactory.UIElements.RightSideStrip = (ToolTabStrip)hw.Parent;
        //                //((ToolTabStrip)hw.Parent).SizeInfo.SizeMode = SplitPanelSizeMode.Absolute;
        //                //((ToolTabStrip)hw.Parent).SizeInfo.AbsoluteSize = new Size(350, 0);
        //            }
        //            else
        //            {
        //                //make sure there isnt already one here..
        //                //if (!ControlFactory.UIElements.RightSideStrip.Contains(control))
        //                //{
        //                //    hw = _radDock.DockControl(control, ControlFactory.UIElements.RightSideStrip, subDockPosition);
        //                //}
        //            }
        //            break;
        //        case DockPosition.Bottom:

        //            if (ControlFactory.UIElements.BottomSideStrip == null)
        //            {
        //                content.Show(_dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockBottom);
        //                //hw = _radDock.DockControl(control, DockPosition.Bottom);
        //                //ControlFactory.UIElements.BottomSideStrip = (ToolTabStrip)hw.Parent;
        //                //((ToolTabStrip)hw.Parent).SizeInfo.SizeMode = SplitPanelSizeMode.Absolute;
        //                //((ToolTabStrip)hw.Parent).SizeInfo.AbsoluteSize = new Size(0, 250);
        //            }
        //            else
        //            {
        //                //make sure there isnt already one here..
        //                //if (!ControlFactory.UIElements.BottomSideStrip.Contains(control))
        //                //{
        //                //    hw = _radDock.DockControl(control, ControlFactory.UIElements.BottomSideStrip, subDockPosition);
        //                //}
        //            }
        //            break;
        //        case DockPosition.Left:

        //            if (ControlFactory.UIElements.LeftSideStrip == null)
        //            {
        //                content.Show(_dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft);
        //                //content.Size = new Size(350, 0);
        //                //content.Controls.Add(control);
        //            }
        //            else
        //            {
        //                //make sure there isnt already one here..
        //                //if (!ControlFactory.UIElements.LeftSideStrip.Contains(control))
        //                //{
        //                //    hw = _radDock.DockControl(control, ControlFactory.UIElements.LeftSideStrip, subDockPosition);
        //                //}
        //            }
        //            break;
        //        case DockPosition.Fill:

        //            if (ControlFactory.UIElements.FillStrip == null)
        //            {
        //                content.Show(_dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
        //                //hw = _radDock.DockControl(control, DockPosition.Fill);
        //                //ControlFactory.UIElements.FillStrip = (ToolTabStrip)hw.Parent;
        //                //ControlFactory.UIElements.FillStrip.SizeInfo.SizeMode = SplitPanelSizeMode.Absolute;
        //                //ControlFactory.UIElements.FillStrip.SizeInfo.AbsoluteSize = new Size(350, 0);
        //            }
        //            else
        //            {
        //                //make sure there isnt already one here..
        //                //if (!ControlFactory.UIElements.FillStrip.Contains(control))
        //                //{
        //                //    hw = _radDock.DockControl(control, ControlFactory.UIElements.FillStrip, subDockPosition);
        //                //}
        //            }
        //            break;
        //    }

        //    //TODO: Fix this
        //    //if (hw != null)
        //    //{
        //    //    hw.Text = control.Tag.ToString();
        //    //    hw.Name = hostWindowName;

        //    //    if (control.MinimumSize.Width > 0 || control.MinimumSize.Height > 0)
        //    //    {
        //    //        DockTabStrip dockTabStrip = (DockTabStrip)hw.TabStrip;
        //    //        dockTabStrip.SizeInfo.MinimumSize = new System.Drawing.Size(control.MinimumSize.Width, control.MinimumSize.Height);
        //    //    }
        //    //}
        //}

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
            foreach (DockContentEx doc in _dockPanel.Documents)
            {
                DocumentEditor de = doc as DocumentEditor;
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
