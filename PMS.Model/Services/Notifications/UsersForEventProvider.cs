using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services.Notifications
{
    /// <summary>
    /// Поставщик пользователей, которым нужно сообщить о возникновении события
    /// </summary>
    public class UsersForEventNotificationsProvider
    {
        private readonly IUsersService _usersService;
        private readonly IWorkItemRepository _workItemRepository;

        public UsersForEventNotificationsProvider(IUsersService usersService, IWorkItemRepository workItemRepository)
        {
            _usersService = usersService;
            _workItemRepository = workItemRepository;
        }

        public List<string> GetUsersIdsForEventNotification(WorkEvent @event)
        {
            var userIds = new List<string>();
            return userIds;
        }
    }
}
