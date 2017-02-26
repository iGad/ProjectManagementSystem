using PMS.Model.Models;
using PMS.Model.Services;

namespace ProjectManagementSystem.ViewModels
{
    public class WorkItemChangesViewModel
    {
        public WorkItemChangesViewModel(WorkItem oldItem, WorkItem item)
        {
           if(oldItem.Id != item.Id)
                throw new PmsExeption("Id not equals");
            Id = oldItem.Id;
            ChangedProperties = oldItem.GetDifferentProperties(item);
        }
        public int Id { get; set; }
        public string[] ChangedProperties { get; set; }
    }
}