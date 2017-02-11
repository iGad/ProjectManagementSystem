using System.Collections.Generic;
using System.Linq;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Services
{
    public class WorkItemApiService : WorkItemService
    {
        public WorkItemApiService(IWorkItemRepository repository):base(repository)
        {
        }

        public WorkItemViewModel GetWorkItem(int id)
        {
            return new WorkItemViewModel(GetWithParents(id));
        }

        public Dictionary<string, List<WorkItemTileViewModel>> GetActualWorkItemModels()
        {
            return GetActualWorkItems()
                .ToDictionary(pair => pair.Key.ToString(), pair => pair.Value.Select(x => new WorkItemTileViewModel(x)).ToList());
        }

        public List<EnumViewModel<WorkItemState>> GetStatesViewModels(bool isMainEngeneer, bool isManager)
        {
            var states = new List<WorkItemState> {WorkItemState.Planned, WorkItemState.AtWork, WorkItemState.Reviewing};
            if (isManager)
                states.Add(WorkItemState.Done);
            else
            {
                if (isMainEngeneer)
                {
                    states.Insert(0, WorkItemState.New);
                    states.Add(WorkItemState.Done);
                    states.Add(WorkItemState.Archive);
                }
            }
            return states.Select(x => new EnumViewModel<WorkItemState>(x)).ToList();
        }

        public List<LinkedItemsCollection> GetLinkedWorkItemsForItem(int itemId)
        {
            var workItem = GetWorkItemWithAllLinkedItems(itemId);
            var parentsCollection = new LinkedItemsCollection(Resource.ParentElements);
            var parent = workItem.Parent;
            while (parent != null)
            {
                parentsCollection.WorkItems.Insert(0, new WorkItemTileViewModel(parent));
                parent = parent.Parent;
            }
            var childCollection = new LinkedItemsCollection(Resource.ChildElements);
            childCollection.WorkItems.AddRange(workItem.Children.OrderBy(x => x.Id).Select(x => new WorkItemTileViewModel(x)));
            return new List<LinkedItemsCollection> {parentsCollection, childCollection};
        }

        public List<WorkItemTreeViewModel> GetProjectsTree()
        {
            var projects = GetWorkItemsWithAllIncludedElements(x => x.State != WorkItemState.Deleted && !x.ParentId.HasValue);
            var projectsTree = projects.Select(x => new WorkItemTreeViewModel(x)).ToList();
            return projectsTree;
        } 
    }
}