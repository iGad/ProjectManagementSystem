using System.Collections.Generic;
using PMS.Model.Models;

namespace PMS.Model.Services.Notifications
{
    /// <summary>
    /// Базовый класс для объекта, который каким-либо образом сообщает о возникновении события
    /// </summary>
    public abstract class EventNotificator
    {
        public void Notify(WorkEvent @event, ICollection<ApplicationUser> users)
        {
            @event.Validate();
            var responsableUsers = GetOnlyResponsableUsers(@event, users);
            NotifyInner(@event, responsableUsers);
        }

        protected abstract ICollection<ApplicationUser> GetOnlyResponsableUsers(WorkEvent workEvent,
            ICollection<ApplicationUser> users);

        protected abstract void NotifyInner(WorkEvent @event, ICollection<ApplicationUser> users);
        
    }
}
