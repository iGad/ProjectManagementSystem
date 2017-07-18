using System;
using System.Collections.Generic;
using System.Linq;
using PMS.Model.CommonModels.EventModels;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.UnitTests.Fakes
{
    public class TestEventRepository : IEventRepository
    {
        public List<WorkEvent> Events { get; } = new List<WorkEvent>(); 
        public List<WorkEventUserRelation> EventsUsers { get; } = new List<WorkEventUserRelation>();
        public int SaveChangesCalled { get; private set; }
        public IEnumerable<WorkEvent> GetEvents(Func<WorkEvent, bool> filter)
        {
            return Events.Where(filter);
        }

        public WorkEvent AddEvent(WorkEvent workEvent)
        {
            if (workEvent.Id == default(int))
                if (Events.Any())
                    workEvent.Id = this.Events.OrderByDescending(x => x.Id).First().Id + 1;
                else
                    workEvent.Id = 1;
            Events.Add(workEvent);
            return workEvent;
        }

        public WorkEvent Get(int id)
        {
            return Events.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Tuple<WorkEvent, WorkEventUserRelation>> GetEventsForUser(string userId)
        {
            return EventsUsers.Where(x => x.UserId == userId)
                .Join(Events, relation => relation.EventId, @event => @event.Id,
                    (relation, @event) => new Tuple<WorkEvent, WorkEventUserRelation>(@event, relation));
        }

        public IEnumerable<EventUserModel> GetEventsForUser(string userId, EventFilterModel filterModel)
        {
            var eventsUser = EventsUsers.Where(x => x.UserId == userId && x.State == EventState.Seen).ToArray();
            return Events.Where(x => eventsUser.Any(eu => eu.EventId == x.Id))
                .Select(x => new EventUserModel(x, eventsUser.Single(eu => eu.EventId == x.Id)));
        }

        public IEnumerable<EventUserModel> GetNewEventsForUser(string userId)
        {
            var eventsUser = EventsUsers.Where(x => x.UserId == userId && x.State == EventState.New).ToArray();
            return Events.Where(x => eventsUser.Any(eu => eu.EventId == x.Id))
                .Select(x => new EventUserModel(x, eventsUser.Single(eu => eu.EventId == x.Id)));
        }

        public int GetTotalEventsForUserCount(string userId, EventFilterModel filterModel)
        {
            return EventsUsers.Count(x => x.UserId == userId && x.State == EventState.Seen);
        }

        public void AddWorkEventRelation(WorkEventUserRelation relation)
        {
            EventsUsers.Add(relation);
        }

        public WorkEventUserRelation GetRelation(int eventId, string userId)
        {
            return EventsUsers.SingleOrDefault(x => x.EventId == eventId && x.UserId == userId);
        }

        public int GetUnseenEventCountForUser(string userId)
        {
            return EventsUsers.Count(x => x.UserId == userId && x.State == EventState.New);
        }

        public int SaveChanges()
        {
            SaveChangesCalled++;
            return 0;
        }
    }
}
