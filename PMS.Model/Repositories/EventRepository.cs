using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
