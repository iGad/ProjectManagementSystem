using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Models;

namespace PMS.Model.Models
{
    public class WorkItem : NamedEntity, IHierarchicalEntity
    {
        public WorkItem()
        {
        }
        /// <summary>
        /// Конструктор копирования. не копирует связанные сущности
        /// </summary>
        /// <param name="workItem"></param>
        public WorkItem(WorkItem workItem)
        {
            Id = workItem.Id;
            ParentId = workItem.ParentId;
            Type = workItem.Type;
            State = workItem.State;
            Status = workItem.Status;
            Name = workItem.Name;
            Description = workItem.Description;
            DeadLine = workItem.DeadLine;
            FinishDate = workItem.FinishDate;
            ExecutorId = workItem.ExecutorId;
            CreatorId = workItem.CreatorId;
        }

        public int? ParentId { get;
            set; }
        public WorkItem Parent { get; set; }

        IHierarchicalEntity IHierarchicalEntity.Parent
        {
            get
            {
                return Parent;
            }
            set
            {
                Parent = value as WorkItem;
            }
        }

        public WorkItemType Type { get; set; }
        public string Description { get; set; }
        [Required]
        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }
        public string ExecutorId { get; set; }
        public ApplicationUser Executor { get; set; }
        public WorkItemState State { get; set; }
        public WorkItemStatus Status { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime? FinishDate { get; set; }
        public ICollection<WorkItem> Children { get; set; } 
        public ICollection<Comment> Comments { get; set; }
    }
}
