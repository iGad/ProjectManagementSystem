using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using PMS.Model.CommonModels.EventModels;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationContext context;

        public EventRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public IEnumerable<WorkEvent> GetEvents(Func<WorkEvent, bool> filter)
        {
            return this.context.Events.Include(x => x.User).Where(filter);
        }

        public IEnumerable<Tuple<WorkEvent, WorkEventUserRelation>> GetEventsForUser(string userId)
        {
            return this.context.Events.Join(this.context.EventsUsers.Where(x => x.UserId == userId), @event => @event.Id, relation => relation.EventId,
                (@event, relation) => new {Event = @event, Relation = relation})
                .AsEnumerable()
                .Select(x => new Tuple<WorkEvent, WorkEventUserRelation>(x.Event, x.Relation));
        }

        public IEnumerable<EventUserModel> GetEventsForUser(string userId, EventFilterModel filterModel)
        {
            var query = CreateUserEventsQuery(userId, filterModel);
            if (filterModel.Sorting.Direction == SortingDirection.Asc)
                query = query.OrderBy(x => x.Date);
            else
                query = query.OrderByDescending(x => x.Date);
            var pageSize = filterModel.PageSize > 0 ? filterModel.PageSize : 30;
            var pageNumber = filterModel.PageNumber > 0 ? filterModel.PageNumber : 1;
            return query.Skip((pageNumber - 1)*pageSize).Take(pageSize);
        }

        private IQueryable<EventUserModel> CreateUserEventsQuery(string userId, EventFilterModel filterModel)
        {
            var query = this.context.Events.Join(this.context.EventsUsers.Where(x => x.UserId == userId), @event => @event.Id, relation => relation.EventId,
                (@event, relation) => new EventUserModel
                {
                    EventId = @event.Id,
                    EventCreaterId = @event.UserId,
                    ObjectId = @event.ObjectId,
                    ObjectStringId = @event.ObjectStringId,
                    Type = @event.Type,
                    Data = @event.Data,
                    Date = @event.CreatedDate,

                    UserId = relation.UserId,
                    State = relation.State,
                    IsFavorite = relation.IsFavorite,
                });
            if (!string.IsNullOrWhiteSpace(filterModel.UserId))
                query = query.Where(x => x.EventCreaterId == filterModel.UserId);
            if (filterModel.ItemsIds != null && filterModel.ItemsIds.Any())
                query = query.Where(x => x.ObjectId.HasValue && filterModel.ItemsIds.Contains(x.ObjectId.Value));
            if (filterModel.DateRange.Start != default(DateTime))
                query = query.Where(x => x.Date >= filterModel.DateRange.Start);
            if (filterModel.DateRange.End != default(DateTime))
            {
                if (filterModel.DateRange.Start == filterModel.DateRange.End)
                    query = query.Where(x => x.Date <= filterModel.DateRange.Start.AddDays(1).AddMilliseconds(-1));
                else
                    query = query.Where(x => x.Date <= filterModel.DateRange.End);
            }
            return query;
        } 

        public int GetTotalEventsForUserCount(string userId, EventFilterModel filterModel)
        {
            return CreateUserEventsQuery(userId, filterModel).Count();
        }

        public WorkEvent AddEvent(WorkEvent workEvent)
        {
            return this.context.Events.Add(workEvent);
        }

        public WorkEvent Get(int id)
        {
            return this.context.Events.Find(id);
        }

        public void AddWorkEventRelation(WorkEventUserRelation relation)
        {
            this.context.EventsUsers.Add(relation);
        }

        public WorkEventUserRelation GetRelation(int eventId, string userId)
        {
            return this.context.EventsUsers.Find(eventId, userId);
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
    }
}
