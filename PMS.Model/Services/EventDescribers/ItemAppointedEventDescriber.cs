using PMS.Model.Models;

namespace PMS.Model.Services.EventDescribers
{
    public class ItemAppointedEventDescriber : EventDescriber
    {
        private readonly WorkItemService _workItemService;
        private readonly IUsersService _userService;
        public ItemAppointedEventDescriber(WorkItemService workItemService, IUsersService userService)
        {
            _workItemService = workItemService;
            _userService = userService;
        }

        public override bool CanDescribeEventType(EventType eventType)
        {
            return eventType == EventType.WorkItemAppointed;
        }

        protected override string GetDescription(WorkEvent workEvent, ApplicationUser forUser)
        {
            var appointedUser = _userService.Get(workEvent.Data);
            if(appointedUser == null)
                throw new PmsException("Invalid event data. Must be Id.");
            var appointedUserText = appointedUser.Id == forUser.Id
                ? (IsUserAuthor ? " себе " : " вам ")
                : " пользователю " + appointedUser.GetUserIdentityText();
            if (!workEvent.ObjectId.HasValue)
                throw new PmsException("Error in event model");
            var user = _userService.Get(workEvent.UserId);
            var item = _workItemService.GetWithNoTracking(workEvent.ObjectId.Value);
            var text = GetStartText(user);
            text += IsUserAuthor ? $" {NotificationResources.HaveAppointed} " : $" {NotificationResources.Appointed} ";
            text += appointedUserText + $"{LexicalHelper.GetWorkItemTypeInCase(item.Type, "a")} {item.GetWorkItemIdentityText()}.";
            return text;
        }
    }
}
