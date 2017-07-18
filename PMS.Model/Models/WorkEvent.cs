using Common.Models;

namespace PMS.Model.Models
{
    public enum EventState
    {
        New = 0,
        Seen = 1
    }

    public enum EventType
    {
        WorkItemChanged = 0,
        WorkItemStateChanged = 1,
        WorkItemAppointed = 2,
        WorkItemDisappointed = 3,
        WorkItemAdded = 4,
        WorkItemDeleted = 5,
        WorkItemCommentAdded = 10,
        User = 100
    }

    public class WorkEvent : Entity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int? ObjectId { get; set; }
        public string ObjectStringId { get; set; }
        public EventType Type { get; set; }
        public string Data { get; set; }
    }
}
