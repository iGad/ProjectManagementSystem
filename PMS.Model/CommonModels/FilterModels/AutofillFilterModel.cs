using System.Collections.Generic;
using PMS.Model.Models;

namespace PMS.Model.CommonModels.FilterModels
{
    public class AutofillFilterModel : FilterModelBase
    {
        public AutofillFilterModel()
        {
            Sorting = new Sorting
            {
                Direction = SortingDirection.Asc,
                FieldName = nameof(Autofill.Name)
            };
        }
        public ICollection<WorkItemType> WorkItemTypes { get; set; } = new List<WorkItemType>();
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
