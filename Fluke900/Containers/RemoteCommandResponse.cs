using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fluke900;
using Fluke900.Containers;


namespace Fluke900.Containers
{
    public class RemoteCommandResponse
    {
        public RemoteCommandResponse()
        {
            Status = CommandResponseStatus.Error;
        }

        public RemoteCommandResponse(RemoteCommand command)
        {
            Command = command;
            Status = CommandResponseStatus.Error;
        }

        public byte[] RawBytes { get; set; }
        public CommandResponseStatus Status { get; set; }
        public string ErrorMessage { get; set; }
        public RemoteCommand Command { get; set; }

        public object ResultObject
        {
            get 
            {
                if (Command != null && Command.GetResultObject != null)
                {
                    return Command.GetResultObject(RawBytes);
                }
                return null;
            }
        }
        public string FormattedResult
        {
            get 
            {
                if (Command != null)
                {
                    if (Status == CommandResponseStatus.Error)
                    {
                        return "ERROR: " + ErrorMessage;
                    }
                    else
                    {
                        return Command.FormatResult(RawBytes);
                    }
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public CompilationResult AsCompilationResult()
        {
            CompilationResult result = new CompilationResult();

            result.Success = false;
            result.Error = CompilationError.Unknown;

            //first byte should be a STX
            if (this.RawBytes[0] == 0x06)
            {
                result.Success = true;
                result.Error = CompilationError.None;
            }
            else if (this.RawBytes[0] == 0x02 
                    && this.RawBytes[this.RawBytes.Length - 1] == 0x15 
                    && this.RawBytes[1] == (byte) ConsoleKey.Y)
            {
                //well structured...
                if (this.RawBytes[3] == (byte)ConsoleKey.S)
                {
                    result.Error = CompilationError.NotSpecified;
                    StringBuilder sb = new StringBuilder();
                    int lineCountIndex = 4;
                    while (this.RawBytes[lineCountIndex] != 0x0d  && lineCountIndex < this.RawBytes.Length -1)
                    {
                        sb.Append(Convert.ToChar(this.RawBytes[lineCountIndex]));
                        lineCountIndex++;
                    }
                    int errorLine = -1;
                    int.TryParse(sb.ToString(), out errorLine);
                    result.LineNumber = errorLine;

                    if (this.RawBytes[lineCountIndex] != 0x15)
                    {
                        lineCountIndex++;
                        if (lineCountIndex < this.RawBytes.Length)
                        {
                            switch (this.RawBytes[lineCountIndex])
                            {
                                case (byte)ConsoleKey.D:
                                    result.Error = CompilationError.DuplicateLabel;
                                    break;
                                case (byte)ConsoleKey.U:
                                    result.Error = CompilationError.LabelNotFound;
                                    break;
                                case (byte)ConsoleKey.N:
                                    result.Error = CompilationError.DefaultsMissing;
                                    break;
                                case (byte)ConsoleKey.L:
                                    result.Error = CompilationError.LocationMissingInSequence;
                                    break;
                            }
                        }
                    }
                }
            }


            return result;
        }

   


    }
}
