using PMS.Model.Models;

namespace PMS.Model.Services.Notifications
{
    /// <summary>
    /// Сервис, который запускает все зарегистрированные уведомления о возникновении события
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IEventNotificatorsUsersProvider _eventNotificatorsUsersForEventProvider;
        private readonly EventNotificator[] _notificators;

        public NotificationService(IEventNotificatorsUsersProvider eventNotificatorsUsersForEventProvider,
            EventNotificator[] notificators)
        {
            _eventNotificatorsUsersForEventProvider = eventNotificatorsUsersForEventProvider;
            _notificators = notificators;
        }

        /// <summary>
        /// Сообщить о событии (послать все возможные уведомления)
        /// </summary>
        /// <param name="event">Произошедшее событие</param>
        public void SendEventNotifications(WorkEvent @event)
        {
            var users = _eventNotificatorsUsersForEventProvider.GetUsersForEventNotification(@event);
            foreach (var eventNotificator in _notificators)
            {
                eventNotificator.Notify(@event, users);
            }
        }
    }
}
