using PMS.Model.Models;

namespace PMS.Model.Services.EventDescribers
{
    public class ItemChangedEventDecriber : SimpleEventDescriber
    {
        public ItemChangedEventDecriber(WorkItemService workItemService) : base(workItemService)
        {
        }

        public override bool CanDescribeEventType(EventType eventType)
        {
            return eventType == EventType.WorkItemChanged;
        }

        protected override string GetActionString()
        {
            return IsCurrentUser ? NotificationResources.HaveChanged : NotificationResources.Changed;
        }
    }
}
