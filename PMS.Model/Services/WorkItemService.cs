using System;
using System.Collections.Generic;
using System.Linq;
using PMS.Model.CommonModels;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class WorkItemService
    {
        private readonly IWorkItemRepository _repository;
        private readonly ISettingsValueProvider _settingsProvider;

        public WorkItemService(IWorkItemRepository repository, ISettingsValueProvider settingsProvider)
        {
            _repository = repository;
            _settingsProvider = settingsProvider;
        }

        protected IWorkItemRepository Repository => _repository;

        public WorkItem Get(int id)
        {
            var workItem = _repository.GetById(id);
            if(workItem == null)
                throw new PmsException(string.Format(Resources.WorkItemNotFound, id));
            return workItem;
        }

        public List<WorkItem> Get(SearchModel searchModel)
        {
            return _repository.Get(searchModel).ToList();    
        }

        public int GetTotalItemCount(SearchModel searchModel)
        {
            return _repository.GetTotalItemCount(searchModel);
        }

        public WorkItem GetWithNoTracking(int id)
        {
            var workItem = _repository.GetByIdNoTracking(id);
            if (workItem == null)
                throw new PmsException(string.Format(Resources.WorkItemNotFound, id));
            return workItem;
        }

        public WorkItem GetWithParents(int id)
        {
            var workItem = _repository.GetByIdWithParents(id);
            if (workItem == null)
                throw new PmsException(string.Format(Resources.WorkItemNotFound, id));
            return workItem;
        }

        public virtual WorkItem Add(WorkItem workItem)
        {
            workItem.State = string.IsNullOrWhiteSpace(workItem.ExecutorId) ? WorkItemState.New : WorkItemState.Planned;
            var item = _repository.Add(workItem);
            _repository.SaveChanges();
            return item;
        }
        

        public List<WorkItem> GetActualProjects()
        {
            return
                _repository.Get(
                    x => !x.ParentId.HasValue && x.State != WorkItemState.Deleted).ToList();
        }

        public List<WorkItem> GetChildWorkItems(int parentId)
        {
            return
                _repository.Get(
                    x => x.ParentId == parentId && x.State != WorkItemState.Deleted).ToList();
        }

        public virtual void Update(WorkItem oldWorkItem, WorkItem workItem)
        {
            oldWorkItem.State = workItem.State;
            oldWorkItem.Status = workItem.Status;
            oldWorkItem.Name = workItem.Name;
            oldWorkItem.Description = workItem.Description;
            oldWorkItem.DeadLine = workItem.DeadLine;
            oldWorkItem.ExecutorId = workItem.ExecutorId;
            _repository.SaveChanges();
        }
        
        public virtual void Delete(int id)
        {
            var item = Get(id);
            var children = GetChildWorkItems(id);
            if (children.Any())
            {
                throw new PmsException("Невозможно удалить элемент, у которого есть дочерние элементы");
            }
            _repository.Delete(item);
            _repository.SaveChanges();
        }

        private WorkItemState[] GetActualStates()
        {
            return new[] { WorkItemState.New, WorkItemState.Planned, WorkItemState.AtWork, WorkItemState.Reviewing, WorkItemState.Done };
        }

        public Dictionary<WorkItemState, List<WorkItem>> GetActualWorkItems()
        {
            var states = GetActualStates();
            var itemsPerStateCount = int.Parse(_settingsProvider.GetSettingValue(SettingType.MaxDisplayWorkItemCount));
            var items = _repository.GetItemsWithExecutor(x => states.Contains(x.State)).ToList();
            return states.ToDictionary(state => state, state => items.Where(x => x.State == state).OrderBy(x => x.DeadLine).Take(itemsPerStateCount).ToList());
        }

        public Dictionary<WorkItemState, List<WorkItem>> GetActualWorkItems(string userId)
        {
            var states = GetActualStates(); ;
            var itemsPerStateCount = int.Parse(_settingsProvider.GetSettingValue(SettingType.MaxDisplayWorkItemCount));
            var items = _repository.GetItemsWithExecutor(x => x.ExecutorId == userId && states.Contains(x.State)).ToList();
            return states.ToDictionary(state => state, state => items.Where(x => x.State == state).OrderBy(x => x.DeadLine).Take(itemsPerStateCount).ToList());
        }

        public List<WorkItemState> GetStates()
        {
            return Extensions.ToEnumList<WorkItemState>().Where(x => x != WorkItemState.Deleted).ToList();
        }

        public WorkItem GetWorkItemWithAllLinkedItems(int workItemId)
        {
            return _repository.GetWorkItemWithAllLinkedItems(workItemId);
        }

        public List<WorkItem> GetWorkItemsWithAllIncludedElements(Func<WorkItem, bool> whereExpression)
        {
            return _repository.GetWorkItemsWithAllIncudedElements(whereExpression).ToList();
        }

        public List<UserItemsAggregateInfo> GetUserItemsAggregateInfos()
        {
            return Repository.GetItemsAggregateInfoPerUser().ToList();
        }

    }
}
