using PMS.Model.Models;

namespace PMS.Model.Services.EventDescribers
{
    public class ItemDisappointedEventDescriber : EventDescriber
    {
        private readonly WorkItemService workItemService;
        private readonly UsersService userService;
        public ItemDisappointedEventDescriber(WorkItemService workItemService, UsersService userService)
        {
            this.workItemService = workItemService;
            this.userService = userService;
        }

        public override bool CanDescribeEventType(EventType eventType)
        {
            return eventType == EventType.WorkItemDisappointed;
        }

        protected override string GetDescription(WorkEvent workEvent, ApplicationUser forUser)
        {
            var user = this.userService.Get(workEvent.Data);
            if (!workEvent.ObjectId.HasValue)
                throw new PmsExeption("Error in event model");
            var item = this.workItemService.Get(workEvent.ObjectId.Value);
            var text = GetStartText(forUser);
            text += IsCurrentUser ? $" {NotificationResources.HaveDisappointed} " : $" {NotificationResources.Disappointed} ";
            text += $"{LexicalHelper.GetWorkItemTypeInCase(item.Type, "a")} {item.GetWorkItemIdentityText()} c пользователя {user.GetUserIdentityText()}.";
            return text;
        }
    }
}
