using Common.Models;

namespace PMS.Model.Models
{
    public class Comment : Entity
    {
        public int WorkItemId { get; set; }
        public WorkItem WorkItem { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        //public virtual ICollection<Event> Events { get; set; }
    }
}
