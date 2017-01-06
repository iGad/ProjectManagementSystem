using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Model.Models
{
    public abstract class WorkItem : NamedEntity
    {
        public string Description { get; set; }
        [Required]
        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }
        public string ExecutorId { get; set; }
        public ApplicationUser Executor { get; set; }
        [Required]
        public WorkItemState State { get; set; }
        [Required]
        public WorkItemStatus Status { get; set; }
    }
}
