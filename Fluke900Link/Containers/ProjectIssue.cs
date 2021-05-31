using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fluke900;
using Fluke900Link.Factories;

namespace Fluke900Link.Containers
{
    public class ProjectIssue
    {
        public CommandFileErrorType ErrorType { get; set; }
        public string Filename { get; set; }
        public string Message { get; set; }
        public int LineNumber { get; set; }

        public string FileLocation
        {
            get
            {
                return Filename + ", Line " + LineNumber.ToString();
            }
        }

        public Image Image
        {
            get
            {
                switch (ErrorType)
                {
                    case CommandFileErrorType.Error:
                        return ControlFactory.ImageList16x16.Images[4];
                    case CommandFileErrorType.Warning:
                        return ControlFactory.ImageList16x16.Images[5];
                }
                return null;
            }
        }

        public ProjectIssue(CommandFileErrorType type, string filename, string message, int lineNumber)
        {
            ErrorType = type;
            Filename = filename;
            Message = message;
            LineNumber = lineNumber;
        }
    }
}
