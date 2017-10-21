using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Models;
using Common.Repositories;
using PMS.Model.CommonModels;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;
using PMS.Model.Services;

namespace PMS.Model.Repositories
{
    public class WorkItemRepository : IWorkItemRepository, ITreeRepository
    {
        private readonly ApplicationContext _context;
        public WorkItemRepository(ApplicationContext context)
        {
            _context = context;
        }

        public WorkItem GetById(int id)
        {
            return _context.WorkItems.SingleOrDefault(x => x.Id == id);
        }

        public WorkItem GetByIdNoTracking(int id)
        {
            return _context.WorkItems.AsNoTracking().SingleOrDefault(x => x.Id == id);
        }

        public WorkItem GetByIdWithParents(int id)
        {
            return _context.WorkItems.AsNoTracking().Include(x => x.Executor).Include(x => x.Parent.Parent.Parent).SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<WorkItem> Get(Expression<Func<WorkItem, bool>> filter)
        {
            return _context.WorkItems.AsNoTracking().Where(filter);
        }

        public IEnumerable<WorkItem> Get(SearchModel searchModel)
        {
            var query = CreateFilterQuery(searchModel);
            if (searchModel.Sorting.Direction == SortingDirection.Asc)
            {
                query = query.OrderBy(x => searchModel.Sorting.FieldName);
            }
            else
            {
                query = query.OrderByDescending(x => searchModel.Sorting.FieldName);
            }
            return query;
        }

        private IQueryable<WorkItem> CreateFilterQuery(SearchModel searchModel)
        {
            var query = _context.WorkItems.Include(x => x.Executor);
            int id;
            if (IsSearchTextNumber(searchModel.SearchText, out id))
            {
                query = query.Where(x => x.Id == id);
            }
            else
            {
                query = query.Where(x => x.Name.Contains(searchModel.SearchText) || x.Description.Contains(searchModel.SearchText));
            }
            if (searchModel.States != null && searchModel.States.Any())
            {
                query = query.Where(x => searchModel.States.Contains(x.State));
            }
            if (searchModel.Types != null && searchModel.Types.Any())
            {
                query = query.Where(x => searchModel.Types.Contains(x.Type));
            }
            if (searchModel.UserIds != null && searchModel.UserIds.Any())
            {
                query =
                    query.Where(x => searchModel.UserIds.Any(i => i.Equals(x.ExecutorId,
                        StringComparison.InvariantCultureIgnoreCase)));
            }
            return query;
        }

        private bool IsSearchTextNumber(string text, out int id)
        {
            return int.TryParse(text, out id);
        }

        public int GetTotalItemCount(SearchModel searchModel)
        {
            return CreateFilterQuery(searchModel).Count();
        }

        public IEnumerable<WorkItem> GetItemsWithExecutor(Expression<Func<WorkItem, bool>> filter)
        {
            return _context.WorkItems.AsNoTracking().Include(x => x.Creator).Include(x => x.Executor).Where(filter);
        }

        public WorkItem GetWorkItemWithAllLinkedItems(int workItemId)
        {
            return _context.WorkItems.Include(x=>x.Parent.Parent.Parent).Include(x=>x.Children).Single(x => x.Id == workItemId);
        }


        public IEnumerable<WorkItem> GetWorkItemsWithAllIncudedElements(Expression<Func<WorkItem, bool>> filter)
        {
            return
                _context.WorkItems.Include(x => x.Parent.Parent.Parent)
                    .Include(x => x.Children.Select(y => y.Children.Select(z => z.Children)))
                    .Include(x => x.Creator)
                    .Include(x => x.Executor)
                    .Where(filter);
        }

        public WorkItem Add(WorkItem workItem)
        {
            return _context.WorkItems.Add(workItem);
        }

        public IEnumerable<UserItemsAggregateInfo> GetItemsAggregateInfoPerUser()
        {
            var itemsUsers = _context.Users.Where(x => !x.IsDeleted)
                .GroupJoin(_context.WorkItems, user => user.Id, item => item.ExecutorId, (user, item) => new {User = user, Item = item})
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
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Delete(WorkItem item)
        {
            _context.WorkItems.Remove(item);
        }

        public ICollection<TreeNode> GetNodes(Expression<Func<IHierarchicalEntity, bool>> whereExpression)
        {
            return _context.WorkItems.AsNoTracking().Where(whereExpression.Compile()).OfType<WorkItem>().Select(x => x.ToTreeNode()).ToList();
        }
    }
}
