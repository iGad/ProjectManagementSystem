using System.Collections.Generic;
using Common.Models;
using PMS.Model.Models;

namespace PMS.Model.Services
{
    public interface IRealtimeNotificationService
    {
        /// <summary>
        /// Отправить оповещение о произошедшем событии
        /// </summary>
        /// <param name="eventName">Название события</param>
        /// <param name="sendedObject">Отправляемый объект</param>
        /// <param name="broadcastType">Тип отправки (кому)</param>
        /// <param name="userNames">Включенные пользователи при отправке определенным пользователям</param>
        void SendEvent(string eventName, object sendedObject, BroadcastType broadcastType, params string[] userNames);
    }
}
