using System;
using System.Collections.Generic;
using System.Linq;
using PMS.Model.Models;
using PMS.Model.Repositories;

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
