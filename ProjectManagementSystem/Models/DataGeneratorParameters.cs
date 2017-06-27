using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementSystem.Models
{
    public class DataGeneratorParameters
    {
        public int ProjectCount { get; set; }
        public int StagesPerProjectCount { get; set; }
        public int PartitionsPerStateFrom { get; set; }
        public int PartitionsPerStateTo { get; set; }
        public int TasksPerPartitionFrom { get; set; }
        public int TasksPerPartitionTo { get; set; }
    }
}