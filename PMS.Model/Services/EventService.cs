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
        private ApplicationUser _currentUser;
        private readonly EventDescriber[] _describers;
        public EventService(IEventRepository repository, IUserRepository userRepository, ICurrentUsernameProvider currentUsernameProvider, EventDescriber[] describers)
        {
            Repository = repository;
            UserRepository = userRepository;
            CurrentUsernameProvider = currentUsernameProvider;
            this._describers = describers;
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

        public int GetUnseenEventCountForCurrentUser()
        {
            var user = GetCurrentUser();
            return Repository.GetUnseenEventCountForUser(user.Id);
        }

        public List<EventDisplayModel> GetNewEventsForCurrentUser()
        {
            var user = GetCurrentUser();
            return Repository.GetNewEventsForUser(user.Id).Select(x => GetEventDisplayModel(x, user)).ToList();
        }

        protected WorkEventUserRelation GetEventUserRelation(int workEventId, string userId)
        {
            var relation = Repository.GetRelation(workEventId, userId);
            if (relation == null)
                throw new PmsException($"Event {workEventId} for user {userId} not found");
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
                throw new PmsException("UserId can not be empty");
        }
            
        protected ApplicationUser GetCurrentUser()
        {
            return this._currentUser ?? (this._currentUser = UserRepository.GetByUserName(CurrentUsernameProvider.GetCurrentUsername()));
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
            var describer = this._describers.Single(x => x.CanDescribeEventType(workEvent.Type));
            return describer.DescribeEvent(workEvent, forUser);
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
        

        #endregion
    }
}
