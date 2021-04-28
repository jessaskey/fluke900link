using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class ProjectSequenceFile : ProjectFile
    {

        public ProjectSequenceFile()
        {
            _fileType = KnownFileType.Seq;
        }

        public ProjectSequenceFile(string pathFileName)
        {
            PathFileName = pathFileName;
            _fileType = KnownFileType.Seq;
        }

    }
}
