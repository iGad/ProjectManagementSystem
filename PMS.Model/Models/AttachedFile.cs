using Common.Models;

namespace PMS.Model.Models
{
    public class AttachedFile : Entity
    {
        public string RelativePath { get; set; }
        public int WorkItemId { get; set; }
        public WorkItem WorkItem { get; set; }
    }
}
