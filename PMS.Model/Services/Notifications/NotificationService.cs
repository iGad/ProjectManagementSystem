using PMS.Model.Models;

namespace PMS.Model.Services.Notifications
{
    public interface INotificationService
    {
        /// <summary>
        /// Сообщить о событии (послать все возможные уведомления)
        /// </summary>
        /// <param name="event">Произошедшее событие</param>
        void SendEventNotifications(WorkEvent @event);
    }

    /// <summary>
    /// Сервис, который запускает все зарегистрированные уведомления о возникновении события
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IUsersForEventNotificationsProvider _usersForEventProvider;
        private readonly EventNotificator[] _notificators;

        public NotificationService(IUsersForEventNotificationsProvider usersForEventProvider,
            EventNotificator[] notificators)
        {
            _usersForEventProvider = usersForEventProvider;
            _notificators = notificators;
        }

        /// <summary>
        /// Сообщить о событии (послать все возможные уведомления)
        /// </summary>
        /// <param name="event">Произошедшее событие</param>
        public void SendEventNotifications(WorkEvent @event)
        {
            var users = _usersForEventProvider.GetUsersForEventNotification(@event);
            foreach (var eventNotificator in _notificators)
            {
                eventNotificator.Notify(@event, users);
            }
        }
    }
}
