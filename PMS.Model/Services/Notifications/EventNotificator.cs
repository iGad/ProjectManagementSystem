using System.Collections.Generic;
using PMS.Model.Models;

namespace PMS.Model.Services.Notifications
{
    /// <summary>
    /// Базовый класс для объекта, который каким-либо образом сообщает о возникновении события
    /// </summary>
    public abstract class EventNotificator
    {
        public abstract void Notify(WorkEvent @event, List<string> userIds);

        protected virtual bool IsUserNeedNotification(string userId)
        {
            return true;
        }
    }
}
