using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class FileCommandCompatibility
    {
        public FileCommand Command { get; set; }

        public bool ValidInSequenceFile { get; set; }
        public bool ValidInLocationFile { get; set; }
        public bool ValidInLibraryFile { get; set; }



        public FileCommandCompatibility ()
        {}

        public FileCommandCompatibility(FileCommand command, bool validInSequence, bool validInLocation, bool validInLibrary)
        {
            Command = command;
            ValidInSequenceFile = validInSequence;
            ValidInLocationFile = validInLocation;
            ValidInLibraryFile = validInLibrary; 
        }
    }

    public static class LibraryFileCommands
    {
        private static List<FileCommandCompatibility> _commandCompatibility = new List<FileCommandCompatibility>() { 
            new FileCommandCompatibility(FileCommand.ACTIVITY,      false, true, true ),
            new FileCommandCompatibility(FileCommand.CLIP_CHK,      false, true, true ),
            new FileCommandCompatibility(FileCommand.COMMENT,       true, false, false ),
            new FileCommandCompatibility(FileCommand.COMPARE,       false, true, false ),
            new FileCommandCompatibility(FileCommand.C_SUM,         false, true, true ),
            new FileCommandCompatibility(FileCommand.DISPLAY,       true, true, false ),
            new FileCommandCompatibility(FileCommand.END,           true, false, false ),
            new FileCommandCompatibility(FileCommand.END_FUNCTION,  true, false, false ),
            new FileCommandCompatibility(FileCommand.F_MASK,        false, true, false ),
            new FileCommandCompatibility(FileCommand.FUNCTION,      true, false, false ),
            new FileCommandCompatibility(FileCommand.GATE,          false, true, true ),
            new FileCommandCompatibility(FileCommand.GATE_DELAY,    false, true, true ),
            new FileCommandCompatibility(FileCommand.IF,            true, false, false ),
            new FileCommandCompatibility(FileCommand.IGNORE,        false, true, true ),
            new FileCommandCompatibility(FileCommand.JUMP,          true, false, false ),
            new FileCommandCompatibility(FileCommand.LOAD,          false, true, true ),
            new FileCommandCompatibility(FileCommand.LOC_FILE,      true, false, false ),
            new FileCommandCompatibility(FileCommand.NAME,          false, true, true ),
            new FileCommandCompatibility(FileCommand.RD_DRV,        false, true, true ),
            new FileCommandCompatibility(FileCommand.RDSIM,         false, true, true ),
            new FileCommandCompatibility(FileCommand.RDT_ENABLE,    false, true, true ),
            new FileCommandCompatibility(FileCommand.RDTEST,        false, false, true ),
            new FileCommandCompatibility(FileCommand.RESET,         false, true, false ),
            new FileCommandCompatibility(FileCommand.SHADOW,        false, false, true ),
            new FileCommandCompatibility(FileCommand.SIZE,          false, true, true ),
            new FileCommandCompatibility(FileCommand.SOUND,         true, false, false ),
            new FileCommandCompatibility(FileCommand.S_TIME,        false, true, true ),
            new FileCommandCompatibility(FileCommand.SYNC_COND,     false, false, true),
            new FileCommandCompatibility(FileCommand.SYNC_GATE,     false, false, true ),
            new FileCommandCompatibility(FileCommand.SYNC_GR_END,   false, false, true ),
            new FileCommandCompatibility(FileCommand.SYNC_IGNORE,   false, false, true ),
            new FileCommandCompatibility(FileCommand.SYNC_PAT,      false, false, true ),
            new FileCommandCompatibility(FileCommand.SYNC_PINS,     false, false, true ),
            new FileCommandCompatibility(FileCommand.SYNC_RESET_OFF,false, false, true ),
            new FileCommandCompatibility(FileCommand.SYNC_RND,      false, false, true ),
            new FileCommandCompatibility(FileCommand.SYNC_VECT,     false, false, true ),
            new FileCommandCompatibility(FileCommand.TEST,          true, false, false ),
            new FileCommandCompatibility(FileCommand.THRSLD,        false, true, true ),
            new FileCommandCompatibility(FileCommand.TRIGGER,       false, true, false ),
            new FileCommandCompatibility(FileCommand.T_TIME,        false, true, false )
        };

        /// <summary>
        /// Returns a list of FileCommand enums that are valid for the passed filetype.
        /// </summary>
        /// <param name="fileType">The known filetype that you wish to know the valid commands for.</param>
        /// <returns>A list of Valid FileCommand objects</returns>
        public static List<FileCommand> GetValidCommands(KnownFileType fileType)
        {
            List<FileCommand> emptyCommands = new List<FileCommand>();
            switch (fileType)
            {
                case KnownFileType.Lib:
                    return _commandCompatibility.Where(cc => cc.ValidInLibraryFile).Select(cc => cc.Command).ToList();
                case KnownFileType.Loc:
                    return _commandCompatibility.Where(cc => cc.ValidInLocationFile).Select(cc => cc.Command).ToList();
                case KnownFileType.Seq:
                    return _commandCompatibility.Where(cc => cc.ValidInSequenceFile).Select(cc => cc.Command).ToList();
            }
            return emptyCommands;
        }

        /// <summary>
        /// Returns a list of FileCommand enums that are *invalid* for the passed filetype.
        /// </summary>
        /// <param name="fileType">The known filetype that you wish to know the *invalid* commands for.</param>
        /// <returns>A list of Invalid FileCommand objects</returns>
        public static List<FileCommand> GetInvalidCommands(KnownFileType fileType)
        {
            return _commandCompatibility.Where(cc => !GetValidCommands(fileType).Contains(cc.Command)).Select(cc => cc.Command).ToList();
        }

    }
}
