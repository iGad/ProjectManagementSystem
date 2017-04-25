using System.Collections.Generic;

namespace PMS.Model.CommonModels
{
    public class TableCollectionModel<T> where T:class
    {
        public int TotalCount { get; set; }
        public List<T> Collection { get; set; } 
    }
}
