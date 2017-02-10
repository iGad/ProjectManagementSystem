using System.Collections.Generic;
using PMS.Model.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class WorkItemTreeViewModel : WorkItemTileViewModel
    {
        public WorkItemTreeViewModel(WorkItem workItem) : base(workItem)
        {
        }

        public WorkItemTreeViewModel Parent { get; set; }
        public List<WorkItemTreeViewModel> Children { get; set; } 
    }
}