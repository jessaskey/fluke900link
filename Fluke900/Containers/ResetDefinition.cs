using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class ResetDefinition
    {
        public string Polarity { get; set; }
        public string Source { get; set; }
        public int NegativeOffset { get; set; }
        public int Duration { get; set; }

        public ResetDefinition() {
            //defaults
            Polarity = "P";
            Source = "I";
            NegativeOffset = -200;
            Duration = 500;
        }

        public ResetDefinition(List<string> parameters)
        {
            Polarity = parameters[0];
            Source = parameters[1];
            NegativeOffset = int.Parse(parameters[2].Replace("--","-"));
            Duration = int.Parse(parameters[3]);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Polarity);
            sb.Append("\r");
            sb.Append(Source);
            sb.Append("\r-");
            sb.Append(NegativeOffset.ToString());
            sb.Append("\r");
            sb.Append(Duration.ToString());
            return sb.ToString();
        }
    }
}
