using PMS.Model.Models;

namespace PMS.Model.Services.EventDescribers
{
    public class ItemDisappointedEventDescriber : EventDescriber
    {
        private readonly WorkItemService _workItemService;
        private readonly UsersService _userService;
        public ItemDisappointedEventDescriber(WorkItemService workItemService, UsersService userService)
        {
            this._workItemService = workItemService;
            this._userService = userService;
        }

        public override bool CanDescribeEventType(EventType eventType)
        {
            return eventType == EventType.WorkItemDisappointed;
        }

        protected override string GetDescription(WorkEvent workEvent, ApplicationUser forUser)
        {
            var user = this._userService.Get(workEvent.Data);
            if (!workEvent.ObjectId.HasValue)
                throw new PmsException("Error in event model");
            var item = this._workItemService.GetWithNoTracking(workEvent.ObjectId.Value);
            var text = GetStartText(forUser);
            text += IsUserAuthor ? $" {NotificationResources.HaveDisappointed} " : $" {NotificationResources.Disappointed} ";
            text += $"{LexicalHelper.GetWorkItemTypeInCase(item.Type, "a")} {item.GetWorkItemIdentityText()} c пользователя {user.GetUserIdentityText()}.";
            return text;
        }
    }
}
