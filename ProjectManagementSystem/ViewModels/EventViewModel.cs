using System;
using PMS.Model.CommonModels;
using PMS.Model.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class EventViewModel 
    {
        public EventViewModel(EventDisplayModel displayModel)
        {
            Id = displayModel.Id;
            Description = displayModel.Description;
            Date = displayModel.Date;
            State = new EnumViewModel<EventState>(displayModel.State);
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string UserInfo { get; set; }
        public EnumViewModel<EventState> State { get; set; }
        public EventType Type { get; set; }
        public object Data { get; set; } 
    }
}