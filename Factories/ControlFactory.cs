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
using Fluke900Link.Dialogs;
using Fluke900Link.Helpers;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace Fluke900Link.Factories
{
    /// <summary>
    /// Class that will instantiate all child controls for the DockControls and deal with the interrelations between them.
    /// </summary>
    static class ControlFactory
    {
        private static RadDock _radDock = null;

        public static void SetDock(RadDock radDock)
        {
            _radDock = radDock;
        }

        public static void LoadSavedDockConfiguration()
        {
            string dockLayoutPath = Path.Combine(Utilities.GetExecutablePath(), Globals.DOCK_CONFIGURATION_FILE);
            if (File.Exists(dockLayoutPath))
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
                            string title = hw.Text;
                            if (hw.Text.StartsWith("Raw Terminal Out"))
                            {
                                TextEditorControl rawTerminal = new TextEditorControl();
                                //rawTerminal.Name = hw.Text;
                                Globals.UIElements.TerminalRawWindow = rawTerminal;
                                hw.LoadContent(rawTerminal);
                                hw.Text = title;
                            }
                            else if (hw.Text.StartsWith("Formatted Terminal Out"))
                            {
                                TextEditorControl formattedTerminal = new TextEditorControl();
                                //formattedTerminal.Name = hw.Text;
                                Globals.UIElements.TerminalFormattedWindow = formattedTerminal;
                                hw.LoadContent(formattedTerminal);
                                hw.Text = title;
                            }
                            else if (hw.Text.StartsWith("Library Browser"))
                            {
                                LibraryBrowser lb = new LibraryBrowser();
                                lb.LoadFiles();
                                lb.Name = title;
                                Globals.UIElements.LibraryBrowser = lb;
                                hw.LoadContent(lb);
                                hw.Text = title;
                            }
                            else if (hw.Text.StartsWith("Directory: LocalComputer"))
                            {
                                DirectoryEditorControl del = new DirectoryEditorControl();
                                del.FileLocation = FileLocations.LocalComputer;
                                //del.Name = hw.Text;
                                del.LoadFiles();
                                Fluke900.OnConnectionStatusChanged += del.OnConnectionStatusChanged;
                                Globals.UIElements.DirectoryEditorLocal = del;
                                hw.LoadContent(del);
                                hw.Text = title;
                            }
                            else if (hw.Text.StartsWith("Directory: FlukeCartridge"))
                            {
                                DirectoryEditorControl del = new DirectoryEditorControl();
                                del.FileLocation = FileLocations.FlukeCartridge;
                                //del.Name = hw.Text;
                                del.LoadFiles();
                                Fluke900.OnConnectionStatusChanged += del.OnConnectionStatusChanged;
                                Globals.UIElements.DirectoryEditorCartridge = del;
                                hw.LoadContent(del);
                                hw.Text = title;
                            }
                            else if (hw.Text.StartsWith("Directory: FlukeSystem"))
                            {
                                DirectoryEditorControl del = new DirectoryEditorControl();
                                del.FileLocation = FileLocations.FlukeSystem;
                                del.Name = hw.Text;
                                del.LoadFiles();
                                Fluke900.OnConnectionStatusChanged += del.OnConnectionStatusChanged;
                                Globals.UIElements.DirectoryEditorSystem = del;
                                hw.LoadContent(del);
                                hw.Text = title;
                            }
                            else if (hw.Text.StartsWith("Project Browser"))
                            {
                                SolutionExplorer se = new SolutionExplorer();
                                Globals.UIElements.SolutionExplorer = se;
                                hw.LoadContent(se);
                                hw.Text = title;
                            }
                            else if (hw.Text.StartsWith("Developer Console"))
                            {
                                DeveloperConsole dc = new DeveloperConsole();
                                Globals.UIElements.DeveloperConsole = dc;
                                hw.LoadContent(dc);
                                hw.Text = title;
                            }
                            else
                            {
                                //close anything not configured here actually
                                hw.Close();
                            }
                        }
                    }
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
                                //de.ToolTipText = radDockMain.DocumentManager.DocumentArray[i].ToolTipText;
                                de.OpenDocumentForEditing(de.ToolTipText);
                                //radDockMain.DocumentManager.DocumentArray[i].Controls.Add(de);
                            }
                        }
                    }
                }

            }
        }

        public static void SaveDockConfiguration()
        {
            string dockLayoutPath = Path.Combine(Utilities.GetExecutablePath(), Globals.DOCK_CONFIGURATION_FILE);
            _radDock.SaveToXml(dockLayoutPath);

        }

        public static void OpenLibraryInEditor(ProjectLibraryFile projectLibrary)
        {
            //see if the current document is already there, if so show it and exit
            if (GetCurrentEditorWindow(projectLibrary.PathFileName))
            {
                return;
            }

            LibraryEditor le = new LibraryEditor();

            if (le.LoadLibrary(projectLibrary))
            {
                _radDock.AddDocument(le);
            }
            else
            {
                MessageBox.Show("Couldn't open file - '" + projectLibrary.PathFileName + "'", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void OpenLocationInEditor(ProjectLocationFile projectLocations)
        {
            //see if the current document is already there, if so show it and exit
            if (GetCurrentEditorWindow(projectLocations.PathFileName))
            {
                return;
            }

            LocationsEditor le = new LocationsEditor();

            if (le.LoadLocations(projectLocations))
            {
                _radDock.AddDocument(le);
            }
            else
            {
                MessageBox.Show("Couldn't open file - '" + projectLocations.PathFileName + "'", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void OpenSequenceInEditor(ProjectSequenceFile projectSequence)
        {
            //see if the current document is already there, if so show it and exit
            if (GetCurrentEditorWindow(projectSequence.PathFileName))
            {
                return;
            }

            SequenceEditor se = new SequenceEditor();

            if (se.LoadSequence(projectSequence))
            {
                _radDock.AddDocument(se);
            }
            else
            {
                MessageBox.Show("Couldn't open file - '" + projectSequence.PathFileName + "'", "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public static DirectoryEditorControl CreateDirectoryWindow(FileLocations fileLocation)
        {
            DirectoryEditorControl directoryWindow = null;

            switch (fileLocation)
            {
                case FileLocations.LocalComputer:
                    directoryWindow = Globals.UIElements.DirectoryEditorLocal;
                    break;
                case FileLocations.FlukeCartridge:
                    directoryWindow = Globals.UIElements.DirectoryEditorCartridge;
                    break;
                case FileLocations.FlukeSystem:
                    directoryWindow = Globals.UIElements.DirectoryEditorSystem;
                    break;
            }

            string caption = "Directory: " + Enum.GetName(typeof(FileLocations), fileLocation);

            if (directoryWindow != null)
            {
                DockToolStrip(directoryWindow, caption, DockPosition.Bottom, DockPosition.Right);
                directoryWindow.Show();
            }
            else
            {
                directoryWindow = new DirectoryEditorControl();
                directoryWindow.Dock = DockStyle.Fill;
                directoryWindow.FileLocation = fileLocation;
                directoryWindow.LoadFiles();
                Fluke900.OnConnectionStatusChanged += directoryWindow.OnConnectionStatusChanged;

                switch (fileLocation)
                {
                    case FileLocations.LocalComputer:
                        Globals.UIElements.DirectoryEditorLocal = directoryWindow;
                        break;
                    case FileLocations.FlukeCartridge:
                        Globals.UIElements.DirectoryEditorCartridge = directoryWindow;
                        break;
                    case FileLocations.FlukeSystem:
                        Globals.UIElements.DirectoryEditorSystem = directoryWindow;
                        break;
                }
            }

            return directoryWindow;
        }

        public static void RemoveDirectoryWindow(FileLocations fileLocation)
        {

            if (Globals.UIElements.BottomSideStrip != null)
            {
                DockWindow dw = Globals.UIElements.BottomSideStrip.DockManager.DockWindows.Where(d => d.Text == "Directory: " + Enum.GetName(typeof(FileLocations), fileLocation)).FirstOrDefault();
                if (dw != null)
                {
                    Globals.UIElements.BottomSideStrip.DockManager.RemoveWindow(dw);
                }
            }
        }

        public static void CreateDocumentEditor(string pathFileName)
        {

        }

        public static void CreateTerminalWindow(TerminalWindowTypes terminalType)
        {
            TextEditorControl editor = null;
            string name = "";
            switch (terminalType)
            {
                case TerminalWindowTypes.Raw:
                    editor = Globals.UIElements.TerminalRawWindow;
                    name = "Raw Terminal Out";
                    break;
                case TerminalWindowTypes.Formatted:
                    editor = Globals.UIElements.TerminalFormattedWindow;
                    name = "Formatted Terminal Out";
                    break;
            }

            if (editor == null)
            {
                editor = new TextEditorControl();
                editor.Name = name;
                editor.Dock = DockStyle.Fill;

                DockToolStrip(editor, name, DockPosition.Right, DockPosition.Fill);

                switch (terminalType)
                {
                    case TerminalWindowTypes.Raw:
                        Globals.UIElements.TerminalRawWindow = editor;
                        break;
                    case TerminalWindowTypes.Formatted:
                        Globals.UIElements.TerminalFormattedWindow = editor;
                        break;
                }

            }
        }

        public static void DockToolStrip(UserControl control, string caption, DockPosition mainDockPosition, DockPosition subDockPosition)
        {
            HostWindow hw = null;

            switch (mainDockPosition)
            {
                case DockPosition.Right:

                    if (Globals.UIElements.RightSideStrip == null)
                    {
                        hw = _radDock.DockControl(control, DockPosition.Right);
                        Globals.UIElements.RightSideStrip = (ToolTabStrip)hw.Parent;
                        ((ToolTabStrip)hw.Parent).SizeInfo.SizeMode = SplitPanelSizeMode.Absolute;
                        ((ToolTabStrip)hw.Parent).SizeInfo.AbsoluteSize = new Size(350, 0);
                    }
                    else
                    {
                        //make sure there isnt already one here..
                        if (!Globals.UIElements.RightSideStrip.Contains(control))
                        {
                            hw = _radDock.DockControl(control, Globals.UIElements.RightSideStrip, subDockPosition);
                        }
                        //Globals.UIElements.RightSideStrip = (ToolTabStrip)hw.Parent;
                    }
                    break;
                case DockPosition.Bottom:

                    if (Globals.UIElements.BottomSideStrip == null)
                    {
                        hw = _radDock.DockControl(control, DockPosition.Bottom);
                        Globals.UIElements.BottomSideStrip = (ToolTabStrip)hw.Parent;
                        ((ToolTabStrip)hw.Parent).SizeInfo.SizeMode = SplitPanelSizeMode.Absolute;
                        ((ToolTabStrip)hw.Parent).SizeInfo.AbsoluteSize = new Size(0, 250);
                    }
                    else
                    {
                        //make sure there isnt already one here..
                        if (!Globals.UIElements.BottomSideStrip.Contains(control))
                        {
                            hw = _radDock.DockControl(control, Globals.UIElements.BottomSideStrip, subDockPosition);
                        }
                        //Globals.UIElements.BottomSideStrip = (ToolTabStrip)hw.Parent;
                    }
                    break;
                case DockPosition.Left:

                    if (Globals.UIElements.LeftSideStrip == null)
                    {
                        hw = _radDock.DockControl(control, DockPosition.Left);
                        Globals.UIElements.LeftSideStrip = (ToolTabStrip)hw.Parent;
                        ((ToolTabStrip)hw.Parent).SizeInfo.SizeMode = SplitPanelSizeMode.Absolute;
                        ((ToolTabStrip)hw.Parent).SizeInfo.AbsoluteSize = new Size(350, 0);
                    }
                    else
                    {
                        //make sure there isnt already one here..
                        if (!Globals.UIElements.LeftSideStrip.Contains(control))
                        {
                            hw = _radDock.DockControl(control, Globals.UIElements.LeftSideStrip, subDockPosition);
                        }
                        //Globals.UIElements.LeftSideStrip = (ToolTabStrip)hw.Parent;
                    }
                    break;
                case DockPosition.Fill:

                    if (Globals.UIElements.FillStrip == null)
                    {
                        hw = _radDock.DockControl(control, DockPosition.Fill);
                        Globals.UIElements.FillStrip = (ToolTabStrip)hw.Parent;
                        Globals.UIElements.FillStrip.SizeInfo.SizeMode = SplitPanelSizeMode.Absolute;
                        Globals.UIElements.FillStrip.SizeInfo.AbsoluteSize = new Size(350, 0);
                    }
                    else
                    {
                        //make sure there isnt already one here..
                        if (!Globals.UIElements.FillStrip.Contains(control))
                        {
                            hw = _radDock.DockControl(control, Globals.UIElements.FillStrip, subDockPosition);
                        }
                        //Globals.UIElements.FillStrip = (ToolTabStrip)hw.Parent;
                    }
                    break;
            }

            if (hw != null)
            {
                hw.Text = caption;

                if (control.MinimumSize.Width > 0 || control.MinimumSize.Height > 0)
                {
                    DockTabStrip dockTabStrip = (DockTabStrip)hw.TabStrip;
                    dockTabStrip.SizeInfo.MinimumSize = new System.Drawing.Size(control.MinimumSize.Width, control.MinimumSize.Height);
                }
            }

        }

        public static void ShowDeveloperConsole()
        {

            string caption = "Developer Console";

            if (Globals.UIElements.DeveloperConsole == null)
            {
                Globals.UIElements.DeveloperConsole = new DeveloperConsole();
                ControlFactory.DockToolStrip(Globals.UIElements.DeveloperConsole, caption, DockPosition.Bottom, DockPosition.Right);
                Globals.UIElements.DeveloperConsole.Dock = DockStyle.Fill;
            }

            if (Globals.UIElements.DeveloperConsole != null)
            {
                HostWindow hw = _radDock.GetHostWindow(Globals.UIElements.DeveloperConsole);
                hw.Show();
                hw.Focus();
            }

        }

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
