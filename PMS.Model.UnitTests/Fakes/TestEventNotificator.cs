using System.Collections.Generic;
using PMS.Model.Models;
using PMS.Model.Services.Notifications;

namespace PMS.Model.UnitTests.Fakes
{
    public class TestEventNotificator : EventNotificator
    {
        public readonly Dictionary<WorkEvent, ICollection<ApplicationUser>> NotifiedEvents = new Dictionary<WorkEvent, ICollection<ApplicationUser>>();
        protected override ICollection<ApplicationUser> GetOnlyResponsableUsers(WorkEvent workEvent, ICollection<ApplicationUser> users)
        {
            return users;
        }

        protected override void NotifyInner(WorkEvent @event, ICollection<ApplicationUser> users)
        {
            NotifiedEvents.Add(@event, users);
        }
    }
}
