using System;
using System.Collections.Generic;
using System.Linq;
using PMS.Model.CommonModels;
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
            if(workItem == null)
                throw new PmsExeption(string.Format(Resources.WorkItemNotFound, id));
            return workItem;
        }

        public WorkItem GetWithNoTracking(int id)
        {
            var workItem = this.repository.GetByIdNoTracking(id);
            if (workItem == null)
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

        public virtual WorkItem Add(WorkItem workItem)
        {
            workItem.State = string.IsNullOrWhiteSpace(workItem.ExecutorId) ? WorkItemState.New : WorkItemState.Planned;
            var item = this.repository.Add(workItem);
            this.repository.SaveChanges();
            return item;
        }
        

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

        public virtual void Update(WorkItem oldWorkItem, WorkItem workItem)
        {
            oldWorkItem.State = workItem.State;
            oldWorkItem.Status = workItem.Status;
            oldWorkItem.Name = workItem.Name;
            oldWorkItem.Description = workItem.Description;
            oldWorkItem.DeadLine = workItem.DeadLine;
            oldWorkItem.ExecutorId = workItem.ExecutorId;
            this.repository.SaveChanges();
        }
        
        public virtual void Delete(int id, bool cascade)
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

        private WorkItemState[] GetActualStates()
        {
            return new[] { WorkItemState.New, WorkItemState.Planned, WorkItemState.AtWork, WorkItemState.Reviewing, WorkItemState.Done };
        }

        public Dictionary<WorkItemState, List<WorkItem>> GetActualWorkItems()
        {
            var states = GetActualStates();
            var items = this.repository.GetItemsWithExecutor(x => states.Contains(x.State)).ToList();
            return states.ToDictionary(state => state, state => items.Where(x => x.State == state).ToList());
        }

        public Dictionary<WorkItemState, List<WorkItem>> GetActualWorkItems(string userId)
        {
            var states = new[] { WorkItemState.New, WorkItemState.Planned, WorkItemState.AtWork, WorkItemState.Reviewing, WorkItemState.Done };
            var items = this.repository.GetItemsWithExecutor(x => x.ExecutorId == userId && states.Contains(x.State)).ToList();
            return states.ToDictionary(state => state, state => items.Where(x => x.State == state).ToList());
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

        public List<UserItemsAggregateInfo> GetUserItemsAggregateInfos()
        {
            return Repository.GetItemsAggregateInfoPerUser().ToList();
        }
    }
}
