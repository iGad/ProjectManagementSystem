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
        private readonly ApplicationContext _context;

        public EventRepository(ApplicationContext context)
        {
            this._context = context;
        }

        public IEnumerable<WorkEvent> GetEvents(Func<WorkEvent, bool> filter)
        {
            return this._context.Events.Include(x => x.User).Where(filter);
        }

        public IEnumerable<Tuple<WorkEvent, WorkEventUserRelation>> GetEventsForUser(string userId)
        {
            return this._context.Events.Join(this._context.EventsUsers.Where(x => x.UserId == userId), @event => @event.Id, relation => relation.EventId,
                (@event, relation) => new {Event = @event, Relation = relation})
                .AsEnumerable()
                .Select(x => new Tuple<WorkEvent, WorkEventUserRelation>(x.Event, x.Relation));
        }

        public IEnumerable<EventUserModel> GetEventsForUser(string userId, EventFilterModel filterModel)
        {
            var query = CreateUserEventsQueryWithFiltration(userId, filterModel);
            if (filterModel.Sorting.Direction == SortingDirection.Asc)
                query = query.OrderBy(x => x.Date);
            else
                query = query.OrderByDescending(x => x.Date);

            var pageSize = filterModel.PageSize > 0 ? filterModel.PageSize : 30;
            var pageNumber = filterModel.PageNumber > 0 ? filterModel.PageNumber : 1;
            return query.Skip((pageNumber - 1)*pageSize).Take(pageSize);
        }

        public IEnumerable<EventUserModel> GetNewEventsForUser(string userId)
        {
            var query = CreateUserEventsQuery(userId).Where(x => x.State == EventState.New);
            return query;
        }

        private IQueryable<EventUserModel> CreateUserEventsQuery(string userId)
        {
            var query = this._context.Events.Join(this._context.EventsUsers.Where(x => x.UserId == userId), @event => @event.Id, relation => relation.EventId,
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
            return query;
        }

        private IQueryable<EventUserModel> CreateUserEventsQueryWithFiltration(string userId, EventFilterModel filterModel)
        {
            var query = CreateUserEventsQuery(userId).Where(x=>x.State == EventState.Seen);
            if (!string.IsNullOrWhiteSpace(filterModel.UserId))
                query = query.Where(x => x.EventCreaterId == filterModel.UserId);
            if (filterModel.IsFavorite.HasValue)
                query = query.Where(x => x.IsFavorite == filterModel.IsFavorite.Value);
            if (filterModel.ItemsIds != null)
            {
                var parsedIds = filterModel.ItemsIds.Trim().Split(',').Select(x => x.Trim()).ToArray();
                var intIds = new List<int>();
                foreach (var parsedId in parsedIds)
                {
                    int id;
                    if(int.TryParse(parsedId, out id))
                        intIds.Add(id);
                }
                if(intIds.Any())
                    query = query.Where(x => x.ObjectId.HasValue && intIds.Contains(x.ObjectId.Value));
            }
               
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
            return CreateUserEventsQueryWithFiltration(userId, filterModel).Count();
        }

        public WorkEvent AddEvent(WorkEvent workEvent)
        {
            return this._context.Events.Add(workEvent);
        }

        public WorkEvent Get(int id)
        {
            return this._context.Events.Find(id);
        }

        public void AddWorkEventRelation(WorkEventUserRelation relation)
        {
            this._context.EventsUsers.Add(relation);
        }

        public WorkEventUserRelation GetRelation(int eventId, string userId)
        {
            return this._context.EventsUsers.Find(eventId, userId);
        }

        public int GetUnseenEventCountForUser(string userId)
        {
            return this._context.EventsUsers.Count(x => x.UserId == userId && x.State == EventState.New);
        }

        public int SaveChanges()
        {
            return this._context.SaveChanges();
        }
    }
}
