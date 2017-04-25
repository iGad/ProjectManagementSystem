using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.CommonModels.EventModels
{
    /// <summary>
    /// Модель, объединяющая таблицу событий и таблицу событий пользователя
    /// </summary>
    public class EventUserModel
    {
        public EventUserModel()
        {
        }

        public EventUserModel(WorkEvent @event, WorkEventUserRelation eventUser)
        {
            EventId = @event.Id;
            EventCreaterId = @event.UserId;
            ObjectId = @event.ObjectId;
            ObjectStringId = @event.ObjectStringId;
            Type = @event.Type;
            Data = @event.Data;
            Date = @event.CreatedDate;

            UserId = eventUser.UserId;
            State = eventUser.State;
            IsFavorite = eventUser.IsFavorite;
        }

        public int EventId { get; set; }
        public string EventCreaterId { get; set; }
        public int? ObjectId { get; set; }
        public string ObjectStringId { get; set; }
        public EventType Type { get; set; }
        public string Data { get; set; }
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public EventState State { get; set; }
        public bool IsFavorite { get; set; }
    }
}
