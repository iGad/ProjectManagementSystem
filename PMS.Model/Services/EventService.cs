using System;
using System.Collections.Generic;
using System.Linq;
using Common.Services;
using PMS.Model.CommonModels;
using PMS.Model.CommonModels.EventModels;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class EventService : IEventService
    {
        private ApplicationUser currentUser;
        private readonly EventDescriber[] describers;
        public EventService(IEventRepository repository, IUserRepository userRepository, ICurrentUsernameProvider currentUsernameProvider, EventDescriber[] describers)
        {
            Repository = repository;
            UserRepository = userRepository;
            CurrentUsernameProvider = currentUsernameProvider;
            this.describers = describers;
        }
        protected IEventRepository Repository { get; }
        protected IUserRepository UserRepository { get; }
        protected ICurrentUsernameProvider CurrentUsernameProvider { get; }

        public WorkEvent AddEvent(WorkEvent workEvent, IEnumerable<string> usersIds)
        {
            Validate(workEvent);
            workEvent = Repository.AddEvent(workEvent);
            foreach (var userId in usersIds)
            {
                var relation = new WorkEventUserRelation(workEvent.Id, userId);
                if (userId == workEvent.UserId)
                    relation.State = EventState.Seen;
                Repository.AddWorkEventRelation(relation);
            }
            Repository.SaveChanges();
            return workEvent;
        }

        protected WorkEventUserRelation GetEventUserRelation(int workEventId, string userId)
        {
            var relation = Repository.GetRelation(workEventId, userId);
            if (relation == null)
                throw new PmsExeption($"Event {workEventId} for user {userId} not found");
            return relation;
        }

        public void ChangeUserEventState(int workEventId, string userId, EventState newState)
        {
            var relation = GetEventUserRelation(workEventId, userId);
            relation.State = newState;
            Repository.SaveChanges();
        }

        public void ChangeUserEventIsFavorite(int workEventId, string userId, bool isFavorite)
        {
            var relation = GetEventUserRelation(workEventId, userId);
            relation.IsFavorite = isFavorite;
            Repository.SaveChanges();
        }

        private void Validate(WorkEvent workEvent)
        {
            if(string.IsNullOrEmpty(workEvent.UserId))
                throw new PmsExeption("UserId can not be empty");
        }
            
        protected ApplicationUser GetCurrentUser()
        {
            return this.currentUser ?? (this.currentUser = UserRepository.GetByUserName(CurrentUsernameProvider.GetCurrentUsername()));
        }

        #region Description
        
        public EventDisplayModel GetEventDisplayModel(EventUserModel eventUserModel, ApplicationUser user)
        {
            var workEvent = ExtractEvent(eventUserModel);
            return new EventDisplayModel(eventUserModel)
            {
                Description = GetEventDescription(workEvent, user),
            };
        }

        private WorkEvent ExtractEvent(EventUserModel eventUserModel)
        {
            return new WorkEvent
            {
                Id = eventUserModel.EventId,
                UserId = eventUserModel.EventCreaterId,
                ObjectId = eventUserModel.ObjectId,
                ObjectStringId = eventUserModel.ObjectStringId,
                Type = eventUserModel.Type,
                Data = eventUserModel.Data,
                CreatedDate = eventUserModel.Date
            };
        }

        public string GetEventDescription(WorkEvent workEvent, ApplicationUser forUser)
        {
            var describer = this.describers.Single(x => x.CanDescribeEventType(workEvent.Type));
            return describer.DescribeEvent(workEvent, forUser, GetCurrentUser());
        }

        public TableCollectionModel<EventDisplayModel> GetEventsForCurrentUser(EventFilterModel filter)
        {
            var events = Repository.GetEventsForUser(GetCurrentUser().Id, filter).ToArray();
            var models = events.Select(x => GetEventDisplayModel(x, GetCurrentUser())).ToList();
            var totalCount = Repository.GetTotalEventsForUserCount(GetCurrentUser().Id, filter);
            return new TableCollectionModel<EventDisplayModel>
            {
                TotalCount = totalCount,
                Collection = models
            };
        }

        //private string GetSimpleEventText(ApplicationUser user, WorkItem item, string eventForCurrentUser, string eventName)
        //{
        //    bool isCurrentUser = GetCurrentUser().Id == user.Id;
        //    var text = isCurrentUser ? NotificationResources.You : (NotificationResources.User + " " + user.GetUserIdentityText());
        //    text += isCurrentUser ? $" {eventForCurrentUser} " : $" {eventName} ";
        //    text += $"{GetWorkItemTypeInCase(item.Type, "a")} {item.GetWorkItemIdentityText()}.";
        //    return text;
        //}


        //private string GetAppointedEventText(ApplicationUser user, WorkItem item, ApplicationUser executor)
        //{
        //    bool isCurrentUser = GetCurrentUser().Id == user.Id;
        //    var text = isCurrentUser ? NotificationResources.You : (NotificationResources.User + " " + user.GetUserIdentityText());
        //    text += isCurrentUser ? $" {NotificationResources.HaveAppointed} " : $" {NotificationResources.Appointed} ";
        //    text += $" пользователю {executor.GetUserIdentityText()} {GetWorkItemTypeInCase(item.Type, "a")} {item.GetWorkItemIdentityText()}.";
        //    return text;
        //}

        //private string GetDisappointedEventText(ApplicationUser user, WorkItem item, ApplicationUser executor)
        //{
        //    bool isCurrentUser = GetCurrentUser().Id == user.Id;
        //    var text = isCurrentUser ? NotificationResources.You : (NotificationResources.User + " " + user.GetUserIdentityText());
        //    text += isCurrentUser ? $" {NotificationResources.HaveDisappointed} " : $" {NotificationResources.Disappointed} ";
        //    text += $" {GetWorkItemTypeInCase(item.Type, "a")} {item.GetWorkItemIdentityText()} c пользователя {executor.GetUserIdentityText()}.";
        //    return text;
        //}

        //private string GetStateChangedText(ApplicationUser user, WorkItem item, WorkItemState oldState, WorkItemState newState)
        //{
        //    bool isCurrentUser = GetCurrentUser().Id == user.Id;
        //    var text = isCurrentUser ? NotificationResources.You : (NotificationResources.User + " " + user.GetUserIdentityText());
        //    bool needAddition = true;
        //    switch (newState)
        //    {
        //        case WorkItemState.Planned:
        //            text += isCurrentUser ? $" {NotificationResources.HaveMovedToPlanned} " : $" {NotificationResources.MovedToPlanned} ";
        //            break;
        //        case WorkItemState.Done:
        //            if (oldState == WorkItemState.Reviewing)
        //            {
        //                text += isCurrentUser ? $" {NotificationResources.HaveConfirmed} " : $" {NotificationResources.Confirmed} ";
        //                text += GetWorkItemTypeInCase(item.Type, "g") + " ";
        //                needAddition = false;
        //            }
        //            else
        //                text += isCurrentUser ? $" {NotificationResources.HaveFinished} " : $" {NotificationResources.Finished} ";
        //            break;
        //        case WorkItemState.Deleted:
        //            text += isCurrentUser ? $" {NotificationResources.HaveDeleted} " : $" {NotificationResources.Deleted} ";
        //            break;
        //        case WorkItemState.Archive:
        //            text += isCurrentUser ? $" {NotificationResources.HaveMovedToArchive} " : $" {NotificationResources.MovedToArchive} ";
        //            break;
        //        case WorkItemState.Reviewing:
        //            text += isCurrentUser ? $" {NotificationResources.HaveMovedToChecking} " : $" {NotificationResources.MovedToChecking} ";
        //            break;
        //        case WorkItemState.AtWork:
        //            if (oldState == WorkItemState.Planned)
        //                text += isCurrentUser ? $" {NotificationResources.HaveTook} " : $" {NotificationResources.Took} ";
        //            else
        //                text += isCurrentUser ? $" {NotificationResources.HaveMovedToRebuild} " : $" {NotificationResources.MovedToRebuild} ";
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        //    }
        //    if (needAddition)
        //        text += GetWorkItemTypeInCase(item.Type, "a") + " ";
        //    text += $"{item.GetWorkItemIdentityText()}.";
        //    return text;
        //}

        //private string GetWorkItemTypeInCase(WorkItemType type, string @case)
        //{
        //    var lowerCase = @case.ToLower();
        //    switch (type)
        //    {
        //        case WorkItemType.Project:
        //            if (lowerCase == "n" || lowerCase == "a")
        //                return NotificationResources.ProjectN.ToLower();
        //            return NotificationResources.ProjectG.ToLower();
        //        case WorkItemType.Stage:
        //            if (lowerCase == "n")
        //                return NotificationResources.StageN.ToLower();
        //            if (lowerCase == "a")
        //                return NotificationResources.StageA.ToLower();
        //            return NotificationResources.StageG.ToLower();
        //        case WorkItemType.Partition:
        //            if (lowerCase == "n" || lowerCase == "a")
        //                return NotificationResources.PartitionN.ToLower();
        //            return NotificationResources.PartitionG.ToLower();
        //        case WorkItemType.Task:
        //            if (lowerCase == "n")
        //                return NotificationResources.TaskN.ToLower();
        //            if (lowerCase == "a")
        //                return NotificationResources.TaskA.ToLower();
        //            return NotificationResources.TaskG.ToLower();
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        //    }
        //}


        #endregion
    }
}
