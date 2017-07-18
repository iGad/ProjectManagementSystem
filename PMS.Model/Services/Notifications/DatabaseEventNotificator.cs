using System.Collections.Generic;
using System.Linq;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services.Notifications
{
    public class DatabaseEventNotificator : EventNotificator
    {
        private readonly IEventRepository _repository;

        public DatabaseEventNotificator(IEventRepository eventRepository)
        {
            _repository = eventRepository;
        }

        protected override ICollection<ApplicationUser> GetOnlyResponsableUsers(WorkEvent workEvent, ICollection<ApplicationUser> users)
        {
            if(users.Any(x=>x.Id == workEvent.UserId))
                return users;
            return users.Union(new[] {new ApplicationUser {Id = workEvent.UserId}}).ToList();
        }

        protected override void NotifyInner(WorkEvent @event, ICollection<ApplicationUser> users)
        {
            AddEvent(@event, users.Select(x => x.Id));
        }

        public WorkEvent AddEvent(WorkEvent workEvent, IEnumerable<string> usersIds)
        {
            workEvent = _repository.AddEvent(workEvent);
            foreach (var userId in usersIds)
            {
                var relation = new WorkEventUserRelation(workEvent.Id, userId);
                if (userId == workEvent.UserId)
                    relation.State = EventState.Seen;
                _repository.AddWorkEventRelation(relation);
            }
            _repository.SaveChanges();
            return workEvent;
        }
        
    }
}
