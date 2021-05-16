using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900Link.Containers
{
    public class ProjectTest
    {
        public enum TestMode
        {
            Develop,
            ReadOnly
        }

        public TestParameterGlobals Globals = null;
        public List<ProjectLocation> Locations = new List<ProjectLocation>();
        public List<TestSequenceLocation> Sequences = new List<TestSequenceLocation>();
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string StartMessage { get; set; }
        public TestMode Mode { get; set; }
        public decimal Version { get; set; }
        public string Software { get; set; }
        public decimal SoftwareVersion { get; set; }
    }
}
