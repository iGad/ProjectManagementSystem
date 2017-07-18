using System.Collections.Generic;
using PMS.Model.Models;
using PMS.Model.Services.Notifications;

namespace PMS.Model.UnitTests.Fakes
{
    public class TestEventNotificatorsUsersProvider : IEventNotificatorsUsersProvider
    {
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public List<ApplicationUser> GetUsersForEventNotification(WorkEvent @event)
        {
            return Users;
        }
    }
}
