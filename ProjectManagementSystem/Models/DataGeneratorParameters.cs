using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementSystem.Models
{
    public class DataGeneratorParameters
    {
        public int ProjectCount { get; set; } = 1;
        public int StagesPerProjectCount { get; set; } = 1;
        public int PartitionsPerStageFrom { get; set; } = 1;
        public int PartitionsPerStageTo { get; set; } = 1;
        public int TasksPerPartitionFrom { get; set; } = 1;
        public int TasksPerPartitionTo { get; set; } = 1;
    }
}