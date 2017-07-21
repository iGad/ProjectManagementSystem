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
}