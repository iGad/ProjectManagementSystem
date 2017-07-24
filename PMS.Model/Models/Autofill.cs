using Common.Models;

namespace PMS.Model.Models
{
    public class Autofill : NamedEntity
    {
        public string Description { get; set; }
        public WorkItemType WorkItemType { get; set; }
    }
}
