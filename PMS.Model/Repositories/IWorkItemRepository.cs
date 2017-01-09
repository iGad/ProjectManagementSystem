using System;
using System.Collections.Generic;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public interface IWorkItemRepository
    {
        WorkItem GetById(int id);
        IEnumerable<WorkItem> Get(Func<WorkItem, bool> filter);
        WorkItem Add(WorkItem workItem);
        int SaveChanges();
    }
}