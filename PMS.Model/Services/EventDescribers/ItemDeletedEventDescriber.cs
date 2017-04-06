using PMS.Model.Models;

namespace PMS.Model.Services.EventDescribers
{
    public class ItemDeletedEventDescriber : SimpleEventDescriber
    {
        public ItemDeletedEventDescriber(WorkItemService workItemService) : base(workItemService)
        {
        }

        public override bool CanDescribeEventType(EventType eventType)
        {
            return eventType == EventType.WorkItemDeleted;
        }

        protected override string GetActionString()
        {
            return IsCurrentUser ? NotificationResources.HaveDeleted : NotificationResources.Deleted;
        }
    }
}
