using System;
using PMS.Model;
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
            State = new EnumViewModel<WorkItemState>(workItem.State);
            FinishDate = workItem.FinishDate?.ToString(Constants.DateTimeFormat);
            DeadLine = workItem.DeadLine.ToString(Constants.DateTimeFormat);
            Executor = workItem.Executor != null ? new UserInfoViewModel(workItem.Executor) : null;
            IsDeadLineSoon = FinishDate == null && workItem.DeadLine > DateTime.Now && (workItem.DeadLine - DateTime.Now).TotalHours < 48;
            IsOverdue = FinishDate == null && workItem.DeadLine < DateTime.Now;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public WorkItemType Type { get; set; }
        public UserInfoViewModel Executor { get; set; }
        public EnumViewModel<WorkItemState>  State { get; set; }
        public string DeadLine { get; set; }
        public string FinishDate { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsDeadLineSoon { get; set; }
    }
}