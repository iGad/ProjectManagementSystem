using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.CommonModels.FilterModels
{
    public enum SortingDirection
    {
        Asc = 0,
        Desc = 1
    }
    public class Sorting
    {
        public SortingDirection Direction { get; set; }
        public string FieldName { get; set; } 
    }
    public class FilterModelBase
    {
        public const int DefaultPageSize = 30;
        public int PageNumber { get; set; }
        public int CorrectPageNumber => PageNumber > 0 ? PageNumber : 1;
        public int CorrectPageSize => PageSize > 0 ? PageSize : DefaultPageSize;
        public int PageSize { get; set; }
        public Sorting Sorting { get; set; }
    }

    public struct DateTimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        
    }

    public class EventFilterModel : FilterModelBase
    {
        public EventFilterModel()
        {
            Sorting = new Sorting {Direction = SortingDirection.Desc, FieldName = nameof(WorkEvent.Data)};
        }

        public DateTimeRange DateRange { get; set; }
        public bool? IsFavorite { get; set; }
        public string[] UserIds { get; set; }
        public string ItemsIds { get; set; }
    }
}
