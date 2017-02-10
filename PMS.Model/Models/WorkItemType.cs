using Common;

namespace PMS.Model.Models
{
    public enum WorkItemType
    {
        [LocalizedDescription("Project", typeof(Resources))]
        Project = 1,
        [LocalizedDescription("Stage", typeof(Resources))]
        Stage = 2,
        [LocalizedDescription("Partition", typeof(Resources))]
        Partition = 3,
        [LocalizedDescription("Task", typeof(Resources))]
        Task = 4
    }
}
