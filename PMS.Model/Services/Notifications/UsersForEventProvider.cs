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
    public class UsersForEventNotificationsProvider : IUsersForEventNotificationsProvider
    {
        private readonly IUsersService _usersService;
        private readonly IWorkItemRepository _workItemRepository;

        public UsersForEventNotificationsProvider(IUsersService usersService, IWorkItemRepository workItemRepository)
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
                    if(!@event.ObjectId.HasValue)
                        throw new PmsException("Не указан идентификатор рабочего элемента");
                    var workItem = _workItemRepository.GetByIdNoTracking(@event.ObjectId.Value);
                    return GetResponsibleUsers(workItem).Distinct(new ApplicationUserEqualityComparer()).ToList();
                case EventType.WorkItemAppointed:
                    if (string.IsNullOrWhiteSpace(@event.Data))
                        throw new PmsException("В поле Data не содержится идентификатор пользователя");
                    return new List<ApplicationUser> { _usersService.Get(@event.Data) };
                case EventType.WorkItemDisappointed:
                    if (string.IsNullOrWhiteSpace(@event.Data))
                        throw new PmsException("В поле Data не содержится идентификатор пользователя");
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
