﻿using PMS.Model.Models;

namespace PMS.Model.Services.EventDescribers
{
    public class ItemAppointedEventDescriber : EventDescriber
    {
        private readonly WorkItemService workItemService;
        private readonly UsersService userService;
        public ItemAppointedEventDescriber(WorkItemService workItemService, UsersService userService)
        {
            this.workItemService = workItemService;
            this.userService = userService;
        }

        public override bool CanDescribeEventType(EventType eventType)
        {
            return eventType == EventType.WorkItemAppointed;
        }

        protected override string GetDescription(WorkEvent workEvent, ApplicationUser forUser)
        {
            var appointedUser = this.userService.Get(workEvent.Data);
            if(appointedUser == null)
                throw new PmsExeption("Invalid event data. Must be Id.");
            var appointedUserText = appointedUser.Id == forUser.Id
                ? (IsCurrentUser ? " себе " : " вам ")
                : " пользователю " + appointedUser.GetUserIdentityText();
            if (!workEvent.ObjectId.HasValue)
                throw new PmsExeption("Error in event model");
            var user = this.userService.Get(workEvent.UserId);
            var item = this.workItemService.GetWithNoTracking(workEvent.ObjectId.Value);
            var text = GetStartText(user);
            text += IsCurrentUser ? $" {NotificationResources.HaveAppointed} " : $" {NotificationResources.Appointed} ";
            text += appointedUserText + $"{LexicalHelper.GetWorkItemTypeInCase(item.Type, "a")} {item.GetWorkItemIdentityText()}.";
            return text;
        }
    }
}
