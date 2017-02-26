using System;
using System.Collections.Generic;
using System.Linq;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class WorkItemService
    {
        private readonly IWorkItemRepository repository;
        public WorkItemService(IWorkItemRepository repository)
        {
            this.repository = repository;
        }

        protected IWorkItemRepository Repository => this.repository;

        public WorkItem Get(int id)
        {
            var workItem = this.repository.GetById(id);
            workItem.Parent = null;
            workItem.Children = null;
            if(workItem == null)
                throw new PmsExeption(string.Format(Resources.WorkItemNotFound, id));
            return workItem;
        }

        public WorkItem GetWithParents(int id)
        {
            var workItem = this.repository.GetByIdWithParents(id);
            if (workItem == null)
                throw new PmsExeption(string.Format(Resources.WorkItemNotFound, id));
            return workItem;
        }

        public WorkItem Add(WorkItem workItem)
        {
            workItem.State = string.IsNullOrWhiteSpace(workItem.ExecutorId) ? WorkItemState.New : WorkItemState.Planned;
            var item = this.repository.Add(workItem);
            this.repository.SaveChanges();
            OnAdded(item);
            return item;
        }

        protected virtual void OnAdded(WorkItem item) { }

        public List<WorkItem> GetActualProjects()
        {
            return
                this.repository.Get(
                    x => !x.ParentId.HasValue && x.State != WorkItemState.Archive && x.State != WorkItemState.Deleted && x.State != WorkItemState.Done).ToList();
        }

        public List<WorkItem> GetChildWorkItems(int parentId)
        {
            return
                this.repository.Get(
                    x => x.ParentId == parentId && x.State != WorkItemState.Archive && x.State != WorkItemState.Deleted && x.State != WorkItemState.Done).ToList();
        }

        public void Update(WorkItem workItem)
        {
            var oldWorkItem = Get(workItem.Id);
            var differentProperties = oldWorkItem.GetDifferentProperties(workItem);
            OnUpdating(oldWorkItem, workItem, differentProperties);
            //var currentUser = GetCurrentUser();
            //var sendModel = new {workItem.Id, ChangedProperties = oldWorkItem.GetDifferentProperties(workItem)};
            //var oldExecutorUsername = this.userRepository.GetById(oldWorkItem.ExecutorId).UserName;
            //bool needNotification = true;
            //if (oldWorkItem.ExecutorId != workItem.ExecutorId)
            //{
            //    var executorUsername = this.userRepository.GetById(workItem.ExecutorId).UserName;
            //    var model = new WorkItemNotificationModel(oldWorkItem, currentUser);
            //    this.notifyService.SendNotification(Constants.DisappointEventName, model, BroadcastType.Users, oldExecutorUsername);
            //    oldWorkItem.ExecutorId = workItem.ExecutorId;
            //    this.notifyService.SendNotification(Constants.DisappointEventName,model, BroadcastType.Users, executorUsername);
            //    needNotification = false;
            //}
            //if (oldWorkItem.State != workItem.State)
            //{
            //    if (needNotification)
            //    {
            //        var model = new WorkItemNotificationModel(oldWorkItem, currentUser)
            //        {
            //            Data = new {Old = oldWorkItem.State.GetDescription(), New = workItem.State.GetDescription()}
            //        };
            //        this.notifyService.SendNotification(Constants.WorkItemChangedEventName, model, BroadcastType.Users, oldExecutorUsername);
            //    }
            //    oldWorkItem.State = workItem.State;
            //}
            oldWorkItem.State = workItem.State;
            oldWorkItem.Status = workItem.Status;
            oldWorkItem.Name = workItem.Name;
            oldWorkItem.Description = workItem.Description;
            oldWorkItem.DeadLine = workItem.DeadLine;
            oldWorkItem.ExecutorId = workItem.ExecutorId;
            this.repository.SaveChanges();
            OnUpdated(oldWorkItem, differentProperties);
            //this.notifyService.SendEvent(Constants.WorkItemChangedEventName, sendModel, BroadcastType.All);
        }

        protected virtual void OnUpdated(WorkItem workItem, string[] differentProperties)
        {
            
        }

        protected virtual void OnUpdating(WorkItem oldWorkItem, WorkItem workItem, string[] differentProperties)
        {
        }

        public void Delete(int id, bool cascade)
        {
            var item = Get(id);
            if (cascade)
            {
                var children = GetChildWorkItems(id);
                foreach (var workItem in children)
                {
                    Delete(workItem.Id, true);
                }
            }
            this.repository.Delete(item);
            this.repository.SaveChanges();
        }


        public Dictionary<WorkItemState, List<WorkItem>> GetActualWorkItems()
        {
            var states = new[] {WorkItemState.New, WorkItemState.Planned, WorkItemState.AtWork, WorkItemState.Reviewing, WorkItemState.Done};
            var items = this.repository.GetItemsWithExecutor(x => states.Contains(x.State)).ToList();
            var itemsDictionary = new Dictionary<WorkItemState, List<WorkItem>>();
            foreach (var state in states)
            {
                itemsDictionary.Add(state, items.Where(x => x.State == state).ToList());
            }
            return itemsDictionary;
        }

        public List<WorkItemState> GetStates()
        {
            return Extensions.ToEnumList<WorkItemState>().Where(x => x != WorkItemState.Deleted).ToList();
        }

        public WorkItem GetWorkItemWithAllLinkedItems(int workItemId)
        {
            return this.repository.GetWorkItemWithAllLinkedItems(workItemId);
        }

        public List<WorkItem> GetWorkItemsWithAllIncludedElements(Func<WorkItem, bool> whereExpression)
        {
            return this.repository.GetWorkItemsWithAllIncudedElements(whereExpression).ToList();
        }

        
    }
}
