using Common.Models;

namespace Common.Services
{
    public interface INotifyService
    {
        /// <summary>
        /// Отправить оповещение о произошедшем событии
        /// </summary>
        /// <param name="eventName">Название события</param>
        /// <param name="sendedObject">Отправляемый объект</param>
        /// <param name="broadcastType">Тип отправки (кому)</param>
        /// <param name="userNames">Включенные пользователи при отправке определенным пользователям</param>
        void SendEvent(string eventName, object sendedObject, BroadcastType broadcastType, params string[] userNames);
        /// <summary>
        /// Отправить оповещение о произошедшем событии
        /// </summary>
        /// <param name="eventName">Название события</param>
        /// <param name="sendedObject">Отправляемый объект</param>
        /// <param name="broadcastType">Тип отправки (кому)</param>
        /// <param name="userNames">Включенные пользователи при отправке определенным пользователям</param>
        void SendNotification(string eventName, object sendedObject, BroadcastType broadcastType, params string[] userNames);
    }
}
