using PMS.Model.Models;

namespace PMS.Model.Services.EventDescribers
{
    public class ItemAddedEventDescriber : SimpleEventDescriber
    {
        public ItemAddedEventDescriber(WorkItemService workItemService) : base(workItemService)
        {
        }

        public override bool CanDescribeEventType(EventType eventType)
        {
            return eventType == EventType.WorkItemAdded;
        }

        protected override string GetActionString()
        {
            return IsCurrentUser ? NotificationResources.HaveAdded : NotificationResources.Added;
        }
    }
}
