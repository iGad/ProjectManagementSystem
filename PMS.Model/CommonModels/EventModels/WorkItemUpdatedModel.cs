using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Model.CommonModels.EventModels
{
    public class WorkItemUpdatedModel
    {
        public int WorkItemId { get; set; }
        public string[] ChangedProperties { get; set; }
    }
}
