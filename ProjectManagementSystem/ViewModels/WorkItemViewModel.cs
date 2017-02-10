using System;
using PMS.Model.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class WorkItemViewModel
    {
        /// <summary>
        /// Конструктор по рабочему элементу. Должны быть загружены родители!
        /// </summary>
        /// <param name="workItem"></param>
        public WorkItemViewModel(WorkItem workItem)
        {
            Type = workItem.Type;
            DeadLine = workItem.DeadLine;
            State = workItem.State;
            ExecutorId = workItem.ExecutorId;
            Description = workItem.Description;
            Id = workItem.Id;
            Name = workItem.Name;
            FillParentIds(workItem);
        }

        private void FillParentIds(WorkItem workItem)
        {
            switch (workItem.Type)
            {
                case WorkItemType.Stage:
                    ProjectId = workItem.ParentId;
                    break;
                case WorkItemType.Partition:
                    StageId = workItem.ParentId;
                    ProjectId = workItem.Parent.ParentId;
                    break;
                case WorkItemType.Task:
                    PartitionId = workItem.ParentId;
                    StageId = workItem.Parent.ParentId;
                    ProjectId = workItem.Parent.Parent.ParentId;
                    break;
            }
        }

        public int? PartitionId { get; set; }
        public int? StageId { get; set; }
        public int? ProjectId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public WorkItemType Type { get; set; }
        public string Description { get; set; }
        public string ExecutorId { get; set; }
        public WorkItemState State { get; set; }
        public DateTime DeadLine { get; set; }
    }
}