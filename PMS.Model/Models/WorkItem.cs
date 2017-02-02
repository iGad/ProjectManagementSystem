using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;

namespace PMS.Model.Models
{
    [Table("WorkItems")]
    public class WorkItem : NamedEntity, IHierarchicalEntity
    {
        public int? ParentId { get; set; }
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
        public ICollection<Comment> Comments { get; set; }
    }

    public static class WorkItemExtensions
    {
        public static TreeNode ToTreeNode(this WorkItem workItem)
        {
            return new TreeNode
            {
                Id = workItem.Id,
                Name = workItem.Name,
                Type = workItem.Type.ToString()
            };
        }
    }
}
