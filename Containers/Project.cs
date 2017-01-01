using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fluke900Link.Helpers;

namespace Fluke900Link.Containers
{
    public class Project
    {
        //private List<string> _files = new List<string>();

        private List<ProjectLibraryFile> _libraryFiles = new List<ProjectLibraryFile>();
        private List<ProjectLocationFile> _locationFiles = new List<ProjectLocationFile>();
        private List<ProjectSequenceFile> _sequenceFiles = new List<ProjectSequenceFile>();

        public string ProjectPathFile { get; set; }
        public bool IsModified { get; set; }
        public bool AutoBuildDeviceLibraries { get; set; }
        public bool IncludeSimulationData { get; set; }
        public FileLocationCopyBehavior FileLocationCopyBehavior { get; set; }


        public List<ProjectFile> Files 
        { 
            get 
            {
                List<ProjectFile> allFiles = new List<ProjectFile>();

                allFiles.AddRange(_libraryFiles.AsEnumerable<ProjectFile>().ToList());
                allFiles.AddRange(_locationFiles.AsEnumerable<ProjectFile>().ToList());
                allFiles.AddRange(_sequenceFiles.AsEnumerable<ProjectFile>().ToList());
                return allFiles.OrderBy(p => p.FileNameOnly).ToList();
            } 
        }

        public List<ProjectLibraryFile> Libraries
        {
            get
            {
                return _libraryFiles;
            }
            set
            {
                _libraryFiles = value;
            }
        }

        public List<ProjectLocationFile> Locations
        {
            get
            {
                return _locationFiles;
            }
            set
            {
                _locationFiles = value;
            }
        }

        public List<ProjectSequenceFile> Sequences
        {
            get
            {
                return _sequenceFiles;
            }
            set
            {
                _sequenceFiles = value;
            }
        }

        public bool AddFile(string pathFileName)
        {
            KnownFileType? filetype = FileHelper.FilenameToKnownFileType(pathFileName);
            if (filetype.HasValue)
            {
                switch (filetype.Value)
                {
                    case KnownFileType.Lib:
                        Libraries.Add(new ProjectLibraryFile(pathFileName, LibraryFileType.TextLibrary));
                        break;
                    case KnownFileType.Loc:
                        Locations.Add(new ProjectLocationFile(pathFileName));
                        break;
                    case KnownFileType.Seq:
                        Sequences.Add(new ProjectSequenceFile(pathFileName));
                        break;
                    default:
                        return false;
                }
                return true;
            }
            return false;
        }

        public bool RemoveFileFromCurrentProject(string fileName)
        {
            if (Path.GetExtension(fileName.ToLower()) == ".lib")
            {
                return _libraryFiles.RemoveAll(l => l.FileNameOnly.ToUpper() == fileName.ToUpper()) > 0;
            }
            return false;
        }
    }


}
