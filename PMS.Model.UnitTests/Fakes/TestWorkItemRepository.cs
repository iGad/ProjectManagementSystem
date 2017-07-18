using System;
using System.Collections.Generic;
using System.Linq;
using PMS.Model.CommonModels;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;

namespace PMS.Model.UnitTests.Fakes
{
    public class TestWorkItemRepository : IWorkItemRepository
    {
        public readonly List<WorkItem> WorkItems = new List<WorkItem>();
        public readonly IUserRepository UserRepository;

        public TestWorkItemRepository()
        {
            this.UserRepository = new TestUserRepository();
        }

        public TestWorkItemRepository(IUserRepository repository)
        {
            this.UserRepository = repository;
        }

        public int SaveChangesCalled { get; private set; }

        public WorkItem GetById(int id)
        {
            return this.WorkItems.SingleOrDefault(x=>x.Id == id);
        }

        public WorkItem GetByIdNoTracking(int id)
        {
            return WorkItems.SingleOrDefault(x=>x.Id == id);
        }

        public WorkItem GetByIdWithParents(int id)
        {
            var workItem = GetById(id);
            var item = workItem;
            while (item.ParentId.HasValue)
            {
                item.Parent = GetById(item.ParentId.Value);
                item = item.Parent;
            }
            return workItem;
        }

        public IEnumerable<WorkItem> Get(SearchModel searchModel)
        {
            return WorkItems;
        }

        public int GetTotalItemCount(SearchModel searchModel)
        {
            return WorkItems.Count;
        }

        public IEnumerable<WorkItem> Get(Func<WorkItem, bool> filter)
        {
            return this.WorkItems.Where(filter);
        }

        public WorkItem Add(WorkItem workItem)
        {
            workItem.Id = this.WorkItems.OrderByDescending(x => x.Id).First().Id + 1;
            this.WorkItems.Add(workItem);
            return workItem;
        }

        public IEnumerable<WorkItem> GetItemsWithExecutor(Func<WorkItem, bool> filter)
        {
            var items = Get(filter).ToList();
            foreach (var workItem in items)
            {
                workItem.Executor = string.IsNullOrWhiteSpace(workItem.ExecutorId) ? null : this.UserRepository.GetById(workItem.ExecutorId);
            }
            return items;
        }

        public WorkItem GetWorkItemWithAllLinkedItems(int workItemId)
        {
            var workItem = GetByIdWithParents(workItemId);
            workItem.Children = Get(x => x.ParentId == workItemId).ToList();
            return workItem;
        }

        public IEnumerable<WorkItem> GetWorkItemsWithAllIncudedElements(Func<WorkItem, bool> filter)
        {
            var items = this.WorkItems.Where(filter);
            return items;
        }

        public IEnumerable<UserItemsAggregateInfo> GetItemsAggregateInfoPerUser()
        {
            var itemsUsers = UserRepository.GetUsers(x => true)
                .Where(x => !x.IsDeleted)
                .GroupJoin(WorkItems, user => user.Id, item => item.ExecutorId,
                    (user, item) => new {User = user, Item = item})
                .SelectMany(xy => xy.Item.DefaultIfEmpty(), (x, item) => new {x.User, Item = item})
                .GroupBy(x => x.User, y => y.Item, (user, items) => new {User = user, Items = items})
                .ToDictionary(x => x.User, x => x.Items.Where(i => i != null).ToArray());
            return itemsUsers.Select(x => new UserItemsAggregateInfo
            {
                UserInfo = x.Key.GetUserIdentityText(),
                UserId = x.Key.Id,
                AtWorkCount = x.Value.Count(i => i.State == WorkItemState.AtWork),
                ReviewingCount = x.Value.Count(i => i.State == WorkItemState.Reviewing),
                PlannedCount = x.Value.Count(i => i.State == WorkItemState.Planned)
            });
        }

        public int SaveChanges()
        {
            SaveChangesCalled++;
            return 0;
        }

        public void Delete(WorkItem item)
        {
            this.WorkItems.Remove(item);
        }
    }
}
