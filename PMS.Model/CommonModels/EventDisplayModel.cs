using System;
using PMS.Model.CommonModels.EventModels;
using PMS.Model.Models;

namespace PMS.Model.CommonModels
{
    public class EventDisplayModel
    {
        public EventDisplayModel(EventUserModel eventUserModel)
        {
            Id = eventUserModel.EventId;
            IsFavorite = eventUserModel.IsFavorite;
            ObjectId = eventUserModel.ObjectId;
            ObjectStringId = eventUserModel.ObjectStringId;
            Date = eventUserModel.Date;
            UserId = eventUserModel.EventCreaterId;
            State = eventUserModel.State;
            Type = eventUserModel.Type;
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public int? ObjectId { get; set; }
        public string UserId { get; set; }
        public EventType Type { get; set; }
        public string ObjectStringId { get; set; }
        public DateTime Date { get; set; }
        public EventState State { get; set; }
        public bool IsFavorite { get; set; }
    }
}
