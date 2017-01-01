using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class ProjectLocationFile : ProjectFile
    {
        public ProjectLocationFile()
        {
            _fileType = KnownFileType.Loc;
        }

        public ProjectLocationFile(string pathFileName)
        {
            PathFileName = pathFileName;
            _fileType = KnownFileType.Loc;
        }

    }
}
