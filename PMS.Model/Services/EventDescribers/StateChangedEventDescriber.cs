using System;
using Newtonsoft.Json;
using PMS.Model.CommonModels.EventModels;
using PMS.Model.Models;

namespace PMS.Model.Services.EventDescribers
{
    public class StateChangedEventDescriber : EventDescriber
    {
        private readonly WorkItemService _workItemService;
        private readonly UsersService _userService;
        private bool _isUserExecutor;
        public StateChangedEventDescriber(WorkItemService workItemService, UsersService userService)
        {
            _workItemService = workItemService;
            _userService = userService;
        }

        public override bool CanDescribeEventType(EventType eventType)
        {
            return eventType == EventType.WorkItemStateChanged;
        }

        protected override string GetDescription(WorkEvent workEvent, ApplicationUser forUser)
        {
            var stateChangedModel = JsonConvert.DeserializeObject<StateChangedModel>(workEvent.Data);
            if(!workEvent.ObjectId.HasValue)
                throw new PmsException("Error in event model");
            var item = _workItemService.GetWithNoTracking(workEvent.ObjectId.Value);
            _isUserExecutor = workEvent.UserId == item.ExecutorId;
            var user = _userService.Get(workEvent.UserId);
            var text = GetStartText(user);
            bool needAddition = true;
            text += GetTextForStateChanging(stateChangedModel, item, ref needAddition);
            if (needAddition)
                text += LexicalHelper.GetWorkItemTypeInCase(item.Type, "a") + " ";
            text += item.GetWorkItemIdentityText()+ ".";
            return text;
        }

        private string GetTextForStateChanging(StateChangedModel stateChangedModel, WorkItem item, ref bool needAddition)
        {
            var oldState = stateChangedModel.Old;
            var newState = stateChangedModel.New;
            var text = "";
            switch (newState)
            {
                case WorkItemState.New:
                case WorkItemState.Planned:
                    text += IsUserAuthor ? $" {NotificationResources.HaveMovedToPlanned} " : $" {NotificationResources.MovedToPlanned} ";
                    break;
                case WorkItemState.Done:
                    if (oldState == WorkItemState.Reviewing)
                    {
                        text += IsUserAuthor ? $" {NotificationResources.HaveConfirmed} " : $" {NotificationResources.Confirmed} ";
                        text += LexicalHelper.GetWorkItemTypeInCase(item.Type, "g") + " ";
                        needAddition = false;
                    }
                    else
                    {
                        text += IsUserAuthor ? $" {NotificationResources.HaveFinished} " : $" {NotificationResources.Finished} ";
                    }
                    break;
                case WorkItemState.Deleted:
                    text += IsUserAuthor ? $" {NotificationResources.HaveDeleted} " : $" {NotificationResources.Deleted} ";
                    break;
                case WorkItemState.Archive:
                    text += IsUserAuthor ? $" {NotificationResources.HaveMovedToArchive} " : $" {NotificationResources.MovedToArchive} ";
                    break;
                case WorkItemState.Reviewing:
                    if (_isUserExecutor)
                        text += IsUserAuthor ? $" {NotificationResources.HaveMovedToChecking} " : $" {NotificationResources.MovedToChecking} ";
                    else
                    {
                        text += IsUserAuthor ? $" {NotificationResources.HaveMoved} " : $" {NotificationResources.Moved} ";
                        text += NotificationResources.ToChecking + " ";
                    }
                    break;
                case WorkItemState.AtWork:
                    if (oldState == WorkItemState.Planned)
                    {
                        if (_isUserExecutor)
                            text += IsUserAuthor ? $" {NotificationResources.HaveTook} " : $" {NotificationResources.Took} ";
                        else
                        {
                            text += IsUserAuthor ? $" {NotificationResources.HaveMoved} " : $" {NotificationResources.Moved} ";
                            text += NotificationResources.ToWork + " ";
                        }
                    }
                    else
                    {
                        if (_isUserExecutor)
                            text += IsUserAuthor ? $" {NotificationResources.HaveMovedToRebuild} " : $" {NotificationResources.MovedToRebuild} ";
                        else
                        {
                            text += IsUserAuthor ? $" {NotificationResources.HaveMoved} " : $" {NotificationResources.Moved} ";
                            text += NotificationResources.ToRebuild + " ";
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
            return text;
        }

        
    }
}
