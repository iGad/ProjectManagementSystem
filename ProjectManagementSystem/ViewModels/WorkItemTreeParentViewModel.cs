using PMS.Model.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class WorkItemTreeParentViewModel
    {
        public WorkItemTreeParentViewModel(WorkItem workItem)
        {
            Id = workItem.Id;
            Name = workItem.Name;
            if(workItem.ParentId.HasValue)
                Parent = new WorkItemTreeParentViewModel(workItem.Parent);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public WorkItemTreeParentViewModel Parent { get; set; }
    }
}