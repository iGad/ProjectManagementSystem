using PMS.Model.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class WorkItemTileViewModel
    {
        public WorkItemTileViewModel(WorkItem workItem)
        {
            Id = workItem.Id;
            Name = workItem.Name;
            Type = workItem.Type;
            State = workItem.State;
            DeadLine = workItem.DeadLine.ToString("dd.MM.yyyy HH:mm");
            Executor = workItem.Executor != null ? new UserViewModel(workItem.Executor) : null;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public WorkItemType Type { get; set; }
        public UserViewModel Executor { get; set; }
        public WorkItemState  State { get; set; }
        public string DeadLine { get; set; }
    }
}