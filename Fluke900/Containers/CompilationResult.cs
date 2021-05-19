using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class CompilationResult
    {
        public bool Success { get; set; }
        public CompilationError Error { get; set;}
        public int LineNumber { get; set; }

        public string GetSummary()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Line:  " + LineNumber.ToString());
            sb.AppendLine("Error: " + Enum.GetName(typeof(CompilationError), Error));
            return sb.ToString();
        }
    }
}
