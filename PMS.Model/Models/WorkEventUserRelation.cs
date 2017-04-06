namespace PMS.Model.Models
{
    public class WorkEventUserRelation
    {
        public WorkEventUserRelation()
        {
        }

        public WorkEventUserRelation(int eventId, string userId)
        {
            EventId = eventId;
            UserId = userId;
        }
        public int EventId { get; set; }
        public WorkEvent Event { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public EventState State { get; set; }
        public bool IsFavorite { get; set; }
    }
}
