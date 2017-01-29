using System.ComponentModel;

namespace PMS.Model.Models
{
    public enum WorkItemType
    {
        [Description("Project")]
        Project = 1,
        [Description("Stage")]
        Stage = 2,
        [Description("Partition")]
        Partition = 3,
        [Description("Task")]
        Task = 4
    }
}
