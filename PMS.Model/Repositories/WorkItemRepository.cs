using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Models;
using Common.Repositories;
using PMS.Model.CommonModels;
using PMS.Model.Models;
using PMS.Model.Services;

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

        public WorkItem GetByIdNoTracking(int id)
        {
            return this.context.WorkItems.AsNoTracking().SingleOrDefault(x => x.Id == id);
        }

        public WorkItem GetByIdWithParents(int id)
        {
            return this.context.WorkItems.AsNoTracking().Include(x => x.Parent.Parent.Parent).SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<WorkItem> Get(Func<WorkItem, bool> filter)
        {
            return this.context.WorkItems.AsNoTracking().Where(filter);
        }

        public IEnumerable<WorkItem> GetItemsWithExecutor(Func<WorkItem, bool> filter)
        {
            return this.context.WorkItems.AsNoTracking().Include(x => x.Creator).Include(x => x.Executor).Where(filter);
        }

        public WorkItem GetWorkItemWithAllLinkedItems(int workItemId)
        {
            return this.context.WorkItems.Include(x=>x.Parent.Parent.Parent).Include(x=>x.Children).Single(x => x.Id == workItemId);
        }

        public IEnumerable<WorkItem> GetWorkItemsWithAllIncudedElements(Func<WorkItem, bool> filter)
        {
            return
                this.context.WorkItems.Include(x => x.Parent.Parent.Parent)
                    .Include(x => x.Children.Select(y => y.Children.Select(z => z.Children)))
                    .Include(x => x.Creator)
                    .Include(x => x.Executor)
                    .Where(filter);
        }

        public WorkItem Add(WorkItem workItem)
        {
            return this.context.WorkItems.Add(workItem);
        }

        public IEnumerable<UserItemsAggregateInfo> GetItemsAggregateInfoPerUser()
        {
            var itemsUsers = this.context.Users.Where(x => !x.IsDeleted)
                .GroupJoin(this.context.WorkItems, user => user.Id, item => item.ExecutorId, (user, item) => new {User = user, Item = item})
                .SelectMany(xy => xy.Item.DefaultIfEmpty(), (x, item) => new {x.User, Item = item})
                .GroupBy(x => x.User, y => y.Item, (user, items) => new {User = user, Items = items}).ToDictionary(x => x.User, x => x.Items.Where(i=>i!=null).ToArray());
            return itemsUsers.Select(x => new UserItemsAggregateInfo
            {
                UserInfo = x.Key.GetUserIdentityText(),
                UserId = x.Key.Id,
                AtWorkCount = x.Value.Count(i => i.State == WorkItemState.AtWork),
                ReviewingCount = x.Value.Count(i=>i.State == WorkItemState.Reviewing),
                PlannedCount = x.Value.Count(i=>i.State == WorkItemState.Planned)
            });
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Delete(WorkItem item)
        {
            this.context.WorkItems.Remove(item);
        }

        public ICollection<TreeNode> GetNodes(Func<IHierarchicalEntity, bool> whereExpression)
        {
            return Get(whereExpression).Select(x => x.ToTreeNode()).ToList();
        }
    }
}
