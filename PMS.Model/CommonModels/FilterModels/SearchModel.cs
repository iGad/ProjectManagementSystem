using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.CommonModels.FilterModels
{
    public class SearchModel : FilterModelBase
    {
        public SearchModel()
        {
            Sorting = new Sorting {Direction = SortingDirection.Asc, FieldName = nameof(WorkItem.Name)};
        }
        public string SearchText { get; set; }
        public WorkItemType[] Types { get; set; }
        public string[] UserIds { get; set; }
        public WorkItemState[] States { get; set; }
    }
}
