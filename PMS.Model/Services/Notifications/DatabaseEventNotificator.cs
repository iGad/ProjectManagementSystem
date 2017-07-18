using System.Collections.Generic;
using System.Linq;
using PMS.Model.Models;

namespace PMS.Model.Services.Notifications
{
    public class DatabaseEventNotificator : EventNotificator
    {
        private readonly IEventService _eventService;

        public DatabaseEventNotificator(IEventService eventService)
        {
            _eventService = eventService;
        }

        protected override ICollection<ApplicationUser> GetOnlyResponsableUsers(WorkEvent workEvent, ICollection<ApplicationUser> users)
        {
            return users;
        }

        protected override void NotifyInner(WorkEvent @event, ICollection<ApplicationUser> users)
        {
            _eventService.AddEvent(@event, users.Select(x => x.Id));
        }
    }
}
