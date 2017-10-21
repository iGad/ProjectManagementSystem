using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PMS.Model.CommonModels;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public interface IWorkItemRepository: IRepository
    {
        WorkItem GetById(int id);
        WorkItem GetByIdNoTracking(int id);
        WorkItem GetByIdWithParents(int id);
        IEnumerable<WorkItem> Get(SearchModel searchModel);
        int GetTotalItemCount(SearchModel searchModel);
        IEnumerable<WorkItem> Get(Expression<Func<WorkItem, bool>> filter);
        WorkItem Add(WorkItem workItem);
        IEnumerable<WorkItem> GetItemsWithExecutor(Expression<Func<WorkItem, bool>> filter);
        WorkItem GetWorkItemWithAllLinkedItems(int workItemId);
        IEnumerable<WorkItem> GetWorkItemsWithAllIncudedElements(Expression<Func<WorkItem, bool>> filter);
        IEnumerable<UserItemsAggregateInfo> GetItemsAggregateInfoPerUser();
        void Delete(WorkItem item);
    }
}