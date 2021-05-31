using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fluke900;
using Fluke900.Containers;
using Fluke900.Helpers;
using Fluke900Link.Controllers;
using Fluke900Link.Dialogs;
using Fluke900Link.Helpers;

namespace Fluke900Link.Containers
{
    public static class ProjectFactory
    {
        public static Project CurrentProject = null;
        private static List<CommandFile> _commandFiles = new List<CommandFile>();

        public static bool HasCommandErrors
        {
            get
            {
                return _commandFiles.Where(f => f.HasErrors).FirstOrDefault() != null;
            }
        }

        public static Project LoadProject(string projectFile)
        {
            if (File.Exists(projectFile))
            {
                Project project = null;
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Project));
                using (FileStream stream = File.Open(projectFile, FileMode.Open))
                {
                    project = x.Deserialize(stream) as Project;
                    stream.Close();
                }
                return project;
            }
            return null;
        }

        public static bool SaveProject()
        {
            if (CurrentProject != null)
            {
                if (File.Exists(CurrentProject.ProjectPathFile))
                {
                    File.Delete(CurrentProject.ProjectPathFile);
                }
                CurrentProject.IsModified = false;
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(CurrentProject.GetType());
                System.IO.FileStream file = System.IO.File.Create(CurrentProject.ProjectPathFile);
                x.Serialize(file, CurrentProject);
                file.Close();
                return true;
            }
            return false;
        }

        public static bool SaveProjectAs(string projectFilePath)
        {
            if (CurrentProject != null)
            {
                if (File.Exists(projectFilePath))
                {
                    File.Delete(projectFilePath);
                }
                CurrentProject.ProjectPathFile = projectFilePath;
                SaveProject();
                return true;
            }
            return false;
        }

        public static ProjectTest ImportZSQFile()
        {
            ProjectTest importedTest = null;
            ImportZSQDialog zd = new ImportZSQDialog();
            DialogResult dr = zd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (CurrentProject != null)
                {
                    importedTest = zd.ImportedProjectTest;
                    CurrentProject.Tests.Add(importedTest);
                }
            }
            return importedTest;
        }

        public static bool ParseProjectCommands(LogIssueHandler issueHandler, LogMessageHandler messageHandler)
        {
            
            if (CurrentProject != null)
            {
                _commandFiles.Clear();
                //load everything first...
                foreach (ProjectFile file in CurrentProject.Files)
                {
                    messageHandler("Checking file: " + file.FileNameOnly);
                    CommandFile f = CommandFileParser.Load(file, issueHandler, messageHandler);
                    _commandFiles.Add(f);
                }
                //look for dependency errors
                CommandFileParser.DependencyCheck(_commandFiles, issueHandler, messageHandler, CurrentProject.AutoBuildDeviceLibraries);
                return true;
            }
            return false; ;
        }      

        public static bool GenerateLibraries()
        {
            bool success = false;
            if (CurrentProject != null)
            {
                if (CurrentProject.AutoBuildDeviceLibraries)
                {
                    string projectPath = Path.GetDirectoryName(CurrentProject.ProjectPathFile);
                    if (Directory.Exists(projectPath))
                    {
                        string projectAutogenLibraryFileName = Path.Combine(projectPath, Properties.Settings.Default.AutoLibraryFilename) + ".LI@";
                        if (File.Exists(projectAutogenLibraryFileName))
                        {
                            File.Delete(projectAutogenLibraryFileName);
                        }
                        List<string> projectDevices = GetProjectReferencedDevices();
                        if (projectDevices.Count > 0)
                        {
                            List<byte> deviceLibraries = LibraryHelper.GetDeviceLibraries(projectDevices, LibraryFileFormat.ASCIIEncodedBinary);
                            //we have the bytes now... 
                            //write them
                            File.WriteAllBytes(projectAutogenLibraryFileName, deviceLibraries.ToArray());
                        }
                        success = true;
                    }
                    else
                    {
                        MessageBox.Show("Project folder is missing or not accessible.", "Project Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else{
                MessageBox.Show("There does not seem to be a project open.", "Project Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return success;
        }

        public static List<string> GetProjectReferencedDevices()
        {
            List<string> deviceNames = new List<string>();
            foreach (CommandFile cf in _commandFiles)
            {
                foreach (CommandFileGroupItem item in cf.FileGroups.SelectMany(g => g.CommandItems.Where(i => i.Command == FileCommand.LOAD)))
                {
                    if (!deviceNames.Contains(item.CommandData))
                    {
                        deviceNames.Add(item.CommandData);
                    }
                }
            }
            return deviceNames.Distinct().ToList();
        }

        public static List<Tuple<string,bool>> CopyFilesToFluke(bool copyAutogenLibrary)
        {

            //Our return container is fileName and compileFlag
            List<Tuple<string,bool>> flukeFiles = new List<Tuple<string,bool>>();

            if (CurrentProject != null)
            {
                if (FlukeController.IsConnected)
                {
                    //turn this into strings
                    //sourceFile, destFile, compileFlag
                    List<Tuple<string, string, bool>> filesToCopy = CurrentProject.Files.Select(f => new Tuple<string,string, bool>( f.PathFileName, f.PathFileName, true)).ToList();

                    if (copyAutogenLibrary)
                    {
                        //this means we need to also copy the autogen library if it exists..
                        string projectPath = Path.GetDirectoryName(CurrentProject.ProjectPathFile);
                        string projectAutogenLibraryFileName = Path.Combine(projectPath, Properties.Settings.Default.AutoLibraryFilename) + ".LI@";
                        string sourceFilename = projectAutogenLibraryFileName;
                        //string destinationFilename = FileHelper.AdjustForTransfer(sourceFilename);
                        filesToCopy.Add(new Tuple<string, string, bool>(sourceFilename, sourceFilename, false));
                    }

                    foreach (Tuple<string,string, bool> fileData in filesToCopy)
                    {
                        FileLocationCopyBehavior copyBehavior = CurrentProject.FileLocationCopyBehavior;

                        try
                        {
                            //since different file location may go to different places on the Fluke, we need
                            //to see what the project is configured for...
                            FileLocations destinationLocation = GetFileDestinationLocation(fileData.Item2);
                            //copy each file over to the Cartridge for now..
                            //TODO: Probably need to delete files if they already exist?
                            string sourceFilename = FileHelper.AppendLocation(fileData.Item1, FileLocations.LocalComputer);
                            string destinationFilename = FileHelper.AppendLocation(Path.GetFileName(fileData.Item2), destinationLocation);
                            int count = 0;
                            Task.Run(async () => { count = await FlukeController.TransferFile(sourceFilename, destinationFilename); }).Wait();
                            //int count = FlukeController.TransferFile(sourceFilename, destinationFilename);
                            if (count > 0)
                            {
                                flukeFiles.Add(new Tuple<string, bool>(destinationFilename, fileData.Item3));
                            }
                        }
                        catch (Exception ex)
                        {
                            ApplicationGlobals.Exceptions.Add(new AppException(ex));
                            MessageBox.Show("Error copying file '" + Path.GetFileName(fileData.Item1) + "' to Fluke '" + Path.GetFileName(fileData.Item2) + "': " + ex.Message, "File Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    //if (copyAutogenLibrary)
                    //{
                    //    //this means we need to also copy the autogen library if it exists..
                    //    string projectPath = Path.GetDirectoryName(CurrentProject.ProjectPathFile);
                    //    string projectAutogenLibraryFileName = Path.Combine(projectPath, Properties.Settings.Default.AutoLibraryFilename) + ".LI@";
                    //    string sourceFilename = FileHelper.AppendLocation(projectAutogenLibraryFileName, FileLocations.LocalComputer);
                    //    string destinationFilename = FileHelper.AppendLocation(Path.GetFileName(projectAutogenLibraryFileName), FileLocations.FlukeCartridge);
                    //    int count = FlukeController.TransferFile(sourceFilename, destinationFilename);
                    //    if (count > 0)
                    //    {
                    //        flukeFiles.Add(destinationFilename);
                    //    }
                    //}
                }
                else
                {
                    MessageBox.Show("Fluke is not connected. Connect and try again.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("There does not seem to be a project open.", "Project Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return flukeFiles;
        }

        public static FileLocations GetFileDestinationLocation(string filename)
        {
            FileLocations returnLocation = FileLocations.FlukeSystem;

            switch(CurrentProject.FileLocationCopyBehavior)
            {
                case FileLocationCopyBehavior.System:
                    returnLocation = FileLocations.FlukeSystem;
                    break;
                case FileLocationCopyBehavior.Cartridge:
                    returnLocation = FileLocations.FlukeCartridge;
                    break;
                //these two are the same for now... we will get smarter later where we look at sizes of files
                //and storage available.
                case FileLocationCopyBehavior.SystemCartridgeDefault:
                case FileLocationCopyBehavior.Optimized:

                    //depends in the filetype on the destination now
                    KnownFileType? knownFiletype = FileHelper.FilenameToKnownFileType(filename);

                    if (knownFiletype.HasValue)
                    {
                        switch (knownFiletype)
                        {
                            case KnownFileType.Lib:
                                returnLocation = FileLocations.FlukeCartridge;
                                break;
                            case KnownFileType.Loc:
                            case KnownFileType.Seq:
                            default:
                                returnLocation = FileLocations.FlukeSystem;
                                break;
                        }
                    }
                    break;
            }
            return returnLocation;

        }

        public static bool CompileProjectFiles(List<string> flukeFiles)
        {
            bool success = false;

            if (CurrentProject != null)
            {
                if (FlukeController.IsConnected)
                {
                    foreach (string file in flukeFiles)
                    {
                        try
                        {
                            Task.Run(async () => { await FlukeController.CompileFile(file); }).Wait();
                            //FlukeController.CompileFile(file);
                        }
                        catch (Exception ex)
                        {
                            ApplicationGlobals.Exceptions.Add(new AppException(ex));
                            MessageBox.Show("Error compiling file '" + file + "' on Fluke: " + ex.Message, "File Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    success = true;
                }
                else
                {
                    MessageBox.Show("Fluke is not connected. Connect and try again.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("There does not seem to be a project open.", "Project Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return success;
        }
        
    }
}
