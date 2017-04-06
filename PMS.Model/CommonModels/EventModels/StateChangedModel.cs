using PMS.Model.Models;

namespace PMS.Model.CommonModels.EventModels
{
    public class StateChangedModel
    {
        public WorkItemState Old { get; set; }
        public WorkItemState New { get; set; }
    }
}
