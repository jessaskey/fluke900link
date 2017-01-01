using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public abstract class ProjectFile
    {
        
        protected List<string> _lines = new List<string>();
        //protected List<CommandFileGroup> _commandGroups = new List<CommandFileGroup>();
        //protected List<CommandFileError> _errors = new List<CommandFileError>();
        protected KnownFileType _fileType;

        public string PathFileName { get; set; }
        public KnownFileType FileType
        {
            get
            {
                return _fileType;
            }
        }

        public string FileNameOnly
        {
            get
            {
                return Path.GetFileName(PathFileName);
            }
        }

    }
}
