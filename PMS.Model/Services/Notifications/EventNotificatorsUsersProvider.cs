using System;
using System.Collections.Generic;
using System.Linq;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services.Notifications
{
    /// <summary>
    /// Поставщик пользователей, которым нужно сообщить о возникновении события
    /// </summary>
    public class EventNotificatorsUsersProvider : IEventNotificatorsUsersProvider
    {
        private readonly IUsersService _usersService;
        private readonly IWorkItemRepository _workItemRepository;

        public EventNotificatorsUsersProvider(IUsersService usersService, IWorkItemRepository workItemRepository)
        {
            _usersService = usersService;
            _workItemRepository = workItemRepository;
        }

        public List<ApplicationUser> GetUsersForEventNotification(WorkEvent @event)
        {
            var userIds = new List<ApplicationUser>();
            switch (@event.Type)
            {
                case EventType.WorkItemChanged:
                case EventType.WorkItemStateChanged:
                case EventType.WorkItemAdded:
                case EventType.WorkItemDeleted:
                case EventType.WorkItemCommentAdded:
                    // ReSharper disable once PossibleInvalidOperationException
                    // Проверяется перед вызовом метода
                    var workItem = _workItemRepository.GetByIdNoTracking(@event.ObjectId.Value);
                    return GetResponsibleUsers(workItem).Distinct(new ApplicationUserEqualityComparer()).ToList();
                case EventType.WorkItemAppointed:
                case EventType.WorkItemDisappointed:
                    return new List<ApplicationUser> { _usersService.Get(@event.Data) };
                case EventType.User:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return userIds;
        }

        private List<ApplicationUser> GetResponsibleUsers(WorkItem workItem, params string[] exceptUsers)
        {
            var executor = _usersService.SafeGet(workItem.ExecutorId);
            var users = new List<ApplicationUser> { _usersService.GetCurrentUser() };
            if (exceptUsers == null || executor != null && !exceptUsers.Contains(executor.Id))
                users.Add(executor);
            if (workItem.ParentId.HasValue)
            {
                var parentItemUserId = _workItemRepository.GetByIdNoTracking(workItem.ParentId.Value).ExecutorId;
                if (parentItemUserId != null)
                {
                    var parentItemExecutor = _usersService.Get(parentItemUserId);
                    if (exceptUsers == null || !exceptUsers.Contains(parentItemExecutor.Id))
                        users.Add(parentItemExecutor);
                }
            }
            var directorNames = _usersService.GetUsersByRole(RoleType.Director).ToArray();
            users.AddRange(directorNames.Where(x => users.All(u => u.Id != x.Id)));
            return users;
        }
    }
}
