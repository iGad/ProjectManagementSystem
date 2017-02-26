using PMS.Model.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class WorkItemNotificationViewModel
    {
        public WorkItemNotificationViewModel(WorkItem workItem, ApplicationUser user)
        {
            Id = workItem.Id;
            Name = workItem.Name;
            User = user.ToString();
            UserId = user.Id;
            Type = new EnumViewModel<WorkItemType>(workItem.Type);
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public EnumViewModel<WorkItemType> Type { get; set; } 
        public string User { get; set; }
        public string UserId { get; set; }
        public object Data { get; set; }
    }
}