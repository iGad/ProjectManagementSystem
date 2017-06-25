using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.Services.Notifications
{
    /// <summary>
    /// Сервис, который запускает все зарегистрированные уведомления о возникновении события
    /// </summary>
    public class NotificationService
    {
        private readonly UsersForEventNotificationsProvider _usersForEventProvider;
        private readonly EventNotificator[] _notificators;

        public NotificationService(UsersForEventNotificationsProvider usersForEventProvider,
            EventNotificator[] notificators)
        {
            _usersForEventProvider = usersForEventProvider;
            _notificators = notificators;
        }

        public void SendEventNotifications(WorkEvent @event)
        {
            var userIds = _usersForEventProvider.GetUsersIdsForEventNotification(@event);
            foreach (var eventNotificator in _notificators)
            {
                eventNotificator.Notify(@event, userIds);
            }
        }
    }
}
