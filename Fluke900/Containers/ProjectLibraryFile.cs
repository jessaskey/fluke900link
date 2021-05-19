using Fluke900;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{

    public enum LibraryFileType
    {
        ReferenceLibrary = 0,
        CloneLibrary = 1,
        TextLibrary = 2
    }

    public class ProjectLibraryFile : ProjectFile
    {
        public LibraryFileType LibraryType { get; set; }
        public Guid LibraryId = Guid.NewGuid();
        
        public ProjectLibraryFile()
        {
            _fileType = KnownFileType.Lib;
        }

        public ProjectLibraryFile(string pathFileName, LibraryFileType libraryType)
        {
            PathFileName = pathFileName;
            LibraryType = libraryType;
            _fileType = KnownFileType.Lib;
        }


        
    }
}
