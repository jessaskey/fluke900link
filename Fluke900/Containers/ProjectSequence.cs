﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class ProjectSequence
    {
        public enum TestMode
        {
            Develop,
            ReadOnly
        }

        public List<string> ImportErrors { get; set; } = new List<string>();
        public ProjectLocation DefaultLocation = null;
        public List<ProjectLocation> Locations = new List<ProjectLocation>();
        public List<SequenceLocation> Sequences = new List<SequenceLocation>();
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string StartMessage { get; set; }
        public TestMode Mode { get; set; }
        public decimal Version { get; set; }
        public string Software { get; set; }
        public decimal SoftwareVersion { get; set; }

        public ResetDefinition Reset { get; set; }
    }
}
