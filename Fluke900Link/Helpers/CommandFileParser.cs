using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Fluke900Link.Containers;

namespace Fluke900Link.Helpers
{
    public static class CommandFileParser
    {
        public static CommandFile Load(ProjectFile projectFile, LogIssueHandler issueHandler, LogMessageHandler messageHandler)
        {
            CommandFile commandFile = new CommandFile();
            commandFile.ProjectFile = projectFile;
            
            if (File.Exists(projectFile.PathFileName))
            {
                messageHandler("Loading '" + projectFile.FileNameOnly + "'...");

                string[] lines = File.ReadAllLines(projectFile.PathFileName);
                int lineIndex = 0;
                while (lineIndex < lines.Length)
                {
                    try
                    {
                        lineIndex += GetNextCommandFileGroup(lines, lineIndex, commandFile.FileGroups);
                    }
                    catch (Exception ex)
                    {
                        Globals.Exceptions.Add(new AppException(ex));
                        commandFile.HasErrors = true;
                        issueHandler(new ProjectIssue(CommandFileErrorType.Error, projectFile.FileNameOnly, ex.Message, lineIndex));
                        break;
                    }
                }
                messageHandler("Finished '" + projectFile.FileNameOnly + "'.");
            }
            else
            {
                commandFile.HasErrors = true;
                issueHandler(new ProjectIssue(CommandFileErrorType.Error, projectFile.FileNameOnly, "File not found: '" + projectFile.PathFileName + "'", 0));
            }
            return commandFile;
        }

        public static List<CommandFileError> DependencyCheck(List<CommandFile> commandFiles, LogIssueHandler issueHandler, LogMessageHandler messageHandler, bool checkReferenceLibrary)
        {
            List<CommandFileError> issues = new List<CommandFileError>();

            //make sure all locations reference valid libraries
            foreach (CommandFile file in commandFiles.Where(c => c.ProjectFile.FileType == KnownFileType.Loc))
            {
                messageHandler("Checking '" + file.ProjectFile.FileNameOnly + "' for LOAD dependecy errors.");
                //get all the load commands here...
                foreach(CommandFileGroupItem loadCommands in file.FileGroups.SelectMany(g=>g.CommandItems.Where(i=>i.Command == FileCommand.LOAD)))
                {
                    string deviceName = loadCommands.CommandData;
                    //now see if the loaded library exists
                    //first in user libraries
                    if (commandFiles.Where(c => c.ProjectFile.FileType == KnownFileType.Lib)
                                        .SelectMany(f => f.FileGroups.SelectMany(g => g.CommandItems
                                            .Where(i => i.Command == FileCommand.NAME && i.CommandData == deviceName)))
                                            .FirstOrDefault() == null)
                    {
                        //if not in a user library, then look in the Reference Library
                        bool referenceLibraryHasDevice = LibraryHelper.HasDevice(deviceName);

                        if (referenceLibraryHasDevice && !checkReferenceLibrary)
                        {
                            file.HasErrors = true;
                            issueHandler(new ProjectIssue(CommandFileErrorType.Error, file.ProjectFile.FileNameOnly, "Device library '" + loadCommands.CommandData + "' was not found in user libraries but exists in Reference library. Enable the project to autogenerate reference device libraries or create a user library for the missing device.", loadCommands.LineNumber));
                        }

                        if (!referenceLibraryHasDevice)
                        {
                            file.HasErrors = true;
                            issueHandler(new ProjectIssue(CommandFileErrorType.Error, file.ProjectFile.FileNameOnly, "Referenced device library '" + loadCommands.CommandData + "' was not found in any User Library files or Reference Library files.", loadCommands.LineNumber));
                        }
                    }
                }
                messageHandler(" -- Finished");
            }

            //make sure all locations reference valid libraries
            foreach (CommandFile file in commandFiles.Where(c => c.ProjectFile.FileType == KnownFileType.Seq))
            {
                messageHandler("Checking sequence '" + file.ProjectFile.FileNameOnly + "' for LOC_FILE dependecy errors.");
                //get all the load commands here...
                foreach (CommandFileGroupItem loadCommands in file.FileGroups.SelectMany(g => g.CommandItems.Where(i => i.Command == FileCommand.LOC_FILE)))
                {
                    string locFile = loadCommands.CommandData.ToLower();
                    //does this projectfile exist?
                    if(commandFiles.Where(c => c.ProjectFile.FileType == KnownFileType.Loc && c.ProjectFile.FileNameOnly.ToLower().Replace(".loc","") == locFile).FirstOrDefault() == null)
                    {
                        file.HasErrors = true;
                        issueHandler(new ProjectIssue(CommandFileErrorType.Error, file.ProjectFile.FileNameOnly, "Referenced location file '" + loadCommands.CommandData + "' was not found in the current project.", loadCommands.LineNumber));
                    }
                }
                messageHandler(" -- Finished");
            }
            return issues;
        }


        public static bool ParseFile(CommandFile commandFile)
        {
            bool errors = false;





            return errors;
        }

        private static int GetNextCommandFileGroup(string[]lines, int index, List<CommandFileGroup> groups)
        {
            int startIndex = index;
            bool groupEnded = false;
            List<Tuple<string,int>> cleanLines = new List<Tuple<string, int>>();
            while(index < lines.Length && !groupEnded)
            {
                string withoutComments = Regex.Replace(lines[index], ";(.*)", "", RegexOptions.Singleline);

                if (!String.IsNullOrEmpty(withoutComments))
                {
                    if (withoutComments.Contains(":"))
                    {
                        groupEnded = true;
                    }
                    if (!String.IsNullOrWhiteSpace(withoutComments) && !String.IsNullOrEmpty(withoutComments))
                    {
                        cleanLines.Add(new Tuple<string, int>(withoutComments, index));
                    }
                }

                index++;
            }

            int totalLines = (index - startIndex) + 1;
            CommandFileGroup group = new CommandFileGroup();
            foreach(Tuple<string,int> cleanLine in cleanLines)
            {
                //make sure this isn't a standalone colon line, we don't add these
                if (!Regex.Match(cleanLine.Item1, @"^\s*?:+?\s*?").Success)
                {
                    CommandFileGroupItem item = new CommandFileGroupItem(cleanLine.Item1);
                    item.LineNumber = cleanLine.Item2;
                    group.CommandItems.Add(item);
                }
            }
            groups.Add(group);
            return totalLines;
        }

        public static FileCommand? ParseCommand(string inputString)
        {
            FileCommand command = FileCommand.ACTIVITY;
            if (Enum.TryParse<FileCommand>(inputString.ToUpper(), true, out command))
            {
                return command;
            }
            return null;
        }

    }
}
