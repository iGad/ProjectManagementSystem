using System.Collections.Generic;
using System.Linq;
using PMS.Model.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class WorkItemTreeViewModel : WorkItemTileViewModel
    {
        public WorkItemTreeViewModel(WorkItem workItem) : base(workItem)
        {
            if(workItem.ParentId.HasValue)
                Parent = new WorkItemTreeParentViewModel(workItem.Parent);
            Children.AddRange(workItem.Children.Select(x => new WorkItemTreeViewModel(x)));
            Creater = new UserInfoViewModel(workItem.Creator);
        }
        public UserInfoViewModel Creater { get; set; }
        public WorkItemTreeParentViewModel Parent { get; set; }
        public List<WorkItemTreeViewModel> Children { get; set; } = new List<WorkItemTreeViewModel>();
    }
}