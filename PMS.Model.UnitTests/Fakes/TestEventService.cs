using System.Collections.Generic;
using System.Linq;
using PMS.Model.CommonModels;
using PMS.Model.Models;
using PMS.Model.Services;

namespace PMS.Model.UnitTests.Fakes
{
    public class TestEventService : IEventService
    {
        public TestEventService() { }

        public TestEventService(TestEventRepository repo)
        {
            Events = repo.Events;
            EventsUsers = repo.EventsUsers;
        }
        public List<WorkEvent> Events { get; } = new List<WorkEvent>();
        public List<WorkEventUserRelation> EventsUsers { get; } = new List<WorkEventUserRelation>();
        public WorkEvent AddEvent(WorkEvent workEvent, IEnumerable<string> usersIds)
        {
            if (workEvent.Id == default(int))
                if(Events.Any())
                    workEvent.Id = this.Events.OrderByDescending(x => x.Id).First().Id + 1;
                else
                {
                    workEvent.Id = 1;
                }
            Events.Add(workEvent);
            foreach (var userId in usersIds)
            {
                EventsUsers.Add(new WorkEventUserRelation(workEvent.Id, userId));
            }
            return workEvent;
        }

        
        public void ChangeUserEventState(int workEventId, string userId, EventState newState)
        {
            var relation = EventsUsers.Single(x => x.EventId == workEventId && x.UserId == userId);
            relation.State = newState;
        }

        public void ChangeUserEventIsFavorite(int workEventId, string userId, bool isFavorite)
        {
            var relation = EventsUsers.Single(x => x.EventId == workEventId && x.UserId == userId);
            relation.IsFavorite = isFavorite;
        }

        public EventDisplayModel GetEventDisplayModel(WorkEvent workEvent, ApplicationUser user)
        {
            return new EventDisplayModel(workEvent);
        }

        public string GetEventDescription(WorkEvent workEvent, ApplicationUser forUser)
        {
            return null;
        }
    }
}
