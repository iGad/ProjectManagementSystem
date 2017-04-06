using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using PMS.Model.Models;

namespace PMS.Model.CommonModels
{
    public class EventDisplayModel
    {
        public EventDisplayModel(WorkEvent @event)
        {
            Id = @event.Id;
            ObjectId = @event.ObjectId;
            ObjectStringId = @event.ObjectStringId;
            Date = @event.CreatedDate;
            UserId = @event.UserId;
            Type = @event.Type;
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public int? ObjectId { get; set; }
        public string UserId { get; set; }
        public EventType Type { get; set; }
        public string ObjectStringId { get; set; }
        public DateTime Date { get; set; }
        public EventState State { get; set; }
    }
}
