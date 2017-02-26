namespace PMS.Model.Models.Notification
{
    public class WorkItemNotificationModel
    {
        public WorkItemNotificationModel(WorkItem workItem, ApplicationUser user)
        {
            Id = workItem.Id;
            Name = workItem.Name;
            User = user.ToString();
            User = user.Id;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string UserId { get; set; }
        public object Data { get; set; }
    }
}