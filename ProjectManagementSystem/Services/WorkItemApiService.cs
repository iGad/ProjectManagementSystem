using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Common.Models;
using Common.Services;
using Newtonsoft.Json;
using PMS.Model;
using PMS.Model.CommonModels;
using PMS.Model.CommonModels.EventModels;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Services
{
    public class WorkItemApiService : WorkItemService
    {
        private readonly IUsersService _userService;
        private readonly INotifyService _notifyService;
        private readonly IEventService _eventService;
        public WorkItemApiService(IWorkItemRepository repository, IUsersService userService, INotifyService notifyService, IEventService eventService) 
            :base(repository)
        {
            _userService = userService;
            _notifyService = notifyService;
            _eventService = eventService;
        }

        public WorkItemViewModel GetWorkItem(int id)
        {
            return new WorkItemViewModel(GetWithParents(id));
        }

        public TableCollectionModel<WorkItemTileViewModel> Find(SearchModel searchModel)
        {
            var collection = Get(searchModel).Select(x => new WorkItemTileViewModel(x)).ToList();
            var totalCount = GetTotalItemCount(searchModel);

            return new TableCollectionModel<WorkItemTileViewModel>{Collection = collection, TotalCount=totalCount};
        }

        public Dictionary<string, List<WorkItemTileViewModel>> GetActualWorkItemModels()
        {
            return ConvertToViewDictionary(GetActualWorkItems());

        }

        private Dictionary<string, List<WorkItemTileViewModel>> ConvertToViewDictionary(Dictionary<WorkItemState, List<WorkItem>> dictionary)
        {
            return dictionary.ToDictionary(pair => pair.Key.ToString(), pair => pair.Value.Select(x => new WorkItemTileViewModel(x)).ToList());
        }

        public Dictionary<string, List<WorkItemTileViewModel>> GetActualWorkItemModels(string userId)
        {
            return ConvertToViewDictionary(GetActualWorkItems(userId));
        }

        public List<EnumViewModel<WorkItemState>> GetStatesViewModels(bool isMainEngeneer, bool isManager)
        {
            var states = new List<WorkItemState> {WorkItemState.Planned, WorkItemState.AtWork, WorkItemState.Reviewing};
            if (isManager)
                states.Add(WorkItemState.Done);
            else
            {
                if (isMainEngeneer)
                {
                    states.Insert(0, WorkItemState.New);
                    states.Add(WorkItemState.Done);
                    states.Add(WorkItemState.Archive);
                }
            }
            return states.Select(x => new EnumViewModel<WorkItemState>(x)).ToList();
        }

        public List<EnumViewModel<WorkItemState>> GetAllStates()
        {
            var states = PMS.Model.Extensions.ToEnumList<WorkItemState>().Where(x=>x != WorkItemState.Deleted).Select(x => new EnumViewModel<WorkItemState>(x)).ToList();
            return states;
        }

        public List<LinkedItemsCollection> GetLinkedWorkItemsForItem(int itemId)
        {
            var workItem = GetWorkItemWithAllLinkedItems(itemId);
            var parentsCollection = new LinkedItemsCollection(Resource.ParentElements);
            var parent = workItem.Parent;
            while (parent != null)
            {
                parentsCollection.WorkItems.Insert(0, new WorkItemTileViewModel(parent));
                parent = parent.Parent;
            }
            var childCollection = new LinkedItemsCollection(Resource.ChildElements);
            childCollection.WorkItems.AddRange(workItem.Children.OrderBy(x => x.Id).Select(x => new WorkItemTileViewModel(x)));
            return new List<LinkedItemsCollection> {parentsCollection, childCollection};
        }

        public List<WorkItemTreeViewModel> GetProjectsTree()
        {
            var projects = GetWorkItemsWithAllIncludedElements(x => x.State != WorkItemState.Deleted && !x.ParentId.HasValue);
            var projectsTree = projects.Select(x => new WorkItemTreeViewModel(x)).ToList();
            return projectsTree;
        }

        private ApplicationUser GetCurrentUser()
        {
            return _userService.GetCurrentUser();
        }
        
        /// <summary>
        /// Получение всех пользователей, кому необходимо сообщить о событии.
        /// Не включает текущего пользователя
        /// </summary>
        /// <param name="workItem">Рабочий элемент, о событии с которым необходимо оповестить пользователей</param>
        /// <returns></returns>
        protected List<ApplicationUser> GetNotifyingUsers(WorkItem workItem)
        {
            var users = new List<ApplicationUser> {GetCurrentUser()};
            var executor = _userService.SafeGet(workItem.ExecutorId);
            if (executor != null && users.All(x=>x.Id != executor.Id))
                users.Add(executor);
            if (workItem.ParentId.HasValue)
            {
                var parentItemExecutorId = Repository.GetByIdNoTracking(workItem.ParentId.Value).ExecutorId;
                var parentItemExecutor = _userService.SafeGet(parentItemExecutorId);
                if (parentItemExecutor != null && users.All(x => x.Id != parentItemExecutor.Id))
                    users.Add(parentItemExecutor);
            }
            var directors = _userService.GetUsersByRole(RoleType.Director);
            users.AddRange(directors.Where(x => users.All(u => u.Id != x.Id)));
            users.RemoveAt(0);
            return users;
        }

        public void UpdateWorkItemState(int workItemId, WorkItemState newState)
        {
            var workItem = Get(workItemId);
            workItem.State = newState;
            Update(workItem);
        }

        #region Overrides

        public override WorkItem Add(WorkItem workItem)
        {
            var item = base.Add(workItem);
            NotifyOnAdded(item);
            return item;
        }

        private void NotifyOnAdded(WorkItem item)
        {
            _notifyService.SendEvent(Constants.WorkItemAddedEventName, item.Id, BroadcastType.All);
            var user = GetCurrentUser();
            var addedEvent = CreateBaseEvent(item.Id, EventType.WorkItemAdded);
            if (!string.IsNullOrWhiteSpace(item.ExecutorId))
            {
                var executor = _userService.Get(item.ExecutorId);
                var appointEvent = CreateBaseEvent(item.Id, EventType.WorkItemAppointed);
                appointEvent.Data = item.ExecutorId;
                SendNotificationToResponsibleUsers(addedEvent, item, user.Id, executor.Id);
                _eventService.AddEvent(appointEvent, new[] {item.ExecutorId});
                if (executor.Id != user.Id)
                {
                    _notifyService.SendNotifications(appointEvent, executor);
                }
            }
            else
            {
                SendNotificationToResponsibleUsers(addedEvent, item, user.Id);
            }
        }

        public void Update(WorkItem workItem)
        {
            var oldWorkItem = Get(workItem.Id);
            var differentProperties = oldWorkItem.GetDifferentProperties(workItem);
            NotifyOnUpdating(oldWorkItem, workItem, differentProperties);
            Update(oldWorkItem, workItem);
            NotifyOnUpdated(oldWorkItem, differentProperties);
        }

        private void NotifyOnUpdating(WorkItem oldWorkItem, WorkItem workItem, string[] differentProperties)
        {
            var user = GetCurrentUser();
            var oldExecutor = _userService.SafeGet(oldWorkItem.ExecutorId);
            bool needNotification = true;
            var changedEvent = CreateBaseEvent(workItem.Id, EventType.WorkItemChanged);
            if (differentProperties.Any(x => x == nameof(WorkItem.ExecutorId)))
            {
                var executor = _userService.SafeGet(workItem.ExecutorId);
                if (oldExecutor != null)
                {
                    var disappointEvent = CreateBaseEvent(workItem.Id, EventType.WorkItemDisappointed, oldExecutor.Id);
                    disappointEvent = _eventService.AddEvent(disappointEvent, new[] {oldExecutor.Id});
                    if (user.UserName != oldExecutor.UserName)
                        _notifyService.SendNotifications(disappointEvent, new[] {oldExecutor});
                }
                if (executor != null)
                {
                    var appointEvent = CreateBaseEvent(workItem.Id, EventType.WorkItemAppointed, executor.Id);
                    _eventService.AddEvent(appointEvent, new[] { executor.Id });
                    if (user.UserName != executor.UserName)
                        _notifyService.SendNotifications(appointEvent, new[] {executor});
                }
                
                SendNotificationToResponsibleUsers(changedEvent, oldWorkItem, user.Id, oldExecutor?.Id, executor?.Id);
                needNotification = false;
            }
            if (needNotification && differentProperties.All(x=>x == nameof(WorkItem.State)))
            {
                var stateChangedEvent = CreateBaseEvent(workItem.Id, EventType.WorkItemStateChanged,
                    new StateChangedModel{Old = oldWorkItem.State, New = workItem.State});
                SendNotificationToResponsibleUsers(stateChangedEvent, oldWorkItem, user.Id);
                needNotification = false;
            }
            if (needNotification)
            {
                SendNotificationToResponsibleUsers(changedEvent, oldWorkItem, user.Id);
            }
        }

        private WorkEvent CreateBaseEvent(int workItemId, EventType type, object data)
        {
            return new WorkEvent
            {
                UserId = GetCurrentUser().Id,
                Type = type,
                ObjectId = workItemId,
                Data = data == null ? string.Empty : JsonConvert.SerializeObject(data)
            };
        }

        private WorkEvent CreateBaseEvent(int workItemId, EventType type, string data = null)
        {
            return new WorkEvent
            {
                UserId = GetCurrentUser().Id,
                Type = type,
                ObjectId = workItemId,
                Data = data ?? string.Empty
            };
        }

        private void SendNotificationToResponsibleUsers(WorkEvent @event, WorkItem workItem, params string[] exceptUsers)
        {
            var users = GetResponsibleUsers(workItem).Distinct(new ApplicationUserEqualityComparer()).ToArray();
            var notifyingUsers = users.Where(x => (exceptUsers == null || !exceptUsers.Contains(x.Id)));
            _eventService.AddEvent(@event, users.Select(x => x.Id));
            _notifyService.SendNotifications(@event, notifyingUsers.ToArray());
        }
        
        private List<ApplicationUser> GetResponsibleUsers(WorkItem workItem, params string[] exceptUsers)
        {
            var executor = _userService.SafeGet(workItem.ExecutorId);
            var users = new List<ApplicationUser> {GetCurrentUser()};
            if (exceptUsers == null || executor!= null && !exceptUsers.Contains(executor.Id))
                users.Add(executor);
            if (workItem.ParentId.HasValue)
            {
                var parentItemUserId = Repository.GetByIdNoTracking(workItem.ParentId.Value).ExecutorId;
                if (parentItemUserId != null)
                {
                    var parentItemExecutor = _userService.Get(parentItemUserId);
                    if (exceptUsers == null || !exceptUsers.Contains(parentItemExecutor.Id))
                        users.Add(parentItemExecutor);
                }
            }
            var directorNames = _userService.GetUsersByRole(RoleType.Director).ToArray();
            users.AddRange(directorNames.Where(x => users.All(u => u.Id != x.Id)));
            return users;
        } 

        private void NotifyOnUpdated(WorkItem workItem, string[] differentProperties)
        {
            var sendModel = new WorkItemUpdatedModel { WorkItemId = workItem.Id, ChangedProperties = differentProperties };
            _notifyService.SendEvent(Constants.WorkItemChangedEventName, sendModel, BroadcastType.All);
        }

        public override void Delete(int id, bool cascade)
        {
            var item = Get(id);
            base.Delete(id, cascade);
            var @event = CreateBaseEvent(id, EventType.WorkItemDeleted, id);
            SendNotificationToResponsibleUsers(@event, item, GetCurrentUser().Id);

        }

        #endregion

        

    }
}