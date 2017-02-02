using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;
using Common.Repositories;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public class WorkItemRepository : IWorkItemRepository, ITreeRepository
    {
        private readonly ApplicationContext context;
        public WorkItemRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public WorkItem GetById(int id)
        {
            return this.context.WorkItems.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<WorkItem> Get(Func<WorkItem, bool> filter)
        {
            return this.context.WorkItems.Where(filter);
        }

        public WorkItem Add(WorkItem workItem)
        {
            return this.context.WorkItems.Add(workItem);
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public ICollection<TreeNode> GetNodes(Func<IHierarchicalEntity, bool> whereExpression)
        {
            return Get(whereExpression).Select(x => x.ToTreeNode()).ToList();
        }
    }
}
