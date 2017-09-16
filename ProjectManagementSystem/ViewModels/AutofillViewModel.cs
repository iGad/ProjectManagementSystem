using PMS.Model.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class AutofillViewModel
    {
        public AutofillViewModel(Autofill autofill)
        {
            Id = autofill.Id;
            Name = autofill.Name;
            Description = autofill.Description;
            WorkItemType = autofill.WorkItemType;
            TypeViewModel = new EnumViewModel<WorkItemType>(WorkItemType);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public WorkItemType WorkItemType { get; set; }
        public EnumViewModel<WorkItemType> TypeViewModel { get; set; }
    }
}