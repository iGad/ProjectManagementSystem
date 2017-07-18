using System.Collections.Generic;
using PMS.Model.Models;

namespace PMS.Model.Services.Notifications
{
    public interface IEventNotificatorsUsersProvider
    {
        List<ApplicationUser> GetUsersForEventNotification(WorkEvent @event);
    }
}