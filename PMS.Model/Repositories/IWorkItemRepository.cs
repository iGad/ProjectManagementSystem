using System;
using System.Collections.Generic;
using PMS.Model.CommonModels;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public interface IWorkItemRepository
    {
        WorkItem GetById(int id);
        WorkItem GetByIdNoTracking(int id);
        WorkItem GetByIdWithParents(int id);
        IEnumerable<WorkItem> Get(Func<WorkItem, bool> filter);
        WorkItem Add(WorkItem workItem);
        IEnumerable<WorkItem> GetItemsWithExecutor(Func<WorkItem, bool> filter);
        WorkItem GetWorkItemWithAllLinkedItems(int workItemId);
        IEnumerable<WorkItem> GetWorkItemsWithAllIncudedElements(Func<WorkItem, bool> filter);
        IEnumerable<UserItemsAggregateInfo> GetItemsAggregateInfoPerUser();
        int SaveChanges();
        void Delete(WorkItem item);
    }
}