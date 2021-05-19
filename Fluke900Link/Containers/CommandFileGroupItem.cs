using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fluke900;
using Fluke900Link.Helpers;

namespace Fluke900Link.Containers
{
    public class CommandFileGroupItem
    {
        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }
        public FileCommand Command { get; set; }
        public string CommandData { get; set; }
        public string Text { get; set; }
        public string Label {get; set;}

        public CommandFileGroupItem()
        { }

        public CommandFileGroupItem(string text)
        {
            Text = text;
            string commandString = text;
            //see if we have a label first...
            Match labelMatch = Regex.Match(text, @"^(?<label>[A-Za-z]\S*)\s+(?<command>.*)");
            if (labelMatch.Groups["label"].Success)
            {
                Label = labelMatch.Groups["label"].Value;
                if (labelMatch.Groups["command"].Success)
                {
                    commandString = labelMatch.Groups["command"].Value;
                }
            }
            else 
            {
                var parts = Regex.Matches(commandString, @"[\""].+?[\""]|[^\W]+").Cast<Match>().Select(m => m.Value).ToList();

                FileCommand? cmd = CommandFileParser.ParseCommand(parts[0]);
                if (cmd.HasValue)
                {
                    Command = cmd.Value;
                }
                else
                {
                    throw new Exception("Unknown command: " + parts[0]);
                }

                if (parts.Count > 1)
                {
                    CommandData = parts[1];
                }
            }
        }
    }
}
