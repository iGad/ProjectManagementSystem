using System.Collections.Generic;

namespace ProjectManagementSystem.ViewModels
{
    public class LinkedItemsCollection
    {
        public LinkedItemsCollection(string linkName)
        {
            LinkName = linkName;
            WorkItems = new List<WorkItemTileViewModel>();
        }
        public string LinkName { get; set; }
        public List<WorkItemTileViewModel> WorkItems { get; set; } 
    }
}