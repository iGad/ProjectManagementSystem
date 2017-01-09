using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Model.Models
{
    public class WorkItem : NamedEntity
    {
        public int? ParentId { get; set; }
        public WorkItem Parent { get; set; }
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
}
