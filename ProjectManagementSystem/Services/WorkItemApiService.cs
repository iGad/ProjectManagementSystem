using System.Collections.Generic;
using System.Linq;
using Common.Models;
using Common.Services;
using PMS.Model;
using PMS.Model.Models;
using PMS.Model.Models.Notification;
using PMS.Model.Repositories;
using PMS.Model.Services;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Services
{
    public class WorkItemApiService : WorkItemService
    {
        private readonly INotifyService notifyService;
        private readonly IUserRepository userRepository;
        private readonly ICurrentUsernameProvider currentUsernameProvider;
        public WorkItemApiService(IWorkItemRepository repository, IUserRepository userRepository, ICurrentUsernameProvider currentUsernameProvider, INotifyService notifyService) :base(repository)
        {
            this.notifyService = notifyService;
            this.userRepository = userRepository;
            this.currentUsernameProvider = currentUsernameProvider;
        }

        public WorkItemViewModel GetWorkItem(int id)
        {
            return new WorkItemViewModel(GetWithParents(id));
        }

        public Dictionary<string, List<WorkItemTileViewModel>> GetActualWorkItemModels()
        {
            return GetActualWorkItems()
                .ToDictionary(pair => pair.Key.ToString(), pair => pair.Value.Select(x => new WorkItemTileViewModel(x)).ToList());
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
            return this.userRepository.GetByUserName(this.currentUsernameProvider.GetCurrentUsername());
        }

        #region Overrides

        protected override void OnAdded(WorkItem item)
        {
            this.notifyService.SendEvent(Constants.WorkItemAddedEventName, item.Id, BroadcastType.All);
            var currentUser = GetCurrentUser();
            var model = new WorkItemNotificationViewModel(item, currentUser);
            if (!string.IsNullOrWhiteSpace(item.ExecutorId))
            {
                var executorName = this.userRepository.GetById(item.ExecutorId).UserName;
                if (executorName != currentUser.UserName)
                {
                    this.notifyService.SendNotification(Constants.AppointEventName, model, BroadcastType.Users, executorName);
                }
                SendNotificationToManagerAndExecutor(Constants.WorkItemAddedEventName, model, item, currentUser.UserName, executorName);
            }
            else
            {
                SendNotificationToManagerAndExecutor(Constants.WorkItemAddedEventName, model, item, currentUser.UserName);
            }
        }

        protected override void OnUpdating(WorkItem oldWorkItem, WorkItem workItem, string[] differentProperties)
        {
            var currentUser = GetCurrentUser();
            var oldExecutorUsername = this.userRepository.GetById(oldWorkItem.ExecutorId)?.UserName;
            var model = new WorkItemNotificationViewModel(oldWorkItem, currentUser);
            bool needNotification = true;
            if (differentProperties.Any(x => x == nameof(WorkItem.ExecutorId)))
            {
                var executorUsername = this.userRepository.GetById(workItem.ExecutorId)?.UserName;
                if (currentUser.UserName != oldExecutorUsername)
                    this.notifyService.SendNotification(Constants.DisappointEventName, model, BroadcastType.Users, oldExecutorUsername);
                if (currentUser.UserName != executorUsername)
                    this.notifyService.SendNotification(Constants.AppointEventName, model, BroadcastType.Users, executorUsername);
                SendNotificationToManagerAndExecutor(Constants.WorkItemChangedEventName, model, oldWorkItem, currentUser.UserName, oldExecutorUsername, executorUsername);
                needNotification = false;
            }
            if (needNotification && differentProperties.All(x=>x == nameof(WorkItem.State)))
            {
                model.Data = new {Old = new EnumViewModel<WorkItemState>(oldWorkItem.State), New = new EnumViewModel<WorkItemState>(workItem.State)};
                SendNotificationToManagerAndExecutor(Constants.WorkItemStateChangedEventName, model, oldWorkItem, currentUser.UserName);
                //if(currentUser.UserName != oldExecutorUsername)
                //    this.notifyService.SendNotification(Constants.WorkItemChangedEventName, model, BroadcastType.Users, oldExecutorUsername);
                //if (currentUser.UserName != oldExecutorUsername)
                //    this.notifyService.SendNotification(Constants.WorkItemChangedEventName, model, BroadcastType.Users, oldExecutorUsername);
                needNotification = false;
            }
            if (needNotification)
            {
                SendNotificationToManagerAndExecutor(Constants.WorkItemChangedEventName, model, oldWorkItem, currentUser.UserName);
            }
        }

        private void SendNotificationToManagerAndExecutor(string notificationName, object model, WorkItem workItem, params string[] exceptUsers)
        {
            var users = GetManagerAndExecutorNames(workItem, exceptUsers);
            //todo: Надо ли всегда директора оповещать?
            if(users.Any())
                this.notifyService.SendNotification(notificationName, model, BroadcastType.Users, users.ToArray());
        }
        
        private List<string> GetManagerAndExecutorNames(WorkItem workItem, params string[] exceptUsers)
        {
            var executorUsername = this.userRepository.GetById(workItem.ExecutorId)?.UserName;
            var users = new List<string>();
            if (exceptUsers == null || !exceptUsers.Contains(executorUsername))
                users.Add(executorUsername);
            if (workItem.ParentId.HasValue)
            {
                var parentItemUserId = Repository.GetById(workItem.ParentId.Value).ExecutorId;
                var parentItemUsername = this.userRepository.GetById(parentItemUserId)?.UserName;
                if (exceptUsers == null || !exceptUsers.Contains(parentItemUsername))
                    users.Add(parentItemUsername);
            }
            return users;
        } 

        protected override void OnUpdated(WorkItem workItem, string[] differentProperties)
        {
            var sendModel = new { workItem.Id, ChangedProperties = differentProperties };
            this.notifyService.SendEvent(Constants.WorkItemChangedEventName, sendModel, BroadcastType.All);
        }

        #endregion

    }
}